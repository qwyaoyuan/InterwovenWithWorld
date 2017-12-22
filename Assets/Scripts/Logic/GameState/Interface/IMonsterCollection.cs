using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 怪物集合接口,用于处理怪物的生成,怪物的销毁,以及怪物的检索
/// </summary>
public interface IMonsterCollection : IBaseState
{
    /// <summary>
    /// 获取怪物,需要指定查询的中心对象(一般是玩家),查询的角度(一般是玩家的正方向),查询的距离
    /// </summary>
    /// <param name="centerObj">查询的中心对象(一般是玩家)</param>
    /// <param name="angle">查询的角度(一般是玩家的正方向)</param>
    /// <param name="distance">查询的距离</param>
    /// <returns></returns>
    GameObject[] GetMonsters(GameObject centerObj, float angle, float distance);
}

