using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置面板UI
/// </summary>
public class UISetting : MonoBehaviour
{
    /// <summary>
    /// ui路径
    /// </summary>
    UIFocusPath uiFocusPath;
    /// <summary>
    /// 当前的标签焦点
    /// </summary>
    UIFocusTabPage nowTabPageFocus;
    /// <summary>
    /// 面板的焦点对象
    /// </summary>
    UIFocus tabPanelFocus;

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
                nowTabPageFocus.SetForcus();
            if (nowTabPageFocus != null && nowTabPageFocus.panel)
                nowTabPageFocus.panel.gameObject.SetActive(true);
            if (nowTabPageFocus != null && nowTabPageFocus.panelFocusPath)
            {
                tabPanelFocus = nowTabPageFocus.panelFocusPath.GetFirstFocus();
                if (tabPanelFocus != null)
                    tabPanelFocus.SetForcus();
            }
        }
        //重新载入数据
        //throw new NotImplementedException();
    }

    private void OnDisable()
    {
        if (nowTabPageFocus != null && nowTabPageFocus.panel)
            nowTabPageFocus.panel.gameObject.SetActive(false);
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
    }

    /// <summary>
    /// 按键检测
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        if (uiFocusPath)//标签页左右切换
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
            /*
            if (nextTabPageFocus != null)
            {
                nowTabPageFocus.panel.gameObject.SetActive(false);
                nowTabPageFocus = nextTabPageFocus as UIFocusTabPage;
                nowTabPageFocus.gameObject.SetActive(true);
                if (nowTabPageFocus.panelFocusPath)
                {
                    if (tabPanelFocus)
                        tabPanelFocus.LostForcus();
                    tabPanelFocus = nowTabPageFocus.panelFocusPath.GetFirstFocus();
                    if (tabPanelFocus)
                        tabPanelFocus.SetForcus();
                }
            }
            */
        }

        if (nowTabPageFocus)//标签页内部切换
        {
            if (nowTabPageFocus.panelFocusPath)
            {
                if (!tabPanelFocus)
                {
                    tabPanelFocus = nowTabPageFocus.panelFocusPath.GetFirstFocus();
                }
                //判断键位
                Action<UIFocusPath.MoveType> MoveFocusAction = (moveType) =>
                {
                    if (tabPanelFocus)
                    {
                        if (tabPanelFocus.CanMoveNext(moveType))
                        {
                            UIFocus nextTabPanelFocus = nowTabPageFocus.panelFocusPath.GetNewNextFocus(tabPanelFocus, moveType);// nowTabPageFocus.panelFocusPath.GetNextFocus(tabPanelFocus, moveType);
                            if (nextTabPanelFocus)
                            {
                                tabPanelFocus = nextTabPanelFocus;
                                tabPanelFocus.SetForcus();
                            }
                        }
                        else
                            tabPanelFocus.MoveChild(moveType);
                    }
                };
                switch (keyType)
                {
                    case UIManager.KeyType.LEFT:
                        MoveFocusAction(UIFocusPath.MoveType.LEFT);
                        break;
                    case UIManager.KeyType.RIGHT:
                        MoveFocusAction(UIFocusPath.MoveType.RIGHT);
                        break;
                    case UIManager.KeyType.UP:
                        MoveFocusAction(UIFocusPath.MoveType.UP);
                        break;
                    case UIManager.KeyType.DOWN:
                        MoveFocusAction(UIFocusPath.MoveType.DOWN);
                        break;
                    case UIManager.KeyType.A:
                        MoveFocusAction(UIFocusPath.MoveType.OK);
                        break;
                }
            }
        }

        switch (keyType)
        {
            case UIManager.KeyType.B://返回
                CloseSettingClick();
                break;
        }
    }

    /// <summary>
    /// 点击关闭设置
    /// </summary>
    public void CloseSettingClick()
    {
        //提示是否保存 更改
        //throw new NotImplementedException();
        //隐藏该面板
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
            nowTabPageFocus = nextTabPageFocus;
            nowTabPageFocus.panel.gameObject.SetActive(true);
            nowTabPageFocus.SetForcus();
            if (nowTabPageFocus.panelFocusPath)
            {
                if (tabPanelFocus)
                    tabPanelFocus.LostForcus();
                tabPanelFocus = nowTabPageFocus.panelFocusPath.GetFirstFocus();
                if (tabPanelFocus)
                    tabPanelFocus.SetForcus();
            }
        }
    }
}
