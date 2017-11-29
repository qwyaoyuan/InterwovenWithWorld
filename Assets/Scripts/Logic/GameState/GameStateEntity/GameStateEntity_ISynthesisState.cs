using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现了ISynthesisState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState : ISynthesisState
{
    /// <summary>
    /// 合成类型 
    /// </summary>
    EnumSynthesisType _SynthesisType;

    public EnumSynthesisType SynthesisType
    {
        get { return _SynthesisType; }
        set
        {
            _SynthesisType = value;
            if (SynthesisObj != null)
            {
                SynthesisObj.SetActive(true);
            }
        }
    }

    public int SynthesisGoods(SynthesisDataStruct synthesisDataStruct)
    {
        throw new Exception("未实现合成");
    }
}
