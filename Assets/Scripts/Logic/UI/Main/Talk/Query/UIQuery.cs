using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 支线或重复任务选择对话
/// </summary>
public class UIQuery : MonoBehaviour
{
    /// <summary>
    /// 存放选项以及对话的容器组件
    /// 用于刷新
    /// </summary>
    [SerializeField]
    RectTransform backQueryUpdateTrans;
    /// <summary>
    /// 显示npc头像的图像控件
    /// </summary>
    [SerializeField]
    Image npcShowImage;
    /// <summary>
    /// 对话标题控件
    /// </summary>
    [SerializeField]
    Text titleText;
    /// <summary>
    /// 选择项容器对象 
    /// </summary>
    [SerializeField]
    RectTransform selectItemBackTrans;
    /// <summary>
    /// 选择项的例子
    /// </summary>
    [SerializeField]
    GameObject exampleItemObj;


    /// <summary>
    /// 对话数据对象
    /// </summary>
    DialogueStructData dialogueStructData;
    /// <summary>
    /// 交互状态接口对象
    /// </summary>
    IInteractiveState iInteractiveState;
    /// <summary>
    /// 运行时任务管理对象 
    /// </summary>
    RuntimeTasksData runtimeTasksData;
    /// <summary>
    /// 当前展示的任务信息对象
    /// </summary>
    RunTimeTaskInfo runTimeTaskInfo;
    /// <summary>
    /// 当前任务状态对象
    /// </summary>
    INowTaskState iNowTaskState;
    /// <summary>
    /// 游戏状态
    /// </summary>
    IGameState iGameState;

    /// <summary>
    /// 对话条件对象 
    /// </summary>
    DialogueCondition dialogueCodition;
    /// <summary>
    /// 当前的对话的节点
    /// </summary>
    DialoguePoint nowDialoguePoint;
    /// <summary>
    /// 显示的条目集合
    /// </summary>
    List<Transform> showItemList;

    /// <summary>
    /// 之前的游戏状态
    /// </summary>
    EnumGameRunType oldGameRunType;

    /// <summary>
    /// 选择的下标 
    /// </summary>
    int selectItemIndex = -1;

    private void OnEnable()
    {
        dialogueStructData = DataCenter.Instance.GetMetaData<DialogueStructData>();
        runtimeTasksData = DataCenter.Instance.GetEntity<RuntimeTasksData>();
        iInteractiveState = GameState.Instance.GetEntity<IInteractiveState>();
        iNowTaskState = GameState.Instance.GetEntity<INowTaskState>();
        iGameState = GameState.Instance.GetEntity<IGameState>();
        showItemList = new List<Transform>();
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
    }

