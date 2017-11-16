using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画状态接口,主要保存当前的状态以及此时的附加数据
/// </summary>
public interface IAnimatorState : IBaseState
{
    /// <summary>
    /// 当前的魔法动画类型
    /// </summary>
    EnumMagicAnimatorType MagicAnimatorType { get; set; }
    /// <summary>
    /// 当前的物理动画类型
    /// </summary>
    EnumPhysicAnimatorType PhysicAnimatorType { get; set; }
    /// <summary>
    /// 当前的交互动画类型
    /// </summary>
    EnumMutualAnimatorType MutualAnimatorType { get; set; }
    /// <summary>
    /// 当前的移动方向类型(EnumMoveAnimatorVectorType枚举值表示了四个方向,更详细的的方向请使用-180到180表示(左-右))
    /// </summary>
    int MoveAnimatorVectorType { get; set; }
}

/// <summary>
/// 当前的魔法动画类型
/// </summary>
public enum EnumMagicAnimatorType
{
    None,
}

/// <summary>
/// 当前的物理动画类型
/// </summary>
public enum EnumPhysicAnimatorType
{
    None,
}

/// <summary>
/// 当前的交互动画类型
/// </summary>
public enum EnumMutualAnimatorType
{
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