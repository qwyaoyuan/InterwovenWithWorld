using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔力脉冲粒子控制
/// </summary>
public class MagicPulsePartical : ParticalControlEntry
{
    /// <summary>
    /// 显示的颜色
    /// </summary>
    public Color magicColor;

/*
    private void Start()
    {
        Init(Vector3.up, Vector3.forward, Color.red, ~1, temp => true, 10);
    }
*/

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
        if (magicColor.a != 0)
            SetColor(magicColor);
        else
            SetColor(color);
        SetCollisionCallback(CollisionCallback);
    }
}

