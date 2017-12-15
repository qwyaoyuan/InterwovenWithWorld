using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 炎爆术碰撞后的粒子控制
/// </summary>
public class FieryBlastCollisionPartical : ParticalControlEntry
{
    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        this.transform.position = pos;
        this.transform.forward = -forward;
        //设置范围
        SetRange(range);
        transform.localScale = Vector3.one * range;
    }
}
