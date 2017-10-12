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

    public void KeyDown(int key)
    {
        //从按键对应数据对象中获取该键位对应的按键数组（选取条件为选择技能）
        KeyContactStruct[] keyContactStructs =
          KeyContactData.Instance.GetKeyContactStruct(key, temp => temp.keyContactType == EnumKeyContactType.Skill);
        if (keyContactStructs.Length > 0)
        {
            //只处理其中的一个
            KeyContactStruct keyContactStruct = keyContactStructs[0];
            
        }
    }

    public void KeyPress(int key)
    {
        //从按键对应数据对象中获取该键位对应的按键数组（选取条件为选择技能）
        KeyContactStruct[] keyContactStructs =
         KeyContactData.Instance.GetKeyContactStruct(key, temp => temp.keyContactType == EnumKeyContactType.Skill);
        if (keyContactStructs.Length > 0)
        {
            //只处理其中的一个
            KeyContactStruct keyContactStruct = keyContactStructs[0];
            
        }
    }

    public void KeyUp(int key)
    {
        //从按键对应数据对象中获取该键位对应的按键数组（选取条件为选择技能）
        KeyContactStruct[] keyContactStructs =
           KeyContactData.Instance.GetKeyContactStruct(key, temp => temp.keyContactType == EnumKeyContactType.Skill);
        if (keyContactStructs.Length > 0)
        {
            //只处理其中的一个
            KeyContactStruct keyContactStruct = keyContactStructs[0];
            //通过对象中的技能id在技能结构对象中获取技能结构对象
            SkillBaseStruct skillBaseStruct = SkillStructData.Instance.GetSkillBaseStruct(keyContactStruct.id);

        }
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
