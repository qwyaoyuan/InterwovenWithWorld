using System;

/// <summary>
/// 技能的基础结构
/// </summary>
public class SkillBaseStruct : ICloneable
{
    /// <summary>
    /// 技能名
    /// </summary>
    [FieldExplan("技能名")]
    public string skillName;
    /// <summary>
    /// 技能id
    /// </summary>
    [FieldExplan("技能ID")]
    public int id;
    /// <summary>
    /// 技能的释放方式
    /// </summary>
    [FieldExplan("技能的释放方式")]
    public EnumReleaseMode releaseMode;

    /// <summary>
    /// 技能的释放组合技能
    /// 表示技能是组合技能还是单独的技能
    /// </summary>
    [FieldExplan("技能的释放组合类型")]
    public EnumReleaseType releaseType;

    /// <summary>
    /// 克隆
    /// </summary>
    /// <param name="target">对象</param>
    protected virtual T Clone<T>(T target) where T : SkillBaseStruct
    {
        target.skillName = skillName;
        target.id = id;
        target.releaseMode = releaseMode;
        return target;
    }

    /// <summary>
    /// 克隆
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
        return Clone((SkillBaseStruct)Activator.CreateInstance(GetType()));
    }
}
