using System;

/// <summary>
/// 技能释放类型
/// </summary>
public enum EnumReleaseType
{
    /// <summary>
    /// 直接释放
    /// </summary>
    [FieldExplan("非组合技能")]
    Direct,
    /// <summary>
    /// 组合释放
    /// </summary>
    [FieldExplan("组合技能")]
    Combination
}
