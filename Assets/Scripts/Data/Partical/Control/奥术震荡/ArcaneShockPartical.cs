using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 奥术震荡粒子控制 
/// </summary>
public class ArcaneShockPartical : ParticalControlEntry
{
    /// <summary>
    /// 触发器
    /// </summary>
    SphereCollider sphereCollider;
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
    /// 开始检测的时间
    /// </summary>
    public float StartCheckTime = 0.5f;
    /// <summary>
    /// 当前检测的时间
    /// </summary>
    private float nowCheckTime;

    /// <summary>
    /// 已经检测过了 
    /// </summary>
    private bool isChecked;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        baseRadius = sphereCollider.radius;
        sphereCollider.enabled = false;
    }

    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        //奥术震荡不需要方向
        transform.position = pos;
        SetLayerMask(layerMask);
        SetCollisionCallback(CollisionCallback);
        SetRange(range);
        nowCheckTime = 0;
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
        sphereCollider.radius = baseRadius * range;
    }

    protected override void Update()
    {
        base.Update();
        nowCheckTime += Time.deltaTime;
        if (nowCheckTime > StartCheckTime)
        {
            if (!isChecked)
            {
                sphereCollider.enabled = true;
            }
            else
            {
                sphereCollider.enabled = false;
            }
        }
    }

    /// <summary>
    /// 单次触发
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (nowCheckTime>StartCheckTime  && CallBack != null)
        {
            isChecked = true;
            int layer = (int)Math.Pow(2, other.gameObject.layer);
            int endLayer = layer | layerMask;
            if (endLayer == layerMask)
            {
                CallBack(new CollisionHitCallbackStruct() { hitPoint = other.transform.position, targetObj = other.transform.gameObject });
            }
        }
    }
}
