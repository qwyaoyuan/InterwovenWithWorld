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
    /// <summary>
    /// 武器类型,表示的是图标在左手还是右手
    /// </summary>
    public EnumWeaponType handedType;
    /// <summary>
    /// 数据对象
    /// </summary>
    public object value;

    /// <summary>
    /// 武器类型
    /// </summary>
    public enum EnumWeaponType
    {
        /// <summary>
        /// 什么都不是
        /// </summary>
        None = 1,
        /// <summary>
        /// 单手副手武器
        /// </summary>
        LeftOneHanded = 2,
        /// <summary>
        /// 单手主手武器
        /// </summary>
        RightOneHanded = 4
    }

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
