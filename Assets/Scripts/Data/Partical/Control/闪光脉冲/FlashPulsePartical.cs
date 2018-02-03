using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 闪光脉冲粒子控制
/// </summary>
public class FlashPulsePartical : ParticalControlEntry
{
    /// <summary>
    /// 显示的颜色
    /// </summary>
    public Color magicColor;

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
        SetColor(magicColor);
        SetCollisionCallback(CollisionCallback);
    }
}
