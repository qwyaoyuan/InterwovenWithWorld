using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 当前任务事件接口
/// 提供注册与解除注册函数,用于控制状态(还有一些事件的内部处理)
/// </summary>
public interface INowTaskStateEvent : INowTaskState
{
    /// <summary>
    /// 处理该任务的事件
    /// </summary>
    /// <param name="runTimeTaskInfo"></param>
    void TriggeringEvents(TaskMap.RunTimeTaskInfo runTimeTaskInfo);
    /// <summary>
    /// 注册任务事件,并返回该事件的ID
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="CallBack">回掉</param>
    /// <returns>该事件的id</returns>
    int RegistTaskEvent(TaskMap.Enums.EnumTaskEventType eventType, Action<TaskMap.TaskEventData> CallBack);
    /// <summary>
    /// 移除该id的事件
    /// </summary>
    /// <param name="id">事件id</param>
    void UnRegistTaskEvent(int id);
    /// <summary>
    /// 移除事件(所有该对象的事件都会移除)
    /// </summary>
    /// <param name="CallBack"></param>
    void UnRegistTaskEvent(Action<TaskMap.TaskEventData> CallBack);

}

