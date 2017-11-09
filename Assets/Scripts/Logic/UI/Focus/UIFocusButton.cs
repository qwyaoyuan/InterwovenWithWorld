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
}
