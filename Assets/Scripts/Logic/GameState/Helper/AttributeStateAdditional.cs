using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using UnityEngine;

/// <summary>
/// 属性的单独实现
/// 可以是附加属性,也可以是最后的计算结果
/// </summary>
public class AttributeStateAdditional : IAttributeState
{
    /// <summary>
    /// 种族成长对象
    /// </summary>
    RoleOfRaceInfoStruct roleOfRaceInfoStruct;

    /// <summary>
    /// 构造函数
    /// </summary>
    public AttributeStateAdditional()
    {
        callBackDic = new Dictionary<Type, List<KeyValuePair<object, Action<IBaseState, string>>>>();
        Type[] allType = GetType().GetInterfaces();
        foreach (Type type in allType)
        {
            if (type.IsInterface
                && !Type.Equals(type, typeof(IBaseState))
                && type.GetInterface(typeof(IBaseState).Name) != null)
            {
                List<KeyValuePair<object, Action<IBaseState, string>>> callBackList = new List<KeyValuePair<object, Action<IBaseState, string>>>();
                callBackDic.Add(type, callBackList);
            }
        }
    }

    public void Init()
    {
        //使用反射重置
        Type t = typeof(IAttributeState);
        PropertyInfo[] infos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (PropertyInfo info in infos)
        {
            Type targetType = info.PropertyType;
            MethodInfo setMethod = info.GetSetMethod();
            if (targetType.Equals(typeof(float)))
            {
                setMethod.Invoke(this, new object[] { 0 });
            }
            else if (targetType.Equals(typeof(float[])))
            {
                setMethod.Invoke(this, new object[] { null });
            }
        }
    }

    /// <summary>
    /// 设置种族成长对象
    /// </summary>
    /// <param name="roleOfRaceInfoStruct">种族成长对象</param>
    public void SetRoleOfRaceAddition(RoleOfRaceInfoStruct roleOfRaceInfoStruct)
    {
        this.roleOfRaceInfoStruct = roleOfRaceInfoStruct;
    }

    public static IAttributeState operator +(AttributeStateAdditional a, IAttributeState b)
    {
        if (a == null || b == null)
            return null;
        AttributeStateAdditional result = new AttributeStateAdditional();
        Type t = typeof(IAttributeState);
        PropertyInfo[] infos = t.GetProperties();
        foreach (PropertyInfo info in infos)
        {
            MethodInfo getMethod = info.GetGetMethod();
            MethodInfo setMethod = info.GetSetMethod();
            if (getMethod != null && setMethod != null)
            {
                if (info.PropertyType.Equals(typeof(float)))
                {
                    float a_v = (float)getMethod.Invoke(a, null);
                    float b_v = (float)getMethod.Invoke(b, null);
                    float r_v = a_v + b_v;
                    setMethod.Invoke(result, new object[] { r_v });
                }
            }
        }
        //处理数组的相加
        float[] aElementResistances = a.ElementResistances;
        float[] bElementResistances = b.ElementResistances;
        if (aElementResistances.Length == bElementResistances.Length)
        {
            float[] elementResistances = new float[result.ElementResistances.Length];
            for (int i = 0; i < result.ElementResistances.Length; i++)
            {
                elementResistances[i] = aElementResistances[i] + bElementResistances[i];
            }
            result.ElementResistances = elementResistances;
        }
        float[] aStateResistances = a.StateResistances;
        float[] bStateResistances = b.StateResistances;
        if (aStateResistances.Length == bStateResistances.Length)
        {
            float[] stateResistances = new float[result.StateResistances.Length];
            for (int i = 0; i < result.StateResistances.Length; i++)
            {
                stateResistances[i] = aStateResistances[i] + bStateResistances[i];
            }
            result.StateResistances = stateResistances;
        }
        return result;
    }

    public static IAttributeState operator +(IAttributeState a, AttributeStateAdditional b)
    {
        return b + a;
    }

    /// <summary>
    /// 返回深拷贝对象
    /// </summary>
    /// <returns></returns>
    public AttributeStateAdditional Clone()
    {
        return (AttributeStateAdditional)((IAttributeState)this + (new AttributeStateAdditional()));
    }



    private float _Quick;
    /// <summary>
    /// 敏捷
    /// </summary>
    public float Quick
    {
        get { return _Quick; }
        set
        {
            float tempQuick = _Quick;
            _Quick = value;
            if (tempQuick != _Quick)
            {
                UpdateAttribute();
                Call<IAttributeState, float>(temp => temp.Quick);
            }
        }
    }

    private float _Mental;
    /// <summary>
    /// 精神
    /// </summary>
    public float Mental
    {
        get { return _Mental; }
        set
        {
            float tempMental = _Mental;
            _Mental = value;
            if (tempMental != _Mental)
            {
                UpdateAttribute();
                Call<IAttributeState, float>(temp => temp.Mental);
            }
        }
    }

    private float _Power;
    /// <summary>
    /// 力量
    /// </summary>
    public float Power
    {
        get { return _Power; }
        set
        {
            float tempPower = _Power;
            _Power = value;
            if (tempPower != _Power)
            {
                UpdateAttribute();
                Call<IAttributeState, float>(temp => temp.Power);
            }
        }
    }

