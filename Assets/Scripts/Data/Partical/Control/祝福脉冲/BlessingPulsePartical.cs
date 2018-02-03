using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 祝福脉冲(不同于魔力脉冲,不需要设置颜色,粒子已经设置好了)
/// </summary>
public class BlessingPulsePartical : ParticalControlEntry
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
        SetLayerMask(layerMask);;
        SetCollisionCallback(CollisionCallback);
    }

}
