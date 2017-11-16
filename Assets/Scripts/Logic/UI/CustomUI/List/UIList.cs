using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using System;

/// <summary>
/// List控件
/// 自定义控件
/// </summary>
public class UIList : MonoBehaviour
{
    /// <summary>
    /// 每个条目的高度
    /// </summary>
    public int itemHeight = 40;
    /// <summary>
    /// 显示面板
    /// </summary>
    public RectTransform showRect;
    /// <summary>
    /// 滑杆
    /// </summary>
    public Scrollbar scrollbar;
    /// <summary>
    /// 背景面板
    /// </summary>
    RectTransform backRect;

    /// <summary>
    /// 默认的控件样式
    /// </summary>
    public GameObject tempItemObj;

    /// <summary>
    /// 滑杆移动倍率
    /// </summary>
    public float scrollMoveRate = 10;

    /// <summary>
    /// 条目集合
    /// </summary>
    List<GameObject> itemsList;

    /// <summary>
    /// 鼠标进入状态
    /// </summary>
    bool mouseEntered;

    /// <summary>
    /// 点击条目事件 
    /// </summary>
    public event Action<ItemClickMouseType, UIListItem> ItemClickHandle;

    private void Awake()
    {
        if (showRect)
        {
            backRect = showRect.parent.GetComponent<RectTransform>();
        }
        itemsList = new List<GameObject>();
        Init();
    }

    private void Start()
    {
        //添加事件
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
        eventTrigger.triggers = new List<EventTrigger.Entry>();

        EventTrigger.Entry mouseEnter = new EventTrigger.Entry();
        mouseEnter.eventID = EventTriggerType.PointerEnter;
        mouseEnter.callback = new EventTrigger.TriggerEvent();
        mouseEnter.callback.AddListener(MouseEnter);
        eventTrigger.triggers.Add(mouseEnter);

        EventTrigger.Entry mouseExit = new EventTrigger.Entry();
        mouseExit.eventID = EventTriggerType.PointerExit;
        mouseExit.callback = new EventTrigger.TriggerEvent();
        mouseExit.callback.AddListener(MouseExit);
        eventTrigger.triggers.Add(mouseExit);

        //滑轮的事件
        scrollbar.onValueChanged.AddListener(ScrollBarValueChanged);
    }


    /// <summary>
    /// 滑轮值变化
    /// </summary>
    /// <param name="value"></param>
    private void ScrollBarValueChanged(float value)
    {
        //在这里移动面板
        float showHeight = showRect.rect.height;
        float backHeight = backRect.rect.height;
        if (showHeight > backHeight)
        {
            float maxOffset = showHeight - backHeight;
            float upOffset = maxOffset * value;
            showRect.offsetMax = new Vector2(0, upOffset);
            showRect.offsetMin = new Vector2(0, upOffset - showHeight);
        }
        else
        {
            showRect.offsetMin = new Vector2(0, -backHeight);
            showRect.offsetMax = new Vector2(0, 0);
        }
    }

    private void MouseEnter(BaseEventData e)
    {
        mouseEntered = true;
        StopCoroutine("CheckMouseWheel");
        StartCoroutine("CheckMouseWheel");
    }

    private void MouseExit(BaseEventData e)
    {
        mouseEntered = false;
        StopCoroutine("CheckMouseWheel");
    }

    /// <summary>
    /// 检测鼠标滚轮
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckMouseWheel()
    {
        while (mouseEntered)
        {
            float wheelValue = Input.GetAxis("Mouse ScrollWheel");
            MoveScroll(-wheelValue * 10);
            yield return null;
        }
    }

    /// <summary>
    /// 移动滑杆
    /// </summary>
    /// <param name="value">移动的数值(这里指的是面板移动的像素值)</param>
    public void MoveScroll(float value)
    {
        if (value == 0)
            return;
        value *= scrollMoveRate;
        //这里不移动面板,只计算偏差值
        if (scrollbar.size < 0.99f && scrollbar.size > 0)
        {
            int allHeigth = itemHeight * itemsList.Count;
            if (allHeigth > backRect.rect.height)
            {
                float every = 1f / (allHeigth - backRect.rect.height);//每个像素的占比
                float end = every * value;//最终的移动占比
                float nowValue = scrollbar.value;
                nowValue += end;
                nowValue = Mathf.Clamp(nowValue, 0, 1);
                scrollbar.value = nowValue;
            }
        }
    }

    /// <summary>
    /// 获取指定的对象是否正在显示
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool ItemIsShow(UIListItem target)
    {
        if (!target)
            return false;
        int index = itemsList.IndexOf(target.gameObject);
        if (index < 0)
            return false;
        float showUp = Mathf.Abs(showRect.offsetMax.y);
        float showDown = showUp + backRect.rect.height;
        float itemUp = Mathf.Abs(index * itemHeight);
        float itemDown = itemUp + itemHeight;
        if (showUp < itemUp && showDown > itemUp)
            return true;
        return false;
    }

    /// <summary>
    /// 获取显示界面最后的条目
    /// </summary>
    /// <returns></returns>
    public UIListItem LastShowItem()
    {
        if (itemsList.Count == 0)
            return null;
        float showUp = Mathf.Abs(showRect.offsetMax.y);
        float showDown = showUp + backRect.rect.height;
        int index = (int)showDown % itemHeight;
        index = Mathf.Clamp(index, 0, itemsList.Count - 1);
        if (index < itemsList.Count)
        {
            if (itemsList[index] != null)
                return itemsList[index].GetComponent<UIListItem>();
        }
        return null;
    }

