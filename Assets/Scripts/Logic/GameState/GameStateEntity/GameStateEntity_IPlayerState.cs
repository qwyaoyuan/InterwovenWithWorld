using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现了IPlayerState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState : IPlayerState
{
    /// <summary>
    /// 玩家状态接口实现对象的加载函数
    /// </summary>
    partial void LoadIPlayerState()
    {
        _Level = playerState.Level;
        _Experience = playerState.Experience;
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
    /// 更新自身属性
    /// 在等级变化 装备变化时触发
    /// 主要更新的是HP MP上限,防御攻击等等随等级装备变化的属性等
    /// </summary>
    public void UpdateAttribute() { }

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