using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 玩家的状态
/// 除了基本的属性(根据等级 被动技能计算出的属性)
/// 以及临时属性(技能buff 光环 debuff等)
/// </summary>
public interface IPlayerAttributeState : IAttributeState
{
    /// <summary>
    /// 创建一个状态句柄,返回句柄的id
    /// </summary>
    /// <returns></returns>
    int CreateAttributeHandle();
    /// <summary>
    /// 通过句柄获取对应的状态对象
    /// </summary>
    /// <param name="handle"></param>
    /// <returns></returns>
    IAttributeState GetAttribute(int handle);
    /// <summary>
    /// 使用指定的句柄id创建一个状态
    /// 如果存在这个句柄了,则不会创建(这个一般不要用)
    /// </summary>
    /// <param name="index">句柄ID</param>
    /// <returns>返回是否创建成功</returns>
    bool CreateAttributeHandle(int index);
    /// <summary>
    /// 获取合计属性
    /// </summary>
    /// <returns></returns>
    IAttributeState GetResultAttribute();
    /// <summary>
    /// 移除一个状态,注意只能移除大于零的句柄
    /// </summary>
    /// <param name="handle"></param>
    void RemoveAttribute(int handle);
}

