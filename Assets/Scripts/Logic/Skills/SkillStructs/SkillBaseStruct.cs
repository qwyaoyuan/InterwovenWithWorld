using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 技能基础数据（只包含最基础的技能，组合技能拆开计算）
/// </summary>
public class SkillBaseStruct
{
    /// <summary>
    /// 技能id（唯一标识）
    /// </summary>
    public string id;
    /// <summary>
    /// 技能名
    /// </summary>
    public string name;
    /// <summary>
    /// 技能类型，具体的每个技能
    /// </summary>
    public EnumSkillType skillType;
    /// <summary>
    /// 技能的释放模式，标识这是魔法1 还是魔法2 或者是信仰1之类的
    /// </summary>
    public EnumReleaseMode skillMode;
    /// <summary>
    /// 该技能包含的粒子
    /// </summary>
    //public GameObject[] particals;
    /// <summary>
    /// 技能的信仰类型(如果非信仰的技能释放模式，则忽略该字段 )
    /// </summary>
    public EnumSkillBelief skillBelief;
    /// <summary>
    /// 技能的特殊效果
    /// </summary>
    public EnumStatusEffect[] skillStatusEffect;
    /// <summary>
    /// 技能的最高等级 
    /// </summary>
    public int maxLevel;
    /// <summary>
    /// 技能的属性结构数组
    /// 数据长度与技能的最高等级一致
    /// </summary>
    public SkillAttributeStruct[] skillAttributeStructs;

    /// <summary>
    /// 技能的前置条件
    /// </summary>
    public SkillPrecondition skillPrecondition;

    /// <summary>
    /// 技能所属分组数组
    /// </summary>
    public EnumSkillZone[] skillZones;

    //运行时生成的数据↓↓↓
    /// <summary>
    /// 技能的图标
    /// </summary>
    public Sprite skillSprite;
    /// <summary>
    /// 技能显示的名字(根据语言进行翻译)
    /// </summary>
    public string skillName;
}

/// <summary>
/// 技能的前置条件
/// </summary>
public class SkillPrecondition
{
    /// <summary>
    /// 需求某组技能投入的总点数
    /// </summary>
    public Dictionary<EnumSkillZone, int> mustSkillZonePointDic;
    /// <summary>
    /// 需求某技能投入的点数
    /// </summary>
    public Dictionary<EnumSkillType, int> mustSkillPointDic;
}

/// <summary>
/// 技能的属性结构
/// </summary>
public class SkillAttributeStruct
{
    /// <summary>
    /// 最大耗魔上限
    /// </summary>
    [FieldExplan("最大耗魔上限", "MaxMP")] public int MaxMP;
    /// <summary>
    /// 法力上限加成百分比
    /// </summary>
    [FieldExplan("法力上限加成百分比", "MP")] public int MP;
    /// <summary>
    /// 法伤
    /// </summary>
    [FieldExplan("法伤", "DMG")] public int DMG;
    /// <summary>
    /// 特效影响力(伤害-->点燃 减抗性等)
    /// </summary>
    [FieldExplan("特效影响力(伤害-->点燃 减抗性等)", "ERST")] public int ERST;
    /// <summary>
    /// 驻留时间(持续时间-->特效 buff 屏障 范围伤害持续)
    /// </summary>
    [FieldExplan("驻留时间(持续时间-->特效 buff 屏障 范围伤害持续)", "RETI")] public int RETI;
    /// <summary>
    /// 物理伤害附加
    /// </summary>
    [FieldExplan("物理伤害附加", "PHYAD")] public int PHYAD;
    /// <summary>
    /// 魔法伤害附加
    /// </summary>
    [FieldExplan("魔法伤害附加", "MPAD")] public int MPAD;
    /// <summary>
    /// 伤害穿透
    /// </summary>
    [FieldExplan("伤害穿透", "PEDMG")] public int PEDMG;
    /// <summary>
    /// HP附加
    /// </summary>
    [Obsolete("该属性已经过时", true)]
    [FieldExplan("HP附加", "ADDHP")]
    public int ADDHP;
    /// <summary>
    /// MP附加
    /// </summary>
    [FieldExplan("MP附加", "ADDMP")] public int ADDMP;
    /// <summary>
    /// 魔法攻击力加成百分比
    /// </summary>
    [FieldExplan("魔法攻击力加成百分比", "MpAttack")] public int MpAttack;
    /// <summary>
    /// 魔法防御力加成百分比
    /// </summary>
    [FieldExplan("魔法防御力加成百分比", "MpDefence")] public int MpDefence;
    /// <summary>
    /// 魔法回复速度加成
    /// </summary>
    [FieldExplan("魔法回复速度加成", "MpReload")] public int MpReload;
    /// <summary>
    /// 魔法亲和性
    /// </summary>
    [FieldExplan("魔法亲和性", "MagicFit")] public int MagicFit;
    /// <summary>
    /// 光明信仰强度
    /// </summary>
    [FieldExplan("光明信仰强度", "光明信仰强度")] public int Light;
    /// <summary>
    /// 黑暗信仰强度
    /// </summary>
    [FieldExplan("黑暗信仰强度", "黑暗信仰强度")] public int Dark;
    /// <summary>
    /// 生物信仰强度
    /// </summary>
    [FieldExplan("生物信仰强度", "生物信仰强度")] public int Life;
    /// <summary>
    /// 自然信仰强度
    /// </summary>
    [FieldExplan("自然信仰强度", "自然信仰强度")] public int Natural;
    /// <summary>
    /// 技能特效等级 0 表示岁当前技能等级  -1表示没有特效  大于0表示有特效且等级为该数值
    /// </summary>
    [FieldExplan("技能特效等级", "SkillSpecialLevel")] public int SkillSpecialLevel;
}
