using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 风龙卷粒子控制
/// </summary>
public class WindDragonRollPartical : ParticalControlEntry
{
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

    private void Awake()
    {
        lastCheckCollisionTime = 0;
    }

    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        //风龙卷不需要方向
        transform.position = pos;
        SetLayerMask(layerMask);
        SetCollisionCallback(CollisionCallback);
        //风龙卷暂时无法调节范围
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

    protected override void Update()
    {
        base.Update();
        lastCheckCollisionTime += Time.deltaTime;
        if (lastCheckCollisionTime > checkCollisionIntervalTime)
        {
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
                CallBack(new CollisionHitCallbackStruct() { hitPoint = other.transform.position, targetObj = other.transform.gameObject });
            }
        }
    }


    private void Start()
    {
        SetLifeCycle(10);
        Init(Vector3.up, Vector3.forward, Color.red, ~1, temp => true, 10);
    }
}
