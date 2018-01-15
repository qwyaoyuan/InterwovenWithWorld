using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 道具类型 (具体类型先不要用,等编辑好了用)
/// </summary>
public enum EnumGoodsType
{
    #region 素材类
    /// <summary>
    /// 素材类的起始
    /// </summary>
    [FieldExplan("素材")]
    SourceMaterial = 1000000,

    #region 矿石大类
    /// <summary>
    /// 矿石大类
    /// </summary>
    [FieldExplan("矿石大类")]
    MineralBig = 1100000,

    #region 矿石
    /// <summary>
    /// 矿石
    /// </summary>
    [FieldExplan("矿石")]
    MineralLittle = 1110000,

    #region 具体的矿石
    #endregion

    #endregion

    #region 宝石
    /// <summary>
    /// 宝石
    /// </summary>
    [FieldExplan("宝石 ")]
    Gemston = 1111000,

    #region 具体的宝石

    #endregion

    #endregion

    #region 铸锭
    /// <summary>
    /// 铸锭
    /// </summary>
    [FieldExplan("铸锭")]
    IngotCasting = 1112000,

    #region 具体的铸锭
    #endregion

    #endregion

    #endregion

    #region 生物素材类
    /// <summary>
    /// 生物素材
    /// </summary>
    [FieldExplan("生物素材")]
    BiologyMaterial = 1200000,

    #region 兽皮
    /// <summary>
    /// 兽皮 
    /// </summary>
    [FieldExplan("兽皮")]
    Hide = 1210000,

    #region 具体的兽皮 

    #endregion

    #endregion

    #region 兽爪 
    /// <summary>
    /// 兽爪
    /// </summary>
    [FieldExplan("兽爪")]
    KemonoZume = 1211000,

    #region 具体的兽爪

    #endregion

    #endregion

    #region 兽骨 
    /// <summary>
    /// 兽骨
    /// </summary>
    [FieldExplan("兽骨")]
    AnimalBone = 1212000,

    #region 具体的兽骨 

    #endregion

    #endregion

    #region 兽肉
    /// <summary>
    /// 兽肉
    /// </summary>
    [FieldExplan("兽肉")]
    Meat = 1213000,

    #region  具体的兽肉
    #endregion

    #endregion

    #endregion

    #region 自然素材类
    /// <summary>
    /// 自然素材类 
    /// </summary>
    [FieldExplan("自然素材")]
    NaturalMaterial = 1300000,

    #region 草药
    /// <summary>
    /// 草药
    /// </summary>
    HerbalMedicine = 1310000,

    #region 具体的草药

    #endregion

    #endregion

    #region 木材
    /// <summary>
    /// 木材
    /// </summary>
    [FieldExplan("木材")]
    Wood = 1311000,

    #region 具体的木材

    #endregion

    #endregion

    #region 果实
    /// <summary>
    /// 果实
    /// </summary>
    [FieldExplan("果实")]
    Fruit = 1312000,

    #region 具体的果实

    #endregion

    #endregion

    #endregion

    #region 特殊素材类
    /// <summary>
    /// 特殊素材类
    /// </summary>
    [FieldExplan("特殊素材")]
    SpecialMaterial = 1400000,

    #region 掉落特殊素材类
    /// <summary>
    /// 掉落特殊素材
    /// </summary>
    [FieldExplan("掉落特殊素材")]
    DropSpecialMaterial = 1410000,

    #region 具体的掉落特殊素材

    #endregion

    #endregion

    #region 任务特殊素材类
    /// <summary>
    /// 任务特殊素材
    /// </summary>
    [FieldExplan("任务特殊素材")]
    TaskSpecialMaterial = 1411000,

    #region 具体的任务特殊素材

    #endregion

    #endregion

    #endregion

    #endregion

    #region 装备类
    /// <summary>
    /// 装备类
    /// </summary>
    [FieldExplan("装备")]
    Equipment = 2000000,

    #region 武器类
    /// <summary>
    /// 武器类
    /// </summary>
    [FieldExplan("武器")]
    Arms = 2100000,

