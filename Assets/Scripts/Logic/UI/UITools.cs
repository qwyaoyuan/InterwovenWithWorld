using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UITools
{
    /// <summary>
    /// 查找对象在父对象上
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    /// <returns></returns>
    public static T FindTargetPopup<T>(Transform target)where T:class
    {
        if (target == null)
            return null;
        T t = target.GetComponent<T>();
        if (t == null && target.parent != null)
            t = FindTargetPopup<T>(target.parent);
        return t;
    }
}
