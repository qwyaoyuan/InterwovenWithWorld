using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 炎舞粒子控制
/// </summary>
public class FireDancePartical : ParticalControlEntry
{
    /// <summary>
    /// 触发器
    /// </summary>
    CapsuleCollider capsuleCollider;
    /// <summary>
    /// 基础半径
    /// </summary>
    float baseRadius;
    /// <summary>
    /// 碰撞的遮罩层
    /// </summary>
    LayerMask layerMask;
    /// <summary>
    /// 碰撞到对象后回调
    /// </summary>
    Func<CollisionHitCallbackStruct, bool> CallBack;

    /// <summary>
    /// 距离上一次检测的时间
    /// </summary>
    float lastCheckCollisionTime;

    /// <summary>
    /// 检测对象
    /// </summary>
    List<GameObject> tempObjs;

    private void Awake()
    {
        tempObjs = new List<GameObject>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        baseRadius = capsuleCollider.radius;
        lastCheckCollisionTime = 0;
    }

    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        //炎舞不需要方向
        transform.position = pos;
        SetLayerMask(layerMask);
        SetCollisionCallback(CollisionCallback);
        SetRange(range);
        lastCheckCollisionTime = 0;
    }

    public override void SetLayerMask(LayerMask layerMask)
    {
        this.layerMask = layerMask;
    }

    public override void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {
        this.CallBack = CallBack;
    }

    public override void SetRange(float range)
    {
        base.SetRange(range);
        capsuleCollider.radius = baseRadius * range;
    }

    protected override void Update()
    {
        base.Update();
        lastCheckCollisionTime += Time.deltaTime;
        if (lastCheckCollisionTime > checkCollisionIntervalTime)
        {
            tempObjs.Clear();
            lastCheckCollisionTime = 0;
        }
    }

    /// <summary>
    /// 持续触发
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (lastCheckCollisionTime == 0 && CallBack != null)
        {
            int layer = (int)Math.Pow(2, other.gameObject.layer);
            int endLayer = layer | layerMask;
            if (endLayer == layerMask)
            {
                if (tempObjs.Contains(other.gameObject))
                    return;
                tempObjs.Add(other.gameObject);
                CallBack(new CollisionHitCallbackStruct() { hitPoint = other.transform.position, targetObj = other.transform.gameObject });
            }
        }
    }
}
