using System;

/// <summary>
/// 技能释放模式
/// </summary>
public enum EnumReleaseMode
{
    /// <summary>
    /// 不用释放的技能
    /// 大多是被动技能
    /// </summary>
    [FieldExplan("非释放技能")]
    None,
    /// <summary>
    /// 魔法弹的释放方式
    /// 前方一条直线
    /// </summary>
    [FieldExplan("魔法弹类技能")]
    Magic_Bullet,
    /// <summary>
    /// 魔法震荡的释放方式
    /// 指定中心位置（锁定后默认锁定目标，正面法术默认自身中心，伤害魔法默认自身前方向）
    /// </summary>
    [FieldExplan("魔法震荡类技能")]
	Magic_Vibrate,
    /// <summary>
    /// 魔法屏障的释放方式
    /// 默认正前方，按住释放后改变释放方向
    /// </summary>
    [FieldExplan("魔法屏障类技能")]
    Magic_Barrier,
    /// <summary>
    /// 魔力导向的释放方式
    /// 锁定目标释放，如果未锁定则寻找最近目标
    /// </summary>
    [FieldExplan("魔力导向类技能")]
    Magic_Point,
    /// <summary>
    /// 魔力脉冲的释放方式
    /// 前方一条直线发射
    /// </summary>
    [FieldExplan("魔力脉冲类技能")]
    Magic_Pulse,
    /// <summary>
    /// 魔法buff
    /// 给自身加buff
    /// </summary>
    [FieldExplan("魔力Buff类技能")]
    Magic_Buff,
    /// <summary>
    /// 魔法召唤
    /// </summary>
    [FieldExplan("魔法召唤类技能")]
    Magic_Call,
    /// <summary>
    /// 魔法指令动作
    /// </summary>
    [FieldExplan("魔法指令类技能")]
    Magic_Action,

    /// <summary>
    /// 物理buff
    /// 给自身加buff
    /// </summary>
    [FieldExplan("物理Buff类技能")]
    Physics_Buff,
    /// <summary>
    /// 物理攻击动作
    /// </summary>
    [FieldExplan("物理攻击动作")]
    Physics_Attack,
}
