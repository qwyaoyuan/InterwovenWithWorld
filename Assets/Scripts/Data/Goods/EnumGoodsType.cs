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

    #region 特殊功能道具类
    /// <summary>
    /// 特殊功能道具类
    /// </summary>
    [FieldExplan("投掷道具")]
    SpecialActionItem = 3400000,

    #region 魔法卷轴类
    /// <summary>
    /// 魔法卷轴类
    /// </summary>
    [FieldExplan("魔法卷轴类")]
    MagicScroll = 3410000,

    #region 具体的魔法卷轴

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

    #region 我写的特殊物品
    /// <summary>
    /// 传讯魔法卷轴
    /// </summary>
    [FieldExplan("传讯魔法卷轴")]
    CXMFJZ = 3410001,
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
    ///小型兽爪
    /// </summary>
    [FieldExplan("小型兽爪")]
    XXSZ = 1211001,
    /// <summary>
    ///坚韧兽爪
    /// </summary>
    [FieldExplan("坚韧兽爪")]
    JRSZ = 1211002,
    /// <summary>
    ///小型兽骨
    /// </summary>
    [FieldExplan("小型兽骨")]
    XXSG = 1212001,
    /// <summary>
    ///坚硬兽骨
    /// </summary>
    [FieldExplan("坚硬兽骨")]
    JYSG = 1212002,
    /// <summary>
    ///疗伤草
    /// </summary>
    [FieldExplan("疗伤草")]
    LSC = 1310001,
    /// <summary>
    ///血红花
    /// </summary>
    [FieldExplan("血红花")]
    XHH = 1310002,
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
    ///酒浆果
    /// </summary>
    [FieldExplan("酒浆果")]
    JJG = 1312002,
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
    ///反曲猎弓
    /// </summary>
    [FieldExplan("反曲猎弓")]
    FQLG = 2112001,
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
    ///钛钢弓
    /// </summary>
    [FieldExplan("钛钢弓")]
    XGG = 2112004,
    /// <summary>
    ///钢玉弓
    /// </summary>
    [FieldExplan("钢玉弓")]
    GYG = 2112005,
    /// <summary>
    ///龙纹弓
    /// </summary>
    [FieldExplan("龙纹弓")]
    LWG = 2112006,
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
    ///钛钢盾
    /// </summary>
    [FieldExplan("钛钢盾")]
    XGD = 2114003,
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
    ///龙纹盾
    /// </summary>
    [FieldExplan("龙纹盾")]
    LWD1 = 2114006,
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
    ///晨星匕首
    /// </summary>
    [FieldExplan("晨星匕首")]
    CXXS = 2115005,
    /// <summary>
    ///逐光之影
    /// </summary>
    [FieldExplan("逐光之影")]
    ZGZY = 2115006,
    /// <summary>
    ///侦察兵帽
    /// </summary>
    [FieldExplan("侦察兵帽")]
    ZCBM = 2210001,
    /// <summary>
    ///精钢鳞甲盔
    /// </summary>
    [FieldExplan("精钢鳞甲盔")]
    JGLJK = 2210002,
    /// <summary>
    ///王国制式盔
    /// </summary>
    [FieldExplan("王国制式盔")]
    WGZSK = 2210003,
    /// <summary>
    ///钛金覆面盔
    /// </summary>
    [FieldExplan("钛金覆面盔")]
    XJFMK = 2210004,
    /// <summary>
    ///钛钢链盔
    /// </summary>
    [FieldExplan("钛钢链盔")]
    XGLK = 2210005,
    /// <summary>
    ///精金翼盔
    /// </summary>
    [FieldExplan("精金翼盔")]
    JJYK = 2210006,
    /// <summary>
    ///龙角盔
    /// </summary>
    [FieldExplan("龙角盔")]
    LJK = 2210007,
    /// <summary>
    ///智慧头环
    /// </summary>
    [FieldExplan("智慧头环")]
    ZHTH = 2211001,
    /// <summary>
    ///智者之光
    /// </summary>
    [FieldExplan("智者之光")]
    ZZZG = 2211002,
    /// <summary>
    ///占星头环
    /// </summary>
    [FieldExplan("占星头环")]
    ZXTH = 2211003,
    /// <summary>
    ///王者守护
    /// </summary>
    [FieldExplan("王者守护")]
    WZSH = 2211004,
    /// <summary>
    ///祭祀权冠
    /// </summary>
    [FieldExplan("祭祀权冠")]
    JXQG = 2211005,
    /// <summary>
    ///星光之冠
    /// </summary>
    [FieldExplan("星光之冠")]
    XGZG = 2211006,
    /// <summary>
    ///兽皮兜帽
    /// </summary>
    [FieldExplan("兽皮兜帽")]
    SPDM = 2212001,
    /// <summary>
    ///猎手头巾
    /// </summary>
    [FieldExplan("猎手头巾")]
    LSTJ = 2212002,
    /// <summary>
    ///隐匿头巾
    /// </summary>
    [FieldExplan("隐匿头巾")]
    YNTJ = 2212003,
    /// <summary>
    ///蛛网兜帽
    /// </summary>
    [FieldExplan("蛛网兜帽")]
    ZWDM = 2212004,
    /// <summary>
    ///残酷头巾
    /// </summary>
    [FieldExplan("残酷头巾")]
    CKTJ = 2212005,
    /// <summary>
    ///刺客兜帽
    /// </summary>
    [FieldExplan("刺客兜帽")]
    CKDM = 2212006,
    /// <summary>
    ///铁鳞甲
    /// </summary>
    [FieldExplan("铁鳞甲")]
    TLJ = 2310001,
    /// <summary>
    ///王国板甲
    /// </summary>
    [FieldExplan("王国板甲")]
    WGBJ = 2310002,
    /// <summary>
    ///精钢链甲
    /// </summary>
    [FieldExplan("精钢链甲")]
    JGLJ = 2310003,
    /// <summary>
    ///钛金战甲
    /// </summary>
    [FieldExplan("钛金战甲")]
    XJZJ = 2310004,
    /// <summary>
    ///钢玉战甲
    /// </summary>
    [FieldExplan("钢玉战甲")]
    GYZJ = 2310005,
    /// <summary>
    ///精金战甲
    /// </summary>
    [FieldExplan("精金战甲")]
    JJZJ = 2310006,
    /// <summary>
    ///龙纹甲
    /// </summary>
    [FieldExplan("龙纹甲")]
    LWJ = 2310007,
    /// <summary>
    ///衬钢锁子甲
    /// </summary>
    [FieldExplan("衬钢锁子甲")]
    CGSZJ = 2311001,
    /// <summary>
    ///精钢锁子甲
    /// </summary>
    [FieldExplan("精钢锁子甲")]
    JGSZJ = 2311002,
    /// <summary>
    ///圣堂护胸甲
    /// </summary>
    [FieldExplan("圣堂护胸甲")]
    STHXJ = 2311003,
    /// <summary>
    ///逐光锁甲
    /// </summary>
    [FieldExplan("逐光锁甲")]
    ZGSJ = 2311004,
    /// <summary>
    ///炎魔鳞铠
    /// </summary>
    [FieldExplan("炎魔鳞铠")]
    YMLX = 2311005,
    /// <summary>
    ///星光秘银甲
    /// </summary>
    [FieldExplan("星光秘银甲")]
    XGMYJ = 2311006,
    /// <summary>
    ///智慧法袍
    /// </summary>
    [FieldExplan("智慧法袍")]
    ZHFP = 2312001,
    /// <summary>
    ///宫廷法袍
    /// </summary>
    [FieldExplan("宫廷法袍")]
    GTFP = 2312002,
    /// <summary>
    ///占星法袍
    /// </summary>
    [FieldExplan("占星法袍")]
    ZXFP = 2312003,
    /// <summary>
    ///祭祀法袍
    /// </summary>
    [FieldExplan("祭祀法袍")]
    JXFP = 2312004,
    /// <summary>
    ///炼狱法衣
    /// </summary>
    [FieldExplan("炼狱法衣")]
    LYFY = 2312005,
    /// <summary>
    ///混沌长袍
    /// </summary>
    [FieldExplan("混沌长袍")]
    HXCP = 2312006,
    /// <summary>
    ///防护靴
    /// </summary>
    [FieldExplan("防护靴")]
    FHX = 2410001,
    /// <summary>
    ///王国锁链靴
    /// </summary>
    [FieldExplan("王国锁链靴")]
    WGSLX = 2410002,
    /// <summary>
    ///王国重军靴
    /// </summary>
    [FieldExplan("王国重军靴")]
    WGZJX = 2410003,
    /// <summary>
    ///圣堂护胫
    /// </summary>
    [FieldExplan("圣堂护胫")]
    STHX = 2410004,
    /// <summary>
    ///钛钢护胫
    /// </summary>
    [FieldExplan("钛钢护胫")]
    XGHX = 2410005,
    /// <summary>
    ///野兽护胫
    /// </summary>
    [FieldExplan("野兽护胫")]
    YSHX = 2410006,
    /// <summary>
    ///巨龙之靴
    /// </summary>
    [FieldExplan("巨龙之靴")]
    JLZX = 2410007,
    /// <summary>
    ///轻皮靴
    /// </summary>
    [FieldExplan("轻皮靴")]
    QPX = 2411001,
    /// <summary>
    ///防滑靴
    /// </summary>
    [FieldExplan("防滑靴")]
    FHX1 = 2411002,
    /// <summary>
    ///硬皮特种靴
    /// </summary>
    [FieldExplan("硬皮特种靴")]
    YPTZX = 2411003,
    /// <summary>
    ///隔离靴
    /// </summary>
    [FieldExplan("隔离靴")]
    GLX = 2411004,
    /// <summary>
    ///逐光之靴
    /// </summary>
    [FieldExplan("逐光之靴")]
    ZGZX = 2411005,
    /// <summary>
    ///星光之靴
    /// </summary>
    [FieldExplan("星光之靴")]
    XGZX = 2411006,
    /// <summary>
    ///奥术鞋
    /// </summary>
    [FieldExplan("奥术鞋")]
    ASX = 2412001,
    /// <summary>
    ///急速靴
    /// </summary>
    [FieldExplan("急速靴")]
    JSX = 2412002,
    /// <summary>
    ///蛛网靴
    /// </summary>
    [FieldExplan("蛛网靴")]
    ZWX = 2412003,
    /// <summary>
    ///秘法鞋
    /// </summary>
    [FieldExplan("秘法鞋")]
    MFX = 2412004,
    /// <summary>
    ///飞龙靴
    /// </summary>
    [FieldExplan("飞龙靴")]
    FLX = 2412005,
    /// <summary>
    ///神行靴
    /// </summary>
    [FieldExplan("神行靴")]
    SXX = 2412006,
    /// <summary>
    ///百合项链
    /// </summary>
    [FieldExplan("百合项链")]
    BHXL = 2510001,
    /// <summary>
    ///晨星项链
    /// </summary>
    [FieldExplan("晨星项链")]
    CXXL = 2510002,
    /// <summary>
    ///猎鹰项链
    /// </summary>
    [FieldExplan("猎鹰项链")]
    LYXL = 2510003,
    /// <summary>
    ///蔷薇项链
    /// </summary>
    [FieldExplan("蔷薇项链")]
    QXXL = 2510004,
    /// <summary>
    ///皎月项链
    /// </summary>
    [FieldExplan("皎月项链")]
    XYXL = 2510005,
    /// <summary>
    ///独角兽项链
    /// </summary>
    [FieldExplan("独角兽项链")]
    DJSXL = 2510006,
    /// <summary>
    ///隐匿之戒
    /// </summary>
    [FieldExplan("隐匿之戒")]
    YNZJ = 2511001,
    /// <summary>
    ///吸血指环
    /// </summary>
    [FieldExplan("吸血指环")]
    XXZH = 2511002,
    /// <summary>
    ///占星指环
    /// </summary>
    [FieldExplan("占星指环")]
    ZXZH = 2511003,
    /// <summary>
    ///逐光指环
    /// </summary>
    [FieldExplan("逐光指环")]
    ZGZH = 2511004,
    /// <summary>
    ///恶魔之戒
    /// </summary>
    [FieldExplan("恶魔之戒")]
    EMZJ = 2511005,
    /// <summary>
    ///星光之戒
    /// </summary>
    [FieldExplan("星光之戒")]
    XGZJ = 2511006,
    /// <summary>
    ///幸运护符
    /// </summary>
    [FieldExplan("幸运护符")]
    XYHF = 2512001,
    /// <summary>
    ///庇佑护符
    /// </summary>
    [FieldExplan("庇佑护符")]
    BYHF = 2512002,
    /// <summary>
    ///太阳神护符
    /// </summary>
    [FieldExplan("太阳神护符")]
    TYSHF = 2512003,
    /// <summary>
    ///死神护符
    /// </summary>
    [FieldExplan("死神护符")]
    SSHF = 2512004,
    /// <summary>
    ///生命护符
    /// </summary>
    [FieldExplan("生命护符")]
    SMHF = 2512005,
    /// <summary>
    ///自然护符
    /// </summary>
    [FieldExplan("自然护符")]
    ZRHF = 2512006,
    /// <summary>
    ///战神护符
    /// </summary>
    [FieldExplan("战神护符")]
    ZSHF = 2512007,
    /// <summary>
    ///月神护符
    /// </summary>
    [FieldExplan("月神护符")]
    YSHF = 2512008,
    /// <summary>
    ///王国骑士勋章
    /// </summary>
    [FieldExplan("王国骑士勋章")]
    WGQSXZ = 2513001,
    /// <summary>
    ///祭司印记
    /// </summary>
    [FieldExplan("祭司印记")]
    JSYJ = 2513002,
    /// <summary>
    ///不死克星勋章
    /// </summary>
    [FieldExplan("不死克星勋章")]
    BSKXXZ = 2513003,
    /// <summary>
    ///净化者之证
    /// </summary>
    [FieldExplan("净化者之证")]
    JHZZZ = 2513004,
    /// <summary>
    ///恶魔头骨勋章
    /// </summary>
    [FieldExplan("恶魔头骨勋章")]
    EMTGXZ = 2513005,
    /// <summary>
    ///暗影勋章
    /// </summary>
    [FieldExplan("暗影勋章")]
    AYXZ = 2513006,
    /// <summary>
    ///毒液炸弹
    /// </summary>
    [FieldExplan("毒液炸弹")]
    DYZD = 3110001,
    /// <summary>
    ///圣光炸弹
    /// </summary>
    [FieldExplan("圣光炸弹")]
    SGZD = 3110002,
    /// <summary>
    ///毒镖
    /// </summary>
    [FieldExplan("毒镖")]
    DX = 3111001,
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
    ///果酒
    /// </summary>
    [FieldExplan("果酒")]
    GJ = 4110001,
    /// <summary>
    ///个性者药剂
    /// </summary>
    [FieldExplan("个性者药剂")]
    GXZYJ = 4210001,
    /// <summary>
    ///恢复药剂
    /// </summary>
    [FieldExplan("恢复药剂")]
    HFYJ = 4210002,
    /// <summary>
    ///力量药剂
    /// </summary>
    [FieldExplan("力量药剂")]
    LLYJ = 4310001,
    /// <summary>
    ///吸血药剂
    /// </summary>
    [FieldExplan("吸血药剂")]
    XXYJ = 4410001,
    /// <summary>
    ///感知药剂
    /// </summary>
    [FieldExplan("感知药剂")]
    GZYJ = 4510001,
    /// <summary>
    ///恶魔终焉
    /// </summary>
    [FieldExplan("恶魔终焉")]
    EMZY = 5110001,
    /// <summary>
    ///情报魔法入门
    /// </summary>
    [FieldExplan("情报魔法入门")]
    QBMFRM = 5111001,
    /// <summary>
    ///王国骑士委任状
    /// </summary>
    [FieldExplan("王国骑士委任状")]
    WGQSWRZ = 5112001,
    /// <summary>
    ///远古恶魔之书
    /// </summary>
    [FieldExplan("远古恶魔之书")]
    YGEMZS = 5113001,
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
                    .OrderBy(temp => temp.value).ToArray();
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