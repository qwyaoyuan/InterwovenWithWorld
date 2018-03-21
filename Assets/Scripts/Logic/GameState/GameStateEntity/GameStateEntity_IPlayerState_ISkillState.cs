using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 实现了ISkillState接口的GameState类的一个分支实体
/// </summary> 
public partial class GameState
{
    partial void Start_IPlayerState_ISkillState()
    {
        skillRuntimeCoolingTimeDic = new Dictionary<int, float>();
        skillMaxCoolingTimeDic = new Dictionary<int, float>();
        _CombineSkills = new SkillBaseStruct[4];
    }

    partial void Load_IPlayerState_ISkillState()
    {
        _IsSkillStartHolding = false;
        _SkillStartHoldingTime = 0;
        InitSpecialSkillStateStruct();
    }

    /// <summary>
    /// 组合技能数组
    /// </summary>
    SkillBaseStruct[] _CombineSkills;

    /// <summary>
    /// 清理临时的组合技能属性函数
    /// </summary>
    Action ClearTempCombineSkillAttributeAction;

    /// <summary>
    /// 技能运行时的冷却时间字典
    /// </summary>
    private Dictionary<int, float> skillRuntimeCoolingTimeDic;

    /// <summary>
    /// 技能当前最大的冷却时间字典
    /// </summary>
    private Dictionary<int, float> skillMaxCoolingTimeDic;

    /// <summary>
    /// 是否开始蓄力
    /// </summary>
    bool _IsSkillStartHolding;
    /// <summary>
    /// 是否开始蓄力
    /// </summary>
    public bool IsSkillStartHolding
    {
        get { return _IsSkillStartHolding; }
        private set
        {
            bool tempIsSkillStartHolding = _IsSkillStartHolding;
            _IsSkillStartHolding = value;
            if (tempIsSkillStartHolding != _IsSkillStartHolding)
                Call<ISkillState, bool>(temp => temp.IsSkillStartHolding);
        }
    }

    /// <summary>
    /// 基础的最大蓄力时间
    /// </summary>
    private static float _BaseSkillStartHoldingTime = 2;
    /// <summary>
    /// 基础的最大蓄力时间
    /// </summary>
    public static float BaseSkillStartHoldingTime
    {
        get
        {
            return _BaseSkillStartHoldingTime;
        }
        private set
        {
            _BaseSkillStartHoldingTime = value;
        }
    }
    /// <summary>
    /// 技能蓄力时间
    /// </summary>
    float _SkillStartHoldingTime;
    /// <summary>
    /// 技能蓄力时间
    /// </summary>
    public float SkillStartHoldingTime
    {
        get { return _SkillStartHoldingTime; }
        private set
        {
            float tempSkillStartHoldingTime = _SkillStartHoldingTime;
            _SkillStartHoldingTime = value;
            if (_SkillStartHoldingTime != tempSkillStartHoldingTime)
                Call<ISkillState, float>(temp => temp.SkillStartHoldingTime);
        }
    }

    /// <summary>
    /// 基础的公共冷却时间常量
    /// </summary>
    const float BasePublicCoolingTime = 1;
    /// <summary>
    /// 公共冷却时间
    /// </summary>
    float _PublicCoolingTime;
    /// <summary>
    /// 公共冷却时间
    /// </summary>
    public float PublicCoolingTime
    {
        get { return _PublicCoolingTime; }
        private set
        {
            float tempPublicCoolingTime = _PublicCoolingTime;
            _PublicCoolingTime = value;
            if (_PublicCoolingTime != tempPublicCoolingTime)
                Call<ISkillState, float>(temp => temp.PublicCoolingTime);
        }
    }
    /// <summary>
    /// 普通攻击冷却时间(这个冷却时间和公共冷却不一样,主要对应与普通攻击)
    /// </summary>
    float normalAttackCoolintTime;

