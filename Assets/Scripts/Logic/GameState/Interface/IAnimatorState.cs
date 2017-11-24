using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画状态接口,主要保存当前的状态以及此时的附加数据
/// </summary>
public interface IAnimatorState : IBaseState
{
    #region  外部设置 
    /// <summary>
    /// 当前的魔法动画类型
    /// </summary>
    EnumMagicAnimatorType MagicAnimatorType { get; set; }
    /// <summary>
    /// 当前的物理动画类型
    /// </summary>
    EnumPhysicAnimatorType PhysicAnimatorType { get; set; }
    /// <summary>
    /// 当前的物理技能类型,用于检索动画
    /// </summary>
    EnumSkillType PhysicAnimatorSkillType { get; set; }
    /// <summary>
    /// 当前的交互动画类型
    /// </summary>
    EnumMutualAnimatorType MutualAnimatorType { get; set; }
    /// <summary>
    /// 动画的移动速度
    /// </summary>
    float AnimatorMoveSpeed { get; set; }
    /// <summary>
    /// 当前的移动方向类型(EnumMoveAnimatorVectorType枚举值表示了四个方向,更详细的的方向请使用-180到180表示(左-右))
    /// </summary>
    int MoveAnimatorVectorType { get; set; }
    /// <summary>
    /// 翻滚动画
    /// </summary>
    bool RollAnimator { get; set; }
    #endregion
    #region 内部设置 
    /// <summary>
    /// 是否正在进行魔法动作
    /// </summary>
    bool IsMagicActionState { get; set; }
    /// <summary>
    ///  是否正在进行物理动作(普通攻击)
    /// </summary>
    bool IsPhycisActionState { get; set; }
    /// <summary>
    /// 当前物理动作(普通攻击)的阶段 
    /// </summary>
    int PhycisActionNowType { get; set; }
    /// <summary>
    /// 是否正在进行技能动作
    /// </summary>
    bool IsSkillState { get; set; }
    #endregion 
}

/// <summary>
/// 当前的魔法动画类型
/// </summary>
public enum EnumMagicAnimatorType
{
    /// <summary>
    /// 没有
    /// </summary>
    None,
    /// <summary>
    /// 吟唱
    /// </summary>
    Sing,
    /// <summary>
    /// 发射
    /// </summary>
    Shot,

}

/// <summary>
/// 当前的物理动画类型
/// </summary>
public enum EnumPhysicAnimatorType
{
    /// <summary>
    /// 没有
    /// </summary>
    None,
    /// <summary>
    /// 普通攻击
    /// </summary>
    Normal,
    /// <summary>
    /// 技能
    /// </summary>
    Skill,
}

/// <summary>
/// 当前的交互动画类型
/// </summary>
public enum EnumMutualAnimatorType
{
    /// <summary>
    /// 没有
    /// </summary>
    None,
}

/// <summary>
/// 当前的移动动画方向类型
/// </summary>
public enum EnumMoveAnimatorVectorType
{
    Forward = 0,
    Left = -90,
    Right = 90,
    Back = 180
}