    /// <summary>
    /// 更新属性
    /// </summary>
    private void UpdateAttribute()
    {
        if (roleOfRaceInfoStruct == null)
            return;
        //通过基础属性修改下方的衍生属性
        //敏捷
        _EvadeRate = Quick * roleOfRaceInfoStruct.additionQuickToEvadeRate;
        _HitRate = Quick * roleOfRaceInfoStruct.additionQuickToHitRate;
        _CritRate = Quick * roleOfRaceInfoStruct.additionQuickToCritRate;
        _AttackSpeed = Quick * roleOfRaceInfoStruct.additionQuickToAttackSpeed;
        _MoveSpeed = Quick * roleOfRaceInfoStruct.additionQuickToMoveSpeed;
        //精神
        _MaxMana = Mental * roleOfRaceInfoStruct.additionMentalToMana;
        _MagicAttacking = Mental * roleOfRaceInfoStruct.additionMentalToMagicAttacking;
        _MaxUseMana = Mental * roleOfRaceInfoStruct.additionMentalToMaxUseMana;
        _MagicResistance = Mental * roleOfRaceInfoStruct.additionMentalToMagicResistance;
        _ManaRecovery = Mental * roleOfRaceInfoStruct.additionMentalToManaRecovery;
        //力量 
        _MaxHP = Power * roleOfRaceInfoStruct.additionPowerToHP;
        _PhysicsAttacking = Power * roleOfRaceInfoStruct.additionPowerToPhysicAttacking;
        _AbnormalStateResistance = Power * roleOfRaceInfoStruct.additionPowerToAbnormalStateResistance;
        _PhysicsResistance = Power * roleOfRaceInfoStruct.additionPowerToPhysicsResistance;
        _LifeRecovery = Power * roleOfRaceInfoStruct.additionPowerToHPRecovery;
    }

    private float _BasePhysicDefense;
    /// <summary>
    /// 基础物理护甲
    /// </summary>
    public float BasePhysicDefense
    {
        get { return _BasePhysicDefense; }
        set
        {
            float tempBasePhysicDefense = _BasePhysicDefense;
            _BasePhysicDefense = value;
            if (tempBasePhysicDefense != _BasePhysicDefense)
                Call<IAttributeState, float>(temp => temp.BasePhysicDefense);
        }
    }

    private float _BasePhysicDamage;
    /// <summary>
    /// 基础物理伤害
    /// </summary>
    public float BasePhysicDamage
    {
        get { return _BasePhysicDamage; }
        set
        {
            float tempBasePhysicDamage = _BasePhysicDamage;
            _BasePhysicDamage = value;
            if (tempBasePhysicDamage != _BasePhysicDamage)
                Call<IAttributeState, float>(temp => temp.BasePhysicDamage);
        }
    }

    private float _HP;
    /// <summary>
    /// 血量
    /// </summary>
    public float HP
    {
        get { return _HP; }
        set
        {
            float tempHP = _HP;
            _HP = value;
            if (_HP < 0)
                _HP = 0;
            if (tempHP != _HP)
            {
                Call<IAttributeState, float>(temp => temp.HP);
            }
        }
    }

    private float _MaxHP;
    /// <summary>
    /// 最大血量
    /// </summary>
    public float MaxHP
    {
        get { return _MaxHP; }
        set
        {
            float tempMaxHP = _MaxHP;
            _MaxHP = value;
            if (tempMaxHP != _MaxHP)
            {
                Call<IAttributeState, float>(temp => temp.MaxHP);
            }
        }
    }

    private float _Mana;
    /// <summary>
    /// 魔力量
    /// </summary>
    public float Mana
    {
        get { return _Mana; }
        set
        {
            float tempMana = _Mana;
            _Mana = value;
            if (_Mana < 0)
                _Mana = 0;
            if (tempMana != _Mana)
            {
                Call<IAttributeState, float>(temp => temp.Mana);
            }
        }
    }

    private float _MaxMana;
    /// <summary>
    /// 最大魔力量
    /// </summary>
    public float MaxMana
    {
        get { return _MaxMana; }
        set
        {
            float tempMaxMana = _MaxMana;
            _MaxMana = value;
            if (tempMaxMana != _MaxMana)
            {
                Call<IAttributeState, float>(temp => temp.MaxMana);
            }
        }
    }

    private float _Mentality;
    /// <summary>
    /// 精神力计量
    /// </summary>
    public float Mentality
    {
        get { return _Mentality; }
        set
        {
            float tempMentality = _Mentality;
            _Mentality = value;
            if (tempMentality != _Mentality)
                Call<IAttributeState, float>(temp => temp.MaxMana);
        }
    }

    private float _MaxMentality;
    /// <summary>
    /// 最大精神力计量
    /// </summary>
    public float MaxMentality
    {
        get { return _MaxMentality; }
        set
        {
            float tempMaxMentality = _MaxMentality;
            _MaxMentality = value;
            if (tempMaxMentality != _MaxMentality)
                Call<IAttributeState, float>(temp => temp.MaxMentality);
        }
    }

    private float _MindTraining;
    /// <summary>
    /// 心志力计量
    /// </summary>
    public float MindTraining
    {
        get { return _MindTraining; }
        set
        {
            float tempMindTraining = _MindTraining;
            _MindTraining = value;
            if (_MindTraining != tempMindTraining)
                Call<IAttributeState, float>(temp => temp.MindTraining);
        }
    }

