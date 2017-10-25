/// <summary>
/// 合成的条目（于熟练度挂钩）
/// 这里指的是具体条目
/// </summary>
public enum EnumSynthesisItem
{
    /// <summary> 
    /// 武器类型
    /// </summary>
    [FieldExplan("武器")]
    Weapon,
    /// <summary>
    /// 防具类型
    /// </summary>
    [FieldExplan("防具")]
    Armor,
    /// <summary>
    /// 打造材料
    /// </summary>
    [FieldExplan("打造材料")]
    Make,
}