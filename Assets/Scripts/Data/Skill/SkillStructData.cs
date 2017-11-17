using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

/// <summary>
/// 技能结构数据
/// </summary>
public class SkillStructData:ILoadable<SkillStructData>
{
 
    /// <summary>
    /// 技能结构对象数组 
    /// </summary>
    private SkillBaseStruct[] skillBaseStructs;
    /// <summary>
    /// 技能数据解析对象
    /// </summary>
    private SkillAnalysisData skillAnalysisData;

    /// <summary>
    /// 技能结构数据
    /// </summary>
    public SkillStructData()
    {
        skillAnalysisData = new SkillAnalysisData();
    }

    /// <summary>
    /// 实现Iloadable接口
    /// </summary>
    public void Load()
    {
        ReadSkillStructData(true);
    }

    /// <summary>
    /// 从文件读取技能结构数据
    /// </summary>
    /// <param name="must">是否必须读取</param>
    public void ReadSkillStructData(bool must = false)
    {
        if (skillBaseStructs == null || must)
        {
            TextAsset skillPathTextAsset = Resources.Load<TextAsset>("Data/Skill/Skills");
            if (skillPathTextAsset == null)
                skillBaseStructs = new SkillBaseStruct[0];
            else
            {
                string[] splits = new string[] { "^^^" };
                string[] skillPaths = skillPathTextAsset.text.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(temp => temp.Split(splits, StringSplitOptions.RemoveEmptyEntries))
                    .Where(temp => temp != null && temp.Length == 3)
                    .Select(temp => temp[2]).ToArray();
                string[] skillValues = skillPaths
                    .Select(temp => Resources.Load<TextAsset>("Data/Skill/" + temp))
                    .Where(temp => temp != null)
                    .Select(temp => temp.text)
                    .ToArray();
                skillAnalysisData.AnalysisData(skillValues);
                string[] ids = skillAnalysisData.GetIDArray();
                skillBaseStructs = new SkillBaseStruct[ids.Length];
                for (int i = 0; i < ids.Length; i++)
                {
                    string id = ids[i];
                    SkillBaseStruct skillBaseStruct = new SkillBaseStruct();
                    skillBaseStructs[i] = skillBaseStruct;
                    skillBaseStruct.id = id;
                    skillBaseStruct.name = skillAnalysisData.GetValue<string>(id, "skillName");
                    skillBaseStruct.skillType = skillAnalysisData.GetEnum<EnumSkillType>(id, "skillType");
                    skillBaseStruct.skillMode = skillAnalysisData.GetEnum<EnumReleaseMode>(id, "releaseMode");
                    string[] particalsNames = skillAnalysisData.GetValues<string>(id, "particleNames").Where(temp => !string.IsNullOrEmpty(temp)).ToArray();
                    skillBaseStruct.particals = new GameObject[particalsNames.Length];
                    //加载粒子 

                    //完成加载粒子
                    skillBaseStruct.skillBelief = skillAnalysisData.GetEnum<EnumSkillBelief>(id, "skillBelief");
                    string[] skillStatusEffectStrs = skillAnalysisData.GetValues<string>(id, "skillStatusEffect").Where(temp => !string.IsNullOrEmpty(temp)).ToArray();
                    skillBaseStruct.skillStatusEffect = new EnumStatusEffect[skillStatusEffectStrs.Length];
                    for (int j = 0; j < skillStatusEffectStrs.Length; j++)
                    {
                        skillBaseStruct.skillStatusEffect[j] = (EnumStatusEffect)Enum.Parse(typeof(EnumStatusEffect), skillStatusEffectStrs[j]);
                    }
                    skillBaseStruct.maxLevel = skillAnalysisData.GetValue<int>(id, "skillLevel");
                    skillBaseStruct.skillAttributeStructs = new SkillAttributeStruct[skillBaseStruct.maxLevel];
                    Dictionary<string, Array> skillAttributeStructDic = new Dictionary<string, Array>();
                    Type skillAttributeStructType = typeof(SkillAttributeStruct);
                    FieldInfo[] skillAttributeStructFieldInfos = skillAttributeStructType.GetFields();
                    foreach (FieldInfo fieldInfo in skillAttributeStructFieldInfos)
                    {
                        object[] skillAttributeStructValue = skillAnalysisData.GetValues(fieldInfo.FieldType, id, fieldInfo.Name);
                        if (skillAttributeStructValue.Length == skillBaseStruct.maxLevel)
                        {
                            skillAttributeStructDic.Add(fieldInfo.Name, skillAttributeStructValue);
                        }
                    }
                    for (int j = 0; j < skillBaseStruct.maxLevel; j++)
                    {
                        SkillAttributeStruct skillAttributeStruct = new SkillAttributeStruct();
                        foreach (FieldInfo fieldInfo in skillAttributeStructFieldInfos)
                        {
                            if (skillAttributeStructDic.ContainsKey(fieldInfo.Name))
                            {
                                if (skillAttributeStructDic[fieldInfo.Name].GetValue(j) == null)
                                {
                                    if (j > 0)
                                        skillAttributeStructDic[fieldInfo.Name].SetValue(skillAttributeStructDic[fieldInfo.Name].GetValue(j - 1), j);
                                    else continue;
                                }
                                fieldInfo.SetValue(skillAttributeStruct, skillAttributeStructDic[fieldInfo.Name].GetValue(j));
                            }
                        }
                        skillBaseStruct.skillAttributeStructs[j] = skillAttributeStruct;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 使用指定的选择器获取技能数据
    /// </summary>
    /// <param name="selector">选择器</param>
    /// <returns></returns>
    public SkillBaseStruct[] SearchSkillDatas(Func<SkillBaseStruct, bool> selector = null)
    {
        if (selector == null)
            return skillBaseStructs;
        return skillBaseStructs.Where(selector).ToArray();
    }
}
