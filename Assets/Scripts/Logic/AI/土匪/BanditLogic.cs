using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 土匪的逻辑脚本(用于接收消息并处理)
/// </summary>
public class BanditLogic : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 攻击函数
    /// </summary>
    /// <param name="attackAIDataStruct"></param>
    public void AttackMessage(AttackAIDataStruct attackAIDataStruct)
    {
        animator.SetTrigger("Attack" + attackAIDataStruct.AttackID);
    }
}
