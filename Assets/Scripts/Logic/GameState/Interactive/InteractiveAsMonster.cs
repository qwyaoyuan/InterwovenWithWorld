using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 怪物身上的交互脚本
/// </summary>
class InteractiveAsMonster : InteractiveBaseMono, IObjInteractive
{
    /// <summary>
    /// 造成伤害
    /// </summary>
    /// <param name="attackHurtStruct"></param>
    public override CalculateHurt.Result GiveAttackHurtStruct(AttackHurtStruct attackHurtStruct)
    {
        MonsterControl monsterControl = GetComponent<MonsterControl>();
        if (monsterControl != null
            && monsterControl.monsterDataInfo != null
            && monsterControl.monsterDataInfo.MonsterBaseAttribute != null)
        {
            PhysicDefenseFactor physicDefenseFactor = monsterControl.monsterDataInfo.PhysicDefenseFactor;//物理防御系数
            MagicDefenseFactor magicDefenseFactor = monsterControl.monsterDataInfo.MagicDefenseFactor;//魔法防御系数
            CalculateHurt.Result result = CalculateHurt.Calculate(attackHurtStruct, monsterControl.thisAttribute, physicDefenseFactor, magicDefenseFactor);
            monsterControl.GiveHit();
            //显示伤害 
            base.ShowHurt(result, gameObject);
            //显示怪物的血条
            iGameState.ShowMonsterHP = new MonsterHPUIStruct()
            {
                monsterName = monsterControl.monsterDataInfo.monsterPrefabName,
                maxHP = monsterControl.thisAttribute.MaxHP,
                nowHP = monsterControl.thisAttribute.HP,
                monsterObj = gameObject
            };
            return result;
        }
        return default(CalculateHurt.Result);
    }
}

