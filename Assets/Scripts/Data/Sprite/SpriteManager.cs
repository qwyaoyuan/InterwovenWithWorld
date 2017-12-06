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
    public static string spriteDirectoryPath = "Sprites";

    /// <summary>
    /// 字符串与精灵的对应关系
    /// </summary>
    static Dictionary<string, Sprite> strToSpriteDic;

    static SpriteManager()
    {
        strToSpriteDic = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>(spriteDirectoryPath);
        foreach (Sprite sprite in sprites)
        {
            strToSpriteDic.Add(sprite.texture.name + ":" + sprite.name, sprite);
        }
    }

    /// <summary>
    /// 通过名字查找精灵
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Sprite GetSrpite(string name)
    {
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
        string name = strToSpriteDic.Where(temp => temp.Value == sprite).Select(temp => temp.Key).FirstOrDefault();
        return name;
    }
}
