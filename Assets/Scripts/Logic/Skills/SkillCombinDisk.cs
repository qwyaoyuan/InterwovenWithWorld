using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 技能组合盘
/// </summary>
public class SkillCombinDisk
{
    /// <summary>
    /// 技能组合盘静态私有对象
    /// </summary>
    private static SkillCombinDisk instance;
    /// <summary>
    /// 技能组合盘的单例对象
    /// </summary>
    public static SkillCombinDisk Intance
    {
        get
        {
            if (instance == null) instance = new SkillCombinDisk();
            return instance;
        }
    }
    /// <summary>
    /// 技能组合盘的私有构造函数
    /// </summary>
    private SkillCombinDisk() { }

    /// <summary>
    /// 获取指定技能的下一阶段可组合技能
    /// </summary>
    /// <param name="fromSkill"></param>
    /// <returns></returns>
    public SkillBaseStruct[] GetNextSkill(params SkillBaseStruct[] fromSkill)
    {
        if (fromSkill.Length == 0)
            return SkillStructData.Instance.GetSkillDatas(temp => temp.skillType > EnumSkillType.MagicCombinedLevel1Start && temp.skillType < EnumSkillType.MagicCombinedLevel1End);
        if (fromSkill.Length == 1)
        {
            SkillBaseStruct skill1 = fromSkill[0];
            SkillBaseStruct[] skills2 = SkillStructData.Instance.GetSkillDatas(temp => temp.skillType > EnumSkillType.MagicCombinedLevel2Start && temp.skillType < EnumSkillType.MagicCombinedLevel2End);
            skills2 = skills2.Where(temp => GetCanCombinSkill(skill1, temp)).ToArray();
            return skills2;
        }
        if (fromSkill.Length == 2)
        {
            SkillBaseStruct skill1 = fromSkill[0];
            SkillBaseStruct skill2 = fromSkill[1];
            SkillBaseStruct[] skills3 = SkillStructData.Instance.GetSkillDatas(temp => temp.skillType > EnumSkillType.MagicCombinedLevel3Start && temp.skillType < EnumSkillType.MagicCombinedLevel3End);
            skills3 = skills3.Where(temp => GetCanCombinSkill(skill1, skill2, temp)).ToArray();
            return skills3;
        }
        if (fromSkill.Length == 3)
        {
            SkillBaseStruct skill1 = fromSkill[0];
            SkillBaseStruct skill2 = fromSkill[1];
            SkillBaseStruct skill3 = fromSkill[2];
            SkillBaseStruct[] skills4 = SkillStructData.Instance.GetSkillDatas(temp => temp.skillType > EnumSkillType.MagicCombinedLevel4Start && temp.skillType < EnumSkillType.MagicCombinedLevel4End);
            skills4 = skills4.Where(temp => GetCanCombinSkill(skill1, skill2, skill3, temp)).ToArray();
            return skills4;
        }
        return new SkillBaseStruct[0];
    }

    /// <summary>
    /// 获取技能的可组合性
    /// </summary>
    /// <param name="skills"></param>
    /// <returns></returns>
    public bool GetCanCombinSkill(params SkillBaseStruct[] skills)
    {
        if (skills.Length == 0)
            return true;
        if (skills.Length == 1)
        {
            if (skills[0] == null)
                return false;
            if (skills[0].skillType <= EnumSkillType.MagicCombinedLevel1Start || skills[0].skillType >= EnumSkillType.MagicCombinedLevel1End)
                return false;
            else return true;
        }
        if (skills.Length == 2)
        {
            bool canFirstSkill = GetCanCombinSkill(skills[0]);
            if (skills[1] == null)
                return false;
            if (skills[1].skillType <= EnumSkillType.MagicCombinedLevel2Start || skills[1].skillType >= EnumSkillType.MagicCombinedLevel2End)
                return false;
            else return true;
        }
        if (skills.Length == 3)
        {
            bool canSecondSkill = GetCanCombinSkill(skills[0], skills[1]);
            if (skills[2] == null)
                return false;
            if (skills[2].skillType <= EnumSkillType.MagicCombinedLevel3Start || skills[2].skillType >= EnumSkillType.MagicCombinedLevel3End)
                return false;
            else
            {
                //其他条件

                return true;
            }
        }
        if (skills.Length == 4)
        {
            bool canThirdSkill = GetCanCombinSkill(skills[0], skills[1], skills[2]);
            if (skills[3] == null)
                return false;
            if (skills[3].skillType <= EnumSkillType.MagicCombinedLevel4Start || skills[3].skillType >= EnumSkillType.MagicCombinedLevel4End)
                return false;
            else
            {
                //其他条件

                return true;
            }
        }
        return false;
    }

}
