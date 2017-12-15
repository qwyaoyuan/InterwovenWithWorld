using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置RFX4_RaycastCollision类的参数,主要是距离遮罩以及碰撞后的回调
/// </summary>
public class SetPartical_RFX4_RaycastCollision : MonoBehaviour, IParticalConduct
{

    /// <summary>
    /// 当前回调
    /// </summary>
    private EventHandler<RFX4_RaycastCollision.RFX4_RaycastCollision_Data> nowCallBack;

    public void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {
        RFX4_RaycastCollision rfx4_RaycastCollision = GetComponent<RFX4_RaycastCollision>();
        if (rfx4_RaycastCollision != null)
        {
            if (nowCallBack != null)
            {
                try
                {
                    rfx4_RaycastCollision.CollisionHandle -= nowCallBack;
                    nowCallBack = null;
                }
                catch { }
                nowCallBack = (sender, e) =>
                {
                    if (CallBack != null && e != null)
                    {
                        CallBack(new CollisionHitCallbackStruct() { hitPoint = e.point, targetObj = e.collisionObj });
                    }
                };
                rfx4_RaycastCollision.CollisionHandle += nowCallBack;
            }
        }
    }

    public void SetColor(Color color)
    {

    }

    public void SetForward(Vector3 forward)
    {

    }

    public void SetLayerMask(LayerMask layerMask)
    {
        RFX4_RaycastCollision rfx4_RaycastCollision = GetComponent<RFX4_RaycastCollision>();
        if (rfx4_RaycastCollision != null)
        {
            rfx4_RaycastCollision.CollidesWith = layerMask;
        }
    }

    public void SetRange(float range)
    {
        RFX4_RaycastCollision rfx4_RaycastCollision = GetComponent<RFX4_RaycastCollision>();
        if (rfx4_RaycastCollision != null)
        {
            rfx4_RaycastCollision.RaycastDistance = range;
        }
    }


}
