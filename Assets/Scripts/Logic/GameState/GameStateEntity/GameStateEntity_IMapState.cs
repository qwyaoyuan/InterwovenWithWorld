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

    #endregion

}