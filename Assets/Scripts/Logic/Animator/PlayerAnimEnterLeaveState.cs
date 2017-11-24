using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家动画进入与离开状态
/// </summary>
public class PlayerAnimEnterLeaveState : StateMachineBehaviour
{
    /// <summary>
    /// 状态名
    /// </summary>
    [SerializeField]
    private string StateName;

    /// <summary>
    /// 动画进入或离开事件
    /// </summary>
    public event Action<string, bool> AnimEnterLeaveHandle;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (AnimEnterLeaveHandle != null)
            AnimEnterLeaveHandle(StateName, true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (AnimEnterLeaveHandle != null)
            AnimEnterLeaveHandle(StateName, false);
    }
}