    private float _MaxMindTraining;
    /// <summary>
    ///  最大心志力计量
    /// </summary>
    public float MaxMindTraining
    {
        get { return _MaxMindTraining; }
        set
        {
            float tempMaxMindTraining = _MaxMindTraining;
            _MaxMindTraining = value;
            if (tempMaxMindTraining != _MaxMindTraining)
                Call<IAttributeState, float>(temp => temp.MaxMindTraining);
        }
    }

    private float _MaxUseMana;
    /// <summary>
    /// 最大耗魔上限
    /// </summary>
    public float MaxUseMana
    {
        get { return _MaxUseMana; }
        set
        {
            float tempMaxUseMana = _MaxUseMana;
            _MaxUseMana = value;
            if (tempMaxUseMana != _MaxUseMana)
            {
                Call<IAttributeState, float>(temp => temp.MaxUseMana);
            }
        }
    }

    private float _View;
    /// <summary>
    /// 视野范围
    /// </summary>
    public float View
    {
        get { return _View; }
        set
        {
            float tempView = _View;
            _View = value;
            if (tempView != _View)
            {
                Call<IAttributeState, float>(temp => temp.View);
            }
        }
    }

    private float _SightDef;
    /// <summary>
    /// 降低被怪物发现的概率(被发现的距离倍率)
    /// </summary>
    public float SightDef
    {
        get { return _SightDef; }
        set
        {
            float tempSightDef = _SightDef;
            _SightDef = value;
            if (_SightDef != tempSightDef)
                Call<IAttributeState, float>(temp => temp.SightDef);
        }
    }

    private float _MoveSpeed;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float MoveSpeed
    {
        get { return _MoveSpeed; }
        set
        {
            float tempMoveSpeed = _MoveSpeed;
            _MoveSpeed = value;
            if (tempMoveSpeed != _MoveSpeed)
            {
                Call<IAttributeState, float>(temp => temp.MoveSpeed);
            }
        }
    }

    private float _AttackSpeed;
    /// <summary>
    /// 攻击速度
    /// </summary>
    public float AttackSpeed
    {
        get { return _AttackSpeed; }
        set
        {
            float tempAttackSpeed = _AttackSpeed;
            _AttackSpeed = value;
            if (tempAttackSpeed != _AttackSpeed)
            {
                Call<IAttributeState, float>(temp => temp.AttackSpeed);
            }
        }
    }

    private float _HitRate;
    /// <summary>
    /// 命中率
    /// </summary>
    public float HitRate
    {
        get { return _HitRate; }
        set
        {
            float tempHitRate = _HitRate;
            _HitRate = value;
            if (tempHitRate != _HitRate)
            {
                Call<IAttributeState, float>(temp => temp.HitRate);
            }
        }
    }

    private float _EvadeRate;
    /// <summary>
    /// 闪避率
    /// </summary>
    public float EvadeRate
    {
        get { return _EvadeRate; }
        set
        {
            float tempEvadeRate = _EvadeRate;
            _EvadeRate = value;
            if (tempEvadeRate != _EvadeRate)
            {
                Call<IAttributeState, float>(temp => temp.EvadeRate);
            }
        }
    }

    private float _CritRate;
    /// <summary>
    /// 暴击率
    /// </summary>
    public float CritRate
    {
        get { return _CritRate; }
        set
        {
            float tempCritRate = _CritRate;
            _CritRate = value;
            if (tempCritRate != _CritRate)
            {
                Call<IAttributeState, float>(temp => temp.CritRate);
            }
        }
    }

    private float _LifeRecovery;
    /// <summary>
    /// 生命恢复速度
    /// </summary>
    public float LifeRecovery
    {
        get { return _LifeRecovery; }
        set
        {
            float tempLifeRecovery = _LifeRecovery;
            _LifeRecovery = value;
            if (tempLifeRecovery != _LifeRecovery)
            {
                Call<IAttributeState, float>(temp => temp.LifeRecovery);
            }
        }
    }

    private float _ManaRecovery;
    /// <summary>
    /// 法力恢复速度
    /// </summary>
    public float ManaRecovery
    {
        get { return _ManaRecovery; }
        set
        {
            float tempManaRecovery = _ManaRecovery;
            _ManaRecovery = value;
            if (tempManaRecovery != _ManaRecovery)
            {
                Call<IAttributeState, float>(temp => temp.ManaRecovery);
            }
        }
    }

    private float _EquipBlock;
    /// <summary>
    /// 伤害格挡率
    /// </summary>
    public float EquipBlock
    {
        get { return _EquipBlock; }
        set
        {

            float tempEquipBlock = _EquipBlock;
            _EquipBlock = value;
            if (tempEquipBlock != _EquipBlock)
            {
                Call<IAttributeState, float>(temp => temp.EquipBlock);
            }
        }
    }

    private float _CriticalDef;
    /// <summary>
    /// 暴击率伤害减少率
    /// </summary>
    public float CriticalDef
    {
        get
        {
            return _CriticalDef;
        }
        set
        {
            float tempCriticalDef = _CriticalDef;
            _CriticalDef = value;
            if (tempCriticalDef != _CriticalDef)
            {
                Call<IAttributeState, float>(temp => temp.CriticalDef);
            }
        }
    }

