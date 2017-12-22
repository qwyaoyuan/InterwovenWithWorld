using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/******************这是所有技能类的脚本,所有其他属性存在特殊数据的都会在这里*************/

/// <summary>
/// 所有技能字段名的常量
/// </summary>
public static class SkillStructConstString
{
    /// <summary>
    /// 解锁技能法术数组
    /// </summary>
    public const string DeblockingMagicCombin = "解锁法术组合";
    /// <summary>
    /// 免除咏唱
    /// </summary>
    public const string ExemptionFromChanting = "免除咏唱";
    /// <summary>
    /// 下一次同元素魔法获得全效果加成
    /// </summary>
    public const string NextSameElementMagicAddition = "下一次同元素魔法获得全效果加成";
    /// <summary>
    /// 元素法术特效产生几率提升
    /// </summary>
    public const string ElementMagicSpeciallyEffectRateUp = "元素法术特效产生几率提升";
    /// <summary>
    /// 魔法特效强度增加
    /// </summary>
    public const string ElementMagicSpeciallyEffectPowerUp = "魔法特效强度增加";
    /// <summary>
    /// 攻击不死族伤害
    /// </summary>
    public const string AttackUndeadHurt = "攻击不死族伤害";
    /// <summary>
    /// 对非不死族造成回复
    /// </summary>
    public const string RecoveryNotUndead = "对非不死族造成回复";
    /// <summary>
    /// 对不死族附加特效（混乱）
    /// </summary>
    public const string AttackUndeadMakeItMessy = "对不死族附加特效（混乱）";
    /// <summary>
    /// 根据环境转化为同耗魔的元素魔法（三阶以下）
    /// </summary>
    public const string EnvironmentTranslateElementMagic = "根据环境转化为同耗魔的元素魔法（三阶以下）";
    /// <summary>
    /// 对不死组附加加速
    /// </summary>
    public const string UndeadAccelerate = "对不死族附加加速";
    /// <summary>
    /// 解锁魔偶解析
    /// </summary>
    public const string DeblockingMagicIdolResolution = "解锁魔偶解析";
    /// <summary>
    /// 解锁特殊技能——魔偶操控（二阶）
    /// </summary>
    public const string DeblockingMagicIdolControl = "解锁特殊技能——魔偶操控（二阶）";
    /// <summary>
    /// 额外的魔法伤害穿透
    /// </summary>
    public const string MagicThrough = "额外的魔法伤害穿透";
    /// <summary>
    /// 计算最终法术耗魔量时获得减免
    /// </summary>
    public const string ReliefManaAmount = "计算最终法术耗魔量时获得减免";
    /// <summary>
    /// 开放神灵信仰栏位
    /// </summary>
    public const string DeblockingDivineBeliefField = "开放神灵信仰栏位";
    /// <summary>
    /// 解锁献祭功能
    /// </summary>
    public const string DeblockingSacrifice = "解锁献祭功能";
    /// <summary>
    /// 解锁祝福光环
    /// </summary>
    public const string DeblockingBlessingHalo = "解锁祝福光环";
    /// <summary>
    /// 解锁特殊技能——后方支援（二阶）
    /// </summary>
    public const string DeblockingRearSupport = "解锁特殊技能——后方支援（二阶）";
    /// <summary>
    /// 根据信仰强度差值获得额外魔法效果加成
    /// </summary>
    public const string BeliefDifferenceToMagicAdd = "根据信仰强度差值获得额外魔法效果加成";
    /// <summary>
    /// 异常状态抗性加成
    /// </summary>
    public const string AbnormalStateResistance = "异常状态抗性加成";
    /// <summary>
    /// 咏唱时移动速度加成
    /// </summary>
    public const string MoveSpeedAtChanting = "咏唱时移动速度加成";
    /// <summary>
    /// 元素伤害效果变为双倍（三阶以上）
    /// </summary>
    public const string ElementHurtEffectToDouble = "元素伤害效果变为双倍（三阶以上）";
    /// <summary>
    /// 本魔法咏唱时间百分比减免
    /// </summary>
    public const string MagicChantingTimeRelief = "本魔法咏唱时间百分比减免";
    /// <summary>
    /// 本魔法咏唱时间百分比增加
    /// </summary>
    public const string MagicChantingTimeIncrease = "本魔法咏唱时间百分比增加";
    /// <summary>
    /// 本魔法冷却时间百分比减免
    /// </summary>
    public const string MagicCoolingTimeRelief = "本魔法冷却时间百分比减免";
    /// <summary>
    /// 本魔法冷却时间百分比增加
    /// </summary>
    public const string MagicCoolingTimeIncrease = "本魔法冷却时间百分比增加";
    /// <summary>
    /// 解锁大魔法槽
    /// </summary>
    public const string DeblockingBigMagicField = "解锁大魔法槽";
    /// <summary>
    /// 法术陷阱获得伤害强化
    /// </summary>
    public const string MagicTrapHurtStrengthen = "法术陷阱获得伤害强化";
    /// <summary>
    /// 法术陷阱获得基础特效产生率
    /// </summary>
    public const string MagicTrapBaseSpeciallyEffectRate = "法术陷阱获得基础特效产生率";
    /// <summary>
    /// 解锁高阶魔偶（带有一定魔免）
    /// </summary>
    public const string DeblockingHighMagicIdol = "解锁高阶魔偶（带有一定魔免）";
    /// <summary>
    /// 解锁特殊技能——信仰赋予（二阶）
    /// </summary>
    public const string DeblockingBeliefGiven = "解锁特殊技能——信仰赋予（二阶）";
    /// <summary>
    /// 解锁特殊技能——魔力扰乱（二阶）
    /// </summary>
    public const string DeblockingMagicDisrupt = "解锁特殊技能——魔力扰乱（二阶）";
    /// <summary>
    /// 成功使立场失效时造成伤害
    /// </summary>
    public const string MagicDisruptHurt = "成功使立场失效时造成伤害";
    /// <summary>
    /// 解锁元素魔偶制作，对应元素魔免
    /// </summary>
    public const string DeblockingElementMagicIdol = "解锁元素魔偶制作，对应元素魔免";
    /// <summary>
    /// 同时操控的魔偶，知性生物数量提升到3个
    /// </summary>
    public const string ControlMagicIdolCount = "同时操控的魔偶，知性生物数量提升到3个";
    /// <summary>
    /// 召唤生物会根据生物信仰强度常驻随机BUFF光环
    /// </summary>
    public const string SummoningBiologicalBeliefRandomBuffHalo = "召唤生物会根据生物信仰强度常驻随机BUFF光环";
    /// <summary>
    /// 解锁特殊技能——精神集中（萨满）
    /// </summary>
    public const string DeblockingMentalConcentration = "解锁特殊技能——精神集中（萨满）";
    /// <summary>
    /// 解锁特殊技能——体能突破（萨满）
    /// </summary>
    public const string DeblockingPhysicalBreakthrough = "解锁特殊技能——体能突破（萨满）";
    /// <summary>
    /// 死亡时被动复活
    /// </summary>
    public const string ResurrectionAtTheTimeOfDeath = "死亡时被动复活";
    /// <summary>
    /// 解锁特殊技能——传送（萨满）
    /// </summary>
    public const string DeblockingTransfer = "解锁特殊技能——传送（萨满）";
    /// <summary>
    /// 神秘信仰强度提升
    /// </summary>
    public const string MysticalBeliefsPowerUp = "神秘信仰强度提升";
    /// <summary>
    /// 根据神秘信仰强度额外提升对应神秘信仰特效
    /// </summary>
    public const string MysticalBeliefsParticalEffectByPower = "根据神秘信仰强度额外提升对应神秘信仰特效";
    /// <summary>
    /// 形成对应神秘信仰的增强区域，效果随时间逐渐衰减
    /// </summary>
    public const string MysticalBeliefsEnhancedRegional = "形成对应神秘信仰的增强区域，效果随时间逐渐衰减";
    /// <summary>
    /// 信仰1加成
    /// </summary>
    public const string ImproveBeliefs1 = "信仰1加成";
    /// <summary>
    /// 根据神秘信仰差值决定召唤物强度，数量，类型
    /// </summary>
    public const string MysticalBeliefsDifferenceSummoningType = "根据神秘信仰差值决定召唤物强度，数量，类型";
    /// <summary>
    /// 力量值百分比提升
    /// </summary>
    public const string PowerUp = "力量值百分比提升";
    /// <summary>
    /// 精神值百分比提升
    /// </summary>
    public const string SpiritUp = "精神值百分比提升";
    /// <summary>
    /// 解锁神灵信仰的笃信层数功能，一定时间叠加一层
    /// </summary>
    public const string DeblockingDivineBelieve = "解锁神灵信仰的笃信层数功能，一定时间叠加一层";
    /// <summary>
    /// 有一定几率再对随机目标作用一次
    /// </summary>
    public const string RandomEffectsOnTheTarget = "有一定几率再对随机目标作用一次";
    /// <summary>
    /// 解锁敬畏光环
    /// </summary>
    public const string DeblockingAweHalo = "解锁敬畏光环";
    /// <summary>
    /// 崇拜信仰强度提升
    /// </summary>
    public const string WorshipBeliefPowerUp = "崇拜信仰强度提升";
    /// <summary>
    /// 根据神灵笃信层数为魔法附加随机效果
    /// </summary>
    public const string RandomEffectByDivineBelieve = "根据神灵笃信层数为魔法附加随机效果";
    /// <summary>
    /// 解锁认定圣物能力，每次重新认定时需要经过一定时间
    /// </summary>
    public const string SetTheSacredObject = "解锁认定圣物能力，每次重新认定时需要经过一定时间";
    /// <summary>
    /// 连续释放带有本字节的魔法提升威力（释放其他传承法术则层数重置）
    /// </summary>
    public const string WorshipBeliefInheritance = "连续释放带有本字节的魔法提升威力（释放其他传承法术则层数重置）";
    /// <summary>
    /// 解锁特殊技能——神依
    /// </summary>
    public const string DeblockingGodDepend = "解锁特殊技能——神依";
    /// <summary>
    /// 本技能获得基础暴击率
    /// </summary>
    public const string ThisSkillBaseCritRateUp = "本技能获得基础暴击率";
    /// <summary>
    /// 本技能获得基础暴击伤害加成
    /// </summary>
    public const string ThisSkillBaseCritHurtUp = "本技能获得基础暴击伤害加成";
    /// <summary>
    /// 根据最近一次主动释放的四阶元素魔法切换元素力场，提升对应元素威力
    /// </summary>
    public const string ElementStandPower = "根据最近一次主动释放的四阶元素魔法切换元素力场，提升对应元素威力";
}

