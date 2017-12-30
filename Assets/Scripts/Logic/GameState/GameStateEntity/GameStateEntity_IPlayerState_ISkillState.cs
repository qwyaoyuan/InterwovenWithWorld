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
    partial void Load_IPlayerState_ISkillState()
    {
        _CombineSkills = new SkillBaseStruct[4];
        _IsSkillStartHolding = false;
        _SkillStartHoldingTime = 0;
        InitSpecialSkillStateStruct();
    }

    /// <summary>
    /// 组合技能数组
    /// </summary>
    SkillBaseStruct[] _CombineSkills;

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

    partial void Update_IPlayerState_ISkillState()
    {
        if (IsSkillStartHolding)
        {
            SkillStartHoldingTime += Time.deltaTime;
        }
        if (PublicCoolingTime > 0)
            PublicCoolingTime -= Time.deltaTime;
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
        if (canRelease)
        {
            IsSkillStartHolding = true;
            SkillStartHoldingTime = 0;
        }
        return canRelease;
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
            //处理技能伤害数据以及粒子(粒子上包含技能伤害判定的回调函数)
            if (canRelease)
            {
                //判断这个是攻击用途还是辅助用途
                SkillBaseStruct combine_secondSkill = _CombineSkills.FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel2Start && temp.skillType < EnumSkillType.MagicCombinedLevel2End);
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
                    SkillStructData skillStructData = DataCenter.Instance.GetMetaData<SkillStructData>();
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
                    IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
                    if (attackType)//如果是攻击用途则实例化粒子
                    {
                        IDamage iDamage = GameState.Instance.GetEntity<IDamage>();
                        ParticalInitParamData[] particalInitParamDatas = iDamage.GetParticalInitParamData(iPlayerState.PlayerObj, _CombineSkills);
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
                            SetBuff(iBuffState, _CombineSkills);
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
                                        SetBuff(targetIBuffState, _CombineSkills);
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
                               0,//这里的层是玩家和召唤物
                               ThisCallBack,//如果是buff则这里会有回调
                               1);

                    }
                }
            }
            PublicCoolingTime = 2;//公共冷却时间为1秒
            //初始化
            IsSkillStartHolding = false;
            SkillStartHoldingTime = 0;
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
    private void SetBuff(IBuffState iBuffState, SkillBaseStruct[] buffSkills)
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
        if (IsSkillStartHolding)
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
                    //需要先完善技能类的基础功能以及角色属性类的基础功能
                    break;
            }
            //物理技能
            return true;
        }
        return false;
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
                Call<ISkillState, EnumSkillType>(temp => temp.SpecialSkillStateChanged);
            _SpecialSkillStateChanged = EnumSkillType.None;
        }
    }
    #endregion
}

