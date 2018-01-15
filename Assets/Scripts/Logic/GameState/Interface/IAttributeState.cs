using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 角色或怪物的属性（血量 攻击 防御等）
/// </summary>
public interface IAttributeState : IBaseState
{
    /// <summary>
    /// 初始化属性
    /// </summary>
    void Init();

    #region 这些属性会影响到下面的计算 而这些属性由角色自身以及装备累加计算
    /// <summary>
    /// 敏捷
    /// </summary>
    float Quick { get; set; }
    /// <summary>
    /// 精神
    /// </summary>
    float Mental { get; set; }
    /// <summary>
    /// 力量
    /// </summary>
    float Power { get; set; }
    /// <summary>
    /// 基础物理护甲
    /// </summary>
    float BasePhysicDefense { get; set; }
    /// <summary>
    /// 基础物理伤害
    /// </summary>
    float BasePhysicDamage { get; set; }
    #endregion

    #region 常规属性
    /// <summary>
    /// 血量
    /// </summary>
    float HP { get; set; }
    /// <summary>
    /// 最大血量
    /// </summary>
    float MaxHP { get; set; }
    /// <summary>
    /// 魔力量
    /// </summary>
    float Mana { get; set; }
    /// <summary>
    /// 最大魔力量
    /// </summary>
    float MaxMana { get; set; }
    /// <summary>
    /// 最大耗魔上限
    /// </summary>
    float MaxUseMana { get; set; }
    /// <summary>
    /// 视野范围
    /// </summary>
    float View { get; set; }
    /// <summary>
    /// 降低被怪物发现的概率(被发现的距离倍率)
    /// </summary>
    float SightDef { get; set; }
    /// <summary>
    /// 移动速度
    /// </summary>
    float MoveSpeed { get; set; }
    /// <summary>
    /// 攻击速度
    /// </summary>
    float AttackSpeed { get; set; }
    /// <summary>
    /// 命中率
    /// </summary>
    float HitRate { get; set; }
    /// <summary>
    /// 闪避率
    /// </summary>
    float EvadeRate { get; set; }
    /// <summary>
    /// 暴击率
    /// </summary>
    float CritRate { get; set; }
    #endregion

    #region 回复
    /// <summary>
    /// 生命恢复速度
    /// </summary>
    float LifeRecovery { get; set; }
    /// <summary>
    /// 法力恢复速度
    /// </summary>
    float ManaRecovery { get; set; }
    #endregion