    private float _AttackRigidity;
    /// <summary>
    /// 攻击僵直
    /// </summary>
    public float AttackRigidity
    {
        get { return _AttackRigidity; }
        set
        {
            float tempAttackRigidity = _AttackRigidity;
            _AttackRigidity = value;
            if (tempAttackRigidity != _AttackRigidity)
            {
                Call<IAttributeState, float>(temp => temp.AttackRigidity);
            }
        }
    }

    private float _ItemAttacking;
    /// <summary>
    /// 道具攻击力
    /// </summary>
    public float ItemAttacking
    {
        get { return _ItemAttacking; }
        set
        {
            float tempItemAttacking = _ItemAttacking;
            _ItemAttacking = value;
            if (tempItemAttacking != _ItemAttacking)
            {
                Call<IAttributeState, float>(temp => temp.ItemAttacking);
            }
        }
    }

    private float _MagicAttacking;
    /// <summary>
    /// 魔法攻击力
    /// </summary>
    public float MagicAttacking
    {
        get { return _MagicAttacking; }
        set
        {
            float tempMagicAttacking = _MagicAttacking;
            _MagicAttacking = value;
            if (tempMagicAttacking != _MagicAttacking)
            {
                Call<IAttributeState, float>(temp => temp.MagicAttacking);
            }
        }
    }

    private float _PhysicsAttacking;
    /// <summary>
    /// 物理攻击力
    /// </summary>
    public float PhysicsAttacking
    {
        get { return _PhysicsAttacking; }
        set
        {
            float tempPhysicsAttacking = _PhysicsAttacking;
            _PhysicsAttacking = value;
            if (tempPhysicsAttacking != _PhysicsAttacking)
            {
                Call<IAttributeState, float>(temp => temp.PhysicsAttacking);
            }
        }
    }

    private float _PhysicsMinHurt;
    /// <summary>
    /// 物理最小伤害(通过敏捷计算出来的值,也有一些装备会附加该数值)
    /// </summary>
    public float PhysicsMinHurt
    {
        get { return _PhysicsMinHurt; }
        set
        {
            float tempPhysicsMinHurt = _PhysicsMinHurt;
            _PhysicsMinHurt = value;
            if (tempPhysicsMinHurt != _PhysicsMinHurt)
            {
                Call<IAttributeState, float>(temp => temp.PhysicsMinHurt);
            }
        }
    }

    private float _MagicMinHurt;
    /// <summary>
    /// 魔法最小伤害(通过敏捷计算出来的值,也有一些装备会附加该数值)
    /// </summary>
    public float MagicMinHurt
    {
        get { return _MagicMinHurt; }
        set
        {
            float tempMagicMinHurt = _MagicMinHurt;
            _MagicMinHurt = value;
            if (_MagicMinHurt != tempMagicMinHurt)
                Call<IAttributeState, float>(temp => temp.MagicMinHurt);
        }
    }

    private float _MagicAdditionalDamage;
    /// <summary>
    /// 魔法附加伤害
    /// </summary>
    public float MagicAdditionalDamage
    {
        get { return _MagicAdditionalDamage; }
        set
        {
            float tempMagicAdditionalDamage = _MagicAdditionalDamage;
            _MagicAdditionalDamage = value;
            if (tempMagicAdditionalDamage != _MagicAdditionalDamage)
            {
                Call<IAttributeState, float>(temp => temp.MagicAdditionalDamage);
            }
        }
    }

    private float _PhysicsAdditionalDamage;
    /// <summary>
    /// 物理附加伤害
    /// </summary>
    public float PhysicsAdditionalDamage
    {
        get { return _PhysicsAdditionalDamage; }
        set
        {
            float tempPhysicsAdditionalDamage = _PhysicsAdditionalDamage;
            _PhysicsAdditionalDamage = value;
            if (tempPhysicsAdditionalDamage != _PhysicsAdditionalDamage)
            {
                Call<IAttributeState, float>(temp => temp.PhysicsAdditionalDamage);
            }
        }
    }

    private float _MagicPenetrate;
    /// <summary>
    /// 魔法攻击穿透
    /// </summary>
    public float MagicPenetrate
    {
        get { return _MagicPenetrate; }
        set
        {
            float tempMagicPenetrate = _MagicPenetrate;
            _MagicPenetrate = value;
            if (tempMagicPenetrate != _MagicPenetrate)
            {
                Call<IAttributeState, float>(temp => temp.MagicPenetrate);
            }
        }
    }

    private float _PhysicsPenetrate;
    /// <summary>
    /// 物理攻击穿透
    /// </summary>
    public float PhysicsPenetrate
    {
        get { return _PhysicsPenetrate; }
        set
        {
            float tempPhysicsPenetrate = _PhysicsPenetrate;
            _PhysicsPenetrate = value;
            if (tempPhysicsPenetrate != _PhysicsPenetrate)
            {
                Call<IAttributeState, float>(temp => temp.PhysicsPenetrate);
            }
        }
    }

    private float _MagicFinalDamage;
    /// <summary>
    /// 魔法最终伤害
    /// </summary>
    public float MagicFinalDamage
    {
        get { return _MagicFinalDamage; }
        set
        {
            float tempMagicFinalDamage = _MagicFinalDamage;
            _MagicFinalDamage = value;
            if (tempMagicFinalDamage != _MagicFinalDamage)
            {
                Call<IAttributeState, float>(temp => temp.MagicFinalDamage);
            }
        }
    }

