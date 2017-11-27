using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现了IMapState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState : IMapState
{
    #region IMapState

    /// <summary>
    /// 需要在地图上标记的任务的id
    /// </summary>
    int _MarkTaskID = -1;
    /// <summary>
    /// 需要在的地图上标记的任务的id
    /// </summary>
    public int MarkTaskID
    {
        get
        {
            return _MarkTaskID;
        }
        set
        {
            int tempMarkTaskID = _MarkTaskID;
            _MarkTaskID = value;
            if (tempMarkTaskID != _MarkTaskID)
                Call<IMapState, int>(temp => temp.MarkTaskID);
        }
    }

    /// <summary>
    /// 是否显示功能NPC(功能NPC指的是合成 打造 商人等特殊功能的NPC)
    /// </summary>
    bool _ShowFunctionalNPC;
    /// <summary>
    /// 是否显示功能NPC(功能NPC指的是合成 打造 商人等特殊功能的NPC)
    /// </summary>
    public bool ShowFunctionalNPC
    {
        get { return _ShowFunctionalNPC; }
        set
        {
            bool tempShowFunctionalNPC = _ShowFunctionalNPC;
            _ShowFunctionalNPC = value;
            if (_ShowFunctionalNPC != tempShowFunctionalNPC)
                Call<IMapState, bool>(temp => temp.ShowFunctionalNPC);
        }
    }

    /// <summary>
    /// 是否显示存在任务的NPC(待接取 正在执行 条件达成)
    /// </summary>
    bool _ShowTaskNPC;
    /// <summary>
    /// 是否显示存在任务的NPC(待接取 正在执行 条件达成)
    /// </summary>
    public bool ShowTaskNPC
    {
        get { return _ShowTaskNPC; }
        set
        {
            bool tempShowTaskNPC = _ShowTaskNPC;
            _ShowTaskNPC = value;
            if (_ShowTaskNPC != tempShowTaskNPC)
                Call<IMapState, bool>(temp => temp.ShowTaskNPC);
        }
    }

    /// <summary>
    /// 地图的背景图
    /// </summary>
    Sprite _MapBackSprite;
    /// <summary>
    /// 地图的背景图
    /// </summary>
    public Sprite MapBackSprite
    {
        get { return _MapBackSprite; }
        set
        {
            Sprite tempMapBackSprite = _MapBackSprite;
            _MapBackSprite = value;
            if (_MapBackSprite != tempMapBackSprite)
                Call<IMapState, Sprite>(temp => temp.MapBackSprite);
        }
    }

    /// <summary>
    /// 地图的遮罩图
    /// </summary>
    Sprite _MaskMapSprite;
    /// <summary>
    /// 地图的遮罩图
    /// </summary>
    public Sprite MaskMapSprite
    {
        get { return _MaskMapSprite; }
        set
        {
            Sprite tempMaskMapSprite = _MaskMapSprite;
            _MaskMapSprite = value;
            if (_MaskMapSprite != tempMaskMapSprite)
                Call<IMapState, Sprite>(temp => temp.MaskMapSprite);
        }
    }

    /// <summary>
    /// 地图所在场景的矩形区域
    /// </summary>
    Rect _MapRectAtScene;
    /// <summary>
    /// 地图所在场景的矩形区域
    /// </summary>
    public Rect MapRectAtScene
    {
        get { return _MapRectAtScene; }
        set
        {
            Rect tempMapRectAtScene = _MapRectAtScene;
            _MapRectAtScene = value;
            if (_MapRectAtScene != tempMapRectAtScene)
                Call<IMapState, Rect>(temp => temp.MapRectAtScene);
        }
    }

    #endregion

}