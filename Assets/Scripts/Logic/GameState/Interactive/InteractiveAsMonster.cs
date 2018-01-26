using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 怪物身上的交互脚本
/// </summary>
class InteractiveAsMonster : MonoBehaviour, IObjInteractive
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
        MonsterControl monsterControl = GetComponent<MonsterControl>();
        if (monsterControl != null 
            && monsterControl.monsterDataInfo != null 
            && monsterControl.monsterDataInfo.MonsterBaseAttribute != null)
        {
            
        }
    }
}

