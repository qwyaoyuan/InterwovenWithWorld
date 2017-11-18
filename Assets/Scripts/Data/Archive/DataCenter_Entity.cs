﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataCenter
{
    /// <summary>
    /// 玩家状态
    /// </summary>
    private PlayerState playerState;

    /// <summary>
    /// 任务进度
    /// </summary>
    private TaskProgress taskProgress;


    /// <summary>
    /// 按键映射数据
    /// </summary>
    private KeyContactData keyConatactData;


    public DataCenter()
    {
        playerState = new PlayerState();
        taskProgress = new TaskProgress();
        keyConatactData = new KeyContactData();
    }


}

/// <summary>
/// 玩家状态
/// </summary>
public class PlayerState
{
    /// <summary>
    /// 人物等级
    /// </summary>
    public int Level;

    /// <summary>
    /// 自由点
    /// </summary>
    public int FreedomPoint;


    /// <summary>
    /// 技能点
    /// </summary>
    public Dictionary<EnumSkillType, int> SkillPoint;


    /// <summary>
    /// 组合技能的数据
    /// </summary>
    public List<EnumSkillType[]> CombineSkills;


    /// <summary>
    /// 种族路线
    /// </summary>
    public List<RoleOfRace> RoleOfRaceRoute;

    /// <summary>
    /// 属性点
    /// </summary>
    public int PropertyPoint;

    /// <summary>
    /// 力量
    /// </summary>
    public int Strength;

    /// <summary>
    /// 精神
    /// </summary>
    public int Spirit;

    /// <summary>
    /// 敏捷
    /// </summary>
    public int Agility;

    /// <summary>
    /// 专注
    /// </summary>
    public int Concentration;

    /// <summary>
    /// 声望
    /// </summary>
    public int Reputation;

    /// <summary>
    /// 任务进度
    /// </summary>
    public TaskProgress TaskProgress;


    /// <summary>
    /// 玩家所有物品
    /// </summary>
    public List<PlayGoods> PlayerAllGoods;


    public PlayerState()
    {
        SkillPoint = new Dictionary<EnumSkillType, int>();
        RoleOfRaceRoute = new List<RoleOfRace>();
        TaskProgress = new TaskProgress();
        PlayerAllGoods = new List<PlayGoods>();
        CombineSkills = new List<EnumSkillType[]>();
    }


}

/// <summary>
/// 玩家的物品
/// </summary>
public class PlayGoods
{
    /// <summary>
    /// 物品信息
    /// </summary>
    public Goods GoodsInfo { get; set; }
    /// <summary>
    /// 物品所在位置
    /// </summary>
    GoodsLocation GoodsLocation { get; set; }

    public PlayGoods()
    {
        GoodsInfo = new Goods();
        GoodsLocation = GoodsLocation.None;
    }
    public PlayGoods(Goods goodsInfo, GoodsLocation location)
    {
        this.GoodsInfo = goodsInfo;
        this.GoodsLocation = location;
    }
}

/// <summary>
/// 物品位置
/// </summary>
public enum GoodsLocation
{
    /// <summary>
    /// 穿戴中
    /// </summary>
    Wearing,
    /// <summary>
    /// 包裹
    /// </summary>
    Package,
    /// <summary>
    /// 仓库
    /// </summary>
    warehouse,

    None,
}


/// <summary>
/// 任务进度
/// </summary>
public class TaskProgress
{
    public List<TaskItem> allDoingTasks;
    public TaskProgress()
    {
        allDoingTasks = new List<TaskItem>();
    }

}
/// <summary>
/// 任务项
/// </summary>
public class TaskItem
{
    /// <summary>
    /// 进行的任务id
    /// </summary>
    public int TaskId;
    /// <summary>
    /// 已经杀死的怪物数量
    /// </summary>
    public Dictionary<int, int> HaveKillMonsterCount { get; set; }

    /// <summary>
    /// 已经获取的物品数量
    /// </summary>
    public Dictionary<int, int> HaveGoodsAssignCount { get; set; }

    /// <summary>
    /// 此任务已经经过的时间
    /// </summary>
    public int TimeElasped;

    public TaskItem()
    {
        TaskId = -1;
        HaveKillMonsterCount = new Dictionary<int, int>();
        HaveGoodsAssignCount = new Dictionary<int, int>();
    }

}



