using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色或怪物的属性（血量 攻击 防御等）
/// </summary>
public interface IAttributeState : IBaseState
{
    #region 这些属性会影响到下面的计算 而这些属性由角色自身以及装备累加计算
    /// <summary>
    /// 敏捷
    /// </summary>
    float Quick { get; set; }
    /// <summary>
    /// 专注
    /// </summary>
    float Dedicated { get; set; }
    /// <summary>
    /// 精神
    /// </summary>
    float Mental { get; set; }
    /// <summary>
    /// 力量
    /// </summary>
    float Power { get; set; }
    #endregion

    #region 常规属性
    /// <summary>
    /// 血量
    /// </summary>
    float HP { get; set; }
    /// <summary>
    /// 最大血量
    /// </summary>
    float MaxHP { get; set; }
    /// <summary>
    /// 魔力量
    /// </summary>
    float Mana { get; set; }
    /// <summary>
    /// 最大魔力量
    /// </summary>
    float MaxMana { get; set; }
    /// <summary>
    /// 视野范围
    /// </summary>
    float View { get; set; }
    /// <summary>
    /// 移动速度
    /// </summary>
    float MoveSpeed { get; set; }
    /// <summary>
    /// 攻击速度
    /// </summary>
    float AttackSpeed { get; set; }
    /// <summary>
    /// 命中率
    /// </summary>
    float HitRate { get; set; }
    /// <summary>
    /// 闪避率
    /// </summary>
    float EvadeRate { get; set; }
    /// <summary>
    /// 暴击率
    /// </summary>
    float CritRate { get; set; }
    #endregion

    #region 回复
    /// <summary>
    /// 生命恢复速度
    /// </summary>
    float LifeRecovery { get; set; }
    /// <summary>
    /// 法力恢复速度
    /// </summary>
    float ManaRecovery { get; set; }
    #endregion

    #region 攻击与防御属性
    /// <summary>
    /// 道具攻击力
    /// </summary>
    float ItemAttacking { get; set; }
    /// <summary>
    /// 魔法攻击力
    /// </summary>
    float MagicAttacking { get; set; }
    /// <summary>
    /// 物理攻击力
    /// </summary>
    float PhysicsAttacking { get; set; }
    /// <summary>
    /// 魔法附加伤害 
    /// </summary>
    float MagicAdditionalDamage { get; set; }
    /// <summary>
    /// 物理伤害附加
    /// </summary>
    float PhysicsAdditionalDamage { get; set; }
    /// <summary>
    /// 魔法攻击穿透
    /// </summary>
    float MagicPenetrate { get; set; }
    /// <summary>
    /// 物理攻击穿透
    /// </summary>
    float PhysicsPenetrate { get; set; }
    /// <summary>
    /// 魔法最终伤害
    /// </summary>
    float MagicFinalDamage { get; set; }
    /// <summary>
    /// 物理最终伤害
    /// </summary>
    float PhysicsFinalDamage { get; set; }
    /// <summary>
    /// 元素亲和
    /// </summary>
    float ElementAffine { get; set; }
    /// <summary>
    /// 魔法亲和
    /// </summary>
    float MagicAffine { get; set; }

    /// <summary>
    /// 魔法抗性（魔法防御）
    /// </summary>
    float MagicResistance { get; set; }
    /// <summary>
    /// 物理抗性（物理防御）
    /// </summary>
    float PhysicsResistance { get; set; }
    /// <summary>
    /// 元素抗性,根据元素类型枚举的顺序
    /// </summary>
    float[] ElementResistances { get; set; }
    /// <summary>
    /// 状态抗性，按照EnumStatusEffect枚举（状态类型）的顺序
    /// </summary>
    float[] StateResistances { get; set; }
    #endregion
}
