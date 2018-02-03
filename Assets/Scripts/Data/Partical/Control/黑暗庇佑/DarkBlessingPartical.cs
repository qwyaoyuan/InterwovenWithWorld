using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑暗庇佑的粒子控制
/// </summary>
public class DarkBlessingPartical : ParticalControlEntry
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
        //黑暗庇佑不需要方向 
        transform.position = pos;
        SetLayerMask(layerMask);
        SetCollisionCallback(CollisionCallback);
        SetRange(range);
        SetLifeCycle(5);
        Invoke("OpenCheck", 1f);
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
                    CallBack(new CollisionHitCallbackStruct() { hitPoint = other.transform.position, targetObj = other.transform.gameObject });
                }
            }
        }
    }
}
