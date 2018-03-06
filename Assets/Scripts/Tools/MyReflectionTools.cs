using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

/// <summary>
/// 反射相关的工具
/// </summary>
public static class MyReflectionTools
{
    /// <summary>
    /// 获取指定类型的指定类型属性
    /// </summary>
    /// <typeparam name="U">对象类型</typeparam>
    /// <typeparam name="T">属性类型</typeparam>
    /// <returns></returns>
    public static PropertyInfo[] GetPropertys<U, T>()
    {
        Type uType = typeof(U);
        PropertyInfo[] temp1 = uType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        return temp1.Where(temp => Type.Equals(temp.PropertyType, typeof(T))).ToArray();
    }
}

