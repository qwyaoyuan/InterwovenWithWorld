using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家动画速度 
/// 只是用来设置全局速度倍率数值的,不会具体的修改动画速度
/// </summary>
public class PlayerAnimSpeed : StateMachineBehaviour
{

    /// <summary>
    /// 速度比率
    /// </summary>
    public static float speedRate = 1;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = PlayerAnimSpeed.speedRate;
    }
}
