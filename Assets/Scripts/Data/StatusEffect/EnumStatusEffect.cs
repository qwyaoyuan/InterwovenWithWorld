using System;
/// <summary>
/// 状态类型
/// 主要是buff debuff
/// </summary>
public enum EnumStatusEffect
{
    [FieldExplan("无特殊效果")]
    None = 0,
    #region Debuff 10个
    /// <summary>
    /// 冰冻
    /// </summary>
    [FieldExplan("冰冻")]
    [StatusAction(EnumStatusAction.MoveSpeed, 
        EnumStatusAction.AttackSpeed, 
        EnumStatusAction.PhysicsResisitance)]
    bd1 = 1,
    /// <summary>
    /// 迟钝
    /// </summary>
    [FieldExplan("迟钝")]
    [StatusAction(EnumStatusAction.StateResistance, 
        EnumStatusAction.MoveSpeed, 
        EnumStatusAction.AttackSpeed)]
    cd1,
    /// <summary>
    /// 点燃
    /// </summary>
    [FieldExplan("点燃")]
    [StatusAction(EnumStatusAction.Life, 
        EnumStatusAction.MagicResisitance, 
        EnumStatusAction.LifeRecoverySpeed)]
    dr1,
    /// <summary>
    /// 凋零
    /// </summary>
    [FieldExplan("凋零")]
    [StatusAction(EnumStatusAction.Life,
        EnumStatusAction.LifeRecoverySpeed)]
    dl3,
    /// <summary>
    /// 减速
    /// </summary>
    [FieldExplan("减速")]
    [StatusAction(EnumStatusAction.MoveSpeed,
        EnumStatusAction.AttackSpeed)]
    js4,
    /// <summary>
    /// 迷惑
    /// </summary>
    [FieldExplan("迷惑")]
    [StatusAction(EnumStatusAction.PhysicsResisitance,
        EnumStatusAction.MagicResisitance, 
        EnumStatusAction.StateResistance, 
        EnumStatusAction.MagicAffine,
        EnumStatusAction.ElementResistance)]
    mh3,
    /// <summary>
    /// 无力
    /// </summary>
    [FieldExplan("无力")]
    [StatusAction(EnumStatusAction.PhysicsFinalDamage, 
        EnumStatusAction.PhysicsResisitance)]
    wl1,
    /// <summary>
    /// 虚弱
    /// </summary>
    [FieldExplan("虚弱")]
    [StatusAction(EnumStatusAction.LifeRecoverySpeed, 
        EnumStatusAction.PhysicsResisitance, 
        EnumStatusAction.MagicResisitance,
        EnumStatusAction.StateResistance,
        EnumStatusAction.ElementResistance)]
    xr2,
    /// <summary>
    /// 中毒
    /// </summary>
    [FieldExplan("中毒")]
    [StatusAction(EnumStatusAction.LifeRecoverySpeed,
        EnumStatusAction.Life, 
        EnumStatusAction.AttributeChange)]
    zd2,
    /// <summary>
    /// 诅咒
    /// </summary>
    [FieldExplan("诅咒")]
    [StatusAction(EnumStatusAction.Life,
        EnumStatusAction.LifeRecoverySpeed,
        EnumStatusAction.MagicResisitance,
        EnumStatusAction.PhysicsResisitance,
        EnumStatusAction.StateResistance,
        EnumStatusAction.ElementResistance,
        EnumStatusAction.MagicFinalDamage,
        EnumStatusAction.PhysicsFinalDamage,
        EnumStatusAction.AttributeChange)]
    zz3,
    /// <summary>
    /// 流血
    /// </summary>
    [FieldExplan("流血")]
    [StatusAction(EnumStatusAction.Life)]
    lx1,
    #endregion
    #region Buff 9个
    /// <summary>
    /// 洞察
    /// </summary>
    [FieldExplan("洞察")]
    [StatusAction(EnumStatusAction.HitRate, 
        EnumStatusAction.CritRate)]
    dc2 = 100,
    /// <summary>
    /// 活力
    /// </summary>
    [FieldExplan("活力")]
    [StatusAction(EnumStatusAction.Life,
        EnumStatusAction.StateResistance)]
    hl2,
    /// <summary>
    /// 加速
    /// </summary>
    [FieldExplan("加速")]
    [StatusAction(EnumStatusAction.MoveSpeed,
        EnumStatusAction.AttackSpeed)]
    js1,
    /// <summary>
    /// 净化
    /// </summary>
    [FieldExplan("净化")]
    [StatusAction(EnumStatusAction.ElementAffine)]
    jh5,
    /// <summary>
    /// 敏捷
    /// </summary>
    [FieldExplan("敏捷")]
    [StatusAction(EnumStatusAction.MoveSpeed,
        EnumStatusAction.EvadeRate)]
    mj1,
    /// <summary>
    /// 强力
    /// </summary>
    [FieldExplan("强力")]
    [StatusAction(EnumStatusAction.PhysicsPenetrate,
        EnumStatusAction.PhysicsAdditionalDamage)]
    ql1,
    /// <summary>
    /// 驱散
    /// </summary>
    [FieldExplan("驱散")]
    [StatusAction(EnumStatusAction.StateResistance)]
    qs2,
    /// <summary>
    /// 睿智
    /// </summary>
    [FieldExplan("睿智")]
    [StatusAction(EnumStatusAction.ImproveExperience)]
    rz1,
    /// <summary>
    /// 吸血(物理)
    /// </summary>
    [FieldExplan("吸血(物理)")]
    [StatusAction(EnumStatusAction.PhysicsSuckBlood)]
    xx3,
    /// <summary>
    /// 吸血(魔法)
    /// </summary>
    [FieldExplan("吸血(魔法)")]
    [StatusAction(EnumStatusAction.MagicSuckBlood)]
    xx4,
    #endregion

