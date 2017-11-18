using System;
using System.Collections.Generic;

/// <summary>
/// 任务节点
/// </summary>
public class TaskNode
{

    public TaskNode()
    {

    }
    /// <summary>
    /// 任务地点
    /// </summary>
    public TaskLocation TaskLocation { get; set; }

    /// <summary>
    /// 任务标题
    /// </summary>
    public string TaskTitile { get; set; }

    /// <summary>
    /// 任务类型
    /// </summary>
    public TaskType TaskType { get; set; }

    /// <summary>
    /// 等级限制,>=此等级可以开始任务
    /// </summary>
    public int LevelLimit { get; set; }

    /// <summary>
    /// 性格倾向
    /// </summary>
    public CharacterTendency ChaTendency { get; set; }

    /// <summary>
    /// 种族类型
    /// </summary>
    public RoleOfRace RoleOfRace { get; set; }

    /// <summary>
    /// 需要的声望
    /// </summary>
    public float NeedReputation { get; set; }

    /// <summary>
    /// 奖励物品
    /// </summary>
    public List<int> AwardGoods { get; set; }

    /// <summary>
    /// 奖励经验
    /// </summary>
    public int AwardExperience { get; set; }

    /// <summary>
    /// 奖励技能点
    /// </summary>
    public int AwardSkillPoint { get; set; }

    /// <summary>
    /// 奖励的声望
    /// </summary>
    public float AwardReputation { get; set; }

    /// <summary>
    /// 接取任务的npc的id
    /// </summary>
    public int ReceiveTaskNpcId { get; set; }

    /// <summary>
    /// 交付任务的npc的id
    /// </summary>
    public int DeliveryTaskNpcId { get; set; }

    /// <summary>
    /// 杀死某怪物指定数量
    /// </summary>
    public Dictionary<int, int> KillMonsterAssignCount { get; set; }

    /// <summary>
    /// 获取物品指定数量
    /// </summary>
    public Dictionary<int, int> GetGoodsAssignCount { get; set; }

    /// <summary>
    /// 到达指定区域 ,Vector.zero
    /// </summary>
    public Vector3 ArriveAssignPosition { get; set; }


    /// <summary>
    /// 时间限制
    /// </summary>
    public int TimeLimit
    {
        get;
        set;
    }



}

/// <summary>
/// 任务地点
/// </summary>
public class TaskLocation
{
    /// <summary>
    /// 场景名
    /// </summary>
    public string SceneName { get; set; }

    /// <summary>
    /// 到达的中心位置
    /// </summary>
    public Vector3 ArrivedCenterPos { get; set; }


    /// <summary>
    /// 半径
    /// </summary>
    public int Radius { get; set; }

    public TaskLocation()
    {

    }
}


public class Vector3
{
    public float X { get; set; }

    public float Y { get; set; }

    public float Z { get; set; }


    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString()
    {
        return X.ToString() + "," + Y.ToString() + "," + Z.ToString();
    }
}

/// <summary>
/// 任务类型
/// </summary>
public enum TaskType
{
    /// <summary>
    /// 主线任务
    /// </summary>
    PrincipalLine,

    /// <summary>
    /// 支线任务
    /// </summary>
    BranchLine,

    /// <summary>
    /// 重复性任务
    /// </summary>
    Repeat,

    /// <summary>
    /// 随机任务
    /// </summary>
    Random
}

/// <summary>
/// 性格倾向
/// </summary>
public enum CharacterTendency
{

    None,
    /// <summary>
    /// 好杀戮的
    /// </summary>
    Slaughterous,

    /// <summary>
    /// 和平的
    /// </summary>
    Peaceable,
}
