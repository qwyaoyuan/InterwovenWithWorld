using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

/// <summary>
/// 游戏状态
/// </summary>
public class GameState : IEntrance,
    IGameState, IPlayerState
{
    /// <summary>
    /// 单例对象
    /// </summary>
    public static GameState Instance;

    /// <summary>
    /// 回调字典
    /// </summary>
    Dictionary<Type, List<KeyValuePair<Delegate, Action<IBaseState, string>>>> callBackDic;

    public void Start()
    {
        Instance = this;
        callBackDic = new Dictionary<Type, List<KeyValuePair<Delegate, Action<IBaseState, string>>>>();
        Type[] allType = GetType().GetInterfaces();
        foreach (Type type in allType)
        {
            if (type.IsInterface
                && !Type.Equals(type, typeof(IBaseState))
                && type.GetInterface(typeof(IBaseState).Name) != null)
            {
                List<KeyValuePair<Delegate, Action<IBaseState, string>>> callBackList = new List<KeyValuePair<Delegate, Action<IBaseState, string>>>();
                callBackDic.Add(type, callBackList);
            }
        }
    }

    public void Update()
    {

    }

    /// <summary>
    /// 注册监听状态改变回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    /// <param name="CallbackAction"></param>
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
            List<KeyValuePair<Delegate, Action<IBaseState, string>>> callBackList = callBackDic[typeof(T)];
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
                callBackList.Add(new KeyValuePair<Delegate, Action<IBaseState, string>>(CallbackAction, tempAction));
            }
        }
    }

    /// <summary>
    /// 移除注册
    /// </summary>
    /// <param name="d"></param>
    public void UnRegistor(Delegate d)
    {
        if (d == null)
            return;
        foreach (KeyValuePair<Type, List<KeyValuePair<Delegate, Action<IBaseState, string>>>> item in callBackDic)
        {
            List<KeyValuePair<Delegate, Action<IBaseState, string>>> actionList = item.Value;
            int index = actionList.Select(temp => temp.Key).ToList().IndexOf(d);
            if (index > -1)
            {
                actionList.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// 获取字段名
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="expr"></param>
    /// <returns></returns>
    public string GetFieldName<T, U>(Expression<Func<T, U>> expr)
    {
        string propertyName = string.Empty;
        if (expr.Body is MemberExpression)
        {
            propertyName = ((MemberExpression)expr.Body).Member.Name;
        }
        return propertyName;
    }

    /// <summary>
    /// 回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fieldName"></param>
    private void Call<T, U>(Expression<Func<T, U>> expr) where T : IBaseState
    {
        string propertyName = string.Empty;
        if (expr.Body is MemberExpression)
        {
            propertyName = ((MemberExpression)expr.Body).Member.Name;
            List<KeyValuePair<Delegate, Action<IBaseState, string>>> actionList = null;
            if (callBackDic.TryGetValue(typeof(T), out actionList) && actionList != null)
            {
                foreach (KeyValuePair<Delegate, Action<IBaseState, string>> item in actionList)
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

    public void OnDestroy()
    {

    }

    #region IGameState

    /// <summary>
    /// 游戏运行状态
    /// </summary>
    private EnumGameRunType _GameRunType;
    /// <summary>
    /// 游戏运行状态
    /// </summary>
    public EnumGameRunType GameRunType
    {
        get { return _GameRunType; }
        set
        {
            EnumGameRunType tempGameRunType = _GameRunType;
            _GameRunType = value;
            if (tempGameRunType != _GameRunType)
                Call<IGameState, EnumGameRunType>(temp => temp.GameRunType);
        }
    }

    #endregion

    #region IPlayerState 角色的状态，包括属性状态 buff状态 debuff状态 特殊状态 
    #region 自身状态
    /// <summary>
    /// 更新自身属性
    /// 在等级变化 装备变化时触发
    /// 主要更新的是HP MP上限,防御攻击等等随等级装备变化的属性等
    /// </summary>
    public void UpdateAttribute() { }

    /// <summary>
    /// 等级
    /// </summary>
    int _Level;
    /// <summary>
    /// 等级
    /// </summary>
    public int Level
    {
        get
        {
            return _Level;
        }
        set
        {
            int tempLevel = _Level;
            _Level = value;
            if (tempLevel != _Level)
            {
                //处理存档内的等级

                //更新自身属性
                UpdateAttribute();
                //回调
                Call<IPlayerState, int>(temp => temp.Level);
            }
        }
    }

    /// <summary>
    /// 技能等级变化
    /// </summary>
    bool _SkillLevelChanged;

    /// <summary>
    /// 技能等级变化
    /// </summary>
    public bool SkillLevelChanged
    {
        get { return _SkillLevelChanged; }
        set
        {
            if (value)
            {
                _SkillLevelChanged = true;
                //更新自身属性
                UpdateAttribute();
                //回调
                Call<IPlayerState, bool>(temp => temp.SkillLevelChanged);
                _SkillLevelChanged = false;
            }
        }
    }

    /// <summary>
    /// 种族等级变化
    /// </summary>
    bool _RaceLevelChanged;

    /// <summary>
    /// 种族等级变化
    /// </summary>
    public bool RaceLevelChanged
    {
        get { return _RaceLevelChanged; }
        set
        {
            if (value)
            {
                _RaceLevelChanged = value;
                //更新自身属性
                UpdateAttribute();
                //回调
                Call<IPlayerState, bool>(temp => temp.RaceLevelChanged);
                _RaceLevelChanged = false;
            }
        }
    }

    /// <summary>
    /// 装备发生变化
    /// </summary>
    bool _EquipmentChanged;
    /// <summary>
    /// 装备发生变化
    /// </summary>
    public bool EquipmentChanged
    {
        get { return _EquipmentChanged; }
        set
        {
            if (value)
            {
                _EquipmentChanged = value;
                //更新自身属性
                UpdateAttribute();
                //回调
                Call<IPlayerState, bool>(temp => temp.EquipmentChanged);
                _EquipmentChanged = value;
            }
        }
    }

    #endregion
    #region IAttributeState 属性状态
    #region 基础属性
    /// <summary>
    /// 敏捷
    /// </summary>
    float _Quick;
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
                Call<IAttributeState, float>(temp => temp.Quick);
        }
    }

    /// <summary>
    /// 专注
    /// </summary>
    float _Dedicated;
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
                Call<IAttributeState, float>(temp => temp.Dedicated);
        }
    }

    /// <summary>
    /// 精神
    /// </summary>
    float _Mental;
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
            if (_Mental != tempMental)
            {
                Call<IAttributeState, float>(temp => temp.Mental);
            }
        }
    }

    /// <summary>
    /// 力量
    /// </summary>
    float _Power;
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
                Call<IAttributeState, float>(temp => temp.Power);
        }
    }
    #endregion
    #region 常规属性
    /// <summary>
    /// 血量
    /// </summary>
    float _HP;
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
                Call<IAttributeState, float>(temp => temp.HP);
        }
    }

    /// <summary>
    /// 最大血量
    /// </summary>
    float _MaxHP;
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
                Call<IAttributeState, float>(temp => temp.MaxHP);
        }
    }

    /// <summary>
    /// 魔力量
    /// </summary>
    float _Mana;
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
                Call<IAttributeState, float>(temp => temp.Mana);
        }
    }

    /// <summary>
    /// 最大魔力量
    /// </summary>
    float _MaxMana;
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
                Call<IAttributeState, float>(temp => temp.MaxMana);
        }
    }

    /// <summary>
    /// 视野范围
    /// </summary>
    float _View;
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
                Call<IAttributeState, float>(temp => temp.View);
        }
    }

    /// <summary>
    /// 移动速度
    /// </summary>
    float _MoveSpeed;
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
                Call<IAttributeState, float>(temp => temp.MoveSpeed);
        }
    }

    /// <summary>
    /// 攻击速度
    /// </summary>
    float _AttackSpeed;
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
                Call<IAttributeState, float>(temp => temp.AttackSpeed);
        }
    }

    /// <summary>
    /// 命中率
    /// </summary>
    float _HitRate;
    /// <summary>
    ///命中率
    /// </summary>
    public float HitRate
    {
        get { return _HitRate; }
        set
        {
            float tempHitRate = _HitRate;
            _HitRate = value;
            if (tempHitRate != _HitRate)
                Call<IAttributeState, float>(temp => temp.HitRate);
        }
    }

    /// <summary>
    /// 闪避率
    /// </summary>
    float _EvadeRate;
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
                Call<IAttributeState, float>(temp => temp.EvadeRate);
        }
    }

    /// <summary>
    /// 暴击率
    /// </summary>
    float _CritRate;
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
                Call<IAttributeState, float>(temp => temp.CritRate);
        }
    }
    #endregion
    #region 回复
    /// <summary>
    /// 生命恢复速度
    /// </summary>
    float _LifeRecovery;
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
                Call<IAttributeState, float>(temp => temp.LifeRecovery);
        }
    }

    /// <summary>
    /// 法力恢复速度
    /// </summary>
    float _ManaRecovery;
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
                Call<IAttributeState, float>(temp => temp.ManaRecovery);
        }
    }
    #endregion
    #region 攻击与防御属性
    /// <summary>
    /// 道具攻击力
    /// </summary>
    float _ItemAttacking;
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
                Call<IAttributeState, float>(temp => temp.ItemAttacking);
        }
    }

    /// <summary>
    /// 魔法攻击力
    /// </summary>
    float _MagicAttacking;
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
                Call<IAttributeState, float>(temp => temp.MagicAttacking);
        }
    }

    /// <summary>
    /// 物理攻击力
    /// </summary>
    float _PhysicsAttacking;
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
                Call<IAttributeState, float>(temp => temp.PhysicsAttacking);
        }
    }

    /// <summary>
    /// 魔法附加伤害
    /// </summary>
    float _MagicAdditionalDamage;
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
                Call<IAttributeState, float>(temp => temp.MagicAdditionalDamage);
        }
    }

    /// <summary>
    /// 物理附加伤害
    /// </summary>
    float _PhysicsAdditionalDamage;
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
                Call<IAttributeState, float>(temp => temp.PhysicsAdditionalDamage);
        }
    }

    /// <summary>
    /// 魔法攻击穿透
    /// </summary>
    float _MagicPenetrate;
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
                Call<IAttributeState, float>(temp => temp.MagicPenetrate);
        }
    }

    /// <summary>
    /// 物理攻击穿透
    /// </summary>
    float _PhysicsPenetrate;
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
                Call<IAttributeState, float>(temp => temp.PhysicsPenetrate);
        }
    }

    /// <summary>
    /// 魔法最终伤害
    /// </summary>
    float _MagicFinalDamage;
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
                Call<IAttributeState, float>(temp => temp.MagicFinalDamage);
        }
    }

    /// <summary>
    /// 物理最终伤害
    /// </summary>
    float _PhysicsFinalDamage;
    /// <summary>
    /// 物理最终伤害
    /// </summary>
    public float PhysicsFinalDamage
    {
        get { return PhysicsFinalDamage; }
        set
        {
            float tempPhysicsFinalDamage = _PhysicsFinalDamage;
            _PhysicsFinalDamage = value;
            if (tempPhysicsFinalDamage != _PhysicsFinalDamage)
                Call<IAttributeState, float>(temp => temp.PhysicsFinalDamage);
        }
    }

    /// <summary>
    /// 元素亲和
    /// </summary>
    float _ElementAffine;
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
                Call<IAttributeState, float>(temp => temp.ElementAffine);
        }
    }

    /// <summary>
    /// 魔法亲和
    /// </summary>
    float _MagicAffine;
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
                Call<IAttributeState, float>(temp => temp.MagicAffine);
        }
    }

    /// <summary>
    /// 魔法抗性
    /// </summary>
    float _MagicResistance;
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
                Call<IAttributeState, float>(temp => temp.MagicResistance);
        }
    }

    /// <summary>
    /// 物理抗性
    /// </summary>
    float _PhysicsResistance;
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
                Call<IAttributeState, float>(temp => temp.PhysicsResistance);
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
        }
    }
    #endregion
    #endregion
    #region IBuffSate  Buff状态
    /// <summary>
    /// 活力
    /// </summary>
    BuffState? _Huoli;
    /// <summary>
    /// 活力
    /// </summary>
    public BuffState Huoli
    {
        get
        {
            if (_Huoli == null)
                _Huoli = new BuffState(EnumStatusEffect.hl2, 0);
            return _Huoli.Value;
        }
        set
        {
            if (_Huoli == null)
                _Huoli = Huoli;
            if (_Huoli.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempHuoLi = _Huoli.Value;
            _Huoli = value;
            if (!tempHuoLi.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.Huoli);

        }
    }

    /// <summary>
    /// 加速
    /// </summary>
    BuffState? _Jiasu;
    /// <summary>
    /// 加速
    /// </summary>
    public BuffState Jiasu
    {
        get
        {
            if (_Jiasu == null)
                _Jiasu = new BuffState(EnumStatusEffect.js1, 0);
            return _Jiasu.Value;
        }
        set
        {
            if (_Jiasu == null)
                _Jiasu = Jiasu;
            if (_Jiasu.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempJiasu = _Jiasu.Value;
            _Jiasu = value;
            if (!tempJiasu.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.Jiasu);

        }
    }

    /// <summary>
    /// 净化
    /// </summary>
    BuffState? _Jinghua;
    /// <summary>
    /// 净化
    /// </summary>
    public BuffState Jinghua
    {
        get
        {
            if (_Jinghua == null)
                _Jinghua = new BuffState(EnumStatusEffect.jh5, 0);
            return _Jinghua.Value;
        }
        set
        {
            if (_Jinghua == null)
                _Jinghua = Jinghua;
            if (_Jinghua.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempJinghua = _Jinghua.Value;
            _Jinghua = value;
            if (!tempJinghua.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.Jinghua);

        }
    }

    /// <summary>
    /// 敏捷
    /// </summary>
    BuffState? _Minjie;
    /// <summary>
    /// 敏捷
    /// </summary>
    public BuffState Minjie
    {
        get
        {
            if (_Minjie == null)
                _Minjie = new BuffState(EnumStatusEffect.mj1, 0);
            return _Minjie.Value;
        }
        set
        {
            if (_Minjie == null)
                _Minjie = Minjie;
            if (_Minjie.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempMinjie = _Minjie.Value;
            _Minjie = value;
            if (!tempMinjie.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.Minjie);

        }
    }

    /// <summary>
    /// 强力
    /// </summary>
    BuffState? _Qiangli;
    /// <summary>
    /// 强力
    /// </summary>
    public BuffState Qiangli
    {
        get
        {
            if (_Qiangli == null)
                _Qiangli = new BuffState(EnumStatusEffect.ql1, 0);
            return _Qiangli.Value;
        }
        set
        {
            if (_Qiangli == null)
                _Qiangli = Qiangli;
            if (_Qiangli.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempQiangli = _Qiangli.Value;
            _Qiangli = value;
            if (!tempQiangli.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.Qiangli);

        }
    }

    /// <summary>
    /// 驱散
    /// </summary>
    BuffState? _Qusan;
    /// <summary>
    /// 驱散
    /// </summary>
    public BuffState Qusan
    {
        get
        {
            if (_Qusan == null)
                _Qusan = new BuffState(EnumStatusEffect.qs2, 0);
            return _Qusan.Value;
        }
        set
        {
            if (_Qusan == null)
                _Qusan = Qusan;
            if (_Qusan.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempQusan = _Qusan.Value;
            _Qusan = value;
            if (!tempQusan.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.Qusan);

        }
    }

    /// <summary>
    /// 睿智
    /// </summary>
    BuffState? _Ruizhi;
    /// <summary>
    /// 睿智
    /// </summary>
    public BuffState Ruizhi
    {
        get
        {
            if (_Ruizhi == null)
                _Ruizhi = new BuffState(EnumStatusEffect.rz1, 0);
            return _Ruizhi.Value;
        }
        set
        {
            if (_Ruizhi == null)
                _Ruizhi = Ruizhi;
            if (_Ruizhi.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempRuizhi = _Ruizhi.Value;
            _Ruizhi = value;
            if (!tempRuizhi.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.Ruizhi);

        }
    }

    /// <summary>
    /// 吸血-物理
    /// </summary>
    BuffState? _XixueWuli;
    /// <summary>
    /// 吸血-物理
    /// </summary>
    public BuffState XixueWuli
    {
        get
        {
            if (_XixueWuli == null)
                _XixueWuli = new BuffState(EnumStatusEffect.xx3, 0);
            return _XixueWuli.Value;
        }
        set
        {
            if (_XixueWuli == null)
                _XixueWuli = XixueWuli;
            if (_XixueWuli.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempXixueWuli = _XixueWuli.Value;
            _XixueWuli = value;
            if (!tempXixueWuli.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.XixueWuli);

        }
    }

    /// <summary>
    /// 吸血-魔法
    /// </summary>
    BuffState? _XixueMofa;
    /// <summary>
    /// 吸血-魔法
    /// </summary>
    public BuffState XixueMofa
    {
        get
        {
            if (_XixueMofa == null)
                _XixueMofa = new BuffState(EnumStatusEffect.xx4, 0);
            return _XixueMofa.Value;
        }
        set
        {
            if (_XixueMofa == null)
                _XixueMofa = XixueMofa;
            if (_XixueMofa.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempXixueMofa = _XixueMofa.Value;
            _XixueMofa = value;
            if (!tempXixueMofa.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.XixueMofa);

        }
    }


    #endregion
    #region ISpecialState 特殊状态

    /// <summary>
    /// 嘲讽
    /// </summary>
    BuffState? _Chaofeng;
    /// <summary>
    /// 嘲讽
    /// </summary>
    public BuffState Chaofeng
    {
        get
        {
            if (_Chaofeng == null)
                _Chaofeng = new BuffState(EnumStatusEffect.cf2, 0);
            return _Chaofeng.Value;
        }
        set
        {
            if (_Chaofeng == null)
                _Chaofeng = Chaofeng;
            if (_Chaofeng.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempChaofeng = _Chaofeng.Value;
            _Chaofeng = value;
            if (!tempChaofeng.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Chaofeng);

        }
    }

    /// <summary>
    /// 混乱
    /// </summary>
    BuffState? _Hunluan;
    /// <summary>
    /// 混乱
    /// </summary>
    public BuffState Hunluan
    {
        get
        {
            if (_Hunluan == null)
                _Hunluan = new BuffState(EnumStatusEffect.hl1, 0);
            return _Hunluan.Value;
        }
        set
        {
            if (_Hunluan == null)
                _Hunluan = Hunluan;
            if (_Hunluan.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempHunluan = _Hunluan.Value;
            _Hunluan = value;
            if (!tempHunluan.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Hunluan);

        }
    }

    /// <summary>
    /// 僵直
    /// </summary>
    BuffState? _Jiangzhi;
    /// <summary>
    /// 僵直
    /// </summary>
    public BuffState Jiangzhi
    {
        get
        {
            if (_Jiangzhi == null)
                _Jiangzhi = new BuffState(EnumStatusEffect.jz6, 0);
            return _Jiangzhi.Value;
        }
        set
        {
            if (_Jiangzhi == null)
                _Jiangzhi = Jiangzhi;
            if (_Jiangzhi.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempJiangzhi = _Jiangzhi.Value;
            _Jiangzhi = value;
            if (!tempJiangzhi.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Jiangzhi);

        }
    }

    /// <summary>
    /// 恐惧
    /// </summary>
    BuffState? _Kongju;
    /// <summary>
    /// 恐惧
    /// </summary>
    public BuffState Kongju
    {
        get
        {
            if (_Kongju == null)
                _Kongju = new BuffState(EnumStatusEffect.kj1, 0);
            return _Kongju.Value;
        }
        set
        {
            if (_Kongju == null)
                _Kongju = Kongju;
            if (_Kongju.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempKongju = _Kongju.Value;
            _Kongju = value;
            if (!tempKongju.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Kongju);

        }
    }

    /// <summary>
    /// 魅惑
    /// </summary>
    BuffState? _Meihuo;
    /// <summary>
    /// 魅惑
    /// </summary>
    public BuffState Meihuo
    {
        get
        {
            if (_Meihuo == null)
                _Meihuo = new BuffState(EnumStatusEffect.mh4, 0);
            return _Meihuo.Value;
        }
        set
        {
            if (_Meihuo == null)
                _Meihuo = Meihuo;
            if (_Meihuo.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempMeihuo = _Meihuo.Value;
            _Meihuo = value;
            if (!tempMeihuo.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Meihuo);

        }
    }

    /// <summary>
    /// 眩晕
    /// </summary>
    BuffState? _Xuanyun;
    /// <summary>
    /// 眩晕
    /// </summary>
    public BuffState Xuanyun
    {
        get
        {
            if (_Xuanyun == null)
                _Xuanyun = new BuffState(EnumStatusEffect.xy1, 0);
            return _Xuanyun.Value;
        }
        set
        {
            if (_Xuanyun == null)
                _Xuanyun = Xuanyun;
            if (_Xuanyun.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempXuanyun = _Xuanyun.Value;
            _Xuanyun = value;
            if (!tempXuanyun.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Xuanyun);

        }
    }

    /// <summary>
    /// 致盲
    /// </summary>
    BuffState? _Zhimang;
    /// <summary>
    /// 致盲
    /// </summary>
    public BuffState Zhimang
    {
        get
        {
            if (_Zhimang == null)
                _Zhimang = new BuffState(EnumStatusEffect.zm1, 0);
            return _Zhimang.Value;
        }
        set
        {
            if (_Zhimang == null)
                _Zhimang = Zhimang;
            if (_Zhimang.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempZhimang = _Zhimang.Value;
            _Zhimang = value;
            if (!tempZhimang.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Zhimang);

        }
    }

    /// <summary>
    /// 禁锢
    /// </summary>
    BuffState? _Jingu;
    /// <summary>
    /// 禁锢
    /// </summary>
    public BuffState Jingu
    {
        get
        {
            if (_Jingu == null)
                _Jingu = new BuffState(EnumStatusEffect.jg2, 0);
            return _Jingu.Value;
        }
        set
        {
            if (_Jingu == null)
                _Jingu = Jingu;
            if (_Jingu.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempJingu = _Jingu.Value;
            _Jingu = value;
            if (!tempJingu.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Jingu);

        }
    }

    /// <summary>
    /// 禁魔
    /// </summary>
    BuffState? _Jinmo;
    /// <summary>
    /// 禁魔
    /// </summary>
    public BuffState Jinmo
    {
        get
        {
            if (_Jinmo == null)
                _Jinmo = new BuffState(EnumStatusEffect.jm3, 0);
            return _Jinmo.Value;
        }
        set
        {
            if (_Jinmo == null)
                _Jinmo = Jinmo;
            if (_Jinmo.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempJinmo = _Jinmo.Value;
            _Jinmo = value;
            if (!tempJinmo.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Jinmo);

        }
    }

    /// <summary>
    /// 麻痹
    /// </summary>
    BuffState? _Mabi;
    /// <summary>
    /// 麻痹
    /// </summary>
    public BuffState Mabi
    {
        get
        {
            if (_Mabi == null)
                _Mabi = new BuffState(EnumStatusEffect.mb2, 0);
            return _Mabi.Value;
        }
        set
        {
            if (_Mabi == null)
                _Mabi = Mabi;
            if (_Mabi.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempMabi = _Mabi.Value;
            _Mabi = value;
            if (!tempMabi.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Mabi);

        }
    }


    #endregion
    #region IDebuffState Debuff状态
    /// <summary>
    /// 冰冻
    /// </summary>
    BuffState? _Bingdong;
    /// <summary>
    /// 冰冻
    /// </summary>
    public BuffState Bingdong
    {
        get
        {
            if (_Bingdong == null)
                _Bingdong = new BuffState(EnumStatusEffect.bd1, 0);
            return _Bingdong.Value;
        }
        set
        {
            if (_Bingdong == null)
                _Bingdong = Bingdong;
            if (_Bingdong.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempBingdong = _Bingdong.Value;
            _Bingdong = value;
            if (!tempBingdong.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Bingdong);

        }
    }

    /// <summary>
    /// 迟钝
    /// </summary>
    BuffState? _Chidun;
    /// <summary>
    /// 迟钝
    /// </summary>
    public BuffState Chidun
    {
        get
        {
            if (_Chidun == null)
                _Chidun = new BuffState(EnumStatusEffect.cd1, 0);
            return _Chidun.Value;
        }
        set
        {
            if (_Chidun == null)
                _Chidun = Chidun;
            if (_Chidun.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempChidun = _Chidun.Value;
            _Chidun = value;
            if (!tempChidun.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Chidun);

        }
    }

    /// <summary>
    /// 点燃
    /// </summary>
    BuffState? _Dianran;
    /// <summary>
    /// 点燃
    /// </summary>
    public BuffState Dianran
    {
        get
        {
            if (_Dianran == null)
                _Dianran = new BuffState(EnumStatusEffect.dr1, 0);
            return _Dianran.Value;
        }
        set
        {
            if (_Dianran == null)
                _Dianran = Dianran;
            if (_Dianran.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempDianran = _Dianran.Value;
            _Dianran = value;
            if (!tempDianran.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Dianran);

        }
    }

    /// <summary>
    /// 凋零
    /// </summary>
    BuffState? _Diaoling;
    /// <summary>
    /// 凋零
    /// </summary>
    public BuffState Diaoling
    {
        get
        {
            if (_Diaoling == null)
                _Diaoling = new BuffState(EnumStatusEffect.dl3, 0);
            return _Diaoling.Value;
        }
        set
        {
            if (_Diaoling == null)
                _Diaoling = Diaoling;
            if (_Diaoling.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempDiaoling = _Diaoling.Value;
            _Diaoling = value;
            if (!tempDiaoling.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Diaoling);

        }
    }

    /// <summary>
    /// 减速
    /// </summary>
    BuffState? _Jiansu;
    /// <summary>
    /// 减速
    /// </summary>
    public BuffState Jiansu
    {
        get
        {
            if (_Jiansu == null)
                _Jiansu = new BuffState(EnumStatusEffect.js4, 0);
            return _Jiansu.Value;
        }
        set
        {
            if (_Jiansu == null)
                _Jiansu = Jiansu;
            if (_Jiansu.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempJiansu = _Jiansu.Value;
            _Jiansu = value;
            if (!tempJiansu.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Jiansu);

        }
    }

    /// <summary>
    /// 迷惑
    /// </summary>
    BuffState? _Mihuo;
    /// <summary>
    /// 迷惑
    /// </summary>
    public BuffState Mihuo
    {
        get
        {
            if (_Mihuo == null)
                _Mihuo = new BuffState(EnumStatusEffect.mh3, 0);
            return _Mihuo.Value;
        }
        set
        {
            if (_Mihuo == null)
                _Mihuo = Mihuo;
            if (_Mihuo.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempMihuo = _Mihuo.Value;
            _Mihuo = value;
            if (!tempMihuo.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Mihuo);

        }
    }

    /// <summary>
    /// 无力
    /// </summary>
    BuffState? _Wuli;
    /// <summary>
    /// 无力
    /// </summary>
    public BuffState Wuli
    {
        get
        {
            if (_Wuli == null)
                _Wuli = new BuffState(EnumStatusEffect.wl1, 0);
            return _Wuli.Value;
        }
        set
        {
            if (_Wuli == null)
                _Wuli = Wuli;
            if (_Wuli.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempWuli = _Wuli.Value;
            _Wuli = value;
            if (!tempWuli.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Wuli);

        }
    }

    /// <summary>
    /// 虚弱
    /// </summary>
    BuffState? _Xuruo;
    /// <summary>
    /// 虚弱
    /// </summary>
    public BuffState Xuruo
    {
        get
        {
            if (_Xuruo == null)
                _Xuruo = new BuffState(EnumStatusEffect.xr2, 0);
            return _Xuruo.Value;
        }
        set
        {
            if (_Xuruo == null)
                _Xuruo = Xuruo;
            if (_Xuruo.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempXuruo = _Xuruo.Value;
            _Xuruo = value;
            if (!tempXuruo.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Xuruo);

        }
    }

    /// <summary>
    /// 中毒
    /// </summary>
    BuffState? _Zhongdu;
    /// <summary>
    /// 中毒
    /// </summary>
    public BuffState Zhongdu
    {
        get
        {
            if (_Zhongdu == null)
                _Zhongdu = new BuffState(EnumStatusEffect.zd2, 0);
            return _Zhongdu.Value;
        }
        set
        {
            if (_Zhongdu == null)
                _Zhongdu = Zhongdu;
            if (_Zhongdu.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempZhongdu = _Zhongdu.Value;
            _Zhongdu = value;
            if (!tempZhongdu.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Zhongdu);

        }
    }

    /// <summary>
    /// 诅咒
    /// </summary>
    BuffState? _Zuzhou;
    /// <summary>
    /// 诅咒
    /// </summary>
    public BuffState Zuzhou
    {
        get
        {
            if (_Zuzhou == null)
                _Zuzhou = new BuffState(EnumStatusEffect.zz3, 0);
            return _Zuzhou.Value;
        }
        set
        {
            if (_Zuzhou == null)
                _Zuzhou = Zuzhou;
            if (_Zuzhou.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempZuzhou = _Zuzhou.Value;
            _Zuzhou = value;
            if (!tempZuzhou.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Zuzhou);

        }
    }


    #endregion
    #region IAnimatorState 动画状态
    /// <summary>
    /// 当前的魔法动画类型
    /// </summary>
    EnumMagicAnimatorType _MagicAnimatorType;
    /// <summary>
    /// 当前的魔法动画类型
    /// </summary>
    public EnumMagicAnimatorType MagicAnimatorType
    {
        get { return _MagicAnimatorType; }
        set
        {
            EnumMagicAnimatorType tempMagicAnimatorType = _MagicAnimatorType;
            _MagicAnimatorType = value;
            if (tempMagicAnimatorType != _MagicAnimatorType)
                Call<IAnimatorState, EnumMagicAnimatorType>(temp => temp.MagicAnimatorType);
        }
    }

    /// <summary>
    /// 当前的物理动画类型
    /// </summary>
    EnumPhysicAnimatorType _PhysicAnimatorType;
    /// <summary>
    /// 当前的物理动画类型
    /// </summary>
    public EnumPhysicAnimatorType PhysicAnimatorType
    {
        get { return _PhysicAnimatorType; }
        set
        {
            EnumPhysicAnimatorType tempPhysicAnimatorType = _PhysicAnimatorType;
            _PhysicAnimatorType = value;
            if (tempPhysicAnimatorType != _PhysicAnimatorType)
                Call<IAnimatorState, EnumPhysicAnimatorType>(temp => temp.PhysicAnimatorType);
        }
    }

    /// <summary>
    /// 当前的交互动画类型
    /// </summary>
    EnumMutualAnimatorType _MutualAnimatorType;
    /// <summary>
    /// 当前的交互动画类型
    /// </summary>
    public EnumMutualAnimatorType MutualAnimatorType
    {
        get { return _MutualAnimatorType; }
        set
        {
            EnumMutualAnimatorType tempMutualAnimatorType = _MutualAnimatorType;
            _MutualAnimatorType = value;
            if (tempMutualAnimatorType != _MutualAnimatorType)
                Call<IAnimatorState, EnumMutualAnimatorType>(temp => temp.MutualAnimatorType);
        }
    }

    /// <summary>
    /// 当前的移动方向类型
    /// </summary>
    int _MoveAnimatorVectorType;
    /// <summary>
    /// 当前的移动方向类型
    /// </summary>
    public int MoveAnimatorVectorType
    {
        get { return _MoveAnimatorVectorType; }
        set
        {
            int tempMoveAnimatorVectorType = _MoveAnimatorVectorType;
            _MoveAnimatorVectorType = value;
            if (tempMoveAnimatorVectorType != _MoveAnimatorVectorType)
                Call<IAnimatorState, int>(temp => temp.MoveAnimatorVectorType);
        }
    }


    #endregion
    #endregion
}

/// <summary>
/// 基础状态，所有的状态接口都必须继承自本接口
/// </summary>
public interface IBaseState
{
    /// <summary>
    /// 注册监听状态改变回调
    /// </summary>
    /// <param name="target"></param>
    /// <param name="CallBackAction"></param>
    void Registor<T>(Action<T, string> CallBackAction) where T : IBaseState;
    /// <summary>
    /// 移除注册
    /// </summary>
    /// <param name="d"></param>
    void UnRegistor(Delegate d);
    /// <summary>
    /// 获取字段或属性名 
    /// </summary>
    /// <param name="expr"></param>
    /// <returns></returns>
    string GetFieldName<T, U>(Expression<Func<T, U>> expr);
}
