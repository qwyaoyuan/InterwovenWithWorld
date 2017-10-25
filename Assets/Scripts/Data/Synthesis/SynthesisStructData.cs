using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 合成结构数据
/// </summary>
public class SynthesisStructData
{
    /// <summary>
    /// 合成结构数据的私有静态对象
    /// </summary>
    private static SynthesisStructData instance;
    /// <summary>
    /// 合成结构数据的单例对象 
    /// </summary>
    public static SynthesisStructData Instance
    {
        get
        {
            if (instance == null) instance = new SynthesisStructData();
            return instance;
        }
    }
    /// <summary>
    ///  合成结构数据的私有构造函数
    /// </summary>
    private SynthesisStructData()
    {
        synthesisDataAnalysis = new SynthesisDataAnalysis();
        ReadSynthesisStructData(true);
    }

    /// <summary>
    /// 合成文件解析
    /// </summary>
    SynthesisDataAnalysis synthesisDataAnalysis;

    /// <summary>
    /// 从文件中读取合成结构数据
    /// </summary>
    /// <param name="must">是否必须读取</param>
    public void ReadSynthesisStructData(bool must = false)
    {
        if (synthesisDataAnalysis == null || must)
        {
            TextAsset synthesisTextAsset = Resources.Load<TextAsset>("Data/Synthesis/Synthesis");
            if (synthesisTextAsset != null)
            {
                synthesisDataAnalysis.ReadData(synthesisTextAsset.text);
            }
        }
    }

    /// <summary>
    /// 通过id查找指定的合成数据
    /// </summary>
    /// <param name="id">合成数据id</param>
    /// <returns></returns>
    public SynthesisDataStruct SearchSynthesisDataByID(int id)
    {
        return synthesisDataAnalysis.GetDataByID(id);
    }

    /// <summary>
    /// 使用指定的条件检索合成数据集合
    /// </summary>
    /// <param name="selector">检索条件</param>
    /// <returns></returns>
    public SynthesisDataStruct[] SearchSynthesisData(Func<SynthesisDataStruct, bool> selector = null)
    {
        return synthesisDataAnalysis.GetAllData().Where(temp => selector == null ? true : selector(temp)).ToArray();
    }
}
