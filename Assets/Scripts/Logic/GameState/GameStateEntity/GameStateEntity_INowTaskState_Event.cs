using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 实现了INowTaskStateEvent接口的GameState类的一个分支实体
/// </summary>
public partial class GameState : INowTaskStateEvent
{
    /// <summary>
    /// 基础的事件id
    /// </summary>
    private int baseTaskEventID;

    /// <summary>
    /// id对应回掉字典
    /// </summary>
    public Dictionary<int, Action<TaskMap.TaskEventData>> idToCallBackDic;

    /// <summary>
    /// 类型对应id集合字典
    /// </summary>
    public Dictionary<TaskMap.Enums.EnumTaskEventType, List<int>> eventTypeToIDsDic;

    /// <summary>
    /// 开始时时调用
    /// </summary>
    partial void Start_INowTaskStateEvent()
    {
        baseTaskEventID = 0;
        idToCallBackDic = new Dictionary<int, Action<TaskMap.TaskEventData>>();
        eventTypeToIDsDic = new Dictionary<TaskMap.Enums.EnumTaskEventType, List<int>>();
        //遍历枚举填充字典
        foreach (TaskMap.Enums.EnumTaskEventType taskEventType in Enum.GetValues(typeof(TaskMap.Enums.EnumTaskEventType)))
        {
            if (!eventTypeToIDsDic.ContainsKey(taskEventType))
            {
                eventTypeToIDsDic.Add(taskEventType, new List<int>());
            }
        }
    }

    /// <summary>
    /// 加载存档时调用
    /// </summary>
    partial void Load_INowTaskStateEvent()
    {
        Init_INowTaskStateEvent();
        //注册
        RegistTaskEvent(TaskMap.Enums.EnumTaskEventType.CanBigMap, TaskEventCall_CanBigMap);
    }

    /// <summary>
    /// 任务事件的初始化
    /// </summary>
    private void Init_INowTaskStateEvent()
    {
        UnRegistTaskEvent(TaskEventCall_CanBigMap);
    }

    /// <summary>
    /// 处理该任务的事件
    /// </summary>
    /// <param name="runTimeTaskInfo"></param>
    public void TriggeringEvents(TaskMap.RunTimeTaskInfo runTimeTaskInfo)
    {
        if (runTimeTaskInfo.TaskInfoStruct.TaskEventTriggerDic != null)
        {
            if (runTimeTaskInfo.TaskInfoStruct.TaskEventTriggerDic.ContainsKey(runTimeTaskInfo.TaskProgress))
            {
                List<TaskMap.TaskEventData> taskEventDataList = runTimeTaskInfo.TaskInfoStruct.TaskEventTriggerDic[runTimeTaskInfo.TaskProgress];
                if (taskEventDataList != null)
                {
                    foreach (TaskMap.TaskEventData taskEventData in taskEventDataList)
                    {
                        TaskMap.TaskEventData taskEventData_clone = taskEventData.Clone();
                        if (taskEventData != null)
                        {
                            List<int> ids = eventTypeToIDsDic[taskEventData.EventType];
                            foreach (var id in ids)
                            {
                                try
                                {
                                    idToCallBackDic[id](taskEventData_clone);
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 注册任务事件,并返回该事件的ID
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="CallBack">回掉</param>
    /// <returns>该事件的id</returns>
    public int RegistTaskEvent(TaskMap.Enums.EnumTaskEventType eventType, Action<TaskMap.TaskEventData> CallBack)
    {
        baseTaskEventID++;
        idToCallBackDic.Add(baseTaskEventID, CallBack);
        List<int> tempInts = eventTypeToIDsDic[eventType];
        tempInts.Add(baseTaskEventID);
        return baseTaskEventID;
    }

    /// <summary>
    /// 移除该id的事件
    /// </summary>
    /// <param name="id">事件id</param>
    public void UnRegistTaskEvent(int id)
    {
        idToCallBackDic.Remove(id);
        foreach (var item in eventTypeToIDsDic)
        {
            item.Value.Remove(id);
        }
    }

    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="CallBack"></param>
    public void UnRegistTaskEvent(Action<TaskMap.TaskEventData> CallBack)
    {
        List<int> ids = new List<int>();
        foreach (var item in idToCallBackDic)
        {
            if (object.Equals(item.Value, CallBack))
            {
                ids.Add(item.Key);
            }
        }
        foreach (var item in ids)
        {
            UnRegistTaskEvent(item);
        }
    }

    #region 内部处理的事件
    /// <summary>
    /// 处理地图可用性事件
    /// </summary>
    /// <param name="taskEventData">数据对象</param>
    private void TaskEventCall_CanBigMap(TaskMap.TaskEventData taskEventData )
    {
        if (taskEventData.EventType == TaskMap.Enums.EnumTaskEventType.CanBigMap)
        {
            GameRunningStateData gameRunningStateData = DataCenter.Instance.GetEntity<GameRunningStateData>();
            gameRunningStateData.CanBigMap = (bool)taskEventData.EventData;
        }
    }
    #endregion

}

