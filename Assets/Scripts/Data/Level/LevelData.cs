using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 等级数据
/// </summary>
public class LevelData : ILoadable<LevelData>
{
    /// <summary>
    /// 保存等级信息文件的路径
    /// </summary>
    public static string dataFilePath = "Data/Level/Level";

    /// <summary>
    /// 等级与数据对应的字典
    /// </summary>
    Dictionary<int, LevelDataInfo> levelToDataDic;

    public void Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(dataFilePath);
        string assetText = Encoding.UTF8.GetString(textAsset.bytes);
        levelToDataDic = JsonConvert.DeserializeObject<Dictionary<int, LevelDataInfo>>(assetText);
        if (levelToDataDic == null)
            levelToDataDic = new Dictionary<int, LevelDataInfo>(); 
    }

    /// <summary>
    /// 最大等级
    /// </summary>
    public int MaxLevel
    {
        get
        {
            if (levelToDataDic.Count > 0)
                return levelToDataDic.Select(temp => temp.Key).Max();
            return 0;
        }
    }

    /// <summary>
    /// 最小等级
    /// </summary>
    public int MinLevel
    {
        get
        {
            if (levelToDataDic.Count > 0)
                return levelToDataDic.Select(temp => temp.Key).Min();
            return 0;
        }
    }

    /// <summary>
    /// 获取当前等级的数据信息 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public LevelDataInfo this[int index]
    {
        get
        {
            if (levelToDataDic.ContainsKey(index))
            {
                return levelToDataDic[index];
            }
            return null;
        }
    }
}

/// <summary>
/// 该等级包含的信息
/// </summary>
[Serializable]
public class LevelDataInfo
{
    [JsonProperty] private int level;
    /// <summary>
    /// 等级,表示该数据是哪个等级的
    /// </summary>
    [JsonIgnore] public int Level { get { return level; } }

    [JsonProperty] private int experience;
    /// <summary>
    /// 经验值,表示该等级升级的话需要多少经验值
    /// </summary>
    [JsonIgnore] public int Experience { get { return experience; } }

    [JsonProperty] private int strength;
    /// <summary>
    /// 力量,表示升到该等级后附加的属性
    /// </summary>
    [JsonIgnore] public int Strength { get { return strength; } }

    [JsonProperty] private int spirit;
    /// <summary>
    /// 精神,表示升到该等级后附加的属性
    /// </summary>
    [JsonIgnore] public int Spirit { get { return spirit; } }

    [JsonProperty] private int agility;
    /// <summary>
    /// 敏捷,表示升到该等级后附加的属性
    /// </summary>
    [JsonIgnore] public int Agility { get { return agility; } }

    [JsonProperty] private int concentration;
    /// <summary>
    /// 专注,表示升到该等级后附加的属性
    /// </summary>
    [JsonIgnore] public int Concentration { get { return concentration; } }

    [JsonProperty] private int freedomPoint;
    /// <summary>
    /// 自由点数,表示升到该等级后附加的自由点数
    /// </summary>
    [JsonIgnore] public int FreedomPoint { get { return freedomPoint; } }
}

