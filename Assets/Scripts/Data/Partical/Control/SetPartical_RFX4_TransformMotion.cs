using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置RFX4_TransformMotion类的参数,主要是距离遮罩以及碰撞后的回调
/// </summary>
class SetPartical_RFX4_TransformMotion : MonoBehaviour, IParticalConduct
{
    /// <summary>
    /// 当前回调
    /// </summary>
    private EventHandler<RFX4_TransformMotion.RFX4_CollisionInfo> nowCallBack;

    public void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {
        RFX4_TransformMotion rfx4_TransformMotion = GetComponent<RFX4_TransformMotion>();
        if (rfx4_TransformMotion)
        {
            if (nowCallBack != null)
            {
                try
                {
                    rfx4_TransformMotion.CollisionEnter -= nowCallBack;
                    nowCallBack = null;
                }
                catch { }
            }
            nowCallBack = (sender, e) =>
            {
                if (CallBack != null && e != null)
                {
                    CallBack(new CollisionHitCallbackStruct() { hitPoint = e.Hit.point, targetObj = e.Hit.transform.gameObject });
                }
            };
            rfx4_TransformMotion.CollisionEnter += nowCallBack;
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
        RFX4_TransformMotion rfx4_TransformMotion = GetComponent<RFX4_TransformMotion>();
        if (rfx4_TransformMotion)
            rfx4_TransformMotion.CollidesWith = layerMask;
    }

    public void SetRange(float range)
    {
        RFX4_TransformMotion rfx4_TransformMotion = GetComponent<RFX4_TransformMotion>();
        if (rfx4_TransformMotion)
            rfx4_TransformMotion.Distance = range;
    }
}