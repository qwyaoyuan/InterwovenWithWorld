using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 下拉列表的焦点
/// </summary>
public class UIFocusDropdown : UIFocus
{
    /// <summary>
    /// 是否使用左右方向控制选项变化
    /// </summary>
    public bool LeftRightChangeItem;
    /// <summary>
    /// 是否循环
    /// </summary>
    public bool cicle;

    /// <summary>
    /// 下拉列表
    /// </summary>
    public Dropdown dropdown;

    void Awake()
    {
        dropdown = GetComponent<Dropdown>();
    }

    /// <summary>
    /// 设置焦点
    /// </summary>
    public override void SetForcus()
    {
        if (dropdown)
        {
            dropdown.Select();
        }
    }

    /// <summary>
    /// 是否可以移动
    /// </summary>
    /// <param name="moveType"></param>
    /// <returns></returns>
    public override bool CanMoveNext(UIFocusPath.MoveType moveType)
    {
        if (!dropdown)
            return true;
        if (LeftRightChangeItem)//左右控制内部切换
            return moveType != UIFocusPath.MoveType.LEFT && moveType != UIFocusPath.MoveType.RIGHT;
        else
            return moveType == UIFocusPath.MoveType.LEFT || moveType == UIFocusPath.MoveType.RIGHT;
    }

    /// <summary>
    /// 移动子选项
    /// </summary>
    /// <param name="moveType"></param>
    public override void MoveChild(UIFocusPath.MoveType moveType)
    {
        if (dropdown)
        {
            int add = 0;
            if (LeftRightChangeItem)
                add = moveType == UIFocusPath.MoveType.LEFT ? -1 : (moveType == UIFocusPath.MoveType.RIGHT ? 1 : 0);
            else
                add = moveType == UIFocusPath.MoveType.UP ? -1 : (moveType == UIFocusPath.MoveType.DOWN ? 1 : 0);
            List<Dropdown.OptionData> datas = dropdown.options;
            int length = datas.Count;
            int value = dropdown.value;
            value += add;
            if (cicle)
                if (value >= length)
                    value = 0;
                else if (value < 0)
                    value = length - 1;
            if (length > 0)
            {
                if (value >= 0 && value < length)
                {
                    dropdown.value = value;
                }
            }
        }
    }

}
