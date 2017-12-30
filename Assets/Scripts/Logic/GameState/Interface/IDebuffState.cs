using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Debuff的状态
/// </summary>
public interface IDebuffState : IBaseState
{
    /// <summary>
    /// 冰冻
    /// </summary>
    BuffState Bingdong { get; }
	/// <summary>
    /// 迟钝
    /// </summary>
    BuffState Chidun { get; }
    /// <summary>
    /// 点燃
    /// </summary>
    BuffState Dianran { get; }
    /// <summary>
    /// 凋零
    /// </summary>
    BuffState Diaoling { get; }
    /// <summary>
    /// 减速
    /// </summary>
    BuffState Jiansu { get; }
    /// <summary>
    /// 迷惑
    /// </summary>
    BuffState Mihuo { get; }
    /// <summary>
    /// 无力
    /// </summary>
    BuffState Wuli { get; }
    /// <summary>
    /// 虚弱
    /// </summary>
    BuffState Xuruo { get; }
    /// <summary>
    /// 中毒
    /// </summary>
    BuffState Zhongdu { get; }
    /// <summary>
    /// 诅咒
    /// </summary>
    BuffState Zuzhou { get; }
    /// <summary>
    /// 流血
    /// </summary>
    BuffState LiuXue { get; }
    /// <summary>
    /// 清理Debuff
    /// </summary>
    void ClearDebuff(params EnumStatusEffect[] effects);
}
