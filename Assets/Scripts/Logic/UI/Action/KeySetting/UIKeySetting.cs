using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 按键设置UI界面
/// </summary>
public class UIKeySetting : MonoBehaviour
{
    /// <summary>
    /// 按键设置键位路径
    /// </summary>
    UIFocusPath keySettingFocusPath;

    /// <summary>
    /// 当前的焦点框
    /// </summary>
    UIFocusKeySettingLattice nowKeySettingLattice;

    /// <summary>
    /// 按键设置中的状态
    /// </summary>
    EnumKeySettingType enumKeySettingType;

    /// <summary>
    /// uiList对象
    /// </summary>
    public UIList uiKeySettingList;
    /// <summary>
    /// 选择项面板
    /// </summary>
    public RectTransform selectTargetPanel;
    /// <summary>
    /// 当前选择项
    /// </summary>
    private UIListItem nowKeySettingListItem;

    void Awake()
    {
        keySettingFocusPath = GetComponent<UIFocusPath>();
        if (keySettingFocusPath)
        {
            //给每个键位加一个点击事件，处理鼠标点击获取焦点
            UIFocus[] uiKeySettingLatticeArray = keySettingFocusPath.UIFocuesArray;
            foreach (UIFocus uiKeySettingLattice in uiKeySettingLatticeArray)
            {
                if (!uiKeySettingLattice)
                    continue;
                EventTrigger eventTrigger = uiKeySettingLattice.gameObject.AddComponent<EventTrigger>();
                eventTrigger.triggers = new List<EventTrigger.Entry>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback = new EventTrigger.TriggerEvent();
                entry.callback.AddListener(KeySettingLatticeClick);
                eventTrigger.triggers.Add(entry);
            }
        }
        uiKeySettingList.ItemClickHandle += UiList_ItemClickHandle;
    }

    /// <summary>
    /// 点击选择项
    /// </summary>
    /// <param name="mouseType"></param>
    /// <param name="target"></param>
    private void UiList_ItemClickHandle(UIList.ItemClickMouseType mouseType, UIListItem target)
    {
        if (nowKeySettingListItem && nowKeySettingListItem.childImage)
            nowKeySettingListItem.childImage.enabled = false;
        nowKeySettingListItem = target;
        nowKeySettingListItem.childImage.enabled = true;
        //处理
        switch (mouseType)
        {
            case UIList.ItemClickMouseType.Left:
                SelectAction();
                break;
        }
    }

    /// <summary>
    /// 按键设置框被点击
    /// </summary>
    /// <param name="e"></param>
    private void KeySettingLatticeClick(BaseEventData e)
    {
        PointerEventData pe = e as PointerEventData;
        if (pe.pointerCurrentRaycast.gameObject != null)
        {
            UIFocusKeySettingLattice currentKeySettingLattice = UITools.FindTargetPopup<UIFocusKeySettingLattice>(pe.pointerCurrentRaycast.gameObject.transform);
            if (currentKeySettingLattice)
            {
                if(nowKeySettingLattice)
                nowKeySettingLattice.LostForcus();
                nowKeySettingLattice = currentKeySettingLattice;
                nowKeySettingLattice.SetForcus();
                //显示集合
                ShowUIList();
            }
        }
    }

    /// <summary>
    /// 当显示时初始化按键数据
    /// </summary>
    private void OnEnable()
    {
        selectTargetPanel.gameObject.SetActive(false);
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        enumKeySettingType = EnumKeySettingType.Lattice;
        if (keySettingFocusPath)
        {
            nowKeySettingLattice = keySettingFocusPath.GetFirstFocus() as UIFocusKeySettingLattice;
            if (nowKeySettingLattice)
                nowKeySettingLattice.SetForcus();
            //读取数据构建字典，键位与对应的数据
            //--------------------------//
            //初始化显示
            UIFocus[] uiKeySettingLatticeArray = keySettingFocusPath.UIFocuesArray;
            foreach (UIFocus item in uiKeySettingLatticeArray)
            {
                if (!item)
                    continue;
                UIFocusKeySettingLattice uiKeySettingLattice = item as UIFocusKeySettingLattice;
                if (uiKeySettingLattice)
                {
                    //通过上面的字典设置对应的格子的ID和类型
                    //--------------------------//
                    //初始化显示
                    uiKeySettingLattice.InitShow();
                }
            }
        }
    }

