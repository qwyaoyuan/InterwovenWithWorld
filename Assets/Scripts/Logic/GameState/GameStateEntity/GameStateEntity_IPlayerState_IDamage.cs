using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 实现了IPlayerState->IDamage接口的GameState类的一个分支实体
/// </summary>
public partial class GameState
{
    /// <summary>
    /// 实现冲锋时用到的任务对象
    /// </summary>
    RunTaskStruct runTaskStruct_Charge;

    /// <summary>
    /// 实现检测攻击阶段开始时才开始检测
    /// </summary>
    RunTaskStruct runTaskStruct_NormalAttack;

    /// <summary>
    /// 伤害状态接口实现对象的的还是函数
    /// </summary>
    partial void Start_IDamageState()
    {
        runTaskStruct_Charge = TaskTools.Instance.GetRunTaskStruct();
        runTaskStruct_NormalAttack = TaskTools.Instance.GetRunTaskStruct();
    }

    /// <summary>
    /// 根据技能获取粒子的初始化数据
    /// </summary>
    /// <param name="playerObj">对象</param>
    /// <param name="nowIAttributeState">本次计算所使用的状态数据</param>
    /// <param name="skills">技能数组</param>
    /// <returns></returns>
    public ParticalInitParamData[] GetParticalInitParamData(GameObject playerObj, IAttributeState nowIAttributeState, params SkillBaseStruct[] skills)
    {
        List<ParticalInitParamData> resultList = new List<ParticalInitParamData>();
        ParticalInitParamData particalInitParamData = default(ParticalInitParamData);
        ISkillState iSkillState = GameState.Instance.GetEntity<ISkillState>();
        PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
        //耗魔量
        float holdingRate = iSkillState.SkillStartHoldingTime / GameState.BaseSkillStartHoldingTime;//计算蓄力程度
        float maxUseMana = nowIAttributeState.MaxUseMana;//当前的最大耗魔上限
        float baseUseMana = nowIAttributeState.MustUsedBaseMana;//基础耗魔值
        float thisUsedMana = baseUseMana + maxUseMana * holdingRate;//该技能的耗魔值
        //技能附加的特效(特殊效果和debuff)
        StatusData statusData = DataCenter.Instance.GetMetaData<StatusData>();//保存特殊状态的数据
        Dictionary<StatusDataInfo.StatusLevelDataInfo, int> statusEffects = new Dictionary<StatusDataInfo.StatusLevelDataInfo, int>();//特殊效果对应该等级数据与对应等级字典
        foreach (SkillBaseStruct skillBaseStruct in skills)
        {
            if (skillBaseStruct == null)
                continue;
            //选取debuff和特殊效果
            IEnumerable<EnumStatusEffect> enumStatusEffects = skillBaseStruct.skillStatusEffect.Where(temp => (temp > EnumStatusEffect.DebuffStart && temp < EnumStatusEffect.DebuffEnd) || (temp > EnumStatusEffect.SpecialStart && temp < EnumStatusEffect.SpecialEnd));
            foreach (EnumStatusEffect enumStatusEffect in enumStatusEffects)
            {
                StatusDataInfo statusDataInfo = statusData[enumStatusEffect];
                if (statusDataInfo == null)
                    continue;
                int level = 0;
                playerState.SkillPoint.TryGetValue(skillBaseStruct.skillType, out level);
                StatusDataInfo.StatusLevelDataInfo statusLevelDataInfo = statusDataInfo[level];
                if (statusLevelDataInfo != null)
                {
                    if (statusEffects.Count(temp => temp.Key.EffectType == enumStatusEffect) > 0)//如果已经存在相同类型的特效
                    {
                        KeyValuePair<StatusDataInfo.StatusLevelDataInfo, int> tempStatusEffect = statusEffects.FirstOrDefault(temp => temp.Key.EffectType == enumStatusEffect);
                        int nowLevel = tempStatusEffect.Value;
                        if (nowLevel < level)//计算当前存放的特效等级是否高于新加的等级
                        {
                            statusEffects.Remove(tempStatusEffect.Key);
                            statusEffects.Add(statusLevelDataInfo, level);
                        }
                    }
                    else
                    {
                        statusEffects.Add(statusLevelDataInfo, level);
                    }
                }
            }
        }
        StatusDataInfo.StatusLevelDataInfo[] statusLevelDataInfos = statusEffects.Keys.ToArray();
        //这几个是基础数据
        particalInitParamData.position = playerObj.transform.position + playerObj.transform.forward * 0.3f + playerObj.transform.up * 1.5f;
        particalInitParamData.lifeTime = 5;
        particalInitParamData.checkCollisionIntervalTime = 1;
        particalInitParamData.targetObjs = new GameObject[0];
        particalInitParamData.forward = playerObj.transform.forward;
        particalInitParamData.color = new Color(0.5f, 0.5f, 0.5f, 0.1f);
        particalInitParamData.layerMask = LayerMask.GetMask("Monster", "Default");
        //下面的是变化数据
        //颜色
        SkillBaseStruct combine_secondSkill = skills.Where(temp => temp != null).FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel2Start && temp.skillType < EnumSkillType.MagicCombinedLevel2End);
        if (combine_secondSkill != null)
        {
            switch (combine_secondSkill.skillType)
            {
                case EnumSkillType.YSX01://火元素
                    particalInitParamData.color = Color.red;
                    break;
                case EnumSkillType.YSX02://水元素
                    particalInitParamData.color = Color.blue;
                    break;
                case EnumSkillType.YSX03://土元素
                    particalInitParamData.color = Color.yellow;
                    break;
                case EnumSkillType.YSX04://风元素
                    particalInitParamData.color = Color.green;
                    break;
                case EnumSkillType.SM06://冰元素
                    particalInitParamData.color = Color.cyan;
                    break;
                case EnumSkillType.SM07://雷元素
                    particalInitParamData.color = new Color(0.5f, 0, 0.5f, 1);
                    break;
                case EnumSkillType.DSM03://光明元素
                case EnumSkillType.XYX01_Target://光明信仰基础_对敌军
                    particalInitParamData.color = Color.white;
                    break;
                case EnumSkillType.DSM04://黑暗元素
                case EnumSkillType.XYX02_Target://黑暗信仰基础_对敌军
                    particalInitParamData.color = Color.black;
                    break;
                case EnumSkillType.XYX03_Target://生物信仰基础_对敌军
                    particalInitParamData.color = new Color(0, 1, 0.2f, 1);
                    break;
                case EnumSkillType.XYX04_Target://自然信仰基础_对敌军
                    //颜色选择为当前环境对应的元素
                    IEnvironment iEnvironment = GameState.Instance.GetEntity<IEnvironment>();
                    switch (iEnvironment.TerrainEnvironmentType)
                    {
                        case EnumEnvironmentType.Plain:
                            particalInitParamData.color = new Color(0.5f, 0, 0.5f, 1);
                            break;
                        case EnumEnvironmentType.Swamp:
                            particalInitParamData.color = Color.blue;
                            break;
                        case EnumEnvironmentType.Desert:
                            particalInitParamData.color = Color.yellow;
                            break;
                        case EnumEnvironmentType.Forest:
                            particalInitParamData.color = Color.green;
                            break;
                        case EnumEnvironmentType.Volcano:
                            particalInitParamData.color = Color.red;
                            break;
                    }
                    break;
            }
        }
        //技能最基础表现形式
        SkillBaseStruct combine_firstSkill = skills.Where(temp => temp != null).FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel1Start && temp.skillType < EnumSkillType.MagicCombinedLevel1End);
        IMonsterCollection iMonsterCollection = GameState.Instance.GetEntity<IMonsterCollection>();
        GameObject selectTargetObj = null;//魔力导向选中的对象
        if (combine_firstSkill != null)
        {
            switch (combine_firstSkill.skillType)
            {
                case EnumSkillType.FS01://奥数弹
                    particalInitParamData.range = 20;//表示距离
                    particalInitParamData.CollisionCallBack = temp => CalculateCombineMagicHurt(nowIAttributeState, temp, thisUsedMana, statusLevelDataInfos);
                    break;
                case EnumSkillType.FS02://奥数震荡
                    particalInitParamData.range = 1;//表示比例
                    particalInitParamData.CollisionCallBack = temp => CalculateCombineMagicHurt(nowIAttributeState, temp, thisUsedMana, statusLevelDataInfos);
                    particalInitParamData.position = playerObj.transform.position + playerObj.transform.forward * 2f;
                    break;
                case EnumSkillType.FS03://魔力屏障
                    particalInitParamData.range = 1;//表示比例
                    particalInitParamData.CollisionCallBack = temp => CalculateCombineMagicHurt(nowIAttributeState, temp, thisUsedMana, statusLevelDataInfos);
                    break;
                case EnumSkillType.FS04://魔力导向
                    //查找前方45度方位内距离自己最近的怪物
                    GameObject[] selectObjs = iMonsterCollection.GetMonsters(playerObj, 45, 20);
                    if (selectObjs != null && selectObjs.Length > 0)
                        selectTargetObj = selectObjs[0];
                    if (selectTargetObj)
                    {
                        particalInitParamData.range = Vector3.Distance(selectTargetObj.transform.position, playerObj.transform.position);//表示距离
                        particalInitParamData.targetObjs = new GameObject[] { selectTargetObj };
                        particalInitParamData.CollisionCallBack = temp => CalculateCombineMagicHurt(nowIAttributeState, temp, thisUsedMana, statusLevelDataInfos);
                    }
                    else
                        particalInitParamData.range = 10;
                    break;
                case EnumSkillType.MFS05://魔力脉冲
                    particalInitParamData.range = 1;//表示比例
                    if (combine_secondSkill != null)
                    {
                        switch (combine_secondSkill.skillType)
                        {
                            case EnumSkillType.YSX03://土元素 大地咆哮
                            case EnumSkillType.YSX04://风元素 风暴突袭
                            case EnumSkillType.SM06://冰元素 寒霜吐息
                                particalInitParamData.position = playerObj.transform.position + playerObj.transform.forward * 1f;
                                break;
                        }
                    }
                    particalInitParamData.CollisionCallBack = temp => CalculateCombineMagicHurt(nowIAttributeState, temp, thisUsedMana, statusLevelDataInfos);
                    break;
            }
        }
        resultList.Add(particalInitParamData);
        //第三阶段的连续魔力导向有点特殊
        SkillBaseStruct combine_thirdSkill = skills.Where(temp => temp != null).FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel3Start && temp.skillType < EnumSkillType.MagicCombinedLevel3End);
        if (combine_thirdSkill != null && combine_thirdSkill.skillType == EnumSkillType.MFS06)
        {
            //查找周围距离查找到的怪物的最近的怪物
            if (selectTargetObj)
            {
                GameObject[] nextObjs = iMonsterCollection.GetMonsters(selectTargetObj, -1, 100);//测试用100 默认是10
                Queue<GameObject> queueNextObj = new Queue<GameObject>();
                queueNextObj.Enqueue(selectTargetObj);
                foreach (var item in nextObjs)
                {
                    queueNextObj.Enqueue(item);
                }
                while (queueNextObj.Count > 1)
                {
                    GameObject firstObj = queueNextObj.Dequeue();//第一个怪物
                    GameObject secondObj = queueNextObj.Peek();//第二个怪物
                    ParticalInitParamData temp_particalInitParamData = particalInitParamData;
                    temp_particalInitParamData.forward = (secondObj.transform.position - firstObj.transform.position).normalized;
                    temp_particalInitParamData.position = firstObj.transform.position + temp_particalInitParamData.forward;
                    temp_particalInitParamData.range = Vector3.Distance(firstObj.transform.position, secondObj.transform.position);
                    temp_particalInitParamData.targetObjs = new GameObject[] { secondObj };
                    temp_particalInitParamData.CollisionCallBack = (temp) => CalculateCombineMagicHurt(nowIAttributeState, temp, thisUsedMana, statusLevelDataInfos);
                    resultList.Add(temp_particalInitParamData);
                }
            }
        }

        return resultList.ToArray();
    }

    /// <summary>
    /// 计算魔法组合技能的伤害
    /// </summary>
    /// <param name="iAttributeState">伤害计算时使用的属性</param>
    /// <param name="collisionHitCallbackStruct">碰撞到的对象</param>
    /// <param name="thisUsedMana">本次技能的耗魔值</param>
    /// <param name="statusLevelDataInfos">本次技能附加的特殊效果数组</param>
    /// <returns></returns>
    private bool CalculateCombineMagicHurt(IAttributeState iAttributeState, CollisionHitCallbackStruct collisionHitCallbackStruct, float thisUsedMana, StatusDataInfo.StatusLevelDataInfo[] statusLevelDataInfos)
    {
        if (collisionHitCallbackStruct.targetObj == null)
            return false;
        InteractiveAsMonster interactiveAsMonster = collisionHitCallbackStruct.targetObj.GetComponent<InteractiveAsMonster>();
        if (interactiveAsMonster == null)
            return false;
        IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        AttackHurtStruct attackHurtStruct = new AttackHurtStruct()
        {
            hurtTransferNum = 0,
            hurtType = EnumHurtType.Magic,
            attributeState = iAttributeState,
            thisUsedMana = thisUsedMana,
            hurtFromObj = iPlayerState.PlayerObj,
            statusLevelDataInfos = statusLevelDataInfos,
            MagicAttackFactor = new MagicAttackFactor()
            {
                IncreaseRatioInjuryFactor = iPlayerState.SelfRoleOfRaceInfoStruct.magicAttackToDamageRateRatio
            }
        };
        interactiveAsMonster.GiveAttackHurtStruct(attackHurtStruct);
        return true;
    }

    /// <summary>
    /// 设置普通攻击
    /// </summary>
    /// <param name="iPlayerState">玩家状态对象</param>
    /// <param name="attackOrder">攻击的编号</param>
    /// <param name="nowIAttributeState">本技能释放时的数据状态</param>
    /// <param name="weaponTypeByPlayerState">武器类型</param>
    public void SetNormalAttack(IPlayerState iPlayerState, int attackOrder, IAttributeState nowIAttributeState, EnumWeaponTypeByPlayerState weaponTypeByPlayerState)
    {
        if (iPlayerState == null || nowIAttributeState == null)
            return;
        IPlayerState _iPlayerState = iPlayerState;
        IAttributeState _NowIAttributeState = nowIAttributeState;
        EnumWeaponTypeByPlayerState _WeaponTypeByPlayerState = weaponTypeByPlayerState;
        IAnimatorState iAnimatorState = GameState.Instance.GetEntity<IAnimatorState>();
        int _attackOrder = attackOrder;
        PhysicSkillInjuryDetection physicSkillInjuryDetection = _iPlayerState.PhysicSkillInjuryDetection;
        float runTime = 0;//检测超时时间
        if (physicSkillInjuryDetection != null)
        {
            Action BeginCheckAttack = () => 
            {
                runTaskStruct_NormalAttack.StopTask();//停止任务
                physicSkillInjuryDetection.CheckAttack(EnumSkillType.PhysicAttack, _WeaponTypeByPlayerState, 1, null, (innerOrder, target) =>
                {
                    return CalculateNormalActionHurt(_WeaponTypeByPlayerState, _NowIAttributeState, innerOrder, target);
                },
                attackOrder
                );
            };
            //该任务用于检测是否可以进入该普通攻击的检测
            runTaskStruct_NormalAttack.StartTask(0, 
                () => 
                {
                    switch (attackOrder)
                    {
                        case 1:
                            if (iAnimatorState.AnimationClipTypeState != null && iAnimatorState.AnimationClipTypeState.AnimationClipType == EnumAnimationClipType.Attack1 && iAnimatorState.AnimationClipTypeState.TimeType == EnumAnimationClipTimeType.In)
                            {
                                BeginCheckAttack();
                            }
                            break;
                        case 2:
                            if (iAnimatorState.AnimationClipTypeState != null && iAnimatorState.AnimationClipTypeState.AnimationClipType == EnumAnimationClipType.Attack2 && iAnimatorState.AnimationClipTypeState.TimeType == EnumAnimationClipTimeType.In)
                            {
                                BeginCheckAttack();
                            }
                            break;
                        case 3:
                            if (iAnimatorState.AnimationClipTypeState != null && iAnimatorState.AnimationClipTypeState.AnimationClipType == EnumAnimationClipType.Attack3 && iAnimatorState.AnimationClipTypeState.TimeType == EnumAnimationClipTimeType.In)
                            {
                                BeginCheckAttack();
                            }
                            break;
                        default:
                            runTaskStruct_NormalAttack.StopTask();//停止任务
                            break;
                    }
                }, 0, true,
                () =>
                {
                    runTime += Time.deltaTime;
                    if (runTime > 1)
                        return false;
                    return true;
                });
        }
    }

    /// <summary>
    /// 计算物理普通攻击伤害
    /// </summary>
    /// <param name="weaponTypeByPlayerState">武器类型</param>
    /// <param name="iAttribute">攻击时的状态数据</param>
    /// <param name="innerOrder">当前攻击的阶段</param>
    /// <param name="target">攻击的目标</param>
    private bool CalculateNormalActionHurt(EnumWeaponTypeByPlayerState weaponTypeByPlayerState, IAttributeState iAttribute, int innerOrder, GameObject target)
    {
        if (target == null)
            return true;
        InteractiveAsMonster interactiveAsMonster = target.GetComponent<InteractiveAsMonster>();
        if (interactiveAsMonster == null)
            return true;
        IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        AttackHurtStruct attackHurtStruct = new AttackHurtStruct()
        {
            hurtTransferNum = 0,
            hurtType = EnumHurtType.NormalAction,
            attributeState = iAttribute,
            hurtFromObj = iPlayerState.PlayerObj,
            PhysicAttackFactor = new PhysicAttackFactor()
            {
                IncreaseRatioInjuryFactor = iPlayerState.SelfRoleOfRaceInfoStruct.physicAttackToDamageRateRatio,
                MinimumDamageFactor = iPlayerState.SelfRoleOfRaceInfoStruct.physicQuickToMinDamageRatio
            }
        };
        interactiveAsMonster.GiveAttackHurtStruct(attackHurtStruct);
        //如果命中了则自身的动画造成0.1秒的延迟
        IAnimatorState iAnimatorState = GameState.Instance.GetEntity<IAnimatorState>();
        if (weaponTypeByPlayerState != EnumWeaponTypeByPlayerState.Arch)
        {
            iAnimatorState.PhysicHitMonsterAnimDelay = true;
        }
        iPlayerState.SetVibration(0.1f, 1, 1);//设置手柄震动
        //停止持续
        iAnimatorState.SkillSustainable = false;
        //判断武器类型
        switch (weaponTypeByPlayerState)//根据武器类型判断是否还需要下次检测
        {
            case EnumWeaponTypeByPlayerState.Arch:
                return false;
            default:
                return true;
        }
    }

    /// <summary>
    /// 设置物理技能攻击
    /// </summary>
    /// <param name="playerObj">玩家操纵状态对象</param>
    /// <param name="physicsSkillStateStruct">本技能释放时的数据状态</param>
    /// <param name="skillType">技能类型</param>
    /// <param name="weaponTypeByPlayerState">武器类型</param>
    public void SetPhysicSkillAttack(IPlayerState iPlayerState, PhysicsSkillStateStruct physicsSkillStateStruct, EnumSkillType skillType, EnumWeaponTypeByPlayerState weaponTypeByPlayerState)
    {
        if (iPlayerState == null || physicsSkillStateStruct == null)
            return;
        IPlayerState _iPlayerState = iPlayerState;
        PhysicsSkillStateStruct _physicsSkillStateStruct = physicsSkillStateStruct;
        EnumSkillType _SKillType = skillType;
        EnumWeaponTypeByPlayerState _WeaponTypeByPlayerState = weaponTypeByPlayerState;
        PhysicSkillInjuryDetection physicSkillInjuryDetection = _iPlayerState.PhysicSkillInjuryDetection;
        if (physicSkillInjuryDetection != null)
        {
            physicSkillInjuryDetection.CheckAttack(_SKillType, _WeaponTypeByPlayerState, 1, null, (innerOrder, target) =>
            {
                return CalculatePhysicSkillHurt(_SKillType, _WeaponTypeByPlayerState, _physicsSkillStateStruct, innerOrder, target);
            });
        }
        //如果这是冲锋则开启任务(移动对象)
        switch (skillType)
        {
            case EnumSkillType.ZS03:
                float zs03Time = 0;
                runTaskStruct_Charge.StartTask(0,
                    () =>
                    {
                        zs03Time += Time.deltaTime;
                        float speedRate = 0;
                        if (zs03Time < 0.3f)
                            speedRate = 0;
                        else speedRate = 1;
                        _iPlayerState.ForceMoveStruct = new ForceMoveStruct() { MoveSpeed = _physicsSkillStateStruct.AttributeState.MoveSpeed * speedRate };
                        if (zs03Time > 1f)
                        {
                            runTaskStruct_Charge.StopTask();
                            IAnimatorState iAnimatorState = GameState.Instance.GetEntity<IAnimatorState>();
                            iAnimatorState.SkillSustainable = false;
                        }
                    }, 0, false);
                break;
        }
    }

    /// <summary>
    /// 计算物理技能伤害
    /// </summary>
    /// <param name="skillType">技能类型</param>
    /// <param name="weaponTypeByPlayerState">武器类型</param>
    /// <param name="physicsSkillStateStruct">物理技能数据</param>
    /// <param name="innerOrder">当前攻击的阶段</param>
    /// <param name="target">攻击的目标</param>
    private bool CalculatePhysicSkillHurt(EnumSkillType skillType, EnumWeaponTypeByPlayerState weaponTypeByPlayerState, PhysicsSkillStateStruct physicsSkillStateStruct, int innerOrder, GameObject target)
    {
        if (target == null)
            return true;
        InteractiveAsMonster interactiveAsMonster = target.GetComponent<InteractiveAsMonster>();
        if (interactiveAsMonster == null)
            return true;
        IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        AttackHurtStruct attackHurtStruct = new AttackHurtStruct()
        {
            hurtTransferNum = 0,
            hurtType = EnumHurtType.PhysicSkill,
            attributeState = physicsSkillStateStruct.AttributeState,
            hurtFromObj = iPlayerState.PlayerObj,
            PhysicAttackFactor = new PhysicAttackFactor()
            {
                IncreaseRatioInjuryFactor = iPlayerState.SelfRoleOfRaceInfoStruct.physicAttackToDamageRateRatio,
                MinimumDamageFactor = iPlayerState.SelfRoleOfRaceInfoStruct.physicQuickToMinDamageRatio
            }
        };
        interactiveAsMonster.GiveAttackHurtStruct(attackHurtStruct);
        //如果命中了则自身的动画造成0.1秒的延迟
        IAnimatorState iAnimatorState = GameState.Instance.GetEntity<IAnimatorState>();
        if (weaponTypeByPlayerState != EnumWeaponTypeByPlayerState.Arch)
            iAnimatorState.PhysicHitMonsterAnimDelay = true;
        iPlayerState.SetVibration(0.1f, 1, 1);//设置手柄震动
        //停止持续
        iAnimatorState.SkillSustainable = false;
        //如果是冲锋则停止任务
        switch (skillType)
        {
            case EnumSkillType.ZS03:
                runTaskStruct_Charge.StopTask();
                break;
        }
        //判断技能类型,有些紧只需要判断一次
        switch (skillType)
        {
            case EnumSkillType.GJS03:
            case EnumSkillType.SSS03:
            case EnumSkillType.ZS03:
                return false;
            default:
                return true;
        }
    }
}