    private float _PhysicsFinalDamage;
    /// <summary>
    /// 物理最终伤害
    /// </summary>
    public float PhysicsFinalDamage
    {
        get { return _PhysicsFinalDamage; }
        set
        {
            float tempPhysicsFinalDamage = _PhysicsFinalDamage;
            _PhysicsFinalDamage = value;
            if (tempPhysicsFinalDamage != _PhysicsFinalDamage)
            {
                Call<IAttributeState, float>(temp => temp.PhysicsFinalDamage);
            }
        }
    }

    private float _EffectAffine;
    /// <summary>
    /// 元素亲和
    /// </summary>
    public float EffectAffine
    {
        get { return _EffectAffine; }
        set
        {
            float tempEffectAffine = _EffectAffine;
            _EffectAffine = value;
            if (tempEffectAffine != _EffectAffine)
            {
                Call<IAttributeState, float>(temp => temp.EffectAffine);
            }
        }
    }

    private float _MagicFit;
    /// <summary>
    /// 魔法亲和
    /// </summary>
    public float MagicFit
    {
        get { return _MagicFit; }
        set
        {
            float tempMagicFit = _MagicFit;
            _MagicFit = value;
            if (tempMagicFit != _MagicFit)
            {
                Call<IAttributeState, float>(temp => temp.MagicFit);
            }
        }
    }

    private float _MagicResistance;
    /// <summary>
    /// 魔法抗性
    /// </summary>
    public float MagicResistance
    {
        get { return _MagicResistance; }
        set
        {
            float tempMagicResistance = _MagicResistance;
            _MagicResistance = value;
            if (tempMagicResistance != _MagicResistance)
            {
                Call<IAttributeState, float>(temp => temp.MagicResistance);
            }
        }
    }

    private float _PhysicsResistance;
    /// <summary>
    /// 物理抗性
    /// </summary>
    public float PhysicsResistance
    {
        get { return _PhysicsResistance; }
        set
        {
            float tempPhysicsResistance = _PhysicsResistance;
            _PhysicsResistance = value;
            if (tempPhysicsResistance != _PhysicsResistance)
            {
                Call<IAttributeState, float>(temp => temp.PhysicsResistance);
            }
        }
    }

    float[] _ElementResistances;
    /// <summary>
    /// 元素抗性
    /// </summary>
    public float[] ElementResistances
    {
        get
        {
            if (_ElementResistances == null)
            {
                //根据元素类型建立数组
                _ElementResistances = new float[0];
                //------------------//
            }
            return _ElementResistances.Clone() as float[];
        }
        set
        {
            if (_ElementResistances == null)
                _ElementResistances = ElementResistances;
            if (_ElementResistances != null && value != null && value.Length == _ElementResistances.Length)
            {
                bool valueChanged = false;
                int length = value.Length;
                for (int i = 0; i < length; i++)
                {
                    if (_ElementResistances[i] != value[i])
                        valueChanged = true;
                    _ElementResistances[i] = value[i];
                }
                if (valueChanged)
                    Call<IAttributeState, float[]>(temp => temp.ElementResistances);
            }
            else if (value == null)
                _ElementResistances = null;
        }
    }

    float[] _StateResistances;
    /// <summary>
    /// 状态抗性
    /// </summary>
    public float[] StateResistances
    {
        get
        {
            if (_StateResistances == null)
            {
                //根据状态类型建立数组
                _StateResistances = new float[0];
                //------------------//
            }
            return _StateResistances.Clone() as float[];
        }
        set
        {
            if (_StateResistances == null)
                _StateResistances = StateResistances;
            if (_StateResistances != null && value != null && value.Length == _StateResistances.Length)
            {
                bool valueChanged = false;
                int length = value.Length;
                for (int i = 0; i < length; i++)
                {
                    if (_StateResistances[i] != value[i])
                        valueChanged = true;
                    _StateResistances[i] = value[i];
                }
                if (valueChanged)
                    Call<IAttributeState, float[]>(temp => temp.StateResistances);
            }
            else if (value == null)
                _StateResistances = null;
        }
    }

    private float _EffectResideTime;
    /// <summary>
    /// 特效影响力
    /// </summary>
    public float EffectResideTime
    {
        get { return _EffectResideTime; }
        set
        {
            float tempEffectResideTime = _EffectResideTime;
            _EffectResideTime = value;
            if (tempEffectResideTime != _EffectResideTime)
            {
                Call<IAttributeState, float>(temp => temp.EffectResideTime);
            }
        }
    }

    private float _LightFaith;
    /// <summary>
    /// 光明信仰强度
    /// </summary>
    public float LightFaith
    {
        get { return _LightFaith; }
        set
        {
            float tempLightFaith = _LightFaith;
            _LightFaith = value;
            if (tempLightFaith != _LightFaith)
            {
                Call<IAttributeState, float>(temp => temp.LightFaith);
            }
        }
    }

    private float _DarkFaith;
    /// <summary>
    /// 黑暗信仰强度
    /// </summary>
    public float DarkFaith
    {
        get { return _DarkFaith; }
        set
        {
            float tempDarkFaith = _DarkFaith;
            _DarkFaith = value;
            if (tempDarkFaith != _DarkFaith)
            {
                Call<IAttributeState, float>(temp => temp.DarkFaith);
            }
        }
    }