/// <summary>
/// 入门法师
/// </summary>
public class SkillStruct_FS05 : SkillBaseStruct
{
    /// <summary>
    /// 解锁法术组合
    /// </summary>
    public int DeblockingMagicCombin;
}

/// <summary>
/// 进阶法师
/// </summary>
public class SkillStruct_FS07 : SkillBaseStruct
{
    /// <summary>
    /// 解锁法术组合
    /// </summary>
    public int DeblockingMagicCombin;
}

/// <summary>
/// 快速咏唱
/// </summary>
public class SkillStruct_FS09 : SkillBaseStruct
{
    /// <summary>
    /// 免除咏唱
    /// </summary>
    public int ExemptionFromChanting;
}

/// <summary>
/// 高阶法师
/// </summary>
public class SkillStruct_FS10 : SkillBaseStruct
{
    /// <summary>
    /// 解锁法术组合
    /// </summary>
    public int DeblockingMagicCombin;
}

/// <summary>
/// 元素驻留
/// </summary>
public class SkillStruct_YSX06 : SkillBaseStruct
{
    /// <summary>
    /// 下一次同元素魔法获得全效果加成
    /// </summary>
    public int NextSameElementMagicAddition;
}

/// <summary>
/// 元素低语
/// </summary>
public class SkillStruct_YSX08 : SkillBaseStruct
{
    /// <summary>
    /// 元素法术特效产生几率提示
    /// </summary>
    public int ElementMagicSpeciallyEffectRateUp;
}