    #region 攻击与防御属性
    /// <summary>
    /// 伤害格挡率
    /// </summary>
    float EquipBlock { get; set; }
    /// <summary>
    /// 暴击率伤害减少率
    /// </summary>
    float CriticalDef { get; set; }
    /// <summary>
    /// 攻击僵直
    /// </summary>
    float AttackRigidity { get; set; }
    /// <summary>
    /// 道具攻击力
    /// </summary>
    float ItemAttacking { get; set; }
    /// <summary>
    /// 魔法攻击力
    /// </summary>
    float MagicAttacking { get; set; }
    /// <summary>
    /// 物理攻击力
    /// </summary>
    float PhysicsAttacking { get; set; }
    /// <summary>
    /// 魔法附加伤害 
    /// </summary>
    float MagicAdditionalDamage { get; set; }
    /// <summary>
    /// 物理伤害附加
    /// </summary>
    float PhysicsAdditionalDamage { get; set; }
    /// <summary>
    /// 魔法攻击穿透
    /// </summary>
    float MagicPenetrate { get; set; }
    /// <summary>
    /// 物理攻击穿透
    /// </summary>
    float PhysicsPenetrate { get; set; }
    /// <summary>
    /// 魔法最终伤害
    /// </summary>
    float MagicFinalDamage { get; set; }
    /// <summary>
    /// 暴击倍率(角色本身为1.5倍)
    /// </summary>
    float CritDamageRatio { get; set; }
    /// <summary>
    /// 法术陷阱伤害提升(百分比)
    /// </summary>
    float SpellTrapDamage { get; set; }
    /// <summary>
    /// 法术陷阱特效产生几率
    /// </summary>
    float SpellTrapEffectProbability { get; set; }
    /// <summary>
    /// 对不死族伤害提升(百分比倍率)
    /// </summary>
    float DamageToTheUndead { get; set; }
    /// <summary>
    /// 对不死族附加混乱几率
    /// </summary>
    float ChaosOfTheUndead { get; set; }
    /// <summary>
    /// 治疗量
    /// </summary>
    float TreatmentVolume { get; set; }
    /// <summary>
    /// 物理最终伤害
    /// </summary>
    float PhysicsFinalDamage { get; set; }
    /// <summary>
    /// 特效影响力
    /// </summary>
    float EffectAffine { get; set; }
    /// <summary>
    /// 驻留时间
    /// </summary>
    float EffectResideTime { get; set; }
    /// <summary>
    /// 魔法亲和
    /// </summary>
    float MagicFit { get; set; }
    /// <summary>
    /// 魔法防御
    /// </summary>
    float MagicResistance { get; set; }
    /// <summary>
    /// 物理防御
    /// </summary>
    float PhysicsResistance { get; set; }
    /// <summary>
    /// 对陷阱的防御力
    /// </summary>
    float TrapDefense { get; set; }
    /// <summary>
    /// 光明信仰强度
    /// </summary>
    float LightFaith { get; set; }
    /// <summary>
    /// 黑暗信仰强度
    /// </summary>
    float DarkFaith { get; set; }
    /// <summary>
    /// 生物信仰强度
    /// </summary>
    float LifeFaith { get; set; }
    /// <summary>
    /// 自然信仰强度
    /// </summary>
    float NaturalFaith { get; set; }
    /// <summary>
    /// 神秘信仰强度
    /// </summary>
    float MysticalBeliefIntensity { get; set; }
    /// <summary>
    /// 神秘信仰特效产生几率
    /// </summary>
    float MysticalBeliefSpecialEffects { get; set; }
    /// <summary>
    /// 崇拜信仰强度
    /// </summary>
    float ImproveWorshipFaith { get; set; }
    /// <summary>
    /// 异常状态抗性
    /// </summary>
    float AbnormalStateResistance { get; set; }
    /// <summary>
    /// 元素抗性,根据元素类型枚举的顺序
    /// </summary>
    float[] ElementResistances { get; set; }
    /// <summary>
    /// 状态抗性，按照EnumStatusEffect枚举（状态类型）的顺序
    /// </summary>
    float[] StateResistances { get; set; }
    /// <summary>
    /// 元素立场强度
    /// </summary>
    float ElementStandStrength { get; set; }
    /// <summary>
    /// 同元素魔法效果加成
    /// </summary>
    float SameElementEffectAdded { get; set; }
    /// <summary>
    /// 技能冷却时间
    /// </summary>
    float CoolingTime { get; set; }
    /// <summary>
    /// 需要使用的基础耗魔量(主要是组合技能以及需要主动释放的技能存在此选项)
    /// </summary>
    float MustUsedBaseMana { get; set; }
    #endregion

    #region 其他杂项
    /// <summary>
    /// 减少该技能的冷却时间
    /// </summary>
    float ExemptionChatingMana { get; set; }
    /// <summary>
    /// 耗魔量减免(百分比)
    /// </summary>
    float ReliefManaAmount { get; set; }
    /// <summary>
    /// 咏唱时间减免(百分比)
    /// </summary>
    float ExemptionChantingTime { get; set; }
    /// <summary>
    /// 公共冷却时间减免(百分比)
    /// </summary>
    float ReduceCoolingTime { get; set; }
    /// <summary>
    /// 对不死族加速
    /// </summary>
    float AccelerateToUndead { get; set; }
    /// <summary>
    /// 经验值加成(与基础经验乘算)
    /// </summary>
    float ExperienceValuePlus { get; set; }
    /// <summary>
    /// 物品掉落率(与基础掉落率乘算)
    /// </summary>
    float GooodsDropRate { get; set; }
    #endregion

    #region 特殊效果
    /// <summary>
    /// 幸运加护,获得优质物品概率与获取经验提升
    /// </summary>
    float LuckShi { get; set; }
    /// <summary>
    /// 庇佑加护,每隔一定时间获得一次免疫致死伤害的能力
    /// </summary>
    float GarShi { get; set; }
    /// <summary>
    /// 战神加护,每隔一段时间获得一次在进入负面状态时清除自身所有负面效果的能力
    /// </summary>
    float WarShi { get; set; }
    #endregion
}
