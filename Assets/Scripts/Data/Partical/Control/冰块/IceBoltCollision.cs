using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBoltCollision : MonoBehaviour
{
    /// <summary>
    /// 物理脚本
    /// </summary>
    RFX4_PhysXSetImpulse phys;

    /// <summary>
    /// 是否已经发生碰撞
    /// </summary>
    [HideInInspector]
    public bool isCollided = false;

    /// <summary>
    /// 发生碰撞的层
    /// </summary>
    LayerMask layerMask = ~0;//默认为全部发生碰撞

    /// <summary>
    /// 碰撞后的回调
    /// </summary>
    Func<CollisionHitCallbackStruct, bool> CallBack;

    /// <summary>
    /// 设置的方向
    /// </summary>
    private Vector3 setForward = Vector3.forward;

    /// <summary>
    /// 碰撞后触发的对象
    /// </summary>
    public GameObject IceBoltExplosion;
    /// <summary>
    /// 冰块对象
    /// </summary>
    public GameObject IceBoltObj;

    public void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {
        this.CallBack = CallBack;
    }

    public void SetLayerMask(LayerMask layerMask)
    {
        this.layerMask = layerMask;
    }

    public void SetForward(Vector3 forward)
    {
        setForward = forward;
    }

    private void Update()
    {
        if (!isCollided)
        {
            transform.forward = setForward;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isCollided)
        {
            int layer = collision.gameObject.layer;
            int targetLayerMask = (int)Math.Pow(2, layer);
            int combineLayerMask = targetLayerMask | layerMask;
            if (layerMask != combineLayerMask)//说明不在检测的层里面
            {
                Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
                transform.forward = setForward;
                return;
            }

            isCollided = true;
            IceBoltExplosion.SetActive(true);
            IceBoltObj.SetActive(false);
            var rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.detectCollisions = false;
            //回调
            if (CallBack != null)
            {
                CallBack(new CollisionHitCallbackStruct() { hitPoint = transform.position, targetObj = collision.transform.gameObject });
            }
        }
    }
}
