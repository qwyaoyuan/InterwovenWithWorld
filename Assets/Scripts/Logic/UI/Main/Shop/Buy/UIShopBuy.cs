using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 买物品
/// </summary>
public class UIShopBuy : UIShopExplan
{

    /// <summary>
    /// 是否正在等待确认
    /// </summary>
    bool waitTip;

    /// <summary>
    /// 开始时第一次的按键抬起(此时不可以使用)
    /// </summary>
    bool firstKeyUP;

    protected override void OnEnable()
    {
        base.OnEnable();
        waitTip = false;
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
    }

    protected override void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
    }

    protected override void Init()
    {
        firstKeyUP = false;
        base.Init();
        //清理之前的道具
        ItemUIList.Init();
        //更新商店
        base.UpdateBusinessman();
        //显示商品道具
        if (businessman != null)
        {
            List<PlayGoods> allPlayGoodses = new List<PlayGoods>();
            allPlayGoodses.AddRange(businessman.BaseList);//首先添加基础商品
            allPlayGoodses.AddRange(businessman.SellPropsList);//然后添加回购商品
            ResetItems(allPlayGoodses);
        }
    }

    private void Instance_KeyUpHandle(UIManager.KeyType arg1, Vector2 arg2)
    {
        if (!firstKeyUP)
        {
            firstKeyUP = true;
            return;
        }

        if (!waitTip)
        {
            base.MoveItemSelect(arg1);
            if (arg1 == UIManager.KeyType.A)
            {
                if (SelectGoods != null)
                {
                    waitTip = true;
                    uiShowTip.Show(result =>
                    {
                        waitTip = false;
                        if (result)//如果确认后则将现在选择的条目进行处理
                        {
                            ItemClickAction();
                        }
                    });
                }
            }
        }
        else
        {
            if (arg1 == UIManager.KeyType.B)
            {
                uiShowTip.gameObject.SetActive(false);
                waitTip = false;
            }
            else
                uiShowTip.GetKeyDown(arg1 == UIManager.KeyType.LEFT ? UIFocusPath.MoveType.LEFT :
                    (arg1 == UIManager.KeyType.RIGHT ? UIFocusPath.MoveType.RIGHT :
                    (arg1 == UIManager.KeyType.A ? UIFocusPath.MoveType.OK : UIFocusPath.MoveType.DOWN)));
        }
    }

    /// <summary>
    /// 通过计算的价格返回应该显示的话(出售价格或购买价格等)
    /// </summary>
    /// <param name="sprice"></param>
    /// <returns></returns>
    protected override string ShowSpriceActionValue(int sprice)
    {
        return "购买价格:" + sprice;
    }

    /// <summary>
    /// 点击条目事件
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    protected override void ItemUIList_ItemClickHandle(UIList.ItemClickMouseType arg1, UIListItem arg2)
    {
        if (arg2 != null)
        {
            base.ItemUIList_ItemClickHandle(arg1, arg2);
            if (arg1 == UIList.ItemClickMouseType.Right)
            {
                if (SelectGoods != null)
                {
                    waitTip = true;
                    uiShowTip.Show(result =>
                    {
                        waitTip = false;
                        if (result)//如果确认后则将现在选择的条目进行处理
                        {
                            ItemClickAction();
                        }
                    });
                }
            }

        }
    }

    /// <summary>
    /// 点击条目后的动作
    /// </summary>
    private void ItemClickAction()
    {
        //购买物品到包裹
        if (SelectGoods == null)
            return;
        PlayGoods playGoods = SelectGoods.value as PlayGoods;
        if (playGoods == null)
            return;
        //判断商品的价格,如果可以购买则减去金钱
        int sprice = GoodsSprice(playGoods.GoodsInfo);
        if (playerState.Sprice < sprice)
            return;
        playerState.Sprice -= sprice;
        iPlayerState.SpriteChanged = true;//设置金钱发生变化

        EnumGoodsType baseGoodsType = playGoods.GoodsInfo.EnumGoodsType;
        //如果是药品或道具等消耗品
        if (baseGoodsType > EnumGoodsType.Item && baseGoodsType < EnumGoodsType.SpecialItem)
        {
            //玩家自身是否存在消耗品
            PlayGoods player_playGoods = playerState.PlayerAllGoods.FirstOrDefault(temp => temp.GoodsInfo.EnumGoodsType == baseGoodsType);
            if (player_playGoods != null)//如果存在则直接加count
            {
                player_playGoods.Count++;
            }
            else//如果不存在则创建
            {
                PlayGoods newPlayGoods = new PlayGoods(playGoods.ID, playGoods.GoodsInfo, GoodsLocation.Package);
                newPlayGoods.Count = 1;
                newPlayGoods.QualityType = playGoods.QualityType;
                playerState.PlayerAllGoods.Add(newPlayGoods);
            }
            //商店数量减少
            playGoods.Count--;
            if (playGoods.Count <= 0)//如果当前数量归0则移除
            {
                if (businessman.BaseList.Contains(playGoods))
                {
                    businessman.BaseList.Remove(playGoods);
                }
                else businessman.SellPropsList.Remove(playGoods);
                //显示列表减少
                RemoveItemShow(playGoods);
            }
            else//如果没有归零则修改数量的显示
            {
                ChangeItemsShow(playGoods);
            }
        }
        else
        {
            //将道具转移到玩家身上
            playGoods.GoodsLocation = GoodsLocation.Package;
            playerState.PlayerAllGoods.Add(playGoods);
            //移除商人身上的道具
            if (businessman.BaseList.Contains(playGoods))
            {
                businessman.BaseList.Remove(playGoods);
            }
            else businessman.SellPropsList.Remove(playGoods);
            //显示列表减少
            RemoveItemShow(playGoods);
        }
    }

    /// <summary>
    /// 当前是否可以退出
    /// </summary>
    /// <returns></returns>
    public override bool CanExit()
    {
        return waitTip == false;
    }

}
