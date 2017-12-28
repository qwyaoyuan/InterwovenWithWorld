using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 攻击AI的数据结构
/// </summary>
[Serializable]
public struct AttackAIDataStruct
{
    /// <summary>
    /// 攻击的距离(最好留有一定的余地)
    /// </summary>
    public float AttackDistance;
    /// <summary>
    /// 攻击的最小角度
    /// </summary>
    public float AttackAngle;
    /// <summary>
    /// 攻击的ID,用于内部判断处理
    /// </summary>
    public int AttackID;
    /// <summary>
    /// 本次攻击的冷却时间
    /// </summary>
    public float CoolTime;
    /// <summary>
    /// 本次攻击的持续时间
    /// </summary>
    public float AttackDurationTime;
    /// <summary>
    /// 最大等待时间(如果到等待时间还没有攻击则放弃本次攻击)
    /// </summary>
    public float MaxWaitTime;
    /// <summary>
    /// 本次攻击后的等待时间 
    /// </summary>
    public float ThisAttackNextWaitTime;

    /// <summary>
    /// 用于外部计算的剩余冷却时间
    /// </summary>
    public float TempCoolTime;
    /// <summary>
    /// 用于外部计算的剩余最大等待时间
    /// </summary>
    public float TempMaxWaitTime;
}

