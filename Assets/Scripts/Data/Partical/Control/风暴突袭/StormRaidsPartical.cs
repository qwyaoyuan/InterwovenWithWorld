using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 风暴突袭的粒子控制
/// </summary>
public class StormRaidsPartical : ParticalControlEntry
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
    /// 最多发射次数
    /// </summary>
    public int MaxShotCount = 10;
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
        InvokeRepeating("StormRaids_Shot", 0.1f, 0.1f);
    }

    /// <summary>
    /// 开火
    /// </summary>
    void StormRaids_Shot()
    {
        tempShotCount++;
        if (tempShotCount > MaxShotCount)
        {
            CancelInvoke("StormRaids_Shot");
            return;
        }
        //生成新的发射
        GameObject go = GameObject.Instantiate(tempChildObj);
        go.transform.SetParent(transform);
        go.transform.position = transform.position +
            go.transform.TransformPoint(new Vector3(UnityEngine.Random.Range(-4, 4), UnityEngine.Random.Range(0, 4), 0));
        Vector3 thisForward = (transform.position + transform.forward * 20 -go.transform.position).normalized;
        go.transform.forward = thisForward;
        ParticalControlEntry particalControlEntry = go.GetComponent<ParticalControlEntry>();
        if (particalControlEntry != null)
        {
            particalControlEntry.Init(go.transform.position, go.transform.forward, Color.red, layerMask, CollisionCallback, 1);
        }
        go.SetActive(true);
        StormRaids_Shot();
    }

    private void Start()
    {
        SetLifeCycle(100);
        Init(Vector3.zero, Vector3.forward, Color.red, ~1, temp => { Debug.Log(temp.targetObj); return true; }, 1);
    }
}
