using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 对象交互接口
/// </summary>
public interface IObjInteractive
{
    /// <summary>
    /// 造成伤害
    /// </summary>
    /// <param name="attackHurtStruct"></param>
    CalculateHurt.Result GiveAttackHurtStruct(AttackHurtStruct attackHurtStruct);
    /// <summary>
    /// 获取指定类型的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T GetEntity<T>() where T : IBaseState;


}


