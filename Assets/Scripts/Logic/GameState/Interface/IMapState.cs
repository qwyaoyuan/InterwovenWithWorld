using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图的状态 
/// </summary>
public interface IMapState : IBaseState
{
    /// <summary>
    /// 需要在地图上标记的任务id
    /// </summary>
    int MarkTaskID { get; set; }

}
