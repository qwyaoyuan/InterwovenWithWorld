using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 实现了IPlayerState->IPlayerAttributeState接口的GameState类的一个分支实体
/// 下标0表示自身
/// </summary>
public partial class GameState
{
    /// <summary>
    /// 保存临时属性的字典
    /// </summary>
    private Dictionary<int, IAttributeState> iAttributeStateDic;
    /// <summary>
    /// 属性操作句柄的当前下标
    /// </summary>
    int iAttributeHandleIndex;

    /// <summary>
    /// 用于初始化角色属性的对象 
    /// </summary>
    partial void Start_IPlayerState_IAttribute()
    {
        iAttributeStateDic = new Dictionary<int, IAttributeState>();
        iAttributeHandleIndex = 0;
        //构建自身的基础属性(下标是0)
        CreateAttributeHandle(0);
        //构建装备属性(下表是1)
        CreateAttributeHandle(1);
        //构建技能的属性(注意技能的属性从负数开始)
        //有些技能只存在特殊效果,而且这些特殊效果不涉及这些属性,则这些特殊效果在具体的位置处理
        //被动技能  注:光环技能不需要初始化,因为光环是动态的
        #region 魔法
        CreateAttributeHandle(-(int)EnumSkillType.FS05);
        CreateAttributeHandle(-(int)EnumSkillType.FS06);
        CreateAttributeHandle(-(int)EnumSkillType.FS07);
        CreateAttributeHandle(-(int)EnumSkillType.FS08);
        CreateAttributeHandle(-(int)EnumSkillType.FS10);
        CreateAttributeHandle(-(int)EnumSkillType.YSX05);
        CreateAttributeHandle(-(int)EnumSkillType.YSX08);
        CreateAttributeHandle(-(int)EnumSkillType.MFS03);
        CreateAttributeHandle(-(int)EnumSkillType.MFS07);
        CreateAttributeHandle(-(int)EnumSkillType.SM01);
        CreateAttributeHandle(-(int)EnumSkillType.SM05);
        CreateAttributeHandle(-(int)EnumSkillType.MS07);
        CreateAttributeHandle(-(int)EnumSkillType.DFS01);
        CreateAttributeHandle(-(int)EnumSkillType.DFS02);
        CreateAttributeHandle(-(int)EnumSkillType.DSM01);
        CreateAttributeHandle(-(int)EnumSkillType.DSM09);
        CreateAttributeHandle(-(int)EnumSkillType.JS01);
        CreateAttributeHandle(-(int)EnumSkillType.JS02);
        CreateAttributeHandle(-(int)EnumSkillType.JH01);
        CreateAttributeHandle(-(int)EnumSkillType.JH02);
        CreateAttributeHandle(-(int)EnumSkillType.JH03);
        CreateAttributeHandle(-(int)EnumSkillType.DSM09);
        #endregion
        #region 物理
        CreateAttributeHandle(-(int)EnumSkillType.WL02);
        CreateAttributeHandle(-(int)EnumSkillType.WL03);
        CreateAttributeHandle(-(int)EnumSkillType.WL04);
        CreateAttributeHandle(-(int)EnumSkillType.ZS01);
        CreateAttributeHandle(-(int)EnumSkillType.WL02);
        CreateAttributeHandle(-(int)EnumSkillType.ZS04);
        CreateAttributeHandle(-(int)EnumSkillType.GJS01);
        CreateAttributeHandle(-(int)EnumSkillType.GJS04);
        CreateAttributeHandle(-(int)EnumSkillType.KZS01);
        CreateAttributeHandle(-(int)EnumSkillType.KZS02);
        CreateAttributeHandle(-(int)EnumSkillType.JAS01);
        CreateAttributeHandle(-(int)EnumSkillType.JAS02);
        CreateAttributeHandle(-(int)EnumSkillType.YX01);
        CreateAttributeHandle(-(int)EnumSkillType.YX03);
        CreateAttributeHandle(-(int)EnumSkillType.DZ01);
        CreateAttributeHandle(-(int)EnumSkillType.DZ02);
        CreateAttributeHandle(-(int)EnumSkillType.SSS01);
        CreateAttributeHandle(-(int)EnumSkillType.SSS02);
        #endregion
    }

