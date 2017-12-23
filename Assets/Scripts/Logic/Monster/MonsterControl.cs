using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 怪物控制器
/// </summary>
public class MonsterControl : MonoBehaviour
{
    /// <summary>
    /// 相同组的对象(包括自己)
    /// </summary>
    public List<GameObject> SameGroupObjList;
    /// <summary>
    /// 怪物的数据对象
    /// </summary>
    public MonsterDataInfo monsterDataInfo;
}
