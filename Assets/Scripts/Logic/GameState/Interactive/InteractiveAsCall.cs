using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 召唤物身上的交互脚本
/// </summary>
class InteractiveAsCall : MonoBehaviour, IObjInteractive
{
    public T GetEntity<T>() where T : IBaseState
    {
        throw new NotImplementedException();
    }

    public CalculateHurt.Result GiveAttackHurtStruct(AttackHurtStruct attackHurtStruct)
    {
        throw new NotImplementedException();
    }
}

