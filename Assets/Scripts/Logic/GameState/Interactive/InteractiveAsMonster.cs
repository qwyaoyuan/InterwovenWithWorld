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
        throw new NotImplementedException();
    }

    public void GiveAttackHurtStruct(AttackHurtStruct attackHurtStruct)
    {
        throw new NotImplementedException();
    }
}

