using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 装备格子的焦点
/// </summary>
public class UIFocusItemEquipmentLattice : UIFocus
{
    /// <summary>
    /// 装备的范围（最小值）
    /// 使用EnumItemType枚举
    /// </summary>
    public int minType;
    /// <summary>
    /// 装备的范围（最大值）
    /// 使用EnumItemType枚举
    /// </summary>
    public int maxType;
    /// <summary>
    /// 装备的id
    /// </summary>
    public int id;
    /// <summary>
    /// 背景边框图品
    /// </summary>
    public Image backLineImage;

    private void Awake()
    {
        backLineImage.enabled = false;
    }

    /// <summary>
    /// 设置焦点
    /// </summary>
    public override void SetForcus()
    {
        //设置当前显示选中的装备id
        backLineImage.enabled = true;
    }

    /// <summary>
    /// 失去焦点
    /// </summary>
    public override void LostForcus()
    {
        //设置没有选中装备
        backLineImage.enabled = false;
    }

}
