using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// 采集点信息
/// </summary>
public class StuffData : ILoadable<StuffData>
{
    /// <summary>
    /// 保存采集点信息的路径
    /// </summary>
    public static string dataDirectoryPath = "Data/Stuff";

    /// <summary>
    /// 采集点信息集合数组
    /// </summary>
    StuffDataInfoCollection[] stuffDataInfoCollections;

    public void Load()
    {
        TextAsset[] allTextAssets = Resources.LoadAll<TextAsset>(StuffData.dataDirectoryPath);//加载所有场景的stuff信息 
        List<StuffDataInfoCollection> stuffDataInfoCollectionList = new List<StuffDataInfoCollection>();
        foreach (TextAsset textAsset in allTextAssets)
        {
            string assetName = textAsset.name;
            string assetText = Encoding.UTF8.GetString(textAsset.bytes);
            StuffDataInfoCollection stuffDataInfoCollection = DeSerializeNow<StuffDataInfoCollection>(assetText);
            if (stuffDataInfoCollection != null)
            {
                stuffDataInfoCollectionList.Add(stuffDataInfoCollection);
            }
        }
        stuffDataInfoCollections = stuffDataInfoCollectionList.ToArray();
        foreach (StuffDataInfoCollection stuffDataInfoCollection in stuffDataInfoCollections)
        {
            if(stuffDataInfoCollection.StuffDataInfos!=null)
                foreach (StuffDataInfo stuffDataInfo in stuffDataInfoCollection.StuffDataInfos)
                {
                    stuffDataInfo.Init();
                }
        }
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="target">对象</param>
    /// <returns>返回的字符串</returns>
    //public string SerializeNow<T>(T target) where T : class
    //{
    //    if (target == null)
    //        return "";
    //    string value = JsonConvert.SerializeObject(target, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
    //    return value;
    //}

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
    /// 通过场景名和采集点id获取采集点信息
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="stuffID"></param>
    /// <returns></returns>
    public StuffDataInfo GetStuffDataInfo(string sceneName, int stuffID)
    {
        StuffDataInfoCollection stuffDataInfoColletion = stuffDataInfoCollections.FirstOrDefault(temp => string.Equals(temp.sceneName, sceneName));
        if (stuffDataInfoCollections != null)
            return stuffDataInfoColletion.StuffDataInfos.FirstOrDefault(temp => temp.StuffID == stuffID);
        return null;
    }

    /// <summary>
    /// 通过场景名获取该场景的所有采集点信息
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public StuffDataInfo[] GetStuffDataInfos(string sceneName)
    {
        StuffDataInfoCollection stuffDataInfoCollection = stuffDataInfoCollections.FirstOrDefault(temp => string.Equals(temp.sceneName, sceneName)); ;
        if (stuffDataInfoCollection != null && stuffDataInfoCollection.StuffDataInfos != null)
            return stuffDataInfoCollection.StuffDataInfos.ToArray();
        return new StuffDataInfo[0];
    }
}

/// <summary>
/// 采集点信息集合(要给场景一个该对象)
/// </summary>
[Serializable]
public class StuffDataInfoCollection
{
    /// <summary>
    /// 采集点信息集合
    /// </summary>
    public List<StuffDataInfo> StuffDataInfos;
    /// <summary>
    /// 场景名
    /// </summary>
    public string sceneName;

    public StuffDataInfoCollection()
    {
        StuffDataInfos = new List<StuffDataInfo>();
    }
}

/// <summary>
/// 单个采集点信息集合
/// </summary>
[Serializable]
public class StuffDataInfo
{
    /// <summary>
    /// 保存stuff预设提的路径
    /// </summary>
    [JsonIgnore]
    public static string stuffPrefabDirectoryPath = "Prefabs/Stuff";

