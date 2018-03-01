using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 当前NPC图片状态接口
/// </summary>
public interface INPCSpriteState : IBaseState
{
    /// <summary>
    /// 通过NPCID获取该NPC的头像
    /// </summary>
    /// <param name="npcID"></param>
    /// <returns></returns>
    Sprite GetSprite(int npcID);
    /// <summary>
    /// 通过NPCID设置该NPC的头像
    /// </summary>
    /// <param name="npcID"></param>
    /// <param name="sprite"></param>
    void SetSprite(int npcID, Sprite sprite);
	
}
