
/// <summary>
/// 魔法技能释放结构
/// </summary>
public class SkillMagicBaseStruct : SkillBaseStruct
{
    /// <summary>
    /// 技能类型
    /// 表示的是本技能
    /// </summary>
    [FieldExplan("技能类型枚举")]
    public EnumSkillType skillType;
    /// <summary>
    /// 该技能是由哪些技能组合出来的，如果不是组合出来的或者这是一个最基础技能，则数组长度位0
    /// </summary>
    [FieldExplan("技能组合数组")]
    public EnumSkillType[] combinSkillTypes;
    /// <summary>
    /// 魔力注入倍率
    /// </summary>
    public float powerRate;

    protected override T Clone<T>(T target)
    {    
        SkillMagicBaseStruct temp = target as SkillMagicBaseStruct;
        temp.skillType = skillType;
        temp.combinSkillTypes = (EnumSkillType[])combinSkillTypes.Clone();
        return base.Clone(target);
    }
}
