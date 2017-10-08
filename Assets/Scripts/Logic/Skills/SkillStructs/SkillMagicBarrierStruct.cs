using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 障碍魔法释放结构
/// </summary>
public class SkillMagicBarrierStruct : SkillMagicBaseStruct
{
    /// <summary>
    /// 该魔法的粒子
    /// </summary>
    [FieldExplan("障碍魔法的粒子")]
    public string particleName;
    /// <summary>
    /// 障碍魔法的展现方式
    /// </summary>
    [FieldExplan("障碍魔法的展开方式")]
    public EnumSkillMagicBarrierShowType showType;

    protected override T Clone<T>(T target)
    {
        SkillMagicBarrierStruct temp = target as SkillMagicBarrierStruct;
        temp.particleName = particleName;
        temp.showType = showType;
        return base.Clone(target);
    }
}

/// <summary>
/// 障碍魔法显示时的展现方式
/// </summary>
public enum EnumSkillMagicBarrierShowType
{
    /// <summary>
    /// 从中心展开
    /// </summary>
    [FieldExplan("从中心展开")]
    Center,
    /// <summary>
    /// 从前方围绕展开
    /// </summary>
    [FieldExplan("从前方围绕展开")]
    Around
}
