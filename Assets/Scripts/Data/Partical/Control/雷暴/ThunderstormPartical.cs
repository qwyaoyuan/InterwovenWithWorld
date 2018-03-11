using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 雷暴粒子控制
/// </summary>
public class ThunderstormPartical : ParticalControlEntry
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
    /// <summary>
    /// 胶囊碰撞器
    /// </summary>
    CapsuleCollider capsuleCollider;
    /// <summary>
    /// 基础半径
    /// </summary>
    float baseRadius;

    /// <summary>
    /// 用于生成链条的预设
    /// </summary>
    public GameObject tempTrailPrefab;

    /// <summary>
    /// 链条长度
    /// </summary>
    public float thundersLineDis;

    /// <summary>
    /// 创建的链条集合
    /// </summary>
    public List<GameObject> createTrailObjList;

    private void Awake()
    {
        createTrailObjList = new List<GameObject>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        baseRadius = capsuleCollider.radius;
        lastCheckCollisionTime = 0;
    }

    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        //default层
        int defaultValue = LayerMask.GetMask("Default");
        int layerValue = layerMask.value | defaultValue;
        layerValue -= defaultValue;

        //雷暴不需要方向
        transform.position = pos + Vector3.up * 3.5f;
        SetLayerMask(layerValue);
        SetCollisionCallback(CollisionCallback);
        SetRange(range);
        lastCheckCollisionTime = 0;
        SetLifeCycle(5);
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
            lastCheckCollisionTime = 0;
            createTrailObjList.Clear();
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
            GameObject target = other.transform.root.gameObject;
            int layer = (int)Math.Pow(2, target.layer);
            int endLayer = layer | layerMask;
            if (endLayer == layerMask && !createTrailObjList.Contains(target))
            {
                createTrailObjList.Add(other.gameObject);
                //生成一个粒子
                GameObject go = GameObject.Instantiate(tempTrailPrefab);
                go.transform.SetParent(transform);
                go.transform.position = transform.position;
                go.transform.forward = (other.transform.position - transform.position).normalized;
                go.SetActive(true);
                go.AddComponent<SetThunderstormParticalRange>().SetRange(thundersLineDis);
                Destroy(go, 1f);
                CallBack(new CollisionHitCallbackStruct() { hitPoint = other.transform.position, targetObj = other.transform.gameObject });
            }
        }
    }

    //private void Start()
    //{
    //    SetLifeCycle(1000);
    //    Init(Vector3.up*5, Vector3.forward, Color.red, 16, temp => true, 1);
    //}
}
