using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// buff的状态
/// </summary>
public struct BuffState
{
    /// <summary>
    /// buff类型
    /// </summary>
    public readonly EnumStatusEffect statusEffect;
    /// <summary>
    /// 时间
    /// </summary>
    public float Time;
    /// <summary>
    /// 临时数据
    /// </summary>
    public object tempData;

    public BuffState(EnumStatusEffect statusEffect,object data)
    {
        this.statusEffect = statusEffect;
        Time = 0;
        tempData = data;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (!obj.GetType().Equals(typeof(BuffState)))
            return false;
        BuffState target = (BuffState)obj;
        if (target.statusEffect != statusEffect)
            return false;
        if (target.Time != Time)
            return false;
        if (!object.Equals(target.tempData, tempData))
            return false;
        return true;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}


