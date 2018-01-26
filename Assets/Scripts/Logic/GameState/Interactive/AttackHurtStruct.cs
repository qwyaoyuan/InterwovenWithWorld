using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击伤害结构
/// </summary>
public struct AttackHurtStruct 
{
    /// <summary>
    /// 伤害传递次数,主要是为了保证这是第一次传递,如果两边都有反伤则不会造成死锁
    /// </summary>
    public int hurtTransferNum;
    /// <summary>
    /// 伤害类型
    /// </summary>
    public EnumHurtType hurtType;
    /// <summary>
    /// 造成本次伤害时的基础状态
    /// </summary>
    public IAttributeState attributeState;
    /// <summary>
    /// 本次技能的耗魔量(仅用于组合魔法)
    /// </summary>
    public float thisUsedMana;
    /// <summary>
    /// 附加状态数组
    /// </summary>
    public StatusDataInfo.StatusLevelDataInfo[] statusLevelDataInfos;
    /// <summary>
    /// 伤害来自对象
    /// </summary>
    public GameObject hurtFromObj;
}

/// <summary>
/// 伤害类型 
/// </summary>
public enum EnumHurtType
{
    /// <summary>
    /// 魔法伤害
    /// </summary>
    Magic,
    /// <summary>
    /// 物理技能伤害
    /// </summary>
    PhysicSkill,
    /// <summary>
    /// 普通攻击伤害
    /// </summary>
    NormalAction,
}

/// <summary>
/// 计算伤害的静态类
/// </summary>
public static class CalculateHurt
{
    /// <summary>
    /// 具体的计算
    /// </summary>
    /// <param name="from">伤害来自于</param>
    /// <param name="to">伤害指向</param>
    public static void Calculate(AttackHurtStruct from, IAttributeState to)
    { }
}
