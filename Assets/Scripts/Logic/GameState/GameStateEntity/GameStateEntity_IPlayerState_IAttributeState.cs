using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现了IPlayerState->IAttributeState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState
{

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

}