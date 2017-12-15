using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilBlockCollision : MonoBehaviour, IParticalConduct
{

    public GameObject PhysicsObjects;

    /// <summary>
    /// 是否已经发生碰撞
    /// </summary>
    private bool isCollided = false;

    /// <summary>
    /// 设置的方向
    /// </summary>
    private Vector3 setForward;

    /// <summary>
    /// 发生碰撞的层
    /// </summary>
    LayerMask layerMask = ~0;//默认为全部发生碰撞

    /// <summary>
    /// 碰撞后的回调
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
        setForward = forward;
    }

    public void SetLayerMask(LayerMask layerMask)
    {
        this.layerMask = layerMask;
    }

    public void SetRange(float range)
    {

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
            PhysicsObjects.SetActive(true);
            var mesh = GetComponent<MeshRenderer>();
            if (mesh != null)
                mesh.enabled = false;
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

    void OnEnable()
    {
        isCollided = false;
        PhysicsObjects.SetActive(false);
        var mesh = GetComponent<MeshRenderer>();
        if (mesh != null)
            mesh.enabled = true;
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.detectCollisions = true;
    }
}
