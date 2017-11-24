using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色动画控制
/// </summary>
public class PlayerAnimControl : MonoBehaviour
{
    /// <summary>
    /// 角色的动画控制器
    /// </summary>
    public Animator playerAnimator;

    void Start()
    {
        if (!playerAnimator)
            playerAnimator = GetComponent<Animator>();
        //注册监听
        GameState.Instance.Registor<IAnimatorState>(CallBackAnimatorState);
        GameState.Instance.Registor<ISpecialState>(CallBackSpecialState);
    }

    /// <summary>
    /// 监听函数-动画状态发生变化
    /// </summary>
    /// <param name="iAnimatorState"></param>
    /// <param name="fieldName"></param>
    private void CallBackAnimatorState(IAnimatorState iAnimatorState, string fieldName)
    {
        if (!playerAnimator)
            return;
        //移动速度发生变化
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAnimatorState, float>(temp => temp.AnimatorMoveSpeed)))
        {
            playerAnimator.SetFloat("Speed", iAnimatorState.AnimatorMoveSpeed);
        }
        //移动方向发生变化
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAnimatorState, int>(temp => temp.MoveAnimatorVectorType)))
        {
            float forwardFloat = iAnimatorState.MoveAnimatorVectorType / 180f;
            playerAnimator.SetFloat("Forward", forwardFloat);
        }
        //魔法动画类型发生变化
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAnimatorState, EnumMagicAnimatorType>(temp => temp.MagicAnimatorType)))
        {
            switch (iAnimatorState.MagicAnimatorType)
            {
                case EnumMagicAnimatorType.Sing:
                    playerAnimator.SetBool("Sing", true);
                    playerAnimator.SetTrigger("Magic");
                    playerAnimator.SetTrigger("ChangeMode");
                    break;
                case EnumMagicAnimatorType.Shot:
                    playerAnimator.SetBool("Sing", false);
                    break;
                default:
                    playerAnimator.SetBool("Sing", false);
                    break;
            }
        }
    }

    /// <summary>
    /// 监听函数-特殊状态变化
    /// </summary>
    /// <param name="iSpecialState"></param>
    /// <param name="fieldName"></param>
    private void CallBackSpecialState(ISpecialState iSpecialState, string fieldName)
    {
        if (!playerAnimator)
            return;
        //眩晕状态发生变化
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Xuanyun)))
        {
            playerAnimator.SetBool("Dizzy", iSpecialState.Xuanyun.Time > 0);
        }
    }
}
