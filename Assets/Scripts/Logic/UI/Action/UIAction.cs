using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 功能界面
/// </summary>
public class UIAction : MonoBehaviour
{
    /// <summary>
    /// 标签路径
    /// </summary>
    UIFocusPath uiFocusPath;
    /// <summary>
    /// 当前的标签页焦点
    /// </summary>
    UIFocusTabPage nowTabPageFocus;

    void Awake()
    {
        uiFocusPath = GetComponent<UIFocusPath>();
    }

    private void OnEnable()
    {
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        if (uiFocusPath)
        {
            nowTabPageFocus = uiFocusPath.GetFirstFocus() as UIFocusTabPage;
            if (nowTabPageFocus)
            {
                nowTabPageFocus.SetForcus();
                if (nowTabPageFocus.panel)
                    nowTabPageFocus.panel.gameObject.SetActive(true);
            }
        }
        //重新载入数据
        //throw new NotImplementedException();
    }

    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
    }

    /// <summary>
    /// 按键检测
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        if (uiFocusPath)//切换标签页
        {
            UIFocus nextTabPageFocus = null;
            switch (keyType)
            {
                case UIManager.KeyType.R1:
                    nextTabPageFocus = uiFocusPath.GetNextFocus(nowTabPageFocus, UIFocusPath.MoveType.RIGHT, true);
                    break;
                case UIManager.KeyType.L1:
                    nextTabPageFocus = uiFocusPath.GetNextFocus(nowTabPageFocus, UIFocusPath.MoveType.LEFT, true);
                    break;
            }
            TabPageClick(nextTabPageFocus as UIFocusTabPage);
        }
        switch (keyType)
        {
            case UIManager.KeyType.B://返回
                CloseActionClick();
                break;
        }
    }

    /// <summary>
    /// 点击关闭功能面板
    /// </summary>
    public void CloseActionClick()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 点击标签页
    /// </summary>
    /// <param name="nextTabPageFocus"></param>
    public void TabPageClick(UIFocusTabPage nextTabPageFocus)
    {
        if (nextTabPageFocus != null)
        {
            nowTabPageFocus.LostForcus();
            nowTabPageFocus.panel.gameObject.SetActive(false);
            nowTabPageFocus = nextTabPageFocus as UIFocusTabPage;
            nowTabPageFocus.panel.gameObject.SetActive(true);
            nowTabPageFocus.SetForcus();
        }
    }
}
