/// <summary>
/// 道具类型
/// </summary>
public enum EnumItemType
{
    #region 素材类
    /// <summary>
    /// 素材类的起始
    /// </summary>
    [FieldExplan("素材")]
    SourceMaterial = 1000000,
    #region 矿石大类
    /// <summary>
    /// 矿石大类
    /// </summary>
    [FieldExplan("矿石大类")]
    KuangShiLeiBig = 1100000,
    #region 矿石小类
    /// <summary>
    /// 矿石小类
    /// </summary>
    [FieldExplan("矿石小类")]
    KuangShiLeiLittle = 1101000,
    #region 具体的矿石
    /// <summary>
    /// 铁矿石
    /// </summary>
    [FieldExplan("铁矿石")]
    TieKuangShi= 1101001,
    #endregion
    #endregion
    #endregion
    #endregion
}