    /// <summary>
    /// 采集点的材料类型
    /// </summary>
    public EnumGoodsType StuffType;
    /// <summary>
    /// 采集点ID
    /// </summary>
    public int StuffID;
    /// <summary>
    /// stuff的名字
    /// </summary>
    public string StuffName;
    /// <summary>
    /// 每次刷新后的数量
    /// </summary>
    public int StuffCount;
    /// <summary>
    /// 每次刷新的冷却时间
    /// </summary>
    public float CoolingTime;
    /// <summary>
    /// stuff的位置(分量x)
    /// </summary>
    [JsonProperty]
    private float locationX;
    /// <summary>
    /// stuff的位置(分量y)
    /// </summary>
    [JsonProperty]
    private float locationY;
    /// <summary>
    /// stuff的位置(分量z)
    /// </summary>
    [JsonProperty]
    private float locationZ;
    /// <summary>
    /// stuff的位置
    /// </summary>
    [JsonIgnore]
    public Vector3 StuffLocation
    {
        get
        {
            return new Vector3(locationX, locationY, locationZ);
        }
        set
        {
            locationX = value.x;
            locationY = value.y;
            locationZ = value.z;
        }
    }
    /// <summary>
    /// stuff的角度(分量x)
    /// </summary>
    [JsonProperty]
    private float angleX;
    /// <summary>
    /// stuff的角度(分量y)
    /// </summary>
    [JsonProperty]
    private float angleY;
    /// <summary>
    /// stuff的角度(分量z)
    /// </summary>
    [JsonProperty]
    private float angleZ;
    /// <summary>
    /// stuff的角度
    /// </summary>
    [JsonIgnore]
    public Vector3 StuffAngle
    {
        get
        {
            return new Vector3(angleX, angleY, angleZ);
        }
        set
        {
            angleX = value.x;
            angleY = value.y;
            angleZ = value.z;
        }
    }
    /// <summary>
    /// stuff所在场景
    /// </summary>
    public string SceneName;
    /// <summary>
    /// stuff的预设提名字
    /// </summary>
    public string stuffPrefabName;
    /// <summary>
    /// stuff的预设提对象
    /// </summary>
    [JsonIgnore]
    private GameObject StuffObjPrefab;
    /// <summary>
    /// 游戏对象
    /// </summary>
    [JsonIgnore]
    public GameObject StuffObj { get; set; }
    /// <summary>
    /// 附加的数据
    /// </summary>
    public string OtherValue;

    /// <summary>
    /// 当前该对象存在的数量
    /// </summary>
    [JsonIgnore]
    private int _StuffCount;
    /// <summary>
    /// 到指定时间可以冷却刷新
    /// </summary>
    [JsonIgnore]
    private float _CoolingTime;
    /// <summary>
    /// 初始化
    /// </summary>
    public void  Init()
    {
        _StuffCount = StuffCount;
        _CoolingTime = Time.time + CoolingTime;
    }

    /// <summary>
    /// 剩余的数量
    /// </summary>
    /// <returns></returns>
    public int ResidualCount()
    {
        return _StuffCount;
    }

    /// <summary>
    /// 采集
    /// </summary>
    /// <param name="num"></param>
    public void Collect(int num)
    {
        if (_StuffCount > 0)
        {
            _StuffCount -= num;
            if (_StuffCount <= 0)
            {
                _CoolingTime = Time.time + CoolingTime;//设置冷却时间
            }
        }
    }

    /// <summary>
    /// 尝试刷新
    /// </summary>
    /// <returns>返回刷新是否成功</returns>
    public bool Update()
    {
        if (Time.time > _CoolingTime && _StuffCount <= 0)
            _StuffCount = StuffCount;
        return _StuffCount > 0;
    }

    public void Load()
    {
        if (string.IsNullOrEmpty(stuffPrefabName))
            StuffObjPrefab = null;
        if (StuffObjPrefab == null)
            if (!string.IsNullOrEmpty(stuffPrefabName))
                StuffObjPrefab = Resources.Load<GameObject>(stuffPrefabDirectoryPath + "/" + stuffPrefabName);
        if (StuffObj == null && StuffObjPrefab != null)
        {
            StuffObj = GameObject.Instantiate<GameObject>(StuffObjPrefab);
            StuffObj.transform.position = StuffLocation;
            StuffObj.transform.eulerAngles = StuffAngle;
            StuffObj.name = StuffName;
            StuffObj.AddComponent<StuffDataInfoMono>().StuffDataInfo = this;
        }
    }


}

/// <summary>
/// stuff的数据(用于挂在在对象身上)
/// </summary>
public class StuffDataInfoMono : DataInfoType<StuffDataInfoMono>
{
    public StuffDataInfo StuffDataInfo;
}