using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Linq;

/// <summary>
/// 粒子管理器
/// </summary>
public class ParticalManager
{
    /// <summary>
    /// 粒子对象配置所在的文件路径
    /// </summary>
    public static string particalObjDirectoryPath = "Data/Partical/Partical";

    /// <summary>
    /// 名字与粒子名的对应关系
    /// </summary>
    static Dictionary<string, GameObject> nameToPrefabDic;

    /// <summary>
    /// 是否已经加载了
    /// </summary>
    static bool loaded;

    static ParticalManager()
    {
        loaded = false;
    }

    /// <summary>
    /// 关闭加载资源携程
    /// </summary>
    static Action CloseEnumerableLoad;

    /// <summary>
    /// 使用携程加载资源
    /// 传入关闭携程函数
    /// 返回加载携程对象
    /// </summary>
    /// <param name="returnLoad">调用返回加载携程</param>
    /// <param name="CloseEnumerableLoad">调用关闭携程</param>
    public static void Load(Action<IEnumerable> returnLoad, Action CloseEnumerableLoad)
    {
        if (ParticalManager.CloseEnumerableLoad != null)
        {
            try
            {
                ParticalManager.CloseEnumerableLoad();
            }
            catch { }
            ParticalManager.CloseEnumerableLoad = null;
        }
        ParticalManager.CloseEnumerableLoad = CloseEnumerableLoad;
        if (returnLoad != null)
        {
            returnLoad(EnumerableLoad());
        }
    }

    /// <summary>
    /// 使用携程加载资源
    /// </summary>
    /// <returns></returns>
    static IEnumerable EnumerableLoad()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(particalObjDirectoryPath);
        string allLine = Encoding.UTF8.GetString(textAsset.bytes);
        string[] lines = allLine.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        nameToPrefabDic = new Dictionary<string, GameObject>();
        foreach (string line in lines)
        {
            try
            {
                GameObject obj = Resources.Load<GameObject>(line.Trim());
                nameToPrefabDic.Add(obj.name, obj);
            }
            catch (Exception ex) { Debug.Log(ex); }
            yield return null;
        }
        loaded = true;
    }

    static void Load()
    {
        if (ParticalManager.CloseEnumerableLoad != null)
        {
            try
            {
                ParticalManager.CloseEnumerableLoad();
            }
            catch { }
            ParticalManager.CloseEnumerableLoad = null;
        }
        TextAsset textAsset = Resources.Load<TextAsset>(particalObjDirectoryPath);
        string allLine = Encoding.UTF8.GetString(textAsset.bytes);
        string[] lines = allLine.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        nameToPrefabDic = new Dictionary<string, GameObject>();
        foreach (string line in lines)
        {
            try
            {
                GameObject obj = Resources.Load<GameObject>(line.Trim());
                nameToPrefabDic.Add(obj.name, obj);
            }
            catch (Exception ex) { Debug.Log(ex); }
        }
        loaded = true;
    }

    /// <summary>
    /// 增加加载
    /// </summary>
    public static void IncrementalLoad()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(particalObjDirectoryPath);
        string allLine = Encoding.UTF8.GetString(textAsset.bytes);
        string[] lines = allLine.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        char[] splits = new char[] { '/' };
        if (nameToPrefabDic == null)
            nameToPrefabDic = new Dictionary<string, GameObject>();
        foreach (string line in lines)
        {
            try
            {
                string name = line.Split(splits, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                if (name != null && !nameToPrefabDic.ContainsKey(name))
                {
                    GameObject obj = Resources.Load<GameObject>(line.Trim());
                    nameToPrefabDic.Add(obj.name, obj);
                }
            }
            catch (Exception ex) { Debug.Log(ex); }
        }
    }


    /// <summary>
    /// 根据名字查找粒子
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject GetPartical(string name)
    {
        if (!loaded)
            Load();
        if (name == null)
            return null;
        if (nameToPrefabDic.ContainsKey(name))
            return nameToPrefabDic[name];
        return null;
    }

    /// <summary>
    /// 通过对象查找名字 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetName(GameObject obj)
    {
        string name = nameToPrefabDic.Where(temp => temp.Value == obj).Select(temp => temp.Key).FirstOrDefault();
        return name;
    }
}
