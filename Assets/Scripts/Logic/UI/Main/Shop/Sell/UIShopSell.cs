using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 卖物品
/// </summary>
public class UIShopSell : UIShopExplan
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
        //显示包裹道具
        List<PlayGoods> playGoodsList = playerState.PlayerAllGoods.Where(temp => temp.GoodsLocation == GoodsLocation.Package).ToList();
        ResetItems(playGoodsList);
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
        //卖出物品到商店
        if (SelectGoods == null)
            return;
        PlayGoods playGoods = SelectGoods.value as PlayGoods;
        if (playGoods == null)
            return;
        //计算物品的价格
        int sprice = GoodsSprice(playGoods.GoodsInfo);
        playerState.Sprice += sprice / 2;
        iPlayerState.SpriteChanged = true;//设置金钱发生变化

        EnumGoodsType baseGoodsType = playGoods.GoodsInfo.EnumGoodsType;
        //如果是药品或道具等消耗品
        if (baseGoodsType > EnumGoodsType.Item && baseGoodsType < EnumGoodsType.SpecialItem)
        {
            //商人自身是否有该消耗品
            PlayGoods businessman_playGoods = businessman.BaseList.FirstOrDefault(temp => temp.GoodsInfo.EnumGoodsType == baseGoodsType);
            //商人回收道具是否有该消耗品
            if (businessman_playGoods == null)
            {
                businessman_playGoods = businessman.SellPropsList.FirstOrDefault(temp => temp.GoodsInfo.EnumGoodsType == baseGoodsType);
            }
            //如果还没有则添加到回收栏
            if (businessman_playGoods == null)
            {
                businessman_playGoods = new PlayGoods(playGoods.ID, playGoods.GoodsInfo, GoodsLocation.None);
                businessman.SellPropsList.Add(businessman_playGoods);
            }
            businessman_playGoods.Count++;
            playGoods.Count--;
            //如果当前数量为0则移除
            if (playGoods.Count <= 0)
            {
                playerState.PlayerAllGoods.Remove(playGoods);
            }
        }
        else
        {
            //移除玩家身上的道具
            playGoods.GoodsLocation = GoodsLocation.None;
            playerState.PlayerAllGoods.Remove(playGoods);
            //将道具转移到商人的回收栏
            businessman.SellPropsList.Add(playGoods);
            //显示列表减少
            RemoveItemShow(playGoods);
        }
    }

    /// <summary>
    /// 通过计算的价格返回应该显示的话(出售价格或购买价格等)
    /// </summary>
    /// <param name="sprice"></param>
    /// <returns></returns>
    protected override string ShowSpriceActionValue(int sprice)
    {
        return "出售价格:" + (sprice / 2);
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
