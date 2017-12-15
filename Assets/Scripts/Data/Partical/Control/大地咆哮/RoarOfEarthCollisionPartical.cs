using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大地咆哮的碰撞粒子控制
/// </summary>
public class RoarOfEarthCollisionPartical : ParticalControlEntry
{
    private void Awake()
    {
        SetLifeCycle(10);
    }

    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        SetCollisionCallback(CollisionCallback);
        SetLayerMask(layerMask);
    }
}
