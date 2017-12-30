using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

/// <summary>
/// 技能结构数据
/// </summary>
public class SkillStructData : ILoadable<SkillStructData>
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
                //获取其他类型的宏定义
                Type edfineType = typeof(SkillStructConstString);
                FieldInfo[] edfineInfos = edfineType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                Dictionary<string, string> edfineNameToValueDic = edfineInfos.ToDictionary(
                    temp => temp.Name,
                    temp => (string)temp.GetValue(null));
                string[] otherSplit = new string[] { "***" };//截取其他数据时所用的分隔符

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
                    EnumSkillType enumSkillType = skillAnalysisData.GetEnum<EnumSkillType>(id, "skillType");
                    SkillBaseStruct skillBaseStruct = null;
                    Type newType = null;
                    //尝试使用该类型构造一个新的类
                    try
                    {
                        newType = Type.GetType("SkillStruct_" + enumSkillType.ToString());
                        skillBaseStruct = Activator.CreateInstance(newType) as SkillBaseStruct;
                    }
                    catch { }
                    if (skillBaseStruct == null)
                        skillBaseStruct = new SkillBaseStruct();
                    //加载其他属性
                    if (newType != null)
                    {
                        FieldInfo[] otherFieldInfos = newType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        foreach (FieldInfo otherFieldInfo in otherFieldInfos)
                        {
                            string otherFieldName = otherFieldInfo.Name;
                            if (edfineNameToValueDic.ContainsKey(otherFieldName))
                            {
                                string otherFieldKey = edfineNameToValueDic[otherFieldName];
                                int[] otherFieldValues = skillAnalysisData.GetValues<int>(id, otherFieldKey);
                                if (otherFieldValues != null && otherFieldValues.Length > 0)
                                {
                                    otherFieldInfo.SetValue(skillBaseStruct, otherFieldValues[0]);
                                }
                            }
                        }
                    }
                    //加载常规属性
                    skillBaseStructs[i] = skillBaseStruct;
                    skillBaseStruct.id = id;
                    skillBaseStruct.name = skillAnalysisData.GetValue<string>(id, "skillName");
                    skillBaseStruct.skillType = enumSkillType;
                    skillBaseStruct.skillMode = skillAnalysisData.GetEnum<EnumReleaseMode>(id, "releaseMode");
                    skillBaseStruct.skillZones = skillAnalysisData.GetEnums<EnumSkillZone>(id, "correlationZone").Where(temp => temp != EnumSkillZone.None).ToArray();
                    //加载技能图标
                    skillBaseStruct.skillSprite = SkillSpriteData.GetSprite(skillBaseStruct.skillType);
                    //计算技能名(现在暂定使用元名字)
                    skillBaseStruct.skillName = skillBaseStruct.name;
                    //完成加载粒子
                    skillBaseStruct.skillBelief = skillAnalysisData.GetEnum<EnumSkillBelief>(id, "skillBelief");
                    string[] skillStatusEffectStrs = skillAnalysisData.GetValues<string>(id, "skillStatusEffect").Where(temp => !string.IsNullOrEmpty(temp)).ToArray();
                    skillBaseStruct.skillStatusEffect = new EnumStatusEffect[skillStatusEffectStrs.Length];
                    for (int j = 0; j < skillStatusEffectStrs.Length; j++)
                    {
                        skillBaseStruct.skillStatusEffect[j] = (EnumStatusEffect)Enum.Parse(typeof(EnumStatusEffect), skillStatusEffectStrs[j]);
                    }
                    //技能前置
                    skillBaseStruct.skillPrecondition = new SkillPrecondition();
                    skillBaseStruct.skillPrecondition.mustSkillZonePointDic = new Dictionary<EnumSkillZone, int>();
                    skillBaseStruct.skillPrecondition.mustSkillPointDic = new Dictionary<EnumSkillType, int>();
                    EnumSkillZone[] preconditionSkillZones = skillAnalysisData.GetEnums<EnumSkillZone>(id, "correlationBeforeZone");//前置技能组数组
                    int[] preconditionSkillZoneNums = skillAnalysisData.GetValues<int>(id, "correlationBeforeZoneCount");//前置技能组加点
                    int preconditionSkillZoneCount = preconditionSkillZones.Length < preconditionSkillZoneNums.Length ? preconditionSkillZones.Length : preconditionSkillZoneNums.Length;
                    for (int j = 0; j < preconditionSkillZoneCount; j++)
                    {
                        if (preconditionSkillZones[j] != EnumSkillZone.None)
                            skillBaseStruct.skillPrecondition.mustSkillZonePointDic.Add(preconditionSkillZones[j], preconditionSkillZoneNums[j]);
                    }
                    EnumSkillType[] preconditionSkills = skillAnalysisData.GetEnums<EnumSkillType>(id, "correlationBeforeSkill");//前置技能数组
                    int[] preconditionSkillNums = skillAnalysisData.GetValues<int>(id, "correlationBeforeSkillCount");//前置技能加点
                    int preconditionSkillCount = preconditionSkillZones.Length < preconditionSkillNums.Length ? preconditionSkillZones.Length : preconditionSkillNums.Length;
                    for (int j = 0; j < preconditionSkillCount; j++)
                    {
                        if (preconditionSkills[j] != EnumSkillType.None)
                            skillBaseStruct.skillPrecondition.mustSkillPointDic.Add(preconditionSkills[j], preconditionSkillNums[j]);
                    }
                    //技能的技能等级以及属性
                    skillBaseStruct.maxLevel = skillAnalysisData.GetValue<int>(id, "skillLevel");
                    skillBaseStruct.skillAttributeStructs = new SkillAttributeStruct[skillBaseStruct.maxLevel];
                    Dictionary<string, Array> skillAttributeStructDic = new Dictionary<string, Array>();
                    Type skillAttributeStructType = typeof(SkillAttributeStruct);
                    FieldInfo[] skillAttributeStructFieldInfos = skillAttributeStructType.GetFields();
                    foreach (FieldInfo fieldInfo in skillAttributeStructFieldInfos)
                    {
                        FieldExplanAttribute fieldExplan = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
                        if (fieldExplan == null)
                            continue;
                        object[] skillAttributeStructValue = skillAnalysisData.GetValues(fieldInfo.FieldType, id, fieldExplan.GetExplan(1));//explan的第一个下标表示说明
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
