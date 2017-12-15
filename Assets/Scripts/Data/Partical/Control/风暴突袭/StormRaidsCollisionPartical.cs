using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 风暴突袭子粒子控制
/// </summary>
public class StormRaidsCollisionPartical : ParticalControlEntry
{
    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        SetCollisionCallback(CollisionCallback);
        SetLayerMask(layerMask);
    }
}
