/// <summary>
/// 技能类型枚举
/// </summary>
public enum EnumSkillType
{
    /// <summary>
    /// 没有技能
    /// </summary>
    None = 0,

    #region 特殊技能
    SpecialReleaseStart = 1,
    /// <summary>
    /// 释放魔法
    /// </summary>
    MagicRelease =199,
    SpecialReleaseEnd = 200,
    #endregion

    #region 被动技能
    PassiveStart = 300,
    PassiveEnd = 500,
    #endregion

    #region 光环技能
    SpecialCircleStart = 600,
    SpecialCircleEnd = 800,
    #endregion

    #region 魔法技能
    MagicStart = 1000,
    #region 需要结合的技能(1阶段)
    MagicCombinedLevel1Start = 1000,
    MagicCombinedLevel1End = 1100,
    #endregion
    #region 需要结合的技能(2阶段)
    MagicCombinedLevel2Start = 1100,
    MagicCombinedLevel2End = 1200,
    #endregion
    #region 需要结合的技能(3阶段)
    MagicCombinedLevel3Start = 1200,
    MagicCombinedLevel3End = 1300,
    #endregion
    #region 需要结合的技能(4阶段)
    MagicCombinedLevel4Start = 1300,
    MagicCombinedLevel4End = 1400,
    #endregion

    /*组合技能的方法是：各个阶段减去它们的开始枚举数值，根据不同的阶段乘以100的n次方，然后相加，最终结果加1亿*/

    EndMagic = 200000000,
    #endregion



    /*物理技能*/
    PhysicsStart = 200000002,
}
