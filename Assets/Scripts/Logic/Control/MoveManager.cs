using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移动管理器
/// 负责角色的移动
/// </summary>
public class MoveManager : IInput
{
    /// <summary>
    /// 移动管理器私有静态对象
    /// </summary>
    private static MoveManager instance;
    /// <summary>
    /// 移动管理器的单例对象
    /// </summary>
    public static MoveManager Instance
    {
        get
        {
            if (instance == null) instance = new MoveManager();
            return instance;
        }
    }
    /// <summary>
    /// 移动管理器的私有构造函数
    /// </summary>
    private MoveManager() { }

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
