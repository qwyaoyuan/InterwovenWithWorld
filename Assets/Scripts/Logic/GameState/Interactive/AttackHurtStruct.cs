using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击伤害结构
/// </summary>
public struct AttackHurtStruct 
{
    /// <summary>
    /// 伤害传递次数,主要是为了保证这是第一次传递,如果两边都有反伤则不会造成死锁
    /// </summary>
    public int hurtTransferNum;
    /// <summary>
    /// 物理伤害
    /// </summary>
    public int physicsHurt;
    /// <summary>
    /// 物理附加伤害(最终伤害附加)
    /// </summary>
    public int addPhysicsHurt;
    /// <summary>
    /// 魔法伤害
    /// </summary>
    public int magicHurt;
    /// <summary>
    /// 魔法伤害附加(最终伤害附加)
    /// </summary>
    public int addMagicHurt;
    /// <summary>
    /// 附加状态数组
    /// </summary>
    public StatusDataInfo.StatusLevelDataInfo[] statusLevelDataInfos;
    /// <summary>
    /// 伤害来自对象
    /// </summary>
    public GameObject hurtFromObj;
}

/// <summary>
/// 计算伤害的静态类
/// </summary>
public static class CalculateHurt
{ }
