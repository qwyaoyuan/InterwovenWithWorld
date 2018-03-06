using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自动传送点
/// </summary>
public class TransportPointAction : MonoBehaviour
{

    /// <summary>
    /// 目标场景
    /// </summary>
    public string targetSceneName;

    /// <summary>
    /// 目标场景中的位置
    /// </summary>
    public Vector3 targetPostion;

    /// <summary>
    /// 当前是否可以检测
    /// </summary>
    bool canCheck = true;

    /// <summary>
    /// 检测进入的物体
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //角色进入到了里面并且传送镇是激活的
        if (other.gameObject.tag == "Player" && "Player" == LayerMask.LayerToName(other.gameObject.layer) && gameObject.activeSelf && canCheck)
        {
            canCheck = false;
            IGameState iGameState = GameState.Instance.GetEntity<IGameState>();
            iGameState.ChangedScene(targetSceneName, targetPostion, temp => canCheck = true);
        }
    }
}
