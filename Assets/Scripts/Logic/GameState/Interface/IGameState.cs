using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏状态接口，主要用于保存一些运行时的数据，比如说怪物的生成间隔，天气与时间的变化等 游戏当前模式（野外 设置  城镇等）
/// </summary>
public interface IGameState : IBaseState
{

    /// <summary>
    /// 游戏运行状态
    /// </summary>
    EnumGameRunType GameRunType { get; set; }
}

/// <summary>
/// 游戏运行状态
/// </summary>
public enum EnumGameRunType
{
    /// <summary>
    /// 开始界面
    /// </summary>
    Start,
    /// <summary>
    /// 安全区 城镇
    /// </summary>
    Safe,
    /// <summary>
    /// 非安全区 野外
    /// </summary>
    Unsafa,
    /// <summary>
    /// 设置界面中
    /// </summary>
    Setting,
    /// <summary>
    /// 任务对话中
    /// </summary>
    Task,
    /// <summary>
    /// 过场动画中
    /// </summary>
    Cutscenes
}