    #region 单手剑
    /// <summary>
    /// 单手剑
    /// </summary>
    [FieldExplan("单手剑")]
    SingleHanedSword = 2110000,

    #region 具体的单手剑
    #endregion

    #endregion

    #region 双手剑 
    /// <summary>
    /// 双手剑
    /// </summary>
    [FieldExplan("双手剑")]
    TwoHandedSword = 2111000,

    #region 具体的双手剑

    #endregion

    #endregion

    #region 弓
    /// <summary>
    /// 弓
    /// </summary>
    [FieldExplan("弓")]
    Arch = 2112000,

    #region 具体的弓 

    #endregion

    #endregion

    #region 弩 
    /// <summary>
    /// 弩
    /// </summary>
    [FieldExplan("弩")]
    CrossBow = 2113000,

    #region 具体的弩

    #endregion

    #endregion

    #region 盾
    /// <summary>
    /// 盾
    /// </summary>
    [FieldExplan("具体的盾")]
    Shield = 2114000,

    #region 具体的盾

    #endregion

    #endregion

    #region 匕首
    /// <summary>
    /// 匕首
    /// </summary>
    [FieldExplan("匕首")]
    Dagger = 2115000,

    #region 具体的匕首

    #endregion

    #endregion

    #region 长杖
    /// <summary>
    /// 长杖
    /// </summary>
    [FieldExplan("长杖")]
    LongRod = 2116000,

    #region 具体的长杖

    #endregion

    #endregion

    #region 短杖
    /// <summary>
    /// 短杖
    /// </summary>
    [FieldExplan("短杖")]
    ShortRod = 2117000,

    #region 具体的短杖 

    #endregion

    #endregion

    #region 水晶球
    /// <summary>
    /// 水晶球 
    /// </summary>
    [FieldExplan("水晶球")]
    CrystalBall = 2118000,

    #region 具体的水晶球

    #endregion

    #endregion

    #endregion

    #region 头盔大类
    /// <summary>
    /// 头盔大类
    /// </summary>
    [FieldExplan("头盔大类")]
    HelmetBig = 2200000,

    #region 头盔
    /// <summary>
    /// 头盔
    /// </summary>
    [FieldExplan("头盔")]
    HelmetLittle = 2210000,

    #region 具体的头盔

    #endregion

    #endregion

    #region 头环
    /// <summary>
    /// 头环
    /// </summary>
    [FieldExplan("头环")]
    HeadRing = 2211000,

    #region 具体的头环
    #endregion

    #endregion

    #region 兜帽
    /// <summary>
    /// 兜帽
    /// </summary>
    [FieldExplan("兜帽")]
    Hood = 2212000,

    #region 具体的兜帽

    #endregion

    #endregion

    #endregion

    #region 盔甲类
    /// <summary>
    /// 盔甲类
    /// </summary>
    [FieldExplan("盔甲")]
    Armor = 2300000,

    #region 重甲 
    /// <summary>
    /// 重甲
    /// </summary>
    [FieldExplan("重甲")]
    HeavyArmor = 2310000,

    #region 具体的重甲

    #endregion

    #endregion

    #region 皮甲
    /// <summary>
    /// 皮甲
    /// </summary>
    [FieldExplan("皮甲")]
    LeatherArmor = 2311000,

    #region 具体的皮甲

    #endregion

    #endregion

    #region 法袍
    /// <summary>
    /// 法袍
    /// </summary>
    [FieldExplan("法袍")]
    Robe = 2312000,

    #region 具体的法袍

    #endregion

    #endregion

    #endregion

    #region 鞋子类
    /// <summary>
    /// 鞋子类
    /// </summary>
    [FieldExplan("鞋子")]
    Shoes = 2400000,

    #region 甲靴
    /// <summary>
    /// 甲靴
    /// </summary>
    [FieldExplan("甲靴")]
    ArmoredBoots = 2410000,

    #region 具体的甲靴

    #endregion

