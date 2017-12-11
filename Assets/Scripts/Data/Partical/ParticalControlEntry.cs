using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 粒子控制入口
/// </summary>
public abstract class ParticalControlEntry : MonoBehaviour
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="forward">方向</param>
    /// <param name="color">颜色</param>
    /// <param name="layerMask">碰撞遮罩</param>
    /// <param name="CollisionCallback">碰撞后回调</param>
    /// 下面的这些参数属于可选参数,不同的粒子不一定有效 
    /// <param name="range">范围</param>
    /// <param name="targetObjs">跟踪目标(如果需要跟踪)</param>
    public abstract void Init(
        Vector3 pos,
        Vector3 forward,
        Color color,
        LayerMask layerMask,
        Action<GameObject> CollisionCallback,
        float range,
        params GameObject[] targetObjs);
}
