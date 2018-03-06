using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 怪物的基础逻辑脚本,主要负责动画状态的注册监听
/// 还用于处理伤害传递等工作
/// </summary>
public class MonsterBaseLogic : MonoBehaviour
{

    /// <summary>
    /// 动画组件
    /// </summary>
    protected Animator animator;

    /// <summary>
    /// 怪物攻击检测结构
    /// </summary>
    public MonsterAttackStruct monsterAttackStruct;

    /// <summary>
    /// 临时的检测到的对象集合,在切换检测片段时初始化
    /// </summary>
    protected List<IObjInteractive> tempCheckedTargetList;

    private MonsterAttackStruct.MonsterAttackFrame _nowCheckFrame;
    /// <summary>
    /// 当前检测的片段
    /// </summary>
    protected MonsterAttackStruct.MonsterAttackFrame NowCheckFrame
    {
        get { return _nowCheckFrame; }
        set
        {
            if (_nowCheckFrame != null)
                _nowCheckFrame.monsterAttackCheck.CallBack = null;
            if (value != null)
            {
                value.monsterAttackCheck.CallBack = CheckTargetResult;
            }
            _nowCheckFrame = value;
            tempCheckedTargetList.Clear();
        }
    }

    /// <summary>
    /// 动技动画的开始时间如果大于等于0则update累加当前时间,如果小于零表示结束了
    /// </summary>
    protected float attackAnimationTime;

    protected virtual void Awake()
    {
        attackAnimationTime = -1;
        tempCheckedTargetList = new List<IObjInteractive>();
        animator = GetComponent<Animator>();
        PlayerAnimEnterLeaveState[] playerAnimEnterLeaveStates = animator.GetBehaviours<PlayerAnimEnterLeaveState>();
        foreach (PlayerAnimEnterLeaveState playerAnimEnterLeaveState in playerAnimEnterLeaveStates)
        {
            playerAnimEnterLeaveState.AnimEnterLeaveHandle += PlayerAnimEnterLeaveState_AnimEnterLeaveHandle;
        }
    }

    /// <summary>
    /// 动画进入或离开某状态
    /// </summary>
    /// <param name="actionTag"></param>
    /// <param name="enter"></param>
    protected virtual void PlayerAnimEnterLeaveState_AnimEnterLeaveHandle(string actionTag, bool enter)
    {
        if (enter)
        {
            attackAnimationTime = 0;
            MonsterAttackStruct.MonsterAttackFrame tempMonsterAttackFrame = monsterAttackStruct.Search(actionTag);
            if (tempMonsterAttackFrame != null && tempMonsterAttackFrame != NowCheckFrame)
            {
                NowCheckFrame = tempMonsterAttackFrame;
            }
        }
        else
        {
            attackAnimationTime = -1;
            if (NowCheckFrame != null && NowCheckFrame.ActionAnimationTag == actionTag)
                NowCheckFrame = null;
        }
    }

    /// <summary>
    /// 检测到攻击触发到对象(注意通过该函数实现的都是物理攻击,魔法攻击请在各自的子类中独立实现)
    /// </summary>
    /// <param name="iOjbInteractive"></param>
    protected virtual void CheckTargetResult(IObjInteractive iOjbInteractive)
    {
        if (NowCheckFrame == null)
            return;
        if (attackAnimationTime > NowCheckFrame.endTime || attackAnimationTime < NowCheckFrame.startTime)
            return;
        if (tempCheckedTargetList.Contains(iOjbInteractive))
            return;
        tempCheckedTargetList.Add(iOjbInteractive);
        //计算伤害
        MonsterControl monsterControl = GetComponent<MonsterControl>();
        if (monsterControl == null)
            return;
        IAttributeState iAttributeState = monsterControl.GetMonsterAttributeState();
        if (iAttributeState == null)
            return;
        iAttributeState.PhysicsAttacking *= NowCheckFrame.magnification;
        AttackHurtStruct attackHurtStruct = new AttackHurtStruct()
        {
            attributeState = iAttributeState,
            hurtFromObj = gameObject,
            hurtType = EnumHurtType.NormalAction,//怪物的伤害都依照普通攻击来计算(普通攻击和技能的区别暂时不知道)
            statusLevelDataInfos = new StatusDataInfo.StatusLevelDataInfo[0],//暂时没有附加状态
            hurtTransferNum = 0,
            thisUsedMana = 0//物理技能没有耗魔
        };
        iOjbInteractive.GiveAttackHurtStruct(attackHurtStruct);
    }

    protected virtual void Update()
    {
        if (attackAnimationTime >= 0)
        {
            attackAnimationTime += Time.deltaTime;
            if (NowCheckFrame == null)// 如果检测片段是空的不需要下次检测了
                attackAnimationTime = -1;
            if (NowCheckFrame != null && NowCheckFrame.endTime < attackAnimationTime)//如果超过判断时间则不需要检测了
            {
                NowCheckFrame = null;
                attackAnimationTime = -1;
            }
        }
    }

}
