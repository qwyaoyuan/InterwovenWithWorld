using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 巨人之怒粒子控制
/// </summary>
public class FuryGiantsPartical : ParticalControlEntry
{

    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        transform.position = pos;
        transform.forward = forward;
        SetLayerMask(layerMask);
        SetCollisionCallback(CollisionCallback);
    }


    private void Start()
    {
        Init(Vector3.up, Vector3.forward, Color.red, ~1, temp => true, 10);
    }
}
