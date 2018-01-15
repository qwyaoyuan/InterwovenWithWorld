using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 道具栏UI选择物体接口
/// </summary>
public interface IUIItemSelectGoods
{
    /// <summary>
    /// 注册一个选择物品ID的回调
    /// </summary>
    /// <param name="SelectGoodsIDAction"></param>
    void RegistorSelectGoodsID(Action<int> SelectGoodsIDAction);

    /// <summary>
    /// 选择了该ID的物品
    /// </summary>
    /// <param name="goodsID"></param>
    void SelectID(int goodsID);
}

