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
    /// <param name="nowIAttributeState">本次计算所使用的状态数据</param>
    /// <returns></returns>
    ParticalInitParamData[] GetParticalInitParamData(GameObject playerObj, IAttributeState nowIAttributeState, params SkillBaseStruct[] skills);
    /// <summary>
    /// 设置物理技能攻击
    /// </summary>
    /// <param name="playerObj">释放技能的对象(玩家操纵的角色)</param>
    /// <param name="nowIAttributeState">本技能释放时的数据状态</param>
    /// <param name="skillType">技能类型</param>
    /// <param name="weaponTypeByPlayerState">武器类型</param>
    void SetPhysicSkillAttack(GameObject playerObj, IAttributeState nowIAttributeState, EnumSkillType skillType, EnumWeaponTypeByPlayerState weaponTypeByPlayerState);
}

