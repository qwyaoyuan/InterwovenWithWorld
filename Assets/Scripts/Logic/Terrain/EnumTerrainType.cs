using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地域类型
/// </summary>
public enum EnumTerrainType
{
    /// <summary>
    /// 陆地，包括可以移动到的山区
    /// </summary>
    Land,
    /// <summary>
    /// 海洋，不可移动到的位置
    /// </summary>
    Sea,
    /// <summary>
    /// 山地，不可移动到的位置
    /// </summary>
    Montain
}
