using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按键与技能或物品之间关系的结构
/// </summary>
public struct KeyContactStruct 
{
    /// <summary>
    /// 按下该键的类型
    /// 如果是功能按键则按功能键提取信息
    /// 如果是技能则按技能释放
    /// 如果是道具则按道具选择使用
    /// </summary>
    public EnumKeyContactType keyContactType;

    /// <summary>
    /// 对应的id
    /// </summary>
    public int id;

    /// <summary>
    /// 按键id
    /// </summary>
    public int key;
}