    /// <summary>
    /// 当隐藏时处理
    /// </summary>
    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        if (nowKeySettingLattice)
        {
            nowKeySettingLattice.LostForcus();
        }
        //保存状态
        //-------------------//
    }

    /// <summary>
    /// 接收输入
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        if (!nowKeySettingLattice && keySettingFocusPath)
            nowKeySettingLattice = keySettingFocusPath.GetFirstFocus() as UIFocusKeySettingLattice;
        switch (enumKeySettingType)
        {
            case EnumKeySettingType.Lattice://此时可以移动和确认
                Action<UIFocusPath.MoveType> MoveNExtEndAction = (moveType) => 
                {
                    if (keySettingFocusPath)
                    {
                        UIFocusKeySettingLattice currentKeySettingLattice = keySettingFocusPath.GetNextFocus(nowKeySettingLattice, moveType) as UIFocusKeySettingLattice;
                        if (currentKeySettingLattice != null && !object.Equals(currentKeySettingLattice, nowKeySettingLattice))
                        {
                            nowKeySettingLattice.LostForcus();
                            nowKeySettingLattice = currentKeySettingLattice;
                            nowKeySettingLattice.SetForcus();
                        }
                    }
                };
                switch (keyType)
                {
                    case UIManager.KeyType.A://进入选择
                        //显示集合
                        ShowUIList();
                        break;
                    case UIManager.KeyType.LEFT:
                        MoveNExtEndAction(UIFocusPath.MoveType.LEFT);
                        break;
                    case UIManager.KeyType.RIGHT:
                        MoveNExtEndAction(UIFocusPath.MoveType.RIGHT);
                        break;
                    case UIManager.KeyType.UP:
                        MoveNExtEndAction(UIFocusPath.MoveType.UP);
                        break;
                    case UIManager.KeyType.DOWN:
                        MoveNExtEndAction(UIFocusPath.MoveType.DOWN);
                        break;
                }
                break;
            case EnumKeySettingType.Select://此时可以选择技能
                Action<int> MoveListSelect = (addOffset) => 
                {
                    UIListItem[] tempArrays = uiKeySettingList.GetAllImtes();
                    int index = 0;
                    if (nowKeySettingListItem)
                        index = tempArrays.ToList().IndexOf(nowKeySettingListItem);
                    if (index < 0)
                        index = 0;
                    index += addOffset;
                    index = Mathf.Clamp(index, 0, tempArrays.Length - 1);
                    if (index < tempArrays.Length && tempArrays.Length>0)
                    {
                        uiKeySettingList.ShowItem(tempArrays[index]);
                        if (nowKeySettingListItem && nowKeySettingListItem.childImage)
                            nowKeySettingListItem.childImage.enabled = false;
                        nowKeySettingListItem = tempArrays[index];
                        if (nowKeySettingListItem && nowKeySettingListItem.childImage)
                            nowKeySettingListItem.childImage.enabled = true;
                    }
                };
                switch (keyType)
                {
                    case UIManager.KeyType.A://确认选择
                        SelectAction();
                        break;
                    case UIManager.KeyType.UP:
                        MoveListSelect(-1);
                        break;
                    case UIManager.KeyType.DOWN:
                        MoveListSelect(1);
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// 显示集合
    /// </summary>
    private void ShowUIList()
    {
        uiKeySettingList.Init();
        //注意第一项弄成空的
        UIListItem firstItem = uiKeySettingList.NewItem();
        firstItem.transform.GetChild(1).GetComponent<Text>().text = "None";
        firstItem.value = null;
        uiKeySettingList.UpdateUI();
        //其他项从技能和道具中检索

        //最后的设置
        nowKeySettingListItem = uiKeySettingList.FirstShowItem();
        uiKeySettingList.ShowItem(nowKeySettingListItem);
        if (nowKeySettingListItem)
            nowKeySettingListItem.childImage.enabled = true;
        selectTargetPanel.gameObject.SetActive(true);
        //状态改为选择技能
        enumKeySettingType = EnumKeySettingType.Select;
    }

    /// <summary>
    /// 选择当前项
    /// </summary>
    private void SelectAction()
    {
        //设置当前框内的显示图片

        //最后的设置
        selectTargetPanel.gameObject.SetActive(false);
        //状态改为格子状态
        enumKeySettingType = EnumKeySettingType.Lattice;
    }

    /// <summary>
    /// 按键设置的状态
    /// </summary>
    private enum EnumKeySettingType
    {
        /// <summary>
        /// 格子状态
        /// </summary>
        Lattice,
        /// <summary>
        /// 选择状态
        /// </summary>
        Select
    }
}