    private float _LifeFaith;
    /// <summary>
    /// 生物信仰强度 
    /// </summary>
    public float LifeFaith
    {
        get { return _LifeFaith; }
        set
        {
            float tempLifeFaith = _LifeFaith;
            _LifeFaith = value;
            if (tempLifeFaith != _LifeFaith)
            {
                Call<IAttributeState, float>(temp => temp.LifeFaith);
            }
        }
    }

    private float _NaturalFaith;
    /// <summary>
    /// 自然信仰强度
    /// </summary>
    public float NaturalFaith
    {
        get { return _NaturalFaith; }
        set
        {
            float tempNaturalFaith = _NaturalFaith;
            _NaturalFaith = value;
            if (tempNaturalFaith != _NaturalFaith)
            {
                Call<IAttributeState, float>(temp => temp.NaturalFaith);
            }
        }
    }

    private float _CritDamageRatio;
    /// <summary>
    /// 暴击倍率(角色本身为1.5倍)
    /// </summary>
    public float CritDamageRatio
    {
        get { return _CritDamageRatio; }
        set
        {
            float tempCritDamageRatio = _CritDamageRatio;
            _CritDamageRatio = value;
            if (tempCritDamageRatio != _CritDamageRatio)
            {
                Call<IAttributeState, float>(temp => temp.CritDamageRatio);
            }
        }
    }

    private float _SpellTrapDamage;
    /// <summary>
    /// 法术陷阱伤害
    /// </summary>
    public float SpellTrapDamage
    {
        get { return _SpellTrapDamage; }
        set
        {
            float tempSpellTrapDamage = _SpellTrapDamage;
            _SpellTrapDamage = value;
            if (tempSpellTrapDamage != _SpellTrapDamage)
            {
                Call<IAttributeState, float>(temp => temp.SpellTrapDamage);
            }
        }
    }

    private float _SpellTrapEffectProbability;
    /// <summary>
    /// 法术陷阱特效产生几率
    /// </summary>
    public float SpellTrapEffectProbability
    {
        get { return _SpellTrapEffectProbability; }
        set
        {
            float tempSpellTrapEffectProbability = _SpellTrapEffectProbability;
            _SpellTrapEffectProbability = value;
            if (tempSpellTrapEffectProbability != _SpellTrapEffectProbability)
            {
                Call<IAttributeState, float>(temp => temp.SpellTrapEffectProbability);
            }
        }
    }

    private float _DamageToTheUndead;
    /// <summary>
    /// 对不死族伤害提升(百分比倍率)
    /// </summary>
    public float DamageToTheUndead
    {
        get { return _DamageToTheUndead; }
        set
        {
            float tempDamageToTheUndead = _DamageToTheUndead;
            _DamageToTheUndead = value;
            if (tempDamageToTheUndead != _DamageToTheUndead)
            {
                Call<IAttributeState, float>(temp => temp.DamageToTheUndead);
            }
        }
    }

    private float _ChaosOfTheUndead;
    /// <summary>
    /// 对不死族附加混乱几率
    /// </summary>
    public float ChaosOfTheUndead
    {
        get { return _ChaosOfTheUndead; }
        set
        {
            float tempChaosOfTheUndead = _ChaosOfTheUndead;
            _ChaosOfTheUndead = value;
            if (tempChaosOfTheUndead != _ChaosOfTheUndead)
            {
                Call<IAttributeState, float>(temp => temp.ChaosOfTheUndead);
            }
        }
    }

    private float _TreatmentVolume;
    /// <summary>
    /// 治疗量
    /// </summary>
    public float TreatmentVolume
    {
        get { return _TreatmentVolume; }
        set
        {
            float tempTreatmentVolume = _TreatmentVolume;
            _TreatmentVolume = value;
            if (tempTreatmentVolume != _TreatmentVolume)
            {
                Call<IAttributeState, float>(temp => temp.TreatmentVolume);
            }
        }
    }

    private float _TrapDefense;
    /// <summary>
    /// 对陷阱的防御力
    /// </summary>
    public float TrapDefense
    {
        get { return _TrapDefense; }
        set
        {
            float tempTrapDefense = _TrapDefense;
            _TrapDefense = value;
            if (tempTrapDefense != _TrapDefense)
            {
                Call<IAttributeState, float>(temp => temp.TrapDefense);
            }
        }
    }

    private float _MysticalBeliefIntensity;
    /// <summary>
    /// 神秘信仰强度
    /// </summary>
    public float MysticalBeliefIntensity
    {
        get { return _MysticalBeliefIntensity; }
        set
        {
            float tempMysticalBeliefIntensity = _MysticalBeliefIntensity;
            _MysticalBeliefIntensity = value;
            if (tempMysticalBeliefIntensity != _MysticalBeliefIntensity)
            {
                Call<IAttributeState, float>(temp => temp.MysticalBeliefIntensity);
            }
        }
    }

