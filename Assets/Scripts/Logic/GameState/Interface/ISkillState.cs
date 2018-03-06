using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 技能运行时状态接口
/// </summary>
public interface ISkillState : IBaseState
{
    /// <summary>
    /// 获取或设置组合技能
    /// 设置:如果传入的长度为0,则清空,否则如果可以组合则附加,如果无法组合则替换;如果处于蓄力阶段,则无法设置新的技能
    /// 获取:返回当前组合的技能数组
    /// </summary>
    SkillBaseStruct[] CombineSkills { get; set; }

    /// <summary>
    /// 开始按住释放魔法键(用于初始化计时)
    /// </summary>
    /// <returns>是否可以释放该技能</returns>
    bool StartCombineSkillRelease();
    /// <summary>
    /// 结束按住(松开)释放魔法键(用于结束计时并释放)
    /// </summary>
    /// <returns>是否可以释放该技能</returns>
    bool EndCombineSkillRelease();
    /// <summary>
    /// 受到攻击后的技能状态改变,如果当前是咏唱状态则打断咏唱,如果当前是释放技能状态则不做处理,如果当前在普通攻击则打断
    /// </summary>
    void GetHitToSkillState();
    /// <summary>
    /// 释放普通技能,如果正在释放其他技能则无法释放 
    /// </summary>
    /// <param name="skillBaseStruct"></param>
    /// <returns>是否可以释放该技能</returns>
    bool ReleaseNormalSkill(SkillBaseStruct skillBaseStruct);
    /// <summary>
    /// 是否开始蓄力
    /// </summary>
    bool IsSkillStartHolding { get; }
    /// <summary>
    /// 技能蓄力时间
    /// </summary>
    float SkillStartHoldingTime { get; }
    /// <summary>
    /// 公共冷却时间
    /// </summary>
    float PublicCoolingTime { get; }
    /// <summary>
    /// 获取技能的冷却时间
    /// </summary>
    /// <param name="skillID"></param>
    /// <returns></returns>
    float GetSkillRuntimeCoolingTime(int skillID);
    /// <summary>
    /// 获取技能最大的冷却时间
    /// </summary>
    /// <param name="skillID"></param>
    /// <returns></returns>
    float GetSkillMaxCoolingTime(int skillID);
    /// <summary>
    /// 设置技能的冷却时间(注意该函数会触发GetSkillCoolingTime的回调,但不会触发自身的回调)
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    void SetSkillCoolingTime(int skillID,float time);
    /// <summary>
    /// 上一次释放魔法的元素类型
    /// 主要是魔法2的类型(风火水土冰雷)
    /// 该字段没有回调事件
    /// </summary>
    EnumSkillType LastMagicElementType { get; }
    //----------光环技能状态-----------//
    /// <summary>
    /// 获取指定光环技能数据
    /// </summary>
    /// <param name="skillType">技能类型</param>
    /// <returns></returns>
    SpecialSkillStateStruct GetSpecialSkillStateStruct(EnumSkillType skillType);
    /// <summary>
    /// 获取当前正在发生变化的光环技能
    /// </summary>
    EnumSkillType SpecialSkillStateChanged { get; }

    //物理技能相关
    /// <summary>
    /// 获取当前物理技能的状态数据
    /// </summary>
    PhysicsSkillStateStruct NowPhysicsSkillStateStruct { get; }
}

/// <summary>
/// 一些特殊(光环)技能状态对象
/// </summary>
public struct SpecialSkillStateStruct
{
    /// <summary>
    /// 技能类型
    /// </summary>
    private EnumSkillType _skillType;
    /// <summary>
    /// 是否打开
    /// </summary>
    private bool _isOpen;
    /// <summary>
    /// 技能等级
    /// </summary>
    private int _skillLevel;
    /// <summary>
    /// 技能对象
    /// </summary>
    private SkillBaseStruct _skillBaseStruct;
    /// <summary>
    /// 数据发生变更后回掉
    /// </summary>
    Action<SpecialSkillStateStruct> _ChangeCallback;

    /// <summary>
    /// 是否打开
    /// </summary>
    public bool IsOpen
    {
        get { return _isOpen; }
        set
        {
            bool tempIsOpen = _isOpen;
            _isOpen = value;
            if (tempIsOpen != IsOpen)
            {
                if (_ChangeCallback != null)
                    _ChangeCallback(this);
            }
        }
    }
    /// <summary>
    /// 技能等级
    /// </summary>
    public int SkillLevel
    {
        get { return _skillLevel; }
        set
        {
            int tempSkillLevel = _skillLevel;
            _skillLevel = value;
            if (tempSkillLevel != _skillLevel)
            {
                if (_ChangeCallback != null)
                    _ChangeCallback(this);
            }
        }
    }
    /// <summary>
    /// 技能对象 
    /// </summary>
    public SkillBaseStruct SkillBaseStruct
    {
        get{ return _skillBaseStruct; }
    }
    /// <summary>
    /// 技能类型
    /// </summary>
    public EnumSkillType SkillType
    {
        get
        {
            return _skillType;
        }
    }

    public SpecialSkillStateStruct(EnumSkillType skillType, bool isOpen, int skillLevel, SkillBaseStruct skillBaseStruct, Action<SpecialSkillStateStruct> ChangeCallback)
    {
        this._skillType = skillType;
        this._isOpen = isOpen;
        this._skillLevel = skillLevel;
        this._skillBaseStruct = skillBaseStruct;
        this._ChangeCallback = ChangeCallback;
    }
}

/// <summary>
/// 物理技能状态整合结构
/// </summary>
public class PhysicsSkillStateStruct
{
    /// <summary>
    /// 该技能的类型
    /// </summary>
    public EnumSkillType SkillType;
    /// <summary>
    /// 释放此技能时的数据状态
    /// </summary>
    public IAttributeState AttributeState;
    /// <summary>
    /// 蓄力进度(0-1)
    /// </summary>
    public float StorageSchedule;
}