using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置管理器
/// 负责设置游戏以及界面菜单
/// </summary>
public class SettingManager : IInput
{
    /// <summary>
    /// 设置管理器私有静态对象
    /// </summary>
    private static SettingManager instance;
    /// <summary>
    /// 设置管理器的单例对象
    /// </summary>
    public static SettingManager Instance
    {
        get
        {
            if (instance == null) instance = new SettingManager();
            return instance;
        }
    }
    /// <summary>
    /// 设置管理器的私有构造函数
    /// </summary>
    private SettingManager() { }

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

    public void KeyDown(int key)
    {
        throw new NotImplementedException();
    }
}