    #region 用于操纵附加状态的功能
    /// <summary>
    /// 创建一个状态句柄,返回句柄的id
    /// </summary>
    /// <returns>如果返回的句柄小于定于0则表示失败了(小于等于0的句柄被用于内部)</returns>
    public int CreateAttributeHandle()
    {
        int testCount = 0;//当前尝试次数
        ReTest:
        iAttributeHandleIndex++;
        testCount++;//每次执行尝试次数加1
        if (CreateAttributeHandle(iAttributeHandleIndex))
        {
            return iAttributeHandleIndex;
        }
        else if (testCount < 100)//如果尝试次数小于100则可以继续尝试
        {
            if (iAttributeHandleIndex == int.MaxValue)
            {
                iAttributeHandleIndex = 0;
            }
            goto ReTest;
        }
        return 0;
    }

    /// <summary>
    /// 使用指定的句柄id创建一个状态
    /// 如果存在这个句柄了,则不会创建(这个一般不要用)
    /// </summary>
    /// <param name="index">句柄ID</param>
    /// <returns>返回是否创建成功</returns>
    public bool CreateAttributeHandle(int index)
    {
        if (iAttributeStateDic == null)
            iAttributeStateDic = new Dictionary<int, IAttributeState>();
        if (iAttributeStateDic.ContainsKey(index))
            return false;
        IAttributeState iAttributeState = new AttributeStateAdditional();
        iAttributeStateDic.Add(index, iAttributeState);
        //添加回调
        iAttributeState.Registor<IAttributeState>((target, fieldName) =>
        {
            Call<IPlayerAttributeState>(fieldName);
        });
        return true;
    }

    /// <summary>
    /// 通过句柄获取对应的状态对象
    /// </summary>
    /// <param name="handle">状态句柄</param>
    /// <returns></returns>
    public IAttributeState GetAttribute(int handle)
    {
        if (iAttributeStateDic != null && iAttributeStateDic.ContainsKey(handle))
            return iAttributeStateDic[handle];
        return null;
    }

