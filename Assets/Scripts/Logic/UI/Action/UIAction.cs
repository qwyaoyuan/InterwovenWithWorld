using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// 游戏状态接口对象 
    /// </summary>
    IGameState iGameState;

    void Awake()
    {
        uiFocusPath = GetComponent<UIFocusPath>();
    }

    private void OnEnable()
    {
        iGameState = GameState.Instance.GetEntity<IGameState>();
        //压入状态 
        iGameState.PushEnumGameRunType(EnumGameRunType.Setting);

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
        ShowIndex(0);
    }

    private void OnDisable()
    {
        //弹出状态
        iGameState.PopEnumGameRunType();
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        ShowIndex(-1);
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
            case UIManager.KeyType.START://返回
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

    /// <summary>
    /// 显示指定的标签
    /// </summary>
    /// <param name="index"></param>
    public void ShowIndex(int index)
    {
        UIFocusTabPage[] tabPages = uiFocusPath.UIFocuesArray.OfType<UIFocusTabPage>().ToArray();
        if (tabPages.Length > 0)
        {
            if (index < 0)
                tabPages.ToList().ForEach(temp => 
                {
                    temp.panel.gameObject.SetActive(false);
                    temp.LostForcus();
                });
            else
            {
                if (index > tabPages.Length - 1)
                    index = 0;
                for (int i = 0; i < tabPages.Length; i++)
                {
                    tabPages[i].panel.gameObject.SetActive(i == index);
                    if (i == index)
                        tabPages[i].SetForcus();
                    else
                        tabPages[i].LostForcus();
                }
            }
        }
    }
}
