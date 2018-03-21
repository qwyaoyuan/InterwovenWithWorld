using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鼠人的逻辑脚本
/// </summary>
public class SkavenLogic : MonsterBaseLogic
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void PlayerAnimEnterLeaveState_AnimEnterLeaveHandle(string actionTag, bool enter)
    {
        base.PlayerAnimEnterLeaveState_AnimEnterLeaveHandle(actionTag, enter);
    }
}
