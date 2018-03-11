using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏状态接口，主要用于保存一些运行时的数据，比如说怪物的生成间隔，天气与时间的变化等 游戏当前模式（野外 设置  城镇等） 游戏的其他设置(镜头移动速度 声音大小等)
/// </summary>
public interface IGameState : IBaseState
{

    /// <summary>
    /// 在加载存档后调用来初始化数据
    /// </summary>
    void LoadArchive();

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
    /// 镜头的移动速度(如果当前模式是第三人称模式)
    /// x表示围绕旋转(角色)
    /// y表示上下旋转(摄像机)
    /// </summary>
    Vector2 CameraRotateSpeed { get; set; }

    /// <summary>
    /// 镜头与对象的Y轴夹角范围(如果当前模式是第三人称模式)
    /// x为最小夹角
    /// y为最大夹角
    /// 计算夹角的向量为对象的Y轴正方向,对象到摄像机的方向 
    /// </summary>
    Vector2 CameraYAngleRange { get; set; }

    /// <summary>
    /// 镜头与对象的距离(如果当前模式是第三人称模式)
    /// </summary>
    float CameraDistanceOfPlayer { get; set; }

    /// <summary>
    /// 镜头朝向目标时向Z轴方向的偏差(如果当前模式是固定视角模式)
    /// </summary>
    float CameraArmOffsetZ { get; set; }

    /// <summary>
    /// 镜头与目标在Z轴方向上的偏差(如果当前模式是固定视角模式)
    /// </summary>
    float CameraPosOffsetZ { get; set; }

    /// <summary>
    /// 镜头与目标在Y轴方向上的偏差(如果当前模式是固定视角模式)
    /// </summary>
    float CameraPosOffsetY { get; set; }

    /// <summary>
    /// 选择目标的模式
    /// </summary>
    EnumSelectTargetModel SelectTargetModel { get; set; }

    /// <summary>
    /// 游戏的视角模式
    /// </summary>
    EnumViewModel ViewModel { get; set; }

    /// <summary>
    /// 设置面板的UI
    /// </summary>
    Canvas SettingPanel { get; set; }
    /// <summary>
    /// 功能面板的UI
    /// </summary>
    Canvas ActionPanel { get; set; }
    /// <summary>
    /// 主面板的UI
    /// </summary>
    Canvas MainPanel { get; set; }
    /// <summary>
    /// 过场动画幕布的UI
    /// </summary>
    Canvas InterludesPanel { get; set; }
    /// <summary>
    /// 过场动画用到的摄像机
    /// </summary>
    Camera InterludesCamera { get; set; }
    /// <summary>
    /// 设置伤害字体数据
    /// </summary>
    HurtFontStruct ShowHurtFont { get; set; }
    /// <summary>
    /// 显示怪物的当前血量 
    /// </summary>
    MonsterHPUIStruct ShowMonsterHP { get; set; }
}

/// <summary>
/// 游戏的视角模式
/// </summary>
public enum EnumViewModel
{
    /// <summary>
    /// 自由的第三人称摄像机
    /// </summary>
    Free,
    /// <summary>
    /// 固定的俯视摄像机
    /// </summary>
    Solid
}

/// <summary>
/// 游戏的选择目标模式
/// </summary>
public enum EnumSelectTargetModel
{
    /// <summary>
    /// 不选择,此时也不锁定
    /// </summary>
    None,
    /// <summary>
    /// 选择怪物,此时按View的上下左右可以切换选择的目标
    /// </summary>
    SelectMonster,
    /// <summary>
    /// 选择玩家自己或友军
    /// </summary>
    SelectSelf
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


/// <summary>
/// 怪物血条UI结构
/// </summary>
public struct MonsterHPUIStruct
{
    /// <summary>
    /// 怪物名
    /// </summary>
    public string monsterName;

    /// <summary>
    /// 怪物的图片
    /// </summary>
    public Sprite monsterSprite;

    /// <summary>
    /// 最大血量
    /// </summary>
    public float maxHP;

    /// <summary>
    /// 当前血量
    /// </summary>
    public float nowHP;

    /// <summary>
    /// 怪物对象
    /// </summary>
    public GameObject monsterObj;

    /// <summary>
    /// 异常数据 
    /// </summary>
    public StatusDataInfo.StatusLevelDataInfo[] statusDatas;

    /// <summary>
    /// 本次技能的耗魔量(仅用于组合魔法,配合异常使用)
    /// </summary>
    public float thisUsedMana;
}

/// <summary>
/// 伤害字体数据结构
/// </summary>
public struct HurtFontStruct
{
    /// <summary>
    /// 目标对象
    /// </summary>
    public GameObject TargetObj;

    /// <summary>
    /// 伤害
    /// </summary>
    public float Hurt;

    /// <summary>
    /// 是否暴击
    /// </summary>
    public bool IsCrit;

    /// <summary>
    /// 偏差值
    /// </summary>
    public float Offset;
}