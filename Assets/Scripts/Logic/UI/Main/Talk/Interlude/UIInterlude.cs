using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 主线过场对话
/// </summary>
public class UIInterlude : MonoBehaviour
{
    /// <summary>
    /// 左侧图像
    /// </summary>
    [SerializeField]
    Image leftImage;
    /// <summary>
    /// 左侧文字显示 
    /// </summary>
    [SerializeField]
    Text leftText;

    /// <summary>
    /// 右侧图像
    /// </summary>
    [SerializeField]
    Image rightImage;
    /// <summary>
    /// 右侧文字显示 
    /// </summary>
    [SerializeField]
    Text rightText;

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
    TaskMap.RunTimeTaskData runtimeTasksData;
    /// <summary>
    /// 当前展示的任务信息对象
    /// </summary>
    TaskMap.RunTimeTaskInfo runTimeTaskInfo;
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
    /// 显示在左侧还是显示在右侧
    /// false表示左侧 true表示右侧
    /// </summary>
    bool showLeftOrRight;

    /// <summary>
    /// 开始时第一次的按键抬起(此时不可以使用)
    /// </summary>
    bool fisrtKeyUP;

    /// <summary>
    /// 第一次显示时的npcid(包括主角0)
    /// </summary>
    int lastNPCID;

    private void OnEnable()
    {
        dialogueStructData = DataCenter.Instance.GetMetaData<DialogueStructData>();
        runtimeTasksData = DataCenter.Instance.GetEntity<TaskMap.RunTimeTaskData>();
        iInteractiveState = GameState.Instance.GetEntity<IInteractiveState>();
        iNowTaskState = GameState.Instance.GetEntity<INowTaskState>();
        iGameState = GameState.Instance.GetEntity<IGameState>();
        iGameState.PushEnumGameRunType(EnumGameRunType.TaskTalk);
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        InitTalk();
    }

    /// <summary>
    /// 初始化对话
    /// 展示的是接取主线任务前的对话
    /// </summary>
    private void InitTalk()
    {
        fisrtKeyUP = false;
        int touchNPCID = iInteractiveState.ClickInteractiveNPCID;
        INowTaskState iNowTaskState = GameState.Instance.GetEntity<INowTaskState>();
        TaskMap.RunTimeTaskInfo[] runTimeTaskInfos = runtimeTasksData.GetAllToDoList()
                       .Where(temp => temp.TaskInfoStruct.ReceiveTaskNpcId == touchNPCID && temp.IsOver == false && temp.IsStart == false)
                       .ToArray();
        TaskMap.RunTimeTaskInfo runTimeTaskInfo = runTimeTaskInfos.Where(temp => temp.TaskInfoStruct.TaskType == TaskMap.Enums.EnumTaskType.Main).FirstOrDefault();
        if (runTimeTaskInfo != null)
        {
            this.runTimeTaskInfo = runTimeTaskInfo;
            this.dialogueCodition = dialogueStructData.SearchDialogueConditionsByNPCID(runTimeTaskInfo.TaskInfoStruct.ReceiveTaskNpcId,
                temp => temp.enumDialogueType == EnumDialogueType.Task && temp.thisTask == runTimeTaskInfo.ID).FirstOrDefault();
            if (this.dialogueCodition != null)
            {
                this.nowDialoguePoint = this.dialogueCodition.topPoint;
                showLeftOrRight = true;
                ShowTalk();
            }
            else
            {
                gameObject.SetActive(false);
                iNowTaskState.StartTask = runTimeTaskInfo.ID;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 展示对话
    /// </summary>
    private void ShowTalk()
    {
        //通过npc的id获取显示的图片
        Sprite showImage = null;
        DialogueValue nowDialogueValue = dialogueStructData.SearchDialogueValueByID(nowDialoguePoint.dialogueID);
        string showText = nowDialogueValue.showValue;
        if (lastNPCID != nowDialogueValue.npcID)//本次的NPCid和上次的不一致则左右显示切换
            showLeftOrRight = !showLeftOrRight;
        lastNPCID = nowDialogueValue.npcID;
        if (showLeftOrRight)
        {
            rightImage.gameObject.SetActive(false);
            leftImage.gameObject.SetActive(true);
            leftImage.sprite = showImage;
            leftText.text = showText;
        }
        else
        {
            rightImage.gameObject.SetActive(true);
            leftImage.gameObject.SetActive(false);
            rightImage.sprite = showImage;
            rightText.text = showText;
        }
    }

    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        iGameState.PopEnumGameRunType();
    }

    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        if (!fisrtKeyUP)
        {
            fisrtKeyUP = true;
            return;
        }
        switch (keyType)
        {
            case UIManager.KeyType.A:
            case UIManager.KeyType.B:
                NextTalkOrEnd();
                break;
        }
    }

    /// <summary>
    /// 点击确认
    /// </summary>
    public void ClickOK()
    {
        NextTalkOrEnd();
    }

    /// <summary>
    /// 下一个任务或结束任务
    /// </summary>
    void NextTalkOrEnd()
    {
        if (nowDialoguePoint.childDialoguePoints != null && nowDialoguePoint.childDialoguePoints.Length > 0)
        {
            nowDialoguePoint = nowDialoguePoint.childDialoguePoints[0];
            ShowTalk();
        }
        else
        {
            gameObject.SetActive(false);
            //接受任务
            iNowTaskState.StartTask = runTimeTaskInfo.ID;
        }
    }
}