    /// <summary>
    /// 获取合计的属性
    /// </summary>
    /// <returns></returns>
    public IAttributeState GetResultAttribute()
    {
        AttributeStateAdditional result = new AttributeStateAdditional();
        //使用反射进行数据整合
        Type t = typeof(IAttributeState);
        PropertyInfo[] infos = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo info in infos)
        {
            MethodInfo setInfo = info.GetSetMethod();
            MethodInfo getInfo = info.GetGetMethod();
            object value = getInfo.Invoke(this, null);
            setInfo.Invoke(result, new object[] { value });
        }
        return result;
    }

    /// <summary>
    /// 移除一个状态,注意只能移除大于零的句柄(光环除外)
    /// </summary>
    /// <param name="handle"></param>
    public void RemoveAttribute(int handle)
    {
        if (iAttributeStateDic != null && iAttributeStateDic.ContainsKey(handle))
        {
            iAttributeStateDic.Remove(handle);
            Call<IPlayerAttributeState, Action<int>>(temp => temp.RemoveAttribute);//通知属性变动
        }
    }

    /// <summary>
    /// 从技能对象中取出数据设置到状态对象中
    /// </summary>
    /// <param name="iAttributeState">要设置数据的状态对象</param>
    /// <param name="baseAttributeState">玩家的基础状态对象</param>
    /// <param name="skillAttributeStruct">技能属性对象</param>
    public void SetIAttributeStateDataBySkillData(IAttributeState iAttributeState, IAttributeState baseAttributeState, SkillAttributeStruct skillAttributeStruct)
    {
        if (iAttributeState == null || baseAttributeState == null || skillAttributeStruct == null)
        {
            if (iAttributeState != null)
                iAttributeState.Init();
            return;
        }
        iAttributeState.Power = skillAttributeStruct.StrengthAdded;//力量加成
        iAttributeState.Quick = skillAttributeStruct.AgileBonus;//敏捷加成
        iAttributeState.Power += skillAttributeStruct.LiftingForceRatio * baseAttributeState.Power / 100f;//力量百分比提升
        iAttributeState.Mental = skillAttributeStruct.RaiseSpiritRatio * baseAttributeState.Mental / 100f;//精神百分比提升
        iAttributeState.MagicAttacking = skillAttributeStruct.DMG * baseAttributeState.MagicAttacking / 100f;//魔法攻击力
        iAttributeState.PhysicsAttacking = skillAttributeStruct.PDMG * baseAttributeState.PhysicsAttacking / 100f;//物理攻击力
        iAttributeState.EffectAffine = skillAttributeStruct.ERST;//特效影响力
        iAttributeState.AttackRigidity = skillAttributeStruct.Catalepsy;//僵直
        iAttributeState.EffectResideTime = skillAttributeStruct.RETI;//特效驻留时间
        iAttributeState.PhysicsAdditionalDamage = skillAttributeStruct.PHYAD;//物理伤害附加
        iAttributeState.MagicAdditionalDamage = skillAttributeStruct.MPAD;//魔法伤害附加
        iAttributeState.EvadeRate = skillAttributeStruct.DodgeRate / 100f;//闪避率
        iAttributeState.MagicPenetrate = skillAttributeStruct.PEDMG;//魔法伤害穿透
        iAttributeState.PhysicsPenetrate = skillAttributeStruct.PEDMG;//物理伤害穿透
        iAttributeState.MaxUseMana = skillAttributeStruct.MaxMP;//最大耗魔上限
        iAttributeState.MaxMana += skillAttributeStruct.MP * baseAttributeState.MaxMana / 100f;//百分比的法力上限
        iAttributeState.MagicFit = skillAttributeStruct.MagicFit;//魔法亲和 
        iAttributeState.MagicAttacking += skillAttributeStruct.MpAttack * baseAttributeState.MagicAttacking / 100f;//百分比的魔法攻击力
        iAttributeState.MagicResistance += skillAttributeStruct.MpDefence * baseAttributeState.MagicResistance / 100f;//百分比的魔法防御力
        iAttributeState.PhysicsAttacking += skillAttributeStruct.PhyAttack * baseAttributeState.PhysicsAttacking / 100f;//物理攻击力百分比加成
        iAttributeState.PhysicsResistance += skillAttributeStruct.PhyDefense * baseAttributeState.PhysicsResistance / 100f;//物理防御力百分比加成
        iAttributeState.ManaRecovery += skillAttributeStruct.MpReload;//魔法回复速度
        iAttributeState.LightFaith = skillAttributeStruct.Light;//光明信仰强度 
        iAttributeState.DarkFaith = skillAttributeStruct.Dark;//黑暗信仰强度 
        iAttributeState.LifeFaith = skillAttributeStruct.Life;//生物信仰强度 
        iAttributeState.NaturalFaith = skillAttributeStruct.Natural;//自然信仰强度
        iAttributeState.ExemptionChatingMana = skillAttributeStruct.ExemptionChatingMana;//减少技能冷却时间
        iAttributeState.SameElementEffectAdded = skillAttributeStruct.SameElementEffectAdded / 100f;//同元素魔法效果加成
        iAttributeState.ReliefManaAmount = skillAttributeStruct.ReliefManaAmount / 100f;//耗魔量减免(百分比)
        iAttributeState.AbnormalStateResistance = skillAttributeStruct.AbnormalStateResistance;//异常状态抗性
        iAttributeState.MoveSpeed = skillAttributeStruct.MoveSpeedAddtion * baseAttributeState.MoveSpeed / 100f;//移动速度(百分比增加量)
        iAttributeState.ExemptionChantingTime = skillAttributeStruct.ExemptionChatingTime / 100f;//咏唱时间减免(百分比)
        iAttributeState.ReduceCoolingTime = skillAttributeStruct.ReduceCoolingTime / 100f;//公共冷却时间减免(百分比)
        iAttributeState.CritRate = skillAttributeStruct.IncreasedCritRate / 100f;//暴击率
        iAttributeState.TrapDefense = skillAttributeStruct.TrapDefense;//对陷阱防御力
        iAttributeState.SpellTrapDamage += skillAttributeStruct.SpellTrapDamagePromotion / 100f;//法术陷阱伤害提升(百分比)
        iAttributeState.SpellTrapEffectProbability = skillAttributeStruct.SpellTrapEffectProbability / 100f;//法术陷阱产生特效几率
        iAttributeState.DamageToTheUndead = skillAttributeStruct.DamageToTheUndead / 100f;//对不死族伤害提升(百分比倍率)
        iAttributeState.ChaosOfTheUndead = skillAttributeStruct.ChaosOfTheUndead / 100f;//对不死族附加混乱几率
        iAttributeState.TreatmentVolume = skillAttributeStruct.TreatmentVolume;//治疗量 //治疗量需要一个公式来计算
        iAttributeState.MysticalBeliefIntensity = skillAttributeStruct.MysticalBeliefIntensity;//神秘信仰强度
        iAttributeState.MysticalBeliefSpecialEffects = skillAttributeStruct.MysticalBeliefSpecialEffects / 100f;//神秘信仰特效产生几率
        iAttributeState.LightFaith += baseAttributeState.LightFaith * skillAttributeStruct.IncreaseFaithA / 100f;//提升信仰1->光明信仰强度
        iAttributeState.DarkFaith += baseAttributeState.DarkFaith * skillAttributeStruct.IncreaseFaithA / 100f;//提升信仰1->黑暗信仰强度
        iAttributeState.LifeFaith += baseAttributeState.LifeFaith * skillAttributeStruct.IncreaseFaithA / 100f;//提升信仰1->生物信仰强度
        iAttributeState.NaturalFaith += baseAttributeState.NaturalFaith * skillAttributeStruct.IncreaseFaithA / 100f;//提升信仰1->自然信仰强度
        iAttributeState.ImproveWorshipFaith = skillAttributeStruct.ImproveWorshipFaith;//崇拜信仰强度
        iAttributeState.AccelerateToUndead = skillAttributeStruct.AccelerateToUndead / 100f;//对不死族加速比率
        //根据信仰强度差值获得额外魔法效果加成以及魔法三(包括)以上魔法伤害加成的数据计算在具体的魔法释放处进行处理
        iAttributeState.ElementStandStrength = skillAttributeStruct.ElementStandStrength;//元素立场强度
        //非战斗状态移动速度提升在具体的技能出进行计算
        iAttributeState.ExperienceValuePlus = skillAttributeStruct.ExperienceValuePlus / 100f;//经验值加成比率
        iAttributeState.AttackSpeed = skillAttributeStruct.AttackSpeed / 100f;//攻击速度(百分比)
        iAttributeState.GooodsDropRate = baseAttributeState.GooodsDropRate * skillAttributeStruct.GooodsDropRate / 100f;//物品掉落率加成(百分比);
        iAttributeState.HitRate = skillAttributeStruct.HitRate / 100f;//命中率(百分比)
        iAttributeState.View = skillAttributeStruct.ViewMul * baseAttributeState.View / 100f;//视野加成
        iAttributeState.MustUsedBaseMana = skillAttributeStruct.MustUsedBaseMana;//技能基础耗魔
        iAttributeState.CoolingTime = skillAttributeStruct.CoolingTime;//冷却时间
    }

    #endregion

    /// <summary>
    /// 没有任何作用
    /// </summary>
    public void Init()
    { }

    #region IAttributeState 属性状态
    #region 基础属性
    /// <summary>
    /// 敏捷
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float Quick
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.Quick).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.Quick = value;
            }
        }
    }

    /// <summary>
    /// 精神
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float Mental
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.Mental).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.Mental = value;
            }
        }
    }

    /// <summary>
    /// 力量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float Power
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.Power).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.Power = value;
            }
        }
    }

    /// <summary>
    /// 基础物理护甲
    /// </summary>
    public float BasePhysicDefense
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.BasePhysicDefense).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.BasePhysicDefense = value;
            }
        }
    }

    /// <summary>
    /// 基础物理伤害
    /// </summary>
    public float BasePhysicDamage
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.BasePhysicDamage).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.BasePhysicDamage = value;
            }
        }
    }

    #endregion
    #region 常规属性
    /// <summary>
    /// 血量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float HP
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.HP).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.HP = value;
            }
        }
    }

    /// <summary>
    /// 最大血量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MaxHP
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MaxHP).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MaxHP = value;
            }
        }
    }

    /// <summary>
    /// 魔力量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float Mana
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.Mana).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.Mana = value;
            }
        }
    }

    /// <summary>
    /// 最大魔力量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MaxMana
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MaxMana).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MaxMana = value;
            }
        }
    }

    /// <summary>
    /// 最大耗魔上限
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MaxUseMana
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MaxUseMana).Sum();
        }

        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MaxUseMana = value;
            }
        }
    }

    /// <summary>
    /// 精神力计量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float Mentality
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.Mentality).Sum();
        }

        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.Mentality = value;
            }
        }
    }

    /// <summary>
    /// 最大精神力计量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MaxMentality
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MaxMentality).Sum();
        }

        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MaxMentality = value;
            }
        }
    }

    /// <summary>
    /// 心志力计量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MindTraining
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MindTraining).Sum();
        }

        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MindTraining = value;
            }
        }
    }

    /// <summary>
    /// 最大心志力计量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MaxMindTraining
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MaxMindTraining).Sum();
        }

        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MaxMindTraining = value;
            }
        }
    }

    /// <summary>
    /// 视野范围
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float View
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.View).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.View = value;
            }
        }
    }

    /// <summary>
    /// 降低被怪物发现的概率(被发现的距离倍率)
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float SightDef
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.SightDef).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.SightDef = value;
            }
        }
    }

    /// <summary>
    /// 移动速度
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MoveSpeed
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MoveSpeed).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MoveSpeed = value;
            }
        }
    }

    /// <summary>
    /// 攻击速度
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float AttackSpeed
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.AttackSpeed).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.AttackSpeed = value;
            }
        }
    }

    /// <summary>
    /// 命中率
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float HitRate
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.HitRate).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.HitRate = value;
            }
        }
    }

    /// <summary>
    /// 闪避率
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float EvadeRate
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.EvadeRate).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.EvadeRate = value;
            }
        }
    }

    /// <summary>
    /// 暴击率
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float CritRate
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.CritRate).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.CritRate = value;
            }
        }
    }
    #endregion
    #region 回复
    /// <summary>
    /// 生命恢复速度
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float LifeRecovery
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.LifeRecovery).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.LifeRecovery = value;
            }
        }
    }

    /// <summary>
    /// 法力恢复速度
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float ManaRecovery
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.ManaRecovery).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ManaRecovery = value;
            }
        }
    }
    #endregion
    #region 攻击与防御属性
    /// <summary>
    /// 伤害格挡率
    /// </summary>
    public float EquipBlock
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.EquipBlock).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.EquipBlock = value;
            }
        }
    }

    /// <summary>
    /// 暴击率伤害减少率
    /// </summary>
    public float CriticalDef
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.CriticalDef).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.CriticalDef = value;
            }
        }
    }

    /// <summary>
    /// 攻击僵直
    /// </summary>
    public float AttackRigidity
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.AttackRigidity).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.AttackRigidity = value;
            }
        }
    }

    /// <summary>
    /// 道具攻击力
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float ItemAttacking
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.ItemAttacking).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ItemAttacking = value;
            }
        }
    }

    /// <summary>
    /// 魔法攻击力
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MagicAttacking
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MagicAttacking).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MagicAttacking = value;
            }
        }
    }

    /// <summary>
    /// 物理攻击力
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float PhysicsAttacking
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.PhysicsAttacking).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.PhysicsAttacking = value;
            }
        }
    }

    /// <summary>
    /// 物理最小伤害(通过敏捷计算出来的值,也有一些装备会附加该数值)
    /// </summary>
    public float PhysicsMinHurt
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.PhysicsMinHurt).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.PhysicsMinHurt = value;
            }
        }
    }


    /// <summary>
    /// 魔法附加伤害
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MagicAdditionalDamage
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MagicAdditionalDamage).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MagicAdditionalDamage = value;
            }
        }
    }

    /// <summary>
    /// 物理伤害附加
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float PhysicsAdditionalDamage
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.PhysicsAdditionalDamage).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.PhysicsAdditionalDamage = value;
            }
        }
    }

    /// <summary>
    /// 魔法攻击穿透
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MagicPenetrate
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MagicPenetrate).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MagicPenetrate = value;
            }
        }
    }

    /// <summary>
    /// 物理攻击穿透
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float PhysicsPenetrate
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.PhysicsPenetrate).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.PhysicsPenetrate = value;
            }
        }
    }

    /// <summary>
    /// 魔法最终伤害
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MagicFinalDamage
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MagicFinalDamage).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MagicFinalDamage = value;
            }
        }
    }

    /// <summary>
    /// 物理最终伤害
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float PhysicsFinalDamage
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.PhysicsFinalDamage).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.PhysicsFinalDamage = value;
            }
        }
    }

    /// <summary>
    /// 元素亲和
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float EffectAffine
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.EffectAffine).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.EffectAffine = value;
            }
        }
    }

    /// <summary>
    /// 魔法亲和
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MagicFit
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MagicFit).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MagicFit = value;
            }
        }
    }

    /// <summary>
    /// 魔法抗性（魔法防御）
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MagicResistance
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MagicResistance).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MagicResistance = value;
            }
        }
    }

    /// <summary>
    /// 物理抗性（物理防御）
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float PhysicsResistance
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.PhysicsResistance).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.PhysicsResistance = value;
            }
        }
    }

    /// <summary>
    /// 元素抗性
    /// </summary>
    float[] _ElementResistances;
    public float[] ElementResistances
    {
        get
        {
            if (iAttributeStateDic != null)
            {
                var tempDatas = iAttributeStateDic.Values.Select(temp => temp.ElementResistances).Where(temp => temp.Length != 0);
                var lengthData = tempDatas.Select(temp => temp.Length).Distinct();
                if (lengthData.Count() == 1 && lengthData.First() > 0)
                {
                    int length = lengthData.First();
                    float[] result = new float[length];
                    foreach (var _tempData in tempDatas)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            result[i] += _tempData[i];
                        }
                    }
                }
            }
            return new float[0];
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ElementResistances = value;
            }
        }
    }

    /// <summary>
    /// 状态抗性
    /// </summary>
    float[] _StateResistances;
    /// <summary>
    /// 状态抗性
    /// </summary>
    public float[] StateResistances
    {
        get
        {
            if (iAttributeStateDic != null)
            {
                var tempDatas = iAttributeStateDic.Values.Select(temp => temp.StateResistances).Where(temp => temp.Length != 0);
                var lengthData = tempDatas.Select(temp => temp.Length).Distinct();
                if (lengthData.Count() == 1 && lengthData.First() > 0)
                {
                    int length = lengthData.First();
                    float[] result = new float[length];
                    foreach (var _tempData in tempDatas)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            result[i] += _tempData[i];
                        }
                    }
                }
            }
            return new float[0];
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.StateResistances = value;
            }
        }
    }

    /// <summary>
    /// 特效影响力
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float EffectResideTime
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.EffectResideTime).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.EffectResideTime = value;
            }
        }
    }

    /// <summary>
    /// 光明信仰强度
    /// </summary>
    public float LightFaith
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.LightFaith).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.LightFaith = value;
            }
        }
    }

    /// <summary>
    /// 黑信仰强度
    /// </summary>
    public float DarkFaith
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.DarkFaith).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.DarkFaith = value;
            }
        }
    }

    /// <summary>
    /// 生物信仰强度
    /// </summary>
    public float LifeFaith
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.LifeFaith).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.LifeFaith = value;
            }
        }
    }

    /// <summary>
    /// 自然信仰强度
    /// </summary>
    public float NaturalFaith
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.NaturalFaith).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.NaturalFaith = value;
            }
        }
    }

    /// <summary>
    /// 暴击倍率(角色本身为1.5倍)
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float CritDamageRatio
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.CritDamageRatio).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.CritDamageRatio = value;
            }
        }
    }

    /// <summary>
    /// 法术陷阱伤害
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float SpellTrapDamage
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.SpellTrapDamage).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.SpellTrapDamage = value;
            }
        }
    }

    /// <summary>
    /// 法术陷阱特效产生几率
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float SpellTrapEffectProbability
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.SpellTrapEffectProbability).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.SpellTrapEffectProbability = value;
            }
        }
    }

    /// <summary>
    /// 对不死族伤害提升(百分比倍率)
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float DamageToTheUndead
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.DamageToTheUndead).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.DamageToTheUndead = value;
            }
        }
    }

    /// <summary>
    /// 对不死族附加混乱几率
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float ChaosOfTheUndead
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.ChaosOfTheUndead).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ChaosOfTheUndead = value;
            }
        }
    }

    /// <summary>
    /// 治疗量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float TreatmentVolume
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.TreatmentVolume).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.TreatmentVolume = value;
            }
        }
    }

    /// <summary>
    /// 对陷阱的防御力
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float TrapDefense
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.TrapDefense).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.TrapDefense = value;
            }
        }
    }

    /// <summary>
    /// 神秘信仰强度
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MysticalBeliefIntensity
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MysticalBeliefIntensity).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MysticalBeliefIntensity = value;
            }
        }
    }

    /// <summary>
    /// 神秘信仰特效产生几率
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MysticalBeliefSpecialEffects
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MysticalBeliefSpecialEffects).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MysticalBeliefSpecialEffects = value;
            }
        }
    }

    /// <summary>
    /// 崇拜信仰强度
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float ImproveWorshipFaith
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.ImproveWorshipFaith).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ImproveWorshipFaith = value;
            }
        }
    }

    /// <summary>
    /// 异常状态抗性
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float AbnormalStateResistance
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.AbnormalStateResistance).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.AbnormalStateResistance = value;
            }
        }
    }

    /// <summary>
    /// 元素立场强度
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float ElementStandStrength
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.ElementStandStrength).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ElementStandStrength = value;
            }
        }
    }

    /// <summary>
    /// 同元素魔法效果加成
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float SameElementEffectAdded
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.SameElementEffectAdded).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.SameElementEffectAdded = value;
            }
        }
    }

    /// <summary>
    /// 技能冷却时间
    /// </summary>
    public float CoolingTime
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MustUsedBaseMana).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.CoolingTime = value;
            }
        }
    }
    #endregion
    #region 其他杂项
    /// <summary>
    /// 需要使用的基础耗魔量(主要是组合技能以及需要主动释放的技能存在此选项)
    /// </summary>
    public float MustUsedBaseMana
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MustUsedBaseMana).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MustUsedBaseMana = value;
            }
        }
    }

    /// <summary>
    /// 减少该技能的冷却时间
    /// </summary>
    public float ExemptionChatingMana
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.ExemptionChatingMana).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ExemptionChatingMana = value;
            }
        }
    }

    /// <summary>
    /// 耗魔量减免(百分比)
    /// </summary>
    public float ReliefManaAmount
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.ReliefManaAmount).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ReliefManaAmount = value;
            }
        }
    }

    /// <summary>
    /// 咏唱时间减免(百分比)
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float ExemptionChantingTime
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.ExemptionChantingTime).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ExemptionChantingTime = value;
            }
        }
    }

    /// <summary>
    /// 冷却时间减免(百分比)
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float ReduceCoolingTime
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.ReduceCoolingTime).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ReduceCoolingTime = value;
            }
        }
    }

    /// <summary>
    /// 对不死族加速
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float AccelerateToUndead
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.AccelerateToUndead).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.AccelerateToUndead = value;
            }
        }
    }

    /// <summary>
    /// 经验值加成(与基础经验乘算)
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float ExperienceValuePlus
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.ExperienceValuePlus).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ExperienceValuePlus = value;
            }
        }
    }

    /// <summary>
    /// 物品掉落率(与基础掉落率乘算)
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float GooodsDropRate
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.GooodsDropRate).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.GooodsDropRate = value;
            }
        }
    }
    #endregion
    #region 特殊效果
    /// <summary>
    /// 幸运加护,获得优质物品概率与获取经验提升
    /// </summary>
    public float LuckShi
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.LuckShi).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.LuckShi = value;
            }
        }
    }

    /// <summary>
    /// 庇佑加护,每隔一定时间获得一次免疫致死伤害的能力
    /// </summary>
    public float GarShi
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.GarShi).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.GarShi = value;
            }
        }
    }

    /// <summary>
    /// 战神加护,每隔一段时间获得一次在进入负面状态时清除自身所有负面效果的能力
    /// </summary>
    public float WarShi
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.WarShi).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.WarShi = value;
            }
        }
    }
    #endregion
    #endregion

}