    #region 特殊状态 10个
    /// <summary>
    /// 嘲讽
    /// </summary>
    [FieldExplan("嘲讽")]
    [StatusAction(EnumStatusAction.CantLeaveTheFight,
        EnumStatusAction.HitOtherHurt)]
    cf2 = 200,
    /// <summary>
    /// 混乱
    /// </summary>
    [FieldExplan("混乱")]
    [StatusAction(EnumStatusAction.MoveSpeed,
        EnumStatusAction.MagicFinalDamage,
        EnumStatusAction.PhysicsFinalDamage,
        EnumStatusAction.CantReadTheArticle)]
    hl1,
    /// <summary>
    /// 禁锢
    /// </summary>
    [FieldExplan("禁锢")]
    [StatusAction(EnumStatusAction.CantMove,
        EnumStatusAction.ElementResistance,
        EnumStatusAction.MagicResisitance,
        EnumStatusAction.PhysicsResisitance,
        EnumStatusAction.StateResistance)]
    jg2,
    /// <summary>
    /// 禁魔
    /// </summary>
    [FieldExplan("禁魔")]
    [StatusAction(EnumStatusAction.CantMagic,
        EnumStatusAction.MagicResisitance)]
    jm3,
    /// <summary>
    /// 僵直
    /// </summary>
    [FieldExplan("僵直")]
    [StatusAction(EnumStatusAction.Stiff,
        EnumStatusAction.PhysicsResisitance)]
    jz6,
    /// <summary>
    /// 恐惧
    /// </summary>
    [FieldExplan("恐惧")]
    [StatusAction(EnumStatusAction.ElementResistance,
        EnumStatusAction.MagicResisitance,
        EnumStatusAction.PhysicsResisitance,
        EnumStatusAction.StateResistance,
        EnumStatusAction.MagicFinalDamage,
        EnumStatusAction.PhysicsFinalDamage)]
    kj1,
    /// <summary>
    /// 麻痹
    /// </summary>
    [FieldExplan("麻痹")]
    [StatusAction(EnumStatusAction.Stiff,
        EnumStatusAction.MagicResisitance)]
    mb2,
    /// <summary>
    /// 魅惑
    /// </summary>
    [FieldExplan("魅惑")]
    [StatusAction(EnumStatusAction.HitTargetHurt,
        EnumStatusAction.ElementResistance)]
    mh4,
    /// <summary>
    /// 眩晕
    /// </summary>
    [FieldExplan("眩晕")]
    [StatusAction(EnumStatusAction.Vertigo,
        EnumStatusAction.ElementResistance,
        EnumStatusAction.MagicResisitance,
        EnumStatusAction.PhysicsResisitance,
        EnumStatusAction.StateResistance)]
    xy1,
    /// <summary>
    /// 致盲
    /// </summary>
    [FieldExplan("致盲")]
    [StatusAction(EnumStatusAction.View,
        EnumStatusAction.HitRate)]
    zm1,
    #endregion

}

