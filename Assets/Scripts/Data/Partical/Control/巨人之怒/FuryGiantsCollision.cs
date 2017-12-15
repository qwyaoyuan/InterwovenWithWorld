using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 巨人之怒技能检测碰撞脚本
/// </summary>
public class FuryGiantsCollision : MonoBehaviour, IParticalConduct
{
    /// <summary>
    /// 碰撞遮罩 
    /// </summary>
    LayerMask layerMask = ~0;

    /// <summary>
    /// 回调
    /// </summary>
    Func<CollisionHitCallbackStruct, bool> CallBack;

    /// <summary>
    /// 缸体
    /// </summary>
    Rigidbody thisRigidbody;

    /// <summary>
    /// 已经检测过的对象 
    /// </summary>
    List<GameObject> checkedObjList;

    /// <summary>
    /// 检测时间 
    /// </summary>
    public float CheckTime;

    /// <summary>
    /// 运行时间
    /// </summary>
    float nowRunTime;

    private void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        thisRigidbody.detectCollisions = false;
        checkedObjList = new List<GameObject>();
    }

    private void OnEnable()
    {
        thisRigidbody.detectCollisions = true;
        checkedObjList.Clear();
        nowRunTime = 0;
    }

    private void Update()
    {
        nowRunTime += Time.deltaTime;
        if (nowRunTime > CheckTime)
        {
            thisRigidbody.detectCollisions = false;
        }
    }

    public void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {
        this.CallBack = CallBack;
    }

    public void SetColor(Color color)
    {
    
    }

    public void SetForward(Vector3 forward)
    {
        
    }

    public void SetLayerMask(LayerMask layerMask)
    {
        this.layerMask = layerMask;
    }

    public void SetRange(float range)
    {
       
    }

    /// <summary>
    /// 持续触发检测
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        int layer = other.gameObject.layer;
        int targetLayerMask = (int)Math.Pow(2, layer);
        int combineLayerMask = targetLayerMask | layerMask;
        if (layerMask != combineLayerMask)//说明不在检测的层里面
        {
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
            return;
        }
        else
        {
            if (!checkedObjList.Contains(other.gameObject))
            {
                checkedObjList.Add(other.gameObject);
                if (CallBack != null)
                {
                    CallBack(new CollisionHitCallbackStruct() { hitPoint = other.transform.position, targetObj = other.gameObject });
                }
            }
        }
    }
}
