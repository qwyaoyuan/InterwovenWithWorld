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
    void CheckNowTask(EnumCheckTaskType checkTaskType,int value);

    /// <summary>
    /// 接取任务
    /// </summary>
    /// <param name="taskID">任务id</param>
    void StartTask(int taskID);

    /// <summary>
    /// 完成任务的id(只有内部可以设置值,外部可以监听更改)
    /// </summary>
    int OverTaskID {  get; }

    /// <summary>
    /// 放弃任务
    /// </summary>
    /// <param name="taskID">任务的id</param>
    void GiveUPTask(int taskID);
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
