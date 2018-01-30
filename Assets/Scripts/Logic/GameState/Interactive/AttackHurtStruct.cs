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
    /// 物理攻击系数(如果伤害类型是PhysicSkill或NormalAction)
    /// </summary>
    public PhysicAttackFactor PhysicAttackFactor;
    /// <summary>
    /// 魔法攻击系数(如果伤害类型是Magic)
    /// </summary>
    public MagicAttackFactor MagicAttackFactor;
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
/// 物理攻击系数
/// </summary>
public struct PhysicAttackFactor
{
    /// <summary>
    /// 增伤倍率系数
    /// </summary>
    public float IncreaseRatioInjuryFactor;
    /// <summary>
    /// 最低伤害系数
    /// </summary>
    public float MinimumDamageFactor;
}

/// <summary>
/// 物理防御系数
/// </summary>
public struct PhysicDefenseFactor
{
    /// <summary>
    /// 减伤倍率系数
    /// </summary>
    public float CoefficientRatioReducingDamageFactor;
    /// <summary>
    /// 伤害豁免系数
    /// </summary>
    public float ImmunityInjury;
}

/// <summary>
/// 魔法攻击系数
/// </summary>
public struct MagicAttackFactor
{ }

/// <summary>
/// 魔法防御系数
/// </summary>
public struct MagicDefenseFactor
{ }

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
    /// <param name="physicDefenseFactor">物理防御系数</param>
    /// <param name="magicDefenseFactor">魔法防御系数</param>
    public static void Calculate(AttackHurtStruct from, IAttributeState to, PhysicDefenseFactor physicDefenseFactor, MagicDefenseFactor magicDefenseFactor)
    {
        //先判断是否命中
        float isHitRate = from.attributeState.HitRate - to.EvadeRate;//本次攻击是否命中的概率(0-?(1))
        if (isHitRate < 1 && Random.Range(0, 1) > isHitRate)//如果本次攻击有几率命中且本次攻击没有命中则返回
        {
            return;
        }
        float baseDamage = 0;
        //根据攻击防御计算初步的基础伤害
        switch (from.hurtType)
        {
            case EnumHurtType.Magic:
                break;
            case EnumHurtType.PhysicSkill:
            case EnumHurtType.NormalAction:
                {
                    float hurtExempt = to.Quick * physicDefenseFactor.ImmunityInjury;//额外豁免=敏捷*额外豁免系数
                    float baseHurt = from.attributeState.BasePhysicDamage - to.BasePhysicDefense - hurtExempt;//基础伤害值= 装备基础伤害-装备基础护甲-额外豁免
                    baseHurt = Mathf.Clamp(baseHurt, 0, float.MaxValue);//如果小于0则取制为0
                    float minHurt = from.attributeState.Quick * from.PhysicAttackFactor.MinimumDamageFactor;//最低伤害=敏捷*最低伤害系数
                    float baseDefHurtRate = 1 / (1 + physicDefenseFactor.CoefficientRatioReducingDamageFactor * to.PhysicsResistance);//受到伤害倍率= 1/(1+减伤倍率系数*防御力)
                    float baseHurtRate = 1 + from.attributeState.PhysicsAttacking * from.PhysicAttackFactor.IncreaseRatioInjuryFactor; //伤害倍率=1+物理攻击力*增伤倍率系数
                    baseDamage = (baseHurt * baseDefHurtRate + minHurt) * baseHurtRate;//根据公式计算出的最初伤害 = (伤害基础值*受伤害倍率+最低伤害值)*伤害倍率
                }
                break;
        }
        //计算暴击
        float isCrit = from.attributeState.CritRate;
        if (isHitRate >= 1 || Random.Range(0, 1) < isHitRate)//如果一定暴击或者本次攻击随机到了暴击概率阶段
        {
            float critDamageRatio = from.attributeState.CritDamageRatio - to.CriticalDef;
            critDamageRatio = Mathf.Clamp(critDamageRatio, 0.2f, 10);//将暴击倍率倍率范围限定在0.2到10之间,也就是有可能暴击伤害可能会更低
            baseDamage *= critDamageRatio;
        }
        //计算格挡
        float isEquipBlock = to.EquipBlock;
        if (isHitRate >= 1 || Random.Range(0, 1) < isEquipBlock)//如果一定会格挡或者本次随机到了格挡概率阶段
        {
            baseDamage *= 0.7f;
        }
        //附加或倍率等处理
        //.......
        to.HP -= baseDamage;
    }
}
