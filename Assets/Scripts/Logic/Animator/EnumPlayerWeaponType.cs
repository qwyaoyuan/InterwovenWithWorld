using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 玩家武器类型枚举
/// 主要使用在动画系统的状态判断中
/// </summary>
public enum EnumPlayerWeaponType
{
    /// <summary>
    /// 空手
    /// </summary>
    None=0,
    /// <summary>
    /// 单手剑
    /// </summary>
    OneHandSword = 1,
    /// <summary>
    /// 双手剑
    /// </summary>
    TwoHandSword = 2,
    /// <summary>
    /// 弓
    /// </summary>
    Bow = 3
}

/// <summary>
/// 武器类型对应动画速度
/// </summary>
[Serializable]
public class WeaponTypeToSpeed
{
    /// <summary>
    /// 动画类型
    /// </summary>
    public EnumPlayerWeaponType weaponType;
    /// <summary>
    /// 播放速度 
    /// </summary>
    public float speed = 1;
}
