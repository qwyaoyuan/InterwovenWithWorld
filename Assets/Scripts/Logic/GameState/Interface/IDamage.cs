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
    /// 设置普通攻击
    /// </summary>
    /// <param name="iPlayerState">玩家状态对象</param>
    /// <param name="attackOrder">攻击的编号</param>
    /// <param name="nowIAttributeState">本技能释放时的数据状态</param>
    /// <param name="weaponTypeByPlayerState">武器类型</param>
    void SetNormalAttack(IPlayerState iPlayerState, int attackOrder, IAttributeState nowIAttributeState, EnumWeaponTypeByPlayerState weaponTypeByPlayerState);
    /// <summary>
    /// 设置物理技能攻击
    /// </summary>
    /// <param name="iPlayerState">玩家状态对象</param>
    /// <param name="physicsSkillStateStruct">本技能释放时的数据状态</param>
    /// <param name="skillType">技能类型</param>
    /// <param name="weaponTypeByPlayerState">武器类型</param>
    void SetPhysicSkillAttack(IPlayerState iPlayerState, PhysicsSkillStateStruct physicsSkillStateStruct, EnumSkillType skillType, EnumWeaponTypeByPlayerState weaponTypeByPlayerState);

    /// <summary>
    /// 物理攻击命中,武器类型
    /// </summary>
    int WeaponPhysicHit { get; set; }
    /// <summary>
    /// 魔法攻击命中,二阶段类型
    /// </summary>
    int MagicTypeHit { get; set; }
}

