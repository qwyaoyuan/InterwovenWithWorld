using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 商店中显示自身剩余金钱
/// </summary>
public class UIShopSelfSprice : MonoBehaviour
{
    /// <summary>
    /// 显示金币的文本框 
    /// </summary>
    public Text spriceText;

    /// <summary>
    /// 玩家状态
    /// </summary>
    PlayerState playerState ;

    private void OnEnable()
    {
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        GameState.Instance.Registor<IPlayerState>(IPlayerState_Changed);
        spriceText.text = playerState.Sprice.ToString();
    }

    private void IPlayerState_Changed(IPlayerState arg1, string arg2)
    {
        if (string.Equals(arg2, GameState.Instance.GetFieldName<IPlayerState, bool>(temp => temp.SpriteChanged)))
        {
            spriceText.text = playerState.Sprice.ToString();
        }
    }

    private void OnDisable()
    {
        GameState.Instance.UnRegistor<IPlayerState>(IPlayerState_Changed);
    }
}

