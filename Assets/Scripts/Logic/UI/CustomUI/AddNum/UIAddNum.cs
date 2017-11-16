using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// ADDNum控件（显示整型数字加减变化）
/// </summary>
public class UIAddNum : MonoBehaviour
{
    /// <summary>
    /// 最小值（闭区间）
    /// </summary>
    public int min;
    /// <summary>
    /// 最大值（闭区间）
    /// </summary>
    public int max;
    /// <summary>
    /// 当前值
    /// </summary>
    private int _value;
    /// <summary>
    /// 当前值
    /// </summary>
    public int Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            _value = Mathf.Clamp(_value, min, max);
            if (showText)
            {
                if (_value == min)
                    showText.text = minStr;
                else if (_value == max)
                    showText.text = maxStr;
                else showText.text = _value.ToString();
            }
        }
    }
    /// <summary>
    /// 当处于最小值时显示的文字
    /// </summary>
    [SerializeField]
    private string minStr = "-";
    /// <summary>
    /// 当处于最大值时显示的文字
    /// </summary>
    [SerializeField]
    private string maxStr = "Max";
    /// <summary>
    /// 当选中时显示的图片(边框)
    /// </summary>
    [SerializeField]
    private Image selectImage;
    /// <summary>
    /// 左操作手柄
    /// </summary>
    [SerializeField]
    private RectTransform leftRect;
    /// <summary>
    /// 右操作手柄
    /// </summary>
    [SerializeField]
    private RectTransform rightRect;
    /// <summary>
    /// 显示文字的对象
    /// </summary>
    [SerializeField]
    private Text showText;
    /// <summary>
    /// 点击左手柄
    /// </summary>
    public event Action ClickLeftHandle;
    /// <summary>
    /// 点击右手柄
    /// </summary>
    public event Action ClickRightHandle;

    void Awake()
    {
        Action<EventTrigger, UnityAction<BaseEventData>> RegistorClickEvent = (eTarget, action) =>
        {
            if (eTarget.triggers == null)
                eTarget.triggers = new List<EventTrigger.Entry>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback = new EventTrigger.TriggerEvent();
            entry.callback.AddListener(action);
            eTarget.triggers.Add(entry);
        };
        if (leftRect)
            RegistorClickEvent(leftRect.gameObject.AddComponent<EventTrigger>(), LeftHandleClick);
        if (rightRect)
            RegistorClickEvent(rightRect.gameObject.AddComponent<EventTrigger>(), RightHandleClick);
    }

    /// <summary>
    /// 点击左手柄
    /// </summary>
    /// <param name="e"></param>
    private void LeftHandleClick(BaseEventData e)
    {
        PointerEventData pe = e as PointerEventData;
        if (pe != null)
        {
            if (ClickLeftHandle != null)
                ClickLeftHandle();
        }
    }
    /// <summary>
    /// 点击右手柄
    /// </summary>
    /// <param name="e"></param>
    private void RightHandleClick(BaseEventData e)
    {
        PointerEventData pe = e as PointerEventData;
        if (pe != null)
        {
            if (ClickRightHandle != null)
                ClickRightHandle();
        }
    }

    /// <summary>
    /// 设置左手柄状态
    /// </summary>
    /// <param name="state"></param>
    public void SetLeftHandleState(bool state)
    {
        if (leftRect)
            leftRect.gameObject.SetActive(state);
    }

    /// <summary>
    /// 设置右手柄状态
    /// </summary>
    /// <param name="state"></param>
    public void SetRightHandleState(bool state)
    {
        if (rightRect)
            rightRect.gameObject.SetActive(state);
    }

    /// <summary>
    /// 边框图片的显示状态
    /// </summary>
    /// <param name="state"></param>
    public void SetImageState(bool state)
    {
        if (selectImage)
            selectImage.gameObject.SetActive(state);
    }
}