    #endregion

    #region 皮靴
    /// <summary>
    /// 皮靴
    /// </summary>
    [FieldExplan("皮靴")]
    Boots = 2411000,

    #region 具体的皮靴

    #endregion

    #endregion

    #region 布鞋
    /// <summary>
    /// 布鞋
    /// </summary>
    [FieldExplan("布鞋")]
    ClothShoes = 2412000,

    #region 具体的布鞋

    #endregion

    #endregion

    #endregion

    #region 饰品类
    /// <summary>
    /// 饰品类
    /// </summary>
    [FieldExplan("饰品")]
    Ornaments = 2500000,

    #region 项链
    /// <summary>
    /// 项链
    /// </summary>
    [FieldExplan("项链")]
    Necklace = 2510000,

    #region 具体的项链

    #endregion

    #endregion

    #region 戒指
    /// <summary>
    /// 戒指
    /// </summary>
    [FieldExplan("戒指")]
    Ring = 2511000,

    #region 具体的戒指
    #endregion

    #endregion

    #region 护身符
    /// <summary>
    /// 护身符
    /// </summary>
    [FieldExplan("护身符")]
    Amulet = 2512000,

    #region 具体的护身符

    #endregion

    #endregion

    #region 勋章
    /// <summary>
    /// 勋章
    /// </summary>
    [FieldExplan("勋章")]
    Medal = 2513000,

    #region 具体的勋章 

    #endregion

    #endregion

    #endregion

    #endregion

    #region 道具类
    /// <summary>
    /// 道具类
    /// </summary>
    [FieldExplan("道具")]
    Item = 3000000,

    #region 投掷道具类
    /// <summary>
    /// 投掷道具类
    /// </summary>
    [FieldExplan("投掷道具")]
    ThrowItem = 3100000,

    #region 炸弹类 
    /// <summary>
    /// 炸弹类
    /// </summary>
    [FieldExplan("炸弹")]
    Bomb = 3110000,

    #region 具体的炸弹

    #endregion

    #endregion

    #region 飞行道具类
    /// <summary>
    /// 飞行道具类
    /// </summary>
    [FieldExplan("飞行道具")]
    FlyItem = 3111000,

    #region 具体的飞行道具

    #endregion

    #endregion

    #endregion

    #region 魔偶类
    /// <summary>
    /// 魔偶类
    /// </summary>
    [FieldExplan("魔偶")]
    Golem = 3200000,

    #region 可制作魔偶
    /// <summary>
    /// 可制作魔偶
    /// </summary>
    [FieldExplan("可制作魔偶")]
    CanMakeGolem = 3210000,

    #region 具体的可制作魔偶

    #endregion

    #endregion

    #region 不可制作魔偶
    /// <summary>
    /// 不可制作魔偶
    /// </summary>
    [FieldExplan("不可制作魔偶")]
    CannotMakeGolem = 3211000,

    #region 具体的不可制作魔偶

    #endregion

    #endregion

    #endregion

    #region 陷阱类
    /// <summary>
    /// 陷阱类
    /// </summary>
    [FieldExplan("陷阱")]
    Trap = 3300000,

    #region 固定点陷阱
    /// <summary>
    /// 固定点陷阱
    /// </summary>
    [FieldExplan("固定点陷阱")]
    FixedTrap = 3310000,

    #region 具体的固定点陷阱 

    #endregion

    #endregion

    #region 特殊用法陷阱
    /// <summary>
    /// 特殊用法陷阱
    /// </summary>
    [FieldExplan("特殊用法陷阱")]
    SpecialTrap = 3311000,

    #region 具体的特殊用法陷阱

    #endregion

    #endregion

    #endregion

    #endregion

    #region 炼金药剂类
    /// <summary>
    /// 炼金药剂类
    /// </summary>
    [FieldExplan("炼金药剂")]
    Elixir = 4000000,

    #region 酒类
    /// <summary>
    /// 酒类
    /// </summary>
    [FieldExplan("酒")]
    Wine = 4100000,