    /// <summary>
    /// 初始化对话
    /// </summary>
    private void InitTalk()
    {
        oldGameRunType = iGameState.GameRunType;
        int touchNPCID = iInteractiveState.ClickInteractiveNPCID;
        RunTimeTaskInfo[] runTimeTaskInfos = runtimeTasksData.GetAllToDoList()
                       .Where(temp => temp.RunTimeTaskNode.ReceiveTaskNpcId == touchNPCID && temp.IsOver == false && temp.IsStart == false)
                       .ToArray();
        //首先选择
        RunTimeTaskInfo runTimeTaskInfo = runTimeTaskInfos.Where(temp => temp.RunTimeTaskNode.TaskType == Enums.TaskType.BranchLine).FirstOrDefault();
        if (runTimeTaskInfo == null)
            runTimeTaskInfo = runTimeTaskInfos.Where(temp => temp.RunTimeTaskNode.TaskType == Enums.TaskType.Repeat).FirstOrDefault();

        if (runTimeTaskInfo != null)
        {
            this.runTimeTaskInfo = runTimeTaskInfo;//这个表示当前的任务
            DialogueCondition[] dialogueConditions = dialogueStructData.SearchDialogueConditionsByNPCID(runTimeTaskInfo.RunTimeTaskNode.ReceiveTaskNpcId,
                temp => temp.enumDialogueType == EnumDialogueType.Ask).ToArray();
            dialogueCodition = dialogueConditions.Where(temp => DialoguePointHasThisTask(temp.topPoint, this.runTimeTaskInfo.ID.ToString())).FirstOrDefault();
            if (dialogueCodition != null)//如果存在该任务
            {
                this.nowDialoguePoint = this.dialogueCodition.topPoint;
                iGameState.GameRunType = EnumGameRunType.TaskTalk;
                ShowTalk();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 判断指定的节点是否存在指定的任务
    /// </summary>
    /// <param name="target"></param>
    /// <param name="taskID"></param>
    /// <returns></returns>
    private bool DialoguePointHasThisTask(DialoguePoint target, string taskID)
    {
        if (string.IsNullOrEmpty(taskID)) return false;
        DialogueValue dialogueValue = dialogueStructData.SearchDialogueValueByID(target.dialogueID);
        if (!string.IsNullOrEmpty(dialogueValue.otherValue) && string.Equals(dialogueValue.otherValue.Trim(), taskID.Trim()))
            return true;
        else
        {
            if (target.childDialoguePoints != null && target.childDialoguePoints.Length > 0)//如果存在子节点
            {
                foreach (DialoguePoint child in target.childDialoguePoints)
                {
                    bool result = DialoguePointHasThisTask(child, taskID);
                    if (result)
                        return true;
                }
            }
            return false;
        }

    }

    /// <summary>
    /// 展示对话
    /// </summary>
    private void ShowTalk()
    {
        DialogueValue dialogueValue = dialogueStructData.SearchDialogueValueByID(nowDialoguePoint.dialogueID);
        titleText.text = dialogueValue.showValue;
        int childCount = selectItemBackTrans.childCount;
        Transform[] childTransfroms = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            childTransfroms[i] = selectItemBackTrans.GetChild(i);
        }
        for (int i = 0; i < childCount; i++)
        {
            if (!Transform.Equals(childTransfroms[i], exampleItemObj.transform))//如果不是例子对象则销毁
            {
                GameObject.DestroyImmediate(childTransfroms[i].gameObject);
            }
        }
        showItemList.Clear();
        //添加
        Action<string, Action<Transform>> AddItemAction = (showValue, ClickCallback) =>
        {
            GameObject createItemObj = GameObject.Instantiate<GameObject>(exampleItemObj);
            createItemObj.transform.SetParent(selectItemBackTrans);
            createItemObj.SetActive(true);
            Transform itemTextTrans = createItemObj.transform.GetChild(0);
            Text itemText = itemTextTrans.GetComponent<Text>();
            itemText.text = showValue;
            showItemList.Add(createItemObj.transform);
            //添加事件
            EventTrigger eventTrigger = createItemObj.AddComponent<EventTrigger>();
            eventTrigger.triggers = new List<EventTrigger.Entry>();
            //点击事件
            EventTrigger.Entry entry_Click = new EventTrigger.Entry();
            entry_Click.eventID = EventTriggerType.PointerClick;
            entry_Click.callback = new EventTrigger.TriggerEvent();
            entry_Click.callback.AddListener(temp => ClickCallback(itemTextTrans));
            eventTrigger.triggers.Add(entry_Click);
            //鼠标进入事件
            EventTrigger.Entry entry_Enter = new EventTrigger.Entry();
            entry_Enter.eventID = EventTriggerType.PointerEnter;
            entry_Enter.callback = new EventTrigger.TriggerEvent();
            entry_Enter.callback.AddListener(temp =>
            {
                createItemObj.GetComponent<Image>().color = new Color(1, 1, 1, 0.4f);
                selectItemIndex = showItemList.IndexOf(createItemObj.transform);
            });
            eventTrigger.triggers.Add(entry_Enter);
            //鼠标离开事件
            EventTrigger.Entry entry_Leave = new EventTrigger.Entry();
            entry_Leave.eventID = EventTriggerType.PointerExit;
            entry_Leave.callback = new EventTrigger.TriggerEvent();
            entry_Leave.callback.AddListener(temp => createItemObj.GetComponent<Image>().color = new Color(1, 1, 1, 0));
            eventTrigger.triggers.Add(entry_Leave);
        };
        DialoguePoint[] childPoints = nowDialoguePoint.childDialoguePoints;
        foreach (DialoguePoint dialoguePoint in childPoints)//便利添加子选项
        {
            DialogueValue childDialogueValue = dialogueStructData.SearchDialogueValueByID(dialoguePoint.dialogueID);
            AddItemAction(childDialogueValue.titleValue, temp =>
             {
                 int index = showItemList.IndexOf(temp);
                 DialoguePoint clickPoint = nowDialoguePoint.childDialoguePoints[index];
                 if (clickPoint.childDialoguePoints == null || clickPoint.childDialoguePoints.Length == 0)//如果有任务则接取,如果没有则不做处理
                {
                     DialogueValue clickValue = dialogueStructData.SearchDialogueValueByID(clickPoint.dialogueID);
                     if (!string.IsNullOrEmpty(clickValue.otherValue))
                     {
                         int taskID = -1;
                         if (int.TryParse(clickValue.otherValue, out taskID))
                         {
                             RunTimeTaskInfo runTimeTaskInfo = runtimeTasksData.GetTasksWithID(taskID);
                             if (runTimeTaskInfo.IsOver == false && runTimeTaskInfo.IsStart == false)
                             {
                                 iNowTaskState.StartTask(runTimeTaskInfo.ID);
                                 gameObject.SetActive(false);
                             }
                         }
                     }
                 }
                 else//继续进入下面节点
                {
                     nowDialoguePoint = clickPoint;
                     ShowTalk();
                 }
             });
        }
        //最后添加一个返回选项
        AddItemAction("返回", temp =>
        {
            if (nowDialoguePoint.parentDialoguePoint != null)
            {
                nowDialoguePoint = nowDialoguePoint.parentDialoguePoint;
                ShowTalk();
            }
            else
            {
                gameObject.SetActive(false);
            }
        });
        //选择第一个下标 
        selectItemIndex = 0;
        SetSelectItemIndex();
    }

    /// <summary>
    /// 设置选择的下标
    /// </summary>
    private void SetSelectItemIndex()
    {
        if (selectItemIndex >= showItemList.Count)
            selectItemIndex = 0;
        if (selectItemIndex < 0)
            selectItemIndex = showItemList.Count - 1;
        for (int i = 0; i < showItemList.Count; i++)
        {
            if (i == selectItemIndex)
                showItemList[i].transform.GetComponent<Image>().color = new Color(1, 1, 1, 0.4f);
            else
                showItemList[i].transform.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        
    }

    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        iGameState.GameRunType = oldGameRunType;
    }

    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        switch (keyType)
        {
            case UIManager.KeyType.A:
                SetSelectItemIndex();
                Transform nextTrans = showItemList[selectItemIndex];
                EventTrigger.Entry nextEntry = nextTrans.GetComponent<EventTrigger>().triggers.Where(temp => temp.eventID == EventTriggerType.PointerClick).FirstOrDefault();
                if (nextEntry != null)
                    nextEntry.callback.Invoke(null);
                break;
            case UIManager.KeyType.B:
                Transform returnTrans = showItemList[showItemList.Count - 1];
                EventTrigger.Entry returnEntry = returnTrans.GetComponent<EventTrigger>().triggers.Where(temp => temp.eventID == EventTriggerType.PointerClick).FirstOrDefault();
                if (returnEntry != null)
                    returnEntry.callback.Invoke(null);
                break;
            case UIManager.KeyType.UP:
                selectItemIndex++;
                SetSelectItemIndex();
                break;
            case UIManager.KeyType.DOWN:
                selectItemIndex--;
                SetSelectItemIndex();
                break;


        }
    }
}
