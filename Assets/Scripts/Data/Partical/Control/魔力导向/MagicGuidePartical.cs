using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔力导向粒子控制
/// </summary>
public class MagicGuidePartical : ParticalControlEntry
{
    /// <summary>
    /// 当碰触到对象时触发的粒子
    /// </summary>
    public GameObject CollisionPartical;

    /// <summary>
    /// 碰撞后的回调
    /// </summary>
    Func<CollisionHitCallbackStruct, bool> CollisionCallback;

    /// <summary>
    /// 目标
    /// </summary>
    private GameObject target;

    /// <summary>
    /// 碰撞后粒子的大小
    /// </summary>
    private float collisionRange;
    /// <summary>
    /// 碰撞后的粒子的颜色
    /// </summary>
    private Color collisionColor;
    /// <summary>
    /// 朝向(该朝向表示从发射地到目标)
    /// </summary>
    private Vector3 collisionForward;

    public override void Init(Vector3 pos, Vector3 forward, Color color, LayerMask layerMask, Func<CollisionHitCallbackStruct, bool> CollisionCallback, float range, params GameObject[] targetObjs)
    {
        transform.position = pos;
        if (targetObjs.Length > 0)
        {
            forward = (targetObjs[0].transform.position - transform.position).normalized;
            collisionForward = forward;
            //魔力导向的Range表示的是首尾长度
            float distance = Vector3.Distance(pos, targetObjs[0].transform.position);
            SetRange(distance);
            target = targetObjs[0];
        }
        else
        {
            collisionForward = Vector3.down;
            SetRange(20);//如果没有目标则长度为20
        }
        collisionRange = range;
        collisionColor = color;
        transform.forward = forward;
        SetForward(forward);
        SetLayerMask(layerMask);
        SetColor(color);
        this.CollisionCallback = CollisionCallback;
        Invoke("MagicGuidePartical_CallBack", 0.2f);
    }

    public void MagicGuidePartical_CallBack()
    {
        CollisionHitCallbackStruct data = new CollisionHitCallbackStruct() { targetObj = target, hitPoint = target ? target.transform.position : Vector3.zero };
        //真正的回调
        bool result = false;
        if(CollisionCallback!=null)
            result = CollisionCallback(data);
        //如果结果判定可以触发效果则触发
        if (result  && CollisionCallback != null)
        {
            //在该位置生成粒子
            if (CollisionPartical)
            {
                GameObject go = GameObject.Instantiate(CollisionPartical);
                go.transform.position = data.hitPoint + Vector3.up *1.5f;
                ParticalControlEntry collisionEntry = go.GetComponent<ParticalControlEntry>();
                if (collisionEntry)
                {
                    collisionEntry.Init(go.transform.position, collisionForward, collisionColor, 0, null, collisionRange, target);
                }
            }
        }
    }

    //public GameObject testObj;

    //private void Start()
    //{
    //    //魔力导向的配置
    //    Init(Vector3.up, Vector3.forward, new Color(1f, 1f, 1f, 0.02f), ~1, temp => true, 1, testObj);

    //    Init(Vector3.up, Vector3.forward, new Color(1, 0, 0, 1), ~1, temp => true, 1f, testObj);
    //}
}
