using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
///  大地咆哮的碰撞检测脚本
/// </summary>
public class RoarOfEarthCollision : MonoBehaviour, IParticalConduct
{
    /// <summary>
    /// 检测结构
    /// </summary>
    [Serializable]
    public struct CheckStruct
    {
        /// <summary>
        /// 检测的事件点
        /// </summary>
        public float CheckTime;
        /// <summary>
        /// 本次检测的持续时间
        /// </summary>
        public float SustainableTime;
    }

    /// <summary>
    /// 检测结构数组
    /// </summary>
    public CheckStruct[] checkStructs;
    /// <summary>
    /// 当前时间
    /// </summary>
    float nowTime;
    /// <summary>
    /// 已经检测到的对象集合
    /// </summary>
    List<GameObject> checkedObjList;
    /// <summary>
    /// 刚体
    /// </summary>
    Rigidbody myRigidbody;
    /// <summary>
    /// 临时的检测集合
    /// </summary>
    List<CheckStruct> tempCheckStructList;

    /// <summary>
    /// 检测的层 
    /// </summary>
    LayerMask layerMask;
    /// <summary>
    /// 回调
    /// </summary>
    Func<CollisionHitCallbackStruct, bool> CallBack;

    private void Awake()
    {
        tempCheckStructList = new List<CheckStruct>();
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.detectCollisions = false;
        checkedObjList = new List<GameObject>();
    }

    /// <summary>
    /// 显示的时候初始化检测
    /// </summary>
    private void OnEnable()
    {
        nowTime = 0;
        tempCheckStructList = checkStructs.ToList();
        myRigidbody.detectCollisions = false;
    }

    private void Update()
    {
        float tempNowTime = nowTime;
        nowTime += Time.deltaTime;
        if (tempCheckStructList != null && tempCheckStructList.Count > 0)
        {
            CheckStruct checkStruct = tempCheckStructList[0];
            //如果刚刚达到检测点则开启检测
            if (nowTime > checkStruct.CheckTime && tempNowTime < checkStruct.CheckTime)
            {
                checkedObjList.Clear();
                myRigidbody.detectCollisions = true;
            }
            //如果超出检测点则移除检测
            else if (nowTime > checkStruct.CheckTime + checkStruct.SustainableTime)
            {
                checkedObjList.Clear();
                myRigidbody.detectCollisions = false;
                tempCheckStructList.RemoveAt(0);
            }
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

    private void OnTriggerStay(Collider other)
    {
        GameObject targetObj = other.gameObject;
        int layer = targetObj.layer;
        int targetLayerMask = (int)Math.Pow(2, layer);
        int combineLayerMask = targetLayerMask | layerMask;
        if (layerMask == combineLayerMask)//说明在检测的层里面
        {
            if (!checkedObjList.Contains(targetObj))
            {
                checkedObjList.Add(targetObj);
                if (CallBack != null)
                {
                    CallBack(new CollisionHitCallbackStruct() { targetObj = targetObj, hitPoint = targetObj.transform.position });
                }
            }
        }
    }
}

