using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 冰洁咒碰撞后粒子控制
/// </summary>
public class FreezeErosionCollisionPartical : ParticalControlEntry
{
    public LayerMask terrainLayerMask;

    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        Ray ray = new Ray(pos, Vector3.down);
        RaycastHit rch;
        if (Physics.Raycast(ray, out rch, 100, terrainLayerMask))
        {
            this.transform.position = rch.point;
        }
        else
        {
            this.transform.position = pos;
        }
        //设置范围
        SetRange(range);
        transform.localScale = Vector3.one * range;
    }
}