/// <summary>
/// 状态的效果枚举
/// EnumStatusEffect枚举可以包含一个或多个该效果
/// </summary>
public enum EnumStatusAction
{
    /// <summary>
    /// 移动速度变化
    /// </summary>
    [FieldExplan("移动速度变化")]
    MoveSpeed,
    /// <summary>
    /// 无法移动
    /// </summary>
    [FieldExplan("无法移动")]
    CantMove,
    /// <summary>
    /// 攻击速度变化
    /// </summary>
    [FieldExplan("攻击速度变化")]
    AttackSpeed,
    /// <summary>
    /// 抗性变化（主要对应的是各种Buff状态的抗性）
    /// </summary>
    [FieldExplan("抗性变化（主要对应的是各种Buff状态的抗性）")]
    StateResistance,  
    /// <summary>
    /// 无法离开战斗，攻击嘲讽目标以外的敌人时伤害降低
    /// </summary>
    [FieldExplan("无法离开战斗,攻击嘲讽目标以外的敌人时伤害降低")]
    CantLeaveTheFight,
    /// <summary>
    /// 攻击其他目标伤害变化
    /// </summary>
    [FieldExplan("攻击其他目标伤害变化")]
    HitOtherHurt,
    /// <summary>
    /// 攻击指定目标伤害变化
    /// </summary>
    [FieldExplan("攻击指定目标伤害变化")]
    HitTargetHurt,
    /// <summary>
    /// 生命值变化
    /// </summary>
    [FieldExplan("生命值变化")]
    Life,
    /// <summary>
    /// 元素抗性变化（主要适用于防御计算）
    /// </summary>
    [FieldExplan("元素抗性变化（主要适用于防御计算）")]
    ElementResistance,
    /// <summary>
    /// 元素亲和性变化(主要适用于攻击计算)
    /// </summary>
    [FieldExplan("元素亲和性变化(主要适用于攻击计算)")]
    ElementAffine,
    /// <summary>
    /// 魔法亲和性变化
    /// </summary>
    [FieldExplan("魔法亲和性变化")]
    MagicAffine,
    /// <summary>
    /// 生命恢复速度变化
    /// </summary>
    [FieldExplan("生命恢复速度变化")]
    LifeRecoverySpeed,
    /// <summary>
    /// 法力恢复速度变化
    /// </summary>
    [FieldExplan("法力恢复速度变化")]
    ManaRecoverySpeed,
    /// <summary>
    /// 命中率变化
    /// </summary>
    [FieldExplan("命中率变化")]
    HitRate,
    /// <summary>
    /// 闪避率变化
    /// </summary>
    [FieldExplan("闪避率变化")]
    EvadeRate,
    /// <summary>
    /// 暴击率变化
    /// </summary>
    [FieldExplan("暴击率变化")]
    CritRate,
    /// <summary>
    /// 禁止使用读条技能
    /// </summary>
    [FieldExplan("禁止使用读条技能")]
    CantReadTheArticle,
    /// <summary>
    /// 禁止使用魔法(消耗魔力的)技能
    /// </summary>
    [FieldExplan("禁止使用魔法(消耗魔力的)技能")]
    CantMagic,
    /// <summary>
    /// 禁止使用物理技能
    /// </summary>
    [FieldExplan("禁止使用物理技能")]
    CanPhysics,
    /// <summary>
    /// 僵直
    /// </summary>
    [FieldExplan("僵直")]
    Stiff,
    /// <summary>
    /// 眩晕
    /// </summary>
    [FieldExplan("眩晕")]
    Vertigo,
    /// <summary>
    /// 提高经验变化
    /// </summary>
    [FieldExplan("提高经验变化")]
    ImproveExperience,
    /// <summary>
    /// 视野变化
    /// </summary>
    [FieldExplan("视野变化")]
    View,
    /// <summary>
    /// 属性变化
    /// </summary>
    [FieldExplan("属性变化")]
    AttributeChange,
    /// <summary>
    /// 魔法攻击造成吸血效果
    /// </summary>
    [FieldExplan("魔法攻击造成吸血效果")]
    MagicSuckBlood,
    /// <summary>
    /// 物理攻击造成吸血效果
    /// </summary>
    [FieldExplan("物理攻击造成吸血效果")]
    PhysicsSuckBlood,
    /// <summary>
    /// 魔法抗性变化
    /// </summary>
    [FieldExplan("魔法抗性变化")]
    MagicResisitance,
    /// <summary>
    /// 物理抗性变化
    /// </summary>
    [FieldExplan("物理抗性变化")]
    PhysicsResisitance,
    /// <summary>
    /// 魔法攻击穿透(无视指定的魔法抗性)
    /// </summary>
    [FieldExplan("魔法攻击穿透（无视指定的魔法抗性）")]
    MagicPenetrate,
    /// <summary>
    /// 物理攻击穿透（无视指定的物理抗性）
    /// </summary>
    [FieldExplan("物理攻击穿透（无视指定的物理抗性）")]
    PhysicsPenetrate,
    /// <summary>
    /// 魔法伤害附加(最终结算时附加)
    /// </summary>
    [FieldExplan("魔法伤害附加(最终结算时附加)")]
    MagicAdditionalDamage,
    /// <summary>
    /// 物理伤害附加(最终结算时附加)
    /// </summary>
    [FieldExplan("物理伤害附加(最终结算时附加)")]
    PhysicsAdditionalDamage,
    /// <summary>
    /// 魔法最终伤害变化
    /// </summary>
    [FieldExplan("魔法最终伤害变化")]
    MagicFinalDamage,
    /// <summary>
    /// 物理最终伤害变化
    /// </summary>
    [FieldExplan("物理最终伤害变化")]
    PhysicsFinalDamage,
}

/// <summary>
/// 用于挂载在EnumStatusEffect枚举上，表示该状态类型包含了哪些状态效果
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class StatusActionAttribute : Attribute
{
    private EnumStatusAction[] statusActions;

    public StatusActionAttribute(params EnumStatusAction[] statusActions)
    {
        this.statusActions = statusActions;
    }

    public EnumStatusAction[] GetStatusActions()
    {
        return statusActions.Clone() as EnumStatusAction[];
    }
}
