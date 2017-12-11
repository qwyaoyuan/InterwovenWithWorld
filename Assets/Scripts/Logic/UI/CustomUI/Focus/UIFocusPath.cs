using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// 焦点路径
/// </summary>
public class UIFocusPath : MonoBehaviour
{
    /// <summary>
    /// 行
    /// </summary>
    public int row;
    /// <summary>
    /// 列
    /// </summary>
    public int column;

    /// <summary>
    /// UI路径数组
    /// </summary>
    public UIFocus[] UIFocuesArray;

    /// <summary>
    /// 获取第一个焦点
    /// </summary>
    /// <returns></returns>
    public UIFocus GetFirstFocus()
    {
        if (UIFocuesArray != null)
            return UIFocuesArray.FirstOrDefault(temp => temp != null && temp.gameObject.activeSelf != false);
        return null;
    }

    /// <summary>
    /// 获取第一个指定类型的焦点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetFirstFocus<T>() where T : UIFocus
    {
        if (UIFocuesArray != null)
            return UIFocuesArray.First(temp => temp != null && (temp as T) != null && temp.gameObject.activeSelf != false) as T;
        return null;
    }

    /// <summary>
    /// 获取下一个焦点
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="moveType">移动方式</param>
    /// <param name="cycle">是否周期检索</param>
    /// <returns></returns>
    public UIFocus GetNextFocus(UIFocus target, MoveType moveType, bool cycle = false)
    {
        if (target == null)
            return null;
        int index = UIFocuesArray.ToList().FindIndex(temp => target.Equals(temp));
        if (index < 0)
            return null;
        int _row = (index) / column;
        int _column = index % column;
        switch (moveType)
        {
            case MoveType.LEFT:
                _column -= 1;
                break;
            case MoveType.RIGHT:
                _column += 1;
                break;
            case MoveType.UP:
                _row -= 1;
                break;
            case MoveType.DOWN:
                _row += 1;
                break;
        }
        if (_row >= row && cycle)
            _row = 0;
        if (_row < 0 && cycle)
            _row = row - 1;
        if (_column >= column && cycle)
            _column = 0;
        if (_column < 0 && cycle)
            _column = column - 1;
        if (_row >= 0 && _row < row && _column >= 0 && _column < column)
        {
            Func<int, int, int, int, UIFocus> CheckUIFocus = null;
            CheckUIFocus = (__row, __column, _add_row, _add_column) =>
            {
                if (__row >= 0 && __column >= 0 && __row < row && __column < column)
                {
                    __row += _add_row;
                    __column += _add_column;
                    UIFocus _tempUIFocus = GetFocus(__row, __column);
                    if (!_tempUIFocus)
                    {
                        return CheckUIFocus(__row, __column, _add_row, _add_column);
                    }
                }
                return null;
            };
            UIFocus nextUIFocus = GetFocus(_row, _column);//  UIFocuesArray[_row * column + _column];
            if (!nextUIFocus)//如果为空，则根据方向确定
            {
                UIFocus tempUIFocus = null;
                switch (moveType)
                {
                    case MoveType.RIGHT:
                    case MoveType.LEFT:
                        tempUIFocus = CheckUIFocus(_row, _column, -1, 0);
                        if (!tempUIFocus)
                            tempUIFocus = CheckUIFocus(_row, _column, 1, 0);
                        break;
                    case MoveType.UP:
                    case MoveType.DOWN:
                        tempUIFocus = CheckUIFocus(_row, _column, 0, -1);
                        if (!tempUIFocus)
                            tempUIFocus = CheckUIFocus(_row, _column, 0, 1);
                        break;
                }
                if (tempUIFocus)
                    nextUIFocus = tempUIFocus;
            }
            if (nextUIFocus==null || nextUIFocus.gameObject.activeSelf)//如果没有隐藏则直接返回
                return nextUIFocus;
            return GetNextFocus(nextUIFocus, moveType, cycle);//如果隐藏了则以当前点为坐标继续向下查找
        }
        else
            return null;
    }

    /// <summary>
    /// 根据下标获取对象
    /// </summary>
    /// <param name="x">第一维度</param>
    /// <param name="y">第二维度</param>
    /// <returns></returns>
    private UIFocus GetFocus(int x, int y)
    {
        int index = x * column + y;
        if (index >= UIFocuesArray.Length || index < 0)
            return null;
        else return UIFocuesArray[index];
    }

    /// <summary>
    /// 移动方式
    /// </summary>
    public enum MoveType
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        OK
    }
}


