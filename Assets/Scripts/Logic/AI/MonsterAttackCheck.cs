using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// 怪物攻击检测脚本
/// </summary>
public class MonsterAttackCheck : MonoBehaviour
{
    /// <summary>
    /// 返回触发的交互接口对象
    /// </summary>
    public Action<IObjInteractive> CallBack;

    /// <summary>
    /// 检测的层
    /// </summary>
    public LayerMask checkLayer;

    /// <summary>
    /// 持续触发
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (CallBack == null)
            return;
        int otherLayer = (int)Mathf.Pow(2, other.gameObject.layer);
        int result = checkLayer.value | otherLayer;
        if (result == checkLayer.value)//包含检测的层
        {
            IObjInteractive iObjInteractive = other.gameObject.GetComponent<IObjInteractive>();
            if (iObjInteractive != null)
                CallBack(iObjInteractive);
        }
    }
}

