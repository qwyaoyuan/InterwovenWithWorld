using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 道具界面的中间装备栏焦点对象
/// 当接收焦点后，负责通知给他的子节点
/// </summary>
public class UIFocusItemEquipment : UIFocus, IUIItemSelectGoods
{
    /// <summary>
    /// 装备格子的路径
    /// </summary>
    UIFocusPath equipentsLatticePath;

    /// <summary>
    /// 当前获得焦点的格子
    /// </summary>
    UIFocusItemEquipmentLattice nowLattice;

    /// <summary>
    /// 所有的格子
    /// </summary>
    UIFocusItemEquipmentLattice[] allLttices;

    /// <summary>
    /// 玩家状态对象
    /// </summary>
    PlayerState playerState;
    /// <summary>
    /// 玩家运行时状态
    /// </summary>
    IPlayerState iPlayerStateRun;

    /// <summary>
    /// 选择物体后的回调
    /// </summary>
    Action<int> SelectGoodsIDAction;

    private void Awake()
    {
        equipentsLatticePath = GetComponent<UIFocusPath>();
        allLttices = equipentsLatticePath.NewUIFocusArray.Select(temp => temp as UIFocusItemEquipmentLattice).ToArray();// equipentsLatticePath.UIFocuesArray.Select(temp => temp as UIFocusItemEquipmentLattice).ToArray();
    }

    /// <summary>
    /// 显示时设置图片
    /// </summary>
    private void OnEnable()
    {
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        iPlayerStateRun = GameState.Instance.GetEntity<IPlayerState>();
        GameState.Instance.Registor<IPlayerState>(IPlayerStateChanged);
        ResetLatticeValueAndShow();
    }

    private void OnDisable()
    {
        GameState.Instance.UnRegistor<IPlayerState>(IPlayerStateChanged);
        try
        {
            UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        }
        catch { }
    }

    /// <summary>
    /// 注册选择物体后的回调
    /// </summary>
    /// <param name="SelectGoodsIDAction"></param>
    public void RegistorSelectGoodsID(Action<int> SelectGoodsIDAction)
    {
        this.SelectGoodsIDAction = SelectGoodsIDAction;
    }

    /// <summary>
    /// 不需要外部去处理如何选择
    /// </summary>
    /// <param name="goodsID"></param>
    public void SelectID(int goodsID) { }

    /// <summary>
    /// 重新设置格子的显示与数据
    /// </summary>
    private void ResetLatticeValueAndShow()
    {
        PlayGoods[] WearingPlayGoods = playerState.PlayerAllGoods.Where(temp => temp.GoodsLocation == GoodsLocation.Wearing).ToArray();
        //设置对应位置的装备
        foreach (UIFocusItemEquipmentLattice equipmentLattice in allLttices)
        {
            if (equipmentLattice == null)
                continue;
            PlayGoods[] firstCheck = WearingPlayGoods.Where(temp => (int)temp.GoodsInfo.EnumGoodsType > equipmentLattice.minType && (int)temp.GoodsInfo.EnumGoodsType < equipmentLattice.maxType).ToArray();
            if (firstCheck.Length > 0)
            {
                switch (equipmentLattice.handedType)
                {
                    case UIFocusItemEquipmentLattice.EnumWeaponType.None://不是左右主手武器则直接显示
                        PlayGoods playGoods_None = firstCheck.First();
                        equipmentLattice.value = playGoods_None;
                        equipmentLattice.SetShowImage(playGoods_None.GetGoodsSprite);
                        break;
                    case UIFocusItemEquipmentLattice.EnumWeaponType.LeftOneHanded://副手武器
                        PlayGoods[] leftOneHanded = firstCheck.Where(temp => temp.leftRightArms != null && temp.leftRightArms.Value == true).ToArray();
                        if (leftOneHanded.Length > 0)
                        {
                            PlayGoods playGoods_LeftOneHanded = leftOneHanded.First();
                            equipmentLattice.value = playGoods_LeftOneHanded;
                            equipmentLattice.SetShowImage(playGoods_LeftOneHanded.GetGoodsSprite);
                        }
                        else
                        {
                            equipmentLattice.value = null;
                            equipmentLattice.SetShowImage(null);
                        }
                        break;
                    case UIFocusItemEquipmentLattice.EnumWeaponType.RightOneHanded:
                        PlayGoods[] rightOneHanded = firstCheck.Where(temp => temp.leftRightArms != null && temp.leftRightArms.Value == false).ToArray();
                        if (rightOneHanded.Length > 0)
                        {
                            PlayGoods playGoods_RightOneHanded = rightOneHanded.First();
                            equipmentLattice.value = playGoods_RightOneHanded;
                            equipmentLattice.SetShowImage(playGoods_RightOneHanded.GetGoodsSprite);
                        }
                        else
                        {
                            equipmentLattice.value = null;
                            equipmentLattice.SetShowImage(null);
                        }
                        break;
                }
            }
            else
            {
                equipmentLattice.value = null;
                equipmentLattice.SetShowImage(null);
            }
        }
    }

