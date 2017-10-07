using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// buff魔法释放结构
/// </summary>
public class SkillMagicBuffStruct : SkillMagicBaseStruct
{
    /// <summary>
    /// buff释放时的粒子特效
    /// </summary>
    [FieldExplan("buff魔法释放时的粒子")]
    public string particleName;

    protected override T Clone<T>(T target)
    {
        SkillMagicBuffStruct temp = target as SkillMagicBuffStruct;
        temp.particleName = particleName;
        return base.Clone(target);
    }
}
