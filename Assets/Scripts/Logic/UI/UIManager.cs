using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI管理器（通过输入按键驱动）
/// </summary>
public class UIManager : IInput
{
    /// <summary>
    /// UI管理器私有静态对象
    /// </summary>
    private static UIManager instance;
    /// <summary>
    /// UI管理器的单例对象
    /// </summary>
    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = new UIManager();
            return instance;
        }
    }
    /// <summary>
    /// UI管理器的私有构造函数
    /// </summary>
    private UIManager() { }

    public event Action<UIManager.KeyType, Vector2> KeyPressHandle;

    public event Action<UIManager.KeyType, Vector2> KeyUpHandle;

    public void KeyDown(int key)
    {
        
    }

    public void KeyPress(int key)
    {
        
    }

    public void KeyUp(int key)
    {
       
    }

    public void Move(Vector2 forward)
    {
        
    }

    public void View(Vector2 view)
    {
       
    }

    /// <summary>
    /// UI上的按键类型
    /// </summary>
    public enum KeyType
    {
        A,
        B,
        X,
        Y,
        R1,
        R2,
        R3,
        L1,
        L2,
        L3,
        LEFT,
        RIGHT,
        UP,
        DOWN,
        LEFT_ROCKER,
        RIGHT_ROCKER,
        OPTION,
        START
    }
}
