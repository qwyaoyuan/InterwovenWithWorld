using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 不管是买还是卖都要显示对应的属性,该脚本就是用来显示属性的
/// </summary>
public abstract class UIShopExplan : MonoBehaviour
{
    /// <summary>
    /// 显示选择对象描述文字的文本框
    /// </summary>
    public Text ExplanText;
    /// <summary>
    /// 显示选择对象卖出或买入价格的文本框
    /// </summary>
    public Text ActionText;
    /// <summary>
    /// 确认取消按钮的面板
    /// </summary>
    public UIShopTip uiShowTip;
    /// <summary>
    /// 道具集合对象
    /// </summary>
    public UIList ItemUIList;

    /// <summary>
    /// 当前选择的物品条目
    /// </summary>
    private UIListItem _selectGoods;

    /// <summary>
    ///  物品的基础以及可以附加的属性对象
    /// </summary>
    protected GoodsMetaInfoMations goodsMetaInfoMations;

    /// <summary>
    /// 玩家的存档状态 
    /// </summary>
    protected PlayerState playerState;

    /// <summary>
    /// 商人的状态
    /// </summary>
    protected BusinessmanStates businessmanStates;

    /// <summary>
    /// 交互状态
    /// </summary>
    protected IInteractiveState iInteractiveState;

    /// <summary>
    /// 游戏状态
    /// </summary>
    protected IGameState iGameState;

    /// <summary>
    /// npc的数据
    /// </summary>
    protected NPCData npcData;

    /// <summary>
    /// 玩家运行时状态
    /// </summary>
    protected IPlayerState iPlayerState;

    /// <summary>
    /// 当前打开的商人
    /// </summary>
    protected Businessman businessman;

    protected virtual void Awake()
    {
        ItemUIList.ItemClickHandle += ItemUIList_ItemClickHandle;
    }

    protected virtual void Init()
    {
        goodsMetaInfoMations = DataCenter.Instance.GetMetaData<GoodsMetaInfoMations>();
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        businessmanStates = DataCenter.Instance.GetEntity<BusinessmanStates>();
        npcData = DataCenter.Instance.GetMetaData<NPCData>();
        iGameState = GameState.Instance.GetEntity<IGameState>();
        iInteractiveState = GameState.Instance.GetEntity<IInteractiveState>();
        iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        uiShowTip.gameObject.SetActive(false);
    }

