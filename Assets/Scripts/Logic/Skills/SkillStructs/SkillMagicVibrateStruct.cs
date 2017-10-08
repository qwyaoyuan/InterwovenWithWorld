using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔法震荡的释放结构
/// </summary>
public class SkillMagicVibrateStruct : SkillMagicBaseStruct
{
    /// <summary>
    /// 魔法震荡释放时的粒子特效
    /// </summary>
    [FieldExplan("魔法震荡释放时的粒子","魔法震荡范围内的特效","魔法震荡攻击到敌人时的特效")]
    public string[] particleName;

    protected override T Clone<T>(T target)
    {
        SkillMagicVibrateStruct temp = target as SkillMagicVibrateStruct;
        temp.particleName = particleName;
        return base.Clone(target);
    }
}
