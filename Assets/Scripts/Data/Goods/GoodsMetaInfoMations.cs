using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsMetaInfoMations : ILoadable<GoodsMetaInfoMations>
{

    Dictionary<int, Goods> allGoodsInfo = new Dictionary<int, Goods>();

    public GoodsMetaInfoMations()
    {

    }

    public void Load()
    {
    
        allGoodsInfo = JsonConvert.DeserializeObject<Dictionary<int, Goods>>(Resources.Load<TextAsset>("Data/Goods/allGoodsMetaInfo").text);
    }


    /// <summary>
    /// 根据物品枚举获取基本信息
    /// </summary>
    /// <param name="enumGoodsType"></param>
    /// <returns></returns>
    public Goods this[EnumGoodsType enumGoodsType]
    {
        get
        {
            int intType = (int)enumGoodsType;
            if (allGoodsInfo.ContainsKey(intType))
            {
                return allGoodsInfo[intType];
            }
            else
            {
                return null;
            }
        }

    }
}