    /// <summary>
    /// 更新商人的道具
    /// </summary>
    protected void UpdateBusinessman()
    {
        //获取当前商人
        businessman = businessmanStates.BusinessmanList.FirstOrDefault(temp => temp.BusinessmanID == iInteractiveState.ClickInteractiveNPCID && string.Equals(iGameState.SceneName, temp.BusinessmanScene));
        NPCDataInfo npcDataInfo = npcData.GetNPCDataInfo(iGameState.SceneName, iInteractiveState.ClickInteractiveNPCID);
        if (businessman == null && npcDataInfo != null)
        {
            businessman = new Businessman();
            businessman.BusinessManDataInfo = BusinessmanDataInfo.DeSerializeNow<BusinessmanDataInfo>(npcDataInfo.OtherValue);
            businessman.BusinessmanID = iInteractiveState.ClickInteractiveNPCID;
            businessman.BusinessmanScene = iGameState.SceneName;
            businessmanStates.BusinessmanList.Add(businessman);
        }
        if (businessman != null)
        {
            #region 刷新商品
            //如果商人的基础道具不足则刷新
            EnumGoodsType[] baseGoodsTypes = businessman.BusinessManDataInfo.GoodsDic.Select(temp => temp.Key).ToArray();//商人基础的物品
            foreach (EnumGoodsType baseGoodsType in baseGoodsTypes)
            {
                //如果不存在该物品则添加
                if (businessman.BaseList.FirstOrDefault(temp => temp.GoodsInfo.EnumGoodsType == baseGoodsType) == null)
                {
                    Goods addGoods = goodsMetaInfoMations[baseGoodsType].Clone(true);//获取需要添加物品的原始属性
                    BusinessmanDataInfo.GoodsDataInfoInner mustAddDataInfoInner = businessman.BusinessManDataInfo.GoodsDic[baseGoodsType];
                    EnumQualityType qualityTypeMin = mustAddDataInfoInner.MinQualityType;
                    EnumQualityType qualityTypeMax = mustAddDataInfoInner.MaxQualityType;
                    int count = mustAddDataInfoInner.Count;
                    //如果是道具或炼金物品则可以叠加,但是没有品质
                    if (baseGoodsType > EnumGoodsType.Item && baseGoodsType < EnumGoodsType.SpecialItem)
                    {
                        //尝试从玩家道具中获取id,如果没有则新建一个(这个id并没有什么卵用)
                        int addGoodsID = playerState.PlayerAllGoods.Where(temp => temp.GoodsInfo.EnumGoodsType == baseGoodsType).Select(temp => temp.ID).FirstOrDefault();
                        if (addGoodsID == 0)
                            addGoodsID = NowTimeToID.GetNowID(DataCenter.Instance.GetEntity<GameRunnedState>());
                        PlayGoods playGoods = new PlayGoods(addGoodsID, addGoods, GoodsLocation.None);
                        playGoods.QualityType = EnumQualityType.White;
                        playGoods.Count = count;
                        businessman.BaseList.Add(playGoods);
                    }
                    //如果不是道具和炼金物品则不可以叠加,但是存在品质
                    else
                    {
                        int addGoodsID = NowTimeToID.GetNowID(DataCenter.Instance.GetEntity<GameRunnedState>());
                        EnumQualityType qualityType = (EnumQualityType)UnityEngine.Random.Range((int)qualityTypeMin, (int)qualityTypeMax + 1);
                        // 根据品质设置属性
                        if (qualityType != EnumQualityType.Red)//如果是唯一的则不用改变属性
                        {
                            //将所有属性随机
                            addGoods.goodsAbilities.ForEach(temp =>
                            {
                                float min = temp.Value * 0.8f;
                                float max = temp.Value * 1.2f;
                                temp.Value = (int)UnityEngine.Random.Range(min, max);
                            });
                            //取出用于分割固定属性以及随机属性的条目
                            GoodsAbility goodsAbility_nothing = addGoods.goodsAbilities.FirstOrDefault(temp => temp.AbilibityKind == EnumGoodsAbility.Nothing);
                            int index = -1;
                            if (goodsAbility_nothing != null)
                                index = addGoods.goodsAbilities.IndexOf(goodsAbility_nothing);
                            //存在分割属性
                            if (index > -1 && index + 1 < addGoods.goodsAbilities.Count)
                            {
                                //取出分割项后的可变项
                                List<EnumGoodsAbility> randomAbilitys = Enumerable.Range(index + 1, addGoods.goodsAbilities.Count - index - 1).
                                    Select(temp => addGoods.goodsAbilities[temp]).Select(temp => temp.AbilibityKind).ToList();
                                int saveAbilityCount = (int)qualityType;//可保留的属性
                                while (randomAbilitys.Count > saveAbilityCount)
                                {
                                    int removeIndex = UnityEngine.Random.Range(0, randomAbilitys.Count);
                                    EnumGoodsAbility removeGoodsAbility = randomAbilitys[removeIndex];
                                    addGoods.goodsAbilities.RemoveAll(temp => temp.AbilibityKind == removeGoodsAbility);
                                    randomAbilitys.RemoveAt(removeIndex);
                                }
                            }
                        }
                        addGoods.goodsAbilities.RemoveAll(temp => temp.AbilibityKind == EnumGoodsAbility.Nothing);
                        //--------------------
                        PlayGoods playGoods = new PlayGoods(addGoodsID, addGoods, GoodsLocation.None);
                        playGoods.QualityType = qualityType;
                        playGoods.Count = 1;
                        businessman.BaseList.Add(playGoods);
                    }
                }
            }
            #endregion
        }
    }

    protected virtual void OnEnable()
    {
        Init();
    }

    protected virtual void OnDisable()
    { }

    /// <summary>
    /// 重新设置显示物品的集合
    /// </summary>
    /// <param name="allPlayGoodses"></param>
    protected virtual void ResetItems(List<PlayGoods> allPlayGoodses)
    {
        ItemUIList.Init();
        foreach (PlayGoods playGoods in allPlayGoodses)
        {
            string showExplan = playGoods.GoodsInfo.GoodsName + "    X" + playGoods.Count;
            UIListItem uiListImte = ItemUIList.NewItem();
            uiListImte.value = playGoods;
            uiListImte.childText.text = showExplan;
        }
        SelectGoods = ItemUIList.FirstShowItem();
        if (SelectGoods)
        {
            SelectGoods.childImage.enabled = true;
            SelectGoods.childImage.gameObject.SetActive(true);
        }
        ItemUIList.ShowItem(SelectGoods);
        ItemUIList.UpdateUI();
    }

    /// <summary>
    /// 修改物品的显示(一般用于药品的数量变化)
    /// </summary>
    /// <param name="playGoods"></param>
    protected virtual void ChangeItemsShow(PlayGoods playGoods)
    {
        UIListItem[] uiListItems = ItemUIList.GetAllImtes();
        UIListItem thisUIListItem = uiListItems.FirstOrDefault(temp =>
        {
            PlayGoods thisPlayGoods = temp.value as PlayGoods;
            if (thisPlayGoods != null && thisPlayGoods == playGoods)
                return true;
            return false;
        });
        if (thisUIListItem != null)
        {
            thisUIListItem.childText.text = playGoods.GoodsInfo.GoodsName + "    X" + playGoods.Count;
        }
    }

