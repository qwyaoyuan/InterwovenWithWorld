using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 闪电冲击的粒子控制
/// </summary>
public class LightningImpactPartical : ParticalControlEntry
{
    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        transform.position = pos;
        if (forward != Vector3.zero)
        {
            transform.forward = forward;
        }
        else
        {
            transform.forward = Vector3.forward;
        }
        SetRange(range);
        SetLayerMask(layerMask);
        SetCollisionCallback(CollisionCallback);
        SetLifeCycle(5);
    }

    //private void Start()
    //{
    //    SetLifeCycle(100);
    //    Init(Vector3.up, Vector3.forward, Color.red, ~1, tep => true, 20);
    //}
}
