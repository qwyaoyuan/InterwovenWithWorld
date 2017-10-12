/// <summary>
/// 状态效果
/// 主要是buff debuff
/// </summary>
public enum EnumStatusEffect
{
    [FieldExplan("无特殊效果")]
    None = 0,
    #region Debuff 10个
    /// <summary>
    /// 冰冻
    /// </summary>
    [FieldExplan("冰冻")]
    bd1 = 1,
    /// <summary>
    /// 迟钝
    /// </summary>
    [FieldExplan("迟钝")]
    cd1,
    /// <summary>
    /// 点燃
    /// </summary>
    [FieldExplan("点燃")]
    dr1,
    /// <summary>
    /// 凋零
    /// </summary>
    [FieldExplan("凋零")]
    dl3,
    /// <summary>
    /// 减速
    /// </summary>
    [FieldExplan("减速")]
    js4,
    /// <summary>
    /// 迷惑
    /// </summary>
    [FieldExplan("迷惑")]
    mh3,
    /// <summary>
    /// 无力
    /// </summary>
    [FieldExplan("无力")]
    wl1,
    /// <summary>
    /// 虚弱
    /// </summary>
    [FieldExplan("虚弱")]
    xr2,
    /// <summary>
    /// 中毒
    /// </summary>
    [FieldExplan("中毒")]
    zd2,
    /// <summary>
    /// 诅咒
    /// </summary>
    [FieldExplan("诅咒")]
    zz3,
    #endregion
    #region Buff 9个
    /// <summary>
    /// 洞察
    /// </summary>
    [FieldExplan("洞察")]
    dc2 = 100,
    /// <summary>
    /// 活力
    /// </summary>
    [FieldExplan("活力")]
    hl2,
    /// <summary>
    /// 加速
    /// </summary>
    [FieldExplan("加速")]
    js1,
    /// <summary>
    /// 净化
    /// </summary>
    [FieldExplan("净化")]
    jh5,
    /// <summary>
    /// 敏捷
    /// </summary>
    [FieldExplan("敏捷")]
    mj1,
    /// <summary>
    /// 强力
    /// </summary>
    [FieldExplan("强力")]
    ql1,
    /// <summary>
    /// 驱散
    /// </summary>
    [FieldExplan("驱散")]
    qs2,
    /// <summary>
    /// 睿智
    /// </summary>
    [FieldExplan("睿智")]
    rz1,
    /// <summary>
    /// 吸血(物理)
    /// </summary>
    [FieldExplan("吸血(物理)")]
    xx3,
    /// <summary>
    /// 吸血(魔法)
    /// </summary>
    [FieldExplan("吸血(魔法)")]
    xx4,
    #endregion

    #region 特殊状态 10个
    /// <summary>
    /// 嘲讽
    /// </summary>
    [FieldExplan("嘲讽")]
    cf2 =200,
    /// <summary>
    /// 混乱
    /// </summary>
    [FieldExplan("混乱")]
    hl1,
    /// <summary>
    /// 禁锢
    /// </summary>
    [FieldExplan("禁锢")]
    jg2,
    /// <summary>
    /// 禁魔
    /// </summary>
    [FieldExplan("禁魔")]
    jm3,
    /// <summary>
    /// 僵直
    /// </summary>
    [FieldExplan("僵直")]
    jz6,
    /// <summary>
    /// 恐惧
    /// </summary>
    [FieldExplan("恐惧")]
    kj1,
    /// <summary>
    /// 麻痹
    /// </summary>
    [FieldExplan("麻痹")]
    mb2,
    /// <summary>
    /// 魅惑
    /// </summary>
    [FieldExplan("魅惑")]
    mh4,
    /// <summary>
    /// 眩晕
    /// </summary>
    [FieldExplan("眩晕")]
    xy1,
    /// <summary>
    /// 致盲
    /// </summary>
    [FieldExplan("致盲")]
    zm1,
    #endregion

}
