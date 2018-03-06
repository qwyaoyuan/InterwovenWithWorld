using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 地图数据(场景地图的图片,场景的比例)
/// </summary>
public class MapData : ILoadable<MapData>
{
    /// <summary>
    /// 保存地图数据的路径
    /// </summary>
    public static string dataFilePath = "Data/Map/Map";

    /// <summary>
    /// 场景对应地图数据字典
    /// </summary>
    Dictionary<string, MapDataInfo> sceneToMapDataDic;

    public void Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(dataFilePath);
        string assetText = Encoding.UTF8.GetString(textAsset.bytes);
        sceneToMapDataDic = DeSerializeNow<Dictionary<string, MapDataInfo>>(assetText);
    }

    /// <summary>
    /// 通过场景名获取地图信息
    /// 如果该场景没有地图信息则返回null
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <returns></returns>
    public MapDataInfo this[string sceneName]
    {
        get
        {
            if (sceneToMapDataDic != null && sceneToMapDataDic.ContainsKey(sceneName))
                return sceneToMapDataDic[sceneName];
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
/// 地图数据(对应每个场景的)
/// </summary>
[Serializable]
public class MapDataInfo
{
    /// <summary>
    /// 场景名
    /// </summary>
    [JsonProperty]
    private string sceneName;
    /// <summary>
    /// 场景名
    /// </summary>
    [JsonIgnore]
    public string SceneName { get { return sceneName; } }

    /// <summary>
    /// 地图图片的唯一id
    /// </summary>
    [JsonProperty]
    private string mapSpriteID;
    /// <summary>
    /// 地图的显示精灵
    /// </summary>
    [JsonIgnore]
    private Sprite mapSprite;
    [JsonIgnore]
    public Sprite MapSprite
    {
        get { return mapSprite; }
    }

    /// <summary>
    /// 场景的边界X
    /// </summary>
    [JsonProperty] private float sceneRectX;
    /// <summary>
    /// 场景的边界Y
    /// </summary>
    [JsonProperty] private float sceneRectY;
    /// <summary>
    /// 场景的边界W
    /// </summary>
    [JsonProperty] private float sceneRectW;
    /// <summary>
    /// 场景的边界H
    /// </summary>
    [JsonProperty] private float sceneRectH;
    /// <summary>
    /// 场景的边界
    /// </summary>
    [JsonIgnore]
    public Rect SceneRect
    {
        get
        {
            return new Rect(sceneRectX, sceneRectY, sceneRectW, sceneRectH);
        }
        private set
        {
            sceneRectX = value.x;
            sceneRectY = value.y;
            sceneRectW = value.width;
            sceneRectH = value.height;
        }
    }

    public void Load(bool init = false)
    {
        if (mapSprite == null || init)
            if (!string.IsNullOrEmpty(mapSpriteID))
                mapSprite = SpriteManager.GetSrpite(mapSpriteID);
    }
}
