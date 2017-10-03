using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// linq工具
/// </summary>
public static class LinqTools
{
    /// <summary>
    /// 截取数组的指定长度
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="index"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static T[] SelectToIndex<T>(this T[] source, int index = 0, int count = 0)
    {
        if (index < source.Length && index + count <= source.Length)
        {
            T[] ts = new T[count];
            for (int i = 0; i < count; i++)
            {
                ts[i] = source[i + index];
            }
            return ts;
        }
        return new T[0];
    }

    /// <summary>
    /// 合并数组集合中的每一数组中的数到一个数组中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T[] Combine<T>(this IEnumerable<T[]> source)
    {
        List<T> result = new List<T>();
        foreach (T[] item in source)
        {
            result.AddRange(item);
        }
        return result.ToArray();
    }

}
