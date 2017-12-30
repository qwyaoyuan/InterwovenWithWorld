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
    BuffState Huoli { get; set; }
    /// <summary>
    /// 加速
    /// </summary>
    BuffState Jiasu { get; set; }
    /// <summary>
    /// 净化
    /// </summary>
    BuffState Jinghua { get; set; }
    /// <summary>
    /// 敏捷
    /// </summary>
    BuffState Minjie { get; set; }
    /// <summary>
    /// 强力
    /// </summary>
    BuffState Qiangli { get; set; }
    /// <summary>
    /// 驱散
    /// </summary>
    BuffState Qusan { get; set; }
    /// <summary>
    /// 睿智
    /// </summary>
    BuffState Ruizhi { get; set; }
    /// <summary>
    /// 吸血-物理
    /// </summary>
    BuffState XixueWuli { get; set; }
    /// <summary>
    /// 吸血-魔法
    /// </summary>
    BuffState XixueMofa { get; set; }
}
