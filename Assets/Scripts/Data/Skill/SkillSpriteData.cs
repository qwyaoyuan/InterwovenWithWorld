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
    /// 保存技能图标信息的路径
    /// </summary>
    public static string dataFilePath = "Data/SkillSprite/SkillSprite";

    /// <summary>
    /// 技能类型关联技能图标的字典
    /// </summary>
    static Dictionary<EnumSkillType, Sprite> skillTypeToSpriteDic;

    /// <summary>
    /// 技能类型关联技能图标ID的字典
    /// </summary>
    static Dictionary<EnumSkillType, string> skillTypeToSpriteIDDic;

    static SkillSpriteData()
    {
        skillTypeToSpriteDic = new Dictionary<EnumSkillType, Sprite>();
        TextAsset textAsset = Resources.Load<TextAsset>(SkillSpriteData.dataFilePath);//加载技能图标数据
        string assetText = Encoding.UTF8.GetString(textAsset.bytes);
        skillTypeToSpriteIDDic = DeSerializeNow<Dictionary<EnumSkillType, string>>(assetText);
        foreach (KeyValuePair<EnumSkillType,string> item in skillTypeToSpriteIDDic)
        {
            Sprite sprite = SpriteManager.GetSrpite(item.Value);
            if (sprite != null)
                skillTypeToSpriteDic.Add(item.Key, sprite);
        }
    }

    /// <summary>
    /// 获取精灵
    /// </summary>
    /// <param name="enumSkillType">技能类型</param>
    /// <returns></returns>
    public static Sprite GetSprite(EnumSkillType enumSkillType)
    {
        if (skillTypeToSpriteDic.ContainsKey(enumSkillType))
            return skillTypeToSpriteDic[enumSkillType];
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
