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
    [FieldExplan("生命值附加", "获得额外指定数值的生命值")]
    HP = 10,
    /// <summary>
    /// 法力值附加
    /// </summary>
    [EnumDisplay("法力值附加")]
    [FieldExplan("法力值附加", "获得额外指定数值的法力值")]
    MP = 20,
    /// <summary>
    /// 灵巧值附加
    /// </summary>
    [EnumDisplay("敏捷值附加")]
    [FieldExplan("敏捷值附加", "获得额外指定数值的敏捷值")]
    DEX = 30,
    /// <summary>
    /// 意志值附加
    /// </summary>
    [EnumDisplay("精神值附加")]
    [FieldExplan("精神值附加", "获得额外指定数值的精神值")]
    WIL = 40,
    /// <summary>
    /// 力量值附加
    /// </summary>
    [EnumDisplay("力量值附加")]
    [FieldExplan("力量值附加", "获得额外指定数值的力量值")]
    STR = 50,
    /// <summary>
    /// 魔法亲和性附加
    /// </summary>
    [EnumDisplay("魔法亲和性附加")]
    [FieldExplan("魔法亲和性附加", "获得额外指定数值的魔法亲和性")]
    MAFF = 60,
    /// <summary>
    /// 异常抗性附加
    /// </summary>
    [EnumDisplay("异常抗性附加")]
    [FieldExplan("异常抗性附加", "获得额外指定数值的异常状态抗性")]
    SRES = 70,
    /// <summary>
    /// 视野附加
    /// </summary>
    [EnumDisplay("视野附加")]
    [FieldExplan("视野附加", "获得额外指定数值的视野(百分比)")]
    Sight = 80,
    /// <summary>
    /// 移动速度附加
    /// </summary>
    [EnumDisplay("移动速度附加")]
    [FieldExplan("移动速度附加", "获得额外指定数值的移动速度(百分比)")]
    MSPD = 90,
    /// <summary>
    /// 攻击速度附加
    /// </summary>
    [EnumDisplay("攻击速度附加")]
    [FieldExplan("攻击速度附加", "获得额外指定数值的攻击速度(百分比)")]
    ASPD = 100,
    /// <summary>
    /// 闪避率附加
    /// </summary>
    [EnumDisplay("闪避率附加")]
    [FieldExplan("闪避率附加", "获得额外指定数值的闪避率(百分比)")]
    AVD = 110,
    /// <summary>
    /// 命中率附加
    /// </summary>
    [EnumDisplay("命中率附加")]
    [FieldExplan("命中率附加", "获得额外指定数值的命中率(百分比)")]
    HIT = 120,
    /// <summary>
    /// 暴击率附加
    /// </summary>
    [EnumDisplay("暴击率附加")]
    [FieldExplan("暴击率附加", "获得额外指定数值的暴击率(百分比)")]
    Critical = 130,
    /// <summary>
    /// 魔法攻击力附加
    /// </summary>
    [EnumDisplay("魔法攻击力附加")]
    [FieldExplan("魔法攻击力附加", "获得额外指定数值的魔法攻击力")]
    INT = 140,
    /// <summary>
    /// 魔法防御力附加
    /// </summary>
    [EnumDisplay("魔法防御力附加")]
    [FieldExplan("魔法防御力附加", "获得额外指定数值的魔法防御力")]
    RES = 150,
    /// <summary>
    /// 物理攻击力附加
    /// </summary>
    [EnumDisplay("物理攻击力附加")]
    [FieldExplan("物理攻击力附加", "获得额外指定数值的物理攻击力")]
    ATN = 160,
    /// <summary>
    /// 物理防御力附加
    /// </summary>
    [EnumDisplay("物理防御力附加")]
    [FieldExplan("物理防御力附加", "获得额外指定数值的物理防御力")]
    DEF = 170,
    /// <summary>
    /// 光明信仰强度附加
    /// </summary>
    [EnumDisplay("光明信仰强度附加")]
    [FieldExplan("光明信仰强度附加", "获得额外指定数值的光明信仰强度")]
    Light = 180,
    /// <summary>
    /// 黑暗信仰强度附加
    /// </summary>
    [EnumDisplay("黑暗信仰强度附加")]
    [FieldExplan("黑暗信仰强度附加", "获得额外指定数值的黑暗信仰强度")]
    Dark = 190,
    /// <summary>
    /// 生物信仰强度附加
    /// </summary>
    [EnumDisplay("生物信仰强度附加")]
    [FieldExplan("生物信仰强度附加", "获得额外指定数值的生物信仰强度")]
    Bioligy = 200,
    /// <summary>
    /// 自然信仰强度附加
    /// </summary>
    [EnumDisplay("自然信仰强度附加")]
    [FieldExplan("自然信仰强度附加", "获得额外指定数值的自然信仰强度")]
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
    [FieldExplan("装备物理伤害", "装备的物理伤害基础值")]
    EquipATK = 1010,
    /// <summary>
    /// 装备护甲
    /// </summary>
    [EnumDisplay("装备护甲")]
    [FieldExplan("装备护甲", "装备的护甲基础值")]
    EquipDEF = 1020,
    #endregion
    /// <summary>
    /// 物品特殊能力类型（例如暗影勋章这种物品的特殊能力）
    /// </summary>
    特殊能力类型开始 = 10000,
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