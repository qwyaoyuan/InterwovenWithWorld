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
                IAttributeState iAttributeState =  iPlayerAttributeState.GetAttribute(handle);
                if (iAttributeState != null)
                {
                    iPlayerAttributeState.RemoveAttribute(handle);
                    UpdateAttribute();
                }
            }
        }
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
    /// 更新自身属性
    /// 在等级变化 装备变化时触发
    /// 主要更新的是HP MP上限,防御攻击等等随等级装备变化的属性等
    /// </summary>
    public void UpdateAttribute()
    {
        PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
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
                if (playerState.SkillPoint.ContainsKey(skillType) && playerState.SkillPoint[skillType] > 0)//如果存在该技能并且该技能加点了
                {
                    SkillBaseStruct skillBaseStruct = skillStructData.SearchSkillDatas(temp => temp.skillType == skillType).FirstOrDefault();
                    if (skillBaseStruct != null)//如果不存在该技能则需要在编辑器查看
                    {
                        int skillLevel = playerState.SkillPoint[skillType];
                        if (skillLevel >= skillBaseStruct.maxLevel)
                            skillLevel = skillBaseStruct.maxLevel;
                        SkillAttributeStruct skillAttributeStruct = skillBaseStruct.skillAttributeStructs[skillLevel - 1];
                        if (skillAttributeStruct != null)//存在该等级的技能属性数据 
                        {
                            iAttributeState.MaxUseMana = skillAttributeStruct.MaxMP;//最大耗魔上限
                            iAttributeState.MaxMana = skillAttributeStruct.MP * iAttributeState_Base.MaxMana / 100f;//百分比的法力上限
                            iAttributeState.MagicAttacking = skillAttributeStruct.DMG;//魔法攻击力
                            iAttributeState.ElementAffine = skillAttributeStruct.ERST;//特效影响力(原元素亲和)
                            iAttributeState.EffectResideTime = skillAttributeStruct.RETI;//特效驻留时间
                            iAttributeState.PhysicsAdditionalDamage = skillAttributeStruct.PHYAD;//物理伤害附加
                            iAttributeState.MagicAdditionalDamage = skillAttributeStruct.MPAD;//魔法伤害附加
                            iAttributeState.MagicPenetrate = skillAttributeStruct.PEDMG;//魔法伤害穿透
                            iAttributeState.PhysicsPenetrate = skillAttributeStruct.PEDMG;//物理伤害穿透
                            iAttributeState.Mana += skillAttributeStruct.ADDMP;//MP附加
                            iAttributeState.MagicAttacking += skillAttributeStruct.MpAttack * iAttributeState_Base.MagicAttacking / 100;//百分比的魔法攻击力
                            iAttributeState.MagicResistance = skillAttributeStruct.MpDefence * iAttributeState_Base.MagicResistance / 100;//百分比的魔法防御力
                            iAttributeState.ManaRecovery = skillAttributeStruct.MpReload;//魔法回复速度
                            iAttributeState.MagicAffine = skillAttributeStruct.MagicFit;//魔法亲和 
                            iAttributeState.LightFaith = skillAttributeStruct.Light;//光明信仰强度 
                            iAttributeState.DarkFaith = skillAttributeStruct.Dark;//黑暗信仰强度 
                            iAttributeState.LifeFaith = skillAttributeStruct.Life;//生物信仰强度 
                            iAttributeState.NaturalFaith = skillAttributeStruct.Natural;//自然信仰强度
                        }
                    }
                }
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
                _EquipmentChanged = value;
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
    #endregion

}