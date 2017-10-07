using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔法弹魔法释放结构
/// </summary>
public class SkillMagicBulletStruct : SkillMagicBaseStruct
{
    /// <summary>
    /// 魔法弹释放时的粒子特效
    /// </summary>
    [FieldExplan("魔法弹释放时的粒子特效","魔法弹飞行时的粒子","魔法弹爆炸时的粒子")]
    public string[] particleName;

    protected override T Clone<T>(T target)
    {
        SkillMagicBulletStruct temp = target as SkillMagicBulletStruct;
        temp.particleName = particleName;
        return base.Clone(target);
    }
}
