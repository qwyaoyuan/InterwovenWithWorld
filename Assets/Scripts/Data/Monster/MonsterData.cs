using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 怪物的数据
/// </summary>
public class MonsterData : ILoadable<MonsterData>
{
    /// <summary>
    /// 保存怪物信息的路径
    /// </summary>
    public static string dataFilePath = "Data/Monster/Monster";

    /// <summary>
    /// 怪物信息集合数组 
    /// </summary>
    MonsterDataInfoCollection[] monsterDataInfoCollections;

    public void Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(MonsterData.dataFilePath);//加载怪物信息
        string assetName = textAsset.name;
        string assetText = Encoding.UTF8.GetString(textAsset.bytes);
        monsterDataInfoCollections = DeSerializeNow<MonsterDataInfoCollection[]>(assetText);

    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T">反序列化后的类型</typeparam>
    /// <param name="value">字符串</param>
    /// <returns>对象</returns>
    public T DeSerializeNow<T>(string value) where T : class
    {
        T target = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
        return target;
    }

    /// <summary>
    /// 通过场景名获取该场景的所有怪物信息
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public MonsterDataInfo[] GetMonsterDataInfos(string sceneName)
    {
        MonsterDataInfoCollection monsterDataInfoCollection = monsterDataInfoCollections.FirstOrDefault(temp => string.Equals(temp.sceneName, sceneName));
        if (monsterDataInfoCollection != null && monsterDataInfoCollection.MonsterDataInofs != null)
        {
            return monsterDataInfoCollection.MonsterDataInofs.ToArray();
        }
        return new MonsterDataInfo[0];
    }
}

/// <summary>
/// 怪物信息集合(一个场景一个该对象)
/// </summary>
[Serializable]
public class MonsterDataInfoCollection
{
    /// <summary>
    /// 怪物策略信息
    /// </summary>
    public List<MonsterDataInfo> MonsterDataInofs;
    /// <summary>
    /// 场景名
    /// </summary>
    public string sceneName;

    public MonsterDataInfoCollection()
    {
        MonsterDataInofs = new List<MonsterDataInfo>();
    }
}

/// <summary>
/// 单个怪物策略信息
/// 区域中心,区域范围,怪物类型,怪物AI类型
/// </summary>
[Serializable]
public class MonsterDataInfo
{
    /// <summary>
    /// 保存怪物预设体的路径
    /// </summary>
    [JsonIgnore]
    public static string monsterPrefabDirectoryPath = "Prefabs";
    /// <summary>
    /// 该数据的id(因为有可能一个场景有相同类型的怪物,但是数据不一样)
    /// </summary>
    public int ID;
    /// <summary>
    /// 怪物的类型
    /// </summary>
    public EnumMonsterType MonsterType;

    [JsonProperty]
    private float Center_X,Center_Y,Center_Z;
    /// <summary>
    /// 区域中心
    /// </summary>
    [JsonIgnore]
    public Vector3 Center
    {
        get { return new Vector3(Center_X, Center_Y, Center_Z); }
        set
        {
            Center_X = value.x;
            Center_Y = value.y;
            Center_Z = value.z;
        }
    }
    /// <summary>
    /// 区域范围
    /// </summary>
    public float Range;
    /// <summary>
    /// 怪物的AI类型
    /// </summary>
    public EnumMonsterAIType AIType;
    /// <summary>
    /// AI的数据
    /// </summary>
    public MonsterAIDataStruct AIData;
    /// <summary>
    /// 物品掉落类型
    /// </summary>
    public EnumGoodsType[] ItemDropTypes;
    /// <summary>
    /// 物品掉落概率
    /// </summary>
    public float[] ItemDropRates;
    /// <summary>
    /// 经验值
    /// </summary>
    public int Experience;
    /// <summary>
    /// 该配置的简述
    /// </summary>
    public string Briefly;
    /// <summary>
    /// 该配置的详细说明
    /// </summary>
    public string Explane;

    /// <summary>
    /// 怪物的预设提名字
    /// </summary>
    public string monsterPrefabName;
    [JsonIgnore]
    private GameObject _MonsterPrefab;
    /// <summary>
    /// 怪物的预设提 
    /// </summary>
    [JsonIgnore]
    public GameObject MonsterPrefab
    {
        get
        {
            if (_MonsterPrefab == null)
            {
                if (!string.IsNullOrEmpty(monsterPrefabName))
                    _MonsterPrefab = Resources.Load<GameObject>(monsterPrefabDirectoryPath + "/" + monsterPrefabName);
            }
            return _MonsterPrefab;
        }
    }
}

/// <summary>
/// 怪物AI的基础结构
/// </summary>
public class MonsterAIDataStruct
{
    /// <summary>
    /// 怪物刷新时间
    /// </summary>
    [FieldExplan("怪物刷新时间")]
    public float UpdateTime;
    /// <summary>
    /// 跟随的最远距离
    /// </summary>
    [FieldExplan("跟随的最远距离")]
    public float FollowDistance;

}

/// <summary>
/// 触发型AI
/// </summary>
[Serializable]
public class MonsterAIData_Trigger : MonsterAIDataStruct
{
    /// <summary>
    /// 该区域内生成怪物的数量
    /// </summary>
    [FieldExplan("该区域内生成怪物的数量")]
    public int Count;
    /// <summary>
    /// 触发范围(触发中心按照区域中心计算)
    /// </summary>
    [FieldExplan("触发范围(触发中心按照区域中心计算)")]
    public float TriggerRange;
}

/// <summary>
/// 巡逻型
/// </summary>
[Serializable]
public class MonsterAIData_GoOnPatrol : MonsterAIDataStruct
{
    /// <summary>
    /// 该区域内生成怪物的数量
    /// </summary>
    [FieldExplan("该区域内生成怪物的数量")]
    public int Count;
    /// <summary>
    /// 触发后是否通知同组的怪物
    /// </summary>
    [FieldExplan("触发后是否通知同组的怪物")]
    public bool NotifyOther;
}

/// <summary>
/// Boss型
/// </summary>
[Serializable]
public class MonsterAIData_Boss : MonsterAIDataStruct
{
    /// <summary>
    /// Boss是否会狂暴
    /// </summary>
    [FieldExplan("Boss是否会狂暴")]
    public bool CanViolent;
    /// <summary>
    /// Boss在指定的百分比血量后狂暴
    /// </summary>
    [FieldExplan("Boss在指定的百分比血量后狂暴")]
    [Range(0, 1)]
    public float ViolentValue;
}
