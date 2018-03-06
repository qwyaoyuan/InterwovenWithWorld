using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// 道具界面的左侧存储栏
/// </summary>
public class UIFocusItemDeposit : UIFocus, IUIItemSelectGoods
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

    /// <summary>
    /// 玩家对象
    /// </summary>
    PlayerState playerState;

    /// <summary>
    /// 玩家的运行时状态
    /// </summary>
    IPlayerState iPlayerStateRun;

    /// <summary>
    /// 选择物体后的回调
    /// </summary>
    Action<int> SelectGoodsIDAction;

    private void Awake()
    {
        //获取List控件
        uiDepostiList = GetComponent<UIList>();
        uiDepostiList.ItemClickHandle += UiDepostiList_ItemClickHandle;
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
        //设置选择了该物体
        PlayGoods playGoods = (PlayGoods)focusUIListItem.value;
        if (SelectGoodsIDAction != null)
            SelectGoodsIDAction(playGoods.ID);
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
        GameState.Instance.Registor<IPlayerState>(IPlayerStateChanged);
        iPlayerStateRun = GameState.Instance.GetEntity<IPlayerState>();
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        //读取数据并初始化控件
        //给List控件重新填充数据
        uiDepostiList.Init();
        PlayGoods[] playGoodsArray = playerState.PlayerAllGoods.Where(temp => temp.GoodsLocation == GoodsLocation.Package).ToArray();//玩家所有包括中的物品
        foreach (PlayGoods playGoods in playGoodsArray)
        {
            UIListItem uiListItem = uiDepostiList.NewItem();
            uiListItem.childText.text = playGoods.GoodsInfo.GoodsName;
            uiListItem.value = playGoods;
        }
        uiDepostiList.UpdateUI();
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
        GameState.Instance.UnRegistor<IPlayerState>(IPlayerStateChanged);
    }

    /// <summary>
    /// 设置焦点
    /// 已经在OnEnable处理过第一个焦点获取了，此时不需要重设
    /// </summary>
    public override void SetForcus()
    {
        focused = true;
        //如果当前没有选择焦点的选项则设置一个 
        if (!focusUIListItem && uiDepostiList)
            focusUIListItem = uiDepostiList.FirstShowItem();
        if (focusUIListItem && uiDepostiList)
        {
            uiDepostiList.ShowItem(focusUIListItem);
            if (focusUIListItem.childImage)
                focusUIListItem.childImage.enabled = true;
            //设置选择了该物体
            PlayGoods playGoods = (PlayGoods)focusUIListItem.value;
            if (SelectGoodsIDAction != null)
                SelectGoodsIDAction(playGoods.ID);
        }
        else//否则就没有选择
        {
            if (SelectGoodsIDAction != null)
                SelectGoodsIDAction(-1);
        }
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
    /// 玩家状态发生变化
    /// </summary>
    /// <param name="iPlayerState"></param>
    /// <param name="name"></param>
    private void IPlayerStateChanged(IPlayerState iPlayerState, string name)
    {
        if (name == GameState.Instance.GetFieldName<IPlayerState, bool>(temp => temp.EquipmentChanged))//主要用于装备发生变化时修改集合
        {
            UIListItem[] nowUIListItems = uiDepostiList.GetAllImtes();
            List<UIListItem> mustDeleteItems = new List<UIListItem>();
            foreach (UIListItem item in nowUIListItems)
            {
                PlayGoods playGoods = item.value as PlayGoods;
                if (playGoods.GoodsLocation == GoodsLocation.Wearing)
                    mustDeleteItems.Add(item);
            }
            PlayGoods[] packageGoods = playerState.PlayerAllGoods.Where(temp => temp.GoodsLocation == GoodsLocation.Package).ToArray();
            List<PlayGoods> mustAddPlayGoods = new List<PlayGoods>();
            PlayGoods[] lastPackageGoods = nowUIListItems.Select(temp => temp.value as PlayGoods).ToArray();
            foreach (PlayGoods item in packageGoods)
            {
                if (!lastPackageGoods.Contains(item))
                {
                    mustAddPlayGoods.Add(item);
                }
            }
            //删除需要删除的条目
            foreach (UIListItem item in mustDeleteItems)
            {
                uiDepostiList.RemoveItem(item);
            }
            //添加需要添加的条目
            foreach (PlayGoods item in mustAddPlayGoods)
            {
                UIListItem uiListItem = uiDepostiList.NewItem();
                uiListItem.childText.text = item.GoodsInfo.GoodsName;
                uiListItem.value = item;
                uiDepostiList.ShowItem(uiListItem);
            }
            uiDepostiList.UpdateUI();
        }
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
        if (focusUIListItem == null)
            focusUIListItem = uiDepostiList.FirstShowItem();
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
            //设置选择了该物体
            PlayGoods playGoods = (PlayGoods)focusUIListItem.value;
            if (SelectGoodsIDAction != null)
                SelectGoodsIDAction(playGoods.ID);
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
        if (focused)
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
            //物品对象
            PlayGoods playGoods = (PlayGoods)focusUIListItem.value;
            EnumGoodsType enumGoodsType = playGoods.GoodsInfo.EnumGoodsType;
            int goodsTypeInt = (int)enumGoodsType;
            if (GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.Equipment))//如果是装备类型则替换装备
            {
                //如果是副手武器
                if (GoodsStaticTools.IsLeftOneHandedWeapon(enumGoodsType))
                {
                    //需要判断当前是否佩戴了双手武器以及副手武器(左手武器),如果佩戴了则需要卸载双手武器
                    PlayGoods[] twoHandedWeapons = playerState.PlayerAllGoods.Where(
                        temp => temp.GoodsLocation == GoodsLocation.Wearing
                        && (GoodsStaticTools.IsTwoHandedWeapon(temp.GoodsInfo.EnumGoodsType) ||
                        (temp.leftRightArms != null && temp.leftRightArms.Value == false))).ToArray();
                    foreach (PlayGoods _playGoods in twoHandedWeapons)
                    {
                        _playGoods.leftRightArms = null;
                        _playGoods.GoodsLocation = GoodsLocation.Package;
                    }
                    playGoods.leftRightArms = false;
                }
                //如果是双手武器 
                else if (GoodsStaticTools.IsTwoHandedWeapon(enumGoodsType))
                {
                    //需要判断当前是否佩戴了副手武器以及主手武器(右手武器),如果佩戴了则需要卸载副手武器 
                    PlayGoods[] oneHandedWeapons = playerState.PlayerAllGoods.Where(
                        temp => temp.GoodsLocation == GoodsLocation.Wearing &&
                        (GoodsStaticTools.IsLeftOneHandedWeapon(temp.GoodsInfo.EnumGoodsType) ||
                        (temp.leftRightArms != null && temp.leftRightArms.Value == true))).ToArray();
                    foreach (PlayGoods _playGoods in oneHandedWeapons)
                    {
                        _playGoods.leftRightArms = null;
                        _playGoods.GoodsLocation = GoodsLocation.Package;
                    }
                    playGoods.leftRightArms = true;
                }
                //如果是饰品
                else if (GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.Ornaments))
                {
                    EnumGoodsType? ornamentsType = GoodsStaticTools.GetParentGoodsType(playGoods.GoodsInfo.EnumGoodsType);//从基础的饰品类型向上跳到饰品的分类类型(项链 戒指 护身符 勋章)
                    if (ornamentsType != null && GoodsStaticTools.IsChildGoodsNode(ornamentsType.Value, EnumGoodsType.Ornaments))
                    {
                        PlayGoods[] ornaments = playerState.PlayerAllGoods.Where(
                            temp => temp.GoodsLocation == GoodsLocation.Wearing && GoodsStaticTools.IsChildGoodsNode(temp.GoodsInfo.EnumGoodsType, ornamentsType.Value)).ToArray();
                        foreach (PlayGoods _playGoods in ornaments)
                        {
                            _playGoods.GoodsLocation = GoodsLocation.Package;
                        }
                    }
                }
                //如果是武器(这里是单手武器,双手武器以及副手在上面已经判断过了 )
                else if (GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.Arms))
                {
                    //需要卸载所有的右手装备
                    PlayGoods[] rightHandedWeapons = playerState.PlayerAllGoods.Where(
                        temp => temp.GoodsLocation == GoodsLocation.Wearing && temp.leftRightArms != null && temp.leftRightArms.Value == true).ToArray();
                    foreach (PlayGoods _playGoods in rightHandedWeapons)
                    {
                        _playGoods.leftRightArms = null;
                        _playGoods.GoodsLocation = GoodsLocation.Package;
                    }
                    playGoods.leftRightArms = true;
                }
                //如果是其他装备(防具等)
                else
                {
                    EnumGoodsType? armorType = GoodsStaticTools.GetParentGoodsType(playGoods.GoodsInfo.EnumGoodsType, 2);//从基础的防具类型向上跳到防具的大类(头盔大类 盔甲类 鞋子类)
                    if (armorType != null && GoodsStaticTools.IsChildGoodsNode(armorType.Value, EnumGoodsType.Equipment))
                    {
                        PlayGoods[] ammors = playerState.PlayerAllGoods.Where(
                            temp => temp.GoodsLocation == GoodsLocation.Wearing && GoodsStaticTools.IsChildGoodsNode(temp.GoodsInfo.EnumGoodsType, armorType.Value)).ToArray();
                        foreach (PlayGoods _playGoods in ammors)
                        {
                            _playGoods.GoodsLocation = GoodsLocation.Package;
                        }
                    }
                }
                playGoods.GoodsLocation = GoodsLocation.Wearing;
                iPlayerStateRun.EquipmentChanged = true;
            }
            else if (GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.Elixir))//如果是炼金药剂类则直接服用
            {
                Debug.Log("物品栏吃药功能暂时未实现");
            }
            else if (GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.Item))//如果是道具
            {
                if (enumGoodsType == EnumGoodsType.MagicScroll)//此处的魔法卷轴有一个任务的特殊检测
                {
                    INowTaskState iNowTaskState = GameState.Instance.GetEntity<INowTaskState>();
                    iNowTaskState.CheckNowTask(EnumCheckTaskType.Special, (int)TaskMap.Enums.EnumTaskSpecialCheck.SummonsScrollMagic);
                }
                Debug.Log("道具功能暂未实现");
            }
        }
    }
}