/// <summary>
/// 光明信仰_净化
/// </summary>
public class SkillStruct_XYX05 : SkillBaseStruct
{
    /// <summary>
    /// 攻击不死族伤害
    /// </summary>
    public int AttackUndeadHurt;
    /// <summary>
    /// 对不死族附加特效（混乱）
    /// </summary>
    public int AttackUndeadMakeItMessy;
}

/// <summary>
/// 自然信仰_自然之力
/// </summary>
public class SkillStruct_XYX07 : SkillBaseStruct
{
    /// <summary>
    /// 根据环境转化为同耗魔的元素魔法（三阶以下）
    /// </summary>
    public int EnvironmentTranslateElementMagic;
}

/// <summary>
/// 黑暗信仰_凋零
/// </summary>
public class SkillStruct_XYX08 : SkillBaseStruct
{
    /// <summary>
    /// 对不死族附加加速
    /// </summary>
    public int UndeadAccelerate;
}

/// <summary>
/// 魔偶制成
/// </summary>
public class SkillStruct_MFS01 : SkillBaseStruct
{
    /// <summary>
    /// 解锁魔偶解析
    /// </summary>
    public int DeblockingMagicIdolResolution;
}

/// <summary>
/// 魔偶操控
/// </summary>
public class SkillStruct_MFS02 : SkillBaseStruct
{
    /// <summary>
    /// 解锁特殊技能——魔偶操控（二阶）
    /// </summary>
    public int DeblockingMagicIdolControl;
}

