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
    /// 技能结构字典
    /// </summary>
    private Dictionary<int, SkillBaseStruct> skillStructDatas;

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
        //如果此时还未读取或者必须读取
        if (must || skillStructDatas == null)
        {
            skillStructDatas = new Dictionary<int, SkillBaseStruct>();
            //从文件中读取
        }
    }

    /// <summary>
    /// 通过技能id查找数据结构
    /// </summary>
    /// <param name="id">技能id</param>
    /// <returns></returns>
    public SkillBaseStruct GetSkillBaseStruct(int id)
    {
        if (skillStructDatas != null && skillStructDatas.ContainsKey(id))
            return skillStructDatas[id];
        return null;
    }

    /// <summary>
    /// 通过筛选器查找数据结构
    /// </summary>
    /// <param name="selecter"></param>
    /// <returns></returns>
    public SkillBaseStruct GetSkillBaseStruct(Func<SkillBaseStruct, bool> selecter)
    {
        return skillStructDatas.Where(temp => selecter(temp.Value)).Select(temp=>temp.Value).FirstOrDefault();
    }
}
