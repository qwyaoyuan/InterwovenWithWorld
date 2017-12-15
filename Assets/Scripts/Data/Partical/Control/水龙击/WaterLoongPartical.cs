using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 水龙击粒子控制
/// </summary>
public class WaterLoongPartical : ParticalControlEntry
{
    /// <summary>
    /// 缸体
    /// </summary>
    Rigidbody rigidbody;
    /// <summary>
    /// 方形触发器
    /// </summary>
    BoxCollider boxCollider;
    /// <summary>
    /// 基础的检测范围 
    /// </summary>
    Vector3 baseColliderSize;
    /// <summary>
    /// 基础的粒子大小
    /// </summary>
    Vector3 baseSize;
    /// <summary>
    /// 碰撞的遮罩层
    /// </summary>
    LayerMask layerMask;
    /// <summary>
    /// 碰撞到对象后回调
    /// </summary>
    Func<CollisionHitCallbackStruct, bool> CallBack;

    /// <summary>
    /// 动画组件
    /// </summary>
    public Animator animator;
    /// <summary>
    /// 水龙击动画监听脚本
    /// </summary>
    WaterLoongAnimatorListen waterLoongAnimatorListion;
    /// <summary>
    /// 已经检测到的集合
    /// </summary>
    List<GameObject> checkedTargetObjList;

    private void Start()
    {
        SetLifeCycle(100000);
        Init(Vector3.zero, Vector3.zero, Color.red, ~0, temp => { Debug.Log(temp.targetObj); return false; }, 2);
    }

    private void Awake()
    {
        checkedTargetObjList = new List<GameObject>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.detectCollisions = false;
        boxCollider = GetComponent<BoxCollider>();
        baseColliderSize = boxCollider.size;
        baseSize = transform.localScale;
        if (animator != null)
        {
            waterLoongAnimatorListion = animator.GetBehaviour<WaterLoongAnimatorListen>();
            if (waterLoongAnimatorListion != null)
            {
                waterLoongAnimatorListion.StateCallBack = AnimatorStateCallBack;
            }
        }
    }

    private void OnEnable()
    {
        if (animator != null)
        {
            waterLoongAnimatorListion = animator.GetBehaviour<WaterLoongAnimatorListen>();
            if (waterLoongAnimatorListion != null)
            {
                waterLoongAnimatorListion.StateCallBack = AnimatorStateCallBack;
            }
        }
    }

    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        //水龙击不需要方向
        transform.position = pos;
        SetLayerMask(layerMask);
        SetCollisionCallback(CollisionCallback);
        SetRange(range);
        rigidbody.detectCollisions = false;
        checkedTargetObjList.Clear();
    }

    /// <summary>
    /// 动画状态回调
    /// </summary>
    /// <param name="enterOrExit"></param>
    public void AnimatorStateCallBack(bool enterOrExit)
    {
        checkedTargetObjList.Clear();
        if (enterOrExit)
        {
            rigidbody.detectCollisions = true;
        }
        else
        {
            rigidbody.detectCollisions = false;
        }
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
        boxCollider.size = baseColliderSize * range;
        transform.localScale = baseSize * range;
    }

    /// <summary>
    /// 持续检测触发
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
            if (!checkedTargetObjList.Contains(other.gameObject))
            {
                checkedTargetObjList.Add(other.gameObject);
                if (CallBack != null)
                {
                    CallBack(new CollisionHitCallbackStruct() { hitPoint = other.transform.position, targetObj = other.gameObject });
                }
            }
        }
    }
}