/// <summary>
/// 魔法精通
/// </summary>
public class SkillStruct_MFS03 : SkillBaseStruct
{
    /// <summary>
    /// 额外的魔法伤害穿透
    /// </summary>
    public int MagicThrough;
}

/// <summary>
/// 精灵交谈
/// </summary>
public class SkillStruct_MFS07 : SkillBaseStruct
{
    /// <summary>
    /// 元素法术特效产生几率提示
    /// </summary>
    public int ElementMagicSpeciallyEffectRateUp;
}

/// <summary>
/// 双重法术
/// </summary>
public class SkillStruct_MFS08 : SkillBaseStruct
{
    /// <summary>
    /// 计算最终法术耗魔量时获得减免
    /// </summary>
    public int ReliefManaAmount;
}

/// <summary>
/// 自然聆听
/// </summary>
public class SkillStruct_SM08 : SkillBaseStruct
{
    /// <summary>
    /// 魔法特效强度增加
    /// </summary>
    public int ElementMagicSpeciallyEffectPowerUp;
}

/// <summary>
/// 神灵信奉
/// </summary>
public class SkillStruct_MS01 : SkillBaseStruct
{
    /// <summary>
    /// 开放神灵信仰栏位
    /// </summary>
    public int DeblockingDivineBeliefField;
    /// <summary>
    /// 解锁献祭功能
    /// </summary>
    public int DeblockingSacrifice;
}

/// <summary>
/// 祝福光环
/// </summary>
public class SkillStruct_MS04 : SkillBaseStruct
{
    /// <summary>
    /// 解锁祝福光环
    /// </summary>
    public int DeblockingBlessingHalo;
}

/// <summary>
/// 后方支援
/// </summary>
public class SkillStruct_MS05 : SkillBaseStruct
{
    /// <summary>
    /// 解锁特殊技能——后方支援（二阶）
    /// </summary>
    public int DeblockingRearSupport;
}

/// <summary>
/// 信仰冲击
/// </summary>
public class SkillStruct_MS08 : SkillBaseStruct
{
    /// <summary>
    /// 根据信仰强度差值获得额外魔法效果加成
    /// </summary>
    public int BeliefDifferenceToMagicAdd;
}

/// <summary>
/// 大法师
/// </summary>
public class SkillStruct_DFS01 : SkillBaseStruct
{
    /// <summary>
    /// 异常状态抗性加成
    /// </summary>
    public int AbnormalStateResistance;
    /// <summary>
    /// 咏唱时移动速度加成
    /// </summary>
    public int MoveSpeedAtChanting;
}

/// <summary>
/// 附加元素
/// </summary>
public class SkillStruct_DFS03 : SkillBaseStruct
{
    /// <summary>
    /// 元素伤害效果变为双倍（三阶以上）
    /// </summary>
    public int ElementHurtEffectToDouble;
}

/// <summary>
/// 神速咏唱
/// </summary>
public class SkillStruct_DFS04 : SkillBaseStruct
{
    /// <summary>
    /// 本魔法咏唱时间百分比减免
    /// </summary>
    public int MagicChantingTimeRelief;
    /// <summary>
    /// 本魔法冷却时间百分比减免
    /// </summary>
    public int MagicCoolingTimeRelief;
}

/// <summary>
/// 大魔法
/// </summary>
public class SkillStruct_DFS09 : SkillBaseStruct
{
    /// <summary>
    /// 解锁大魔法槽
    /// </summary>
    public int DeblockingBigMagicField;
}

/// <summary>
/// 高阶法术陷阱
/// </summary>
public class SkillStruct_ZHS01 : SkillBaseStruct
{
    /// <summary>
    /// 法术陷阱获得伤害强化
    /// </summary>
    public int MagicTrapHurtStrengthen;
    /// <summary>
    /// 法术陷阱获得基础特效产生率
    /// </summary>
    public int MagicTrapBaseSpeciallyEffectRate;
}

