using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大地图
/// </summary>
public class UIBigMap : MonoBehaviour
{
    /// <summary>
    /// 标签路径
    /// 显示哪些UI
    /// </summary>
    UIFocusPath uiFocusPath;
    /// <summary>
    /// 当前选中的对象
    /// </summary>
    UIFocusToggle nowUIFocusToggle;

    private void Awake()
    {
        uiFocusPath = GetComponent<UIFocusPath>();
    }
}
