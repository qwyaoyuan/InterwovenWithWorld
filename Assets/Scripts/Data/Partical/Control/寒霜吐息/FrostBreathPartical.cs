using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostBreathPartical : ParticalControlEntry
{
    /// <summary>
    /// 检测的遮罩
    /// </summary>
    LayerMask layerMask;
    /// <summary>
    /// 回调
    /// </summary>
    Func<CollisionHitCallbackStruct, bool> CollisionCallback;

    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        this.transform.position = pos;
        if (forward != Vector3.zero)
            this.transform.forward = forward;
        else this.transform.forward = Vector3.forward;
        this.layerMask = layerMask;
        this.CollisionCallback = CollisionCallback;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject targetObj = other.gameObject;
        int layer = targetObj.layer;
        int targetLayerMask = (int)Math.Pow(2, layer);
        int combineLayerMask = targetLayerMask | layerMask;
        if (layerMask == combineLayerMask)//说明在检测的层里面
        {
            if (CollisionCallback != null)
            {
                CollisionCallback(new CollisionHitCallbackStruct() { targetObj = targetObj, hitPoint = targetObj.transform.position });
            }
        }
    }

    private void Start()
    {
        SetLifeCycle(4);
        Init(Vector3.zero, Vector3.forward, Color.red, ~1, temp => { Debug.Log(temp.targetObj); return true; }, 1);
    }
}