    /// <summary>
    /// 获取或设置组合技能
    /// 设置:如果传入的长度为0或空,则清空,否则如果可以组合则附加,如果无法组合则替换;如果处于蓄力阶段,则无法设置新的技能
    /// 获取:返回当前组合的技能数组
    /// </summary>
    public SkillBaseStruct[] CombineSkills
    {
        get { if (_CombineSkills == null) return new SkillBaseStruct[0]; else return _CombineSkills.Where(temp => temp != null).ToArray(); }
        set
        {
            if (_IsSkillStartHolding)
                return;
            if (_CombineSkills == null)
                _CombineSkills = new SkillBaseStruct[4];
            if (value == null || value.Count(temp => temp == null) > 0)
            {
                SkillBaseStruct[] tempCombineSkills = _CombineSkills;
                _CombineSkills = new SkillBaseStruct[4];
                if (tempCombineSkills.Count(temp => temp == null) != 4)
                    Call<ISkillState, SkillBaseStruct[]>(temp => temp.CombineSkills);
            }
            else
            {
                SkillBaseStruct firstSkillBaseStruct = _CombineSkills[0];
                SkillBaseStruct secondSkillBaseStruct = _CombineSkills[1];
                SkillBaseStruct thirdSkillBaseStruct = _CombineSkills[2];
                SkillBaseStruct fourthSkillBaseStruct = _CombineSkills[3];
                SkillBaseStruct _firstSkillBaseStruct = value.FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel1Start && temp.skillType < EnumSkillType.MagicCombinedLevel1End);
                SkillBaseStruct _secondSkillBaseStruct = value.FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel2Start && temp.skillType < EnumSkillType.MagicCombinedLevel2End);
                SkillBaseStruct _thirdSkillBaseStruct = value.FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel3Start && temp.skillType < EnumSkillType.MagicCombinedLevel3End);
                SkillBaseStruct _fourthSkillBaseStruct = value.FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel4Start && temp.skillType < EnumSkillType.MagicCombinedLevel4End);
                SkillBaseStruct combineFirstSkillBaseStruct = _firstSkillBaseStruct != null ? _firstSkillBaseStruct : firstSkillBaseStruct;
                SkillBaseStruct combineSecondSkillBaseStruct = _secondSkillBaseStruct != null ? _secondSkillBaseStruct : secondSkillBaseStruct;
                SkillBaseStruct combineThirdSkillBaseStruct = _thirdSkillBaseStruct != null ? _thirdSkillBaseStruct : thirdSkillBaseStruct;
                SkillBaseStruct combineFourthSkillBaseStruct = _fourthSkillBaseStruct != null ? _fourthSkillBaseStruct : fourthSkillBaseStruct;
                bool canUseThisCombine = SkillCombineStaticTools.GetCanCombineSkills(
                    combineFirstSkillBaseStruct != null ? combineFirstSkillBaseStruct.skillType : EnumSkillType.None,
                    combineSecondSkillBaseStruct != null ? combineSecondSkillBaseStruct.skillType : EnumSkillType.None,
                    combineThirdSkillBaseStruct != null ? combineThirdSkillBaseStruct.skillType : EnumSkillType.None,
                    combineFourthSkillBaseStruct != null ? combineFourthSkillBaseStruct.skillType : EnumSkillType.None);
                if (canUseThisCombine)
                {
                    _CombineSkills[0] = combineFirstSkillBaseStruct;
                    _CombineSkills[1] = combineSecondSkillBaseStruct;
                    _CombineSkills[2] = combineThirdSkillBaseStruct;
                    _CombineSkills[3] = combineFourthSkillBaseStruct;
                    if (firstSkillBaseStruct != combineFirstSkillBaseStruct ||
                        secondSkillBaseStruct != combineSecondSkillBaseStruct ||
                        thirdSkillBaseStruct != combineThirdSkillBaseStruct ||
                        fourthSkillBaseStruct != combineFourthSkillBaseStruct)
                        Call<ISkillState, SkillBaseStruct[]>(temp => temp.CombineSkills);
                }
                else
                {
                    _CombineSkills[0] = _firstSkillBaseStruct;
                    _CombineSkills[1] = _secondSkillBaseStruct;
                    _CombineSkills[2] = _thirdSkillBaseStruct;
                    _CombineSkills[3] = _fourthSkillBaseStruct;
                    if (firstSkillBaseStruct != _firstSkillBaseStruct ||
                        secondSkillBaseStruct != _secondSkillBaseStruct ||
                        thirdSkillBaseStruct != _thirdSkillBaseStruct ||
                        fourthSkillBaseStruct != _fourthSkillBaseStruct)
                        Call<ISkillState, SkillBaseStruct[]>(temp => temp.CombineSkills);
                }
            }
        }
    }

    /// <summary>
    /// 上一次释放魔法的元素类型
    /// 主要是魔法2的类型(风火水土冰雷)
    /// 该字段没有回调事件
    /// </summary>
    public EnumSkillType LastMagicElementType { get; private set; }

    partial void Update_IPlayerState_ISkillState()
    {
        if (IsSkillStartHolding)
        {
            SkillStartHoldingTime += Time.deltaTime;
            if (SkillStartHoldingTime > BaseSkillStartHoldingTime)
                SkillStartHoldingTime = BaseSkillStartHoldingTime;
            normalAttackCoolintTime = 0;//如果正在释放魔法则普通攻击冷却时间重置
        }
        else
        {
            //如果此时已经不需要释放了,但是存在临时技能属性,则移除
            if (ClearTempCombineSkillAttributeAction != null)
            {
                ClearTempCombineSkillAttributeAction();
                ClearTempCombineSkillAttributeAction = null;
            }
        }
        //公共冷却时间变化
        if (PublicCoolingTime > 0)
            PublicCoolingTime -= Time.deltaTime;
        //普通攻击冷却时间变化
        if (normalAttackCoolintTime > 0)
            normalAttackCoolintTime -= Time.deltaTime;
        //自身冷却时间变化
        bool skillCoolingChanged = false;
        int[] coolingTimeSkillIDArray = skillRuntimeCoolingTimeDic.Keys.ToArray();
        foreach (int coolingTimeSkillID in coolingTimeSkillIDArray)
        {
            if (skillRuntimeCoolingTimeDic[coolingTimeSkillID] > 0)
            {
                skillRuntimeCoolingTimeDic[coolingTimeSkillID] -= Time.deltaTime;
                skillCoolingChanged = true;
            }
        }
        if (skillCoolingChanged)
            Call<ISkillState, Func<int, float>>(temp => temp.GetSkillRuntimeCoolingTime);
        //如果此时没有组合技能并且存在临时技能属性,则移除
        if ((_CombineSkills == null || _CombineSkills.Length == 0) && ClearTempCombineSkillAttributeAction != null)
        {
            ClearTempCombineSkillAttributeAction();
            ClearTempCombineSkillAttributeAction = null;
        }
        //如果此时不是物理技能释放状态的动画,则清楚本次的技能数据
        IAnimatorState iAnimatorState = GameState.Instance.GetEntity<IAnimatorState>();
        if (!iAnimatorState.IsSkillState && NowPhysicsSkillStateStruct != null)
        {
            NowPhysicsSkillStateStruct = null;
        }
        if (iAnimatorState.IsSkillState)
        {
            normalAttackCoolintTime = 0;//如果正在释放技能则普通攻击冷却时间重置 
        }
    }

    /// <summary>
    /// 开始按住释放魔法键(用于初始化计时)
    /// </summary>
    /// <returns>是否可以释放该技能</returns>
    public bool StartCombineSkillRelease()
    {
        bool canRelease = _CombineSkills != null
            && _CombineSkills.Count(temp => temp != null) > 0
            && SkillCombineStaticTools.GetCanCombineSkills(_CombineSkills.Select(temp => temp != null ? temp.skillType : EnumSkillType.None).ToArray())
            && PublicCoolingTime <= 0;
        float thisSkillCoolingTime = 0;
        if (canRelease)//判断冷却时间
        {
            int skillID = SkillCombineStaticTools.GetCombineSkillKey(_CombineSkills);
            thisSkillCoolingTime = skillRuntimeCoolingTimeDic.ContainsKey(skillID) ? skillRuntimeCoolingTimeDic[skillID] : 0;
            canRelease = canRelease && thisSkillCoolingTime <= 0;
        }
        IAnimatorState iAnimatorState = GameState.Instance.GetEntity<IAnimatorState>();
        if (canRelease)//判断角色动作
        {
            canRelease = canRelease && (!iAnimatorState.IsSkillState && !iAnimatorState.IsPhycisActionState && !iAnimatorState.IsMagicActionState && !iAnimatorState.IsGetHitAnimator && !iAnimatorState.IsDeathAnimator);
        }
        if (canRelease)
        {
            //移除之前的添加的属性(如果存在)
            if (ClearTempCombineSkillAttributeAction != null)
            {
                ClearTempCombineSkillAttributeAction();
                ClearTempCombineSkillAttributeAction = null;
            }
            //构造技能的属性
            IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
            SkillStructData skillStructData = DataCenter.Instance.GetMetaData<SkillStructData>();
            IPlayerAttributeState iPlayerAttributeState = GameState.Instance.GetEntity<IPlayerAttributeState>();
            IAttributeState baseAttributeState = iPlayerAttributeState.GetAttribute(0);//基础属性
            Dictionary<int, EnumSkillType> handleDic = _CombineSkills.Select(temp => temp != null ? temp.skillType : EnumSkillType.None)
                .Where(temp => temp != EnumSkillType.None)
                .ToDictionary(temp => iPlayerAttributeState.CreateAttributeHandle(), temp => temp);
            //提前设置移除函数
            ClearTempCombineSkillAttributeAction = () =>
            {
                foreach (KeyValuePair<int, EnumSkillType> handle in handleDic)
                {
                    iPlayerAttributeState.RemoveAttribute(handle.Key);
                }
            };
            //int mustUseMana = 0;
            //设置技能属性
            foreach (KeyValuePair<int, EnumSkillType> handle in handleDic)
            {
                IAttributeState tempAttributeState = iPlayerAttributeState.GetAttribute(handle.Key);
                SkillAttributeStruct tempSkillAttributeState = iPlayerState.GetSkillAttributeStruct(handle.Value, skillStructData);
                //if (tempSkillAttributeState != null)
                //    mustUseMana += tempSkillAttributeState.MustUsedBaseMana;//基础的耗魔量
                tempAttributeState.SetRoleOfRaceAddition(iPlayerState.SelfRoleOfRaceInfoStruct);//设置种族成长对象
                iPlayerAttributeState.SetIAttributeStateDataBySkillData(tempAttributeState, baseAttributeState, tempSkillAttributeState);
                tempAttributeState.MustUsedBaseMana = 50;
            }
            IAttributeState resultAttributeState = iPlayerAttributeState.GetResultAttribute();
            //判断技能耗魔量,如果基础耗魔量很大则取消
            if (resultAttributeState.MustUsedBaseMana > iPlayerState.Mana)
            {
                ClearTempCombineSkillAttributeAction();
                ClearTempCombineSkillAttributeAction = null;
                return false;
            }
            //设置基础蓄力时间
            BaseSkillStartHoldingTime = Mathf.Pow(2, _CombineSkills.Count(temp => temp != null) - 1);
            //设置动作
            iAnimatorState.MagicAnimatorType = EnumMagicAnimatorType.Sing;
            //初始化蓄力数据 
            IsSkillStartHolding = true;
            SkillStartHoldingTime = 0;
        }
        return canRelease;
    }

    /// <summary>
    /// 受到攻击后的技能状态改变,如果当前是咏唱状态则打断咏唱,如果当前是释放技能状态则不做处理,如果当前在普通攻击则打断
    /// </summary>
    public void GetHitToSkillState()
    {
        IAnimatorState iAnimatorState = GameState.Instance.GetEntity<IAnimatorState>();
        if ((IsSkillStartHolding && iAnimatorState.IsMagicActionState)//此时正在进行魔法动作(咏唱)
                                                                      //|| (!IsSkillStartHolding && iAnimatorState.IsPhycisActionState)//此时正在进行普通攻击
           || (!IsSkillStartHolding && !iAnimatorState.IsMagicActionState && !iAnimatorState.IsPhycisActionState && !iAnimatorState.IsSkillState && !iAnimatorState.RollAnimator))
        {
            if (ClearTempCombineSkillAttributeAction != null)
            {
                ClearTempCombineSkillAttributeAction();
            }
            IsSkillStartHolding = false;
            SkillStartHoldingTime = 0;
            iAnimatorState.IsGetHitAnimator = true;
        }
    }

    /// <summary>
    /// 结束按住(松开)释放魔法键(用于结束计时并释放)
    /// </summary>
    /// <returns>是否可以释放该技能</returns>
    public bool EndCombineSkillRelease()
    {
        if (_IsSkillStartHolding)
        {
            bool canRelease = _CombineSkills != null
                && _CombineSkills.Count(temp => temp != null) > 0
                && SkillCombineStaticTools.GetCanCombineSkills(_CombineSkills.Select(temp => temp != null ? temp.skillType : EnumSkillType.None).ToArray());
            IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
            IAttributeState baseAttributeState = iPlayerState.GetAttribute(0);//基础属性
            IAttributeState IAttributeState_YSX06_Data_Start = iPlayerState.GetAttribute(10);//元素驻留的属性
            if (_CombineSkills.FirstOrDefault(temp => temp != null && temp.skillType == LastMagicElementType) == null)//本次释放与上次释放不包含相同元素则移除元素驻留的增幅属性
            {
                IAttributeState_YSX06_Data_Start.Init();
            }
            IAttributeState nowIAttributeState = iPlayerState.GetResultAttribute();
            IAttributeState_YSX06_Data_Start.Init();//不管是不是已经清理了,但是在释放完技能后都要清理元素驻留的属性
            SkillStructData skillStructData = DataCenter.Instance.GetMetaData<SkillStructData>();
            //处理技能伤害数据以及粒子(粒子上包含技能伤害判定的回调函数)
            if (canRelease)
            {
                int skillCount = 0;
                //计算被技能整合后的属性(在上面已经计算过了,这里不需要计算了)
                foreach (SkillBaseStruct tempSkillBaseStruct in _CombineSkills)
                {
                    if (tempSkillBaseStruct == null)
                        continue;
                    skillCount++;
                    //SkillAttributeStruct skillAttributeStruct = iPlayerState.GetSkillAttributeStruct(tempSkillBaseStruct.skillType, skillStructData);
                    //AttributeStateAdditional skillAttributeStateAdditional = new AttributeStateAdditional();
                    //skillAttributeStateAdditional.SetRoleOfRaceAddition(iPlayerState.SelfRoleOfRaceInfoStruct);
                    //iPlayerState.SetIAttributeStateDataBySkillData(skillAttributeStateAdditional, baseAttributeState, skillAttributeStruct);
                    //nowIAttributeState += skillAttributeStateAdditional;
                }
                //根据组合的数量计算最终法伤
                if (skillCount > 0)
                    nowIAttributeState.BaseMagicDamage /= skillCount;

                //判断这个是攻击用途还是辅助用途
                SkillBaseStruct combine_secondSkill = _CombineSkills.Where(temp => temp != null).FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel2Start && temp.skillType < EnumSkillType.MagicCombinedLevel2End);
                bool attackType = true;
                if (combine_secondSkill != null)
                {
                    switch (combine_secondSkill.skillType)
                    {
                        case EnumSkillType.XYX01_Self:
                        case EnumSkillType.XYX02_Self:
                        case EnumSkillType.XYX03_Self:
                        case EnumSkillType.XYX03_None:
                            attackType = false;
                            break;
                    }
                }
                //这里需要根据类型判断粒子;
                int key = SkillCombineStaticTools.GetCombineSkillKey(_CombineSkills);
                //如果存在二阶段,且二阶段是信仰1中的自然(敌),则需要特殊处理
                if (combine_secondSkill != null && combine_secondSkill.skillType == EnumSkillType.XYX04_Target)
                {
                    IEnvironment iEnvironment = GameState.Instance.GetEntity<IEnvironment>();
                    int index = _CombineSkills.ToList().IndexOf(combine_secondSkill);
                    SkillBaseStruct[] temp_CombineSkills = (SkillBaseStruct[])_CombineSkills.Clone();
                    switch (iEnvironment.TerrainEnvironmentType)
                    {
                        case EnumEnvironmentType.Plain:
                            temp_CombineSkills[index] = skillStructData.SearchSkillDatas(temp => temp.skillType == EnumSkillType.SM07).FirstOrDefault();
                            break;
                        case EnumEnvironmentType.Swamp:
                            temp_CombineSkills[index] = skillStructData.SearchSkillDatas(temp => temp.skillType == EnumSkillType.YSX02).FirstOrDefault();
                            break;
                        case EnumEnvironmentType.Desert:
                            temp_CombineSkills[index] = skillStructData.SearchSkillDatas(temp => temp.skillType == EnumSkillType.YSX03).FirstOrDefault();
                            break;
                        case EnumEnvironmentType.Forest:
                            temp_CombineSkills[index] = skillStructData.SearchSkillDatas(temp => temp.skillType == EnumSkillType.YSX04).FirstOrDefault();
                            break;
                        case EnumEnvironmentType.Volcano:
                            temp_CombineSkills[index] = skillStructData.SearchSkillDatas(temp => temp.skillType == EnumSkillType.YSX01).FirstOrDefault();
                            break;
                    }
                    key = SkillCombineStaticTools.GetCombineSkillKey(temp_CombineSkills);
                }
                GameObject particalPrefab = SkillCombineStaticTools.GetCombineSkillsPartical(key);
                if (particalPrefab != null)
                {
                    if (attackType)//如果是攻击用途则实例化粒子
                    {
                        IDamage iDamage = GameState.Instance.GetEntity<IDamage>();
                        ParticalInitParamData[] particalInitParamDatas = iDamage.GetParticalInitParamData(iPlayerState.PlayerObj, nowIAttributeState, _CombineSkills);
                        if (iPlayerState.PlayerObj)
                        {
                            foreach (ParticalInitParamData particalInitParamData in particalInitParamDatas)
                            {
                                GameObject particalObj = GameObject.Instantiate<GameObject>(particalPrefab);
                                ParticalControlEntry particalControlEntry = particalObj.GetComponent<ParticalControlEntry>();
                                particalControlEntry.SetLifeCycle(particalInitParamData.lifeTime);
                                particalControlEntry.SetCheckCollisionIntervalTime(particalInitParamData.checkCollisionIntervalTime);
                                particalControlEntry.Init(
                                    particalInitParamData.position,
                                    particalInitParamData.forward,
                                    particalInitParamData.color,
                                    particalInitParamData.layerMask,
                                    particalInitParamData.CollisionCallBack,
                                    particalInitParamData.range,
                                    particalInitParamData.targetObjs);
                            }
                        }
                    }
                    else//如果是辅助用途则不同效果不同的处理
                    {
                        Func<CollisionHitCallbackStruct, bool> ThisCallBack = temp => true;
                        //如果是召唤类则特殊处理
                        if (combine_secondSkill != null && combine_secondSkill.skillType == EnumSkillType.XYX03_None)
                        {
                            //召唤怪物
                        }
                        else//其他的类型暂定为buff,例子释放到身上,同时给自身加buff 
                        {
                            //加buff
                            IBuffState iBuffState = GameState.Instance.GetEntity<IBuffState>();
                            IDebuffState iDebuffState = GameState.Instance.GetEntity<IDebuffState>();
                            ISpecialState iSpecialState = GameState.Instance.GetEntity<ISpecialState>();
                            SetBuff(iBuffState, _CombineSkills, nowIAttributeState);
                            SetSpecialSkillSetting(iBuffState, iDebuffState, iSpecialState, _CombineSkills);
                            //设置buff的回调
                            ThisCallBack += temp =>
                            {
                                if (temp.targetObj)
                                {
                                    IBuffState targetIBuffState = temp.targetObj.GetComponent<IBuffState>();
                                    IDebuffState targetDebuffState = temp.targetObj.GetComponent<IDebuffState>();
                                    ISpecialState targetSpecialState = temp.targetObj.GetComponent<ISpecialState>();
                                    if (targetIBuffState != null)
                                    {
                                        SetBuff(targetIBuffState, _CombineSkills, nowIAttributeState);
                                        SetSpecialSkillSetting(targetIBuffState, targetDebuffState, targetSpecialState, _CombineSkills);
                                        return true;
                                    }
                                }
                                return false;
                            };
                        }
                        //例子释放到身上
                        GameObject particalObj = GameObject.Instantiate<GameObject>(particalPrefab);
                        ParticalControlEntry particalControlEntry = particalObj.GetComponent<ParticalControlEntry>();
                        particalControlEntry.SetLifeCycle(5);
                        particalControlEntry.SetCheckCollisionIntervalTime(5);
                        particalControlEntry.Init(
                               iPlayerState.PlayerObj.transform.position + Vector3.up,
                               iPlayerState.PlayerObj.transform.forward,
                               Color.red,
                               LayerMask.GetMask("Player", "Summon"),//这里的层是玩家和召唤物
                               ThisCallBack,//如果是buff则这里会有回调
                               1);

                    }
                }
            }
            #region 特殊技能的设置
            //使用本次释放魔法的元素类型替换
            SkillBaseStruct secondSkillBaseStruct_Element = _CombineSkills.Where(temp => temp != null).FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel2Start && temp.skillType < EnumSkillType.MagicCombinedLevel2End);
            if (secondSkillBaseStruct_Element == null ||
                (secondSkillBaseStruct_Element.skillType != EnumSkillType.YSX01 &&
                secondSkillBaseStruct_Element.skillType != EnumSkillType.YSX02 &&
                secondSkillBaseStruct_Element.skillType != EnumSkillType.YSX03 &&
                secondSkillBaseStruct_Element.skillType != EnumSkillType.YSX04 &&
                secondSkillBaseStruct_Element.skillType != EnumSkillType.SM06 &&
                secondSkillBaseStruct_Element.skillType != EnumSkillType.SM07 &&
                secondSkillBaseStruct_Element.skillType != EnumSkillType.DSM03 &&
                secondSkillBaseStruct_Element.skillType != EnumSkillType.DSM04))
                LastMagicElementType = EnumSkillType.None;
            else LastMagicElementType = secondSkillBaseStruct_Element.skillType;
            //元素驻留的增幅属性
            IAttributeState IAttributeState_YSX06_Data_End = iPlayerState.GetAttribute(10);
            //如果存在元素驻留的效果元素驻留 
            if (_CombineSkills.Count(temp => temp != null && temp.skillType == EnumSkillType.YSX06) == 1)
            {
                IAttributeState_YSX06_Data_End.MagicAttacking = baseAttributeState.MagicAttacking * 0.25f;//将法伤提高,下次在释放时,会先判断元素类型是否一致,如果不一致则会先初始化该对象
            }
            #endregion
            #region 公共设置
            //根据当前技能设置本次的冷却时间以及耗魔量
            //技能冷却时间
            int thisSkillID = SkillCombineStaticTools.GetCombineSkillKey(_CombineSkills);
            float selfSkillCoolingTime = nowIAttributeState.CoolingTime;//魔法字节自带的冷却时间
            float baseSkillCoolingTime = Mathf.Pow(2, _CombineSkills.Count(temp => temp != null)) /2;//根据阶段算出来的基础冷却时间
            float thisSkillHodlingCoolintTime = BaseSkillStartHoldingTime * SkillStartHoldingTime;//根据最大蓄力时间和本次蓄力时间计算出的冷却时间
            float thisSkillCoolingTime = selfSkillCoolingTime + baseSkillCoolingTime + thisSkillHodlingCoolintTime;//整合后的冷却时间
            float exemptionSkillCoolintTime = nowIAttributeState.ExemptionChatingMana;//该技能减少冷却时间百分比
            exemptionSkillCoolintTime = Mathf.Clamp(exemptionSkillCoolintTime, 0, 0.5f);//将数值锁定到0-0.5的范围之间 
            thisSkillCoolingTime = thisSkillCoolingTime - thisSkillCoolingTime * exemptionSkillCoolintTime;//计算出该技能的冷却时间
            if (skillRuntimeCoolingTimeDic != null)//运行时逐步递减的当前剩余冷却时间
            {
                if (skillRuntimeCoolingTimeDic.ContainsKey(thisSkillID))
                    skillRuntimeCoolingTimeDic[thisSkillID] = thisSkillCoolingTime;
                else skillRuntimeCoolingTimeDic.Add(thisSkillID, thisSkillCoolingTime);
            }
            if (skillMaxCoolingTimeDic != null)//释放技能后当前技能的最大冷却时间
            {
                if (skillMaxCoolingTimeDic.ContainsKey(thisSkillID))
                    skillMaxCoolingTimeDic[thisSkillID] = thisSkillCoolingTime;
                else skillMaxCoolingTimeDic.Add(thisSkillID, thisSkillCoolingTime);
            }
            //公共冷却时间
            float exemptionPublicCoolingTime = nowIAttributeState.ReduceCoolingTime;//公共冷却时间减少百分比
            exemptionSkillCoolintTime = Mathf.Clamp(exemptionPublicCoolingTime, 0, 0.75f);//将数值锁定到0-0.75的范围之间 
            PublicCoolingTime = BasePublicCoolingTime - BasePublicCoolingTime * exemptionSkillCoolintTime;//计算出公共冷却时间
            //耗魔量
            float holdingRate = SkillStartHoldingTime / BaseSkillStartHoldingTime;//计算蓄力程度
            float maxUseMana = nowIAttributeState.MaxUseMana;//当前的最大耗魔上限
            float baseUseMana = nowIAttributeState.MustUsedBaseMana;//基础耗魔值
            float thisUsedMana = baseUseMana + maxUseMana * holdingRate;//该技能的耗魔值
            iPlayerState.Mana -= thisUsedMana;
            //初始化
            //移除之前的添加的属性(如果存在)
            if (ClearTempCombineSkillAttributeAction != null)
            {
                ClearTempCombineSkillAttributeAction();
                ClearTempCombineSkillAttributeAction = null;
            }
            //初始化蓄力结束数据
            IsSkillStartHolding = false;
            SkillStartHoldingTime = 0;
            //设置动作
            IAnimatorState iAnimatorState = GameState.Instance.GetEntity<IAnimatorState>();
            iAnimatorState.MagicAnimatorType = EnumMagicAnimatorType.Shot;
            #endregion
            //CombineSkills = null;
            return canRelease;
        }
        return false;
    }

    /// <summary>
    /// 一些特殊技能的处理
    /// </summary>
    /// <param name="buffSkills"></param>
    private void SetSpecialSkillSetting(IBuffState iBuffState, IDebuffState iDebuffState, ISpecialState iSpecialState, SkillBaseStruct[] buffSkills)
    {
        if (buffSkills == null)
            return;
        EnumSkillType[] skilltypes = buffSkills.Where(temp => temp != null).Select(temp => temp.skillType).ToArray();
        if (skilltypes.Contains(EnumSkillType.XYX05))//处理光明信仰净化
        {
            iDebuffState.ClearDebuff();
        }
    }

    /// <summary>
    /// 给对象设置正面buff
    /// </summary>
    /// <param name="iBuffState">buff对象</param>
    /// <param name="buffSkills">带有buff的技能数组</param>
    /// <param name="nowIAttributeState">本次技能所使用的状态数据</param>
    private void SetBuff(IBuffState iBuffState, SkillBaseStruct[] buffSkills, IAttributeState nowIAttributeState)
    {
        if (playerState == null)
            return;
        //技能对应的特殊效果数组以及该技能的附加信息(主要是使用附加信息的技能等级对应特效等级)
        var skillEffectsMessage_1 = buffSkills.Select(temp => new
        {
            skill = temp,
            effects = temp.skillStatusEffect,
            skillAttributes = temp.skillAttributeStructs,
            skillLevel = playerState.SkillPoint.ContainsKey(temp.skillType) ? playerState.SkillPoint[temp.skillType] - 1 : -1
        });
        StatusData statusData = DataCenter.Instance.GetMetaData<StatusData>();
        var skillEffectsMessage_2 = skillEffectsMessage_1.Select(temp => new
        {
            skill = temp.skill,
            effectDatas = temp.effects.Select(effect => statusData[effect]).Where(effectData => effectData != null).ToArray(),
            skillAttribute = temp.skillLevel > -1 && temp.skillAttributes.Count() > temp.skillLevel ? temp.skillAttributes[temp.skillLevel] : null
        });
        foreach (var skillEffectsMessage in skillEffectsMessage_2)
        {
            EnumSkillType skillType = skillEffectsMessage.skill.skillType;
            StatusDataInfo.StatusLevelDataInfo[] statusLevelDataInfos = null;
            if (skillEffectsMessage.skillAttribute != null)
            {
                int level = skillEffectsMessage.skillAttribute.SkillSpecialLevel;
                if (level == 0)
                {
                    level = playerState.SkillPoint.ContainsKey(skillType) ? playerState.SkillPoint[skillType] : 0;
                }
                if (level <= 0)
                {
                    continue;
                }
                else
                {
                    //选取出该技能特效的数据
                    statusLevelDataInfos = skillEffectsMessage.effectDatas.Where(temp => temp.MaxLevel >= level).Select(temp => temp[level]).ToArray();
                }
                //每一个技能对应多个特效,这里将每个特效的等级数据都取出来了,需要便利每个特效,然后根据上面计算的等级来确定是否替换数据
                foreach (StatusDataInfo.StatusLevelDataInfo statusLevelDataInfo in statusLevelDataInfos)
                {
                    //新的buff数据
                    BuffState buffState = new BuffState()
                    {
                        level = level,//等级
                        tempData = statusLevelDataInfo,//数据
                        Time = skillEffectsMessage.skillAttribute.RETI//驻留时间
                    };
                    switch (statusLevelDataInfo.EffectType)
                    {
                        case EnumStatusEffect.hl2:
                            if (iBuffState.Huoli.level <= level)//如果当前的buff等级小于现在的buff则替换
                                iBuffState.Huoli = buffState;
                            break;
                        case EnumStatusEffect.js1:
                            if (iBuffState.Jiasu.level <= level)//如果当前的buff等级小于现在的buff则替换
                                iBuffState.Jiasu = buffState;
                            break;
                        case EnumStatusEffect.jh5:
                            if (iBuffState.Jinghua.level <= level)//如果当前的buff等级小于现在的buff则替换
                                iBuffState.Jinghua = buffState;
                            break;
                        case EnumStatusEffect.mj1:
                            if (iBuffState.Minjie.level <= level)//如果当前的buff等级小于现在的buff则替换
                                iBuffState.Minjie = buffState;
                            break;
                        case EnumStatusEffect.ql1:
                            if (iBuffState.Qiangli.level <= level)//如果当前的buff等级小于现在的buff则替换
                                iBuffState.Qiangli = buffState;
                            break;
                        case EnumStatusEffect.qs2:
                            if (iBuffState.Qusan.level <= level)//如果当前的buff等级小于现在的buff则替换
                                iBuffState.Qusan = buffState;
                            break;
                        case EnumStatusEffect.rz1:
                            if (iBuffState.Ruizhi.level <= level)//如果当前的buff等级小于现在的buff则替换
                                iBuffState.Ruizhi = buffState;
                            break;
                        case EnumStatusEffect.xx3:
                            if (iBuffState.XixueWuli.level <= level)//如果当前的buff等级小于现在的buff则替换
                                iBuffState.XixueWuli = buffState;
                            break;
                        case EnumStatusEffect.xx4:
                            if (iBuffState.XixueMofa.level <= level)//如果当前的buff等级小于现在的buff则替换
                                iBuffState.XixueMofa = buffState;
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 释放普通技能,如果正在释放其他技能则无法释放 
    /// </summary>
    /// <param name="skillBaseStruct"></param>
    public bool ReleaseNormalSkill(SkillBaseStruct skillBaseStruct)
    {
        IAnimatorState iAnimatorState = GameState.Instance.GetEntity<IAnimatorState>();
        int thisSkillID = skillBaseStruct != null ? (int)skillBaseStruct.skillType : (int)EnumSkillType.None;
        if (thisSkillID == 0)
            return false;
        if (skillRuntimeCoolingTimeDic == null)
            return false;
        float thisSkillCoolingTime = 0;//技能冷却时间
        if (skillRuntimeCoolingTimeDic.ContainsKey(thisSkillID))
            thisSkillCoolingTime = skillRuntimeCoolingTimeDic[thisSkillID];
        //判断是否在蓄力(魔法组合技能) 是否在公共冷却时间中 是否在技能冷却时间中
        //判断是否在魔法动作(吟唱 释放)
        //判断是否在技能动作
        //判断是否在被攻击中
        //判断是否是死亡状态
        if (!IsSkillStartHolding && PublicCoolingTime <= 0 && thisSkillCoolingTime <= 0
            && !iAnimatorState.IsMagicActionState
            && !iAnimatorState.IsSkillState
            && !iAnimatorState.IsGetHitAnimator
            && !iAnimatorState.IsDeathAnimator
            )
        {
            IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
            SkillStructData skillStructData = DataCenter.Instance.GetMetaData<SkillStructData>();
            IAttributeState baseAttributeState = iPlayerState.GetAttribute(0);//基础数据状态
            if (!iAnimatorState.IsPhycisActionState)
            {
                //处理技能伤害数据以及粒子(粒子上包含技能伤害判定的回调函数)
                //如果是光环技能则更改光环状态
                switch (skillBaseStruct.skillType)
                {
                    case EnumSkillType.MS04://祝福光环
                    case EnumSkillType.MS05://后方支援
                    case EnumSkillType.ZHS04://信仰赋予
                    case EnumSkillType.DSM05://精神集中
                    case EnumSkillType.DSM06://体能突破
                    case EnumSkillType.JS05://神佑光环
                    case EnumSkillType.JH05://敬畏光环
                        SpecialSkillStateStruct spcialSkillStateStruct = GetSpecialSkillStateStruct(skillBaseStruct.skillType);
                        if (playerState.SkillPoint.ContainsKey(skillBaseStruct.skillType))
                        {
                            int level = playerState.SkillPoint[skillBaseStruct.skillType];
                            spcialSkillStateStruct.SkillLevel = level;
                            if (level == 0)
                            {
                                spcialSkillStateStruct.IsOpen = false;
                            }
                            else
                            {
                                spcialSkillStateStruct.IsOpen = !spcialSkillStateStruct.IsOpen;
                            }
                        }
                        break;
                }
                //如果是特殊技能 
                switch (skillBaseStruct.skillType)
                {
                    case EnumSkillType.MFS02://魔偶操控
                                             //需要相关的召唤脚本
                        break;
                    case EnumSkillType.ZHS05://魔力扰乱
                                             //需要相关的关系脚本
                        break;
                    case EnumSkillType.DSM08://传送
                                             //需要摇杆方向数据用于传送地点计算,以及寻路组件用于判断该地点是否合法
                        break;
                    case EnumSkillType.JS09://神秘信仰_降临
                                            //需要先完善召唤物模块
                        break;
                    case EnumSkillType.JH09://神依
                    case EnumSkillType.ZS02://魔剑士
                    case EnumSkillType.GJS02://魔矢
                    case EnumSkillType.YX02://风行
                    case EnumSkillType.DZ03://暗杀术
                        int specialSkillID = (int)skillBaseStruct.skillType;
                        SkillAttributeStruct skillAttributeStruct = iPlayerState.GetSkillAttributeStruct((EnumSkillType)specialSkillID, skillStructData);
                        //魔法值必须够用才行 
                        if (baseAttributeState.Mana > skillAttributeStruct.MustUsedBaseMana && iPlayerState.CreateAttributeHandle(-specialSkillID))
                        {
                            IAttributeState specialSkill_AttributeState = iPlayerState.GetAttribute(-specialSkillID);
                            iPlayerState.SetIAttributeStateDataBySkillData(specialSkill_AttributeState, baseAttributeState, skillAttributeStruct);
                            float specialSkill_StateTime = specialSkill_AttributeState.EffectAffine;//特效影响力表示的技能持续时间
                            specialSkill_AttributeState.EffectAffine = 0;//将其归零(为了不影响别的模块)
                            float mustUseMana = specialSkill_AttributeState.MustUsedBaseMana;//本技能的耗魔
                            specialSkill_AttributeState.MustUsedBaseMana = 0;//将其归零(为了不影响别的模块)
                            RunTaskStruct runTaskStruct = TaskTools.Instance.GetRunTaskStruct();//获取一个任务
                            runTaskStruct.StartTask(specialSkill_StateTime,
                                () => //用于移除该状态
                                {
                                    iPlayerState.RemoveAttribute(specialSkillID);
                                }
                                , 1, false);
                            //设置冷却时间
                            if (skillRuntimeCoolingTimeDic != null)
                            {
                                float specialSkillCoolingTime = specialSkill_AttributeState.CoolingTime;
                                if (skillRuntimeCoolingTimeDic.ContainsKey(specialSkillID))
                                    skillRuntimeCoolingTimeDic[specialSkillID] = specialSkillCoolingTime;
                                else
                                    skillRuntimeCoolingTimeDic.Add(specialSkillID, specialSkillCoolingTime);
                            }
                            //设置耗魔
                            iPlayerState.Mana -= mustUseMana;
                            //特殊技能的特有处理
                            if (skillBaseStruct.skillType == EnumSkillType.DZ03)//如果是暗杀术,则必须清楚所有内置数据(因为暗杀术是特殊的处理,这里仅仅使用了他的buff状态以及时间)
                            {
                                specialSkill_AttributeState.Init();
                            }
                        }
                        break;
                }
                //如果是物理攻击技能
                if (skillBaseStruct.skillType > EnumSkillType.SpecialPhysicActionReleaseStart && skillBaseStruct.skillType < EnumSkillType.SpecialPhysicReleaseEnd)
                {
                    //统一的处理(构建当前状态对象)
                    int physicSkillID = (int)skillBaseStruct.skillType;
                    SkillAttributeStruct skillAttributeStruct = iPlayerState.GetSkillAttributeStruct((EnumSkillType)physicSkillID, skillStructData);
                    //魔法值必须够用才行 
                    if (baseAttributeState.Mana >= skillAttributeStruct.MustUsedBaseMana && iPlayerState.CreateAttributeHandle(-physicSkillID))
                    {
                        //共有设置
                        IAttributeState physic_AttributeState = iPlayerState.GetAttribute(-physicSkillID);
                        physic_AttributeState.SetRoleOfRaceAddition(iPlayerState.SelfRoleOfRaceInfoStruct);//设置种族成长属性
                        iPlayerState.SetIAttributeStateDataBySkillData(physic_AttributeState, baseAttributeState, skillAttributeStruct);
                        IAttributeState thisPhysicsAttributeState = iPlayerState.GetResultAttribute();
                        iPlayerState.RemoveAttribute(-physicSkillID);
                        //私有设置
                        //判断武器是否复合
                        EnumWeaponTypeByPlayerState weaponTypeByPlayerState = iPlayerState.WeaponTypeByPlayerState;//武器类型
                        weaponTypeByPlayerState = weaponTypeByPlayerState | EnumWeaponTypeByPlayerState.Shield - EnumWeaponTypeByPlayerState.Shield;//去除盾牌
                        switch (skillBaseStruct.skillType)
                        {
                            case EnumSkillType.WL01://重击
                            case EnumSkillType.ZS03://冲锋
                            case EnumSkillType.KZS03://战吼
                                                     //这些不需要判断武器
                                break;
                            case EnumSkillType.GJS03://散射
                            case EnumSkillType.SSS03://狙击术
                                                     //判断是否是弓类武器
                                if (weaponTypeByPlayerState != EnumWeaponTypeByPlayerState.Arch)
                                    return false;
                                break;
                            case EnumSkillType.JAS03://燕返
                                                     //判断是否是如下近战武器
                                if (//weaponTypeByPlayerState != EnumWeaponTypeByPlayerState.Dagger&&
                                    // weaponTypeByPlayerState != EnumWeaponTypeByPlayerState.LongRod&&
                                    // weaponTypeByPlayerState != EnumWeaponTypeByPlayerState.ShortRod&&
                                       weaponTypeByPlayerState != EnumWeaponTypeByPlayerState.SingleHandedSword &&
                                        weaponTypeByPlayerState != EnumWeaponTypeByPlayerState.TwoHandedSword)
                                {
                                    return false;
                                }
                                break;
                            default:
                                return false;

                        }
                        //.....................//
                        //共有设置
                        //设置冷却时间
                        if (skillRuntimeCoolingTimeDic != null)
                        {
                            float physicSkillCoolingTime = physic_AttributeState.CoolingTime;
                            if (skillRuntimeCoolingTimeDic.ContainsKey(physicSkillID))
                                skillRuntimeCoolingTimeDic[physicSkillID] = physicSkillCoolingTime;
                            else
                                skillRuntimeCoolingTimeDic.Add(physicSkillID, physicSkillCoolingTime);
                        }
                        //设置耗魔量
                        iPlayerState.Mana -= physic_AttributeState.MustUsedBaseMana;
                        //构建当前物理技能的数据
                        PhysicsSkillStateStruct physicsSkillStateStruct = new PhysicsSkillStateStruct()
                        {
                            SkillType = skillBaseStruct.skillType,
                            AttributeState = thisPhysicsAttributeState,
                            StorageSchedule = 0//暂时没有蓄力进度
                        };
                        //设置技能动作
                        iAnimatorState.PhysicAnimatorSkillType = skillBaseStruct.skillType;
                        iAnimatorState.PhysicAnimatorType = EnumPhysicAnimatorType.Skill;
                        //设置持续
                        iAnimatorState.SkillSustainable = true;
                        //交给IDamge脚本处理伤害以及开关粒子
                        IDamage iDamage = GameState.Instance.GetEntity<IDamage>();
                        iDamage.SetPhysicSkillAttack(iPlayerState, physicsSkillStateStruct, skillBaseStruct.skillType, weaponTypeByPlayerState);
                    }
                }
            }
            //如果是物理普通攻击 (必须等普通攻击的冷却归零) 如果当前状态是move或者攻击的指定段位
            bool canNormalAttack = false;
            if (iAnimatorState.AnimationClipTypeState != null)
            {
                AnimationClipTypeState animationClipTypeState = iAnimatorState.AnimationClipTypeState;
                if (animationClipTypeState != null)
                {
                    if (animationClipTypeState.AnimationClipType == EnumAnimationClipType.Move)
                        canNormalAttack = true;
                    else if (animationClipTypeState.AnimationClipType == EnumAnimationClipType.Attack1 ||
                        animationClipTypeState.AnimationClipType == EnumAnimationClipType.Attack2 ||
                        animationClipTypeState.AnimationClipType == EnumAnimationClipType.Attack3)
                    {
                        if (animationClipTypeState.TimeType == EnumAnimationClipTimeType.Start)
                        {
                            canNormalAttack = true;
                        }
                    }
                }
            }
            if (skillBaseStruct.skillType == EnumSkillType.PhysicAttack && normalAttackCoolintTime <= 0 && canNormalAttack)
            {
                switch (iAnimatorState.PhycisActionNowType)
                {
                    case 0://当前第零阶段,可以进入第一段
                        normalAttackCoolintTime = 0.5f;
                        break;
                    case 1://当前第一阶段,可以进入第二段
                        normalAttackCoolintTime = 0.5f;
                        break;
                    case 2://当前第二阶段,可以进入第三段
                        normalAttackCoolintTime = 1f;
                        break;
                    default:
                        return false;
                }
                IAttributeState thisAttackAttributeState = iPlayerState.GetResultAttribute();
                EnumWeaponTypeByPlayerState weaponTypeByPlayerState = iPlayerState.WeaponTypeByPlayerState;//武器类型
                weaponTypeByPlayerState = weaponTypeByPlayerState | EnumWeaponTypeByPlayerState.Shield - EnumWeaponTypeByPlayerState.Shield;//去除盾牌
                //如果没有武器则不可以攻击
                if (weaponTypeByPlayerState == EnumWeaponTypeByPlayerState.None)
                    return false;
                //设置攻击动作
                iAnimatorState.PhysicAnimatorType = EnumPhysicAnimatorType.Normal;
                //交给IDamage脚本处理伤害以及开关粒子
                IDamage iDamage = GameState.Instance.GetEntity<IDamage>();
                iDamage.SetNormalAttack(iPlayerState, iAnimatorState.PhycisActionNowType + 1, thisAttackAttributeState, weaponTypeByPlayerState);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取技能运行时的当前剩余冷却时间
    /// </summary>
    /// <param name="skillID">技能id(包括组合技能)</param>
    /// <returns></returns>
    public float GetSkillRuntimeCoolingTime(int skillID)
    {
        if (skillRuntimeCoolingTimeDic != null && skillRuntimeCoolingTimeDic.ContainsKey(skillID))
            return skillRuntimeCoolingTimeDic[skillID];
        return 0;
    }

    /// <summary>
    /// 获取技能最大的冷却时间
    /// </summary>
    /// <param name="skillID"></param>
    /// <returns></returns>
    public float GetSkillMaxCoolingTime(int skillID)
    {
        if (skillMaxCoolingTimeDic != null && skillMaxCoolingTimeDic.ContainsKey(skillID))
            return skillMaxCoolingTimeDic[skillID];
        return 0;
    }

    /// <summary>
    /// 设置技能的冷却时间(注意该函数会触发GetSkillCoolingTime的回调,但不会触发自身的回调)
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public void SetSkillCoolingTime(int skillID, float time)
    {
        if (skillRuntimeCoolingTimeDic != null)
        {
            if (!skillRuntimeCoolingTimeDic.ContainsKey(skillID))
                skillRuntimeCoolingTimeDic.Add(skillID, 0);
            float oldCoolingTime = skillRuntimeCoolingTimeDic[skillID];
            skillRuntimeCoolingTimeDic[skillID] = time;
            if (skillMaxCoolingTimeDic != null)//释放技能后当前技能的最大冷却时间
            {
                if (skillMaxCoolingTimeDic.ContainsKey(skillID))
                    skillMaxCoolingTimeDic[skillID] = time;
                else skillMaxCoolingTimeDic.Add(skillID, time);
            }
            if (oldCoolingTime != time)
                Call<ISkillState, Func<int, float>>(temp => temp.GetSkillRuntimeCoolingTime);
        }
    }

    #region  光环技能状态

    /// <summary>
    /// 光环技能状态字典
    /// </summary>
    private Dictionary<EnumSkillType, SpecialSkillStateStruct> specialSkillStateStructDic;

    /// <summary>
    /// 初始化光环技能状态结构
    /// </summary>
    private void InitSpecialSkillStateStruct()
    {
        specialSkillStateStructDic = new Dictionary<EnumSkillType, SpecialSkillStateStruct>();
        Action<EnumSkillType> AddSpecialSkillToDic = (skillType) =>
        {
            SkillStructData skillStructData = DataCenter.Instance.GetMetaData<SkillStructData>();
            SkillBaseStruct skillBaseStruct = skillStructData.SearchSkillDatas(temp => temp.skillType == skillType).FirstOrDefault();
            if (skillBaseStruct != null)
            {
                SpecialSkillStateStruct specialSkillStateStruct = new SpecialSkillStateStruct(skillType, false, -1, skillBaseStruct,
                    (changedData) =>
                    {
                        if (specialSkillStateStructDic.ContainsKey(changedData.SkillType))
                        {
                            specialSkillStateStructDic[changedData.SkillType] = changedData;
                            //通知其他地方
                            SpecialSkillStateChanged = changedData.SkillType;
                        }
                    });
                specialSkillStateStructDic.Add(skillType, specialSkillStateStruct);
            }
        };
        //添加光环技能到字典
        AddSpecialSkillToDic(EnumSkillType.MS04);
        AddSpecialSkillToDic(EnumSkillType.MS05);
        AddSpecialSkillToDic(EnumSkillType.ZHS04);
        AddSpecialSkillToDic(EnumSkillType.DSM05);
        AddSpecialSkillToDic(EnumSkillType.DSM06);
        AddSpecialSkillToDic(EnumSkillType.JS05);
        AddSpecialSkillToDic(EnumSkillType.JH05);
    }

    /// <summary>
    /// 获取指定光环技能数据
    /// </summary>
    /// <param name="skillType">技能类型</param>
    /// <returns></returns>
    public SpecialSkillStateStruct GetSpecialSkillStateStruct(EnumSkillType skillType)
    {
        if (specialSkillStateStructDic.ContainsKey(skillType))
            return specialSkillStateStructDic[skillType];
        else
            return default(SpecialSkillStateStruct);
    }

    /// <summary>
    /// 当前正在发生变化的光环技能
    /// </summary>
    EnumSkillType _SpecialSkillStateChanged;
    /// <summary>
    /// 获取当前正在发生变化的光环技能
    /// </summary>
    public EnumSkillType SpecialSkillStateChanged
    {
        get { return _SpecialSkillStateChanged; }
        private set
        {
            EnumSkillType tempSpecialSkillStateChanged = _SpecialSkillStateChanged;
            _SpecialSkillStateChanged = value;
            if (tempSpecialSkillStateChanged != _SpecialSkillStateChanged)
            {
                Call<ISkillState, EnumSkillType>(temp => temp.SpecialSkillStateChanged);
            }
            _SpecialSkillStateChanged = EnumSkillType.None;
        }
    }
    #endregion

    #region 物理技能状态
    /// <summary>
    /// 当前物理技能的状态数据
    /// </summary>
    private PhysicsSkillStateStruct _NowPhysicsSkillStateStruct;
    /// <summary>
    /// 获取当前物理技能的状态数据
    /// </summary>
    public PhysicsSkillStateStruct NowPhysicsSkillStateStruct
    {
        get { return _NowPhysicsSkillStateStruct; }
        private set
        {
            PhysicsSkillStateStruct tempNowPhysicsSkillStateStruct = _NowPhysicsSkillStateStruct;
            _NowPhysicsSkillStateStruct = value;
            if (tempNowPhysicsSkillStateStruct != _NowPhysicsSkillStateStruct)
                Call<ISkillState, PhysicsSkillStateStruct>(temp => temp.NowPhysicsSkillStateStruct);
        }
    }

    #endregion
}

