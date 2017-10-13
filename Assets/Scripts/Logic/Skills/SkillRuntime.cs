using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能运行时数据
/// </summary>
public class SkillRuntime
{
    /// <summary>
    /// 技能运行时数据静态私有对象
    /// </summary>
    private static SkillRuntime instance;
    public static SkillRuntime Instance
    {
        get
        {
            if (instance == null) instance = new SkillRuntime();
            return instance;
        }
    }
    public const int SkillLevelMax = 4;
    private SkillRuntime()
    {
       
    }


}
