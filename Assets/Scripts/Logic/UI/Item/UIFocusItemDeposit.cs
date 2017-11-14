using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 道具界面的左侧存储栏
/// </summary>
public class UIFocusItemDeposit : UIFocus
{
    /// <summary>
    /// 获取焦点状态
    /// </summary>
    bool focused;

    /// <summary>
    /// 存储栏集合控件
    /// </summary>
    UIList uiDepostiList;

    /// <summary>
    /// 获取焦点的集合条目
    /// </summary>
    UIListItem focusUIListItem;

    private void Awake()
    {
        //获取List控件
        uiDepostiList = GetComponent<UIList>();
        uiDepostiList.ItemClickHandle += UiDepostiList_ItemClickHandle;
    }

    /// <summary>
    /// 集合中的条目被点击
    /// </summary>
    /// <param name="mouseType"></param>
    /// <param name="target"></param>
    private void UiDepostiList_ItemClickHandle(UIList.ItemClickMouseType mouseType, UIListItem target)
    {
        if (focusUIListItem != null && focusUIListItem.childImage != null)
            focusUIListItem.childImage.enabled = false;
        focusUIListItem = target;
        focusUIListItem.childImage.enabled = true;
        //处理功能
        switch (mouseType)
        {
            case UIList.ItemClickMouseType.Right:
                ItemAction();
                break;
        }
    }

    private void OnEnable()
    {
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        UIManager.Instance.KeyPressHandle += Instance_KeyPressHandle;
        //读取数据并初始化控件
        //给List控件重新填充数据
        //需要读取数据因此这里未实现
        //UIListItem uiListItem = uiDepostiList.NewItem();
        //uiDepostiList.UpdateUI();
        focusUIListItem = uiDepostiList.GetAllImtes().FirstOrDefault();
        if (focusUIListItem)
        {
            if (focusUIListItem.childImage)
            {
                focusUIListItem.childImage.enabled = true;
            }
        }
    }

    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        UIManager.Instance.KeyPressHandle -= Instance_KeyPressHandle;
    }

    /// <summary>
    /// 设置焦点
    /// 已经在OnEnable处理过第一个焦点获取了，此时不需要重设
    /// </summary>
    public override void SetForcus()
    {
        focused = true;
    }

    /// <summary>
    /// 失去焦点
    /// 不需要重设
    /// </summary>
    public override void LostForcus()
    {
        focused = false;
    }

    /// <summary>
    /// 是否可以移动本焦点
    /// </summary>
    /// <param name="moveType"></param>
    /// <returns></returns>
    public override bool CanMoveNext(UIFocusPath.MoveType moveType)
    {
        if (!uiDepostiList)
            return true;
        switch (moveType)
        {
            case UIFocusPath.MoveType.LEFT:
            case UIFocusPath.MoveType.RIGHT:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// 移动子焦点（list中的数据，这里不用焦点）
    /// </summary>
    /// <param name="moveType"></param>
    public override void MoveChild(UIFocusPath.MoveType moveType)
    {
        UIListItem[] tempArrays = uiDepostiList.GetAllImtes();
        if (tempArrays.Length == 0)
            return;
        int index = 0;
        if (focusUIListItem)
            index = tempArrays.ToList().IndexOf(focusUIListItem);
        if (index < 0)
            index = 0;
        switch (moveType)
        {
            case UIFocusPath.MoveType.UP://-
                index--;
                break;
            case UIFocusPath.MoveType.DOWN://+
                index++;
                break;
        }
        index = Mathf.Clamp(index, 0, tempArrays.Length - 1);
        if (index < tempArrays.Length)
        {
            uiDepostiList.ShowItem(tempArrays[index]);
            if (focusUIListItem && focusUIListItem.childImage)
                focusUIListItem.childImage.enabled = false;
            focusUIListItem = tempArrays[index];
            if (focusUIListItem && focusUIListItem.childImage)
                focusUIListItem.childImage.enabled = true;
        }
    }

    /// <summary>
    /// 按键检测
    /// 检测功能，移动处理在MoveChild
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        switch (keyType)
        {
            case UIManager.KeyType.A:
                ItemAction();
                break;
        }
    }

    /// <summary>
    /// 检测持续按下
    /// 主要用于检测摇杆实现快速翻动
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyPressHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        //只有获取焦点时才可以使用摇杆
        if (focused && keyType == UIManager.KeyType.LEFT_ROCKER)
        {
            //向上- 向下+
            if (Mathf.Abs(rockValue.y) > 0)
            {
                uiDepostiList.MoveScroll(-rockValue.y);
                if (!uiDepostiList.ItemIsShow(focusUIListItem))
                {
                    if (focusUIListItem && focusUIListItem.childImage)
                        focusUIListItem.childImage.enabled = false;
                    focusUIListItem = rockValue.y > 0 ? uiDepostiList.FirstShowItem() : uiDepostiList.LastShowItem();
                    if (focusUIListItem && focusUIListItem.childImage)
                        focusUIListItem.childImage.enabled = true;
                }
            }
        }
    }

    /// <summary>
    /// 开始道具的功能
    /// 如果是装备则替换，如果是药水则恢复
    /// </summary>
    private void ItemAction()
    {
        if (focusUIListItem)
        {
            //通过id获取对象
            int id = (int)focusUIListItem.value;
        }
    }
}
