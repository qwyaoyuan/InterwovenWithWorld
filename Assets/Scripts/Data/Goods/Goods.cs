using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有游戏内物品基类
/// </summary>
public class Goods
{
    private EnumGoodsType enumGoodsType;

    /// <summary>
    /// 商品名称
    /// </summary>
    public string GoodsName { get; set; }
    #region 物品基础通用属性
    //物品ID
    public EnumGoodsType EnumGoodsType { get { return enumGoodsType; } }
    //重量
    public int Weight { get; set; }
    //基础价格
    public int Price { get; set; }
    //文字说明
    public string Explain { get; set; }

    //物品具有的属性列表
    public List<GoodsAbility> goodsAbilities = new List<GoodsAbility>();
    /// <summary>
    /// 物品图片
    /// </summary>
    [JsonIgnore]
    public Sprite Sprite { get; set; }
    #endregion

    public Goods()
    {

    }
    public Goods(EnumGoodsType enumGoodsType)
    {
        this.enumGoodsType = enumGoodsType;
    }

    public Goods(EnumGoodsType enumGoodsType, string name, int weight, int price, string expain)
    {
        this.enumGoodsType = enumGoodsType;
        this.GoodsName = name;
        this.Weight = weight;
        this.Price = price;
        this.Explain = expain;
    }
}


