using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 控制框的手柄
/// </summary>
public class UIFocusToggle : UIFocus
{
    /// <summary>
    /// 显示的文字
    /// </summary>
    [SerializeField]
    private Text label;

    /// <summary>
    /// 单选框的图标对象
    /// </summary>
    [SerializeField]
    private RectTransform toggleImageRect;

    /// <summary>
    /// 如果处于选中状态则显示的图片
    /// </summary>
    [SerializeField]
    private Image selectImage;

    /// <summary>
    /// 单选框
    /// </summary>
    private Toggle toggle;

    /// <summary>
    /// 附带的值
    /// </summary>
    public object value;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    public override void SetForcus()
    {
        selectImage.gameObject.SetActive(true);
    }

    public override void LostForcus()
    {
        selectImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 在输入OK键时更改选中状态
    /// </summary>
    /// <param name="moveType"></param>
    public override void MoveChild(UIFocusPath.MoveType moveType)
    {
        if (moveType == UIFocusPath.MoveType.OK)
        {
            if (toggle == null)
                toggle = GetComponent<Toggle>();
            toggle.isOn = !toggle.isOn;
        }
    }

    /// <summary>
    /// 设置显示的文字 
    /// </summary>
    /// <param name="label"></param>
    public void SetLable(string label)
    {
        this.label.text = label;
    }

    /// <summary>
    /// 是否选中了该项
    /// </summary>
    public bool Checked
    {
        get { return toggle.isOn; }
    }
}
