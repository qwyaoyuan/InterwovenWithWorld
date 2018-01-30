using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 怪物类型枚举
/// </summary>
public enum EnumMonsterType
{
    /// <summary>
    /// 小猪
    /// </summary>
    [FieldExplan("小猪")]
    LittlePig,
    /// <summary>
    /// 骷髅王
    /// </summary>
	[FieldExplan("骷髅王")]
    SkullKing,
    /// <summary>
    /// 骷髅
    /// </summary>
    [FieldExplan("骷髅")]
    Skull,
    /// <summary>
    /// 土匪
    /// </summary>
    [FieldExplan("土匪")]
    Bandit
}

/// <summary>
/// 怪物AI类型
/// </summary>
public enum EnumMonsterAIType
{
    /// <summary>
    /// 触发型
    /// </summary>
    [FieldExplan("触发型")]
    Trigger,
    /// <summary>
    /// 巡逻型
    /// </summary>
    [FieldExplan("巡逻型")]
    GoOnPatrol,
    /// <summary>
    /// Boss类型
    /// </summary>
    [FieldExplan("Boss")]
    Boss,
}


/// <summary>
/// boss的行动状态
/// </summary>
public enum EnumBossActionType
{
    /// <summary>
    /// 待机
    /// </summary>
    Standby,
    /// <summary>
    /// 攻击
    /// </summary>
    Attack,
    /// <summary>
    /// 防御
    /// </summary>
    Defense,
    /// <summary>
    /// 死亡
    /// </summary>
    Death
}