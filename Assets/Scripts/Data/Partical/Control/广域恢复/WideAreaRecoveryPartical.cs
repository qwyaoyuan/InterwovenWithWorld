﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 广域恢复粒子控制
/// </summary>
public class WideAreaRecoveryPartical : ParticalControlEntry
{

    /// <summary>
    /// 触发后生成的粒子
    /// </summary>
    public GameObject collisionPrefab;

    /// <summary>
    /// 碰撞的遮罩层
    /// </summary>
    LayerMask layerMask;
    /// <summary>
    /// 碰撞到对象后回调
    /// </summary>
    Func<CollisionHitCallbackStruct, bool> CallBack;
    /// <summary>
    /// 刚体
    /// </summary>
    Rigidbody thisRigidbody;
    /// <summary>
    /// 检测到的对象集合
    /// </summary>
    List<GameObject> checkedObjList;

    private void Awake()
    {
        checkedObjList = new List<GameObject>();
        thisRigidbody = GetComponent<Rigidbody>();
        thisRigidbody.detectCollisions = false;
    }

    private void OnEnable()
    {
        checkedObjList = new List<GameObject>();
    }

    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        //广域恢复不需要方向 
        transform.position = pos;
        SetLayerMask(layerMask);
        SetCollisionCallback(CollisionCallback);
        SetRange(range);
        SetLifeCycle(5);
        Invoke("OpenCheck", 0.5f);
    }

    private void OpenCheck()
    {
        thisRigidbody.detectCollisions = true;
    }

    public override void SetRange(float range)
    {
        base.SetRange(range);
        transform.localScale = new Vector3(range, range, range);
    }

    public override void SetLayerMask(LayerMask layerMask)
    {
        this.layerMask = layerMask;
    }

    public override void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {
        this.CallBack = CallBack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CallBack != null)
        {
            int layer = (int)Math.Pow(2, other.gameObject.layer);
            int endLayer = layer | layerMask;
            if (endLayer == layerMask)
            {
                if (!checkedObjList.Contains(other.gameObject))
                {
                    checkedObjList.Add(other.gameObject);
                    //生成粒子
                    if (collisionPrefab != null)
                    {
                        GameObject createObj = GameObject.Instantiate(collisionPrefab);
                        createObj.transform.position = other.gameObject.transform.position + Vector3.up;
                        createObj.transform.SetParent(other.gameObject.transform);
                        GameObject.Destroy(createObj, 1.5f);
                    }
                    CallBack(new CollisionHitCallbackStruct() { hitPoint = other.transform.position, targetObj = other.transform.gameObject });
                }
            }
        }
    }
}