    /// <summary>
    /// 获取显示界面开始的条目
    /// </summary>
    /// <returns></returns>
    public UIListItem FirstShowItem()
    {
        if (itemsList.Count == 0)
            return null;
        float showUp = Mathf.Abs(showRect.offsetMax.y);
        float showDown = showUp + backRect.rect.height;
        int index = (int)showUp % itemHeight;
        index = Mathf.Clamp(index, 0, itemsList.Count - 1);
        if (index < itemsList.Count)
        {
            if (itemsList[index] != null)
                return itemsList[index].GetComponent<UIListItem>();
        }
        return null;
    }

    /// <summary>
    /// 显示指定的条目
    /// </summary>
    /// <param name="target"></param>
    public void ShowItem(UIListItem target)
    {
        if (!target)
            return;
        int index = itemsList.IndexOf(target.gameObject);
        if (index < 0)
            return;
        float showUp = Mathf.Abs(showRect.offsetMax.y);
        float showDown = showUp + backRect.rect.height;
        float itemUp = Mathf.Abs(index * itemHeight);
        float itemDown = itemUp + itemHeight;
        //判断当前条目实在显示面板的上面还是下面
        if (showUp > itemUp)//条目在显示区域上方，需要向上移动(负数)
        {
            if (scrollMoveRate > 0)
                MoveScroll((itemUp - showUp) / scrollMoveRate);
        }
        else if (showDown < itemDown)//条目在显示区域下方，需要向下移动(正数)
        {
            if (scrollMoveRate > 0)
                MoveScroll((itemDown - showDown) / scrollMoveRate);
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        if (itemsList != null)
            foreach (GameObject item in itemsList)
                if (item != null)
                    try { GameObject.DestroyImmediate(item); } catch { }
        itemsList = new List<GameObject>();
        UpdateUI();
    }

    /// <summary>
    /// 获取所有的条目游戏对象
    /// </summary>
    /// <returns></returns>
    public UIListItem[] GetAllImtes()
    {
        if (itemsList == null)
            itemsList = new List<GameObject>();
        return itemsList.Select(temp => temp.GetComponent<UIListItem>()).ToArray();
    }

    /// <summary>
    /// 创建并添加一个条目
    /// </summary>
    /// <param name="updateUI">更新UI界面</param>
    /// <returns></returns>
    public UIListItem NewItem(bool updateUI = false)
    {
        if (tempItemObj)
        {
            GameObject createItemObj = Instantiate(tempItemObj);
            createItemObj.SetActive(true);
            //添加事件
            EventTrigger eventTrigger = createItemObj.AddComponent<EventTrigger>();
            eventTrigger.triggers = new List<EventTrigger.Entry>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback = new EventTrigger.TriggerEvent();
            entry.callback.AddListener(ItemMouseUp);
            eventTrigger.triggers.Add(entry);
            // 添加到集合中
            itemsList.Add(createItemObj);
            //设置父物体
            createItemObj.transform.SetParent(showRect);
            if (updateUI)
                UpdateUI();
            return createItemObj.GetComponent<UIListItem>();
        }
        return null;
    }

    /// <summary>
    /// 点击条目事件
    /// </summary>
    /// <param name="e"></param>
    private void ItemMouseUp(BaseEventData e)
    {
        PointerEventData pe = e as PointerEventData;
        if (ItemClickHandle != null)
        {
            if (pe.pointerCurrentRaycast.gameObject != null)
            {
                UIListItem currentItem = UITools.FindTargetPopup<UIListItem>(pe.pointerCurrentRaycast.gameObject.transform);
                if (currentItem)
                {
                    switch (pe.button)
                    {
                        case PointerEventData.InputButton.Left:
                            ItemClickHandle(ItemClickMouseType.Left, currentItem);
                            break;
                        case PointerEventData.InputButton.Right:
                            ItemClickHandle(ItemClickMouseType.Right, currentItem);
                            break;
                        case PointerEventData.InputButton.Middle:
                            ItemClickHandle(ItemClickMouseType.Middle, currentItem);
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 移除指定的条目
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool RemoveItem(UIListItem target)
    {
        int removeCount = itemsList.RemoveAll(temp => object.Equals(temp.GetComponent<UIListItem>(), target));
        if (removeCount > 0)
        {
            UpdateUI();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 更新UI
    /// </summary>
    public void UpdateUI()
    {
        float allHeight = itemsList.Count * itemHeight;
        if (!showRect)
            return;
        float showHieght = showRect.rect.height;
        if(!backRect)
            backRect = showRect.parent.GetComponent<RectTransform>();
        if (!backRect)
            return;
        float backHeight = backRect.rect.height;
        if (allHeight > backHeight)
        {
            showRect.offsetMin = new Vector2(0, showRect.offsetMin.y - (allHeight - showHieght));
            scrollbar.size = backHeight / allHeight;
        }
        else
        {
            showRect.offsetMin = new Vector2(0, -backHeight);
            showRect.offsetMax = new Vector2(0, 0);
            scrollbar.size = 1;
            scrollbar.value = 0;
        }
        //重新设置各个条目的位置
        for (int i = 0; i < itemsList.Count; i++)
        {
            RectTransform rect = itemsList[i] ? itemsList[i].GetComponent<RectTransform>() : null;
            if (rect)
            {
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(1, 1);
                rect.offsetMin = new Vector2(0, -(i + 1) * itemHeight);
                rect.offsetMax = new Vector2(0, -i * itemHeight);
            }
        }
    }



    /// <summary>
    /// 在条目上点击鼠标的键位
    /// </summary>
    public enum ItemClickMouseType
    {
        /// <summary>
        /// 左键
        /// </summary>
        Left,
        /// <summary>
        /// 右键
        /// </summary>
        Right,
        /// <summary>
        /// 中键
        /// </summary>
        Middle
    }
}
