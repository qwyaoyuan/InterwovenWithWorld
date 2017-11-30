using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 输入管理器
/// </summary>
public class InputControl : IEntrance
{
    /// <summary>
    /// 输入对象集合
    /// </summary>
    List<IInput> inputList;

    public void Start()
    {
        inputList = new List<IInput>();
        inputList.Add(UIManager.Instance);
        inputList.Add(SettingManager.Instance);
        inputList.Add(MoveManager.Instance);
        inputList.Add(InteractiveManager.Instance);
        inputList.Add(SkillManager.Instance);
    }

    public void Update()
    {

    }

    public void OnDestroy() { }
    
}
