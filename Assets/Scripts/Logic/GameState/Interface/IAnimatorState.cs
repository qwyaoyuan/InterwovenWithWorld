using System;
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
    /// <summary>
    /// 技能动作是否持续
    /// </summary>
    bool SkillSustainable { get; set; }
    /// <summary>
    /// 物理攻击命中了目标设置动画延迟
    /// </summary>
    bool PhysicHitMonsterAnimDelay { get; set; }
    /// <summary>
    /// 当前动画剪辑状态对象 
    /// </summary>
    AnimationClipTypeState AnimationClipTypeState { get; set; }

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
/// 动画剪辑状态
/// </summary>
public class AnimationClipTypeState
{
    /// <summary>
    /// 动画剪辑的类型
    /// </summary>
    public EnumAnimationClipType AnimationClipType;

    /// <summary>
    /// 动画剪辑的当前时间类型(只计算进入时间)
    /// </summary>
    public EnumAnimationClipTimeType TimeType;

    /// <summary>
    /// 当前运行的时间
    /// </summary>
    public float ClipTime;
}

/// <summary>
/// 当前的动画剪辑类型
/// </summary>
public enum EnumAnimationClipType
{
    /// <summary>
    /// 移动
    /// </summary>
    Move,
    /// <summary>
    /// 普通攻击1
    /// </summary>
    Attack1,
    /// <summary>
    /// 普通攻击2
    /// </summary>
    Attack2,
    /// <summary>
    /// 普通攻击3
    /// </summary>
    Attack3,
    /// <summary>
    /// 咏唱魔法
    /// </summary>
    MagicSing,
    /// <summary>
    /// 发射魔法
    /// </summary>
    MagicShot,
    /// <summary>
    /// 物理技能
    /// </summary>
    PhysicSkill,
    /// <summary>
    /// 异常状态
    /// </summary>
    Dizzy,
    /// <summary>
    /// 翻滚
    /// </summary>
    Roll,
}

/// <summary>
/// 当前动画剪辑的时间类型
/// </summary>
public enum EnumAnimationClipTimeType
{
    /// <summary>
    /// 进入,只在开始时存在
    /// </summary>
    In,
    /// <summary>
    /// 开始事件
    /// </summary>
    Start,
    /// <summary>
    /// 结束事件
    /// </summary>
    End,
    /// <summary>
    /// 出去,只在结束时存在,且会立刻移除出集合
    /// </summary>
    Out,
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