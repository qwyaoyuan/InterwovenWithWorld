using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 交互管理器
/// 负责角色与NPC的交互
/// </summary>
public class InteractiveManager : IInput
{
    /// <summary>
    /// 交互管理器私有静态对象
    /// </summary>
    private static InteractiveManager instance;
    /// <summary>
    /// 交互管理器的单例对象
    /// </summary>
    public static InteractiveManager Instance
    {
        get
        {
            if (instance == null) instance = new InteractiveManager();
            return instance;
        }
    }
    /// <summary>
    /// 交互管理器的私有构造函数
    /// </summary>
    private InteractiveManager() { }

    public void KeyPress(int key)
    {
        throw new NotImplementedException();
    }

    public void KeyUp(int key)
    {
        throw new NotImplementedException();
    }

    public void Move(Vector2 forward)
    {
        throw new NotImplementedException();
    }

    public void View(Vector2 view)
    {
        throw new NotImplementedException();
    }
}
