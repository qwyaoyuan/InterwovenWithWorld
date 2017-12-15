using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 水啸碰撞后生成粒子控制
/// </summary>
public class WaterSquealingCollisionPartical : ParticalControlEntry
{
    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        transform.forward = -forward;
        transform.position = pos;
    }
}
