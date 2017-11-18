using System.Collections.Generic;
/// <summary>
/// 所有游戏内物品基类
/// </summary>
public class Goods
{
    private int ID;

    /// <summary>
    /// 商品名称
    /// </summary>
    public string GoodsName { get; set; }
    #region 物品基础通用属性
    //物品ID
    public EnumGoodsType EnumGoodsType { get { return (EnumGoodsType)ID; } }
    //重量
    public int Weight { get; set; }
    //基础价格
    public int Price { get; set; }
    //文字说明
    public string Explain { get; set; }

    //物品具有的属性列表
    public List<GoodsAbility> goodsAbilities = new List<GoodsAbility>();
    #endregion

    public Goods()
    {

    }
    public Goods(int id)
    {
        this.ID = id;
    }

    public Goods(int id, string name, int weight, int price, string expain)
    {
        this.ID = id;
        this.GoodsName = name;
        this.Weight = weight;
        this.Price = price;
        this.Explain = expain;
    }
}


