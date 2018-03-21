using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 道具相关的管理器(吃药)
/// </summary>
public class GoodsItemManager : IInput
{
    /// <summary>
    /// 道具管理器私有静态对象
    /// </summary>
    private static GoodsItemManager instance;

    /// <summary>
    /// 道具管理器的单利对象
    /// </summary>
    public static GoodsItemManager Instance
    {
        get
        {
            if (instance == null) instance = new GoodsItemManager();
            return instance;
        }
    }

    private GoodsItemManager()
    {
        GameState.Instance.Registor<IGameState>(GameStateChanged);
    }

    /// <summary>
    /// 游戏状态发生变化
    /// </summary>
    /// <param name="iGameState"></param>
    /// <param name="fieldName"></param>
    private void GameStateChanged(IGameState iGameState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<GameState, Action>(temp => temp.LoadArchive)))
        {
            InitDataTarget();
        }
    }

    /// <summary>
    /// 按键对应数据对象
    /// </summary>
    KeyContactData keyContactData;

    /// <summary>
    /// 游戏状态
    /// </summary>
    IGameState iGameState;

    /// <summary>
    /// 角色状态
    /// </summary>
    IPlayerState iPlayerState;

    /// <summary>
    /// 角色存档对象
    /// </summary>
    PlayerState playerState;

    /// <summary>
    /// 初始化数据对象
    /// </summary>
    private void InitDataTarget()
    {
        keyContactData = DataCenter.Instance.GetEntity<KeyContactData>();
        iGameState = GameState.Instance.GetEntity<IGameState>();
        iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
    }

    public void KeyDown(int key)
    {
        if (keyContactData == null || iPlayerState == null || iGameState == null || playerState == null)
            return;
        if (iGameState.GameRunType != EnumGameRunType.Unsafa && iGameState.GameRunType != EnumGameRunType.Safe)
            return;
        KeyContactStruct[] keyContactStructs = keyContactData.GetKeyContactStruct(key, temp => temp.keyContactType == EnumKeyContactType.Prap, iPlayerState.KeyContactDataZone);
        if (keyContactStructs.Length == 0)
            return;
        KeyContactStruct keyContactStruct = keyContactStructs[0];
        PlayGoods playGoods = playerState.PlayerAllGoods.Where(temp => temp.ID == keyContactStruct.id).FirstOrDefault();
        if (playGoods == null)
            return;
        //暂时这么写
        if (playGoods.GoodsInfo.EnumGoodsType == EnumGoodsType.HFYJ)
        {
            iPlayerState.EatMedicine(playGoods.ID);
        }
    }

    public void KeyPress(int key)
    {
   
    }

    public void KeyUp(int key)
    {
       
    }

    public void Move(Vector2 forward)
    {
   
    }

    public void SetStep(int step)
    {
  
    }

    public void Update()
    {
   
    }

    public void View(Vector2 view)
    {
    }
}
