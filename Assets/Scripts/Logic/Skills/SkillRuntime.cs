using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能运行时数据
/// </summary>
public class SkillRuntime
{
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
    public const int SkillLevelMax = 4;
    private SkillRuntime()
    {
        combineSkills = new SkillBaseStruct[SkillLevelMax];
    }

    /// <summary>
    /// 组合的最终技能
    /// 如果技能释放完成则该项置空
    /// 当按住释放键时，将增加储魔
    /// </summary>
    public SkillBaseStruct combineSkill_End;
    /// <summary>
    /// 正在组合的技能数组
    /// 当松开技能键时，将如果技能可以添加则添加到数组中
    /// 当按下释放键时，将技能组合后存放到combineSkill_End中
    /// </summary>
    public SkillBaseStruct[] combineSkills;


    /// <summary>
    /// 从组合技能数组中获取空对象下标
    /// </summary>
    /// <returns></returns>
    public int GetNullIndexByCombineSkills()
    {
        for (int i = 0; i < SkillLevelMax; i++)
        {
            if (combineSkills[i] == null)
                return i ;
        }
        return 4;
    }

    /// <summary>
    /// 将技能设置到最后一个空位
    /// </summary>
    /// <param name="target"></param>
    public void SetToCombineSkill(SkillBaseStruct target)
    {
        int index = GetNullIndexByCombineSkills();
        if (index < 4)
        {
            combineSkills[index] = target;
        }
    }

    /// <summary>
    /// 清空组合技能数组
    /// </summary>
    public void ClearCombineSkills()
    {
        for (int i = 0; i < SkillLevelMax; i++)
        {
            combineSkills[i] = null;
        }
    }
}
