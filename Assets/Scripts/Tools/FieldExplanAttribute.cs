using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 用于解释字段或有限长度数组字段中每个元素的含义
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class FieldExplanAttribute : Attribute
{
    /// <summary>
    /// 存储解释内容的数组
    /// </summary>
    string[] explans;

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="explans">要解释的内容，如果这是一个数组，则第一个元素表示元素自身的内容</param>
    public FieldExplanAttribute(params string[] explans)
    {
        this.explans = explans;
    }

    /// <summary>
    /// 获取字段的含义
    /// </summary>
    /// <returns></returns>
    public string GetExplan()
    {
        return GetExplan(-1);
    }

    /// <summary>
    /// 获取数组指定位置元素的含义
    /// </summary>
    /// <param name="index">数组指定位置</param>
    /// <returns></returns>
    public string GetExplan(int index)
    {
        if (explans != null && explans.Length > index + 1)
            return explans[index + 1];
        return "";
    }
}
