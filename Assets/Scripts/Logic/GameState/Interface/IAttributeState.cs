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

    /// <summary>
    /// 设置种族成长对象
    /// </summary>
    /// <param name="roleOfRaceInfoStruct">种族成长对象</param>
    void SetRoleOfRaceAddition(RoleOfRaceInfoStruct roleOfRaceInfoStruct);

    #region 这些属性会影响到下面的计算 而这些属性由角色自身以及装备累加计算
    /// <summary>
    /// 敏捷
    /// </summary>
    [FieldExplan("敏捷")] float Quick { get; set; }
    /// <summary>
    /// 精神
    /// </summary>
    [FieldExplan("精神")] float Mental { get; set; }
    /// <summary>
    /// 力量
    /// </summary>
    [FieldExplan("力量")] float Power { get; set; }
    /// <summary>
    /// 基础物理护甲
    /// </summary>
    [FieldExplan("基础物理护甲")] float BasePhysicDefense { get; set; }
    /// <summary>
    /// 基础物理伤害
    /// </summary>
    [FieldExplan("基础物理伤害")] float BasePhysicDamage { get; set; }
    /// <summary>
    /// 基础法术伤害
    /// </summary>
    [FieldExplan("基础法术伤害")] float BaseMagicDamage { get; set; }
    #endregion

    #region 常规属性
    /// <summary>
    /// 血量
    /// </summary>
    [FieldExplan("血量")] float HP { get; set; }
    /// <summary>
    /// 最大血量
    /// </summary>
    [FieldExplan("最大血量")] float MaxHP { get; set; }
    /// <summary>
    /// 魔力量
    /// </summary>
    [FieldExplan("魔力量")] float Mana { get; set; }
    /// <summary>
    /// 最大魔力量
    /// </summary>
    [FieldExplan("最大魔力量")] float MaxMana { get; set; }
    /// <summary>
    /// 最大耗魔上限
    /// </summary>
    [FieldExplan("最大耗魔上限")] float MaxUseMana { get; set; }
    /// <summary>
    /// 精神力计量
    /// </summary>
    [FieldExplan("精神力计量")] float Mentality { get; set; }
    /// <summary>
    /// 最大精神力计量
    /// </summary>
    [FieldExplan("最大精神力计量")] float MaxMentality { get; set; }
    /// <summary>
    /// 心志力计量
    /// </summary>
    [FieldExplan("心志力计量")] float MindTraining { get; set; }
    /// <summary>
    /// 最大心志力计量
    /// </summary>
    [FieldExplan("最大心志力计量")] float MaxMindTraining { get; set; }
    /// <summary>
    /// 视野范围
    /// </summary>
    [FieldExplan("视野范围")] float View { get; set; }
    /// <summary>
    /// 降低被怪物发现的概率(被发现的距离倍率)
    /// </summary>
    [FieldExplan("降低被怪物发现的概率")] float SightDef { get; set; }
    /// <summary>
    /// 移动速度
    /// </summary>
    [FieldExplan("移动速度")] float MoveSpeed { get; set; }
    /// <summary>
    /// 攻击速度
    /// </summary>
    [FieldExplan("攻击速度")] float AttackSpeed { get; set; }
    /// <summary>
    /// 命中率
    /// </summary>
    [FieldExplan("命中率")] float HitRate { get; set; }
    /// <summary>
    /// 闪避率
    /// </summary>
    [FieldExplan("闪避率")] float EvadeRate { get; set; }
    /// <summary>
    /// 暴击率
    /// </summary>
    [FieldExplan("暴击率")] float CritRate { get; set; }
    #endregion

    #region 回复
    /// <summary>
    /// 生命恢复速度
    /// </summary>
    [FieldExplan("生命回复速度")] float LifeRecovery { get; set; }
    /// <summary>
    /// 法力恢复速度
    /// </summary>
    [FieldExplan("法力恢复速度")] float ManaRecovery { get; set; }
    #endregion

    #region 攻击与防御属性
    /// <summary>
    /// 伤害格挡率
    /// </summary>
    [FieldExplan("伤害格挡率")] float EquipBlock { get; set; }
    /// <summary>
    /// 暴击伤害减少率
    /// </summary>
    [FieldExplan("暴击伤害减少率")] float CriticalDef { get; set; }
    /// <summary>
    /// 攻击僵直
    /// </summary>
    [FieldExplan("攻击僵直")] float AttackRigidity { get; set; }
    /// <summary>
    /// 道具攻击力
    /// </summary>
    [FieldExplan("道具攻击力")] float ItemAttacking { get; set; }
    /// <summary>
    /// 魔法攻击力
    /// </summary>
    [FieldExplan("魔法攻击力")] float MagicAttacking { get; set; }
    /// <summary>
    /// 物理攻击力
    /// </summary>
    [FieldExplan("物理攻击力")] float PhysicsAttacking { get; set; }
    /// <summary>
    /// 物理最小伤害(通过敏捷计算出来的值,也有一些装备会附加该数值)
    /// </summary>
    [FieldExplan("物理最小伤害")] float PhysicsMinHurt { get; set; }
    /// <summary>
    /// 魔法最小伤害(通过敏捷计算出来的值,也有一些装备会附加该数值)
    /// </summary>
    [FieldExplan("魔法最小伤害")] float MagicMinHurt { get; set; }
    /// <summary>
    /// 魔法附加伤害 
    /// </summary>
    [FieldExplan("魔法附加伤害")] float MagicAdditionalDamage { get; set; }
    /// <summary>
    /// 物理伤害附加
    /// </summary>
    [FieldExplan("物理附加伤害")] float PhysicsAdditionalDamage { get; set; }
    /// <summary>
    /// 魔法攻击穿透
    /// </summary>
    [FieldExplan("魔法攻击穿透")] float MagicPenetrate { get; set; }
    /// <summary>
    /// 物理攻击穿透
    /// </summary>
    [FieldExplan("物理攻击穿透")] float PhysicsPenetrate { get; set; }
    /// <summary>
    /// 魔法最终伤害
    /// </summary>
    [FieldExplan("魔法最终伤害")] float MagicFinalDamage { get; set; }
    /// <summary>
    /// 暴击倍率(角色本身为1.5倍)
    /// </summary>
    [FieldExplan("暴击倍率")] float CritDamageRatio { get; set; }
    /// <summary>
    /// 法术陷阱伤害提升(百分比)
    /// </summary>
    [FieldExplan("法术陷阱伤害提升")] float SpellTrapDamage { get; set; }
    /// <summary>
    /// 法术陷阱特效产生几率
    /// </summary>
    [FieldExplan("法术陷阱特效产生几率")] float SpellTrapEffectProbability { get; set; }
    /// <summary>
    /// 对不死族伤害提升(百分比倍率)
    /// </summary>
    [FieldExplan("对不死族伤害提升")] float DamageToTheUndead { get; set; }
    /// <summary>
    /// 对不死族附加混乱几率
    /// </summary>
    [FieldExplan("对不死族附加混乱几率")] float ChaosOfTheUndead { get; set; }
    /// <summary>
    /// 治疗量
    /// </summary>
    [FieldExplan("治疗量")] float TreatmentVolume { get; set; }
    /// <summary>
    /// 物理最终伤害
    /// </summary>
    [FieldExplan("物理最终伤害")] float PhysicsFinalDamage { get; set; }
    /// <summary>
    /// 特效影响力
    /// </summary>
    [FieldExplan("特效影响力")] float EffectAffine { get; set; }
    /// <summary>
    /// 驻留时间
    /// </summary>
    [FieldExplan("驻留时间")] float EffectResideTime { get; set; }
    /// <summary>
    /// 魔法亲和
    /// </summary>
    [FieldExplan("魔法亲和")] float MagicFit { get; set; }
    /// <summary>
    /// 魔法防御
    /// </summary>
    [FieldExplan("魔法防御")] float MagicResistance { get; set; }
    /// <summary>
    /// 物理防御
    /// </summary>
    [FieldExplan("物理防御")] float PhysicsResistance { get; set; }
    /// <summary>
    /// 对陷阱的防御力
    /// </summary>
    [FieldExplan("对陷阱的防御力")] float TrapDefense { get; set; }
    /// <summary>
    /// 光明信仰强度
    /// </summary>
    [FieldExplan("光明信仰强度")] float LightFaith { get; set; }
    /// <summary>
    /// 黑暗信仰强度
    /// </summary>
    [FieldExplan("黑暗信仰强度")] float DarkFaith { get; set; }
    /// <summary>
    /// 生物信仰强度
    /// </summary>
    [FieldExplan("生物信仰强度")] float LifeFaith { get; set; }
    /// <summary>
    /// 自然信仰强度
    /// </summary>
    [FieldExplan("自然信仰强度")] float NaturalFaith { get; set; }
    /// <summary>
    /// 神秘信仰强度
    /// </summary>
    [FieldExplan("神秘信仰强度")] float MysticalBeliefIntensity { get; set; }
    /// <summary>
    /// 神秘信仰特效产生几率
    /// </summary>
    [FieldExplan("神秘信仰特效产生几率")] float MysticalBeliefSpecialEffects { get; set; }
    /// <summary>
    /// 崇拜信仰强度
    /// </summary>
    [FieldExplan("崇拜信仰强度")] float ImproveWorshipFaith { get; set; }
    /// <summary>
    /// 异常状态抗性
    /// </summary>
    [FieldExplan("异常状态抗性")] float AbnormalStateResistance { get; set; }
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
    [FieldExplan("元素立场强度")] float ElementStandStrength { get; set; }
    /// <summary>
    /// 同元素魔法效果加成
    /// </summary>
    [FieldExplan("同元素魔法效果加成")] float SameElementEffectAdded { get; set; }
    /// <summary>
    /// 技能冷却时间
    /// </summary>
    [FieldExplan("技能冷却时间")] float CoolingTime { get; set; }
    /// <summary>
    /// 需要使用的基础耗魔量(主要是组合技能以及需要主动释放的技能存在此选项)
    /// </summary>
    [FieldExplan("需要使用的基础耗魔量")] float MustUsedBaseMana { get; set; }
    #endregion

    #region 其他杂项
    /// <summary>
    /// 减少该技能的冷却时间(百分比)
    /// </summary>
    [FieldExplan("减少该技能的冷却时间")] float ExemptionChatingMana { get; set; }
    /// <summary>
    /// 耗魔量减免(百分比)
    /// </summary>
    [FieldExplan("耗魔量减免")] float ReliefManaAmount { get; set; }
    /// <summary>
    /// 咏唱时间减免(百分比)
    /// </summary>
    [FieldExplan("咏唱时间减免")] float ExemptionChantingTime { get; set; }
    /// <summary>
    /// 公共冷却时间减免(百分比)
    /// </summary>
    [FieldExplan("公共冷却时间减免")] float ReduceCoolingTime { get; set; }
    /// <summary>
    /// 对不死族加速
    /// </summary>
    [FieldExplan("对不死族加速")] float AccelerateToUndead { get; set; }
    /// <summary>
    /// 经验值加成(与基础经验乘算)
    /// </summary>
    [FieldExplan("经验值加成")] float ExperienceValuePlus { get; set; }
    /// <summary>
    /// 物品掉落率(与基础掉落率乘算)
    /// </summary>
    [FieldExplan("物品掉落率")] float GooodsDropRate { get; set; }
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
