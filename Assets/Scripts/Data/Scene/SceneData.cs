using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 场景数据(主要是场景名和该场景的类型)
/// </summary>
public class SceneData : ILoadable<SceneData>
{
    /// <summary>
    /// 保存场景信息的路径
    /// </summary>
    public static string dataDirectoryPath = "Data/Scene/Scene";

    SceneDataInfo[] sceneDataInfos;

    public void Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(dataDirectoryPath);
        string assetName = textAsset.name;
        string assetText = Encoding.UTF8.GetString(textAsset.bytes);
        sceneDataInfos = DeSerializeNow<SceneDataInfo[]>(assetText);
        if (sceneDataInfos == null)
            sceneDataInfos = new SceneDataInfo[0];
    }

    /// <summary>
    /// 获取所有场景信息
    /// </summary>
    /// <returns></returns>
    public SceneDataInfo[] GetAllSceneData()
    {
        return sceneDataInfos.Clone() as SceneDataInfo[];
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

public class SceneDataInfo
{
    /// <summary>
    /// 场景名
    /// </summary>
    public string SceneName;
    /// <summary>
    /// 场景的功能
    /// </summary>
    public EnumSceneAction SceneAction;
}

/// <summary>
/// 场景的功能
/// </summary>
public enum EnumSceneAction
{
    /// <summary>
    /// 开始场景
    /// </summary>
    Start,
    /// <summary>
    /// 游戏场景
    /// </summary>
    Game
}
