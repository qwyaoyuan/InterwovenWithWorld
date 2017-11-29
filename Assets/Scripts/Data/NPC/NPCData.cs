using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC数据
/// </summary>
public class NPCData : ILoadable<NPCData>
{
    public void Load()
    {
        
    }

    /// <summary>
    /// 通过npc id获取npc信息
    /// </summary>
    /// <param name="npcID">npc id</param>
    /// <returns></returns>
    public NPCDataInfo GetNPCDataInfo(int npcID)
    {
        throw new Exception();
    }

    /// <summary>
    /// 通过场景名获取该场景的所有NPC信息
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public NPCDataInfo[] GetNPCDataInfos(string sceneName)
    {
        throw new Exception();
    }
}

/// <summary>
/// 单个NPC信息
/// </summary>
public class NPCDataInfo
{
    /// <summary>
    /// npc的类型
    /// </summary>
    public EnumNPCType NPCType;
    /// <summary>
    /// npc的id
    /// </summary>
    public int NPCID;
    /// <summary>
    /// npc的名字
    /// </summary>
    public string NPCName;
    /// <summary>
    /// npc的位置
    /// </summary>
    public Vector3 NPCLocation;
    /// <summary>
    /// npc的角度
    /// </summary>
    public Vector3 NPCAngle;
    /// <summary>
    /// npc在地图上的图标
    /// </summary>
    public Sprite NPCSprite;
    /// <summary>
    /// npc所在场景,如果是路牌则表示跳转到指定场景
    /// </summary>
    public string SceneName;
}

/// <summary>
/// npc类型
/// </summary>
public enum EnumNPCType
{
    /// <summary>
    /// 普通
    /// </summary>
    Normal,
    /// <summary>
    /// 商人 
    /// </summary>
    Businessman,
    /// <summary>
    /// 合成人
    /// </summary>
    Synthesiser,
    /// <summary>
    /// 打造人
    /// </summary>
    Forge,
    /// <summary>
    /// 佣兵提交
    /// </summary>
    Mercenarier,
    /// <summary>
    /// 路牌
    /// </summary>
    Street
}