    #region 普通酒
    /// <summary>
    /// 普通酒 
    /// </summary>
    [FieldExplan("普通酒")]
    NormalWine = 4110000,

    #region 具体的普通酒

    #endregion

    #endregion

    #endregion

    #region 恢复药剂大类
    /// <summary>
    /// 恢复药剂大类
    /// </summary>
    [FieldExplan("恢复药剂大类")]
    RestorativeDrugsBig = 4200000,

    #region 恢复药剂
    /// <summary>
    /// 恢复药剂
    /// </summary>
    [FieldExplan("恢复药剂")]
    RestorativeDrugsLittle = 4210000,

    #region 具体的回复药剂

    #endregion

    #endregion

    #endregion

    #region 强化药剂大类
    /// <summary>
    /// 强化药剂大类
    /// </summary>
    [FieldExplan("强化药剂大类")]
    StrengthenAgentBig = 4300000,

    #region 强化药剂 
    /// <summary>
    /// 强化药剂
    /// </summary>
    [FieldExplan("强化药剂")]
    StrengthenAgentLittle = 4310000,

    #region 具体的强化药剂

    #endregion

    #endregion

    #endregion

    #region 附魔药剂大类
    /// <summary>
    /// 附魔药剂大类
    /// </summary>
    [FieldExplan("附魔药剂大类")]
    EnchantElixirBig = 4400000,

    #region 附魔药剂 
    /// <summary>
    /// 附魔药剂
    /// </summary>
    [FieldExplan("附魔药剂")]
    EnchantElixirLittle = 4410000,

    #region 具体的附魔药剂

    #endregion

    #endregion

    #endregion

    #region 功能药剂大类
    /// <summary>
    /// 功能药剂大类
    /// </summary>
    [FieldExplan("功能药剂大类")]
    FunctionalAgnetBig = 4500000,

    #region 功能药剂
    /// <summary>
    /// 功能药剂
    /// </summary>
    [FieldExplan("功能药剂")]
    FunctionalAgnetLittle = 4510000,

    #region 具体的功能药剂

    #endregion

    #endregion

    #endregion

    #endregion

    #region 特殊物品类
    /// <summary>
    /// 特殊物品类
    /// </summary>
    [FieldExplan("特殊物品")]
    SpecialItem = 5000000,

    #region 书籍类
    /// <summary>
    /// 书籍类
    /// </summary>
    [FieldExplan("书籍")]
    Book = 5100000,

    #region 故事书 
    /// <summary>
    /// 故事书
    /// </summary>
    [FieldExplan("故事书")]
    StoryBook = 5110000,

    #region 具体的故事书

    #endregion

    #endregion

    #region 技能书
    /// <summary>
    /// 技能书
    /// </summary>
    [FieldExplan("技能书")]
    SkillBook = 5111000,

    #region 具体的技能书

    #endregion

    #endregion

    #region 信笺
    /// <summary>
    /// 信笺
    /// </summary>
    [FieldExplan("信笺")]
    Letterhead = 5112000,

    #region 具体的信笺

    #endregion

    #endregion

    #endregion

    #endregion


