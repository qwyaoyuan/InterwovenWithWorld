using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 检测
/// </summary>
public class CheckPlayerState : MonoBehaviour
{

    /// <summary>
    /// 游戏运行状态(主要用于切换Unsafe和Safe)
    /// </summary>
    [Tooltip("游戏运行状态(主要用于切换Unsafe和Safe)")]
    public EnumGameRunType GameRunType;

    /// <summary>
    /// 检测的层 
    /// </summary>
    public LayerMask CheckLayer;

    /// <summary>
    /// 触发器
    /// </summary>
    Collider collider;

    /// <summary>
    /// 游戏状态对象
    /// </summary>
    IGameState iGameState;

    private void Start()
    {
        collider = GetComponent<Collider>();
        collider.enabled = true;
        iGameState = GameState.Instance.GetEntity<IGameState>();
    }

    private void OpenCheck()
    {
        collider.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        int layer = (int)Mathf.Pow(2, other.gameObject.layer);
        int checkLayer = CheckLayer.value;
        int resultLayer = layer | checkLayer;
        if (checkLayer == resultLayer)
        {
            Invoke("OpenCheck", 5);
            collider.enabled = false;
            if (iGameState.GameRunType != GameRunType && (iGameState.GameRunType == EnumGameRunType.Unsafa || iGameState.GameRunType == EnumGameRunType.Safe))
            {
                iGameState.GameRunType = GameRunType;
            }
        }
    }
}
