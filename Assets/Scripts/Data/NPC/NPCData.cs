using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// NPC数据
/// </summary>
public class NPCData : ILoadable<NPCData>
{
    /// <summary>
    /// 保存NPC信息的路径
    /// </summary>
    public static string dataDirectoryPath = "Data/NPC";

    /// <summary>
    /// npc信息集合数组
    /// </summary>
    NPCDataInfoCollection[] npcDataInfoCollections;

    public void Load()
    {
        TextAsset[] allTextAssets = Resources.LoadAll<TextAsset>(NPCData.dataDirectoryPath);//加载所有场景的npc信息
        List<NPCDataInfoCollection> npcDataInfoCollectionList = new List<NPCDataInfoCollection>();
        foreach (TextAsset textAsset in allTextAssets)
        {
            string assetName = textAsset.name;
            string assetText = Encoding.UTF8.GetString(textAsset.bytes);
            NPCDataInfoCollection npcDataInfoCollection = DeSerializeNow<NPCDataInfoCollection>(assetText);
            if (npcDataInfoCollection != null)
            {
                npcDataInfoCollectionList.Add(npcDataInfoCollection);
            }
        }
        npcDataInfoCollections = npcDataInfoCollectionList.ToArray();
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

    /// <summary>
    /// 通过npc id获取npc信息
    /// </summary>
    /// <param name="npcID">npc id</param>
    /// <returns></returns>
    public NPCDataInfo GetNPCDataInfo(string sceneName,int npcID)
    {
        NPCDataInfoCollection npcDataInfoCollection = npcDataInfoCollections.FirstOrDefault(temp => string.Equals(temp.sceneName, sceneName));
        if (npcDataInfoCollection != null)
            return npcDataInfoCollection.NPCDataInfos.Where(temp => temp.NPCID == npcID).FirstOrDefault();
        return null;
    }

    /// <summary>
    /// 通过场景名获取该场景的所有NPC信息
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public NPCDataInfo[] GetNPCDataInfos(string sceneName)
    {
        NPCDataInfoCollection npcDataInfoCollection = npcDataInfoCollections.FirstOrDefault(temp => string.Equals(temp.sceneName, sceneName));
        if (npcDataInfoCollection != null && npcDataInfoCollection.NPCDataInfos != null)
            return npcDataInfoCollection.NPCDataInfos.ToArray();
        return new NPCDataInfo[0];
    }
}

/// <summary>
/// npc信息集合(一个场景一个该对象)
/// </summary>
[Serializable]
public class NPCDataInfoCollection
{
    /// <summary>
    /// NPC信息
    /// </summary>
    public List<NPCDataInfo> NPCDataInfos;

    /// <summary>
    /// 场景名
    /// </summary>
    public string sceneName;

    public NPCDataInfoCollection()
    {
        NPCDataInfos = new List<NPCDataInfo>();
    }
}

/// <summary>
/// 单个NPC信息
/// </summary>
[Serializable]
public class NPCDataInfo
{
    /// <summary>
    /// 保存npc预设提的路径
    /// </summary>
    [JsonIgnore]
    public static string npcPrefabDirectoryPath = "Prefabs";

    /// <summary>
    /// npc的类型
    /// </summary>
    public EnumNPCType NPCType;
    /// <summary>
    /// npc的id
    /// </summary>
    public int NPCID;
    /// <summary>
    /// npc的名字
    /// </summary>
    public string NPCName;
    /// <summary>
    /// npc的位置(分量x)
    /// </summary>
    [JsonProperty]
    private float locationX;
    /// <summary>
    /// npc的位置(分量y)
    /// </summary>
    [JsonProperty]
    private float locationY;
    /// <summary>
    /// npc的位置(分量z)
    /// </summary>
    [JsonProperty]
    private float locationZ;
    /// <summary>
    /// npc的位置
    /// </summary>
    [JsonIgnore]
    public Vector3 NPCLocation
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
    /// npc的角度(分量x)
    /// </summary>
    [JsonProperty]
    private float angleX;
    /// <summary>
    /// npc的角度(分量y)
    /// </summary>
    [JsonProperty]
    private float angleY;
    /// <summary>
    /// npc的角度(分量z)
    /// </summary>
    [JsonProperty]
    private float angleZ;
    /// <summary>
    /// npc的角度
    /// </summary>
    [JsonIgnore]
    public Vector3 NPCAngle
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
    /// 用于查找npc图标的唯一id
    /// </summary>
    public string npcSpriteID;
    /// <summary>
    /// npc在地图上的图标
    /// </summary>
    [JsonIgnore]
    public Sprite NPCSprite { get; set; }
    /// <summary>
    /// npc所在场景
    /// </summary>
    public string SceneName;
    /// <summary>
    /// npc的预设提名字
    /// </summary>
    public string npcPrefabName;
    /// <summary>
    /// npc的预设提对象
    /// </summary>
    [JsonIgnore]
    private GameObject npcObjPrefab;
    /// <summary>
    /// 游戏对象
    /// </summary>
    [JsonIgnore]
    public GameObject NPCObj { get; set; }
    /// <summary>
    /// 附加的数据
    /// </summary>
    public string OtherValue;

    public void Load()
    {
        if (npcObjPrefab == null)
            if (!string.IsNullOrEmpty(npcPrefabName))
                npcObjPrefab = Resources.Load<GameObject>(npcPrefabDirectoryPath + "/" + npcPrefabName);
        if (NPCObj == null && npcObjPrefab != null)
        {
            NPCObj = GameObject.Instantiate<GameObject>(npcObjPrefab);
            NPCObj.transform.position = NPCLocation;
            NPCObj.transform.eulerAngles = NPCAngle;
            NPCObj.name = NPCName;
            NPCObj.AddComponent<NPCDataInfoMono>().NPCDataInfo = this;
        }
        if (NPCSprite == null)
            if (!string.IsNullOrEmpty(npcSpriteID))
                NPCSprite = SpriteManager.GetSrpite(npcSpriteID);

    }
}

/// <summary>
/// npc的数据(用于挂在在对象身上)
/// </summary>
public class NPCDataInfoMono : MonoBehaviour
{
    public NPCDataInfo NPCDataInfo;
}

/// <summary>
/// npc类型
/// </summary>
public enum EnumNPCType
{
    /// <summary>
    /// 普通
    /// </summary>
    Normal,
    /// <summary>
    /// 商人 
    /// </summary>
    Businessman,
    /// <summary>
    /// 合成人
    /// </summary>
    Synthesiser,
    /// <summary>
    /// 打造人
    /// </summary>
    Forge,
    /// <summary>
    /// 佣兵提交
    /// </summary>
    Mercenarier,
    /// <summary>
    /// 路牌
    /// </summary>
    Street
}