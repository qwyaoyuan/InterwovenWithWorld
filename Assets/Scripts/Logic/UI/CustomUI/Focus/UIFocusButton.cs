using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 按钮获取焦点
/// </summary>
public class UIFocusButton : UIFocus
{
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public override void SetForcus()
    {
        if (button)
        {
            button.Select();
        }
    }

    /// <summary>
    /// 激活点击事件
    /// </summary>
    public void ClickThisButton()
    {
        if (button)
            button.onClick.Invoke();
    }
}
