using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 魔力脉冲碰撞后生成粒子控制
/// </summary>
public class MagicPulseCollisionPartical : ParticalControlEntry
{
    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        transform.forward = -forward;
        transform.position = pos;
        SetColor(color);
    }
}

