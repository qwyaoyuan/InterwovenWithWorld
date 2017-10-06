using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 输入类型
/// </summary>
public enum EnumInputType
{
    #region 手柄按键
    /* 这四个按键用于组合释放技能*/
    /// <summary>
    /// 手柄上键
    /// </summary>
    Up = 0,
    /// <summary>
    /// 手柄左键
    /// </summary>
    Left = 1,
    /// <summary>
    /// 手柄右键
    /// </summary>
    Right = 2,
    /// <summary>
    /// 手柄下键
    /// </summary>
    Down = 3,
    /*这四个键用于一些基础只能，也可以组合操作*/
    /// <summary>
    /// 手柄A键
    /// </summary>
    RA = 4,
    /// <summary>
    /// 手柄B键
    /// </summary>
    RB = 5,
    /// <summary>
    /// 手柄X键
    /// </summary>
    RX  = 6,
    /// <summary>
    /// 手柄Y键
    /// </summary>
    RY = 7,
    /*手柄的左右摇杆按下键位，可以组合也可以基础操作*/
    /// <summary>
    /// 手柄左侧摇杆按下
    /// </summary>
    L3 = 8,
    /// <summary>
    /// 手柄右侧摇杆按下
    /// </summary>
    R4 = 9,
    /*这四个按键用于和上面的键进行组合*/
    /// <summary>
    /// L1键
    /// </summary>
    L1 = 16,
    /// <summary>
    /// R1键
    /// </summary>
    R1 = 32,
    /// <summary>
    /// L2键
    /// </summary>
    L2 = 64,
    /// <summary>
    /// R2键
    /// </summary>
    R2 = 128,
    #endregion

    #region 键盘按键,计算出的数值是上面的复合数值

    #endregion
}