    #region 具体的类型(都放外面了)
    /// <summary>
    ///铁矿石
    /// </summary>
    [FieldExplan("铁矿石")]
    TKS = 1110001,
    /// <summary>
    ///魔晶石矿
    /// </summary>
    [FieldExplan("魔晶石矿")]
    MJSK = 1110002,
    /// <summary>
    ///钢玉矿石
    /// </summary>
    [FieldExplan("钢玉矿石")]
    GYKS = 1110003,
    /// <summary>
    ///精金矿石
    /// </summary>
    [FieldExplan("精金矿石")]
    JJKS = 1110004,
    /// <summary>
    ///龙纹晶矿
    /// </summary>
    [FieldExplan("龙纹晶矿")]
    LWJK = 1110005,
    /// <summary>
    ///秘银矿石
    /// </summary>
    [FieldExplan("秘银矿石")]
    MYKS = 1110006,
    /// <summary>
    ///红宝石
    /// </summary>
    [FieldExplan("红宝石")]
    HBS = 1111001,
    /// <summary>
    ///蓝宝石
    /// </summary>
    [FieldExplan("蓝宝石")]
    LBS = 1111002,
    /// <summary>
    ///黄宝石
    /// </summary>
    [FieldExplan("黄宝石")]
    HBS1 = 1111003,
    /// <summary>
    ///绿宝石
    /// </summary>
    [FieldExplan("绿宝石")]
    LBS1 = 1111004,
    /// <summary>
    ///星光水晶
    /// </summary>
    [FieldExplan("星光水晶")]
    XGSJ = 1111005,
    /// <summary>
    ///钢锭
    /// </summary>
    [FieldExplan("钢锭")]
    GD = 1112001,
    /// <summary>
    ///钛金铸锭
    /// </summary>
    [FieldExplan("钛金铸锭")]
    XJZD = 1112002,
    /// <summary>
    ///钢玉锭
    /// </summary>
    [FieldExplan("钢玉锭")]
    GYD = 1112003,
    /// <summary>
    ///精金锭
    /// </summary>
    [FieldExplan("精金锭")]
    JJD = 1112004,
    /// <summary>
    ///龙纹锭
    /// </summary>
    [FieldExplan("龙纹锭")]
    LWD = 1112005,
    /// <summary>
    ///秘银锭
    /// </summary>
    [FieldExplan("秘银锭")]
    MYD = 1112006,
    /// <summary>
    ///柔软毛皮
    /// </summary>
    [FieldExplan("柔软毛皮")]
    RRMP = 1210001,
    /// <summary>
    ///坚韧毛皮
    /// </summary>
    [FieldExplan("坚韧毛皮")]
    JRMP = 1210002,
    /// <summary>
    ///竹木
    /// </summary>
    [FieldExplan("竹木")]
    ZM = 1311001,
    /// <summary>
    ///杉木
    /// </summary>
    [FieldExplan("杉木")]
    SM = 1311002,
    /// <summary>
    ///橡木
    /// </summary>
    [FieldExplan("橡木")]
    XM = 1311003,
    /// <summary>
    ///桦木
    /// </summary>
    [FieldExplan("桦木")]
    XM1 = 1311004,
    /// <summary>
    ///檀木
    /// </summary>
    [FieldExplan("檀木")]
    TM = 1311005,
    /// <summary>
    ///铁剑
    /// </summary>
    [FieldExplan("铁剑")]
    TJ = 2110001,
    /// <summary>
    ///硬钢剑
    /// </summary>
    [FieldExplan("硬钢剑")]
    YGJ = 2110002,
    /// <summary>
    ///钛金剑
    /// </summary>
    [FieldExplan("钛金剑")]
    XJJ = 2110003,
    /// <summary>
    ///钢玉打刀
    /// </summary>
    [FieldExplan("钢玉打刀")]
    GYDD = 2110004,
    /// <summary>
    ///精金长剑
    /// </summary>
    [FieldExplan("精金长剑")]
    JJCJ = 2110005,
    /// <summary>
    ///龙纹长剑
    /// </summary>
    [FieldExplan("龙纹长剑")]
    LWCJ = 2110006,
    /// <summary>
    ///铁制大剑
    /// </summary>
    [FieldExplan("铁制大剑")]
    TZDJ = 2111001,
    /// <summary>
    ///硬钢大剑
    /// </summary>
    [FieldExplan("硬钢大剑")]
    YGDJ = 2111002,
    /// <summary>
    ///钛金大剑
    /// </summary>
    [FieldExplan("钛金大剑")]
    XJDJ = 2111003,
    /// <summary>
    ///钢玉太刀
    /// </summary>
    [FieldExplan("钢玉太刀")]
    GYTD = 2111004,
    /// <summary>
    ///精金大剑
    /// </summary>
    [FieldExplan("精金大剑")]
    JJDJ = 2111005,
    /// <summary>
    ///龙纹大剑
    /// </summary>
    [FieldExplan("龙纹大剑")]
    LWDJ = 2111006,
    /// <summary>
    ///反曲竹弓
    /// </summary>
    [FieldExplan("反曲竹弓")]
    FQZG = 2112001,
    /// <summary>
    ///王国长杉弓
    /// </summary>
    [FieldExplan("王国长杉弓")]
    WGCSG = 2112002,
    /// <summary>
    ///精钢弓
    /// </summary>
    [FieldExplan("精钢弓")]
    JGG = 2112003,
    /// <summary>
    ///钢玉弓
    /// </summary>
    [FieldExplan("钢玉弓")]
    GYG = 2112004,
    /// <summary>
    ///龙纹弓
    /// </summary>
    [FieldExplan("龙纹弓")]
    LWG = 2112005,
    /// <summary>
    ///橡木盾
    /// </summary>
    [FieldExplan("橡木盾")]
    XMD = 2114001,
    /// <summary>
    ///覆铜桦木盾
    /// </summary>
    [FieldExplan("覆铜桦木盾")]
    FTXMD = 2114002,
    /// <summary>
    ///铁皮桦木盾
    /// </summary>
    [FieldExplan("铁皮桦木盾")]
    TPXMD = 2114003,
    /// <summary>
    ///钢玉盾
    /// </summary>
    [FieldExplan("钢玉盾")]
    GYD1 = 2114004,
    /// <summary>
    ///龙纹精金盾
    /// </summary>
    [FieldExplan("龙纹精金盾")]
    LWJJD = 2114005,
    /// <summary>
    ///龙纹秘银盾
    /// </summary>
    [FieldExplan("龙纹秘银盾")]
    LWMYD = 2114006,
    /// <summary>
    ///短钢剑
    /// </summary>
    [FieldExplan("短钢剑")]
    DGJ = 2115001,
    /// <summary>
    ///钛金短剑
    /// </summary>
    [FieldExplan("钛金短剑")]
    XJDJ1 = 2115002,
    /// <summary>
    ///精金短剑
    /// </summary>
    [FieldExplan("精金短剑")]
    JJDJ1 = 2115003,
    /// <summary>
    ///龙纹短剑
    /// </summary>
    [FieldExplan("龙纹短剑")]
    LWDJ1 = 2115004,
    /// <summary>
    ///衬钢锁子甲
    /// </summary>
    [FieldExplan("衬钢锁子甲")]
    CGSZJ = 2311001,
    /// <summary>
    ///秘银锁子甲
    /// </summary>
    [FieldExplan("秘银锁子甲")]
    MYSZJ = 2311002,
    /// <summary>
    ///占星法袍
    /// </summary>
    [FieldExplan("占星法袍")]
    ZXFP = 2312001,
    /// <summary>
    ///宫廷法袍
    /// </summary>
    [FieldExplan("宫廷法袍")]
    GTFP = 2312002,
    /// <summary>
    ///大地魔偶
    /// </summary>
    [FieldExplan("大地魔偶")]
    DDMO = 3210002,
    /// <summary>
    ///烈焰魔偶
    /// </summary>
    [FieldExplan("烈焰魔偶")]
    LYMO = 3210003,
    /// <summary>
    ///土魔偶
    /// </summary>
    [FieldExplan("土魔偶")]
    TMO = 3211001,
    /// <summary>
    ///藤蔓陷阱
    /// </summary>
    [FieldExplan("藤蔓陷阱")]
    TMXX = 3310001,
    /// <summary>
    ///个性者药剂
    /// </summary>
    [FieldExplan("个性者药剂")]
    GXZYJ = 4210001,
    /// <summary>
    ///上古恶魔之书
    /// </summary>
    [FieldExplan("上古恶魔之书")]
    SGEMZS = 5113001,
    #endregion
}

