using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图的状态 
/// </summary>
public interface IMapState : IBaseState
{
    /// <summary>
    /// 需要在地图上标记的任务id(这个指定是任务追踪)
    /// </summary>
    int MarkTaskID { get; set; }

    /// <summary>
    /// 是否显示功能NPC(功能NPC指的是合成 打造 商人等特殊功能的NPC)
    /// </summary>
    bool ShowFunctionalNPC { get; set; }

    /// <summary>
    /// 是否显示存在任务的NPC(待接取 正在执行 条件达成)
    /// </summary>
    bool ShowTaskNPC { get; set; }

    /// <summary>
    /// 地图的背景图
    /// </summary>
    Sprite MapBackSprite { get; set; }
    /// <summary>
    /// 地图的遮罩图
    /// </summary>
    Sprite MaskMapSprite { get; set; }
    /// <summary>
    /// 地图所在场景的矩形区域
    /// </summary>
    Rect MapRectAtScene { get; set; }
}
