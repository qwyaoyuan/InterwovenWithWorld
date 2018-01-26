using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 商店界面的提示面板(主要是提供确认功能)
/// </summary>
public class UIShopTip : MonoBehaviour
{

    /// <summary>
    /// 按钮的焦点
    /// </summary>
    public UIFocusPath uiFocusPath;

    /// <summary>
    /// 结果回调
    /// </summary>
    public Action<bool> ResultHandle;

    /// <summary>
    /// 当前的UI焦点
    /// </summary>
    UIFocus nowUIFocus;

    /// <summary>
    /// 显示并返回结果
    /// </summary>
    /// <param name="ResultCallback"></param>
    public void Show(Action<bool> ResultCallback)
    {
        this.ResultHandle = ResultCallback;
        this.gameObject.SetActive(true);  
    }

    private void OnEnable()
    {
        nowUIFocus = uiFocusPath.GetFirstFocus();
        if (nowUIFocus)
        {
            nowUIFocus.SetForcus();
        }
    }

    private void OnDisable()
    {
        this.ResultHandle = null;
    }

    /// <summary>
    /// 获取焦点
    /// </summary>
    /// <param name="moveType"></param>
    public void GetKeyDown(UIFocusPath.MoveType moveType)
    {
        Action<UIFocusPath.MoveType> MoveNextAction = (_moveType) => 
        {
            if (!nowUIFocus)
                nowUIFocus = uiFocusPath.GetFirstFocus();
            if (nowUIFocus)
            {
                UIFocus nextUIFocus = uiFocusPath.GetNewNextFocus(nowUIFocus, _moveType);
                if (nextUIFocus != null)
                {
                    nowUIFocus = nextUIFocus;
                }
                nowUIFocus.SetForcus();
            }
        };
        switch (moveType)
        {
            case UIFocusPath.MoveType.LEFT:
            case UIFocusPath.MoveType.RIGHT:
                MoveNextAction(moveType);
                break;
            case UIFocusPath.MoveType.OK:
                UIFocusButton uiFocusButton = nowUIFocus as UIFocusButton;
                if (uiFocusButton)
                {
                    uiFocusButton.ClickThisButton();//激活事件
                }
                break;
        }
    }

    /// <summary>
    /// 确认按钮被点击
    /// </summary>
    public void OKButton_Click()
    {
        Action<bool> ResultHandle = this.ResultHandle;
        gameObject.SetActive(false);
        if (ResultHandle != null)
        {
            ResultHandle(true);
        }
    }

    /// <summary>
    /// 取消按钮被点击
    /// </summary>
    public void CancelButton_Click()
    {
        Action<bool> ResultHandle = this.ResultHandle;
        gameObject.SetActive(false);
        if (ResultHandle != null)
        {
            ResultHandle(false);
        }
    }

}
