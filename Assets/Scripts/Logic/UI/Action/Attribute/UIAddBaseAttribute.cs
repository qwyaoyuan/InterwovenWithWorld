using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// 基础属性加点
/// </summary>
public class UIAddBaseAttribute : UIFocus
{

    /// <summary>
    /// 基础属性的类型
    /// </summary>
    [SerializeField]
    EnumBaseAttributeType BaseAttributeType;

    /// <summary>
    /// 显示
    /// </summary>
    [SerializeField]
    Text showText;

    /// <summary>
    /// 加点控件
    /// </summary>
    [SerializeField]
    UIAddNum uiAddNum;

    /// <summary>
    /// 确认键
    /// </summary>
    [SerializeField]
    RectTransform okRectTrans;

    /// <summary>
    /// 玩家属性
    /// </summary>
    IPlayerAttributeState iPlayerAttributeState;

    /// <summary>
    /// 玩家存档
    /// </summary>
    PlayerState playerState;

    /// <summary>
    /// 该属性当前加了多少点
    /// </summary>
    private int addPoint;

    /// <summary>
    /// 属性对应显示的集合
    /// </summary>
    List<KeyValuePair<EnumBaseAttributeType, string>> keyValueList;

    private void Awake()
    {
        uiAddNum.ClickLeftHandle += ClickLeft;
        uiAddNum.ClickRightHandle += ClickRight;
        RegistorEventTrigger(okRectTrans, EventTriggerType.PointerClick, ClickOK);
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="rectTrans"></param>
    /// <param name="callback"></param>
    private void RegistorEventTrigger(RectTransform rectTrans, EventTriggerType type, UnityAction<BaseEventData> callback)
    {
        EventTrigger eventTrigger = rectTrans.GetComponent<EventTrigger>();
        if (!eventTrigger)
            eventTrigger = rectTrans.gameObject.AddComponent<EventTrigger>();
        if (eventTrigger.triggers == null)
            eventTrigger.triggers = new List<EventTrigger.Entry>();
        EventTrigger.Entry triggerEvent = eventTrigger.triggers.Find(temp => temp.eventID == type);
        if (triggerEvent == null)
        {
            triggerEvent = new EventTrigger.Entry();
            triggerEvent.eventID = type;
            triggerEvent.callback = new EventTrigger.TriggerEvent();
            eventTrigger.triggers.Add(triggerEvent);
        }
        triggerEvent.callback.AddListener(callback);

    }

    public override void SetForcus()
    {
        uiAddNum.gameObject.SetActive(true);
        okRectTrans.gameObject.SetActive(false);
        iPlayerAttributeState = GameState.Instance.GetEntity<IPlayerAttributeState>();
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        keyValueList = new List<KeyValuePair<EnumBaseAttributeType, string>>();
        FieldExplanAttribute.SetEnumExplanDic(keyValueList);
        addPoint = 0;
        UpdateShow();
    }

    private void UpdateShow()
    {
        KeyValuePair<EnumBaseAttributeType, string> thisKVP = keyValueList.FirstOrDefault(temp => temp.Key == BaseAttributeType);
        string tempStr = thisKVP.Value + ":";
        switch (BaseAttributeType)
        {
            case EnumBaseAttributeType.Power:
                tempStr += (int)iPlayerAttributeState.Power;
                uiAddNum.Value = (int)iPlayerAttributeState.Power + addPoint;
                break;
            case EnumBaseAttributeType.Mental:
                tempStr += (int)iPlayerAttributeState.Mental;
                uiAddNum.Value = (int)iPlayerAttributeState.Mental + addPoint;
                break;
            case EnumBaseAttributeType.Quick:
                tempStr += (int)iPlayerAttributeState.Quick;
                uiAddNum.Value = (int)iPlayerAttributeState.Quick + addPoint;
                break;
        }
        showText.text = tempStr;
        uiAddNum.SetLeftHandleState(addPoint > 0);
        uiAddNum.SetRightHandleState(playerState.PropertyPoint > addPoint);
        okRectTrans.gameObject.SetActive(addPoint > 0);
    }

    public override void LostForcus()
    {
        uiAddNum.gameObject.SetActive(false);
        okRectTrans.gameObject.SetActive(false);
        addPoint = 0;
    }


    public override void MoveChild(UIFocusPath.MoveType moveType)
    {
        switch (moveType)
        {
            case UIFocusPath.MoveType.LEFT:
                ClickLeft();
                break;
            case UIFocusPath.MoveType.RIGHT:
                ClickRight();
                break;
            case UIFocusPath.MoveType.OK:
                ClickOK(null);
                break;
        }
    }

    private void ClickLeft()
    {
        if (addPoint > 0)
        {
            addPoint--;
            UpdateShow();
        }
    }

    private void ClickRight()
    {
        if (playerState.PropertyPoint > addPoint)
        {
            addPoint++;
            UpdateShow();
        }
    }

    private void ClickOK(BaseEventData data)
    {
        if (addPoint > 0)
        {
            playerState.PropertyPoint -= addPoint;
            IAttributeState baseAttribute = iPlayerAttributeState.GetAttribute(0);//基础属性
            switch (BaseAttributeType)
            {
                case EnumBaseAttributeType.Power:
                    playerState.Strength += addPoint;
                    baseAttribute.Power += addPoint;
                    break;
                case EnumBaseAttributeType.Mental:
                    playerState.Spirit += addPoint;
                    baseAttribute.Mental += addPoint;
                    break;
                case EnumBaseAttributeType.Quick:
                    playerState.Agility += addPoint;
                    baseAttribute.Quick += addPoint;
                    break;
            }
            addPoint = 0;
            UpdateShow();
        }
    }
}


/// <summary>
/// 基础属性枚举
/// </summary>
public enum EnumBaseAttributeType
{
    /// <summary>
    /// 力量
    /// </summary>
    [FieldExplan("力量")]
    Power,
    /// <summary>
    /// 精神
    /// </summary>
    [FieldExplan("精神")]
    Mental,
    /// <summary>
    /// 敏捷
    /// </summary>
    [FieldExplan("敏捷")]
    Quick,
}