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

    /// <summary>
    /// 将EnumInputType类型的数值转换为KeyType类型枚举
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private KeyType? ChangeKeyEnum(int key)
    {
        EnumInputType enumInputType = (EnumInputType)key;
        switch (enumInputType)
        {
            case EnumInputType.Up:
                return KeyType.UP;
            case EnumInputType.Left:
                return KeyType.LEFT;
            case EnumInputType.Right:
                return KeyType.RIGHT;
            case EnumInputType.Down:
                return KeyType.DOWN;
            case EnumInputType.A:
                return KeyType.A;
            case EnumInputType.B:
                return KeyType.B;
            case EnumInputType.X:
                return KeyType.X;
            case EnumInputType.Y:
                return KeyType.Y;
            case EnumInputType.L3:
                return KeyType.LEFT_ROCKER;
            case EnumInputType.R3:
                return KeyType.RIGHT_ROCKER;
            case EnumInputType.LB:
                return KeyType.L1;
            case EnumInputType.RB:
                return KeyType.R1;
            case EnumInputType.LT:
                return KeyType.L2;
            case EnumInputType.RT:
                return KeyType.R2;
            case EnumInputType.Start:
                return KeyType.START;
            case EnumInputType.Back:
                return KeyType.Back;
            default:
                return null;
        }
    }

    public void KeyDown(int key)
    {
      
    }

    public void KeyPress(int key)
    {
        KeyType? keyType = ChangeKeyEnum(key);
        if (keyType != null && KeyPressHandle != null)
            KeyPressHandle(keyType.Value, Vector2.zero);
    }

    public void KeyUp(int key)
    {
        KeyType? keyType = ChangeKeyEnum(key);
        if (keyType != null && KeyUpHandle != null)
            KeyUpHandle(keyType.Value, Vector2.zero);
    }

    public void Move(Vector2 forward)
    {
        if (KeyPressHandle != null)
            KeyPressHandle(KeyType.LEFT_ROCKER, forward);
    }

    public void View(Vector2 view)
    {
        if (KeyPressHandle != null)
            KeyPressHandle(KeyType.RIGHT_ROCKER, view);
    }

    /// <summary>
    /// UI上的按键类型
    /// </summary>
    public enum KeyType
    {
        None,
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
        Back,
        START
    }
}
