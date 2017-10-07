using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 技能释放类型
/// </summary>
[Obsolete("暂时停止使用，因为释放类型是组合或直接释放，在具体的结构中可以被定义，同时如何组合可以在按键对象中等待处理")]
public enum EnumReleaseType
{
    /// <summary>
    /// 直接释放
    /// </summary>
    Direct,
    /// <summary>
    /// 组合释放
    /// </summary>
    Combination
}
