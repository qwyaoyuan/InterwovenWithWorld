﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏状态接口，主要用于保存一些运行时的数据，比如说怪物的生成间隔，天气与时间的变化等 游戏当前模式（野外 设置  城镇等） 游戏的其他设置(镜头移动速度 声音大小等)
/// </summary>
public interface IGameState : IBaseState
{
    /// <summary>
    /// 游戏运行状态
    /// </summary>
    EnumGameRunType GameRunType { get; set; }

    /// <summary>
    /// 将一个游戏运行状态压入栈中
    /// </summary>
    /// <param name="enumGameRunType"></param>
    void PushEnumGameRunType(EnumGameRunType enumGameRunType);

    /// <summary>
    /// 提出最顶层的一个状态
    /// </summary>
    EnumGameRunType PopEnumGameRunType();

    /// <summary>
    /// 当前的场景名
    /// </summary>
    string SceneName { get; }

    /// <summary>
    /// 更改场景
    /// </summary>
    /// <param name="sceneName">场景</param>
    /// <param name="playerLocation">玩家的位置</param>
    /// <param name="LoadResultAction">加载结果回调</param>
    void ChangedScene(string sceneName, Vector3 playerLocation, Action<bool> LoadResultAction = null);

    /// <summary>
    /// 镜头的移动速度
    /// x表示围绕旋转(角色)
    /// y表示上下旋转(摄像机)
    /// </summary>
    Vector2 CameraRotateSpeed { get; set; }

    /// <summary>
    /// 镜头与对象的Y轴夹角范围
    /// x为最小夹角
    /// y为最大夹角
    /// 计算夹角的向量为对象的Y轴正方向,对象到摄像机的方向 
    /// </summary>
    Vector2 CameraYAngleRange { get; set; }

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
    TaskTalk,
    /// <summary>
    /// 过场动画中
    /// </summary>
    Interludes,
    /// <summary>
    /// 合成界面
    /// </summary>
    Synthesis,
}
