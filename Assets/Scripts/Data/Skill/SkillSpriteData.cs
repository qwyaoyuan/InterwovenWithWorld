using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 技能图标数据结构
/// 不能继承ILoadable接口,因为和SkillStructData的加载顺序无法确保
/// </summary>
public static class SkillSpriteData
{
    /// <summary>
    /// 保存技能图标信息的路径(保存的是用于显示的图片)
    /// </summary>
    public static string dataFilePath_Show = "Data/SkillSprite/SkillSprite";

    /// <summary>
    /// 保存技能图标信息的路径(保存的是用于组合的图片信息)
    /// </summary>
    public static string dataFilePath_Combine = "Data/SkillSprite/SkillSprite_Combine";

    /// <summary>
    /// 技能类型关联技能图标的字典(用于显示)
    /// </summary>
    static Dictionary<EnumSkillType, Sprite> skillTypeToSpriteShowDic;

    /// <summary>
    /// 技能类型关联技能图标ID的字典(用于显示)
    /// </summary>
    static Dictionary<EnumSkillType, string> skillTypeToSpriteIDShowDic;

    /// <summary>
    /// 技能类型关联技能图标的字典(用于组合)
    /// </summary>
    static Dictionary<EnumSkillType, Sprite> skillTypeToSpriteCombineDic;

    /// <summary>
    /// 技能类型关联技能图标ID的字典(用于组合)
    /// </summary>
    static Dictionary<EnumSkillType, string> skillTypeToSpriteIDCombineDic;

    static SkillSpriteData()
    {
        //用于显示
        {
            skillTypeToSpriteShowDic = new Dictionary<EnumSkillType, Sprite>();
            TextAsset textAsset = Resources.Load<TextAsset>(SkillSpriteData.dataFilePath_Show);//加载技能图标数据
            string assetText = Encoding.UTF8.GetString(textAsset.bytes);
            skillTypeToSpriteIDShowDic = DeSerializeNow<Dictionary<EnumSkillType, string>>(assetText);
            foreach (KeyValuePair<EnumSkillType, string> item in skillTypeToSpriteIDShowDic)
            {
                Sprite sprite = SpriteManager.GetSrpite(item.Value);
                if (sprite != null)
                    skillTypeToSpriteShowDic.Add(item.Key, sprite);
            }
        }
        //用于组合
        {
            skillTypeToSpriteCombineDic = new Dictionary<EnumSkillType, Sprite>();
            TextAsset textAsset = Resources.Load<TextAsset>(SkillSpriteData.dataFilePath_Combine);//加载技能图标数据
            string assetText = Encoding.UTF8.GetString(textAsset.bytes);
            skillTypeToSpriteIDCombineDic = DeSerializeNow<Dictionary<EnumSkillType, string>>(assetText);
            foreach (KeyValuePair<EnumSkillType, string> item in skillTypeToSpriteIDCombineDic)
            {
                Sprite sprite = SpriteManager.GetSrpite(item.Value);
                if (sprite != null)
                    skillTypeToSpriteCombineDic.Add(item.Key, sprite);
            }
        }
    }

    /// <summary>
    /// 获取精灵(用于显示)
    /// </summary>
    /// <param name="enumSkillType">技能类型</param>
    /// <returns></returns>
    public static Sprite GetSprite(EnumSkillType enumSkillType)
    {
        if (skillTypeToSpriteShowDic.ContainsKey(enumSkillType))
            return skillTypeToSpriteShowDic[enumSkillType];
        return null;
    }

    /// <summary>
    /// 获取精灵(用于组合)
    /// </summary>
    /// <param name="enumSkillType"></param>
    /// <returns></returns>
    public static Sprite GetSpriteCombine(EnumSkillType enumSkillType)
    {
        if (skillTypeToSpriteCombineDic.ContainsKey(enumSkillType))
            return skillTypeToSpriteCombineDic[enumSkillType];
        return null;
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T">反序列化后的类型</typeparam>
    /// <param name="value">字符串</param>
    /// <returns>对象</returns>
    static T DeSerializeNow<T>(string value) where T : class
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
