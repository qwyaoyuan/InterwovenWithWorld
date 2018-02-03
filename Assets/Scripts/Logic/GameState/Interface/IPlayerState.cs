using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于保存玩家运行时的状态
/// 比如是Buff与Debuff 动画的状态 玩家当前的属性（将所有属性叠加后）
/// </summary>
public interface IPlayerState : IBaseState,
    IPlayerAttributeState, IBuffState, IDebuffState, ISpecialState, IAnimatorState, ISkillState, IDamage
{
    /// <summary>
    /// 玩家操纵角色的游戏对象
    /// </summary>
    GameObject PlayerObj { get; set; }
    /// <summary>
    /// 玩家的摄像机
    /// </summary>
    Camera PlayerCamera { get; set; }
    /// <summary>
    /// 玩家自身的种族成长数据
    /// </summary>
    RoleOfRaceInfoStruct SelfRoleOfRaceInfoStruct { get; }
    /// <summary>
    /// 物理检测脚本
    /// </summary>
    PhysicSkillInjuryDetection PhysicSkillInjuryDetection { get; set; }

    /// <summary>
    /// 当前选择的目标 
    /// </summary>
    GameObject SelectObj { get; set; }
    /// <summary>
    /// 更新属性
    /// </summary>
    void UpdateAttribute();
    /// <summary>
    /// 等级
    /// </summary>
    int Level { get; }
    /// <summary>
    /// 经验值(本级的)
    /// </summary>
    int Experience { get; set; }
    /// <summary>
    /// 最大经验值(本级的)
    /// </summary>
    int MaxExperience { get; set; }
    /// <summary>
    /// 技能等级变化
    /// </summary>
    bool SkillLevelChanged { get; set; }
    /// <summary>
    /// 种族等级变化
    /// </summary>
    bool RaceLevelChanged { get; set; }
    /// <summary>
    /// 装备发生变化
    /// </summary>
    bool EquipmentChanged { get; set; }
    /// <summary>
    /// 物品发生变化
    /// </summary>
    bool GoodsChanged { get; set; }
    /// <summary>
    /// 金钱发生变化
    /// </summary>
    bool SpriteChanged { get; set; }
    /// <summary>
    /// 当前的按键应该遵循的状态
    /// </summary>
    EnumKeyContactDataZone KeyContactDataZone { get; }
    /// <summary>
    /// 触碰到目标的结构(主要是NPC 素材等)
    /// </summary>
    TouchTargetStruct TouchTargetStruct { get; set; }
    /// <summary>
    /// 用于初始化角色属性状态(主要用于更新玩属性后,有些字段如血量魔法值等需要等值为最大血量和魔法值)
    /// </summary>
    void InitPlayAttributeState();

    //--------------战斗状态----------------//
    /// <summary>
    /// 上一次进入战斗状态的时间
    /// 如果离开战斗状态则写入负值的当前时间
    /// 如果进入战斗则写入当前的游戏时间
    /// </summary>
    float LastIntoBattleTime { get; set; }
    /// <summary>
    /// 上一次暴击的时间
    /// 默认为负值,且只在初始化时设置为负值
    /// 如果攻击造成暴击则写入当前的游戏时间
    /// </summary>
    float LastCriticalHitTime { get; set; }
    /// <summary>
    /// 上一次闪避的时间
    /// 默认为负值,且只在初始化时设置为负值
    /// 如果闪避攻击则写入当前的游戏时间
    /// </summary>
    float LastDodgeTime { get; set; }
    /// <summary>
    /// 武器的类型(玩家状态使用的枚举)发生变化
    /// </summary>
    EnumWeaponTypeByPlayerState WeaponTypeByPlayerState { get; set; }
    /// <summary>
    /// 上一次切换武器(类型)的时间 
    /// 默认为赋值,且只在初始化时设置为负值
    /// 如果切换则会修改改时间并通知注册方
    /// </summary>
    float LastChangeWeaponTime { get; }

    //--------------------辅助函数--------------------
    /// <summary>
    /// 根据技能类型和技能存档数据获取该技能的当前等级数据
    /// 返回技能当前等级的数据(如果没有加点或者出错则范围null)
    /// </summary>
    /// <param name="skillType">技能类型</param>
    /// <param name="skillStructData">技能固有数据</param>
    /// <returns>技能当前等级的数据(如果没有加点或者出错则范围null)</returns>
    SkillAttributeStruct GetSkillAttributeStruct(EnumSkillType skillType, SkillStructData skillStructData);
}
/// <summary>
/// 武器的类型(玩家状态使用的枚举)
/// </summary>
public enum EnumWeaponTypeByPlayerState
{
    /// <summary>
    /// 没有武器
    /// </summary>
    None = 0,
    /// <summary>
    /// 单手剑
    /// </summary>
    SingleHandedSword = 1,
    /// <summary>
    /// 双手剑
    /// </summary>
    TwoHandedSword = 2,
    /// <summary>
    /// 弓
    /// </summary>
    Arch = 4,
    /// <summary>
    /// 弩
    /// </summary>
    CrossBow = 8,
    /// <summary>
    /// 盾
    /// </summary>
    Shield = 16,
    /// <summary>
    /// 匕首
    /// </summary>
    Dagger = 32,
    /// <summary>
    /// 长杖
    /// </summary>
    LongRod = 64,
    /// <summary>
    /// 短杖
    /// </summary>
    ShortRod = 128,
    /// <summary>
    /// 水晶球
    /// </summary>
    CrystalBall = 256,
}

/// <summary>
/// 触碰到对方的结果
/// </summary>
public struct TouchTargetStruct
{
    /// <summary>
    /// 触碰到对方的类型
    /// </summary>
    public EnumTouchTargetType TouchTargetType;
    /// <summary>
    /// 在那个地图上
    /// </summary>
    public string TerrainName;
    /// <summary>
    /// 对方的id
    /// </summary>
    public int ID;

    /// <summary>
    /// 接触到对方的类型
    /// 用于判断前方是npc还是素材或者都没有
    /// </summary>
    public enum EnumTouchTargetType
    {
        /// <summary>
        /// 没有碰触到任何东西
        /// </summary>
        None,
        /// <summary>
        /// 触碰到NPC
        /// </summary>
        NPC,
        /// <summary>
        /// 触碰到素材原料
        /// </summary>
        Stuff,
        /// <summary>
        /// 触碰到保存点
        /// </summary>
        Save
    }

    public static bool Equals(TouchTargetStruct source, TouchTargetStruct target)
    {
        if (source.TouchTargetType != target.TouchTargetType)
            return false;
        if (source.ID != target.ID)
            return false;
        if (!string.Equals(source.TerrainName, target.TerrainName))
            return false;
        return true;
    }
}

