using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能类型枚举
/// </summary>
public enum EnumSkillType
{
    /// <summary>
    /// 没有技能
    /// </summary>
    None = 0,

    #region 魔法技能
    MagicStart = 1,
    /// <summary>
    /// 释放魔法
    /// </summary>
    MagicRelease,
    #region 需要结合的技能(1阶段)
    MagicCombinedLevel1Start = 100,
    MagicCombinedLevel1End = 200,
    #endregion
    #region 需要结合的技能(2阶段)
    MagicCombinedLevel2Start = 200,
    MagicCombinedLevel2End = 300,
    #endregion
    #region 需要结合的技能(3阶段)
    MagicCombinedLevel3Start = 300,
    MagicCombinedLevel3End = 400,
    #endregion
    #region 需要结合的技能(4阶段)
    MagicCombinedLevel4Start = 400,
    MagicCombinedLevel4End = 500,
    #endregion

    #region 常规直接释放技能
    MagicNormalStart = 1000,
    MagicNormalEnd = 10000,
    #endregion

    #region 组合后直接释放技能
    MagicCombinedStart = 100000000,
    MagicCombinedEnd = 200000000,
    #endregion

    EndMagic = 200000000,
    #endregion

    /*物理技能*/
    PhysicsStart = 200000002,
}
