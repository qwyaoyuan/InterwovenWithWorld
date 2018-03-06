using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 精灵管理器
/// 用于关联编辑器与精灵资源之间的对应关系
/// </summary>
public class SpriteManager
{
    /// <summary>
    /// 精灵所在文件夹路径
    /// </summary>
    public static string spriteDirectoryPath = "Sprites";//,Sprites/Skill,Sprites/Skill/融合魔法";

    /// <summary>
    /// 字符串与精灵的对应关系
    /// </summary>
    static Dictionary<string, Sprite> strToSpriteDic;

    /// <summary>
    /// 是否已经加载完毕
    /// </summary>
    static bool loaded;

    static SpriteManager()
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
        if (SpriteManager.CloseEnumerableLoad != null)
        {
            try
            {
                SpriteManager.CloseEnumerableLoad();
            }
            catch { }
            SpriteManager.CloseEnumerableLoad = null;
        }
        SpriteManager.CloseEnumerableLoad = CloseEnumerableLoad;
        if (returnLoad != null)
        {
            returnLoad(EnumerableLoad());
        }
    }

    /// <summary>
    /// 获取资源
    /// </summary>
    /// <returns></returns>
    static Sprite[] GetResources()
    {
        string[] paths = spriteDirectoryPath.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        List<Sprite> resultList = new List<Sprite>();
        foreach (string path in paths)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(path);
            resultList.AddRange(sprites);
        }
        return resultList.ToArray(); 
    }

    static IEnumerable EnumerableLoad()
    {
        strToSpriteDic = new Dictionary<string, Sprite>();
        Sprite[] sprites = GetResources();//Resources.LoadAll<Sprite>(spriteDirectoryPath);
        int count = 0;
        foreach (Sprite sprite in sprites)
        {
            strToSpriteDic.Add(sprite.texture.name + ":" + sprite.name, sprite);
            count++;
            if (count > 5)
            {
                count = 0;
                yield return null;
            }
        }
        loaded = true;
    }

    public static void Load()
    {
        if (SpriteManager.CloseEnumerableLoad != null)
        {
            try
            {
                SpriteManager.CloseEnumerableLoad();
            }
            catch { }
            SpriteManager.CloseEnumerableLoad = null;
        }
        strToSpriteDic = new Dictionary<string, Sprite>();
        Sprite[] sprites = GetResources();//Resources.LoadAll<Sprite>(spriteDirectoryPath);
        foreach (Sprite sprite in sprites)
        {
            strToSpriteDic.Add(sprite.texture.name + ":" + sprite.name, sprite);
        }
        loaded = true;
    }

    /// <summary>
    /// 通过名字查找精灵
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Sprite GetSrpite(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;
        if (!loaded)
            Load();
        if (strToSpriteDic.ContainsKey(name))
            return strToSpriteDic[name];
        return null;
    }

    /// <summary>
    /// 通过精灵查找名字
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    public static string GetName(Sprite sprite)
    {

        if (!loaded)
            Load();
        string name = strToSpriteDic.Where(temp => temp.Value == sprite).Select(temp => temp.Key).FirstOrDefault();
        return name;
    }
}
