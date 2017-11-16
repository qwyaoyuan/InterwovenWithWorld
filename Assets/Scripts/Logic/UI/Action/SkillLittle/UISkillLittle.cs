using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 技能加点UI
/// </summary>
public class UISkillLittle : MonoBehaviour
{
    /// <summary>
    /// 技能加点面板的路径
    /// </summary>
    UIFocusPath skillFocusPath;

    /// <summary>
    /// 当前的焦点技能
    /// </summary>
    UIFocusSkillLittleLattice nowSkillLattice;

    void Awake()
    {
        skillFocusPath = GetComponent<UIFocusPath>();
        if (skillFocusPath)
        {
            //给每个技能加一个点击事件，处理鼠标点击获取焦点
            UIFocus[] uiSkillLatticeArray = skillFocusPath.UIFocuesArray;
            foreach (UIFocus uiSkillLattice in uiSkillLatticeArray)
            {
                if (!uiSkillLattice)
                    continue;
                EventTrigger eventTrigger = uiSkillLattice.gameObject.AddComponent<EventTrigger>();
                eventTrigger.triggers = new List<EventTrigger.Entry>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback = new EventTrigger.TriggerEvent();
                entry.callback.AddListener(ClickSkillLattice);
                eventTrigger.triggers.Add(entry);
            }
        }
    }

    private void Start()
    {
        //初始化技能控件状态
        UIFocus[] uiSkillLatticeArray = skillFocusPath.UIFocuesArray;
        foreach (UIFocus uiSkillLattice in uiSkillLatticeArray)
        {
            if (!uiSkillLattice)
                continue;
            UIFocusSkillLittleLattice currentSkillLattice = uiSkillLattice as UIFocusSkillLittleLattice;
            if (currentSkillLattice)
                currentSkillLattice.SkillLittleSettingState = false;
        }
    }

    /// <summary>
    /// 点击技能图标
    /// </summary>
    /// <param name="e"></param>
    private void ClickSkillLattice(BaseEventData e)
    {
        PointerEventData pe = e as PointerEventData;
        if (pe.pointerCurrentRaycast.gameObject != null)
        {
            UIFocusSkillLittleLattice currentSkillLattice = UITools.FindTargetPopup<UIFocusSkillLittleLattice>(pe.pointerCurrentRaycast.gameObject.transform);
            if (currentSkillLattice)
            {
                nowSkillLattice.LostForcus();
                nowSkillLattice = currentSkillLattice;
                nowSkillLattice.SetForcus();
                nowSkillLattice.SkillLittleSettingState = true;
            }
        }
    }

    /// <summary>
    /// 当显示时初始化技能数据
    /// </summary>
    private void OnEnable()
    {
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        if (skillFocusPath)
        {
            nowSkillLattice = skillFocusPath.GetFirstFocus() as UIFocusSkillLittleLattice;
            if (nowSkillLattice)
                nowSkillLattice.SetForcus();
            UIFocus[] uiSkillLatticeArray = skillFocusPath.UIFocuesArray;
            foreach (UIFocus uiSkillLattice in uiSkillLatticeArray)
            {
                if (!uiSkillLattice)
                    continue;
                UIFocusSkillLittleLattice uiFocusSkillLittleLattice = uiSkillLattice as UIFocusSkillLittleLattice;
                if (uiFocusSkillLittleLattice)
                    uiFocusSkillLittleLattice.InitSkillShow();
            }
        }
    }

    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        if (nowSkillLattice)
        {
            nowSkillLattice.LostForcus();
        }
    }

    /// <summary>
    /// 接收输入
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        if (!nowSkillLattice && skillFocusPath)
            nowSkillLattice = skillFocusPath.GetFirstFocus() as UIFocusSkillLittleLattice;
        Action<UIFocusPath.MoveType> CanMoveNextEndAction = (moveType) =>
        {
            if (nowSkillLattice.CanMoveNext(moveType))//可以移动则移动
            {
                if (skillFocusPath)
                {
                    UIFocusSkillLittleLattice currentSkillLattice = skillFocusPath.GetNextFocus(nowSkillLattice, moveType) as UIFocusSkillLittleLattice;
                    if (currentSkillLattice != null && !object.Equals(currentSkillLattice, nowSkillLattice))
                    {
                        nowSkillLattice.LostForcus();
                        nowSkillLattice = currentSkillLattice;
                        nowSkillLattice.SetForcus();
                    }
                }
            }
            else //不可以移动则内部处理
            {
                nowSkillLattice.MoveChild(moveType);
            }
        };
        if (nowSkillLattice)
            switch (keyType)
            {
                case UIManager.KeyType.A:
                    CanMoveNextEndAction(UIFocusPath.MoveType.OK);
                    break;
                case UIManager.KeyType.LEFT:
                    CanMoveNextEndAction(UIFocusPath.MoveType.LEFT);
                    break;
                case UIManager.KeyType.RIGHT:
                    CanMoveNextEndAction(UIFocusPath.MoveType.RIGHT);
                    break;
                case UIManager.KeyType.UP:
                    CanMoveNextEndAction(UIFocusPath.MoveType.UP);
                    break;
                case UIManager.KeyType.DOWN:
                    CanMoveNextEndAction(UIFocusPath.MoveType.DOWN);
                    break;

            }
    }
}
