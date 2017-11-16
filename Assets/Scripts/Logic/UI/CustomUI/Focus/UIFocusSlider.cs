using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFocusSlider : UIFocus
{
    /// <summary>
    /// 是否使用左右方向控制值变化
    /// </summary>
    public bool LeftRightChangeValue;
    /// <summary>
    /// 变化量
    /// </summary>
    public float variation;

    /// <summary>
    /// 滑杆
    /// </summary>
    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    /// <summary>
    /// 设置焦点
    /// </summary>
    public override void SetForcus()
    {
        if (slider)
            slider.Select();
    }

    /// <summary>
    /// 是否可以移动
    /// </summary>
    /// <param name="moveType"></param>
    /// <returns></returns>
    public override bool CanMoveNext(UIFocusPath.MoveType moveType)
    {
        if (!slider)
            return true;
        if (LeftRightChangeValue)//左右控制内部切换
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
        if (slider)
        {
            float _variation = variation;
            if (LeftRightChangeValue)
                _variation *= moveType == UIFocusPath.MoveType.LEFT ? -1 : (moveType == UIFocusPath.MoveType.RIGHT ? 1 : 0);
            else
                _variation *= moveType == UIFocusPath.MoveType.UP ? -1 : (moveType == UIFocusPath.MoveType.DOWN ? 1 : 0);
            float value = slider.value;
            value += _variation;
            value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
            slider.value = value;
        }
    }

}
