using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 主线过场图片显示
/// </summary>
public class UIImageTip : MonoBehaviour
{
    /// <summary>
    /// 用于显示的图像对象
    /// </summary>
    [SerializeField]
    Image showImage;
    /// <summary>
    /// 显示页号的文本框
    /// </summary>
    [SerializeField]
    Text pageText;

    /// <summary>
    /// 向左的按钮
    /// </summary>
    [SerializeField]
    RectTransform leftRect;
    /// <summary>
    /// 向右的按钮
    /// </summary>
    [SerializeField]
    RectTransform rightRect;
    /// <summary>
    /// 退出的按钮
    /// </summary>
    [SerializeField]
    RectTransform exitRect;

    /// <summary>
    /// 资源文件夹路径
    /// </summary>
    public static string ResourceDirectoryPath = "Data/Task/ImageTip";

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
    /// 需要显示的图片
    /// </summary>
    Sprite[] needShowArray;

    /// <summary>
    /// 当前的页面号
    /// </summary>
    int nowPage = 0;

    private void Awake()
    {
        Init();
        //注册按钮事件
        RegistorEventTrigger(leftRect, EventTriggerType.PointerClick, LeftClick);
        RegistorEventTrigger(rightRect, EventTriggerType.PointerClick, RightClick);
        RegistorEventTrigger(exitRect, EventTriggerType.PointerClick, ExitClick);
        //注册游戏状态的事件
        GameState.Instance.Registor<INowTaskState>(INowTaskStateChanged);
        gameObject.SetActive(false);
        iInteractiveState.CanImageTip = true;
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="rectTrans"></param>
    /// <param name="callback"></param>
    private void RegistorEventTrigger(RectTransform rectTrans, EventTriggerType type, UnityAction<BaseEventData> callback)
    {
        EventTrigger eventTrigger = rectTrans.GetComponent<EventTrigger>();
        if (!eventTrigger)
            eventTrigger = rectTrans.gameObject.AddComponent<EventTrigger>();
        if (eventTrigger.triggers == null)
            eventTrigger.triggers = new List<EventTrigger.Entry>();
        EventTrigger.Entry triggerEvent = eventTrigger.triggers.Find(temp => temp.eventID == type);
        if (triggerEvent == null)
        {
            triggerEvent = new EventTrigger.Entry();
            triggerEvent.eventID = type;
            triggerEvent.callback = new EventTrigger.TriggerEvent();
            eventTrigger.triggers.Add(triggerEvent);
        }
        triggerEvent.callback.AddListener(callback);

    }

    private void OnDestroy()
    {
        GameState.Instance.UnRegistor<INowTaskState>(INowTaskStateChanged);
    }

    /// <summary>
    /// 只是负责压入状态
    /// </summary>
    private void OnEnable()
    {
        Init();
        iGameState.PushEnumGameRunType(EnumGameRunType.TaskTalk);
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
    }

    /// <summary>
    /// 只是负责弹出状态
    /// </summary>
    private void OnDisable()
    {
        if (iGameState != null)
        {
            UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
            iGameState.PopEnumGameRunType();
        }
    }

    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        switch (keyType)
        {
            case UIManager.KeyType.L1:
            case UIManager.KeyType.LEFT:
                LeftClick(null);
                break;
            case UIManager.KeyType.R1:
            case UIManager.KeyType.RIGHT:
                RightClick(null);
                break;
            case UIManager.KeyType.B:
                ExitClick(null);
                break;
        }
    }

    /// <summary>
    /// 左侧点击
    /// </summary>
    /// <param name="data"></param>
    private void LeftClick(BaseEventData data)
    {
        int tempPage = nowPage - 1;
        if (tempPage >= 0)
        {
            nowPage = tempPage;
            ShowImageTip();
        }
    }
    /// <summary>
    /// 右侧点击
    /// </summary>
    /// <param name="data"></param>
    private void RightClick(BaseEventData data)
    {
        int tempPage = nowPage + 1;
        if (tempPage < needShowArray.Length)
        {
            nowPage = tempPage;
            ShowImageTip();
        }
    }
    /// <summary>
    /// 退出点击
    /// </summary>
    /// <param name="data"></param>
    private void ExitClick(BaseEventData data)
    {
        gameObject.SetActive(false);
    }

    private void Init()
    {
        if (runtimeTasksData == null)
            runtimeTasksData = DataCenter.Instance.GetEntity<TaskMap.RunTimeTaskData>();
        if (iInteractiveState == null)
            iInteractiveState = GameState.Instance.GetEntity<IInteractiveState>();
        if (iNowTaskState == null)
            iNowTaskState = GameState.Instance.GetEntity<INowTaskState>();
        if (iGameState == null)
            iGameState = GameState.Instance.GetEntity<IGameState>();
    }

    /// <summary>
    /// 当前任务状态发生变化
    /// </summary>
    /// <param name="iNowTaskState"></param>
    /// <param name="fieldName"></param>
    private void INowTaskStateChanged(INowTaskState iNowTaskState, string fieldName)
    {
        //开始任务
        if (string.Equals(fieldName, GameState.GetFieldNameStatic<INowTaskState, int>(temp => temp.StartTask)))
        {
            Init();
            // 如果存在该任务且该任务是主线
            TaskMap.RunTimeTaskInfo runTimeTaskInfo = runtimeTasksData.GetTasksWithID(iNowTaskState.StartTask, true);
            if (runTimeTaskInfo != null && runTimeTaskInfo.TaskInfoStruct.TaskType == TaskMap.Enums.EnumTaskType.Main && !runTimeTaskInfo.TaskInfoStruct.NeedShowTalk && runTimeTaskInfo.TaskInfoStruct.NeedShowImageTip)
            {
                string directoryPath = runTimeTaskInfo.TaskInfoStruct.ShowImageTipDirectoryName;
                if (!string.IsNullOrEmpty(directoryPath))//该文件夹不为空
                {
                    directoryPath = ResourceDirectoryPath + "/" + directoryPath;
                    needShowArray = Resources.LoadAll<Sprite>(directoryPath);
                    if (needShowArray.Length > 0)
                    {
                        gameObject.SetActive(true);//显示面板
                        nowPage = 0;
                        ShowImageTip();//显示
                    }
                }
            }
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void ShowImageTip()
    {
        showImage.sprite = needShowArray[nowPage];
        pageText.text = (nowPage+1).ToString();
    }
}
