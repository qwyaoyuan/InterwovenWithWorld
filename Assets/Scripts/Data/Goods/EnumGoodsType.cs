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
    /// <summary>
    /// 铁矿石
    /// </summary>
    [FieldExplan("铁矿石")]
    TieKuangShi = 1110001,
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
    /// <summary>
    /// 毛胚
    /// </summary>
    [FieldExplan("毛胚")]
    MaoPei = 1112001,
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
    /// <summary>
    /// 铁剑
    /// </summary>
    [FieldExplan("铁剑")]
    TieJian = 2110001,
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

}