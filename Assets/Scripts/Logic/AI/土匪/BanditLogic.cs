using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 土匪的逻辑脚本(用于接收消息并处理)
/// </summary>
public class BanditLogic : MonsterBaseLogic
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void PlayerAnimEnterLeaveState_AnimEnterLeaveHandle(string actionTag, bool enter)
    {
        base.PlayerAnimEnterLeaveState_AnimEnterLeaveHandle(actionTag, enter);
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
