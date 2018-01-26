using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI焦点
/// </summary>
[Serializable]
public class UIFocus : MonoBehaviour
{
    /// <summary>
    /// 标签
    /// </summary>
    public string Tag;

    /// <summary>
    /// 设置控件获得焦点
    /// </summary>
    public virtual void SetForcus() { }

    /// <summary>
    /// 设置控件失去焦点
    /// </summary>
    public virtual void LostForcus() { }

    /// <summary>
    /// 在失去焦点前使用该函数判断指定的操作是否会遭致失去焦点
    /// </summary>
    /// <param name="moveType">移动方式</param>
    /// <returns></returns>
    public virtual bool CanMoveNext(UIFocusPath.MoveType moveType)
    {
        return true;
    }
    /// <summary>
    /// 移动控件的内部
    /// </summary>
    /// <param name="moveType"></param>
    public virtual void MoveChild(UIFocusPath.MoveType moveType) { }
}

