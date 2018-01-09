using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// 动画对象
    /// </summary>
    IAnimatorState iAnimatorState;
    /// <summary>
    /// 特殊状态
    /// </summary>
    ISpecialState iSpecialState;

    void Start()
    {
        iAnimatorState = GameState.Instance.GetEntity<IAnimatorState>();
        iSpecialState = GameState.Instance.GetEntity<ISpecialState>();
        if (!playerAnimator)
            playerAnimator = GetComponent<Animator>();
        if (playerAnimator)
        {
            PlayerAnimEnterLeaveState[] playerAnimEnterLeaveStates = playerAnimator.GetBehaviours<PlayerAnimEnterLeaveState>();
            foreach (PlayerAnimEnterLeaveState playerAnimEnumterLeaveState in playerAnimEnterLeaveStates)
            {
                playerAnimEnumterLeaveState.AnimEnterLeaveHandle += PlayerAnimEnumterLeaveState_AnimEnterLeaveHandle;
            }
        }
        //注册监听
        GameState.Instance.Registor<IPlayerState>(CallBackIPlayerStateState);
        GameState.Instance.Registor<IAnimatorState>(CallBackIAnimatorState);
        GameState.Instance.Registor<ISpecialState>(CallBackISpecialState);
    }

    /// <summary>
    /// 离开物理普通攻击函数
    /// </summary>
    IEnumerator LeavePhysicInvoke()
    {
        yield return new WaitForSeconds(0.15f);
        if (iAnimatorState != null)
        {
            iAnimatorState.IsPhycisActionState = false;
            //当从物理动作(普通攻击)中退出时,设置其阶段为0
            iAnimatorState.PhycisActionNowType = 0;
        }
    }

    /// <summary>
    /// 离开技能动作函数
    /// </summary>
    /// <returns></returns>
    IEnumerator LeaveSkillInvoke()
    {
        yield return new WaitForSeconds(0.3f);
        if (iAnimatorState != null)
        {
            iAnimatorState.IsSkillState = false;
        }
    }

    /// <summary>
    /// 离开魔法动作函数
    /// </summary>
    /// <returns></returns>
    IEnumerator LeaveMagicInvoke()
    {
        yield return new WaitForSeconds(0.3f);
        if (iAnimatorState != null)
        {
            iAnimatorState.IsMagicActionState = false;
        }
    }

    /// <summary>
    /// 动画进入或离开某状态
    /// </summary>
    /// <param name="name">动画名(手动输入的)</param>
    /// <param name="state">进入或离开状态</param>
    private void PlayerAnimEnumterLeaveState_AnimEnterLeaveHandle(string name, bool state)
    {
        switch (name)
        {
            case "Dizzy"://如果陷入眩晕则重置普通攻击状态
                if (state)
                {
                    iAnimatorState.PhycisActionNowType = 0;
                    iAnimatorState.IsPhycisActionState = false;
                    iAnimatorState.IsSkillState = false;
                    iAnimatorState.IsMagicActionState = false;
                    playerAnimator.SetLayerWeight(1, 0f);
                }
                break;
            case "SkillIn":
                StopCoroutine("LeaveSkillInvoke");
                iAnimatorState.IsSkillState = true;
                break;
            case "SkillOut":
                if (state)
                    StartCoroutine("LeaveSkillInvoke");
                break;
            case "PhycisIn":
                StopCoroutine("LeavePhysicInvoke");
                iAnimatorState.IsPhycisActionState = true;
                break;
            case "PhycisOut":
                if (state)
                    StartCoroutine("LeavePhysicInvoke");
                break;
            case "MagicIn":
                StopCoroutine("LeaveMagicInvoke");
                iAnimatorState.IsMagicActionState = true;
                break;
            case "MagicOut":
                if (state)
                    StartCoroutine("LeaveMagicInvoke");
                break;
            //当动作进入攻击1 攻击2 攻击3时分别设置对应的物理动作(普通攻击)阶段为对应的阶段
            //当退出动作时设置动作为0(无攻击)
            case "PhycisType1":
                if (state)
                    iAnimatorState.PhycisActionNowType = 1;
                break;
            case "PhycisType2":
                if (state)
                    iAnimatorState.PhycisActionNowType = 2;
                break;
            case "PhycisType3":
                if (state)
                    iAnimatorState.PhycisActionNowType = 3;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 监听角色属性更改(主要是装备)
    /// </summary>
    /// <param name="iPlayerState"></param>
    /// <param name="fieldName"></param>
    private void CallBackIPlayerStateState(IPlayerState iPlayerState, string fieldName)
    {
        if (!playerAnimator)
            return;
        //角色装备发生变化
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IPlayerState, bool>(temp => temp.EquipmentChanged)))
        {
            PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
            PlayGoods playGoods = playerState.PlayerAllGoods.Where(temp => temp.GoodsLocation == GoodsLocation.Wearing && temp.leftRightArms != null && temp.leftRightArms.Value == false).First();
            if (playGoods != null)
            {
                EnumGoodsType enumGoodsType = playGoods.GoodsInfo.EnumGoodsType;
                //与1000相除求出剔除了具体类型后的上层一分类
                //然后与100求余计算出具体的武器分类
                //因为武器的分类是从10开始的因此减去10
                //动画的第一个分类留空为0表示空手,因此加1
                int num = (((int)enumGoodsType) / 1000) % 100 - 10 + 1;
                //如果是武器
                if (enumGoodsType > EnumGoodsType.Arms && enumGoodsType < EnumGoodsType.Arms + 100000)
                    playerAnimator.SetFloat("WeaponType", num);
                else //否则空手
                    playerAnimator.SetFloat("WeaponType", 0);
            }
            else
                playerAnimator.SetFloat("WeaponType", 0);//0表示空手
        }
    }

    /// <summary>
    /// 监听函数-动画状态发生变化
    /// </summary>
    /// <param name="iAnimatorState"></param>
    /// <param name="fieldName"></param>
    private void CallBackIAnimatorState(IAnimatorState iAnimatorState, string fieldName)
    {
        if (!playerAnimator)
            return;
        //移动速度发生变化
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAnimatorState, float>(temp => temp.AnimatorMoveSpeed)))
        {
            playerAnimator.SetFloat("Speed", iAnimatorState.AnimatorMoveSpeed);
        }
        //移动方向发生变化
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAnimatorState, int>(temp => temp.MoveAnimatorVectorType)))
        {
            float forwardFloat = iAnimatorState.MoveAnimatorVectorType / 180f;
            playerAnimator.SetFloat("Forward", forwardFloat);
        }
        //魔法动画类型发生变化
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAnimatorState, EnumMagicAnimatorType>(temp => temp.MagicAnimatorType)))
        {
            switch (iAnimatorState.MagicAnimatorType)
            {
                case EnumMagicAnimatorType.Sing:
                    playerAnimator.SetBool("Sing", true);
                    playerAnimator.SetTrigger("Magic");
                    playerAnimator.SetTrigger("ChangeMode");
                    playerAnimator.SetLayerWeight(1, 0.5f);
                    break;
                case EnumMagicAnimatorType.Shot:
                    playerAnimator.SetBool("Sing", false);
                    playerAnimator.SetLayerWeight(1, 0f);
                    break;
                default:
                    playerAnimator.SetBool("Sing", false);
                    playerAnimator.SetLayerWeight(1, 0f);
                    break;
            }
        }
        //物理(普通攻击)动画类型发生变化
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAnimatorState, EnumPhysicAnimatorType>(temp => temp.PhysicAnimatorType)))
        {
            switch (iAnimatorState.PhysicAnimatorType)
            {
                case EnumPhysicAnimatorType.Normal:
                    playerAnimator.SetTrigger("Phycis");
                    playerAnimator.SetTrigger("ChangeMode");
                    //根据当前的物理攻击状态随机一个动作
                    if (iAnimatorState.PhycisActionNowType == 0)
                        playerAnimator.SetFloat("PhycisType1", (int)(UnityEngine.Random.Range(0, 10)));
                    else if (iAnimatorState.PhycisActionNowType == 1)
                        playerAnimator.SetFloat("PhycisType2", (int)(UnityEngine.Random.Range(0, 10)));
                    else if (iAnimatorState.PhycisActionNowType == 2)
                        playerAnimator.SetFloat("PhycisType3", (int)(UnityEngine.Random.Range(0, 10)));
                    break;
                case EnumPhysicAnimatorType.Skill:
                    playerAnimator.SetTrigger("Skill");
                    playerAnimator.SetTrigger("ChangeMode");
                    playerAnimator.SetBool("SkillSustainable", iAnimatorState.SkillSustainable);//技能保持持续(主要是有些技能可能会有持续动
                    playerAnimator.SetInteger("SkillType", (int)iAnimatorState.PhysicAnimatorSkillType - (int)EnumSkillType.SpecialPhysicActionReleaseStart);
                    break;
                default:
                    playerAnimator.ResetTrigger("Phycis");
                    playerAnimator.ResetTrigger("Skill");
                    playerAnimator.SetBool("SkillSustainable", false);//技能取消持续(主要是有些技能可能会有持续动作)
                    break;
            }
        }
        //翻滚动画
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAnimatorState, bool>(temp => temp.RollAnimator)))
        {
            if (iAnimatorState.RollAnimator && //翻滚并且
                iSpecialState.Xuanyun.Time <= 0)//没有处于眩晕状态,其他状态后期再加
            {
                //设置翻滚
                playerAnimator.SetTrigger("Roll");
                //设置回false
                iAnimatorState.RollAnimator = false;
            }
        }
    }

    /// <summary>
    /// 监听函数-特殊状态变化
    /// </summary>
    /// <param name="iSpecialState"></param>
    /// <param name="fieldName"></param>
    private void CallBackISpecialState(ISpecialState iSpecialState, string fieldName)
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
