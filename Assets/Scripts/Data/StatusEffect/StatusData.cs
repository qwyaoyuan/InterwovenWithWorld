using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// buff debuff 特殊状态的数据
/// </summary>
public class StatusData : ILoadable<StatusData>
{
    /// <summary>
    /// 保存状态数据信息的文件路径 
    /// </summary>
    public static string dataFilePath = "Data/Status/Status";

    /// <summary>
    /// 数据字典
    /// </summary>
    Dictionary<EnumStatusEffect, StatusDataInfo> dataDic;

    public void Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(StatusData.dataFilePath);
        string assetText = Encoding.UTF8.GetString(textAsset.bytes);
        dataDic = DeSerializeNow<Dictionary<EnumStatusEffect, StatusDataInfo>>(assetText);
    }

    /// <summary>
    /// 获取指定状态枚举的数组
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public StatusDataInfo this[EnumStatusEffect statusEffect]
    {
        get
        {
            if (dataDic != null && dataDic.ContainsKey(statusEffect))
                return dataDic[statusEffect];
            return null;
        }
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T">反序列化后的类型</typeparam>
    /// <param name="value">字符串</param>
    /// <returns>对象</returns>
    private T DeSerializeNow<T>(string value) where T : class
    {
        try
        {
            T target = JsonConvert.DeserializeObject<T>(value);
            return target;
        }
        catch { }
        return null;
    }
}

/// <summary>
/// buff debuff 特殊状态的对应数据
/// </summary>
[Serializable]
public class StatusDataInfo
{
    /// <summary>
    /// 用于查找状态图标的唯一id
    /// </summary>
    public string statusSpriteID;
    /// <summary>
    /// 状态显示的图标(主要是在血条状态ui)
    /// </summary>
    [JsonIgnore]
    public Sprite StatusSprite { get; set; }
    /// <summary>
    /// 数据的说明
    /// </summary>
    public string StatusExplane { get; set; }

    /// <summary>
    /// 等级对应的数据字典
    /// </summary>
    [JsonProperty]
    private Dictionary<int, StatusLevelDataInfo> levelToDataDic;

    public StatusDataInfo()
    {
        levelToDataDic = new Dictionary<int, StatusLevelDataInfo>();
    }

    public void Load()
    {
        if (StatusSprite == null)
            if (!string.IsNullOrEmpty(statusSpriteID))
                StatusSprite = SpriteManager.GetSrpite(statusSpriteID);
    }

    /// <summary>
    /// 获取最大的状态等级
    /// </summary>
    [JsonIgnore]
    public int MaxLevel
    {
        get
        {
            if (levelToDataDic.Count > 0)
                return levelToDataDic.Max(temp => temp.Key);
            return 0;
        }
    }

    /// <summary>
    /// 获取当前等级的状态数据
    /// </summary>
    /// <param name="level">等级</param>
    /// <returns></returns>
    [JsonIgnore]
    public StatusLevelDataInfo this[int level]
    {
        get
        {
            level = Mathf.Clamp(level, 0, MaxLevel);
            if (levelToDataDic.ContainsKey(level))
                return levelToDataDic[level];
            return levelToDataDic.Select(temp=>temp.Value).FirstOrDefault();
        }
    }

    [Serializable]
    public class StatusLevelDataInfo
    {
        /// <summary>
        /// 状态在当前等级的说明
        /// </summary>
        public string LevelExplane { get; set; }
    }
}
