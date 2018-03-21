using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  闪电冲击的检测脚本
/// </summary>
public class LightningImpactCollision : MonoBehaviour, IParticalConduct
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
    /// 检测后生成的粒子
    /// </summary>
    public GameObject collisionObj;

    /// <summary>
    /// 检测的间隔时间
    /// </summary>
    public float checkIntervalTime;


    private void OnEnable()
    {
        Invoke("LightningImpact_Shot", 0.1f);
    }

    /// <summary>
    /// 发射
    /// </summary>
    void LightningImpact_Shot()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit[] rchs = Physics.RaycastAll(ray, range, layerMask);
        rchs = Physics.CapsuleCastAll(transform.position, transform.position + transform.forward, 0.5f, transform.forward, range * 10);
        if (rchs != null && rchs.Length > 0 && CallBack != null)
        {
            List<GameObject> objList = new List<GameObject>();
            foreach (RaycastHit rch in rchs)
            {
                if (objList.Contains(rch.transform.gameObject))
                {
                    continue;
                }
                objList.Add(rch.transform.gameObject);
                bool result = CallBack(new CollisionHitCallbackStruct() { targetObj = rch.transform.gameObject, hitPoint = rch.point });
                if (result)
                {
                    //生成粒子
                    GameObject go = GameObject.Instantiate(collisionObj);
                    Destroy(go, 5);
                    go.transform.position = rch.point;
                    ParticalControlEntry particalControlEntry = go.GetComponent<ParticalControlEntry>();
                    if (particalControlEntry)
                    {
                        particalControlEntry.Init(rch.point, transform.forward, Color.red, layerMask, null, 1);//这里的范围不起作用
                    }
                    else
                    {
                        go.transform.forward = -transform.forward;
                    }
                }
            }
        }
        Invoke("LightningImpact_Shot", checkIntervalTime);
    }

    private void OnDisable()
    {
        CancelInvoke("LightningImpact_Shot");
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
        this.range = range;
    }

    public void Open()
    {
       
    }
}
