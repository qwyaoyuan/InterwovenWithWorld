using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 物品能力
/// </summary>
public class GoodsAbility
{
    /// <summary>
    /// 能力种类
    /// </summary>
    public EnumGoodsAbility AbilibityKind { get; set; }

    /// <summary>
    /// 能力值
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// 能力等级
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Explain { get; set; }

    public GoodsAbility()
    {

    }

    public GoodsAbility(EnumGoodsAbility abilityKind, int val, string explain)
    {
        this.AbilibityKind = abilityKind;
        this.Value = val;
        this.Explain = explain;
    }
}

/// <summary>
/// 物品能力类型
/// </summary>
public enum EnumGoodsAbility
{
    #region 附加指示
    None = 0,
    /// <summary>
    /// 区分基础属性与可附加属性
    /// </summary>
    [EnumDisplay("区分基础属性与可附加属性")]
    Nothing = 1,
    /// <summary>
    /// 指示此物品为唯一物品
    /// </summary>
    [EnumDisplay("指示此物品为唯一物品")]
    RedOnly = 2,
    #endregion
    #region 属性附加
    /// <summary>
    /// 生命值附加
    /// </summary>
    [EnumDisplay("生命值附加")]
    HP = 10,
    /// <summary>
    /// 法力值附加
    /// </summary>
    [EnumDisplay("法力值附加")]
    MP = 20,
    /// <summary>
    /// 灵巧值附加
    /// </summary>
    [EnumDisplay("敏捷值附加")]
    DEX = 30,
    /// <summary>
    /// 意志值附加
    /// </summary>
    [EnumDisplay("精神值附加")]
    WIL = 40,
    /// <summary>
    /// 力量值附加
    /// </summary>
    [EnumDisplay("力量值附加")]
    STR = 50,
    /// <summary>
    /// 魔法亲和性附加
    /// </summary>
    [EnumDisplay("魔法亲和性附加")]
    MAFF = 60,
    /// <summary>
    /// 异常抗性附加
    /// </summary>
    [EnumDisplay("异常抗性附加")]
    SRES = 70,
    /// <summary>
    /// 视野附加
    /// </summary>
    [EnumDisplay("视野附加")]
    Sight = 80,
    /// <summary>
    /// 移动速度附加
    /// </summary>
    [EnumDisplay("移动速度附加")]
    MSPD = 90,
    /// <summary>
    /// 攻击速度附加
    /// </summary>
    [EnumDisplay("攻击速度附加")]
    ASPD = 100,
    /// <summary>
    /// 闪避率附加
    /// </summary>
    [EnumDisplay("闪避率附加")]
    AVD = 110,
    /// <summary>
    /// 命中率附加
    /// </summary>
    [EnumDisplay("命中率附加")]
    HIT = 120,
    /// <summary>
    /// 暴击率附加
    /// </summary>
    [EnumDisplay("暴击率附加")]
    Critical = 130,
    /// <summary>
    /// 暴击伤害附加
    /// </summary>
    [EnumDisplay("暴击伤害附加")]
    CriticalDmg = 131,
    /// <summary>
    /// 魔法攻击力附加
    /// </summary>
    [EnumDisplay("魔法攻击力附加")]
    INT = 140,
    /// <summary>
    /// 魔法防御力附加
    /// </summary>
    [EnumDisplay("魔法防御力附加")]
    RES = 150,
    /// <summary>
    /// 物理攻击力附加
    /// </summary>
    [EnumDisplay("物理攻击力附加")]
    ATN = 160,
    /// <summary>
    /// 物理防御力附加
    /// </summary>
    [EnumDisplay("物理防御力附加")]
    DEF = 170,
    /// <summary>
    /// 光明信仰强度附加
    /// </summary>
    [EnumDisplay("光明信仰强度附加")]
    Light = 180,
    /// <summary>
    /// 黑暗信仰强度附加
    /// </summary>
    [EnumDisplay("黑暗信仰强度附加")]
    Dark = 190,
    /// <summary>
    /// 生物信仰强度附加
    /// </summary>
    [EnumDisplay("生物信仰强度附加")]
    Bioligy = 200,
    /// <summary>
    /// 自然信仰强度附加
    /// </summary>
    [EnumDisplay("自然信仰强度附加")]
    Nature = 210,
    #endregion
    #region 装备特有属性
    /// <summary>
    /// 装备属性开始
    /// </summary>
    [EnumDisplay("装备属性开始")]
    EquipMents = 1000,
    /// <summary>
    /// 装备物理伤害
    /// </summary>
    [EnumDisplay("装备物理伤害")]
    EquipATK = 1010,
    /// <summary>
    /// 装备护甲
    /// </summary>
    [EnumDisplay("装备护甲")]
    EquipDEF = 1020,
    /// <summary>
    /// 伤害格挡率(盾)
    /// </summary>
    [EnumDisplay("伤害格挡率(盾)")]
    EquipBlock = 1030,
    /// <summary>
    /// 暴击防御
    /// </summary>
    [EnumDisplay("暴击防御")]
    CriticalDef = 1040,
    /// <summary>
    /// 魔法恢复速度提升
    /// </summary>
    [EnumDisplay("魔法恢复速度提升")]
    MPRec = 1050,
    /// <summary>
    /// 生命恢复速度提升
    /// </summary>
    [EnumDisplay("生命恢复速度提升")]
    HPRec = 1060,
    #endregion
    #region 护身符加护
    /// <summary>
    /// 护身符效果开始
    /// </summary>
    [EnumDisplay("护身符效果开始")]
    Shield = 2000,
    /// <summary>
    /// 幸运加护
    /// </summary>
    [EnumDisplay("幸运加护")]
    LuckShi = 2010,
    /// <summary>
    /// 庇佑加护
    /// </summary>
    [EnumDisplay("庇佑加护")]
    GarShi = 2020,
    /// <summary>
    /// 战神加护
    /// </summary>
    [EnumDisplay("战神加护")]
    WarShi = 2030,
    /// <summary>
    /// 月神加护
    /// </summary>
    [EnumDisplay("月神加护")]
    MoonShi = 2040,
    /// <summary>
    /// 太阳神加护
    /// </summary>
    [EnumDisplay("太阳神加护")]
    SunShi = 2050,
    /// <summary>
    /// 死神加护
    /// </summary>
    [EnumDisplay("死神加护")]
    DeathShi = 2060,
    /// <summary>
    /// 生命女神加护
    /// </summary>
    [EnumDisplay("生命女神加护")]
    LifeShi = 2070,
    /// <summary>
    /// 自然女神加护
    /// </summary>
    [EnumDisplay("自然女神加护")]
    NatureShi = 2080,
    #endregion
    #region 药剂(2500)
    /// <summary>
    /// 药剂提升自身魔法恢复速度倍率
    /// </summary>
    [EnumDisplay("药剂提升自身魔法恢复速度倍率")]
    MPRec_Rate = 2510,
    /// <summary>
    /// 药剂提升自身生命恢复速度倍率
    /// </summary>
    [EnumDisplay("药剂提升自身生命恢复速度倍率")]
    HPRect_Rate = 2520,
    #endregion
    #region 魔法与基础值加成能力
    /// <summary>
    /// 魔法与基础值加成能力开始
    /// </summary>
    [EnumDisplay("魔法与基础值加成能力开始")]
    MagicEffects = 3000,
    /// <summary>
    /// 火焰防护
    /// </summary>
    [EnumDisplay("火焰防护")]
    MagicFireRES = 3010,
    /// <summary>
    /// 精神基础值加成
    /// </summary>
    [EnumDisplay("精神基础值加成")]
    WILPlus = 3020,
    #endregion
    #region 特殊能力类型
    /// <summary>
    /// 物品特殊能力类型（例如暗影勋章这种物品的特殊能力）
    /// </summary>
    特殊能力类型开始 = 10000,
    /// <summary>
    /// 移动速度降低抗性
    /// </summary>
    [EnumDisplay("移动速度降低抗性")]
    MovDef = 10010,
    /// <summary>
    /// 被发现概率降低
    /// </summary>
    [EnumDisplay("被发现概率降低")]
    SightDef = 10020,
    /// <summary>
    /// 物理攻击吸血(百分比)
    /// </summary>
    [EnumDisplay("物理攻击吸血(百分比)")]
    BloodSuck = 10030,
    /// <summary>
    /// 暗影勋章
    /// </summary>
    [EnumDisplay("暗影勋章")]
    ShadowHonor = 10040,
    /// <summary>
    /// 物理伤害反弹
    /// </summary>
    [EnumDisplay("物理伤害反弹")]
    DMGReflect = 10050,
    /// <summary>
    /// 魔法消耗降低
    /// </summary>
    [EnumDisplay("魔法消耗降低")]
    MPReduce = 10060,
    /// <summary>
    /// 一阶魔法免疫
    /// </summary>
    [EnumDisplay("一阶魔法免疫")]
    MPINV = 10070,
    #endregion
}


[AttributeUsage(AttributeTargets.Field)]
public class EnumDisplayAttribute : Attribute
{
    public EnumDisplayAttribute(string display)
    {
        Display = display;
    }
    public string Display
    {
        get;
        private set;
    }
}

public static class EnumExtentions
{
    public static string Display(this Enum t)
    {
        var t_type = t.GetType();
        var fieldName = Enum.GetName(t_type, t);
        var attributes = t_type.GetField(fieldName).GetCustomAttributes(false);
        var enumDisplayAttribute = attributes.FirstOrDefault(p => p.GetType().Equals(typeof(EnumDisplayAttribute))) as EnumDisplayAttribute;
        return enumDisplayAttribute == null ? fieldName : enumDisplayAttribute.Display;
    }
}