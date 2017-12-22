using System.Collections;
using System.Collections.Generic;
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
    None = 0,
    /// <summary>
    /// 生命值附加
    /// </summary>
    [FieldExplan("生命值附加")]
    HP = 10,
    /// <summary>
    /// 法力值附加
    /// </summary>
    [FieldExplan("法力值附加")]
    MP = 20,
    /// <summary>
    /// 灵巧值附加
    /// </summary>
    [FieldExplan("灵巧值附加")]
    DEX = 30,
    /// <summary>
    /// 意志值附加
    /// </summary>
    [FieldExplan("意志值附加")]
    WIL = 50,
    /// <summary>
    /// 力量值附加
    /// </summary>
    [FieldExplan("力量值附加")]
    STR = 60,


    /// <summary>
    /// 物品特殊能力类型（例如暗影勋章这种物品的特殊能力）
    /// </summary>
    特殊能力类型开始 = 10000,
}
