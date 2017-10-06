using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 输入管理器
/// </summary>
public class InputControl : MonoBehaviour
{
    /// <summary>
    /// 输入对象集合
    /// </summary>
    List<IInput> inputList;

    void Start()
    {
        inputList.Add(SettingManager.Instance);
        inputList.Add(MoveManager.Instance);
        inputList.Add(InteractiveManager.Instance);
        inputList.Add(SkillManager.Instance);
    }

    void Update()
    {

    }

    
}
