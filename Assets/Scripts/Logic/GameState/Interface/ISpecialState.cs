using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特殊状态
/// </summary>
public interface ISpecialState : IBaseState
{
    /// <summary>
    /// 嘲讽
    /// </summary>
    BuffState Chaofeng { get; }
    /// <summary>
    /// 混乱
    /// </summary>
    BuffState Hunluan { get; }
    /// <summary>
    /// 僵直
    /// </summary>
    BuffState Jiangzhi { get; }
    /// <summary>
    /// 恐惧
    /// </summary>
    BuffState Kongju { get; }
    /// <summary>
    /// 魅惑
    /// </summary>
    BuffState Meihuo { get; }
    /// <summary>
    /// 眩晕
    /// </summary>
    BuffState Xuanyun { get; }
    /// <summary>
    /// 致盲
    /// </summary>
    BuffState Zhimang { get; }
    /// <summary>
    /// 禁锢
    /// </summary>
    BuffState Jingu { get; }
    /// <summary>
    /// 禁魔
    /// </summary>
    BuffState Jinmo { get;  }
    /// <summary>
    /// 麻痹
    /// </summary>
    BuffState Mabi { get; }
}