    /// <summary>
    /// 玩家状态发生变化
    /// </summary>
    /// <param name="iPlayerState"></param>
    /// <param name="name"></param>
    private void IPlayerStateChanged(IPlayerState iPlayerState, string name)
    {
        if (name == GameState.Instance.GetFieldName<IPlayerState, bool>(temp => temp.EquipmentChanged))//主要用于装备发生变化时修改集合
        {
            ResetLatticeValueAndShow();
        }
    }

    /// <summary>
    /// 设置焦点
    /// </summary>
    public override void SetForcus()
    {
        if (equipentsLatticePath)
        {
            nowLattice = equipentsLatticePath.GetFirstFocus() as UIFocusItemEquipmentLattice;
            if (nowLattice)
                nowLattice.SetForcus();
            SelectNowLattice();
            try
            {
                UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
            }
            catch { }
            UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        }
    }

    /// <summary>
    /// 选择了该格子
    /// </summary>
    private void SelectNowLattice()
    {
        if (nowLattice != null)
        {
            //设置选择了物体
            PlayGoods playGoods = nowLattice.value as PlayGoods;
            if (SelectGoodsIDAction != null)
                if (playGoods != null)
                    SelectGoodsIDAction(playGoods.ID);
                else
                    SelectGoodsIDAction(-1);
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
        UIFocusItemEquipmentLattice tempLattice = equipentsLatticePath.GetNewNextFocus(nowLattice, moveType) as UIFocusItemEquipmentLattice;// equipentsLatticePath.GetNextFocus(nowLattice, moveType) as UIFocusItemEquipmentLattice;//查询下一个位置
        switch (moveType)
        {
            //当左右移动时需要判断下一个目标是不是空，如果是空，则允许上层移动焦点,如果不为空则本身移动
            //当上下移动时，不需要判断，如果为空则不处理，则下方也不会处理，如果不为空则移动
            case UIFocusPath.MoveType.LEFT:
                //case UIFocusPath.MoveType.RIGHT://暂时没有向右移动失去焦点的功能
                if (!tempLattice)
                {
                    if (nowLattice)
                    {
                        nowLattice.LostForcus();
                    }
                    return true;
                }
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
        UIFocusItemEquipmentLattice tempLattice = equipentsLatticePath.GetNewNextFocus(nowLattice, moveType) as UIFocusItemEquipmentLattice;// equipentsLatticePath.GetNextFocus(nowLattice, moveType) as UIFocusItemEquipmentLattice;//查询下一个位置
        if (tempLattice)
        {
            nowLattice.LostForcus();
            nowLattice = tempLattice;
            nowLattice.SetForcus();
        }
        SelectNowLattice();
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
                    EquipmentLatticeAction();
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
                //设置选择了物体
                SelectNowLattice();
                switch (pe.button)
                {
                    case PointerEventData.InputButton.Right:
                        EquipmentLatticeAction();
                        break;

                }
            }
        }
    }

    /// <summary>
    /// 格子的处理动作
    /// </summary>
    public void EquipmentLatticeAction()
    {
        if (nowLattice)
        {
            if (nowLattice.value != null && (nowLattice.value as PlayGoods) != null)
            {
                PlayGoods playGoods = nowLattice.value as PlayGoods;
                playGoods.leftRightArms = null;
                playGoods.GoodsLocation = GoodsLocation.Package;
                iPlayerStateRun.EquipmentChanged = true;
            }
        }
    }
}
