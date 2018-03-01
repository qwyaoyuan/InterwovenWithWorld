using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// 用于解释字段或有限长度数组字段中每个元素的含义
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
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
        return GetExplan(0);
    }

    /// <summary>
    /// 获取数组指定位置元素的含义
    /// </summary>
    /// <param name="index">数组指定位置</param>
    /// <returns></returns>
    public string GetExplan(int index)
    {
        if (explans != null && explans.Length > index)
            return explans[index];
        return "";
    }

    /// <summary>
    /// 获取字段翻译后的显示
    /// </summary>
    /// <returns></returns>
    public string GetDisplay()
    {
        return GetDisplay(0);
    }

    /// <summary>
    /// 获取字段翻译后的显示
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetDisplay(int index)
    {
        if (explans != null && explans.Length > index)
        {
            string explan = GetExplan(index);
            //尝试使用该字段获取当前的翻译
            //......//
            return explan;
        }
        return "";
    }

    /// <summary>
    /// 获取字段上挂在的特性
    /// </summary>
    /// <param name="fieldInfo"></param>
    /// <returns></returns>
    public static FieldExplanAttribute GetFieldInfoExplan(FieldInfo fieldInfo)
    {
        if (fieldInfo == null)
            return null;
        FieldExplanAttribute fieldExplanAttribute = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
        return fieldExplanAttribute;
    }

    /// <summary>
    /// 获取属性上挂载的特性
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    public static FieldExplanAttribute GetPropertyExplan(PropertyInfo propertyInfo)
    {
        if (propertyInfo == null)
            return null;
        FieldExplanAttribute fieldExplanAttribute = propertyInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
        return fieldExplanAttribute;
    }

    /// <summary>
    /// 设置枚举说明到键值对集合中
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="targetList">要添加的键值对字典集合</param>
    /// <param name="selectIndex">选择下标</param>
    /// <param name="CallBackCheck">选择回调</param>
    public static void SetEnumExplanDic<T>(List<KeyValuePair<T, string>> targetList, int selectIndex = 0, Func<T, bool> CallBackCheck = null)
    {
        Type EnumType = typeof(T);
        if (EnumType.IsEnum)
        {
            IEnumerable<T> enumTaskTypes = Enum.GetValues(typeof(T)).OfType<T>();
            foreach (T enumTarget in enumTaskTypes)
            {
                FieldInfo fieldInfo = EnumType.GetField(enumTarget.ToString());
                if (fieldInfo != null)
                {
                    FieldExplanAttribute fieldExplan = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
                    if (fieldExplan != null)
                    {
                        if (CallBackCheck == null || CallBackCheck(enumTarget))
                        {
                            targetList.Add(new KeyValuePair<T, string>(enumTarget, fieldExplan.GetExplan(selectIndex)));
                        }
                    }
                }
            }
        }
    }
}

/// <summary>
/// 用于解释该字段对应的附加数据类型 
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class TargetTypeExplanAttribute : Attribute
{
    /// <summary>
    /// 目标类型
    /// </summary>
    public EnumTargetType TargetType { get; private set; }

    public TargetTypeExplanAttribute(EnumTargetType targetType)
    {
        this.TargetType = targetType;
    }

    /// <summary>
    /// 对象的类型
    /// </summary>
    public enum EnumTargetType
    {
        /// <summary>
        /// 空类型,此类型默认为空
        /// </summary>
        Null,
        /// <summary>
        /// 字符串
        /// </summary>
        String,
        /// <summary>
        /// 整形
        /// </summary>
        Int,
        /// <summary>
        /// 浮点型
        /// </summary>
        Float,
        /// <summary>
        /// Bool
        /// </summary>
        Bool
    }

    /// <summary>
    /// 设置枚举说明到键值对集合中
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="targetDic">要添加的字典</param>
    /// <param name="selectIndex">选择下标</param>
    /// <param name="CallBackCheck">选择回调</param>
    public static void SetEnumExplanDic<T>(Dictionary<T, EnumTargetType> targetDic, int selectIndex = 0, Func<T, bool> CallBackCheck = null)
    {
        Type EnumType = typeof(T);
        if (EnumType.IsEnum)
        {
            IEnumerable<T> enumTaskTypes = Enum.GetValues(typeof(T)).OfType<T>();
            foreach (T enumTarget in enumTaskTypes)
            {
                FieldInfo fieldInfo = EnumType.GetField(enumTarget.ToString());
                if (fieldInfo != null)
                {
                    TargetTypeExplanAttribute targetTypeExplan = fieldInfo.GetCustomAttributes(typeof(TargetTypeExplanAttribute), false).OfType<TargetTypeExplanAttribute>().FirstOrDefault();
                    if (targetTypeExplan != null)
                    {
                        if (CallBackCheck == null || CallBackCheck(enumTarget))
                        {
                            targetDic.Add(enumTarget, targetTypeExplan.TargetType);
                        }
                    }
                }
            }
        }
    }

}

/// <summary>
/// 用于限定枚举的范围
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class EnumRangeAttribute : Attribute
{
    int from;
    int to;

    public EnumRangeAttribute(int from, int to)
    {
        this.from = from;
        this.to = to;
    }

    public T GetFrom<T>()
    {
        try
        {
            string value = from.ToString();
            return (T)Enum.Parse(typeof(T), value);
        }
        catch
        {
            return default(T);
        }
    }

    public T GetTo<T>()
    {
        try
        {
            string value = to.ToString();
            return (T)Enum.Parse(typeof(T), value);
        }
        catch
        {
            return default(T);
        }
    }

    public T[] GetCanUseEnum<T>()
    {
        int fromInt = 0;
        int toInt = 0;
        try
        {
            string value = from.ToString();
            fromInt = (int)Enum.Parse(typeof(T), value);
        }
        catch { }
        try
        {
            string value = to.ToString();
            toInt = (int)Enum.Parse(typeof(T), value);
        }
        catch { }
        List<T> resultList = new List<T>();
        foreach (T item in Enum.GetValues(typeof(T)))
        {
            int thisInt = int.MinValue;
            try
            {
                string value = item.ToString();
                thisInt = (int)Enum.Parse(typeof(T), value);
            }
            catch { }
            if (thisInt != int.MinValue && thisInt <= toInt && thisInt >= fromInt)
            {
                resultList.Add(item);
            }
        }
        return resultList.ToArray();
    }
}