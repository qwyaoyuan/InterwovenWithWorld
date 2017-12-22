using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 与伤害相关的接口,包括提供粒子的初始化数据,伤害回馈后调用伤害计算类进行计算以及血量扣除
/// </summary>
public interface IDamage : IBaseState
{
    /// <summary>
    /// 根据技能获取粒子的初始化数据
    /// </summary>
    /// <param name="skills">技能数组</param>
    /// <param name="playerObj">对象</param>
    /// <returns></returns>
    ParticalInitParamData[] GetParticalInitParamData(GameObject playerObj, params SkillBaseStruct[] skills);
}

