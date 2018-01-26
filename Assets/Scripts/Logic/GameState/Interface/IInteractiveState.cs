using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 交互状态接口
/// 主要用于保存对应的UI,以及对应的反应
/// </summary>
public interface IInteractiveState : IBaseState
{
    /// <summary>
    /// 主线的中间过度展示面板
    /// </summary>
    GameObject InterludeObj { get; set; }
    /// <summary>
    /// 支线的选择展示面板
    /// </summary>
    GameObject QueryObj { get; set; }
    /// <summary>
    /// 对话展示的面板
    /// </summary>
    GameObject TalkShowObj { get; set; }
    /// <summary>
    /// 商店功能的面板
    /// </summary>
    GameObject ShopShowObj { get; set; }
    /// <summary>
    /// 功能面板
    /// </summary>
    GameObject ActionObj { get; set; }
    /// <summary>
    /// 点击的NPC
    /// 在设置时判断使用那个UI来展示
    /// </summary>
    int ClickInteractiveNPCID { get; set; }
    /// <summary>
    /// 合成面板
    /// </summary>
    GameObject SynthesisObj { get; set; }
    /// <summary>
    /// 点击的采集点ID
    /// </summary>
    int ClickInteractiveStuffID { get; set; }
}
