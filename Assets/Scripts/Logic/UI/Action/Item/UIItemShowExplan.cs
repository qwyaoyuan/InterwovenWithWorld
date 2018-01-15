using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 道具界面的右侧显示详细信息栏
/// </summary>
public class UIItemShowExplan : UIFocus, IUIItemSelectGoods
{
    /// <summary>
    /// 标题(一般用于显示物品名字)
    /// </summary>
    [SerializeField]
    private Text Text_Title;

    /// <summary>
    /// 显示面板
    /// </summary>
    [SerializeField]
    private RectTransform AutoPanel;

    /// <summary>
    /// 显示条目UI的预设提
    /// </summary>
    [SerializeField]
    private GameObject ItemPrefab;

    /// <summary>
    /// 显示的游戏对象集合
    /// </summary>
    List<GameObject> ItemExplanValueList;

    /// <summary>
    /// 玩家存档状态
    /// </summary>
    PlayerState playerState;

    /// <summary>
    /// 能力值对应说明字典
    /// </summary>
    List<KeyValuePair<EnumGoodsAbility, string>> goodsAbilityExplanList;

    private void Awake()
    {
        ItemExplanValueList = new List<GameObject>();
        goodsAbilityExplanList = new List<KeyValuePair<EnumGoodsAbility, string>>();
        FieldExplanAttribute.SetEnumExplanDic(goodsAbilityExplanList);
    }

    private void OnEnable()
    {
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
    }

    private void OnDisable()
    {
        ClearNowShow();
    }

    /// <summary>
    /// 清理当前显示
    /// </summary>
    private void ClearNowShow()
    {
        if (Text_Title)
            Text_Title.text = "";
        if (ItemExplanValueList != null)
        {
            foreach (GameObject itemExplanValue in ItemExplanValueList)
            {
                GameObject.Destroy(itemExplanValue);
            }
            ItemExplanValueList.Clear();
        }
    }

    /// <summary>
    /// 显示详细信息栏不需要选择道具,因此注册并没有什么卵用
    /// </summary>
    /// <param name="SelectGoodsIDAction"></param>
    public void RegistorSelectGoodsID(Action<int> SelectGoodsIDAction) { }

    /// <summary>
    /// 选择了指定ID的道具
    /// </summary>
    /// <param name="goodsID"></param>
    public void SelectID(int goodsID)
    {
        ClearNowShow();
        if (playerState != null && goodsID >= 0)
        {
            PlayGoods playGoods = playerState.PlayerAllGoods.FirstOrDefault(temp => temp.ID == goodsID);
            if (playGoods != null)
            {
                if (Text_Title)
                    Text_Title.text = playGoods.GoodsInfo.GoodsName;
                if (ItemPrefab && goodsAbilityExplanList != null && ItemExplanValueList!=null)
                {
                    List<GoodsAbility> goodsAbilityList = playGoods.GoodsInfo.goodsAbilities;
                    foreach (GoodsAbility goodsAbility in goodsAbilityList)
                    {
                        KeyValuePair<EnumGoodsAbility, string> KVP = goodsAbilityExplanList.FirstOrDefault(temp => temp.Key == goodsAbility.AbilibityKind);
                        if (string.IsNullOrEmpty(KVP.Value) || KVP.Key == EnumGoodsAbility.None)
                            continue;
                        GameObject createObj = GameObject.Instantiate(ItemPrefab);
                        createObj.transform.SetParent(AutoPanel);
                        createObj.GetComponent<Text>().text = KVP.Value + ":    " + goodsAbility.Value;
                        createObj.SetActive(true);
                        ItemExplanValueList.Add(createObj);
                    }
                }
            }
        }
    }
}
