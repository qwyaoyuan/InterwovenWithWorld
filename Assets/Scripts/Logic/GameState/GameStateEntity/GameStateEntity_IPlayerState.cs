using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;

/// <summary>
/// 实现了IPlayerState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState : IPlayerState
{
    /// <summary>
    /// 萨满之矛的持续时间
    /// </summary>
    float DSM07_TimeDuration;

    /// <summary>
    /// 用于处理手柄震动的任务对象
    /// </summary>
    RunTaskStruct runTaskStruct_Vibration;

    /// <summary>
    /// 用于处理等待死亡状态的任务对象
    /// </summary>
    RunTaskStruct runTaskStruct_WaitDeath;

    /// <summary>
    /// 用于处理血量以及魔法量的回复的任务对象
    /// </summary>
    RunTaskStruct runTaskStruct_Recovery;
    

    /// <summary>
    /// buff状态的属性数组
    /// </summary>
    Dictionary<string, PropertyInfo> buffStatePropertyDic;

    /// <summary>
    /// buff状态有更新的字典
    /// </summary>
    Dictionary<string, PropertyInfo> buffStateUpdatePropertyDic;

    /// <summary>
    /// 玩家状态接口实现对象的开始函数
    /// </summary>
    partial void Start_IPlayerState()
    {
        //注册监听光环状态发生变化
        GameState.Instance.Registor<ISkillState>(ISkillState_Changed);
        //注册监听存档加载变化,监听切换场景
        GameState.Instance.Registor<IGameState>(IGameState_Changed);
        //注册监听属性变化
        GameState.Instance.Registor<IPlayerAttributeState>(IPlayerAttributeState_Changed);
        //手柄震动的携程对象
        runTaskStruct_Vibration = TaskTools.Instance.GetRunTaskStruct();
        //生命以及魔力恢复的携程对象
        runTaskStruct_Recovery = TaskTools.Instance.GetRunTaskStruct();
        //使用反射初始化buff状态的属性数组
        PropertyInfo[] buffStates = MyReflectionTools.GetPropertys<IBuffState, BuffState>();
        PropertyInfo[] debuffStates = MyReflectionTools.GetPropertys<IDebuffState, BuffState>();
        PropertyInfo[] specialStates = MyReflectionTools.GetPropertys<ISpecialState, BuffState>();
        buffStatePropertyDic = buffStates.Concat(debuffStates).Concat(specialStates).ToDictionary(temp => temp.Name, temp => temp);
        buffStateUpdatePropertyDic = new Dictionary<string, PropertyInfo>();
    }

    /// <summary>
    /// 玩家状态接口实现对象的加载函数
    /// </summary>
    partial void Load_IPlayerState()
    {
        _Level = playerState.Level;
        _Experience = playerState.Experience;
        MaxExperience = GameStateConstValues.GetExperienceAtLevel(_Level);
        //获取该种族的成长数据
        RoleOfRaceData roleOfRaceData = DataCenter.Instance.GetMetaData<RoleOfRaceData>();
        if (playerState.RoleOfRaceRoute.Count == 0)
            playerState.RoleOfRaceRoute.Add(RoleOfRace.Human);
        RoleOfRace nowRoleOfRace = playerState.RoleOfRaceRoute.Last();
        SelfRoleOfRaceInfoStruct = roleOfRaceData[nowRoleOfRace];
        //判断武器类型
        EquipmentChanged = true;
        //更新属性
        UpdateAttribute();
        //注册buff更新
        GameState.Instance.UnRegistor<IBuffState>(IBuffStateChanged_IPlayer);
        GameState.Instance.Registor<IBuffState>(IBuffStateChanged_IPlayer);
        GameState.Instance.UnRegistor<IDebuffState>(IDebuffStateChanged_IPlayer);
        GameState.Instance.Registor<IDebuffState>(IDebuffStateChanged_IPlayer);
        GameState.Instance.UnRegistor<ISpecialState>(ISpecialStateChanged_IPlayer);
        GameState.Instance.Registor<ISpecialState>(ISpecialStateChanged_IPlayer);
        Debug.Log("基础属性"+LifeRecovery + " " + ManaRecovery + " " + Power + " " + Mental);
        //开启恢复
        runTaskStruct_Recovery.StartTask(1f, () => 
        {
            float nowHP = HP;
            float maxHP = MaxHP;
            if (nowHP > 0 && nowHP< maxHP)
            {
                float lifeRecovery = LifeRecovery;
                float addedHP = lifeRecovery + nowHP;
                addedHP = Mathf.Clamp(addedHP, 0, maxHP);
                HP = addedHP;
            }
            float nowMP = Mana;
            float maxMP = MaxMana;
            if (nowMP < maxMP)
            {
                float mpRecovery = ManaRecovery;
                float addedMP = mpRecovery + nowMP;
                addedMP = Mathf.Clamp(addedMP, 0, maxMP);
                Mana = addedMP;
            }
        }, 0, false);
    }


    /// <summary>
    /// 注册监听游戏对象发生变化
    /// </summary>
    /// <param name="iGameState"></param>
    /// <param name="fieldName"></param>
    private void IGameState_Changed(IGameState iGameState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IGameState, Action>(temp => temp.LoadArchive)))
        {
            MapData mapData_PlayState = DataCenter.Instance.GetMetaData<MapData>();
            mapDataInfo_PlayerState = mapData_PlayState[iGameState.SceneName];
            UpdateAttribute();
            InitPlayAttributeState();
        }
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<IGameState, Action<string, Vector3, Action<bool>>>(temp => temp.ChangedScene)))
        {
            KeyContactDataZone = EnumKeyContactDataZone.Normal;
        }
    }

    /// <summary>
    /// 注册监听角色属性发生变化
    /// </summary>
    /// <param name="iPlayerAttributeState"></param>
    /// <param name="fieldName"></param>
    private void IPlayerAttributeState_Changed(IPlayerAttributeState iPlayerAttributeState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IPlayerAttributeState, float>(temp => temp.HP)))//监听血量
        {
            if (iPlayerAttributeState.MaxHP > 0 && iPlayerAttributeState.HP <= 0)//只有血量大于0才表示初始化完毕,此时如果血量小于等于0表示死了
            {
                //如果存在心志力并且心志力满值,则消耗心志力回复hp
                if (MindTraining > 0 && Mathf.Abs(MindTraining - MaxMindTraining) > 0.1f)
                {
                    HP = MaxHP;
                    MindTraining = 0;
                    //判断萨满之矛的冷却,如果已经冷却了,则添加状态
                    ISkillState iSkillState = GameState.Instance.GetEntity<ISkillState>();
                    float coolingTime = iSkillState.GetSkillRuntimeCoolingTime((int)EnumSkillType.DSM07);
                    if (CoolingTime <= 0)
                    {
                        SkillStructData skillStructData = DataCenter.Instance.GetMetaData<SkillStructData>();
                        SkillAttributeStruct skillAttributeStruct = GetSkillAttributeStruct(EnumSkillType.DSM07, skillStructData);
                        if (skillAttributeStruct != null)
                        {
                            IAttributeState iAttributeDSM07 = GetAttribute(-(int)EnumSkillType.DSM07);
                            if (iAttributeDSM07 == null && CreateAttributeHandle(-(int)EnumSkillType.DSM07))
                            {
                                iAttributeDSM07 = GetAttribute(-(int)EnumSkillType.DSM07);
                            }
                            if (iAttributeDSM07 != null)
                            {
                                SetIAttributeStateDataBySkillData(iAttributeDSM07, GetAttribute(0), skillAttributeStruct);
                                DSM07_TimeDuration = 20;
                            }
                        }
                    }
                }
                else
                {
                    if (runTaskStruct_WaitDeath == null)
                    //死亡状态的携程对象
                    {
                        runTaskStruct_WaitDeath = TaskTools.Instance.GetRunTaskStruct();
                        //回去复活
                        runTaskStruct_WaitDeath.StartTask(3f,
                                () =>
                                {
                                    //获取最后一个路牌的ID
                                    int lastStreetID = playerState.StreetID;
                                    string lastStreetScene = playerState.StreetScene;
                                    //判断获取路牌,如果不存在则查找最初的路牌,如果还不存在则原地复活 
                                    NPCData npcData = DataCenter.Instance.GetMetaData<NPCData>();
                                    NPCDataInfo npcDataInfo = npcData.GetNPCDataInfo(lastStreetScene, lastStreetID);
                                    if (npcDataInfo == null)
                                        npcDataInfo = npcData.GetNPCDataInfo(temp => temp.NPCType == EnumNPCType.Street).FirstOrDefault();
                                    IGameState iGameState = GameState.Instance.GetEntity<IGameState>();
                                    iGameState.ChangedScene(npcDataInfo != null ? lastStreetScene : iGameState.SceneName, npcDataInfo != null ? (npcDataInfo.NPCLocation + Vector3.forward) : PlayerObj.transform.position);
                                    //修改血量
                                    HP = MaxHP;
                                    runTaskStruct_WaitDeath = null;
                                }, 1, false);
                    }

                }
            }
        }
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
        UpdatePlayerBuffState();
        UpdatePlayerMovePath();
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
    /// 玩家自身的种族成长数据
    /// </summary>
    public RoleOfRaceInfoStruct SelfRoleOfRaceInfoStruct { get; private set; }

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
        EnumWeaponTypeByPlayerState weaponType_Right = WeaponTypeByPlayerState | EnumWeaponTypeByPlayerState.Shield - EnumWeaponTypeByPlayerState.Shield;//去除盾牌                                                                                                                 
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
                    iAttributeState.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
                    SetIAttributeStateDataBySkillData(iAttributeState, iAttributeState_Base, skillAttributeStruct);
                }
                //近战专精
                if (iPlayerAttributeState.GetAttribute(handle_JZZJ) == null)
                {
                    SkillAttributeStruct skillAttributeStruct = GetSkillAttributeStruct(EnumSkillType.KZS02, skillStructData);
                    iPlayerAttributeState.CreateAttributeHandle(handle_JZZJ);
                    IAttributeState iAttributeState = iPlayerAttributeState.GetAttribute(handle_JZZJ);
                    iAttributeState.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
                    SetIAttributeStateDataBySkillData(iAttributeState, iAttributeState_Base, skillAttributeStruct);
                }
                //移除远程强化
                iPlayerAttributeState.RemoveAttribute(handle_YCQH);
                //移除远程专精 
                iPlayerAttributeState.RemoveAttribute(handle_YCZJ);
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
                    iAttributeState.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
                    SetIAttributeStateDataBySkillData(iAttributeState, iAttributeState_Base, skillAttributeStruct);
                }
                //远程专精
                if (iPlayerAttributeState.GetAttribute(handle_YCZJ) == null)
                {
                    SkillAttributeStruct skillAttributeStruct = GetSkillAttributeStruct(EnumSkillType.SSS01, skillStructData);
                    iPlayerAttributeState.CreateAttributeHandle(handle_YCZJ);
                    IAttributeState iAttributeState = iPlayerAttributeState.GetAttribute(handle_YCZJ);
                    iAttributeState.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
                    SetIAttributeStateDataBySkillData(iAttributeState, iAttributeState_Base, skillAttributeStruct);
                }
                //移除近战强化
                iPlayerAttributeState.RemoveAttribute(handle_JZQH);
                //移除近战专精 
                iPlayerAttributeState.RemoveAttribute(handle_JZZJ);
                break;
            default:
                iPlayerAttributeState.RemoveAttribute(handle_JZQH);
                iPlayerAttributeState.RemoveAttribute(handle_JZZJ);
                iPlayerAttributeState.RemoveAttribute(handle_YCQH);
                iPlayerAttributeState.RemoveAttribute(handle_YCZJ);
                break;
        }

        //检测萨满精神是否可以使用
        SkillAttributeStruct skillAttributeStruct_DSM09 = GetSkillAttributeStruct(EnumSkillType.DSM09, skillStructData);
        SkillStruct_DSM09 skillStruct_DSM09 = skillStructData.SearchSkillDatas(temp => temp.skillType == EnumSkillType.DSM09).FirstOrDefault() as SkillStruct_DSM09;
        int handle_DSM09 = -(int)EnumSkillType.DSM09;//萨满精神
        IAttributeState iAttributeState_DSM09 = GetAttribute(handle_DSM09);
        if (skillAttributeStruct_DSM09 != null && skillStruct_DSM09 != null)
        {
            iAttributeState_DSM09.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
            iAttributeState_DSM09.MaxMentality = 100;
            iAttributeState_DSM09.MaxMindTraining = 100;
            //根据精神力计量计算抗性
            iAttributeState_DSM09.AbnormalStateResistance = iAttributeState_DSM09.Mentality * skillStruct_DSM09.DeblockingMentalityMindTraining / 100;
        }
        else
            iAttributeState_DSM09.Init();

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
                {
                    iAttributeState_JAS02.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
                    if (waitTime_JAS02 > intervalCriticalHitTime)//在效果时间内,则更新攻击速度
                        iAttributeState_JAS02.AttackSpeed = skillAttributeStruct_JAS02.AttackSpeed;
                    else//在效果时间外,则归零
                        iAttributeState_JAS02.CritRate = 0;
                }
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
                {
                    iAttributeState_DZ01.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
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
                {
                    iAttributeState_JAS02.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
                    if (waitTime_JAS02 < intervalDodgeTime)//在效果时间内,则更新攻击速度
                        iAttributeState_JAS02.PhysicsPenetrate = skillAttributeStruct_JAS02.PYEDMG;
                    else//在效果时间外,则归零
                        iAttributeState_JAS02.PhysicsPenetrate = 0;
                }
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
                {
                    iAttributeState_DZ02.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
                    if (waitTime_DZ02 < intervalChangeWeaponTime) //在效果时间内,则更新暴击率和暴击伤害
                    {
                        iAttributeState_DZ02.CritRate = skillAttributeStruct_DZ02.IncreasedCritRate / 100f;
                        iAttributeState_DZ02.CritDamageRatio = skillAttributeStruct_DZ02.CritDamagePromotion / 100f;
                    }
                    else //在效果时间外,则归零
                        iAttributeState_DZ02.Init();
                }
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
                    iAttributeState_YX01.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
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
                {
                    iAttributeState_GJS01.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
                    if (waitTime_GJS01 < intervalIntoBattleTime)//在效果时间内,则更新攻击速度和移动速度 
                    {
                        iAttributeState_GJS01.AttackSpeed = skillAttributeStruct_GJS01.AttackSpeed;
                        iAttributeState_GJS01.MoveSpeed = skillAttributeStruct_GJS01.MoveSpeedAddtion * iAttributeState_Base.MoveSpeed / 100f;
                    }
                    else //在效果时间外,则归零
                        iAttributeState_GJS01.Init();
                }
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
                    iAttributeState_YX01.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
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

            //萨满精神(每个一定时间提升心志力计量和精神力计量)
            if (skillAttributeStruct_DSM09 != null && skillStruct_DSM09 != null)
            {
                float mentality = iAttributeState_DSM09.Mentality + Time.deltaTime * 1;
                float mindTraining = iAttributeState_DSM09.MindTraining + Time.deltaTime * 1;
                if (iAttributeState_DSM09 != null)
                {
                    iAttributeState_DSM09.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
                    iAttributeState_DSM09.Mentality = Mathf.Clamp(mentality, 0, 100);
                    iAttributeState_DSM09.MindTraining = Mathf.Clamp(mindTraining, 0, 100);
                }
            }
        }

        //萨满之矛的检测移除
        if (DSM07_TimeDuration > 0)
        {
            DSM07_TimeDuration -= Time.deltaTime;
            if (DSM07_TimeDuration <= 0)
            {
                //移除萨满之矛状态
                RemoveAttribute(-(int)EnumSkillType.DSM07);
            }
        }
    }

    /// <summary>
    /// 用于初始化角色属性状态(主要用于更新玩属性后,有些字段如血量魔法值等需要等值为最大血量和魔法值)
    /// </summary>
    public void InitPlayAttributeState()
    {
        HP = MaxHP;
        Mana = MaxMana;
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
            iAttributeState_Base.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
            int level = playerState.Level;
            //当前计算方式,基础数据各项值为10,每升一级增加2
            iAttributeState_Base.Quick = SelfRoleOfRaceInfoStruct.baseQuick + level * SelfRoleOfRaceInfoStruct.additionQuick + playerState.Agility;
            iAttributeState_Base.Mental = SelfRoleOfRaceInfoStruct.baseMental + level * SelfRoleOfRaceInfoStruct.additionMental + playerState.Spirit;
            iAttributeState_Base.Power = SelfRoleOfRaceInfoStruct.basePower + level * SelfRoleOfRaceInfoStruct.additionPower + playerState.Strength;
            iAttributeState_Base.MaxHP += SelfRoleOfRaceInfoStruct.baseHP + level * SelfRoleOfRaceInfoStruct.additionHP;
            iAttributeState_Base.MaxMana += SelfRoleOfRaceInfoStruct.baseMana + level * SelfRoleOfRaceInfoStruct.additionMana;
            iAttributeState_Base.BasePhysicDamage = SelfRoleOfRaceInfoStruct.basePhysicDamage + level * SelfRoleOfRaceInfoStruct.additionPhysicDamage;
            iAttributeState_Base.BasePhysicDefense = SelfRoleOfRaceInfoStruct.basePhysicDefense + level * SelfRoleOfRaceInfoStruct.additionPhysicDefense;
            iAttributeState_Base.LifeRecovery += SelfRoleOfRaceInfoStruct.baseHPRecovery;
            iAttributeState_Base.ManaRecovery += SelfRoleOfRaceInfoStruct.baseManaRecovery;
            iAttributeState_Base.EvadeRate += SelfRoleOfRaceInfoStruct.baseEvadeRate;
            iAttributeState_Base.HitRate += SelfRoleOfRaceInfoStruct.baseHitRate;
            iAttributeState_Base.CritRate += SelfRoleOfRaceInfoStruct.baseCritRate;
            iAttributeState_Base.AttackSpeed += SelfRoleOfRaceInfoStruct.baseAttackSpeed;
            iAttributeState_Base.MoveSpeed += SelfRoleOfRaceInfoStruct.baseMoveSpeed;
            iAttributeState_Base.MaxUseMana += SelfRoleOfRaceInfoStruct.baseMaxUseMana;
            iAttributeState_Base.SightDef = 1 - SelfRoleOfRaceInfoStruct.baseSight;
            iAttributeState_Base.ExemptionChatingMana = SelfRoleOfRaceInfoStruct.baseCoolingTime;
            iAttributeState_Base.CritDamageRatio = SelfRoleOfRaceInfoStruct.baseCritHurt;
            iAttributeState_Base.CriticalDef = SelfRoleOfRaceInfoStruct.baseCritHurtDef;
            iAttributeState_Base.LightFaith = SelfRoleOfRaceInfoStruct.baseLightStrength;
            iAttributeState_Base.DarkFaith = SelfRoleOfRaceInfoStruct.baseDarkStrength;
            iAttributeState_Base.LifeFaith = SelfRoleOfRaceInfoStruct.baseLifeStrength;
            iAttributeState_Base.NaturalFaith = SelfRoleOfRaceInfoStruct.baseNaturalStrength;
            iAttributeState_Base.MagicFit = SelfRoleOfRaceInfoStruct.baseMagicFit;
            iAttributeState_Base.AbnormalStateResistance += SelfRoleOfRaceInfoStruct.baseAbnormalStateResistance;
        }
        //处理装备的附加属性
        IAttributeState iAttributeState_Equip = iPlayerAttributeState.GetAttribute(1);
        if (iAttributeState_Equip != null)
        {
            iAttributeState_Equip.Init();
            //获取穿戴中的装备
            PlayGoods[] playGoodses = playerState.PlayerAllGoods.Where(temp => temp.GoodsLocation == GoodsLocation.Wearing).ToArray();
            foreach (PlayGoods playGoods in playGoodses)
            {
                List<GoodsAbility> goodsAbilities = playGoods.GoodsInfo.goodsAbilities;
                foreach (GoodsAbility goodsAbility in goodsAbilities)
                {
                    switch (goodsAbility.AbilibityKind)
                    {
                        case EnumGoodsAbility.HP:
                            iAttributeState_Equip.MaxHP += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.MP:
                            iAttributeState_Equip.MaxMana += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.DEX:
                            iAttributeState_Equip.Quick += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.WIL:
                            iAttributeState_Equip.Mental += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.STR:
                            iAttributeState_Equip.Power += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.MAFF:
                            iAttributeState_Equip.MagicFit += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.SRES:
                            iAttributeState_Equip.AbnormalStateResistance += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.Sight:
                            iAttributeState_Equip.View += (iAttributeState_Base.View * goodsAbility.Value / 100f);
                            break;
                        case EnumGoodsAbility.MSPD:
                            iAttributeState_Equip.MoveSpeed += (iAttributeState_Base.MoveSpeed * goodsAbility.Value / 100);
                            break;
                        case EnumGoodsAbility.ASPD:
                            iAttributeState_Equip.AttackSpeed += (iAttributeState_Base.AttackSpeed * goodsAbility.Value / 100);
                            break;
                        case EnumGoodsAbility.AVD:
                            iAttributeState_Equip.EvadeRate += goodsAbility.Value / 100f;
                            break;
                        case EnumGoodsAbility.HIT:
                            iAttributeState_Equip.HitRate += goodsAbility.Value / 100f;
                            break;
                        case EnumGoodsAbility.Critical:
                            iAttributeState_Equip.CritRate += goodsAbility.Value / 100f;
                            break;
                        case EnumGoodsAbility.INT:
                            iAttributeState_Equip.MagicAttacking += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.RES:
                            iAttributeState_Equip.MagicResistance += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.ATN:
                            iAttributeState_Equip.PhysicsAttacking += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.DEF:
                            iAttributeState_Equip.PhysicsResistance += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.Light:
                            iAttributeState_Equip.LightFaith += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.Dark:
                            iAttributeState_Equip.DarkFaith += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.Bioligy:
                            iAttributeState_Equip.LifeFaith += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.Nature:
                            iAttributeState_Equip.NaturalFaith += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.EquipATK:
                            iAttributeState_Equip.BasePhysicDamage += goodsAbility.Value;
                            break;
                        case EnumGoodsAbility.EquipDEF:
                            iAttributeState_Equip.BasePhysicDefense += goodsAbility.Value;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        //处理被动技能和光环技能等级造成的属性变化,以及处理光环技能开关状态造成的属性变化
        Action<EnumSkillType> CheckSkillChanged = (skillType) =>
        {
            int handle = -(int)skillType;
            IAttributeState iAttributeState = iPlayerAttributeState.GetAttribute(handle);
            if (iAttributeState != null)//如果没有目标被动则需要修改初始化模块)
            {
                iAttributeState.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族成长对象
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
                //更新存档中的技能点和属性点
                playerState.FreedomPoint += SelfRoleOfRaceInfoStruct.additionSkillPoint;
                playerState.PropertyPoint += SelfRoleOfRaceInfoStruct.additionAttributePoint;
                //更新本级的升级经验
                MaxExperience = GameStateConstValues.GetExperienceAtLevel(_Level);
                //更新自身属性
                UpdateAttribute();
                InitPlayAttributeState();
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
                //LevelDataInfo levelDataInfo = levelData[Level];
                //if (levelDataInfo != null)
                //{
                //    if (_Experience > levelDataInfo.Experience)
                //    {
                //        if (_Level < levelData.MaxLevel)
                //        {
                //            _Experience -= levelDataInfo.Experience;
                //            Level += 1;//等级加1
                //                       //可能会有经验超出的情况
                //                       //后期处理
                //        }
                //        else//表示已经满级了
                //        {
                //            _Experience = levelDataInfo.Experience;
                //        }
                //    }
                //}
                if (_Experience > MaxExperience)//经验超出
                {
                    if (_Level < GameStateConstValues.MAXLEVEL)//等级没有超出
                    {
                        _Experience -= MaxExperience;
                        Level += 1;
                    }
                    else
                    {
                        _Experience = MaxExperience;//已经满级了经验不在增加
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
    /// 最大经验值(本级的)
    /// </summary>
    public int MaxExperience { get; set; }

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
                var checkWeapons = playerState.PlayerAllGoods.Where(temp => temp.GoodsInfo.EnumGoodsType > EnumGoodsType.Arms && temp.GoodsInfo.EnumGoodsType < EnumGoodsType.HelmetBig && temp.GoodsLocation == GoodsLocation.Wearing);
                if (checkWeapons.Count() > 0)
                {
                    EnumWeaponTypeByPlayerState thisWeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.None;
                    //再次获取右手武器的信息
                    PlayGoods rightWeaponGoods = checkWeapons.Where(temp => temp.leftRightArms != null && temp.leftRightArms.Value == true).FirstOrDefault();
                    if (rightWeaponGoods != null)
                    {
                        //判断武器的类型
                        if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.SingleHanedSword && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.TwoHandedSword)//单手剑
                        {
                            thisWeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.SingleHandedSword;
                        }
                        else if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.TwoHandedSword && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.Arch)//双手剑
                            thisWeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.TwoHandedSword;
                        else if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.Arch && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.CrossBow)//弓
                            thisWeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.Arch;
                        else if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.CrossBow && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.Shield)//弩
                            thisWeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.CrossBow;
                        else if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.Dagger && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.LongRod)//匕首
                            thisWeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.Dagger;
                        else if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.LongRod && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.ShortRod)//长杖
                            thisWeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.LongRod;
                        else if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.ShortRod && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.CrystalBall)//短杖
                            thisWeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.ShortRod;
                        else if (rightWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.CrystalBall && rightWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.HelmetBig)//水晶球
                            thisWeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.CrystalBall;
                    }
                    //再次获取左手武器的信息
                    PlayGoods leftWeaponGoods = checkWeapons.Where(temp => temp.leftRightArms != null && temp.leftRightArms.Value == false).FirstOrDefault();
                    if (leftWeaponGoods != null)
                    {
                        //判断如果是盾牌则附加
                        if (leftWeaponGoods.GoodsInfo.EnumGoodsType > EnumGoodsType.Shield && leftWeaponGoods.GoodsInfo.EnumGoodsType < EnumGoodsType.Dagger)//盾牌
                        {
                            thisWeaponTypeByPlayerState = thisWeaponTypeByPlayerState | EnumWeaponTypeByPlayerState.Shield;
                        }
                    }
                    WeaponTypeByPlayerState = thisWeaponTypeByPlayerState;
                }
                else
                {
                    WeaponTypeByPlayerState = EnumWeaponTypeByPlayerState.None;
                }
            }
        }
    }

    /// <summary>
    /// 金钱发生变化
    /// </summary>
    bool _SpriteChanged;
    /// <summary>
    /// 金钱发生变化
    /// </summary>
    public bool SpriteChanged
    {
        get { return _SpriteChanged; }
        set
        {
            if (value)
            {
                _SpriteChanged = value;
                //回调
                Call<IPlayerState, bool>(temp => temp.SpriteChanged);
                _SpriteChanged = false;
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
                            case TouchTargetStruct.EnumTouchTargetType.Action:
                                KeyContactDataZone = EnumKeyContactDataZone.Action;
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

    /// <summary>
    /// 命中了怪物的对象
    /// </summary>
    private MonsterControl _HitMonsterTarget;
    /// <summary>
    /// 命中了怪物的对象
    /// </summary>
    public MonsterControl HitMonsterTarget
    {
        get { return _HitMonsterTarget; }
        set
        {
            MonsterControl tempHitMonsterTarget = _HitMonsterTarget;
            _HitMonsterTarget = value;
            if (_HitMonsterTarget != tempHitMonsterTarget)
            {
                Call<IPlayerState, MonsterControl>(temp => temp.HitMonsterTarget);
                _HitMonsterTarget = null;
            }
        }
    }

    /// <summary>
    /// 设置手柄震动
    /// </summary>
    /// <param name="time">手柄震动的时间</param>
    /// <param name="left">手柄左侧的震动频率</param>
    /// <param name="right">手柄右侧的震动频率</param>
    public void SetVibration(float time, float left, float right)
    {
        XInputDotNetPure.PlayerIndex _type = XInputDotNetPure.PlayerIndex.One;
        XInputDotNetPure.GamePad.SetVibration(_type, left, right);//设置震动
        runTaskStruct_Vibration.StartTask(time,
            () =>
            {
                XInputDotNetPure.GamePad.SetVibration(_type, 0, 0);//等待指定时间后归位
            }, 1, false);
    }

    /// <summary>
    /// 强制移动
    /// </summary>
    ForceMoveStruct _ForceMoveStruct;
    /// <summary>
    /// 强制移动
    /// </summary>
    public ForceMoveStruct ForceMoveStruct
    {
        get
        {
            return _ForceMoveStruct;
        }
        set
        {
            _ForceMoveStruct = value;
            Call<IPlayerState, ForceMoveStruct>(temp => temp.ForceMoveStruct);
        }
    }
    #endregion
    #endregion

    #region 玩家的特殊状态(buff debuff  special )

    /// <summary>
    /// buff状态发生变化时触发
    /// </summary>
    /// <param name="iBuffState"></param>
    /// <param name="fieldName"></param>
    private void IBuffStateChanged_IPlayer(IBuffState iBuffState, string fieldName)
    {
        if (buffStatePropertyDic.ContainsKey(fieldName) && !buffStateUpdatePropertyDic.ContainsKey(fieldName))
        {
            buffStateUpdatePropertyDic.Add(fieldName, buffStatePropertyDic[fieldName]);
            //更新属性
            SetBuffStateAttribute(buffStatePropertyDic[fieldName], true);
        }
    }

    /// <summary>
    /// debuff状态发生变化时触发
    /// </summary>
    /// <param name="iDebuffState"></param>
    /// <param name="fieldName"></param>
    private void IDebuffStateChanged_IPlayer(IDebuffState iDebuffState, string fieldName)
    {
        if (buffStatePropertyDic.ContainsKey(fieldName) && !buffStateUpdatePropertyDic.ContainsKey(fieldName))
        {
            buffStateUpdatePropertyDic.Add(fieldName, buffStatePropertyDic[fieldName]);
            //更新属性
            SetBuffStateAttribute(buffStatePropertyDic[fieldName], true);
        }
    }

    /// <summary>
    /// special状态发生变化时触发
    /// </summary>
    /// <param name="iSpecialState"></param>
    /// <param name="fieldName"></param>
    private void ISpecialStateChanged_IPlayer(ISpecialState iSpecialState, string fieldName)
    {
        if (buffStatePropertyDic.ContainsKey(fieldName) && !buffStateUpdatePropertyDic.ContainsKey(fieldName))
        {
            buffStateUpdatePropertyDic.Add(fieldName, buffStatePropertyDic[fieldName]);
            //更新属性
            SetBuffStateAttribute(buffStatePropertyDic[fieldName], true);
        }
    }

    /// <summary>
    /// 更新玩家的状态
    /// </summary>
    private void UpdatePlayerBuffState()
    {
        List<string> mustMove = null;
        foreach (KeyValuePair<string, PropertyInfo> buffStateUpdateProperty in buffStateUpdatePropertyDic)
        {
            MethodInfo getMethod = buffStateUpdateProperty.Value.GetGetMethod();
            MethodInfo setMethod = buffStateUpdateProperty.Value.GetSetMethod();
            if (getMethod != null && setMethod != null)
            {
                object tempObj = getMethod.Invoke(this, null);
                if (tempObj != null && tempObj.GetType().Equals(typeof(BuffState)))
                {
                    BuffState buffState = (BuffState)tempObj;
                    buffState.Time -= Time.deltaTime;
                    setMethod.Invoke(this, new object[] { buffState });
                    if (buffState.Time < 0)
                    {
                        if (mustMove == null)
                            mustMove = new List<string>();
                        mustMove.Add(buffStateUpdateProperty.Key);
                    }
                }
            }
        }
        if (mustMove != null)
        {
            foreach (string item in mustMove)
            {
                PropertyInfo mustMoveInfo = buffStateUpdatePropertyDic[item];
                SetBuffStateAttribute(mustMoveInfo, false);
            }
        }
    }

    /// <summary>
    /// 设置buff状态的属性,true表示添加,false表示移除
    /// </summary>
    /// <param name="mustMoveInfo"></param>
    /// <param name="add"></param>
    private void SetBuffStateAttribute(PropertyInfo mustMoveInfo, bool add)
    {
        MethodInfo getMethod = mustMoveInfo.GetGetMethod();
        if (getMethod != null)
        {
            object tempObj = getMethod.Invoke(this, null);
            if (tempObj != null && tempObj.GetType().Equals(typeof(BuffState)))
            {
                BuffState buffState = (BuffState)tempObj;
                int buffStateAttributeID = -10000 - (int)buffState.statusEffect;
                IPlayerAttributeState iPlayerAttributeState = GameState.Instance.GetEntity<IPlayerAttributeState>();
                IAttributeState iAttributeState = iPlayerAttributeState.GetAttribute(buffStateAttributeID);
                if (iAttributeState != null)
                {
                    if (!add)
                        iAttributeState.Init();
                    else
                    {
                        iAttributeState.SetRoleOfRaceAddition(SelfRoleOfRaceInfoStruct);//设置种族数据
                        SetStateEffectDataToAttribute(buffState, iAttributeState);
                    }
                }

            }
        }
    }

    /// <summary>
    /// 设置该buff对应的属性
    /// </summary>
    /// <param name="buffState"></param>
    /// <param name="iAttributeState"></param>
    private void SetStateEffectDataToAttribute(BuffState buffState, IAttributeState iAttributeState)
    {
        StatusDataInfo.StatusLevelDataInfo data = buffState.tempData as StatusDataInfo.StatusLevelDataInfo;
        IAttributeState iAttributeBase = GetAttribute(0);//基础属性
        if (data != null && data.StatusActionDataInfoDic != null && iAttributeBase != null)
        {
            foreach (KeyValuePair<EnumStatusAction, StatusActionDataInfo_Base> kvp in data.StatusActionDataInfoDic)
            {
                Type t = Type.GetType("StatusActionDataInfo_" + kvp.Key.ToString());
                if (t == null)
                    continue;
                FieldInfo variationInfo = t.GetField("Variation", BindingFlags.Public | BindingFlags.Instance);
                if (variationInfo != null)
                {
                    int variationValue = (int)variationInfo.GetValue(data);
                    switch (kvp.Key)
                    {
                        case EnumStatusAction.MoveSpeed:
                            iAttributeState.MoveSpeed = iAttributeBase.MoveSpeed * variationValue / 100f;
                            break;
                        case EnumStatusAction.AttackSpeed:
                            iAttributeState.AttackSpeed = iAttributeBase.AttackSpeed * variationValue / 100f;
                            break;
                        case EnumStatusAction.StateResistance:
                            iAttributeState.AbnormalStateResistance = variationValue / 100f;
                            break;
                        case EnumStatusAction.Life:
                            iAttributeState.HP += variationValue * Time.deltaTime;
                            break;
                        case EnumStatusAction.ElementResistance:

                            break;
                        case EnumStatusAction.ElementAffine:
                            iAttributeState.EffectAffine = variationValue;
                            break;
                        case EnumStatusAction.MagicAffine:
                            iAttributeState.MagicFit = variationValue;
                            break;
                        case EnumStatusAction.LifeRecoverySpeed:
                            iAttributeState.LifeRecovery = variationValue;
                            break;
                        case EnumStatusAction.ManaRecoverySpeed:
                            iAttributeState.ManaRecovery = variationValue;
                            break;
                        case EnumStatusAction.HitRate:
                            iAttributeState.HitRate = variationValue / 100f;
                            break;
                        case EnumStatusAction.EvadeRate:
                            iAttributeState.EvadeRate = variationValue / 100f;
                            break;
                        case EnumStatusAction.CritRate:
                            iAttributeState.CritRate = variationValue / 100f;
                            break;
                        case EnumStatusAction.ImproveExperience:
                            iAttributeState.ExperienceValuePlus = 1 + variationValue / 100f;
                            break;
                        case EnumStatusAction.View:
                            iAttributeState.View = variationValue;
                            break;
                        case EnumStatusAction.AttributeChange:
                            iAttributeState.Power = iAttributeBase.Power * variationValue / 100f;
                            iAttributeState.Mental = iAttributeBase.Mental * variationValue / 100f;
                            iAttributeState.Quick = iAttributeBase.Quick * variationValue / 100f;
                            break;
                        case EnumStatusAction.MagicSuckBlood:
                            //暂时未实现
                            break;
                        case EnumStatusAction.PhysicsSuckBlood:
                            //暂时未实现
                            break;
                        case EnumStatusAction.MagicResisitance:
                            iAttributeState.MagicResistance = variationValue;
                            break;
                        case EnumStatusAction.PhysicsResisitance:
                            iAttributeState.PhysicsResistance = variationValue;
                            break;
                        case EnumStatusAction.MagicPenetrate:
                            iAttributeState.MagicPenetrate = variationValue;
                            break;
                        case EnumStatusAction.PhysicsPenetrate:
                            iAttributeState.PhysicsPenetrate = variationValue;
                            break;
                        case EnumStatusAction.MagicAdditionalDamage:
                            iAttributeState.MagicAdditionalDamage = variationValue;
                            break;
                        case EnumStatusAction.PhysicsAdditionalDamage:
                            iAttributeState.PhysicsAdditionalDamage = variationValue;
                            break;
                        case EnumStatusAction.MagicFinalDamage:
                            iAttributeState.MagicFinalDamage = variationValue / 100f;
                            break;
                        case EnumStatusAction.PhysicsFinalDamage:
                            iAttributeState.PhysicsFinalDamage = variationValue / 100f;
                            break;
                    }
                }
                //一些没有子类或者需要特殊类处理的类型
                switch (kvp.Key)
                {
                    case EnumStatusAction.Stiff:
                        break;
                    case EnumStatusAction.CantMove:
                        break;
                    case EnumStatusAction.CantLeaveTheFight:
                        break;
                    case EnumStatusAction.HitOtherHurt:
                        break;
                    case EnumStatusAction.HitTargetHurt:
                        break;
                    case EnumStatusAction.CantReadTheArticle:
                        break;
                    case EnumStatusAction.CantMagic:
                        break;
                    case EnumStatusAction.CanPhysics:
                        break;
                    case EnumStatusAction.Vertigo:
                        break;
                }
            }
        }
    }

    #endregion

    /// <summary>
    /// 吃药
    /// </summary>
    /// <param name="goodsID">物品ID</param>
    public void EatMedicine(int goodsID)
    {
        PlayGoods playGoods = playerState.PlayerAllGoods.Where(temp => temp.ID == goodsID).FirstOrDefault();
        if (playGoods == null)
            return;
        RunTaskStruct runTaskStruct = TaskTools.Instance.GetRunTaskStruct();
        int attributeID = CreateAttributeHandle();
        IAttributeState iAttributeState = GetAttribute(attributeID);
        IAttributeState baseAttributeState = GetAttribute(0);//基础属性 
        foreach (GoodsAbility goodsAbility in playGoods.GoodsInfo.goodsAbilities)
        {
            switch (goodsAbility.AbilibityKind)
            {
                case EnumGoodsAbility.HPRect_Rate:
                    iAttributeState.LifeRecovery = baseAttributeState.LifeRecovery * goodsAbility.Value;
                    break;
                case EnumGoodsAbility.MPRec_Rate:
                    iAttributeState.ManaRecovery = baseAttributeState.ManaRecovery * goodsAbility.Value;
                    break;
            }
        }
        playGoods.Count -= 1;
        if (playGoods.Count <= 0)
            playerState.PlayerAllGoods.Remove(playGoods);
        //开启任务,10秒后销毁
        runTaskStruct.StartTask(10f, () => 
        {
            RemoveAttribute(attributeID);//移除属性
            TaskTools.Instance.RemoveRunTaskStruct(runTaskStruct);//移除任务
        }, 1, false);
    }

    #region 玩家的行动路线

    MapDataInfo mapDataInfo_PlayerState;
    /// <summary>
    /// 玩家之前的位置
    /// </summary>
    Vector3 playerLastPos;
    /// <summary>
    /// 更新玩家的行动路线
    /// </summary>
    private void UpdatePlayerMovePath()
    {
        if (playerState != null && PlayerObj != null && Vector3.Distance(playerLastPos, PlayerObj.transform.position) > 1)
        {
            playerLastPos = PlayerObj.transform.position;
            //转换为地图坐标 
            if (mapDataInfo_PlayerState == null || mapDataInfo_PlayerState.SceneName != iGameState.SceneName)
            {
                MapData mapData_PlayState = DataCenter.Instance.GetMetaData<MapData>();
                mapDataInfo_PlayerState = mapData_PlayState[iGameState.SceneName];
            }
            if (mapDataInfo_PlayerState != null && mapDataInfo_PlayerState.MapSprite!=null)
            {
                Rect spriteRect = mapDataInfo_PlayerState.MapSprite.rect;
                Rect mapRect = mapDataInfo_PlayerState.SceneRect;
                Vector2 nowSpriteVec = new Vector2(
                    spriteRect.xMin + spriteRect.width * ((PlayerObj.transform.position.x - mapRect.xMin) / mapRect.width),
                    spriteRect.yMin+ spriteRect.height*((PlayerObj.transform.position.z-mapRect.yMin)/mapRect.height));
                playerState.SetPlayerMovePath(SceneName, nowSpriteVec);
            }
        }
    }
    #endregion

}