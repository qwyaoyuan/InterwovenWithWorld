
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