/// <summary>
/// 道具辅助类
/// </summary>
public static class GoodsStaticTools
{
    /// <summary>
    /// 道具根节点
    /// </summary>
    static GoodsNode root;
    /// <summary>
    /// 道具类型树节点字典(方便查询)
    /// </summary>
    static Dictionary<EnumGoodsType, GoodsNode> goodsNodeDic;

    static GoodsStaticTools()
    {
        InitGoodsNode();
    }

    /// <summary>
    /// 判断是否是双手武器
    /// </summary>
    /// <param name="enumGoodsType"></param>
    /// <returns></returns>
    public static bool IsTwoHandedWeapon(EnumGoodsType enumGoodsType)
    {
        return GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.TwoHandedSword) ||//双手剑
                        GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.Arch) ||//弓
                        GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.CrossBow) ||//弩
                        GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.LongRod);//长杖
    }

    /// <summary>
    /// 判断是否是主手武器 
    /// </summary>
    /// <param name="enumGoodsType"></param>
    /// <returns></returns>
    public static bool IsRightOneHandedWeapon(EnumGoodsType enumGoodsType)
    {
        return GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.SingleHanedSword) ||//单手剑 
            GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.Dagger) ||//匕首
             GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.ShortRod);//短杖
    }

    /// <summary>
    /// 判断是否是副手武器
    /// </summary>
    /// <param name="enumGoodsType"></param>
    /// <returns></returns>
    public static bool IsLeftOneHandedWeapon(EnumGoodsType enumGoodsType)
    {
        return GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.Shield) ||//盾牌
                     GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.CrystalBall);//水晶球 
    }

    /// <summary>
    /// 初始化节点
    /// </summary>
    static void InitGoodsNode()
    {
        root = new GoodsNode();
        goodsNodeDic = new Dictionary<EnumGoodsType, GoodsNode>();
        int layer1 = 1000000;
        int layer2 = 100000;
        int layer3 = 1000;
        var tempDataStruct = Enum.GetValues(typeof(EnumGoodsType)).OfType<EnumGoodsType>()
            .Select(temp => new { type = temp, value = (int)temp })
            .OrderBy(temp => temp.value)
            .ToArray();
        root.Childs = new List<GoodsNode>();
        for (int i = 1; i < 10; i++)//第一层
        {
            #region 第一层
            int layer1Min = layer1 * i;
            int layer1Max = layer1 * (i + 1);
            var layer1TempDataStruct = tempDataStruct.Where(temp => temp.value >= layer1Min && temp.value < layer1Max)
                .Select(temp => new { type = temp.type, value = temp.value % layer1, baseValue = temp.value })
                .OrderBy(temp => temp.value).ToArray();
            if (layer1TempDataStruct.Length == 0)
                continue;
            GoodsNode layer1TreeNode = new GoodsNode();
            layer1TreeNode.Childs = new List<GoodsNode>();
            layer1TreeNode.GoodsType = layer1TempDataStruct[0].type;
            goodsNodeDic.Add(layer1TreeNode.GoodsType, layer1TreeNode);
            root.Childs.Add(layer1TreeNode);
            layer1TreeNode.Parent = root;
            #endregion
            for (int j = 1; j < 10; j++)//第二层
            {
                #region 第二层
                int layer2Min = layer2 * j;
                int layer2Max = layer2 * (j + 1);
                var layer2TempDataStruct = layer1TempDataStruct.Where(temp => temp.value >= layer2Min && temp.value < layer2Max)
                    .Select(temp => new { type = temp.type, value = temp.value % layer2, baseValue = temp.baseValue })
                    .OrderBy(temp=>temp.value).ToArray();
                if (layer2TempDataStruct.Length == 0)
                    continue;
                GoodsNode layer2TreeNode = new GoodsNode();
                layer2TreeNode.Childs = new List<GoodsNode>();
                layer2TreeNode.GoodsType = layer2TempDataStruct[0].type;
                goodsNodeDic.Add(layer2TreeNode.GoodsType, layer2TreeNode);
                layer1TreeNode.Childs.Add(layer2TreeNode);
                layer2TreeNode.Parent = layer1TreeNode;
                #endregion
                for (int k = 1; k < 100; k++)//第三层
                {
                    #region 第三层
                    int layer3Min = layer3 * k;
                    int layer3Max = layer3 * (k + 1);
                    var layer3TempDataStruct = layer2TempDataStruct.Where(temp => temp.value >= layer3Min && temp.value < layer3Max)
                        .Select(temp => new { type = temp.type, value = temp.value % layer3, baseValue = temp.baseValue })
                        .OrderBy(temp => temp.value).ToArray();
                    if (layer3TempDataStruct.Length == 0)
                        continue;
                    GoodsNode layer3TreeNode = new GoodsNode();
                    layer3TreeNode.Childs = new List<GoodsNode>();
                    layer3TreeNode.GoodsType = layer3TempDataStruct[0].type;
                    goodsNodeDic.Add(layer3TreeNode.GoodsType, layer3TreeNode);
                    layer2TreeNode.Childs.Add(layer3TreeNode);
                    layer3TreeNode.Parent = layer2TreeNode;
                    #endregion
                    for (int l = 1; l < layer3TempDataStruct.Length; l++)//第四层 
                    {
                        #region 第四层
                        GoodsNode layer4TreeNode = new GoodsNode();
                        layer4TreeNode.Childs = new List<GoodsNode>();
                        layer4TreeNode.GoodsType = layer3TempDataStruct[l].type;
                        goodsNodeDic.Add(layer4TreeNode.GoodsType, layer4TreeNode);
                        layer3TreeNode.Childs.Add(layer4TreeNode);
                        layer4TreeNode.Parent = layer3TreeNode;
                        #endregion
                    }
                }
            }
        }
    }

    /// <summary>
    /// 获取指定物品的大类
    /// </summary>
    /// <param name="child">子节点</param>
    /// <param name="deep">检索深度,默认为1(即上下层关系 )</param>
    /// <returns></returns>
    public static EnumGoodsType? GetParentGoodsType(EnumGoodsType child, int deep = 1)
    {
        GoodsNode childNode = GetGoodNode(child);
        if (childNode == null)
            return null;
        GoodsNode parentNode = childNode;
        while (deep > 0 && parentNode != null)
        {
            deep--;
            parentNode = parentNode.Parent;
        }
        if (parentNode != null)
            return parentNode.GoodsType;
        return null;
    }

    /// <summary>
    /// 判断给定的父子关系是否存在
    /// </summary>
    /// <param name="child">子节点</param>
    /// <param name="parent">父节点</param>
    /// <param name="ignoreDeep">是否忽略深度,如果忽略则会忽略两者之间的节点</param>
    /// <returns></returns>
    public static bool IsChildGoodsNode(EnumGoodsType child, EnumGoodsType parent, bool ignoreDeep = true)
    {
        if ((int)child < (int)parent)
            return false;
        if ((int)child - (int)parent > 1000000)
            return false;
        GoodsNode childNode = GetGoodNode(child);
        GoodsNode parentNode = GetGoodNode(parent);
        if (childNode == null || parentNode == null)
            return false;
        if (!ignoreDeep)
        {
            return Equals(childNode.Parent, parentNode);
        }
        else
        {
            GoodsNode _parentNode = childNode;
            while (_parentNode != null)
            {
                if (Equals(_parentNode, parentNode))
                {
                    return true;
                }
                _parentNode = _parentNode.Parent;
            }
            return false;
        }
    }

    /// <summary>
    /// 通过类型获取节点对象
    /// </summary>
    /// <param name="targetType"></param>
    /// <returns></returns>
    private static GoodsNode GetGoodNode(EnumGoodsType targetType)
    {
        GoodsNode goodsNode = null;
        goodsNodeDic.TryGetValue(targetType, out goodsNode);
        return goodsNode;
    }

    /// <summary>
    /// 道具节点
    /// </summary>
    public class GoodsNode
    {
        public GoodsNode Parent;

        public List<GoodsNode> Childs;

        public EnumGoodsType GoodsType;
    }
}