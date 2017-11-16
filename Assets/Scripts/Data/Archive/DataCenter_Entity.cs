using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataCenter
{
    private PlayerState playerState;

    private TaskProgress taskProgress;

    private KeyContactData keyConatactData;
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


   



}

public class TaskProgress
{

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
}



