/// <summary>
/// 物品品质
/// </summary>
public enum EnumQualityType
{
    /// <summary>
    /// 白色
    /// </summary>
    [FieldExplan("普通（白色）")]
    White,
    /// <summary>
    /// 绿色
    /// </summary>
    [FieldExplan("绿色（罕见）")]
    Green,
    /// <summary>
    /// 蓝色
    /// </summary>
    [FieldExplan("蓝色（稀有）")]
    Blue,
    /// <summary>
    /// 紫色
    /// </summary>
    [FieldExplan("紫色（传说）")]
    Purple,
    /// <summary>
    /// 黄色
    /// </summary>
    [FieldExplan("黄色（史诗）")]
    Yellow,
    /// <summary>
    /// 红色 - 唯一道具
    /// </summary>
    [FieldExplan("红色（唯一）")]
    Red
}