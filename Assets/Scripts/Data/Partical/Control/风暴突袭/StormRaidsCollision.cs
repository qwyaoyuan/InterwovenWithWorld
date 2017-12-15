using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 风暴突袭碰撞检测
/// </summary>
public class StormRaidsCollision : MonoBehaviour, IParticalConduct
{
    /// <summary>
    /// 检测的层 
    /// </summary>
    LayerMask layerMask;
    /// <summary>
    /// 回调
    /// </summary>
    Func<CollisionHitCallbackStruct, bool> CallBack;

    public void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {
        this.CallBack = CallBack;
    }

    public void SetColor(Color color)
    {

    }

    public void SetForward(Vector3 forward)
    {

    }

    public void SetLayerMask(LayerMask layerMask)
    {
        this.layerMask = layerMask;
    }

    public void SetRange(float range)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject targetObj = other.gameObject;
        int layer = targetObj.layer;
        int targetLayerMask = (int)Math.Pow(2, layer);
        int combineLayerMask = targetLayerMask | layerMask;
        if (layerMask == combineLayerMask)//说明在检测的层里面
        {
            if (CallBack != null)
            {
                CallBack(new CollisionHitCallbackStruct() { targetObj = targetObj, hitPoint = targetObj.transform.position });
            }
        }
    }
}
