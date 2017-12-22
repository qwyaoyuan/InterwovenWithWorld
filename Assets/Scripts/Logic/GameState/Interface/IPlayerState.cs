using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于保存玩家运行时的状态
/// 比如是Buff与Debuff 动画的状态 玩家当前的属性（将所有属性叠加后）
/// </summary>
public interface IPlayerState : IBaseState,
    IAttributeState, IBuffState, IDebuffState, ISpecialState, IAnimatorState, ISkillState,IDamage
{
    /// <summary>
    /// 玩家操纵角色的游戏对象
    /// </summary>
    GameObject PlayerObj { get; set; }
    /// <summary>
    /// 玩家的摄像机
    /// </summary>
    Camera PlayerCamera { get; set; }

    /// <summary>
    /// 更新属性
    /// </summary>
    void UpdateAttribute();
    /// <summary>
    /// 等级
    /// </summary>
    int Level { get; }
    /// <summary>
    /// 经验值(本级的)
    /// </summary>
    int Experience { get; set; }
    /// <summary>
    /// 技能等级变化
    /// </summary>
    bool SkillLevelChanged { get; set; }
    /// <summary>
    /// 种族等级变化
    /// </summary>
    bool RaceLevelChanged { get; set; }
    /// <summary>
    /// 装备发生变化
    /// </summary>
    bool EquipmentChanged { get; set; }
    /// <summary>
    /// 物品发生变化
    /// </summary>
    bool GoodsChanged { get; set; }
}
