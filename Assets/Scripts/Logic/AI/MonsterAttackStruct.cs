using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// 怪物的攻击结构
/// 用于存储本次攻击的检测开始时间与结束时间(注意如果动画已经结束则不管是否结束都会结束)
/// 用于存储本次攻击的攻击倍率
/// </summary>
[Serializable]
public class MonsterAttackStruct
{
    /// <summary>
    /// 怪物本次攻击的检测片段
    /// </summary>
    [SerializeField]
    private MonsterAttackFrame[] checkFrames;

    public int Count
    {
        get { return checkFrames.Length; }
    }

    public MonsterAttackFrame this[int index]
    {
        get
        {
            if (index >= 0 && index < checkFrames.Length)
                return checkFrames[index];
            return null;
        }
    }

    /// <summary>
    /// 通过tag检测第一个检测片段
    /// </summary>
    /// <param name="actionAnimationTag">攻击所用动画</param>
    /// <returns></returns>
    public MonsterAttackFrame Search(string actionAnimationTag)
    {
        return checkFrames.FirstOrDefault(temp => temp.ActionAnimationTag == actionAnimationTag);
    }

    [Serializable]
    public class MonsterAttackFrame
    {
        /// <summary>
        /// 攻击所用的动画(与动画进入离开检测脚本的Tag一直)
        /// </summary>
        [Tooltip("攻击所用的动画(与动画进入离开检测脚本的Tag一直)")] public string ActionAnimationTag;
        /// <summary>
        /// 攻击检测的开始时间
        /// </summary>
        [Tooltip("攻击检测的开始时间")] public float startTime;
        /// <summary>
        /// 攻击检测的结束时间
        /// </summary>
        [Tooltip("攻击检测的结束时间")] public float endTime;
        /// <summary>
        /// 伤害倍率
        /// </summary>
        [Tooltip("伤害倍率")] public float magnification;
        /// <summary>
        /// 怪物攻击检测脚本
        /// </summary>
        [Tooltip("怪物攻击检测脚本")] public MonsterAttackCheck monsterAttackCheck;

    }

}

