using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 粒子行为接口
/// 用于控制粒子的公共行为
/// </summary>
public interface IParticalConduct
{
    /// <summary>
    /// 设置粒子的基准颜色
    /// </summary>
    /// <param name="color"></param>
    void SetColor(Color color);
    /// <summary>
    /// 设置粒子的遮罩层(碰撞检测)
    /// </summary>
    /// <param name="layerMask"></param>
    void SetLayerMask(LayerMask layerMask);
    /// <summary>
    /// 碰撞后的回调
    /// </summary>
    /// <param name="CallBack"></param>
    void SetCollisionCallback(Action<GameObject> CallBack);
    /// <summary>
    /// 设置方向
    /// </summary>
    /// <param name="forward"></param>
    void SetForward(Vector3 forward);
}
