using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 实现了IPlayerState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState : IPlayerState
{
    /// <summary>
    /// 玩家状态接口实现对象的开始函数
    /// </summary>
    partial void Start_IPlayerState()
    {
        //注册监听光环状态发生变化
        GameState.Instance.Registor<ISkillState>(ISkillState_Changed);
    }

    /// <summary>
    /// 玩家状态接口实现对象的加载函数
    /// </summary>
    partial void Load_IPlayerState()
    {
        _Level = playerState.Level;
        _Experience = playerState.Experience;
    }

    /// <summary>
    /// 技能状态对象发生变化
    /// </summary>
    /// <param name="iSkillState"></param>
    /// <param name="fieldName"></param>
    private void ISkillState_Changed(ISkillState iSkillState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<ISkillState, EnumSkillType>(temp => temp.SpecialSkillStateChanged)))
        {
            EnumSkillType skillType = iSkillState.SpecialSkillStateChanged;//状态改变的技能 
            SpecialSkillStateStruct specialSkillStateSTruct = iSkillState.GetSpecialSkillStateStruct(skillType);//获取该光环的数据
            IPlayerAttributeState iPlayerAttributeState = GameState.Instance.GetEntity<IPlayerAttributeState>();
            int handle = -(int)skillType;
            if (specialSkillStateSTruct.IsOpen)//如果当前需要打开
            {
                IAttributeState iAttributeState = iPlayerAttributeState.GetAttribute(handle);
                if (iAttributeState == null)
                {
                    iPlayerAttributeState.CreateAttributeHandle(handle);
                    UpdateAttribute();
                }
            }
            else//当前如果需要关闭
            {
                IAttributeState iAttributeState = iPlayerAttributeState.GetAttribute(handle);
                if (iAttributeState != null)
                {
                    iPlayerAttributeState.RemoveAttribute(handle);
                    UpdateAttribute();
                }
            }
        }
    }

    /// <summary>
    /// 每帧更新
    /// </summary>
    partial void Update_IPlayerState()
    {
        UpdateCheckChanChangingAttribute();
    }

    #region IPlayerState的自身状态
    /// <summary>
    /// 玩家操纵角色的游戏对象 
    /// </summary>
    GameObject _PlayerObj;
    /// <summary>
    /// 玩家操纵角色的游戏对象
    /// </summary>
    public GameObject PlayerObj
    {
        get
        {
            return _PlayerObj;
        }
        set
        {
            GameObject tempPlayObj = _PlayerObj;
            _PlayerObj = value;
            if (!GameObject.Equals(tempPlayObj, _PlayerObj))
                Call<IPlayerState, GameObject>(temp => temp.PlayerObj);
        }
    }

    /// <summary>
    /// 玩家的摄像机
    /// </summary>
    Camera _PlayerCamera;
    /// <summary>
    /// 玩家的摄像机
    /// </summary>
    public Camera PlayerCamera
    {
        get
        {
            return _PlayerCamera;
        }
        set
        {
            Camera tempPlayerCamera = _PlayerCamera;
            _PlayerCamera = value;
            if (!Camera.Equals(tempPlayerCamera, _PlayerCamera))
                Call<IPlayerState, Camera>(temp => temp.PlayerCamera);
        }
    }

    /// <summary>
    /// 物理检测脚本
    /// </summary>
    private PhysicSkillInjuryDetection _PhysicSkillInjuryDetection;
    /// <summary>
    /// 物理检测脚本
    /// </summary>
    public PhysicSkillInjuryDetection PhysicSkillInjuryDetection
    {
        get { return _PhysicSkillInjuryDetection; }
        set
        {
            PhysicSkillInjuryDetection tempPhysicSkillInjuryDetection = _PhysicSkillInjuryDetection;
            _PhysicSkillInjuryDetection = value;
            if (!object.Equals(tempPhysicSkillInjuryDetection, _PhysicSkillInjuryDetection))
                Call<IPlayerState, PhysicSkillInjuryDetection>(temp => temp.PhysicSkillInjuryDetection);
        }
    }

    /// <summary>
    /// 当前选择的目标
    /// </summary>
    private GameObject _SelectObj;
    /// <summary>
    /// 当前选择的目标 
    /// </summary>
    public GameObject SelectObj
    {
        get { return _SelectObj; }
        set
        {
            GameObject tempSelectObj = _SelectObj;
            _SelectObj = value;
            if (!GameObject.Equals(tempSelectObj, _SelectObj))
                Call<IPlayerState, GameObject>(temp => temp.SelectObj);
        }
    }

    /// <summary>
    /// (每一帧都)检测变化的状态
    /// 比如暴击时产生效果,一定时间内产生效果等属性 
    /// </summary>
    private void UpdateCheckChanChangingAttribute()
    {
        if (playerState == null)
            return;
        SkillStructData skillStructData = DataCenter.Instance.GetMetaData<SkillStructData>();
        IPlayerAttributeState iPlayerAttributeState = GameState.Instance.GetEntity<IPlayerAttributeState>();
        //处理等级以及加点影响的基础属性 
        IAttributeState iAttributeState_Base = iPlayerAttributeState.GetAttribute(0);
        //检测武器状态,根据武器状态有些被动的效果会不同
        //首先与盾牌进行异或运算
        EnumWeaponTypeByPlayerState weaponType_Right = WeaponTypeByPlayerState ^ EnumWeaponTypeByPlayerState.Shield;//去除盾牌                                                                                                                 
        int handle_JZQH = -(int)EnumSkillType.ZS04;//近战强化
        int handle_JZZJ = -(int)EnumSkillType.KZS02;//近战专精 
        int handle_YCQH = -(int)EnumSkillType.GJS04;//远程强化 
        int handle_YCZJ = -(int)EnumSkillType.SSS01;//远程专精 
        switch (weaponType_Right)
        {
            //近战强化 近战专精
            case EnumWeaponTypeByPlayerState.SingleHandedSword:
            case EnumWeaponTypeByPlayerState.TwoHandedSword:
            case EnumWeaponTypeByPlayerState.Dagger:
                //近战强化
                if (iPlayerAttributeState.GetAttribute(handle_JZQH) == null)
                {
                    SkillAttributeStruct skillAttributeStruct = GetSkillAttributeStruct(EnumSkillType.ZS04, skillStructData);
                    iPlayerAttributeState.CreateAttributeHandle(handle_JZQH);
                    IAttributeState iAttributeState = iPlayerAttributeState.GetAttribute(handle_JZQH);
                    SetIAttributeStateDataBySkillData(iAttributeState, iAttributeState_Base, skillAttributeStruct);
                }
                //近战专精
                if (iPlayerAttributeState.GetAttribute(handle_JZZJ) == null)
                {
                    SkillAttributeStruct skillAttributeStruct = GetSkillAttributeStruct(EnumSkillType.KZS02, skillStructData);
                    iPlayerAttributeState.CreateAttributeHandle(handle_JZZJ);
                    IAttributeState iAttributeState = iPlayerAttributeState.GetAttribute(handle_JZZJ);
                    SetIAttributeStateDataBySkillData(iAttributeState, iAttributeState_Base, skillAttributeStruct);
                }
                break;
            //远程强化 远程专精
            case EnumWeaponTypeByPlayerState.Arch:
            case EnumWeaponTypeByPlayerState.CrossBow:
                //远程强化
                if (iPlayerAttributeState.GetAttribute(handle_YCQH) == null)
                {
                    SkillAttributeStruct skillAttributeStruct = GetSkillAttributeStruct(EnumSkillType.GJS04, skillStructData);
                    iPlayerAttributeState.CreateAttributeHandle(handle_YCQH);
                    IAttributeState iAttributeState = iPlayerAttributeState.GetAttribute(handle_YCQH);
                    SetIAttributeStateDataBySkillData(iAttributeState, iAttributeState_Base, skillAttributeStruct);
                }
                //远程专精
                if (iPlayerAttributeState.GetAttribute(handle_YCZJ) == null)
                {
                    SkillAttributeStruct skillAttributeStruct = GetSkillAttributeStruct(EnumSkillType.SSS01, skillStructData);
                    iPlayerAttributeState.CreateAttributeHandle(handle_YCZJ);
                    IAttributeState iAttributeState = iPlayerAttributeState.GetAttribute(handle_YCZJ);
                    SetIAttributeStateDataBySkillData(iAttributeState, iAttributeState_Base, skillAttributeStruct);
                }
                break;
            default:
                if (iPlayerAttributeState.GetAttribute(handle_JZQH) != null)
                    iPlayerAttributeState.RemoveAttribute(handle_JZQH);
                if (iPlayerAttributeState.GetAttribute(handle_JZZJ) != null)
                    iPlayerAttributeState.RemoveAttribute(handle_JZZJ);
                if (iPlayerAttributeState.GetAttribute(handle_YCQH) != null)
                    iPlayerAttributeState.RemoveAttribute(handle_YCQH);
                if (iPlayerAttributeState.GetAttribute(handle_YCZJ) != null)
                    iPlayerAttributeState.RemoveAttribute(handle_YCZJ);
                break;
        }
        //检测距离上一次暴击的时间
        if (LastCriticalHitTime > 0)
        {
            float intervalCriticalHitTime = Time.time - LastCriticalHitTime;//距离上次暴击的时间的间隔
            //剑意
            SkillAttributeStruct skillAttributeStruct_JAS02 = GetSkillAttributeStruct(EnumSkillType.JAS02, skillStructData);
            int handle_JAS02 = -(int)EnumSkillType.JAS02;
            IAttributeState iAttributeState_JAS02 = iPlayerAttributeState.GetAttribute(handle_JAS02);
            if (skillAttributeStruct_JAS02 != null)//表示存在该技能或者该技能加点了
            {
                int waitTime_JAS02 = skillAttributeStruct_JAS02.ERST;//使用特效影响力表示持续时间
                if (iAttributeState_JAS02 != null)
                    if (waitTime_JAS02 > intervalCriticalHitTime)//在效果时间内,则更新攻击速度
                        iAttributeState_JAS02.AttackSpeed = skillAttributeStruct_JAS02.AttackSpeed;
                    else//在效果时间外,则归零
                        iAttributeState_JAS02.CritRate = 0;
            }
            else
            {
                //如果没有数据则直接归零
                if (iAttributeState_JAS02 != null)
                    iAttributeState_JAS02.CritRate = 0;
            }
            //巧手夺宝
            SkillAttributeStruct skillAttributeStruct_DZ01 = GetSkillAttributeStruct(EnumSkillType.DZ01, skillStructData);
            int handle_DZ01 = -(int)EnumSkillType.DZ01;
            IAttributeState iAttributeState_DZ01 = iPlayerAttributeState.GetAttribute(handle_DZ01);
            if (skillAttributeStruct_DZ01 != null)//表示存在该技能或者该技能加点了
            {
                int waitTime_DZ01 = skillAttributeStruct_DZ01.ERST;//使用特效影响力表示持续时间
                if (iAttributeState_DZ01 != null)
                    if (waitTime_DZ01 > intervalCriticalHitTime)//在效果时间内,则更新附加伤害(根据敏捷附加)
                    {
                        iAttributeState_DZ01.PhysicsAdditionalDamage = skillAttributeStruct_DZ01.PHYAD * iAttributeState_Base.Quick / 100;
                        iAttributeState_DZ01.GooodsDropRate = iAttributeState_Base.GooodsDropRate * skillAttributeStruct_DZ01.GooodsDropRate / 100;
                    }
                    else //在效果时间外,则归零
                    {
                        iAttributeState_DZ01.PhysicsAdditionalDamage = 0;
                    }
            }
            else
            {
                //如果没有数据则直接归零
                if (iAttributeState_DZ01 != null)
                    iAttributeState_DZ01.Init();
            }
        }

        //检测上一次闪避的时间
        if (LastDodgeTime > 0)
        {
            float intervalDodgeTime = Time.time - LastDodgeTime;//距离上次闪避的时间的间隔
            //剑意
            SkillAttributeStruct skillAttributeStruct_JAS02 = GetSkillAttributeStruct(EnumSkillType.JAS02, skillStructData);
            int handle_JAS02 = -(int)EnumSkillType.JAS02;
            IAttributeState iAttributeState_JAS02 = iPlayerAttributeState.GetAttribute(handle_JAS02);
            if (skillAttributeStruct_JAS02 != null)
            {
                int waitTime_JAS02 = skillAttributeStruct_JAS02.ERST;//使用特效影响力表示持续时间
                if (iAttributeState_JAS02 != null)
                    if (waitTime_JAS02 < intervalDodgeTime)//在效果时间内,则更新攻击速度
                        iAttributeState_JAS02.PhysicsPenetrate = skillAttributeStruct_JAS02.PYEDMG;
                    else//在效果时间外,则归零
                        iAttributeState_JAS02.PhysicsPenetrate = 0;
            }
            else
            {
                //如果没有数据则直接归零
                if (iAttributeState_JAS02 != null)
                    iAttributeState_JAS02.PhysicsPenetrate = 0;
            }
        }

        //检测上一次切换武器类型的时间
        if (LastChangeWeaponTime > 0)
        {
            float intervalChangeWeaponTime = Time.time - LastChangeWeaponTime;//距离上次切换武器类型的时间的间隔
            //巧手
            SkillAttributeStruct skillAttributeStruct_DZ02 = GetSkillAttributeStruct(EnumSkillType.DZ02, skillStructData);
            int handle_DZ02 = -(int)EnumSkillType.DZ02;
            IAttributeState iAttributeState_DZ02 = iPlayerAttributeState.GetAttribute(handle_DZ02);
            if (skillAttributeStruct_DZ02 != null)
            {
                int waitTime_DZ02 = skillAttributeStruct_DZ02.ERST;//使用特效影响力表示持续时间
                if (iAttributeState_DZ02 != null)
                    if (waitTime_DZ02 < intervalChangeWeaponTime) //在效果时间内,则更新暴击率和暴击伤害
                    {
                        iAttributeState_DZ02.CritRate = skillAttributeStruct_DZ02.IncreasedCritRate;
                        iAttributeState_DZ02.CritDamageRatio = skillAttributeStruct_DZ02.CritDamagePromotion / 100f;
                    }
                    else //在效果时间外,则归零
                        iAttributeState_DZ02.Init();
            }
            else
            {
                //如果没有数据则直接归零
                if (iAttributeState_DZ02 != null)
                    iAttributeState_DZ02.Init();
            }
        }

        //检测距离上一次进入战斗的时间
        if (LastIntoBattleTime > 0)
        {
            //进入战斗后状态消失
            IAttributeState iAttributeState_YX01 = iPlayerAttributeState.GetAttribute(-(int)EnumSkillType.YX01);
            SkillAttributeStruct skillAttributeStruct_YX01 = GetSkillAttributeStruct(EnumSkillType.YX01, skillStructData);
            if (skillAttributeStruct_YX01 != null)
            {
                if (iAttributeState_YX01 != null)
                {
                    iAttributeState_YX01.MoveSpeed = 0;
                    iAttributeState_YX01.TrapDefense = skillAttributeStruct_YX01.TrapDefense;
                    iAttributeState_YX01.AbnormalStateResistance = skillAttributeStruct_YX01.AbnormalStateResistance;
                }
            }
            else
            {
                if (iAttributeState_YX01 != null)
                    iAttributeState_YX01.Init();
            }
            //进入战斗后一段时间内获得状态
            float intervalIntoBattleTime = Time.time - LastIntoBattleTime;//距离上次进入战斗的时间的间隔
            //游击
            SkillAttributeStruct skillAttributeStruct_GJS01 = GetSkillAttributeStruct(EnumSkillType.GJS01, skillStructData);
            int handle_GJS01 = -(int)EnumSkillType.GJS01;
            IAttributeState iAttributeState_GJS01 = iPlayerAttributeState.GetAttribute(handle_GJS01);
            if (skillAttributeStruct_GJS01 != null)
            {
                int waitTime_GJS01 = skillAttributeStruct_GJS01.ERST;//使用特效影响力表示持续时间
                if (iAttributeState_GJS01 != null)
                    if (waitTime_GJS01 < intervalIntoBattleTime)//在效果时间内,则更新攻击速度和移动速度 
                    {
                        iAttributeState_GJS01.AttackSpeed = skillAttributeStruct_GJS01.AttackSpeed;
                        iAttributeState_GJS01.MoveSpeed = skillAttributeStruct_GJS01.MoveSpeedAddtion * iAttributeState_Base.MoveSpeed / 100f;
                    }
                    else //在效果时间外,则归零
                        iAttributeState_GJS01.Init();
            }
            else
            {
                //如果没有数据则直接归零
                if (iAttributeState_GJS01 != null)
                    iAttributeState_GJS01.Init();
            }
        }
        //距离离开战斗的时间
        else if (LastIntoBattleTime < 0)
        {
            //离开战斗后buff消失
            //游击
            IAttributeState iAttributeState_GJS01 = iPlayerAttributeState.GetAttribute(-(int)EnumSkillType.GJS01);
            if (iAttributeState_GJS01 != null)
                iAttributeState_GJS01.Init();
            //离开战斗后立刻获得的状态
            IAttributeState iAttributeState_YX01 = iPlayerAttributeState.GetAttribute(-(int)EnumSkillType.YX01);
            SkillAttributeStruct skillAttributeStruct_YX01 = GetSkillAttributeStruct(EnumSkillType.YX01, skillStructData);
            if (skillAttributeStruct_YX01 != null)
            {
                if (iAttributeState_YX01 != null)
                {
                    iAttributeState_YX01.MoveSpeed = iAttributeState_Base.MoveSpeed * skillAttributeStruct_YX01.MoveSpeedAddtionNotFighting / 100f;
                    iAttributeState_YX01.TrapDefense = skillAttributeStruct_YX01.TrapDefense;
                    iAttributeState_YX01.AbnormalStateResistance = skillAttributeStruct_YX01.AbnormalStateResistance;
                }
            }
            else
            {
                if (iAttributeState_YX01 != null)
                    iAttributeState_YX01.Init();
            }
            //离开战斗一段时间后获得的状态
            float intevalOutBattleTime = Time.time + LastIntoBattleTime;//距离上次离开战斗的时间间隔
        }

    }

    /// <summary>
    /// 更新自身属性
    /// 在等级变化 装备变化时触发
    /// 主要更新的是HP MP上限,防御攻击等等随等级装备变化的属性等
    /// </summary>
    public void UpdateAttribute()
    {
        SkillStructData skillStructData = DataCenter.Instance.GetMetaData<SkillStructData>();
        IPlayerAttributeState iPlayerAttributeState = GameState.Instance.GetEntity<IPlayerAttributeState>();
        //处理等级以及加点影响的基础属性 
        IAttributeState iAttributeState_Base = iPlayerAttributeState.GetAttribute(0);
        if (iAttributeState_Base != null)
        {
            int level = playerState.Level;
            //当前计算方式,基础数据各项值为10,每升一级增加2
            iAttributeState_Base.Quick = 10 + level * 2 + playerState.Agility;
            iAttributeState_Base.Mental = 10 + level * 2 + playerState.Spirit;
            iAttributeState_Base.Power = 10 + level * 2 + playerState.Strength;
        }
        //处理被动技能和光环技能等级造成的属性变化,以及处理光环技能开关状态造成的属性变化
        Action<EnumSkillType> CheckSkillChanged = (skillType) =>
        {
            int handle = -(int)skillType;
            IAttributeState iAttributeState = iPlayerAttributeState.GetAttribute(handle);
            if (iAttributeState != null)//如果没有目标被动则需要修改初始化模块)
            {
                SkillAttributeStruct skillAttributeStruct = GetSkillAttributeStruct(skillType, skillStructData);
                SetIAttributeStateDataBySkillData(iAttributeState, iAttributeState_Base, skillAttributeStruct);
            }
        };
        //光环技能 
        CheckSkillChanged(EnumSkillType.MS04);
        CheckSkillChanged(EnumSkillType.MS05);
        CheckSkillChanged(EnumSkillType.ZHS04);
        CheckSkillChanged(EnumSkillType.DSM05);
        CheckSkillChanged(EnumSkillType.DSM06);
        CheckSkillChanged(EnumSkillType.JS05);
        CheckSkillChanged(EnumSkillType.JH05);
        //被动技能
        //魔法
        CheckSkillChanged(EnumSkillType.FS05);
        CheckSkillChanged(EnumSkillType.FS06);
        CheckSkillChanged(EnumSkillType.FS07);
        CheckSkillChanged(EnumSkillType.FS08);
        CheckSkillChanged(EnumSkillType.FS10);
        CheckSkillChanged(EnumSkillType.YSX05);
        CheckSkillChanged(EnumSkillType.YSX08);
        CheckSkillChanged(EnumSkillType.MFS03);
        CheckSkillChanged(EnumSkillType.MFS07);
        CheckSkillChanged(EnumSkillType.SM01);
        CheckSkillChanged(EnumSkillType.SM05);
        CheckSkillChanged(EnumSkillType.MS07);
        CheckSkillChanged(EnumSkillType.DFS01);
        CheckSkillChanged(EnumSkillType.DFS02);
        CheckSkillChanged(EnumSkillType.DSM01);
        CheckSkillChanged(EnumSkillType.DSM09);
        CheckSkillChanged(EnumSkillType.JS01);
        CheckSkillChanged(EnumSkillType.JS02);
        CheckSkillChanged(EnumSkillType.JH01);
        CheckSkillChanged(EnumSkillType.JH02);
        CheckSkillChanged(EnumSkillType.JH03);
        //物理 (注意这里重构的是需要更新的,有些状态是在特定的状态下才触发的,在单独的函数中处理)
        CheckSkillChanged(EnumSkillType.WL02);
        CheckSkillChanged(EnumSkillType.WL03);
        CheckSkillChanged(EnumSkillType.WL04);
        CheckSkillChanged(EnumSkillType.ZS01);
        CheckSkillChanged(EnumSkillType.GJS01);
        CheckSkillChanged(EnumSkillType.JAS01);
        CheckSkillChanged(EnumSkillType.YX01);
        CheckSkillChanged(EnumSkillType.SSS02);
    }

    /// <summary>
    /// 根据技能类型和技能存档数据获取该技能的当前等级数据
    /// 返回技能当前等级的数据(如果没有加点或者出错则范围null)
    /// </summary>
    /// <param name="skillType">技能类型</param>
    /// <param name="skillStructData">技能固有数据</param>
    /// <returns>技能当前等级的数据(如果没有加点或者出错则范围null)</returns>
    public SkillAttributeStruct GetSkillAttributeStruct(EnumSkillType skillType, SkillStructData skillStructData)
    {
        if (playerState.SkillPoint.ContainsKey(skillType) && playerState.SkillPoint[skillType] > 0)//如果存在该技能并且该技能加点了
        {
            SkillBaseStruct skillBaseStruct = skillStructData.SearchSkillDatas(temp => temp.skillType == skillType).FirstOrDefault();
            if (skillBaseStruct != null)//如果不存在该技能则需要在编辑器查看
            {
                int skillLevel = playerState.SkillPoint[skillType];
                if (skillLevel >= skillBaseStruct.maxLevel)
                    skillLevel = skillBaseStruct.maxLevel;
                SkillAttributeStruct skillAttributeStruct = skillBaseStruct.skillAttributeStructs[skillLevel - 1];
                return skillAttributeStruct;
            }
        }
        return null;
    }

    

    /// <summary>
    /// 等级
    /// </summary>
    int _Level;
    /// <summary>
    /// 等级
    /// </summary>
    public int Level
    {
        get
        {
            return _Level;
        }
        private set
        {
            int tempLevel = _Level;
            _Level = value;
            if (tempLevel != _Level)
            {
                //处理存档内的等级
                playerState.Level = _Level;
                //更新自身属性
                UpdateAttribute();
                //回调
                Call<IPlayerState, int>(temp => temp.Level);
            }
        }
    }

    /// <summary>
    /// 当前经验(本级的)
    /// </summary>
    int _Experience;
    /// <summary>
    /// 当前经验(本级的)
    /// </summary>
    public int Experience
    {
        get { return _Experience; }
        set
        {
            int tempExperience = _Experience;
            _Experience = value;
            if (tempExperience != _Experience)
            {
                //计算当前经验是否可以升级,如果可以则升级
                LevelDataInfo levelDataInfo = levelData[Level];
                if (levelDataInfo != null)
                {
                    if (_Experience > levelDataInfo.Experience)
                    {
                        if (_Level < levelData.MaxLevel)
                        {
                            _Experience -= levelDataInfo.Experience;
                            Level += 1;//等级加1
                                       //可能会有经验超出的情况
                                       //后期处理
                        }
                        else//表示已经满级了
                        {
                            _Experience = levelDataInfo.Experience;
                        }
                    }
                }
                //处理存档内的经验
                playerState.Experience = _Experience;
                //回调
                Call<IPlayerState, int>(temp => temp.Experience);
            }
        }
    }

    /// <summary>
    /// 技能等级变化
    /// </summary>
    bool _SkillLevelChanged;

    /// <summary>
    /// 技能等级变化
    /// </summary>
    public bool SkillLevelChanged
    {
        get { return _SkillLevelChanged; }
        set
        {
            if (value)
            {
                _SkillLevelChanged = true;
                //更新自身属性
                UpdateAttribute();
                //回调
                Call<IPlayerState, bool>(temp => temp.SkillLevelChanged);
                _SkillLevelChanged = false;
            }
        }
    }

    /// <summary>
    /// 种族等级变化
    /// </summary>
    bool _RaceLevelChanged;

    /// <summary>
    /// 种族等级变化
    /// </summary>
    public bool RaceLevelChanged
    {
        get { return _RaceLevelChanged; }
        set
        {
            if (value)
            {
                _RaceLevelChanged = value;
                //更新自身属性
                UpdateAttribute();
                //回调
                Call<IPlayerState, bool>(temp => temp.RaceLevelChanged);
                _RaceLevelChanged = false;
            }
        }
    }

    /// <summary>
    /// 装备发生变化
    /// </summary>
    bool _EquipmentChanged;
    /// <summary>
    /// 装备发生变化
    /// </summary>
    public bool EquipmentChanged
    {
        get { return _EquipmentChanged; }
        set
        {
            if (value)
            {
                _EquipmentChanged = value;
                //更新自身属性
                UpdateAttribute();
                //回调
                Call<IPlayerState, bool>(temp => temp.EquipmentChanged);
                _EquipmentChanged = false;

                //设置当前武器类型(远程和近战)
                var checkWeapons = playerState.PlayerAllGoods.Where(temp => temp.GoodsInfo.EnumGoodsType > EnumGoodsType.Arms && temp.GoodsInfo.EnumGoodsType < EnumGoodsType.HelmetBig);
                if (checkWeapons.Count() > 0)
                {
                    EnumWeaponTypeByPlayerState thisWeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.None;
                    //再次获取右手武器的信息
                    PlayGoods rightWeaponGoods = checkWeapons.Where(temp => temp.leftRightArms != null && temp.leftRightArms.Value == false).FirstOrDefault();
                    if (rightWeaponGoods != null)
                    {
                        //判断武器的类型
                        if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.SingleHanedSword && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.TwoHandedSword)//单手剑
                        {
                            thisWeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.SingleHandedSword;
                        }
                        else if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.TwoHandedSword && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.Arch)//双手剑
                            WeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.TwoHandedSword;
                        else if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.Arch && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.CrossBow)//弓
                            WeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.Arch;
                        else if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.CrossBow && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.Shield)//弩
                            WeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.CrossBow;
                        else if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.Dagger && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.LongRod)//匕首
                            WeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.Dagger;
                        else if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.LongRod && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.ShortRod)//长杖
                            WeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.LongRod;
                        else if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.ShortRod && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.CrystalBall)//短杖
                            WeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.ShortRod;
                        else if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.CrystalBall && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.HelmetBig)//水晶球
                            WeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.CrystalBall;
                    }
                    //再次获取左手武器的信息
                    PlayGoods leftWeaponGoods = checkWeapons.Where(temp => temp.leftRightArms != null && temp.leftRightArms.Value == true).FirstOrDefault();
                    if (leftWeaponGoods != null)
                    {
                        //判断如果是盾牌则附加
                        if (leftWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.Shield && leftWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.Dagger)//盾牌
                        {
                            thisWeaponTypeByPlayerState = thisWeaponTypeByPlayerState | EnumWeaponTypeByPlayerState.Shield;
                        }
                    }
                }
                else
                {
                    WeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.None;
                }
            }
        }
    }

    /// <summary>
    /// 物品发生变化
    /// </summary>
    bool _GoodsChanged;
    /// <summary>
    /// 物品发生变化
    /// </summary>
    public bool GoodsChanged
    {
        get { return _GoodsChanged; }
        set
        {
            if (value)
            {
                _GoodsChanged = value;
                //回掉
                Call<IPlayerState, bool>(temp => temp.GoodsChanged);
                _GoodsChanged = false;
            }
        }
    }

    /// <summary>
    /// 当前的按键应该遵循的状态
    /// </summary>
    private EnumKeyContactDataZone _KeyContactDataZone;
    /// <summary>
    /// 当前的按键应该遵循的状态
    /// </summary>
    public EnumKeyContactDataZone KeyContactDataZone
    {
        get { return _KeyContactDataZone; }
        private set
        {
            EnumKeyContactDataZone tempKeyContactDataZone = _KeyContactDataZone;
            _KeyContactDataZone = value;
            if (tempKeyContactDataZone != _KeyContactDataZone)
                Call<IPlayerState, EnumKeyContactDataZone>(temp => temp.KeyContactDataZone);
        }
    }

    /// <summary>
    /// 触碰到目标的结构(主要是NPC 素材等)
    /// </summary>
    private TouchTargetStruct _TouchTargetStruct;
    /// <summary>
    /// 触碰到目标的结构(主要是NPC 素材等)
    /// </summary>
    public TouchTargetStruct TouchTargetStruct
    {
        get
        {
            return _TouchTargetStruct;
        }
        set
        {
            TouchTargetStruct tempTouchTargetStruct = _TouchTargetStruct;
            _TouchTargetStruct = value;
            if (!TouchTargetStruct.Equals(tempTouchTargetStruct, _TouchTargetStruct))
                Call<IPlayerState, TouchTargetStruct>(temp => temp.TouchTargetStruct);
            //判断此时的按键应该遵循什么状态
            IGameState iGameState = GetEntity<IGameState>();
            if (iGameState != null)
            {
                EnumGameRunType gameRunType = iGameState.GameRunType;
                switch (gameRunType)
                {
                    case EnumGameRunType.Safe:
                    case EnumGameRunType.Unsafa:
                        switch (_TouchTargetStruct.TouchTargetType)
                        {
                            case TouchTargetStruct.EnumTouchTargetType.None:
                                KeyContactDataZone = EnumKeyContactDataZone.Normal;
                                break;
                            case TouchTargetStruct.EnumTouchTargetType.NPC:
                                KeyContactDataZone = EnumKeyContactDataZone.Dialogue;
                                break;
                            case TouchTargetStruct.EnumTouchTargetType.Stuff:
                                KeyContactDataZone = EnumKeyContactDataZone.Collect;
                                break;
                        }
                        break;
                    default:
                        KeyContactDataZone = EnumKeyContactDataZone.Normal;
                        break;

                }
            }
        }
    }

    #region 战斗状态
    /// <summary>
    /// 上次进入战斗状态的时间 
    /// </summary>
    private float _LastIntoBattleTime;
    /// <summary>
    /// 上次进入战斗状态的时间 
    /// </summary>
    public float LastIntoBattleTime
    {
        get { return _LastIntoBattleTime; }
        set
        {
            float tempLastIntoBattleTime = _LastIntoBattleTime;
            _LastIntoBattleTime = value;
            if (tempLastIntoBattleTime != _LastIntoBattleTime)
                Call<IPlayerState, float>(temp => temp.LastIntoBattleTime);
        }
    }

    /// <summary>
    /// 上一次暴击的时间
    /// </summary>
    private float _LastCriticalHitTime;
    /// <summary>
    /// 上一次暴击的时间
    /// </summary>
    public float LastCriticalHitTime
    {
        get { return _LastCriticalHitTime; }
        set
        {
            float tempLastCriticalHitTime = _LastCriticalHitTime;
            _LastCriticalHitTime = value;
            if (tempLastCriticalHitTime != _LastCriticalHitTime)
                Call<IPlayerState, float>(temp => temp.LastCriticalHitTime);
        }
    }

    /// <summary>
    /// 上一次闪避的时间
    /// </summary>
    private float _LastDodgeTime;
    /// <summary>
    /// 上一次闪避的时间
    /// </summary>
    public float LastDodgeTime
    {
        get { return _LastDodgeTime; }
        set
        {
            float tempLastDodgeTime = _LastDodgeTime;
            _LastDodgeTime = value;
            if (tempLastDodgeTime != _LastDodgeTime)
                Call<IPlayerState, float>(temp => temp.LastDodgeTime);
        }
    }

    /// <summary>
    /// 武器的类型(玩家状态使用的枚举)发生变化
    /// </summary>
    private EnumWeaponTypeByPlayerState _WeaponTypeByPlayerState;
    /// <summary>
    /// 武器的类型(玩家状态使用的枚举)发生变化
    /// </summary>
    public EnumWeaponTypeByPlayerState WeaponTypeByPlayerState
    {
        get { return _WeaponTypeByPlayerState; }
        set
        {
            EnumWeaponTypeByPlayerState tempWeaponTypeByPlayerState = _WeaponTypeByPlayerState;
            _WeaponTypeByPlayerState = value;
            if (_WeaponTypeByPlayerState != tempWeaponTypeByPlayerState)
            {
                Call<IPlayerState, EnumWeaponTypeByPlayerState>(temp => temp.WeaponTypeByPlayerState);
                LastChangeWeaponTime = Time.time;
            }
        }
    }

    /// <summary>
    /// 上一次切换武器(类型)的时间 
    /// </summary>
    private float _LastChangeWeaponTime;
    /// <summary>
    /// 上一次切换武器(类型)的时间 
    /// </summary>
    public float LastChangeWeaponTime
    {
        get { return _LastChangeWeaponTime; }
        private set
        {
            float tempLastChangeWeaponTime = _LastChangeWeaponTime;
            _LastChangeWeaponTime = value;
            if (tempLastChangeWeaponTime != _LastChangeWeaponTime)
                Call<IPlayerState, float>(temp => temp.LastChangeWeaponTime);
        }
    }
    #endregion
    #endregion



}