    private float _MysticalBeliefSpecialEffects;
    /// <summary>
    /// 神秘信仰特效产生几率
    /// </summary>
    public float MysticalBeliefSpecialEffects
    {
        get { return _MysticalBeliefSpecialEffects; }
        set
        {
            float tempMysticalBeliefSpecialEffects = _MysticalBeliefSpecialEffects;
            _MysticalBeliefSpecialEffects = value;
            if (tempMysticalBeliefSpecialEffects != _MysticalBeliefSpecialEffects)
            {
                Call<IAttributeState, float>(temp => temp.MysticalBeliefSpecialEffects);
            }
        }
    }

    private float _ImproveWorshipFaith;
    /// <summary>
    /// 崇拜信仰强度
    /// </summary>
    public float ImproveWorshipFaith
    {
        get { return _ImproveWorshipFaith; }
        set
        {
            float tempImproveWorshipFaith = _ImproveWorshipFaith;
            _ImproveWorshipFaith = value;
            if (tempImproveWorshipFaith != _ImproveWorshipFaith)
            {
                Call<IAttributeState, float>(temp => temp.ImproveWorshipFaith);
            }
        }
    }

    private float _AbnormalStateResistance;
    /// <summary>
    /// 异常状态抗性
    /// </summary>
    public float AbnormalStateResistance
    {
        get { return _AbnormalStateResistance; }
        set
        {
            float tempAbnormalStateResistance = _AbnormalStateResistance;
            _AbnormalStateResistance = value;
            if (tempAbnormalStateResistance != _AbnormalStateResistance)
            {
                Call<IAttributeState, float>(temp => temp.AbnormalStateResistance);
            }
        }
    }

    private float _ElementStandStrength;
    /// <summary>
    /// 元素立场强度
    /// </summary>
    public float ElementStandStrength
    {
        get { return _ElementStandStrength; }
        set
        {
            float tempElementStandStrength = _ElementStandStrength;
            _ElementStandStrength = value;
            if (tempElementStandStrength != _ElementStandStrength)
            {
                Call<IAttributeState, float>(temp => temp.ElementStandStrength);
            }
        }
    }

    private float _SameElementEffectAdded;
    /// <summary>
    /// 同元素魔法效果加成
    /// </summary>
    public float SameElementEffectAdded
    {
        get { return _SameElementEffectAdded; }
        set
        {
            float tempSameElementEffectAdded = _SameElementEffectAdded;
            _SameElementEffectAdded = value;
            if (tempSameElementEffectAdded != _SameElementEffectAdded)
            {
                Call<IAttributeState, float>(temp => temp.SameElementEffectAdded);
            }
        }
    }

    private float _CoolingTime;
    /// <summary>
    /// 技能冷却时间
    /// </summary>
    public float CoolingTime
    {
        get { return _CoolingTime; }
        set
        {
            float tempCoolingTime = _CoolingTime;
            _CoolingTime = value;
            if (_CoolingTime != tempCoolingTime)
            {
                Call<IAttributeState, float>(temp => temp.CoolingTime);
            }
        }
    }

    private float _MustUsedBaseMana;
    /// <summary>
    /// 需要使用的基础耗魔量(主要是组合技能以及需要主动释放的技能存在此选项)
    /// </summary>
    public float MustUsedBaseMana
    {
        get { return _MustUsedBaseMana; }
        set
        {
            float tempMustUsedBaseMana = _MustUsedBaseMana;
            _MustUsedBaseMana = value;
            if (tempMustUsedBaseMana != _MustUsedBaseMana)
            {
                Call<IAttributeState, float>(temp => temp.MustUsedBaseMana);
            }
        }
    }

    private float _ExemptionChantingTime;
    /// <summary>
    /// 咏唱时间减免(百分比)
    /// </summary>
    public float ExemptionChantingTime
    {
        get { return _ExemptionChantingTime; }
        set
        {
            float tempExemptionChantingTime = _ExemptionChantingTime;
            _ExemptionChantingTime = value;
            if (tempExemptionChantingTime != _ExemptionChantingTime)
            {
                Call<IAttributeState, float>(temp => temp.ExemptionChantingTime);
            }
        }
    }

    private float _ReduceCoolingTime;
    /// <summary>
    /// 冷却时间减免(百分比)
    /// </summary>
    public float ReduceCoolingTime
    {
        get { return _ReduceCoolingTime; }
        set
        {
            float tempReduceCoolingTime = _ReduceCoolingTime;
            _ReduceCoolingTime = value;
            if (tempReduceCoolingTime != _ReduceCoolingTime)
            {
                Call<IAttributeState, float>(temp => temp.ReduceCoolingTime);
            }
        }
    }

    private float _AccelerateToUndead;
    /// <summary>
    /// 对不死族加速
    /// </summary>
    public float AccelerateToUndead
    {
        get { return _AccelerateToUndead; }
        set
        {
            float tempAccelerateToUndead = _AccelerateToUndead;
            _AccelerateToUndead = value;
            if (tempAccelerateToUndead != _AccelerateToUndead)
            {
                Call<IAttributeState, float>(temp => temp.AccelerateToUndead);
            }
        }
    }

    private float _ExperienceValuePlus;
    /// <summary>
    /// 经验值加成(与基础经验乘算)
    /// </summary>
    public float ExperienceValuePlus
    {
        get { return _ExperienceValuePlus; }
        set
        {
            float tempExperienceValuePlus = _ExperienceValuePlus;
            _ExperienceValuePlus = value;
            if (tempExperienceValuePlus != _ExperienceValuePlus)
            {
                Call<IAttributeState, float>(temp => temp.ExperienceValuePlus);
            }
        }
    }