/// <summary>
/// 高阶魔偶
/// </summary>
public class SkillStruct_ZHS03 : SkillBaseStruct
{
    /// <summary>
    /// 解锁高阶魔偶（带有一定魔免）
    /// </summary>
    public int DeblockingHighMagicIdol;
}

/// <summary>
/// 信仰赋予
/// </summary>
public class SkillStruct_ZHS04 : SkillBaseStruct
{
    /// <summary>
    /// 解锁特殊技能——信仰赋予（二阶）
    /// </summary>
    public int DeblockingBeliefGiven;
}

/// <summary>
/// 魔力扰乱
/// </summary>
public class SkillStruct_ZHS05 : SkillBaseStruct
{
    /// <summary>
    /// 解锁特殊技能——魔力扰乱（二阶）
    /// </summary>
    public int DeblockingMagicDisrupt;
    /// <summary>
    /// 成功使立场失效时造成伤害 
    /// </summary>
    public int MagicDisruptHurt;
}

/// <summary>
/// 魔偶指令
/// </summary>
public class SkillStruct_ZHS06 : SkillBaseStruct
{
    /// <summary>
    /// 解锁元素魔偶制作，对应元素魔免
    /// </summary>
    public int DeblockingElementMagicIdol;
}

/// <summary>
/// 操控者
/// </summary>
public class SkillStruct_ZHS07 : SkillBaseStruct
{
    /// <summary>
    /// 同时操控的魔偶，知性生物数量提升到3个
    /// </summary>
    public int ControlMagicIdolCount;
}

/// <summary>
/// 生物信仰_呼唤
/// </summary>
public class SkillStruct_ZHS08 : SkillBaseStruct
{
    /// <summary>
    /// 召唤生物会根据生物信仰强度常驻随机BUFF光环
    /// </summary>
    public int SummoningBiologicalBeliefRandomBuffHalo;
}

/// <summary>
/// 光明元素
/// </summary>
public class SkillStruct_DSM03 : SkillBaseStruct
{
    /// <summary>
    /// 攻击不死族伤害
    /// </summary>
    public int AttackUndeadHurt;
}

/// <summary>
/// 精神集中
/// </summary>
public class SkillStruct_DSM05 : SkillBaseStruct
{
    /// <summary>
    /// 解锁特殊技能——精神集中（萨满）
    /// </summary>
    public int DeblockingMentalConcentration;
}

/// <summary>
/// 体能突破
/// </summary>
public class SkillStruct_DSM06 : SkillBaseStruct
{
    /// <summary>
    /// 解锁特殊技能——体能突破（萨满）
    /// </summary>
    public int DeblockingPhysicalBreakthrough;
}

/// <summary>
/// 萨满之矛
/// </summary>
public class SkillStruct_DSM07 : SkillBaseStruct
{
    /// <summary>
    /// 死亡时被动复活
    /// </summary>
    public int ResurrectionAtTheTimeOfDeath;
}

/// <summary>
/// 传送
/// </summary>
public class SkillStruct_DSM08 : SkillBaseStruct
{
    /// <summary>
    /// 解锁特殊技能——传送（萨满）
    /// </summary>
    public int DeblockingTransfer;
}

/// <summary>
/// 光明信仰_圣光
/// </summary>
public class SkillStruct_JS03 : SkillBaseStruct
{
    /// <summary>
    /// 攻击不死族伤害
    /// </summary>
    public int AttackUndeadHurt;
    /// <summary>
    /// 对非不死族造成回复
    /// </summary>
    public int RecoveryNotUndead;
}

/// <summary>
/// 神佑光环
/// </summary>
public class SkillStruct_JS05 : SkillBaseStruct
{
    /// <summary>
    /// 神秘信仰强度提升
    /// </summary>
    public int MysticalBeliefsPowerUp;
}

/// <summary>
/// 神秘信仰
/// </summary>
public class SkillStruct_JS06 : SkillBaseStruct
{
    /// <summary>
    /// 根据神秘信仰强度额外提升对应神秘信仰特效
    /// </summary>
    public int MysticalBeliefsParticalEffectByPower;
}

/// <summary>
/// 神秘信仰_特化
/// </summary>
public class SkillStruct_JS07 : SkillBaseStruct
{
    /// <summary>
    /// 信仰1加成
    /// </summary>
    public int ImproveBeliefs1;
}

