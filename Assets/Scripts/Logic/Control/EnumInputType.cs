using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 输入类型
/// </summary>
public enum EnumInputType
{
    None = 0,
    #region 手柄按键
    /* 这四个按键用于组合释放技能*/
    /// <summary>
    /// 手柄上键
    /// </summary>
    Up = 1,
    /// <summary>
    /// 手柄左键
    /// </summary>
    Left = 2,
    /// <summary>
    /// 手柄右键
    /// </summary>
    Right = 3,
    /// <summary>
    /// 手柄下键
    /// </summary>
    Down = 4,
    /*这四个键用于一些基础只能，也可以组合操作*/
    /// <summary>
    /// 手柄A键
    /// </summary>
    A = 5,
    /// <summary>
    /// 手柄B键
    /// </summary>
    B = 6,
    /// <summary>
    /// 手柄X键
    /// </summary>
    X = 7,
    /// <summary>
    /// 手柄Y键
    /// </summary>
    Y = 8,
    /*手柄的左右摇杆按下键位，可以组合也可以基础操作*/
    /// <summary>
    /// 手柄左侧摇杆按下
    /// </summary>
    L3 = 9,
    /// <summary>
    /// 手柄右侧摇杆按下
    /// </summary>
    R3 = 10,
    /*
     * 这四个按键用于和上面的键进行组合
     * 这四个按键必须是下面的数值
     * L1=16
     * R1=32
     * L2=64
     * R2=128
     */
    /// <summary>
    /// L1键
    /// </summary>
    LB = 16,
    /// <summary>
    /// R1键
    /// </summary>
    RB = 32,
    /// <summary>
    /// L2键
    /// </summary>
    LT = 64,
    /// <summary>
    /// R2键
    /// </summary>
    RT = 128,
    #endregion

    #region 键盘按键,计算出的数值是上面的复合数值

    #endregion
}
