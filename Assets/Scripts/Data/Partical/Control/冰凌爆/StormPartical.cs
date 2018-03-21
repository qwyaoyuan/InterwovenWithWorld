using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 暴风雪粒子控制
/// </summary>
public class StormPartical : ParticalControlEntry
{
    /// <summary>
    /// 碰撞的遮罩层
    /// </summary>
    LayerMask layerMask;
    /// <summary>
    /// 碰撞到对象后回调
    /// </summary>
    Func<CollisionHitCallbackStruct, bool> CallBack;

    List<GameObject> objList;

    /// <summary>
    /// 刚体
    /// </summary>
    Rigidbody thisRigidbody;

    private void Awake()
    {
        objList = new List<GameObject>();
        thisRigidbody = GetComponent<Rigidbody>();
        thisRigidbody.detectCollisions = false;
    }

    /// <summary>
    /// 设置触发检测
    /// </summary>
    void SetTriggerOn()
    {
        thisRigidbody.detectCollisions = true;
        Invoke("SetTriggerOff", 0.5f);
    }

    /// <summary>
    /// 设置取消检测
    /// </summary>
    void SetTriggerOff()
    {
        thisRigidbody.detectCollisions = false;
    }

    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        //暴风雪不需要方向
        transform.position = pos;
        SetLayerMask(layerMask);
        SetCollisionCallback(CollisionCallback);
        Invoke("SetTriggerOn", 0.5f);
    }

    public override void SetLayerMask(LayerMask layerMask)
    {
        this.layerMask = layerMask;
    }

    public override void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {
        this.CallBack = CallBack;
    }


    /// <summary>
    /// 触发
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (objList.Contains(other.transform.gameObject))
            return;
        objList.Add(other.transform.gameObject);
        int layer = (int)Math.Pow(2, other.gameObject.layer);
        int endLayer = layer | layerMask;
        if (endLayer == layerMask)
        {
            CallBack(new CollisionHitCallbackStruct() { hitPoint = other.transform.position, targetObj = other.transform.gameObject });
        }
    }
}
