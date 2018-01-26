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
    public EnumGoodsType EnumGoodsType { get { return enumGoodsType; } set { enumGoodsType = value; } }
    //重量
    public int Weight { get; set; }
    //基础价格
    public int Price { get; set; }
    //文字说明
    public string Explain { get; set; }
    //图片名
    public string SpriteName { get; set; }
    //物品具有的属性列表
    public List<GoodsAbility> goodsAbilities = new List<GoodsAbility>();
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

    /// <summary>
    /// 获取该对象的深拷贝
    /// </summary>
    /// <param name="all">是否包含全部(主要是对应的特殊属性)</param>
    /// <returns></returns>
    public Goods Clone(bool all)
    {
        Goods newGoods = new Goods(enumGoodsType, GoodsName, Weight, Price, Explain);
        newGoods.SpriteName = SpriteName;
        if (all)
        {
            foreach (GoodsAbility goodsAbility in goodsAbilities)
            {
                GoodsAbility newGoodsAbility = new GoodsAbility(goodsAbility.AbilibityKind, goodsAbility.Value, goodsAbility.Explain);
                newGoods.goodsAbilities.Add(newGoodsAbility);
            }
        }
        return newGoods;
    }
}


