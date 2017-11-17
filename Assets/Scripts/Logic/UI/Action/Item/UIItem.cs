using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装备道具界面属性界面
/// </summary>
public class UIItem : MonoBehaviour
{
    /// <summary>
    /// 标签路径
    /// 道具栏（输入确定键替换装备，如果可以装备）
    /// 装备栏（输入确定键取下装备） 
    /// 属性栏(只可以用按键切换显示，不在焦点路径中)
    /// </summary>
    UIFocusPath uiFocusPath;
    /// <summary>
    /// 当前的焦点
    /// </summary>
    UIFocus nowUIFocus;

    private void Awake()
    {
        uiFocusPath = GetComponent<UIFocusPath>();
    }

    private void OnEnable()
    {
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        if (uiFocusPath)
        {
            nowUIFocus = uiFocusPath.GetFirstFocus();
            if (nowUIFocus)
                nowUIFocus.SetForcus();
        }
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
        //输入
        if (uiFocusPath)
        {
            Action<UIFocusPath.MoveType> MoveFocusAction = (moveType) =>
            {
                if (nowUIFocus)
                {
                    if (nowUIFocus.CanMoveNext(moveType))
                    {
                        UIFocus nextUIFocus = uiFocusPath.GetNextFocus(nowUIFocus, moveType);
                        if (nextUIFocus)
                        {
                            nowUIFocus = nextUIFocus;
                            nowUIFocus.SetForcus();
                        }
                    }
                    else//移动控件内的控件
                        nowUIFocus.MoveChild(moveType);
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
            }
        }
    }

}
