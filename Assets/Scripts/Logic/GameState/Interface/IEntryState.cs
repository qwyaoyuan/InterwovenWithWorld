using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 词条接口
/// </summary>
public interface IEntryState : IBaseState
{
    /// <summary>
    /// 检测词条是否完成--任务完成
    /// </summary>
    /// <param name="taskID">任务ID</param>
    void CheckEntryTaskOver(int taskID);

    /// <summary>
    /// 检测词条是否完成--怪物类型
    /// </summary>
    /// <param name="monsterType">怪物类型</param>
    void CheckEntryKillMonster(EnumMonsterType monsterType);

    /// <summary>
    /// 检测词条是否完成--点击NPC
    /// </summary>
    /// <param name="npcID">NPCID</param>
    void CheckEntryClickNPC(int npcID);
}

