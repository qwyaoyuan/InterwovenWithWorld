using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 技能运行时状态接口
/// </summary>
public interface ISkillState : IBaseState
{
    /// <summary>
    /// 获取或设置组合技能
    /// 设置:如果传入的长度为0,则清空,否则如果可以组合则附加,如果无法组合则替换;如果处于蓄力阶段,则无法设置新的技能
    /// 获取:返回当前组合的技能数组
    /// </summary>
    SkillBaseStruct[] CombineSkills { get; set; }

    /// <summary>
    /// 开始按住释放魔法键(用于初始化计时)
    /// </summary>
    /// <returns>是否可以释放该技能</returns>
    bool StartCombineSkillRelease();
    /// <summary>
    /// 结束按住(松开)释放魔法键(用于结束计时并释放)
    /// </summary>
    /// <returns>是否可以释放该技能</returns>
    bool EndCombineSkillRelease();
    /// <summary>
    /// 释放普通技能,如果正在释放其他技能则无法释放 
    /// </summary>
    /// <param name="skillBaseStruct"></param>
    /// <returns>是否可以释放该技能</returns>
    bool ReleaseNormalSkill(SkillBaseStruct skillBaseStruct);
}
