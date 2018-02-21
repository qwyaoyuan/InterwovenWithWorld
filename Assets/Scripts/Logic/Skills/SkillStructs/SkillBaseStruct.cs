using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 技能基础数据（只包含最基础的技能，组合技能拆开计算）
/// </summary>
public class SkillBaseStruct
{
    /// <summary>
    /// 技能id（唯一标识）
    /// </summary>
    public string id;
    /// <summary>
    /// 技能名
    /// </summary>
    public string name;
    /// <summary>
    /// 技能类型，具体的每个技能
    /// </summary>
    public EnumSkillType skillType;
    /// <summary>
    /// 技能的释放模式，标识这是魔法1 还是魔法2 或者是信仰1之类的
    /// </summary>
    public EnumReleaseMode skillMode;
    /// <summary>
    /// 该技能包含的粒子
    /// </summary>
    //public GameObject[] particals;
    /// <summary>
    /// 技能的信仰类型(如果非信仰的技能释放模式，则忽略该字段 )
    /// </summary>
    public EnumSkillBelief skillBelief;
    /// <summary>
    /// 技能的特殊效果
    /// </summary>
    public EnumStatusEffect[] skillStatusEffect;
    /// <summary>
    /// 技能的最高等级 
    /// </summary>
    public int maxLevel;
    /// <summary>
    /// 技能的属性结构数组
    /// 数据长度与技能的最高等级一致
    /// </summary>
    public SkillAttributeStruct[] skillAttributeStructs;

    /// <summary>
    /// 技能的前置条件
    /// </summary>
    public SkillPrecondition skillPrecondition;

    /// <summary>
    /// 技能所属分组数组
    /// </summary>
    public EnumSkillZone[] skillZones;

    //运行时生成的数据↓↓↓
    /// <summary>
    /// 技能的图标(用于显示)
    /// </summary>
    public Sprite skillSprite;
    /// <summary>
    /// 技能的图标(用于组合)
    /// </summary>
    public Sprite skillSprite_Combine;
    /// <summary>
    /// 技能显示的名字(根据语言进行翻译)
    /// </summary>
    public string skillName;
}

/// <summary>
/// 技能的前置条件
/// </summary>
public class SkillPrecondition
{
    /// <summary>
    /// 需求某组技能投入的总点数
    /// </summary>
    public Dictionary<EnumSkillZone, int> mustSkillZonePointDic;
    /// <summary>
    /// 需求某技能投入的点数
    /// </summary>
    public Dictionary<EnumSkillType, int> mustSkillPointDic;
}

