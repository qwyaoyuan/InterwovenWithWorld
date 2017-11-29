using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 合成状态
/// </summary>
public interface ISynthesisState : IBaseState
{
    /// <summary>
    /// 合成状态
    /// </summary>
    EnumSynthesisType SynthesisType { get; set; }
    /// <summary>
    /// 合成物品
    /// </summary>
    /// <param name="synthesisDataStruct">合成物品的配方</param>
    /// <returns>是否合成成功,如果有物品id则合成成功,如果是-1表示没有合成成功</returns>
    int SynthesisGoods(SynthesisDataStruct synthesisDataStruct);
}
