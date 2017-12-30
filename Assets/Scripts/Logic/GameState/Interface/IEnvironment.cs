using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 环境接口
/// </summary>
public interface IEnvironment : IBaseState
{
    /// <summary>
    /// 当前的环境类型 
    /// </summary>
    EnumEnvironmentType TerrainEnvironmentType { get; set; }
}

/// <summary>
/// 环境类型枚举
/// </summary>
public enum EnumEnvironmentType
{
    /// <summary>
    /// 平原 雷
    /// </summary>
    Plain,
    /// <summary>
    /// 沼泽 水
    /// </summary>
    Swamp,
    /// <summary>
    /// 沙漠 土
    /// </summary>
    Desert,
    /// <summary>
    /// 森林 风
    /// </summary>
    Forest,
    /// <summary>
    /// 火山 火
    /// </summary>
    Volcano
}
