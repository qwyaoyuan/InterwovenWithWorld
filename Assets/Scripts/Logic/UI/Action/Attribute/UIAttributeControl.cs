using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 属性展示界面
/// </summary>
public class UIAttributeControl : MonoBehaviour
{
    /// <summary>
    /// 滑动条
    /// </summary>
    public Scrollbar scrollbar;

    /// <summary>
    /// 属性点加点的路径对象
    /// </summary>
    UIFocusPath uiFocusPath;

    /// <summary>
    /// 当前选中的节点
    /// </summary>
    UIFocus nowUIFocus;

    /// <summary>
    /// 玩家名字相关说明
    /// </summary>
    [SerializeField]
    Text titleText;
    /// <summary>
    /// 等级相关说明
    /// </summary>
    [SerializeField]
    Text levelText;
    /// <summary>
    /// 加点相关说明
    /// </summary>
    [SerializeField]
    Text pointText;

    /// <summary>
    /// 玩家存档
    /// </summary>
    PlayerState playerState;

    /// <summary>
    /// 玩家状态
    /// </summary>
    IPlayerState iPlayerState;

    /// <summary>
    /// 种族与种族名集合
    /// </summary>
    List<KeyValuePair<RoleOfRace, string>> roleOfRaceExplanList;

    private void Awake()
    {
        uiFocusPath = GetComponent<UIFocusPath>();
        UIFocus[] uiFocusArray = uiFocusPath.NewUIFocusArray;
        foreach (var item in uiFocusArray)
        {
            RegistorEventTrigger(item.GetComponent<RectTransform>(), EventTriggerType.PointerClick, ClickFocus);
        }

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

    private void OnEnable()
    {
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        roleOfRaceExplanList = new List<KeyValuePair<RoleOfRace, string>>();
        FieldExplanAttribute.SetEnumExplanDic(roleOfRaceExplanList);
        GameState.Instance.Registor<IPlayerAttributeState>(IPlayerAttributeStateChanged);
        UpdateText();
        UIManager.Instance.KeyPressHandle += Instance_KeyPressHandle;
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        scrollbar.value = 0;
        uiFocusPath = GetComponent<UIFocusPath>();
        UIFocus[] uiFocusArray = uiFocusPath.NewUIFocusArray;
        foreach (UIFocus uiFocus in uiFocusArray)
        {
            uiFocus.SetForcus();
            uiFocus.LostForcus();
        }
        //给任务系统填入状态
        INowTaskState iNowTaskState = GameState.Instance.GetEntity<INowTaskState>();
        iNowTaskState.CheckNowTask(EnumCheckTaskType.Special, (int)TaskMap.Enums.EnumTaskSpecialCheck.OpenAttributeUI);
    }

    /// <summary>
    /// 属性发生变化更新文本
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void IPlayerAttributeStateChanged(IPlayerAttributeState arg1, string arg2)
    {
        UpdateText();
    }

    private void OnDisable()
    {
        UIManager.Instance.KeyPressHandle -= Instance_KeyPressHandle;
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        GameState.Instance.UnRegistor<IPlayerAttributeState>(IPlayerAttributeStateChanged);
        UIFocus[] uiFocusArray = uiFocusPath.NewUIFocusArray;
        foreach (UIFocus uiFocus in uiFocusArray)
        {
            uiFocus.LostForcus();
        }
    }


    /// <summary>
    /// 按键检测(抬起)
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        Func<UIFocusPath.MoveType, UIFocus> GetNext = (key) =>
         {
             UIFocus nextFocus = uiFocusPath.GetNewNextFocus(nowUIFocus, key);
             return nextFocus;
         };
        switch (keyType)
        {
            case UIManager.KeyType.A:
                if (nowUIFocus)
                {
                    nowUIFocus.MoveChild(UIFocusPath.MoveType.OK);
                }
                break;
            case UIManager.KeyType.LEFT:
                if (nowUIFocus)
                {
                    nowUIFocus.MoveChild(UIFocusPath.MoveType.LEFT);
                }
                break;
            case UIManager.KeyType.RIGHT:
                if (nowUIFocus)
                {
                    nowUIFocus.MoveChild(UIFocusPath.MoveType.RIGHT);
                }
                break;
            case UIManager.KeyType.UP:
            case UIManager.KeyType.DOWN:
                if (!nowUIFocus)
                    nowUIFocus = uiFocusPath.GetFirstFocus();
                else
                {
                    UIFocus nextFocus = GetNext(keyType == UIManager.KeyType.UP ? UIFocusPath.MoveType.UP : UIFocusPath.MoveType.DOWN);
                    nowUIFocus.LostForcus();
                    nowUIFocus = nextFocus;
                }
                if (nowUIFocus)
                    nowUIFocus.SetForcus();
                break;
        }
    }

    private void ClickFocus(BaseEventData data)
    {
        PointerEventData pData = data as PointerEventData;
        if (pData == null)
            return;
        UIAddBaseAttribute uiFocus = pData.rawPointerPress.GetComponent<UIFocus>() as UIAddBaseAttribute;
        if (uiFocus != null)
        {
            if (nowUIFocus)
                nowUIFocus.LostForcus();
            if (uiFocus != nowUIFocus)
                nowUIFocus = uiFocus;
            else
                nowUIFocus = null;
            if (nowUIFocus)
                nowUIFocus.SetForcus();
        }
    }

    /// <summary>
    /// 按键检测(按住)
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyPressHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        switch (keyType)
        {
            case UIManager.KeyType.LEFT_ROCKER:
                scrollbar.value += rockValue.y * Time.deltaTime * 0.2f;
                break;
        }
    }

    /// <summary>
    /// 更新文本
    /// </summary>
    private void UpdateText()
    {
        string roleOfRaceName = "";
        try
        {
            roleOfRaceName = roleOfRaceExplanList.Find(temp => temp.Key == playerState.RoleOfRaceRoute.LastOrDefault()).Value;
        }
        catch { }
        levelText.text = playerState.PlayerName + "(" + roleOfRaceName + ")";
        levelText.text = "等级:" + playerState.Level + "    " + "经验:" + iPlayerState.Experience + "/" + iPlayerState.MaxExperience;
        pointText.text = "属性点:" + playerState.PropertyPoint + "    " + "技能点:" + playerState.FreedomPoint;
    }
}
