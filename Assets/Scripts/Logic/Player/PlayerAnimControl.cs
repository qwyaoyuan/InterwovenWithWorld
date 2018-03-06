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

    /// <summary>
    /// 攻击目标造成自身动画的延迟携程
    /// </summary>
    Coroutine physicAttackDelayCoroutine;

    /// <summary>
    /// 动画剪辑的状态
    /// </summary>
    List<AnimationClipTypeState> animationClipTypeStateList;

    void Start()
    {
        animationClipTypeStateList = new List<AnimationClipTypeState>();
        iAnimatorState = GameState.Instance.GetEntity<IAnimatorState>();
        iSpecialState = GameState.Instance.GetEntity<ISpecialState>();
        if (!playerAnimator)
            playerAnimator = GetComponent<Animator>();
        if (playerAnimator)
        {

            PlayerAnimRunningState[] playerAnimRunningStates = playerAnimator.GetBehaviours<PlayerAnimRunningState>();
            foreach (PlayerAnimRunningState playerAnimRunningState in playerAnimRunningStates)
            {
                playerAnimRunningState.AnimRunningStateHandle += PlayerAnimRunningState_AnimRunningStateHandle;
            }
        }
        //注册监听
        GameState.Instance.Registor<IPlayerState>(CallBackIPlayerStateState);
        GameState.Instance.Registor<IAnimatorState>(CallBackIAnimatorState);
        GameState.Instance.Registor<ISpecialState>(CallBackISpecialState);
        GameState.Instance.Registor<IPlayerAttributeState>(CallBackIAttributeState);
        //设置初始武器状态
        CallBackIPlayerStateState(GameState.Instance.GetEntity<IPlayerState>(), GameState.GetFieldNameStatic<IPlayerState, bool>(temp => temp.EquipmentChanged));
        //设置初始速度
        CallBackIAttributeState(GameState.Instance.GetEntity<IPlayerAttributeState>(), GameState.GetFieldNameStatic<IPlayerAttributeState, float>(temp => temp.AttackSpeed));
    }

    private void OnDestroy()
    {
        GameState.Instance.UnRegistor<IPlayerState>(CallBackIPlayerStateState);
        GameState.Instance.UnRegistor<IAnimatorState>(CallBackIAnimatorState);
        GameState.Instance.UnRegistor<ISpecialState>(CallBackISpecialState);
        GameState.Instance.UnRegistor<IPlayerAttributeState>(CallBackIAttributeState);
    }

    /// <summary>
    /// 动画进入离开或触发条件时
    /// </summary>
    /// <param name="animationClipType"></param>
    /// <param name="clipTimeType"></param>
    /// <param name="normalizedTime"></param>
    private void PlayerAnimRunningState_AnimRunningStateHandle(EnumAnimationClipType animationClipType, EnumAnimationClipTimeType clipTimeType, float normalizedTime)
    {
        AnimationClipTypeState animationClipTypeState = animationClipTypeStateList.FirstOrDefault(temp => temp.AnimationClipType == animationClipType);
        if (animationClipTypeState == null)
        {
            animationClipTypeState = new AnimationClipTypeState() { AnimationClipType = animationClipType };
            animationClipTypeStateList.Add(animationClipTypeState);
        }
        animationClipTypeState.TimeType = clipTimeType;
        animationClipTypeState.ClipTime = normalizedTime;
        Action SetAttackAction = () =>
        {
            bool allOut = animationClipTypeStateList.Where(temp => temp.AnimationClipType == EnumAnimationClipType.Attack1 ||
                temp.AnimationClipType == EnumAnimationClipType.Attack2 ||
                temp.AnimationClipType == EnumAnimationClipType.Attack3
                ).ToList().TrueForAll(temp => temp.TimeType == EnumAnimationClipTimeType.Out);
            if (allOut)
            {
                iAnimatorState.IsPhycisActionState = false;
                iAnimatorState.PhycisActionNowType = 0;
            }
            else
                iAnimatorState.IsPhycisActionState = true;

        };
        switch (animationClipType)
        {
            case EnumAnimationClipType.Move:
                break;
            //当动作进入攻击1 攻击2 攻击3时分别设置对应的物理动作(普通攻击)阶段为对应的阶段
            //当退出动作时设置动作为0(无攻击)
            case EnumAnimationClipType.Attack1:
                SetAttackAction();
                if (clipTimeType == EnumAnimationClipTimeType.In)
                    iAnimatorState.PhycisActionNowType = 1;
                break;
            case EnumAnimationClipType.Attack2:
                SetAttackAction();
                if (clipTimeType == EnumAnimationClipTimeType.In)
                    iAnimatorState.PhycisActionNowType = 2;
                break;
            case EnumAnimationClipType.Attack3:
                if (clipTimeType == EnumAnimationClipTimeType.In)
                    iAnimatorState.PhycisActionNowType = 3;
                else if (clipTimeType == EnumAnimationClipTimeType.Out)
                    iAnimatorState.PhycisActionNowType = 0;
                SetAttackAction();
                break;
            case EnumAnimationClipType.MagicSing:
                if (clipTimeType == EnumAnimationClipTimeType.In)
                    iAnimatorState.IsMagicActionState = true;
                break;
            case EnumAnimationClipType.MagicShot:
                if (clipTimeType == EnumAnimationClipTimeType.In)
                    iAnimatorState.IsMagicActionState = true;
                else if (clipTimeType == EnumAnimationClipTimeType.Out)
                    iAnimatorState.IsMagicActionState = false;
                break;
            case EnumAnimationClipType.PhysicSkill:
                if (clipTimeType == EnumAnimationClipTimeType.In)
                    iAnimatorState.IsSkillState = true;
                else if (clipTimeType == EnumAnimationClipTimeType.Out)
                    iAnimatorState.IsSkillState = false;
                break;
            case EnumAnimationClipType.Dizzy:
                if (clipTimeType == EnumAnimationClipTimeType.Out)
                {
                    iAnimatorState.PhycisActionNowType = 0;
                    iAnimatorState.IsPhycisActionState = false;
                    iAnimatorState.IsSkillState = false;
                    iAnimatorState.IsMagicActionState = false;
                    iAnimatorState.IsGetHitAnimator = false;
                    playerAnimator.SetLayerWeight(1, 0f);
                }
                break;
            case EnumAnimationClipType.Roll:
                break;
            case EnumAnimationClipType.GetHit:
                if (clipTimeType == EnumAnimationClipTimeType.Out)
                {
                    iAnimatorState.PhycisActionNowType = 0;
                    iAnimatorState.IsPhycisActionState = false;
                    iAnimatorState.IsSkillState = false;
                    iAnimatorState.IsMagicActionState = false;
                    iAnimatorState.IsGetHitAnimator = false;
                    playerAnimator.SetLayerWeight(1, 0f);
                }
                break;
            case EnumAnimationClipType.Death:
                if (clipTimeType == EnumAnimationClipTimeType.Out)
                {
                    iAnimatorState.PhycisActionNowType = 0;
                    iAnimatorState.IsPhycisActionState = false;
                    iAnimatorState.IsSkillState = false;
                    iAnimatorState.IsMagicActionState = false;
                    iAnimatorState.IsGetHitAnimator = false;
                    iAnimatorState.IsDeathAnimator = true;
                    playerAnimator.SetLayerWeight(1, 0f);
                }
                else if (clipTimeType == EnumAnimationClipTimeType.In)//进入死亡状态
                {
                    iAnimatorState.IsDeathAnimator = false;
                }
                break;
        }
        UpdateAnimationClipTypeState();
    }

    /// <summary>
    /// 更新当前动画剪辑的状态
    /// </summary>
    /// <returns></returns>
    private void UpdateAnimationClipTypeState()
    {
        //获取不是结束状态的对象
        AnimationClipTypeState animationClipTypeState = animationClipTypeStateList.FirstOrDefault(temp => temp.TimeType != EnumAnimationClipTimeType.Out);
        iAnimatorState.AnimationClipTypeState = animationClipTypeState;
    }


    /// <summary>
    /// 监听角色状态更改(主要是装备)
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
            PlayGoods playGoods = playerState.PlayerAllGoods.Where(temp => temp.GoodsLocation == GoodsLocation.Wearing && temp.leftRightArms != null && temp.leftRightArms.Value == true).FirstOrDefault();
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
    /// 监听角色属性更改(主要是速度,还有血量)
    /// </summary>
    /// <param name="iPlayerState"></param>
    /// <param name="fieldName"></param>
    private void CallBackIAttributeState(IPlayerAttributeState iPlayerAttribute, string fieldName)
    {
        if (!playerAnimator)
            return;
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IPlayerAttributeState, float>(temp => temp.AttackSpeed)))
        {
            if (physicAttackDelayCoroutine == null)//如果正在执行携程则不做该操作,携程处理完成后会自动处理该操作
            {
                PlayerAnimSpeed.speedRate = iPlayerAttribute.AttackSpeed;
            }
        }
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<IPlayerAttributeState, float>(temp => temp.HP)))
        {
            //设置动画状态
            playerAnimator.SetBool("Death", iPlayerAttribute.HP <= 0);
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
        //攻击造成的动画延迟
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAnimatorState, bool>(temp => temp.PhysicHitMonsterAnimDelay)))
        {
            if (physicAttackDelayCoroutine == null)
            {
                physicAttackDelayCoroutine = StartCoroutine(PhysicAttackDelay());
            }
        }
        //持续动作发生变化
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAnimatorState, bool>(temp => temp.SkillSustainable)))
        {
            playerAnimator.SetBool("SkillSustainable", iAnimatorState.SkillSustainable);//技能保持持续(主要是有些技能可能会有持续动
        }
        //被攻击
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAnimatorState, bool>(temp => temp.IsGetHitAnimator)))
        {
            if (iAnimatorState.IsGetHitAnimator //被攻击状态
                && iSpecialState.Xuanyun.Time <= 0)//没有处于眩晕,其他状态后期再加
            {
                playerAnimator.SetTrigger("GetHit");
            }
            else//否则重置回false
            {
                iAnimatorState.IsGetHitAnimator = false;
            }
        }
    }

    /// <summary>
    /// 攻击造成的延迟处理携程
    /// </summary>
    /// <returns></returns>
    IEnumerator PhysicAttackDelay()
    {
        PlayerAnimSpeed.speedRate = 0;
        yield return new WaitForSeconds(0.15f);
        IAttributeState iAttribute = GameState.Instance.GetEntity<IAttributeState>();
        PlayerAnimSpeed.speedRate = iAttribute.AttackSpeed;
        physicAttackDelayCoroutine = null;
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
