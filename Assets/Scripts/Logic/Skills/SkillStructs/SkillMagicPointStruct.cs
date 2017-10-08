
/// <summary>
/// 导向魔法释放结构
/// </summary>
public class SkillMagicPointStruct : SkillMagicBaseStruct
{
    /// <summary>
    /// 导向魔法释放时的粒子特效
    /// </summary>
    [FieldExplan("导向魔法在怪物身上显现的粒子")]
    public string particleNames;

    protected override T Clone<T>(T target)
    {
        SkillMagicPointStruct temp = target as SkillMagicPointStruct;
        temp.particleNames = particleNames;
        return base.Clone(target);
    }
}
