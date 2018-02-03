using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// 焦点路径
/// </summary>
[Serializable]
public class UIFocusPath : MonoBehaviour
{
    #region 旧版的焦点路径
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
    /// 检索焦点的结构数组
    /// </summary>
    CheckFocusStruct[,] checkFocusStructArray;
    /// <summary>
    /// 检索焦点的结构数组
    /// </summary>
    CheckFocusStruct[,] CheckFocusStructArray
    {
        get
        {
            if (checkFocusStructArray == null)
                checkFocusStructArray = new CheckFocusStruct[row, column];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (checkFocusStructArray[i, j] == null)
                    {
                        checkFocusStructArray[i, j] = new CheckFocusStruct();
                    }
                    checkFocusStructArray[i, j].Target = UIFocuesArray[i * column + j] ? UIFocuesArray[i * column + j] : null;
                    checkFocusStructArray[i, j].BaseX = j;
                    checkFocusStructArray[i, j].BaseY = i;
                }
            }
            return checkFocusStructArray;
        }
    }
    #endregion

    #region 新版的焦点路径
    /// <summary>
    /// ui焦点路径
    /// </summary>
    public FocusRelaship[] UIFocusArrayRelaships;
    /// <summary>
    /// 所有焦点对象
    /// </summary>
    public UIFocus[] NewUIFocusArray
    {
        get
        {
            if (UIFocusArrayRelaships != null)
            {
                return UIFocusArrayRelaships.Select(temp => temp.This).ToArray();
            }
            return new UIFocus[0];
        }
    }
    #endregion

    /// <summary>
    /// 获取第一个焦点
    /// </summary>
    /// <returns></returns>
    public UIFocus GetFirstFocus()
    {
        if (NewUIFocusArray != null && NewUIFocusArray.Length > 0)
            return NewUIFocusArray.FirstOrDefault(temp => temp != null && temp.gameObject.activeSelf != false);
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
        if (NewUIFocusArray != null && NewUIFocusArray.Length > 0)
            return NewUIFocusArray.First(temp => temp != null && (temp as T) != null && temp.gameObject.activeSelf != false) as T;
        if (UIFocuesArray != null)
            return UIFocuesArray.First(temp => temp != null && (temp as T) != null && temp.gameObject.activeSelf != false) as T;
        return null;
    }

    /// <summary>
    /// 使用检索模式获得下一个焦点
    /// </summary>
    /// <param name="target"></param>
    /// <param name="moveType"></param>
    /// <param name="cycle"></param>
    /// <returns></returns>
    [Obsolete("已过时,请使用新版的获取焦点")]
    public UIFocus GetNextFocus(UIFocus target, MoveType moveType, EnumFocusCheckModel focusCheckModel, bool cycle = false)
    {
        int index = UIFocuesArray.ToList().FindIndex(temp => target.Equals(temp));
        if (index < 0)
            return null;
        int this_BaseRowIndex = (index) / column;
        int this_BaseColumnIndex = index % column;
        //构建检索用的数组
        CheckFocusStruct[,] _CheckFocusStructArray = CheckFocusStructArray;
        CheckFocusStruct[,] useCheckFocusStructArray = null;//本次使用的数组对象
        if (cycle)//如果循环(默认方向向右向下)
        {
            useCheckFocusStructArray = new CheckFocusStruct[row, column];
            foreach (CheckFocusStruct checkFocusStruct in _CheckFocusStructArray)
            {
                int baseX = checkFocusStruct.BaseX;
                int baseY = checkFocusStruct.BaseY;
                switch (moveType)
                {
                    case MoveType.LEFT://左下方向延伸 x--,y++
                        //列
                        baseX = this_BaseColumnIndex >= baseX ?
                            (column - (this_BaseColumnIndex - baseX) - 1)//如果当前下标大于等于遍历对象的下标
                            : (baseX - this_BaseColumnIndex - 1);//如果当前下标小于遍历对象的下标
                        //行
                        baseY = this_BaseRowIndex > baseY ?
                            (row - this_BaseRowIndex + baseY)//如果当前下标大于遍历对象的下标
                            : (baseY - this_BaseRowIndex);//如果当前下标小于等于遍历对象的下标
                        break;
                    case MoveType.UP://右上方向延伸 x++,y--
                        //列
                        baseX = this_BaseColumnIndex > baseX ?
                            (column - this_BaseColumnIndex + baseX)//如果当前下标大于便利对象的下标
                            : (baseX - this_BaseColumnIndex);//如果当前下标小于等于遍历对象的下标
                        //行
                        baseY = this_BaseRowIndex >= baseY ?
                            (row - (this_BaseRowIndex - baseY) - 1)//如果当前下标大于等于遍历对象的下标
                            : (baseY - this_BaseRowIndex - 1);//如果当前下标小于遍历对象的下标
                        break;
                    case MoveType.DOWN://右下方向延伸 x++,y++
                    case MoveType.RIGHT:
                        //列
                        baseX = this_BaseColumnIndex > baseX ?
                            (column - this_BaseColumnIndex + baseX)//如果当前下标大于便利对象的下标
                            : (baseX - this_BaseColumnIndex);//如果当前下标小于等于遍历对象的下标
                        //行
                        baseY = this_BaseRowIndex > baseY ?
                            (row - this_BaseRowIndex + baseY)//如果当前下标大于遍历对象的下标
                            : (baseY - this_BaseRowIndex);//如果当前下标小于等于遍历对象的下标
                        break;
                }
                useCheckFocusStructArray[baseY, baseX] = checkFocusStruct;
            }
        }
        else
        {
            int thisRowCount = 0;
            int thisColumnCount = 0;
            //建立数组
            switch (moveType)
            {
                case MoveType.LEFT://左下方向延伸 x--,y++
                    thisColumnCount = this_BaseColumnIndex + 1;
                    thisRowCount = row - this_BaseRowIndex;
                    break;
                case MoveType.UP://右上方向延伸 x++,y--
                    thisColumnCount = column - this_BaseColumnIndex;
                    thisRowCount = this_BaseRowIndex + 1;
                    break;
                case MoveType.RIGHT://右下方向延伸 x++,y++
                case MoveType.DOWN:
                    thisColumnCount = column - this_BaseColumnIndex;
                    thisRowCount = row - this_BaseRowIndex;
                    break;
            }
            useCheckFocusStructArray = new CheckFocusStruct[thisRowCount, thisColumnCount];
            //存放数据
            for (int i = 0; i < useCheckFocusStructArray.GetLength(0); i++)//行
            {
                for (int j = 0; j < useCheckFocusStructArray.GetLength(1); j++)//列
                {
                    int thisX = -1;
                    int thisY = -1;
                    switch (moveType)
                    {
                        case MoveType.LEFT://左下方向延伸 x--,y++
                            thisX = j;//列
                            thisY = i + this_BaseRowIndex;//行
                            break;
                        case MoveType.UP://右上方向延伸 x++,y--
                            thisX = j + this_BaseColumnIndex;//列
                            thisY = i;//行
                            break;
                        case MoveType.RIGHT://右下方向延伸 x++,y++
                        case MoveType.DOWN:
                            thisX = j + this_BaseColumnIndex;//列
                            thisY = i + this_BaseRowIndex;//行
                            break;
                    }
                    if (thisX > -1 && thisX < _CheckFocusStructArray.GetLength(1) && thisY > -1 && thisY < _CheckFocusStructArray.GetLength(0))
                        useCheckFocusStructArray[i, j] = _CheckFocusStructArray[thisY, thisX];
                }
            }
        }
        //设置起始坐标与增加量
        int startX = -1;
        int startY = -1;
        int addX = 0;
        int addY = 0;
        switch (moveType)
        {
            case MoveType.LEFT:
                startX = useCheckFocusStructArray.GetLength(1) - 1;
                startY = 0;
                addX = -1;
                break;
            case MoveType.UP:
                startX = 0;
                startY = useCheckFocusStructArray.GetLength(0) - 1;
                addY = -1;
                break;
            case MoveType.RIGHT:
                startX = 0;
                startY = 0;
                addX = 1;
                break;
            case MoveType.DOWN:
                startX = 0;
                startY = 0;
                addY = 1;
                break;

        }
        if (startX < 0 || startY < 0)
            return null;
        if (addX == 0 && addY == 0)
            return null;
        //开始检索
        UIFocus checkedFocus = null;
        //寻找指定坐标开始的最近位置
        Action BothCheckFunc = () =>
        {
            int dis = int.MaxValue;
            CheckFocusStruct checkFocusStruct = null;
            for (int i = 0; i < useCheckFocusStructArray.GetLength(0); i++)//行y
            {
                for (int j = 0; j < useCheckFocusStructArray.GetLength(1); j++)//列x
                {
                    if (i == startY && j == startX)
                        continue;
                    CheckFocusStruct tempObj = useCheckFocusStructArray[i, j];
                    if (tempObj.Target != null)
                    {
                        //计算距离
                        int x = startX - j;
                        int y = startY - i;
                        int tempDis = (int)Mathf.Pow(x, 2) + (int)Mathf.Pow(y, 2);
                        if (tempDis < dis)
                        {
                            dis = tempDis;
                            checkFocusStruct = tempObj;
                        }
                        else if (tempDis == dis)//如果相等,则根据移动方向判断,在移动方向上的差距越小约好
                        {
                            int oldInterval = 0, nowInterval = 0;
                            if (addX != 0)//判断列的差距
                            {
                                oldInterval = Mathf.Abs(checkFocusStruct.BaseX - this_BaseColumnIndex);
                                nowInterval = Mathf.Abs(tempObj.BaseX - this_BaseColumnIndex);
                            }
                            else if (addY != 0)//判断行的差距
                            {
                                oldInterval = Mathf.Abs(checkFocusStruct.BaseY - this_BaseRowIndex);
                                nowInterval = Mathf.Abs(tempObj.BaseY - this_BaseRowIndex);
                            }
                            if (nowInterval < oldInterval)
                                checkFocusStruct = tempObj;
                        }
                    }
                }
            }
            if (checkFocusStruct != null)
                checkedFocus = checkFocusStruct.Target;
        };
        //垂直方向检测(检测的是行i(y))
        Func<bool> VertiacalCheckFunc = () =>
        {
            if (addY == 0)
                return false;
            int rowCount = useCheckFocusStructArray.GetLength(0);//总行数
            for (int i = 1; i < rowCount; i++)
            {
                CheckFocusStruct checkFocusStruct = useCheckFocusStructArray[startY + i * addY, startX];
                if (checkFocusStruct.Target != null)
                {
                    checkedFocus = checkFocusStruct.Target;
                    return true;
                }
            }
            return false;
        };
        //水平方向检测(检测的是列j(x))
        Func<bool> HorizontalCheckFunc = () =>
        {
            if (addX == 0)
                return false;
            int columnCount = useCheckFocusStructArray.GetLength(1);//纵列数
            for (int i = 1; i < columnCount; i++)
            {
                CheckFocusStruct checkFocusStruct = useCheckFocusStructArray[startY, startX + i * addX];
                if (checkFocusStruct.Target != null)
                {
                    checkedFocus = checkFocusStruct.Target;
                    return true;
                }
            }
            return false;
        };
        switch (focusCheckModel)
        {
            case EnumFocusCheckModel.Vertical:
                if (!VertiacalCheckFunc())
                    BothCheckFunc();
                break;
            case EnumFocusCheckModel.Horizontal:
                if (!HorizontalCheckFunc())
                    BothCheckFunc();
                break;
            case EnumFocusCheckModel.Both:
                BothCheckFunc();
                break;
        }
        return checkedFocus;
    }

    /// <summary>
    /// 获取下一个焦点
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="moveType">移动方式</param>
    /// <param name="cycle">是否周期检索</param>
    /// <returns></returns>
    [Obsolete("已过时,请使用GetNextFocus([UIFocus],[MoveType],[EnumFocusCheckModel],[Bool])函数")]
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
                    else
                    {
                        return _tempUIFocus;
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
            if (nextUIFocus == null || nextUIFocus.gameObject.activeSelf)//如果没有隐藏则直接返回
                return nextUIFocus;
            return GetNextFocus(nextUIFocus, moveType, cycle);//如果隐藏了则以当前点为坐标继续向下查找
        }
        else
            return null;
    }

    /// <summary>
    /// 检索
    /// </summary>
    /// <param name="target"></param>
    /// <param name="moveType"></param>
    /// <returns></returns>
    public UIFocus GetNewNextFocus(UIFocus target, MoveType moveType)
    {
        FocusRelaship focusRelaship = UIFocusArrayRelaships.FirstOrDefault(temp => temp.This == target);
        if (focusRelaship != null)
        {
            Func<UIFocus, MoveType, UIFocus> CheckThisFocus = (innerTarget, innerMoveType) =>
             {
                 if (innerTarget == null)
                     return null;
                 if (innerTarget.gameObject.activeSelf)
                     return innerTarget;
                 return GetNewNextFocus(innerTarget, innerMoveType);
             };
            switch (moveType)
            {
                case MoveType.LEFT:
                    return CheckThisFocus(focusRelaship.Left, moveType);
                case MoveType.RIGHT:
                    return CheckThisFocus(focusRelaship.Right, moveType);
                case MoveType.UP:
                    return CheckThisFocus(focusRelaship.Up, moveType);
                case MoveType.DOWN:
                    return CheckThisFocus(focusRelaship.Down, moveType);
            }
        }
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
    /// 检索焦点的结构
    /// </summary>
    private class CheckFocusStruct
    {
        /// <summary>
        /// 该对象在原始数组中的下标
        /// </summary>
        public int BaseX;
        /// <summary>
        /// 该对象在原始数组中的下标
        /// </summary>
        public int BaseY;
        /// <summary>
        /// 目标
        /// </summary>
        public UIFocus Target;
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

    /// <summary>
    /// 焦点的检索模式
    /// </summary>
    public enum EnumFocusCheckModel
    {
        /// <summary>
        /// 垂直跳跃,如果在该列中上下检索的时候没有发现任何目标,则移动到相邻的列中距离自己最近的格子
        /// </summary>
        Vertical,
        /// <summary>
        /// 水平跳跃,如果在该行中左右检索的时候没有发现任何目标,则移动到相邻的行中距离自己最近的格子 
        /// </summary>
        Horizontal,
        /// <summary>
        /// 直接移动到指定方向最近的格子
        /// </summary>
        Both,
    }

    /// <summary>
    /// 焦点关系
    /// </summary>
    [Serializable]
    public class FocusRelaship
    {
        public UIFocus This;
        public UIFocus Up;
        public UIFocus Down;
        public UIFocus Left;
        public UIFocus Right;
    }
}




