using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大地图选择类型枚举
/// </summary>
public enum EnumBigMapIconCheck
{
    /// <summary>
    /// 任务
    /// </summary>
    Task,
    /// <summary>
    /// 功能NPC
    /// </summary>
    Action,
    /// <summary>
    /// Boss
    /// </summary>
    Boss,
    /// <summary>
    /// 路牌
    /// </summary>
    Street,
    /// <summary>
    /// 标记(注意这里指的是单纯的标记,其中任务或者功能npc也可以是标记,具体的判断在具体的里面)
    /// </summary>
    Flag,
}
