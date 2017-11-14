using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// buff状态
/// </summary>
public interface IBuffState : IBaseState
{
    /// <summary>
    /// 活力
    /// </summary>
    BuffState Huoli { get; }
    /// <summary>
    /// 加速
    /// </summary>
    BuffState Jiasu { get; }
    /// <summary>
    /// 净化
    /// </summary>
    BuffState Jinghua { get; }
    /// <summary>
    /// 敏捷
    /// </summary>
    BuffState Minjie { get; }
    /// <summary>
    /// 强力
    /// </summary>
    BuffState Qiangli { get; }
    /// <summary>
    /// 驱散
    /// </summary>
    BuffState Qusan { get; }
    /// <summary>
    /// 睿智
    /// </summary>
    BuffState Ruizhi { get; }
    /// <summary>
    /// 吸血-物理
    /// </summary>
    BuffState XixueWuli { get; }
    /// <summary>
    /// 吸血-魔法
    /// </summary>
    BuffState XixueMofa { get; }
}
