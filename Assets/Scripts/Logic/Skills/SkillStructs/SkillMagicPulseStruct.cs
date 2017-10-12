
/// <summary>
/// 脉冲魔法释放结构
/// </summary>
public class SkillMagicPulseStruct : SkillMagicBaseStruct
{
    /// <summary>
    /// 脉冲魔法释放时的粒子特效
    /// </summary>
    [FieldExplan("冒充魔法释放时的粒子","魔法向前脉冲时的特效","魔法攻击到敌人时的特效")]
    public string[] particleNames;

    protected override T Clone<T>(T target)
    {
        SkillMagicPulseStruct temp = target as SkillMagicPulseStruct;
        temp.particleNames = particleNames;
        return base.Clone(target);
    }
}
