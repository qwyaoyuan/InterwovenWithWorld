using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// 角色装备显隐控制器
/// </summary>
public class PlayerEquipmentShowControl : MonoBehaviour
{
    /// <summary>
    /// (安全区)数据数组
    /// </summary>
    public EquipmentTypeToObj[] safeDatas;

    /// <summary>
    /// 数据数组(非安全区)
    /// </summary>
    public EquipmentTypeToObj[] unsafeDatas;

    /// <summary>
    /// 游戏状态
    /// </summary>
    IGameState iGameState;
    /// <summary>
    /// 玩家状态
    /// </summary>
    IPlayerState iPlayerState;

    /// <summary>
    /// 玩家的存档
    /// </summary>
    PlayerState playerState;

    private void Start()
    {
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        iGameState = GameState.Instance.GetEntity<IGameState>();
        iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        GameState.Instance.Registor<IPlayerState>(PlayerStateChanged);
        GameState.Instance.Registor<IGameState>(GameStateChanged);
        PlayerStateChanged(iPlayerState, GameState.GetFieldNameStatic<IPlayerState, bool>(temp => temp.EquipmentChanged));
    }

    /// <summary>
    /// 游戏状态变化
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void GameStateChanged(IGameState iGameState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.GetFieldNameStatic<IGameState, EnumGameRunType>(temp => temp.GameRunType)))
        {
            UpdateShow();
        }
    }

    /// <summary>
    /// 玩家状态变化
    /// </summary>
    /// <param name="iPlayerState"></param>
    /// <param name="fieldName"></param>
    private void PlayerStateChanged(IPlayerState iPlayerState, string fieldName)
    {
        //装备发生变化
        if (string.Equals(fieldName, GameState.GetFieldNameStatic<IPlayerState, bool>(temp => temp.EquipmentChanged)))
        {
            UpdateShow();
        }
    }

    /// <summary>
    /// 更新显示
    /// </summary>
    private void UpdateShow()
    {

        switch (iGameState.GameRunType)
        {
            case EnumGameRunType.Safe:
                SetDisableArray(unsafeDatas);
                SetEnableArray(safeDatas);
                break;
            case EnumGameRunType.Unsafa:
                SetDisableArray(safeDatas);
                SetEnableArray(unsafeDatas);
                break;
        }
    }

    /// <summary>
    /// 设置隐藏的数组 
    /// </summary>
    /// <param name="datas">对象数组</param>
    private void SetDisableArray(EquipmentTypeToObj[] datas)
    {
        foreach (EquipmentTypeToObj data in datas)
        {
            if (data.SelfObj)
                data.SelfObj.SetActive(false);
        }
    }

    /// <summary>
    /// 设置显示的数组
    /// </summary>
    /// <param name="datas"></param>
    private void SetEnableArray(EquipmentTypeToObj[] datas)
    {
        EnumGoodsType[] goodsTypeArray = playerState.PlayerAllGoods.Where(temp => temp.GoodsLocation == GoodsLocation.Wearing).Select(temp => temp.GoodsInfo.EnumGoodsType).ToArray();
        foreach (EquipmentTypeToObj data in datas)
        {
            if (data.SelfObj)
                if (goodsTypeArray.Contains(data.GoodsType) || goodsTypeArray.Count(temp => GoodsStaticTools.IsChildGoodsNode(temp, data.GoodsType, false)) > 0)
                {
                    data.SelfObj.SetActive(true);
                }
                else
                {

                    data.SelfObj.SetActive(false);
                }
        }
    }

    private void OnDestroy()
    {
        GameState.Instance.UnRegistor<IPlayerState>(PlayerStateChanged);
    }

    /// <summary>
    /// 装备类型对应的游戏对象
    /// </summary>
    [Serializable]
    public struct EquipmentTypeToObj
    {
        /// <summary>
        /// 道具类型
        /// </summary>
        [EnumRange((int)EnumGoodsType.Equipment, (int)EnumGoodsType.Ornaments)]
        public EnumGoodsType GoodsType;
        /// <summary>
        /// 该类型对应的游戏对象
        /// </summary>
        public GameObject SelfObj;
    }
}
