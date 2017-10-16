using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 技能结构数据
/// </summary>
public class SkillStructData
{
    /// <summary>
    /// 技能结构数据的静态私有对象
    /// </summary>
    private static SkillStructData instance;
    /// <summary>
    /// 技能结构数据的单例对象
    /// </summary>
    public static SkillStructData Instance
    {
        get
        {
            if (instance == null) instance = new SkillStructData();
            return instance;
        }
    }

    /// <summary>
    /// 技能结构对象数组 
    /// </summary>
    private SkillBaseStruct[] skillBaseStructs;

    /// <summary>
    /// 技能结构数据
    /// </summary>
    private SkillStructData()
    {
        ReadSkillStructData(true);
    }

    /// <summary>
    /// 从文件读取技能结构数据
    /// </summary>
    /// <param name="must">是否必须读取</param>
    public void ReadSkillStructData(bool must)
    {
        skillBaseStructs = new SkillBaseStruct[0];
    }

    /// <summary>
    /// 使用指定的选择器获取技能数据
    /// </summary>
    /// <param name="selector">选择器</param>
    /// <returns></returns>
    public SkillBaseStruct[] GetSkillDatas(Func<SkillBaseStruct, bool> selector)
    {
        if (selector == null)
            return skillBaseStructs;
        return skillBaseStructs.Where(selector).ToArray();
    }
}
