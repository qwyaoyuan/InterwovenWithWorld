﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大地咆哮粒子控制
/// </summary>
public class RoarOfEarthPartical : ParticalControlEntry
{
    /// <summary>
    /// 子粒子
    /// </summary>
    public GameObject tempChildObj;

    /// <summary>
    /// 检测的遮罩
    /// </summary>
    LayerMask layerMask;
    /// <summary>
    /// 回调
    /// </summary>
    Func<CollisionHitCallbackStruct, bool> CollisionCallback;

    /// <summary>
    /// 每一个的间距
    /// </summary>
    public int EveryDistance = 6;
    /// <summary>
    /// 最多发射次数
    /// </summary>
    public int MaxShotCount = 3;
    /// <summary>
    /// 临时发射次数
    /// </summary>
    private int tempShotCount;


    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        this.transform.position = pos;
        if (forward != Vector3.zero)
            this.transform.forward = forward;
        else this.transform.forward = Vector3.forward;
        this.layerMask = layerMask;
        this.CollisionCallback = CollisionCallback;
        tempShotCount = 0;
        InvokeRepeating("RoarOfEarth_Shot", 0.1f, 0.2f);
    }

    /// <summary>
    /// 开火
    /// </summary>
    void RoarOfEarth_Shot()
    {
        tempShotCount++;
        if (tempShotCount > MaxShotCount)
        {
            CancelInvoke("RoarOfEarth_Shot");
            return;
        }

        //生成新的发射
        GameObject go = GameObject.Instantiate(tempChildObj);
        go.transform.SetParent(transform);
        go.transform.position = transform.position + transform.forward * tempShotCount * EveryDistance;
        go.transform.forward = transform.forward;
        ParticalControlEntry particalControlEntry = go.GetComponent<ParticalControlEntry>();
        if (particalControlEntry != null)
        {
            particalControlEntry.Init(go.transform.position, go.transform.forward, Color.red, layerMask, CollisionCallback, 1);
        }
        go.SetActive(true);
    }

}
