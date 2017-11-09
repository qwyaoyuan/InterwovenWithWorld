using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 标签页的焦点
/// </summary>
public class UIFocusTabPage : UIFocus
{
    /// <summary>
    /// 该标签的面板路径
    /// </summary>
    public UIFocusPath panelFocusPath;
    /// <summary>
    /// 指定的面板
    /// </summary>
    public RectTransform panel;
    /// <summary>
    /// 高亮图片
    /// </summary>
    public Sprite highLightSprite;
    /// <summary>
    /// 正常图片
    /// </summary>
    public Sprite nomalSprite;
    /// <summary>
    /// 标签页的图片
    /// </summary>
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// 失去焦点
    /// </summary>
    public override void LostForcus()
    {
        if (image)
            image.sprite = nomalSprite;
    }

    /// <summary>
    /// 设置焦点
    /// </summary>
    public override void SetForcus()
    {
        if (image)
            image.sprite = highLightSprite;
    }

}
