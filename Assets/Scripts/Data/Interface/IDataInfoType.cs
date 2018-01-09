using System;
using UnityEngine;


/// <summary>
/// 数据对象类型接口,此接口仅仅描述数据类型 
/// </summary>
public interface IDataInfoType
{
    /// <summary>
    /// 基础类型
    /// </summary>
    Type T { get; }
}

/// <summary>
/// 封装的用于隐藏Awake函数的类
/// </summary>
public abstract class TempSealedDataInfoType : MonoBehaviour
{
    protected abstract void Awake();
}

/// <summary>
/// 用于挂载在游戏对象身上的对象
/// </summary>
/// <typeparam name="U"></typeparam>
public class DataInfoType<U> : TempSealedDataInfoType, IDataInfoType
{
    /// <summary>
    /// 基础类型
    /// </summary>
    Type baseType;
    /// <summary>
    /// 基础类型
    /// </summary>
    public Type T
    {
        get { return baseType; }
    }

    protected sealed override void Awake()
    {
        baseType = typeof(U);
        InnerAwake();
    }

    protected virtual void InnerAwake() { }
}

