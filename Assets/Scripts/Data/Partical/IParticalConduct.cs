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
    /// <param name="CallBack">碰撞回调,返回是否应该发生碰撞</param>
    void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack);
    /// <summary>
    /// 设置方向
    /// </summary>
    /// <param name="forward"></param>
    void SetForward(Vector3 forward);
    /// <summary>
    /// 设置范围(优先:移动距离>攻击范围)
    /// </summary>
    /// <param name="range"></param>
    void SetRange(float range);
    /// <summary>
    /// 打开显示
    /// </summary>
    void Open();
}

/// <summary>
/// 碰撞后回调的结构
/// </summary>
public class CollisionHitCallbackStruct
{
    /// <summary>
    /// 碰撞的点
    /// </summary>
    public Vector3 hitPoint;
    /// <summary>
    /// 碰撞的游戏对象
    /// </summary>
    public GameObject targetObj;
}
