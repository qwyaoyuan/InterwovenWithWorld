using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能处理类
/// </summary>
public class SkillDealHandle
{
    /// <summary>
    /// 技能处理类的私有静态对象
    /// </summary>
    private static SkillDealHandle instance;
    /// <summary>
    /// 技能处理类的单例对象
    /// </summary>
    public static SkillDealHandle Instance
    {
        get
        {
            if (instance == null) instance = new SkillDealHandle();
            return instance;
        }
    }
    /// <summary>
    /// 私有构造函数
    /// </summary>
    private SkillDealHandle() { }

    /// <summary>
    /// 此时是否可以解决技能
    /// </summary>
    public bool CanDealSkill
    {
        get { return false; }
    }

    /// <summary>
    /// 处理组合技能(开始)，此时开始读条
    /// </summary>
    /// <param name="skillBaseStructs">技能数组</param>
    public void BeginCombineSkill(SkillBaseStruct[] skillBaseStructs)
    {

    }

    /// <summary>
    /// 结束组合技能(结束),此时只设置一个开关，只有读条完毕才可以释放技能
    /// </summary>
    /// <param name="savingMagicPowerTime">储蓄魔力时间</param>
    public void EndCombineSkill(float savingMagicPowerTime)
    {

    }
}
