using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家物理动作的速度
/// </summary>
public class PlayerPhycisActionSpeed : StateMachineBehaviour
{
    /// <summary>
    /// 动画类型对应速度集合
    /// </summary>
    [SerializeField]
    private List<WeaponTypeToSpeed> weaponTypeToSpeedList;
    /// <summary>
    /// 武器类型字段名
    /// </summary>
    [SerializeField]
    private string weaponTypeFieldName = "WeaponType";

    /// <summary>
    /// 临时的速度 
    /// </summary>
    private float tempSpeed = 1;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int weaponType = (int)animator.GetFloat(weaponTypeFieldName);
        EnumPlayerWeaponType enumPlayerWeaponType = (EnumPlayerWeaponType)weaponType;
        WeaponTypeToSpeed weaponTypeToSpeed = weaponTypeToSpeedList.Find(temp => temp.weaponType == enumPlayerWeaponType);
        if (weaponTypeToSpeed != null)
        {
            tempSpeed = weaponTypeToSpeed.speed;
            animator.speed = weaponTypeToSpeed.speed * PlayerAnimSpeed.speedRate;

        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (tempSpeed == 0)
            tempSpeed = 1;
        animator.speed = tempSpeed * PlayerAnimSpeed.speedRate;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = PlayerAnimSpeed.speedRate;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}


}


