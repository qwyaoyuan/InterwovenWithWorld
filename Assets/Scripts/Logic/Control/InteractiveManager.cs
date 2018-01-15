using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 交互管理器
/// 负责角色与NPC以及物品进行的交互
/// </summary>
public class InteractiveManager : IInput
{
    /// <summary>
    /// 交互管理器私有静态对象
    /// </summary>
    private static InteractiveManager instance;
    /// <summary>
    /// 交互管理器的单例对象
    /// </summary>
    public static InteractiveManager Instance
    {
        get
        {
            if (instance == null) instance = new InteractiveManager();
            return instance;
        }
    }
    /// <summary>
    /// 交互管理器的私有构造函数
    /// </summary>
    private InteractiveManager()
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
    /// 交互状态
    /// </summary>
    IInteractiveState iInteractiveState;

    /// <summary>
    /// 初始化数据对象
    /// </summary>
    private void InitDataTarget()
    {
        keyContactData = DataCenter.Instance.GetEntity<KeyContactData>();
        iGameState = GameState.Instance.GetEntity<IGameState>();
        iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        iInteractiveState = GameState.Instance.GetEntity<IInteractiveState>();
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

    public void View(Vector2 view)
    {
      
    }

    public void KeyDown(int key)
    {
        if (keyContactData == null || iPlayerState == null || iInteractiveState == null)
            return;
        if (iGameState.GameRunType != EnumGameRunType.Unsafa &&
            iGameState.GameRunType != EnumGameRunType.Safe)
            return;
        switch (iPlayerState.KeyContactDataZone)
        {
            case EnumKeyContactDataZone.Normal:
                break;
            case EnumKeyContactDataZone.Collect:
                if (iPlayerState.TouchTargetStruct.ID > -1)
                {
                    iInteractiveState.ClickInteractiveStuffID = iPlayerState.TouchTargetStruct.ID;
                }
                break;
            case EnumKeyContactDataZone.Dialogue:
                if (iPlayerState.TouchTargetStruct.ID > -1)
                {
                    iInteractiveState.ClickInteractiveNPCID = iPlayerState.TouchTargetStruct.ID;
                }
                break;
        }
    }
}
