using System;

/// <summary>
/// 技能释放模式
/// </summary>
public enum EnumReleaseMode
{
    /// <summary>
    /// 没有技能
    /// </summary>
    [FieldExplan("没有技能")]
    None,
    /// <summary>
    /// 魔法指令动作
    /// </summary>
    [FieldExplan("魔法指令类技能")]
    Magic_Action,
    /// <summary>
    /// 魔法1
    /// 所有魔法的基本表现形式
    /// </summary>
    [FieldExplan("魔法1,所有魔法的基本表现形式")]
    Magic1,
    /// <summary>
    /// 魔法2
    /// 所有魔法附加的元素属性
    /// </summary>
    [FieldExplan("魔法2,所有魔法附加的元素属性")]
    Magic2,
    /// <summary>
    /// 魔法3
    /// 魔法表现的进阶
    /// </summary>
    [FieldExplan("魔法3,魔法表现的进阶")]
    Magic3,
    /// <summary>
    /// 魔法4
    /// 魔法效果的进阶
    /// </summary>
    [FieldExplan("魔法4,魔法效果的进阶")]
    Magic4,
    /// <summary>
    /// 信仰1
    /// 具体的信仰类型
    /// </summary>
    [FieldExplan("信仰1,具体的信仰类型")]
    Belief1,
    /// <summary>
    /// 信仰2
    /// 信仰魔法的具体效果与表现
    /// </summary>
    [FieldExplan("信仰2,信仰魔法的具体效果与表现")]
    Belief2,
    /// <summary>
    /// 信仰3
    /// 信仰魔法效果的进阶和功能拓展
    /// </summary>
    [FieldExplan("信仰3，信仰魔法效果的进阶和功能拓展")]
    Belief3,
    /// <summary>
    /// 特殊技能
    /// 需要主动释放
    /// </summary>
    [FieldExplan("特殊技能，需要主动释放")]
    Special_Release,
    /// <summary>
    /// 被动技能 
    /// </summary>
    [FieldExplan("被动技能")]
    Passive,
    /// <summary>
    /// 光环技能
    /// 需要主动开关的被动技能
    /// </summary>
    [FieldExplan("光环技能,需要主动开关的被动技能")]
    Special_Circle,

}
