using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 道具界面的中间装备栏焦点对象
/// 当接收焦点后，负责通知给他的子节点
/// </summary>
public class UIFocusItemEquipment : UIFocus
{
    /// <summary>
    /// 装备格子的路径
    /// </summary>
    UIFocusPath equipentsLatticePath;

    /// <summary>
    /// 当前获得焦点的格子
    /// </summary>
    UIFocusItemEquipmentLattice nowLattice;

    private void Awake()
    {
        equipentsLatticePath = GetComponent<UIFocusPath>();
    }

    /// <summary>
    /// 设置焦点
    /// </summary>
    public override void SetForcus()
    {
        if (equipentsLatticePath)
        {
            nowLattice = equipentsLatticePath.GetFirstFocus() as UIFocusItemEquipmentLattice;
            try
            {
                UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
            }
            catch { }
            UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        }
    }

    /// <summary>
    /// 是否可以移动到下一个
    /// 当左右移动时判断是否需要给予焦点
    /// 当上下移动时移动自身格子的焦点
    /// </summary>
    /// <param name="moveType"></param>
    /// <returns></returns>
    public override bool CanMoveNext(UIFocusPath.MoveType moveType)
    {
        if (!equipentsLatticePath)
            return true;
        UIFocusItemEquipmentLattice tempLattice = equipentsLatticePath.GetNextFocus(nowLattice, moveType) as UIFocusItemEquipmentLattice;//查询下一个位置
        switch (moveType)
        {
            //当左右移动时需要判断下一个目标是不是空，如果是空，则允许上层移动焦点,如果不为空则本身移动
            //当上下移动时，不需要判断，如果为空则不处理，则下方也不会处理，如果不为空则移动
            case UIFocusPath.MoveType.LEFT:
            case UIFocusPath.MoveType.RIGHT:
                if (!tempLattice)
                    return true;
                break;
        }
        return false;
    }

    /// <summary>
    /// 移动子焦点
    /// </summary>
    /// <param name="moveType"></param>
    public override void MoveChild(UIFocusPath.MoveType moveType)
    {
        if (!equipentsLatticePath)
            return;
        UIFocusItemEquipmentLattice tempLattice = equipentsLatticePath.GetNextFocus(nowLattice, moveType) as UIFocusItemEquipmentLattice;//查询下一个位置
        if (tempLattice)
        {
            nowLattice.LostForcus();
            nowLattice = tempLattice;
            nowLattice.SetForcus();
        }
    }

    /// <summary>
    /// 失去焦点
    /// </summary>
    public override void LostForcus()
    {
        if (nowLattice)
        {
            nowLattice.LostForcus();
            try
            {
                UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
            }
            catch { }
        }
    }

    /// <summary>
    /// 按键检测
    /// 处理每个单元格的状态
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        if (nowLattice)
        {
            switch (keyType)
            {
                case UIManager.KeyType.A:
                    //写下装备
                    break;
            }
        }
    }

    /// <summary>
    /// 用户送开始鼠标在装备格子上
    /// </summary>
    /// <param name="e">事件</param>
    public void LatticMouseUp(BaseEventData e)
    {
        PointerEventData pe = e as PointerEventData;
        if (pe.pointerCurrentRaycast.gameObject != null)
        {
            UIFocusItemEquipmentLattice currentLattice = UITools.FindTargetPopup<UIFocusItemEquipmentLattice>(pe.pointerCurrentRaycast.gameObject.transform);
            if (currentLattice)
            {
                if (nowLattice)
                    nowLattice.LostForcus();
                nowLattice = currentLattice;
                nowLattice.SetForcus();
                switch (pe.button)
                {
                    case PointerEventData.InputButton.Right:
                        //处理拿下装备
                        break;
 
                }
            }
        }
    }
}
