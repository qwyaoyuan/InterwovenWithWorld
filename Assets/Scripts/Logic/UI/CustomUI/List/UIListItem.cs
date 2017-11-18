using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// List条目控件 
/// </summary>
public class UIListItem : MonoBehaviour {

    /// <summary>
    /// 子节点图片(主要用于是否选中)
    /// </summary>
    public Image childImage;

    /// <summary>
    /// 存储的数据
    /// </summary>
    public object value;

    /// <summary>
    /// 子节点文字
    /// </summary>
    public Text childText;
}
