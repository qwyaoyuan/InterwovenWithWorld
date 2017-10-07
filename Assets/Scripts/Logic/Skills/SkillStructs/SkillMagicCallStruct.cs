using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 召唤魔法释放结构
/// </summary>
public class SkillMagicCallStruct : SkillMagicBaseStruct
{
    /// <summary>
    /// 召唤时的粒子特效
    /// </summary>
    [FieldExplan("召唤生物周围的粒子")]
    public string particleName;

    /// <summary>
    /// 召唤物预设体的名字
    /// </summary>
    [FieldExplan("召唤物预设体的名字")]
    public string callTargetPrefabName;

    protected override T Clone<T>(T target)
    {
        SkillMagicCallStruct temp = target as SkillMagicCallStruct;
        temp.particleName = particleName;
        return base.Clone(target);
    }

}
