using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 玩家身上的交互脚本
/// </summary>
class InteractiveAsPlayer : MonoBehaviour, IObjInteractive
{
    public T GetEntity<T>() where T : IBaseState
    {
        return default(T);
    }

    /// <summary>
    /// 造成伤害
    /// </summary>
    /// <param name="attackHurtStruct"></param>
    public void GiveAttackHurtStruct(AttackHurtStruct attackHurtStruct)
    {
        IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        IAttributeState playerAttribute = iPlayerState.GetResultAttribute();
        PhysicDefenseFactor physicDefenseFactor = new PhysicDefenseFactor()//物理防御系数
        {
            CoefficientRatioReducingDamageFactor = iPlayerState.SelfRoleOfRaceInfoStruct.physicDefenseToHurtRateRatio,
            ImmunityInjury = iPlayerState.SelfRoleOfRaceInfoStruct.physicQuickToHurtExemptRatio
        };
        MagicDefenseFactor magicDefenseFactor = new MagicDefenseFactor ()//魔法防御系数
        {

        };
        CalculateHurt.Calculate(attackHurtStruct, playerAttribute, physicDefenseFactor, magicDefenseFactor);
    }
}

