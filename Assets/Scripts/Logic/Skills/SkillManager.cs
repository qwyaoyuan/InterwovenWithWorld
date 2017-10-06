using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能管理器
/// 负责角色的技能释放
/// </summary>
public class SkillManager : IInput
{
    /// <summary>
    /// 技能管理器私有静态对象
    /// </summary>
    private static SkillManager instance;
    /// <summary>
    /// 技能管理器的单例对象
    /// </summary>
    public static SkillManager Instance
    {
        get
        {
            if (instance == null) instance = new SkillManager();
            return instance;
        }
    }
    /// <summary>
    /// 技能管理器的私有构造函数
    /// </summary>
    private SkillManager() { }

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
