using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject[] particals;
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
    /// 技能所属分组
    /// </summary>
    public EnumSkillZone skillZone;

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
    /// 基础耗魔单位
    /// </summary>
    public int MP;
    /// <summary>
    /// 法伤
    /// </summary>
    public int DMG;
    /// <summary>
    /// 特效影响力
    /// </summary>
    public int ERST;
    /// <summary>
    /// 驻留时间
    /// </summary>
    public int RETI;
    /// <summary>
    /// 物理伤害附加
    /// </summary>
    public int PHYAD;
    /// <summary>
    /// 魔法伤害附加
    /// </summary>
    public int MPAD;
    /// <summary>
    /// 伤害穿透
    /// </summary>
    public int PEDMG;
    /// <summary>
    /// HP附加
    /// </summary>
    public int ADDHP;
    /// <summary>
    /// MP附加
    /// </summary>
    public int ADDMP;
}
