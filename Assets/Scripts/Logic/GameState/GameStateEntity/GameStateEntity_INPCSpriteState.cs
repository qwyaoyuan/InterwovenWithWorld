using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 实现了INPCSpriteState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState : INPCSpriteState
{
    /// <summary>
    /// NPCID对应NPC图片的字典
    /// </summary>
    private Dictionary<int, Sprite> npcToSpriteDic;

    /// <summary>
    /// 开始时调用
    /// </summary>
    partial void Start_INPCSpriteState()
    {
        npcToSpriteDic = new Dictionary<int, Sprite>();
    }

    /// <summary>
    /// 通过NPCID获取该NPC的头像
    /// </summary>
    /// <param name="npcID"></param>
    /// <returns></returns>
    public Sprite GetSprite(int npcID)
    {
        if (npcToSpriteDic == null)
            return null;
        if (npcToSpriteDic.ContainsKey(npcID))
            return npcToSpriteDic[npcID];
        else
        {
            npcToSpriteDic.Add(npcID, null);
            return null;
        }
    }

    /// <summary>
    /// 通过NPCID设置该NPC的头像
    /// </summary>
    /// <param name="npcID"></param>
    /// <param name="sprite"></param>
    public void SetSprite(int npcID, Sprite sprite)
    {
        if (npcToSpriteDic.ContainsKey(npcID))
            npcToSpriteDic[npcID] = sprite;
        else npcToSpriteDic.Add(npcID, sprite);

    }
}