    /// <summary>
    /// 移除物品的显示
    /// </summary>
    /// <param name="playGoods"></param>
    protected virtual void RemoveItemShow(PlayGoods playGoods)
    {
        UIListItem[] uiListItems = ItemUIList.GetAllImtes();
        UIListItem thisUIListItem = uiListItems.FirstOrDefault(temp =>
        {
            PlayGoods thisPlayGoods = temp.value as PlayGoods;
            if (thisPlayGoods != null && thisPlayGoods == playGoods)
                return true;
            return false;
        });
        if (thisUIListItem != null)
        {
            ItemUIList.RemoveItem(thisUIListItem);
            ItemUIList.UpdateUI();
            SelectGoods = ItemUIList.FirstShowItem();
            if (SelectGoods != null)
            {
                ItemUIList.ShowItem(SelectGoods);
                SelectGoods.childImage.enabled = true;
                SelectGoods.childImage.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 移动选择条目
    /// </summary>
    /// <param name="keyType"></param>
    protected virtual void MoveItemSelect(UIManager.KeyType keyType)
    {
        switch (keyType)
        {
            case UIManager.KeyType.UP:
            case UIManager.KeyType.DOWN:
                UIListItem[] uiListItems = ItemUIList.GetAllImtes();
                int index = uiListItems.ToList().IndexOf(SelectGoods);
                index += keyType == UIManager.KeyType.UP ? -1 : 1;
                if (index >= uiListItems.Length)
                    index = uiListItems.Length - 1;
                else if (index < 0)
                    index = 0;
                if (index < uiListItems.Length && index >= 0)
                {
                    UIListItem nextSelectGoods = uiListItems[index];
                    if (nextSelectGoods != null)
                    {
                        SelectGoods.childImage.enabled = false;
                        SelectGoods.childImage.gameObject.SetActive(false);
                        SelectGoods = nextSelectGoods;
                        SelectGoods.childImage.enabled = true;
                        SelectGoods.childImage.gameObject.SetActive(true);
                        ItemUIList.ShowItem(SelectGoods);
                        ItemUIList.UpdateUI();
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 当前选择的物品
    /// </summary>
    protected UIListItem SelectGoods
    {
        get { return _selectGoods; }
        set
        {
            _selectGoods = value;
            //显示在面板上
            ShowThisGoods();
        }
    }

    /// <summary>
    /// 将当前选额的对象显示在面板上
    /// </summary>
    private void ShowThisGoods()
    {
        if (_selectGoods == null)
        {
            ExplanText.text = "";
            ActionText.text = "";
            return;
        }
        //显示装备说明
        PlayGoods playGoods = SelectGoods.value as PlayGoods;
        ExplanText.text = GoodsExplan(playGoods.GoodsInfo);
        //显示状态出售或购买价格
        int sprice = GoodsSprice(playGoods.GoodsInfo);
        string actionStr = ShowSpriceActionValue(sprice);
        ActionText.text = actionStr;
    }

    /// <summary>
    /// 获取道具的说明
    /// </summary>
    /// <param name="goods"></param>
    /// <returns></returns>
    protected string GoodsExplan(Goods goods)
    {
        string explan = goods.GoodsName + "\r\n\r\n";
        explan += goods.Explain + "\r\n\r\n";
        foreach (GoodsAbility goodsAbility in goods.goodsAbilities)
        {
            explan += goodsAbility.Explain + ":" + goodsAbility.Value + "\r\n";
        }
        return explan;
    }

    /// <summary>
    /// 获取道具的价格
    /// </summary>
    /// <param name="goods"></param>
    /// <returns></returns>
    protected int GoodsSprice(Goods goods)
    {
        return goods.goodsAbilities.Count();
    }

    /// <summary>
    /// 点击条目事件(将会重新设置选择项)
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    protected virtual void ItemUIList_ItemClickHandle(UIList.ItemClickMouseType arg1, UIListItem arg2)
    {
        //设置当前的选择
        SelectGoods.childImage.enabled = false;
        SelectGoods.childImage.gameObject.SetActive(false);
        SelectGoods = arg2;
        SelectGoods.childImage.enabled = true;
        SelectGoods.childImage.gameObject.SetActive(true);
        ItemUIList.ShowItem(SelectGoods);
        ItemUIList.UpdateUI();
    }


    /// <summary>
    /// 通过计算的价格返回应该显示的话(出售价格或购买价格等)
    /// </summary>
    /// <param name="sprice"></param>
    /// <returns></returns>
    protected abstract string ShowSpriceActionValue(int sprice);

    /// <summary>
    /// 当前状态是否可以退出
    /// </summary>
    /// <returns></returns>
    public abstract bool CanExit();

}
