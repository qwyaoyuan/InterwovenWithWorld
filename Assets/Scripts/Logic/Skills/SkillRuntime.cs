using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// 技能运行时数据
/// </summary>
public class SkillRuntime
{
    /// <summary>
    /// 组合技能的基础值
    /// </summary>
    const int combineBaseNum = 100000000;

    /// <summary>
    /// 技能运行时数据静态私有对象
    /// </summary>
    private static SkillRuntime instance;
    public static SkillRuntime Instance
    {
        get
        {
            if (instance == null) instance = new SkillRuntime();
            return instance;
        }
    }
    private SkillRuntime()
    {
        skillBaseStructList = new List<SkillBaseStruct>();
    }

    /// <summary>
    /// 技能临时集合
    /// </summary>
    List<SkillBaseStruct> skillBaseStructList;
    /// <summary>
    /// 储蓄魔力时间
    /// </summary>
    float savingMagicPowerTime;

    /// <summary>
    /// 添加一个技能到运行时栏位
    /// </summary>
    /// <param name="skillEnum">技能</param>
    /// <returns></returns>
    public bool SetSkill(int skillEnum)
    {
        if (skillEnum > combineBaseNum)
        {
            int skill1Num = skillEnum % 100+1000;skillEnum /= 100;
            int skill2Num = skillEnum % 100+1100;skillEnum /= 100;
            int skill3Num = skillEnum & 100+1200;skillEnum /= 100;
            int skill4Num = skillEnum % 100+1300;skillEnum /= 100;
            SkillBaseStruct[] addSkillBaseStructArray = (new[] { skill1Num, skill2Num, skill3Num, skill4Num })
                .Select(temp => (EnumSkillType)temp)
                .Select(temp => SkillStructData.Instance.SearchSkillDatas(temp1 => temp1.skillType == temp).FirstOrDefault())
                .Where(temp => temp != null)
                .ToArray();
            List<SkillBaseStruct> tempSkillBaseStruct = new List<SkillBaseStruct>(skillBaseStructList.ToArray());
            tempSkillBaseStruct.AddRange(addSkillBaseStructArray);
            bool result = SkillCombinDisk.Intance.GetCanCombinSkill(tempSkillBaseStruct.ToArray());
            skillBaseStructList.AddRange(addSkillBaseStructArray);
            return result;
        }
        else if(skillEnum> (int)EnumSkillType.MagicCombinedLevel1Start && skillEnum<(int)EnumSkillType.MagicCombinedLevel4End)
        {
            List<SkillBaseStruct> tempSkillBaseStruct = new List<SkillBaseStruct>(skillBaseStructList.ToArray());
            SkillBaseStruct skillBaseStruct = SkillStructData.Instance.SearchSkillDatas(temp => temp.skillType == (EnumSkillType)skillEnum).FirstOrDefault();
            tempSkillBaseStruct.Add(skillBaseStruct);
            bool result = SkillCombinDisk.Intance.GetCanCombinSkill(tempSkillBaseStruct.ToArray());
            skillBaseStructList.Add(skillBaseStruct);
            return result;
        }
        return false;
    }

    /// <summary>
    /// 清理当前的技能临时集合以及储蓄魔力时间
    /// </summary>
    public void ClearSkillsData()
    {
        skillBaseStructList.Clear();
    }

    /// <summary>
    /// 获取当前技能
    /// </summary>
    /// <returns></returns>
    public SkillBaseStruct[] GetSkills()
    {
        return skillBaseStructList.ToArray();
    }

    /// <summary>
    /// 储蓄魔力时间
    /// </summary>
    public float SavingMagicPowerTime
    {
        get { return savingMagicPowerTime; }
        set { savingMagicPowerTime = value; }
    }
}
