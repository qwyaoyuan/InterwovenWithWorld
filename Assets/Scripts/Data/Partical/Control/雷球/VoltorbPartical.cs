using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 雷球粒子控制
/// </summary>
public class VoltorbPartical : ParticalControlEntry
{

    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        transform.position = pos;
        if (targetObjs.Length > 0)
        {
            forward = (targetObjs[0].transform.position - transform.position).normalized;
        }
        transform.forward = forward;
        SetForward(forward);
        SetLayerMask(layerMask);
        SetCollisionCallback(CollisionCallback);
    }
}