/// <summary>
/// 技能的属性结构
/// </summary>
public class SkillAttributeStruct
{    
    /// <summary>
    /// 法伤
    /// </summary>
    [FieldExplan("法伤", "DMG")] public int DMG;
    /// <summary>
    /// 物伤
    /// </summary>
    [FieldExplan("物伤", "PDMG")] public int PDMG;
    /// <summary>
    /// 特效影响力(伤害-->点燃 减抗性等)
    /// </summary>
    [FieldExplan("特效影响力(伤害-->点燃 减抗性等)", "ERST")] public int ERST;
    /// <summary>
    /// 僵直
    /// </summary>
    [FieldExplan("僵直", "Catalepsy")] public int Catalepsy;
    /// <summary>
    /// 驻留时间(持续时间-->特效 buff 屏障 范围伤害持续)
    /// </summary>
    [FieldExplan("驻留时间(持续时间-->特效 buff 屏障 范围伤害持续)", "RETI")] public int RETI;
    /// <summary>
    /// 物理伤害附加
    /// </summary>
    [FieldExplan("物理伤害附加", "PHYAD")] public int PHYAD;
    /// <summary>
    /// 魔法伤害附加
    /// </summary>
    [FieldExplan("魔法伤害附加", "MPAD")] public int MPAD;
    /// <summary>
    /// 闪避率
    /// </summary>
    [FieldExplan("闪避率", "DodgeRate")] public int DodgeRate;
    /// <summary>
    /// 力量加成
    /// </summary>
    [FieldExplan("力量加成", "StrengthAdded")] public int StrengthAdded;
    /// <summary>
    /// 魔法伤害穿透
    /// </summary>
    [FieldExplan("魔法伤害穿透", "PEDMG")] public int PEDMG;
    /// <summary>
    /// 物理伤害穿透
    /// </summary>
    [FieldExplan("物理伤害穿透", "PEDMG")] public int PYEDMG;
    /// <summary>
    /// 最大耗魔上限
    /// </summary>
    [FieldExplan("最大耗魔上限", "MaxMP")] public int MaxMP;
    /// <summary>
    /// 法力上限加成百分比
    /// </summary>
    [FieldExplan("法力上限加成百分比", "MP")] public int MP;
    /// <summary>
    /// 魔法亲和性
    /// </summary>
    [FieldExplan("魔法亲和性", "MagicFit")] public int MagicFit;
    /// <summary>
    /// 魔法攻击力加成百分比
    /// </summary>
    [FieldExplan("魔法攻击力加成百分比", "MpAttack")] public int MpAttack;
    /// <summary>
    /// 魔法防御力加成百分比
    /// </summary>
    [FieldExplan("魔法防御力加成百分比", "MpDefence")] public int MpDefence;
        /// <summary>
    /// 魔法回复速度加成
    /// </summary>
    [FieldExplan("魔法回复速度加成", "MpReload")] public int MpReload;
    /// <summary>
    /// 光明信仰强度
    /// </summary>
    [FieldExplan("光明信仰强度", "光明信仰强度")] public int Light;
    /// <summary>
    /// 黑暗信仰强度
    /// </summary>
    [FieldExplan("黑暗信仰强度", "黑暗信仰强度")] public int Dark;
    /// <summary>
    /// 生物信仰强度
    /// </summary>
    [FieldExplan("生物信仰强度", "生物信仰强度")] public int Life;
    /// <summary>
    /// 自然信仰强度
    /// </summary>
    [FieldExplan("自然信仰强度", "自然信仰强度")] public int Natural;
    /// <summary>
    /// 技能特效等级 0 表示岁当前技能等级  -1表示没有特效  大于0表示有特效且等级为该数值
    /// </summary>
    [FieldExplan("技能特效等级", "SkillSpecialLevel")] public int SkillSpecialLevel;
    /// <summary>
    /// 减少技能冷却时间
    /// </summary>
    [FieldExplan("减少技能冷却时间", "ExemptionChatingMana")] public int ExemptionChatingMana;
    /// <summary>
    /// 同元素魔法效果加成
    /// </summary>
    [FieldExplan("同元素魔法效果加成", "SameElementEffectAdded")] public int SameElementEffectAdded;
    /// <summary>
    /// 耗魔量减免
    /// </summary>
    [FieldExplan("耗魔量减免", "ReliefManaAmount")] public int ReliefManaAmount;
    /// <summary>
    /// 异常状态抗性
    /// </summary>
    [FieldExplan("异常状态抗性", "AbnormalStateResistance")] public int AbnormalStateResistance;
    /// <summary>
    /// 移动速度增加量(百分比增加)
    /// </summary>
    [FieldExplan("移动速度增加量", "MoveSpeedAddtion")] public int MoveSpeedAddtion;
    /// <summary>
    /// 咏唱时间减免(百分比减少)
    /// </summary>
    [FieldExplan("咏唱时间减免", "ExemptionChatingTime")] public int ExemptionChatingTime;
    /// <summary>
    /// 冷却时间减免(百分比减少)
    /// </summary>
    [FieldExplan("冷却时间减免", "ReduceCoolingTime")] public int ReduceCoolingTime;
    /// <summary>
    /// 暴击率提升
    /// </summary>
    [FieldExplan("暴击率提升", "IncreasedCritRate")] public int IncreasedCritRate;
    /// <summary>
    /// 暴击伤害提升
    /// </summary>
    [FieldExplan("暴击伤害提升", "CritDamagePromotion")] public int CritDamagePromotion;
    /// <summary>
    /// 法术陷阱伤害提升
    /// </summary>
    [FieldExplan("法术陷阱伤害提升", "SpellTrapDamagePromotion")] public int SpellTrapDamagePromotion;
    /// <summary>
    /// 法术陷阱特效产生几率
    /// </summary>
    [FieldExplan("法术陷阱特效产生几率", "SpellTrapEffectProbability")] public int SpellTrapEffectProbability;
    /// <summary>
    /// 对不死族伤害提升
    /// </summary>
    [FieldExplan("对不死族伤害提升", "DamageToTheUndead")] public int DamageToTheUndead;
    /// <summary>
    /// 对不死族附加混乱几率
    /// </summary>
    [FieldExplan("对不死族附加混乱几率", "ChaosOfTheUndead")] public int ChaosOfTheUndead;
    /// <summary>
    /// 治疗量
    /// </summary>
    [FieldExplan("治疗量", "TreatmentVolume")] public int TreatmentVolume;
    /// <summary>
    /// 神秘信仰强度
    /// </summary>
    [FieldExplan("神秘信仰强度", "MysticalBeliefIntensity")] public int MysticalBeliefIntensity;
    /// <summary>
    /// 神秘信仰特效产生几率
    /// </summary>
    [FieldExplan("神秘信仰特效产生几率", "MysticalBeliefSpecialEffects")] public int MysticalBeliefSpecialEffects;
    /// <summary>
    /// 增幅信仰1
    /// </summary>
    [FieldExplan("增幅信仰1", "IncreaseFaithA")] public int IncreaseFaithA;
    /// <summary>
    /// 提升力量比率
    /// </summary>
    [FieldExplan("提升力量比率", "LiftingForceRatio")] public int LiftingForceRatio;
    /// <summary>
    /// 提升精神比率
    /// </summary>
    [FieldExplan("提升精神比率", "RaiseSpiritRatio")] public int RaiseSpiritRatio;
    /// <summary>
    /// 崇拜信仰强度提升
    /// </summary>
    [FieldExplan("崇拜信仰强度提升", "ImproveWorshipFaith")] public int ImproveWorshipFaith;
    /// <summary>
    /// 对不死族加速
    /// </summary>
    [FieldExplan("对不死族加速", "AccelerateToUndead")] public int AccelerateToUndead;
    /// <summary>
    /// 提升信仰强度差值获得额外魔法效果加成
    /// </summary>
    [FieldExplan("提升信仰强度差值获得额外魔法效果加成", "DamageOfFaithIntensityDifference")] public int DamageOfFaithIntensityDifference;
    /// <summary>
    /// 魔法三(包括)以上魔法伤害加成
    /// </summary>
    [FieldExplan("魔法三(包括)以上魔法伤害加成", "MagicThreeDamagePromotion")] public int MagicThreeDamagePromotion;
    /// <summary>
    /// 元素立场强度提升
    /// </summary>
    [FieldExplan("元素立场强度提升", "ElementStandStrength")] public int ElementStandStrength;
    /// <summary>
    /// 非战斗状态移动速度提升
    /// </summary>
    [FieldExplan("非战斗状态移动速度提升", "MoveSpeedAddtionNotFighting")] public int MoveSpeedAddtionNotFighting;
    /// <summary>
    /// 物理攻击力百分比加成
    /// </summary>
    [FieldExplan("物理攻击力百分比加成", "PhyAttack")] public int PhyAttack;
    /// <summary>
    /// 物理防御力百分比加成
    /// </summary>
    [FieldExplan("物理防御力百分比加成", "PhyDefense")] public int PhyDefense;
    /// <summary>
    /// 敏捷加成
    /// </summary>
    [FieldExplan("敏捷加成", "AgileBonus")] public int AgileBonus;
    /// <summary>
    /// 经验值加成
    /// </summary>
    [FieldExplan("经验值加成", "ExperienceValuePlus")] public int ExperienceValuePlus;
    /// <summary>
    /// 对陷阱防御力
    /// </summary>
    [FieldExplan("对陷阱防御力", "TrapDefense")] public int TrapDefense;
    /// <summary>
    /// 陷阱攻击力
    /// </summary>
    [FieldExplan("陷阱攻击力", "TrapAttack")] public int TrapAttack;
    /// <summary>
    /// 攻击速度
    /// </summary>
    [FieldExplan("攻击速度", "TrapAttack")] public int AttackSpeed;
    /// <summary>
    /// 物品掉落率加成(乘算)
    /// </summary>
    [FieldExplan("物品掉落率加成", "GooodsDropRate")] public int GooodsDropRate;
    /// <summary>
    /// 命中率加成
    /// </summary>
    [FieldExplan("命中率加成", "HitRate")] public int HitRate;
    /// <summary>
    /// 视野加成*
    /// </summary>
    [FieldExplan("视野加成*", "ViewMul")] public int ViewMul;
    /// <summary>
    /// 技能冷却时间
    /// </summary>
    [FieldExplan("技能冷却时间", "CoolingTime")]public int CoolingTime;
    /// <summary>
    /// 技能耗魔量(只有主动释放 组合技能可以设置该属性)
    /// </summary>
    [FieldExplan("技能耗魔量", "MustUsedBaseMana")] public int MustUsedBaseMana;
}
