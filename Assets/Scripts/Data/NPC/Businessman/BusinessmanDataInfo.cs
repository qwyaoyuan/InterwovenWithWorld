using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 商人的数据
/// </summary>
public class BusinessmanDataInfo
{
    /// <summary>
    /// 物品类型对应物品要求字典
    /// </summary>
    public Dictionary<EnumGoodsType, GoodsDataInfoInner> GoodsDic;

    public BusinessmanDataInfo()
    {
        GoodsDic = new Dictionary<EnumGoodsType, GoodsDataInfoInner>();
    }

    public class GoodsDataInfoInner
    {
        /// <summary>
        /// 最小品质
        /// </summary>
        public EnumQualityType MinQualityType;
        /// <summary>
        /// 最大品质
        /// </summary>
        public EnumQualityType MaxQualityType;
        /// <summary>
        /// 数量(主要适用与药品)
        /// </summary>
        public int Count;
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="target">对象</param>
    /// <returns>返回的字符串</returns>
    public static string SerializeNow<T>(T target) where T : class
    {
        if (target == null)
            return "";
        string value = JsonConvert.SerializeObject(target, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
        return value;
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T">反序列化后的类型</typeparam>
    /// <param name="value">字符串</param>
    /// <returns>对象</returns>
    public static T DeSerializeNow<T>(string value) where T : class
    {
        try
        {
            T target = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            return target;
        }
        catch { }
        return null;
    }
}