    private float _GooodsDropRate;
    /// <summary>
    /// 物品掉落率(与基础掉落率乘算)
    /// </summary>
    public float GooodsDropRate
    {
        get { return _GooodsDropRate; }
        set
        {
            float tempGooodsDropRate = _GooodsDropRate;
            _GooodsDropRate = value;
            if (tempGooodsDropRate != _GooodsDropRate)
            {
                Call<IAttributeState, float>(temp => temp.GooodsDropRate);
            }
        }
    }

    private float _ExemptionChatingMana;
    /// <summary>
    /// 减少该技能的冷却时间
    /// </summary>
    public float ExemptionChatingMana
    {
        get { return _ExemptionChatingMana; }
        set
        {
            float tempExemptionChatingMana = _ExemptionChatingMana;
            _ExemptionChatingMana = value;
            if (tempExemptionChatingMana != _ExemptionChatingMana)
            {
                Call<IAttributeState, float>(temp => temp.ExemptionChatingMana);
            }
        }
    }

    private float _ReliefManaAmount;
    /// <summary>
    /// 耗魔量减免(百分比)
    /// </summary>
    public float ReliefManaAmount
    {
        get { return _ReliefManaAmount; }
        set
        {
            float tempReliefManaAmount = _ReliefManaAmount;
            _ReliefManaAmount = value;
            if (tempReliefManaAmount != _ReliefManaAmount)
            {
                Call<IAttributeState, float>(temp => temp.ReliefManaAmount);
            }
        }
    }

    private float _LuckShi;
    /// <summary>
    /// 幸运加护,获得优质物品概率与获取经验提升
    /// </summary>
    public float LuckShi
    {
        get { return _LuckShi; }
        set
        {
            float tempLuckShi = _LuckShi;
            _LuckShi = value;
            if (tempLuckShi != _LuckShi)
            {
                Call<IAttributeState, float>(temp => temp.LuckShi);
            }
        }
    }

    private float _GarShi;
    /// <summary>
    /// 庇佑加护,每隔一定时间获得一次免疫致死伤害的能力
    /// </summary>
    public float GarShi
    {
        get { return _GarShi; }
        set
        {
            float tempGarShi = _GarShi;
            _GarShi = value;
            if (tempGarShi != _GarShi)
            {
                Call<IAttributeState, float>(temp => temp.GarShi);
            }
        }
    }

    private float _WarShi;
    /// <summary>
    /// 战神加护,每隔一段时间获得一次在进入负面状态时清除自身所有负面效果的能力
    /// </summary>
    public float WarShi
    {
        get { return _WarShi; }
        set
        {
            float tempWarShi = _WarShi;
            _WarShi = value;
            if (tempWarShi != _WarShi)
            {
                Call<IAttributeState, float>(temp => temp.WarShi);
            }
        }
    }

    /// <summary>
    /// 回调字典
    /// </summary>
    Dictionary<Type, List<KeyValuePair<object, Action<IBaseState, string>>>> callBackDic;

    public string GetFieldName<T, U>(Expression<Func<T, U>> expr)
    {
        return GameState.GetFieldNameStatic<T, U>(expr);
    }

    public void Registor<T>(Action<T, string> CallbackAction) where T : IBaseState
    {
        if (CallbackAction == null)
            return;
        if (typeof(T).IsInterface
            && !Type.Equals(typeof(T), typeof(IBaseState))
            && typeof(T).GetInterface(typeof(IBaseState).Name) != null)
        {
            if (!callBackDic.ContainsKey(typeof(T)))
                return;
            List<KeyValuePair<object, Action<IBaseState, string>>> callBackList = callBackDic[typeof(T)];
            if (callBackList != null && !callBackList.Select(temp => temp.Key).Contains(CallbackAction))
            {
                Action<IBaseState, string> tempAction = (iBaseState, fieldName) =>
                {
                    try
                    {
                        T t = (T)iBaseState;
                        CallbackAction(t, fieldName);
                    }
                    catch { }
                };
                callBackList.Add(new KeyValuePair<object, Action<IBaseState, string>>(CallbackAction, tempAction));
            }
        }
    }

    public void UnRegistor<T>(Action<T, string> CallbackAction)
    {
        if (CallbackAction == null)
            return;
        foreach (KeyValuePair<Type, List<KeyValuePair<object, Action<IBaseState, string>>>> item in callBackDic)
        {
            List<KeyValuePair<object, Action<IBaseState, string>>> actionList = item.Value;
            int index = actionList.Select(temp => temp.Key).ToList().IndexOf(CallbackAction);
            if (index > -1)
            {
                actionList.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// 回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fieldName"></param>
    private void Call<T, U>(Expression<Func<T, U>> expr) where T : IBaseState
    {
        string propertyName = GetFieldName(expr);
        List<KeyValuePair<object, Action<IBaseState, string>>> actionList = null;
        if (callBackDic.TryGetValue(typeof(T), out actionList) && actionList != null)
        {
            foreach (KeyValuePair<object, Action<IBaseState, string>> item in actionList)
            {
                try
                {
                    item.Value(this, propertyName);
                }
                catch { }
            }
        }
    }
}

