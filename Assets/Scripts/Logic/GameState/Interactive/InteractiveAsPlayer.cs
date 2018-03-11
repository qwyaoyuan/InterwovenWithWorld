using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 玩家身上的交互脚本
/// </summary>
class InteractiveAsPlayer : InteractiveBaseMono, IObjInteractive
{
    /// <summary>
    /// 造成伤害
    /// </summary>
    /// <param name="attackHurtStruct"></param>
    public override CalculateHurt.Result GiveAttackHurtStruct(AttackHurtStruct attackHurtStruct)
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
            CoefficientRatioReducingDamageFactor = iPlayerState.SelfRoleOfRaceInfoStruct.magicDefenseToHurtRateRatio
        };
        CalculateHurt.Result calculateHurtResult = CalculateHurt.Calculate(attackHurtStruct, playerAttribute, physicDefenseFactor, magicDefenseFactor);
        if (calculateHurtResult.hurt >= 0)
        {
            //终端咏唱
            ISkillState iSkillState = GameState.Instance.GetEntity<ISkillState>();
            iSkillState.GetHitToSkillState();
            //手柄震动
            iPlayerState.SetVibration(0.1f, 0.7f, 0.7f);
            //显示伤害 
            base.ShowHurt(calculateHurtResult, iPlayerState.PlayerObj);
        }
        return calculateHurtResult;
    }
}

