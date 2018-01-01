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
    /// 构造函数
    /// </summary>
    public AttributeStateAdditional()
    {
        callBackDic = new Dictionary<Type, List<KeyValuePair<object, Action<IBaseState, string>>>>();
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
    /// 更新属性
    /// </summary>
    private void UpdateAttribute()
    {
        //通过基础属性修改下方的衍生属性
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

    private float _Dedicated;
    /// <summary>
    /// 专注
    /// </summary>
    public float Dedicated
    {
        get { return _Dedicated; }
        set
        {
            float tempDedicated = _Dedicated;
            _Dedicated = value;
            if (tempDedicated != _Dedicated)
            {
                UpdateAttribute();
                Call<IAttributeState, float>(temp => temp.Dedicated);
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

    private float _ElementAffine;
    /// <summary>
    /// 元素亲和
    /// </summary>
    public float ElementAffine
    {
        get { return _ElementAffine; }
        set
        {
            float tempElementAffine = _ElementAffine;
            _ElementAffine = value;
            if (tempElementAffine != _ElementAffine)
            {

                Call<IAttributeState, float>(temp => temp.ElementAffine);
            }
        }
    }

    private float _MagicAffine;
    /// <summary>
    /// 魔法亲和
    /// </summary>
    public float MagicAffine
    {
        get { return _MagicAffine; }
        set
        {
            float tempMagicAffine = _MagicAffine;
            _MagicAffine = value;
            if (tempMagicAffine != _MagicAffine)
            {

                Call<IAttributeState, float>(temp => temp.MagicAffine);
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

