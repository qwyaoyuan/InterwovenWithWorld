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
    /// 根据环境转化为同耗魔的元素魔法（三阶以下）
    /// </summary>
    public const string EnvironmentTranslateElementMagic = "根据环境转化为同耗魔的元素魔法（三阶以下）";
    /// <summary>
    /// 解锁魔偶解析
    /// </summary>
    public const string DeblockingMagicIdolResolution = "解锁魔偶解析";
    /// <summary>
    /// 解锁特殊技能——魔偶操控（二阶）
    /// </summary>
    public const string DeblockingMagicIdolControl = "解锁特殊技能——魔偶操控（二阶）";
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
    /// 解锁大魔法槽
    /// </summary>
    public const string DeblockingBigMagicField = "解锁大魔法槽";
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
    /// 形成对应神秘信仰的增强区域，效果随时间逐渐衰减
    /// </summary>
    public const string MysticalBeliefsEnhancedRegional = "形成对应神秘信仰的增强区域，效果随时间逐渐衰减";
    /// <summary>
    /// 根据神秘信仰差值决定召唤物强度，数量，类型
    /// </summary>
    public const string MysticalBeliefsDifferenceSummoningType = "根据神秘信仰差值决定召唤物强度，数量，类型";
    /// <summary>
    /// 解锁神灵信仰的笃信层数功能，一定时间叠加一层
    /// </summary>
    public const string DeblockingDivineBelieve = "解锁神灵信仰的笃信层数功能，一定时间叠加一层";
    /// <summary>
    /// 有一定几率再对随机目标作用一次
    /// </summary>
    public const string RandomEffectsOnTheTarget = "有一定几率再对随机目标作用一次";
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
    /// 元素立场
    /// 元素立场消失后失效
    /// </summary>
    public const string ElementStandPower = "元素立场";

    /// <summary>
    /// 使用盾牌释放重击造成眩晕
    /// </summary>
    public const string SwipeDizzy = "重击眩晕";

    /// <summary>
    /// 使用巨剑释放重击增加伤害
    /// </summary>
    public const string SwipePhyAdd = "重击增伤";

    /// <summary>
    /// 白刃技能-进入战斗状态后60秒内获得25%的物理伤害穿透
    /// </summary>
    public const string Knives = "白刃";

    /// <summary>
    /// 解锁近战普攻
    /// </summary>
    public const string DeblockingPhyMeleeAttack = "解锁近战普攻";

    /// <summary>
    /// 游击
    /// </summary>
    public const string Guerrilla = "游击";

    /// <summary>
    /// 解锁远程普攻
    /// </summary>
    public const string DeblockingPhyRemoteAttack = "解锁远程普攻";

    /// <summary>
    /// 坚韧狂暴
    /// </summary>
    public const string ToughRage = "坚韧狂暴";

    /// <summary>
    /// 战吼
    /// </summary>
    public const string BattleRoar = "战吼";

    /// <summary>
    /// 剑意
    /// </summary>
    public const string SwordMeaning = "剑意";

    /// <summary>
    /// 燕返 
    /// </summary>
    public const string YanReturn = "燕返";

    /// <summary>
    /// 后撤步
    /// </summary>
    public const string RetreatStep = "后撤步";

    /// <summary>
    /// 巧手夺宝
    /// </summary>
    public const string GrabYreasureSkillful = "巧手夺宝";

    /// <summary>
    /// 巧手
    /// </summary>
    public const string Hands = "巧手";

    /// <summary>
    /// 暗杀术
    /// </summary>
    public const string Assassination = "暗杀术";
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
/// 信仰感召
/// </summary>
public class SkillStruct_JS08 : SkillBaseStruct
{
    /// <summary>
    /// 形成对应神秘信仰的增强区域，效果随时间逐渐衰减
    /// </summary>
    public int MysticalBeliefsEnhancedRegional;
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
/// 元素立场
/// </summary>
public class SkillStruct_DFS07 : SkillBaseStruct
{
    /// <summary>
    /// 元素立场
    /// 元素立场消失后失效
    /// </summary>
    public int ElementStandPower;
}

//--------------------物理技能---------------------//

/// <summary>
/// 重击
/// </summary>
public class SkillStruct_WL01 : SkillBaseStruct
{
    /// <summary>
    /// 使用盾牌造成眩晕
    /// </summary>
    public int SwipeDizzy;
    /// <summary>
    /// 使用巨剑时增加伤害
    /// </summary>
    public int SwipePhyAdd;
}

/// <summary>
/// 白刃
/// </summary>
public class SkillStruct_ZS01 : SkillBaseStruct
{
    /// <summary>
    /// 白刃
    /// </summary>
    public int Knives;
}

/// <summary>
/// 近战强化
/// </summary>
public class SkillStruct_ZS04 : SkillBaseStruct
{
    /// <summary>
    /// 解锁近战普攻2阶段
    /// </summary>
    public int DeblockingPhyMeleeAttack;
}

/// <summary>
/// 游击
/// </summary>
public class SkillStruct_GJS01 : SkillBaseStruct
{
    /// <summary>
    /// 游击
    /// </summary>
    public int Guerrilla;
}

/// <summary>
/// 远程强化
/// </summary>
public class SkillStruct_GJS04 : SkillBaseStruct
{
    /// <summary>
    /// 解锁远程普攻2段
    /// </summary>
    public int DeblockingPhyRemoteAttack;
}

/// <summary>
/// 坚韧狂暴
/// </summary>
public class SkillStrut_KZS01 : SkillBaseStruct
{
    /// <summary>
    /// 坚韧狂暴
    /// </summary>
    public int ToughRage;
}

/// <summary>
/// 近战专精
/// </summary>
public class SkillStruct_KZS02 : SkillBaseStruct
{
    /// <summary>
    /// 解锁近战普攻3阶段
    /// </summary>
    public int DeblockingPhyMeleeAttack;
}

/// <summary>
/// 战吼
/// </summary>
public class SkillStruct_KZS03 : SkillBaseStruct
{
    /// <summary>
    /// 战吼
    /// </summary>
    public int BattleRoar;
}

/// <summary>
/// 剑意
/// </summary>
public class SkillStruct_JAS02 : SkillBaseStruct
{
    /// <summary>
    /// 剑意
    /// </summary>
    public int SwordMeaning;
}

/// <summary>
/// 燕返
/// </summary>
public class SkillStruct_JAS03 : SkillBaseStruct
{
    /// <summary>
    /// 燕返
    /// </summary>
    public int YanReturn;
}

/// <summary>
/// 后撤步
/// </summary>
public class SkillStruct_YX03 : SkillBaseStruct
{
    /// <summary>
    /// 后撤步
    /// </summary>
    public int RetreatStep;
}

/// <summary>
/// 巧手夺宝
/// </summary>
public class SkillStruct_DZ01 : SkillBaseStruct
{
    /// <summary>
    /// 巧手夺宝
    /// </summary>
    public int GrabYreasureSkillful;
}

/// <summary>
/// 巧手
/// </summary>
public class SkillStruct_DZ02 : SkillBaseStruct
{
    /// <summary>
    /// 巧手
    /// </summary>
    public int Hands;
}

/// <summary>
/// 暗杀术
/// </summary>
public class SkillStruct_DZ03 : SkillBaseStruct
{
    /// <summary>
    /// 暗杀术
    /// </summary>
    public int Assassination;
}

/// <summary>
/// 远程专精
/// </summary>
public class SkillStruct_SSS01 : SkillBaseStruct
{
    /// <summary>
    /// 解锁远程普攻三阶段
    /// </summary>
    public int DeblockingPhyRemoteAttack;
}