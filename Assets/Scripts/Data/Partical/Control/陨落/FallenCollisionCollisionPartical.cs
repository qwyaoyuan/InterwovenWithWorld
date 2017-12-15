using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 陨落碰撞后粒子控制
/// </summary>
public class FallenCollisionCollisionPartical : ParticalControlEntry
{
    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        
    }
}
