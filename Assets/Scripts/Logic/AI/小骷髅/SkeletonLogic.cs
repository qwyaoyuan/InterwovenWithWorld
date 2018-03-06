using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小骷髅的逻辑脚本(用于接收消息并处理)
/// </summary>
public class SkeletonLogic : MonsterBaseLogic
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
