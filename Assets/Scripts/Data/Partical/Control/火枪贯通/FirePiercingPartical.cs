using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 火枪贯通粒子控制
/// </summary>
public class FirePiercingPartical : ParticalControlEntry
{
    /// <summary>
    /// 子粒子
    /// </summary>
    public GameObject tempChildObj;
    /// <summary>
    /// 碰撞的遮罩
    /// </summary>
    LayerMask layerMask;
    /// <summary>
    /// 回调
    /// </summary>
    Func<CollisionHitCallbackStruct, bool> CollisionCallback;
    /// <summary>
    /// 范围
    /// </summary>
    float range;
    /// <summary>
    /// 最多发射次数
    /// </summary>
    public int MaxShotCount = 5;
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
        this.range = 10 * range;
        SetLifeCycle(5);
        tempShotCount = 0;
        InvokeRepeating("FirePiercing_ShotFire", 0.1f, 0.5f);
    }

    /// <summary>
    /// 开火
    /// </summary>
    void FirePiercing_ShotFire()
    {
        tempShotCount++;
        if (tempShotCount > MaxShotCount)
        {
            CancelInvoke("FirePiercing_ShotFire");
            return;
        }
        //生成新的发射
        GameObject go = GameObject.Instantiate(tempChildObj);
        go.transform.SetParent(transform);
        go.transform.position = transform.position;
        go.transform.forward = transform.forward;
        IParticalConduct iParticalConduct = go.GetComponent<IParticalConduct>();
        if (iParticalConduct != null)
        {
            iParticalConduct.SetCollisionCallback(CollisionCallback);
            iParticalConduct.SetRange(range);
            iParticalConduct.SetLayerMask(layerMask);
        }
        go.SetActive(true);
    }


    //private void Start()
    //{
    //    SetLifeCycle(100);
    //    Init(Vector3.up, Vector3.forward, Color.red, ~1, tep => true, 10);
    //}
}
