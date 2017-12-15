using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 冰球粒子控制
/// </summary>
public class IceBoltPartical : ParticalControlEntry
{
    /// <summary>
    /// 冰块对象
    /// </summary>
    public GameObject IceBoltObj;
    /// <summary>
    /// 碰撞后触发的对象
    /// </summary>
    public GameObject IceBoltExplosion;

    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        IceBoltExplosion.SetActive(false);
        IceBoltObj.SetActive(true);
        transform.position = pos;
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.detectCollisions = true;
        IceBoltCollision iceBoltCollision = GetComponent<IceBoltCollision>();
        iceBoltCollision.isCollided = false;
        if (targetObjs.Length > 0)
        {
            forward = (targetObjs[0].transform.position - transform.position).normalized;
        }
        transform.forward = forward;
        SetForward(forward);
        SetLayerMask(layerMask);
        SetCollisionCallback(CollisionCallback);
    }

    public override void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {
        IceBoltCollision iceBoltCollision = GetComponent<IceBoltCollision>();
        iceBoltCollision.SetCollisionCallback(CallBack);
    }

    public override void SetLayerMask(LayerMask layerMask)
    {
        IceBoltCollision iceBoltCollision = GetComponent<IceBoltCollision>();
        iceBoltCollision.SetLayerMask(layerMask);
    }

    public override void SetForward(Vector3 forward)
    {
        IceBoltCollision iceBoltCollision = GetComponent<IceBoltCollision>();
        iceBoltCollision.SetForward(forward);
    }

}
