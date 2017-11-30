using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 当前任务状态接口
/// 提供一些函数规则在实现类中实时检测
/// </summary>
public interface INowTaskState : IBaseState
{
    /// <summary>
    /// 检测当前任务情况
    /// Update中持续检测的是位置的状态的数据
    /// </summary>
    /// <param name="checkTaskType">检测类型</param>
    /// <param name="value">数据</param>
    /// <returns>如果返回false表示没有完成该任务,如果范围true表示该任务完成</returns>
    bool CheckNowTask(EnumCheckTaskType checkTaskType,int value);

    /// <summary>
    /// 获取刚刚接取的任务或者设置要接取的任务
    /// </summary>
    int StartTask { get; set; }

    /// <summary>
    /// 完成任务的id(只有内部可以设置值,外部可以监听更改)
    /// </summary>
    int OverTaskID {  get; }

    /// <summary>
    /// 放弃任务
    /// </summary>
    /// <param name="taskID">任务的id</param>
    void GiveUPTask(int taskID);

    /// <summary>
    /// 获取制定场景中的未接取任务,如果场景名为空,则返回所有未接取任务
    /// 检测的是接取任务NPC所在的场景
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    RunTimeTaskInfo[] GetWaitTask(string scene);

    /// <summary>
    /// 获取指定场景中的正在执行的任务,如果场景名为空,则返回所有正在执行的任务
    /// 检测的是完成NPC或者完成地点所在的场景
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    RunTimeTaskInfo[] GetStartTask(string scene);

    /// <summary>
    /// 获取指定场景中的条件达成但是没有交付的任务,如果场景名为空,则会返回所有条件达成但是没有交付的任务
    /// 检测的是完成NPC所在的场景(完成地点类型的立即就完成了)
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    RunTimeTaskInfo[] GetEndTask(string scene);
}

/// <summary>
/// 检测任务类型
/// </summary>
public enum EnumCheckTaskType
{
    /// <summary>
    /// 位置
    /// </summary>
    Position,
    /// <summary>
    /// 怪物
    /// </summary>
    Monster,
    /// <summary>
    /// 物品
    /// </summary>
    Goods,
    /// <summary>
    /// 点击NPC
    /// </summary>
    NPC,
}
