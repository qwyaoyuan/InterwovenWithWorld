using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 魔力脉冲的检测
/// </summary>
public class MagicPulseCollision : MonoBehaviour, IParticalConduct
{

    /// <summary>
    /// 距离
    /// </summary>
    float range;
    /// <summary>
    /// 遮罩
    /// </summary>
    LayerMask layerMask;
    /// <summary>
    /// 碰撞后的回调
    /// </summary>
    Func<CollisionHitCallbackStruct, bool> CallBack;
    /// <summary>
    /// 颜色
    /// </summary>
    Color color;

    /// <summary>
    /// 是否只检测最近的一个
    /// </summary>
    public bool onlyNear;

    /// <summary>
    /// 检测后生成的粒子
    /// </summary>
    public GameObject collisionObj;

    public void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {
        this.CallBack = CallBack;
    }

    public void SetColor(Color color)
    {
        this.color = color;
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
        this.range = range;
    }

    /// <summary>
    /// 在显示的时候检测一次
    /// </summary>
    private void OnEnable()
    {
        if (range > 0 && CallBack != null)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit[] rchs = Physics.RaycastAll(ray, range, layerMask);
            List<GameObject> objsList = new List<GameObject>();
            if (rchs != null && rchs.Length > 0)
            {
                rchs = rchs.OrderBy(temp => Vector3.Distance(temp.point, transform.position)).ToArray();
                foreach (RaycastHit rch in rchs)
                {
                    if (objsList.Contains(rch.transform.gameObject))
                    {
                        continue;
                    }
                    objsList.Add(rch.transform.gameObject);
                    bool result = CallBack(new CollisionHitCallbackStruct() { targetObj = rch.transform.gameObject, hitPoint = rch.point });
                    if (result)
                    {
                        //生成粒子
                        GameObject go = GameObject.Instantiate(collisionObj);
                        go.transform.position = rch.point;
                        ParticalControlEntry particalControlEntry = go.GetComponent<ParticalControlEntry>();
                        if (particalControlEntry)
                        {
                            particalControlEntry.Init(rch.point, transform.forward, color, layerMask, CallBack, 1);//这里的范围不起作用
                        }
                        else
                        {
                            go.transform.forward = -transform.forward;
                        }
                        if (onlyNear)//只生成第一个
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
}