/// <summary>
/// 信仰感召
/// </summary>
public class SkillStruct_JS08 : SkillBaseStruct
{
    /// <summary>
    /// 形成对应神秘信仰的增强区域，效果随时间逐渐衰减
    /// </summary>
    public int MysticalBeliefsEnhancedRegional;
    /// <summary>
    /// 本魔法咏唱时间百分比增加
    /// </summary>
    public int MagicChantingTimeIncrease;
    /// <summary>
    /// 本魔法冷却时间百分比增加
    /// </summary>
    public int MagicCoolingTimeIncrease;
}

/// <summary>
/// 神秘信仰_降临 
/// </summary>
public class SkillStruct_JS09 : SkillBaseStruct
{
    /// <summary>
    /// 根据神秘信仰差值决定召唤物强度，数量，类型
    /// </summary>
    public int MysticalBeliefsDifferenceSummoningType;
}

/// <summary>
/// 高阶生物信仰
/// </summary>
public class SkillStruct_JH01 : SkillBaseStruct
{
    /// <summary>
    /// 力量值百分比提升
    /// </summary>
    public int PowerUp;
}

/// <summary>
/// 高阶自然信仰
/// </summary>
public class SkillStruct_JH02 : SkillBaseStruct
{
    /// <summary>
    /// 精神值百分比提升
    /// </summary>
    public int SpiritUp;
}

/// <summary>
/// 神灵笃信
/// </summary>
public class SkillStruct_JH03 : SkillBaseStruct
{
    /// <summary>
    /// 解锁神灵信仰的笃信层数功能，一定时间叠加一层
    /// </summary>
    public int DeblockingDivineBelieve;
}

/// <summary>
/// 崇拜信仰_信仰之翼
/// </summary>
public class SkillStruct_JH04 : SkillBaseStruct
{
    /// <summary>
    /// 有一定几率再对随机目标作用一次
    /// </summary>
    public int RandomEffectsOnTheTarget;
}

/// <summary>
/// 敬畏光环
/// </summary>
public class SkillStruct_JH05 : SkillBaseStruct
{
    /// <summary>
    /// 解锁敬畏光环
    /// </summary>
    public int DeblockingAweHalo;
    /// <summary>
    /// 崇拜信仰强度提升
    /// </summary>
    public int WorshipBeliefPowerUp;
}

/// <summary>
/// 神灵信仰
/// </summary>
public class SkillStruct_JH06 : SkillBaseStruct
{
    /// <summary>
    /// 根据神灵笃信层数为魔法附加随机效果
    /// </summary>
    public int RandomEffectByDivineBelieve;
}

/// <summary>
/// 圣物
/// </summary>
public class SkillStruct_JH07 : SkillBaseStruct
{
    /// <summary>
    /// 解锁认定圣物能力，每次重新认定时需要经过一定时间
    /// </summary>
    public int SetTheSacredObject;
}

/// <summary>
/// 崇拜信仰_传承
/// </summary>
public class SkillStruct_JH08 : SkillBaseStruct
{
    /// <summary>
    /// 连续释放带有本字节的魔法提升威力（释放其他传承法术则层数重置）
    /// </summary>
    public int WorshipBeliefInheritance;
}

/// <summary>
/// 神依
/// </summary>
public class SkillStruct_JH09 : SkillBaseStruct
{
    /// <summary>
    /// 解锁特殊技能——神依
    /// </summary>
    public int DeblockingGodDepend;
}

/// <summary>
/// 元素爆破
/// </summary>
public class SkillStruct_DFS06 : SkillBaseStruct
{
    /// <summary>
    /// 本技能获得基础暴击率
    /// </summary>
    public int ThisSkillBaseCritRateUp;
    /// <summary>
    /// 本技能获得基础暴击伤害加成
    /// </summary>
    public int ThisSkillBaseCritHurtUp;
}

/// <summary>
/// 元素立场
/// </summary>
public class SkillStruct_DFS07 : SkillBaseStruct
{
    /// <summary>
    /// 根据最近一次主动释放的四阶元素魔法切换元素力场，提升对应元素威力
    /// </summary>
    public int ElementStandPower;
}

/// <summary>
/// 高速咏唱
/// </summary>
public class SkillStruct_DFS08 : SkillBaseStruct
{
    /// <summary>
    /// 免除咏唱
    /// </summary>
    public int ExemptionFromChanting;
}