using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按键与技能或物品之间的对应关系
/// </summary>
public class KeyContactData
{

    /// <summary>
    /// 按键与技能物品之间对应关系的对象字典（key表示按键组合后的数字）
    /// </summary>
    private Dictionary<int, KeyContactStruct> keyContactStructs;

    /*与EnumInputType的L1 R1 L2 R2对应的数值*/
    private const int L1 = 16;
    private const int R1 = 32;
    private const int L2 = 64;
    private const int R2 = 128;

    /// <summary>
    /// 按键与技能
    /// </summary>
    public KeyContactData()
    {
        keyContactStructs = new Dictionary<int, KeyContactStruct>();
    }

    /// <summary>
    /// 设置对应按键信息
    /// </summary>
    /// <param name="key">组合后的按键数字，注意不可以复合组合</param>
    /// <param name="target">对应的数据对象</param>
    public void SetKeyContactStruct(int key, KeyContactStruct target)
    {
        if (keyContactStructs.ContainsKey(key))
            keyContactStructs[key] = target;
        else keyContactStructs.Add(key, target);
    }

    /// <summary>
    /// 通过按键获取对应按键信息的数组
    /// </summary>
    /// <param name="key">组合后的按键数字</param>
    /// <param name="selecter">选择器</param>
    /// <returns></returns>
    public KeyContactStruct[] GetKeyContactStruct(int key, Func<KeyContactStruct, bool> selecter = null)
    {
        List<KeyContactStruct> tempKeyContactStructs = new List<KeyContactStruct>();
        //表示本功能键 除了L1 L2 R1 R2 左右摇杆外的其他键
        int baseFunctionKey = key % 16;
        //附加功能键 L1 L2 R1 R2
        int addFunctionKey = key - baseFunctionKey;
        bool l1 = ((addFunctionKey / L1) % 2) == 1;//是否按下了L1键
        bool r1 = ((addFunctionKey / R1) % 2) == 1;//是否按下了R1键
        bool l2 = ((addFunctionKey / L2) % 2) == 1;//是否按下了L2键
        bool r2 = ((addFunctionKey / R2) % 2) == 1;//是否按下了R2键
        Func<int, KeyContactStruct> GetSpecificTarget = (_key) =>
        {
            if (keyContactStructs.ContainsKey(_key))
                return keyContactStructs[key];
            else
            {
                KeyContactStruct tempKeyContactStruct;
                tempKeyContactStruct.keyContactType = EnumKeyContactType.None;
                tempKeyContactStruct.id = -1;
                tempKeyContactStruct.key = -1;
                return tempKeyContactStruct;
            }
        };
        if (l1)
            tempKeyContactStructs.Add(GetSpecificTarget(L1 + baseFunctionKey));
        if (r1)
            tempKeyContactStructs.Add(GetSpecificTarget(R1 + baseFunctionKey));
        if (l2)
            tempKeyContactStructs.Add(GetSpecificTarget(L2 + baseFunctionKey));
        if (r2)
            tempKeyContactStructs.Add(GetSpecificTarget(R2 + baseFunctionKey));
        if (!l1 && !l2 && !r1 && !r2)
            tempKeyContactStructs.Add(GetSpecificTarget(baseFunctionKey));
        tempKeyContactStructs.RemoveAll(temp => temp.keyContactType == EnumKeyContactType.None);
        if (selecter != null)
            tempKeyContactStructs.RemoveAll(temp => !selecter(temp));
        return tempKeyContactStructs.ToArray();
    }
}
