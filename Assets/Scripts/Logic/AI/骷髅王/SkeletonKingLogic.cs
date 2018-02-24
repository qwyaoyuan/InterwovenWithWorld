using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKingLogic : MonsterBaseLogic
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
