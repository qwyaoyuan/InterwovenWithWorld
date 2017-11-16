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
        //KeyContactStruct[] keyContactStructs =
        //  KeyContactData.Instance.GetKeyContactStruct(key, temp => temp.keyContactType == EnumKeyContactType.Skill);
        //if (keyContactStructs.Length > 0 && SkillDealHandle.Instance.CanDealSkill)
        //{
        //    //只处理其中的一个
        //    KeyContactStruct keyContactStruct = keyContactStructs[0];
        //    //释放魔法
        //    if (keyContactStruct.id == (int)EnumSkillType.MagicRelease && SkillRuntime.Instance.GetSkills().Length > 0)
        //    {
        //        //给魔法技能处理模块
        //        SkillDealHandle.Instance.BeginCombineSkill(SkillRuntime.Instance.GetSkills());
        //        SkillRuntime.Instance.SavingMagicPowerTime = 0;
        //    }
        //}
    }

    public void KeyPress(int key)
    {
        //从按键对应数据对象中获取该键位对应的按键数组（选取条件为选择技能）
        //KeyContactStruct[] keyContactStructs =
        // KeyContactData.Instance.GetKeyContactStruct(key, temp => temp.keyContactType == EnumKeyContactType.Skill);
        //if (keyContactStructs.Length > 0)
        //{
        //    //只处理其中的一个
        //    KeyContactStruct keyContactStruct = keyContactStructs[0];
        //    //释放魔法  蓄力
        //    if (keyContactStruct.id == (int)EnumSkillType.MagicRelease && SkillRuntime.Instance.GetSkills().Length > 0)
        //    {
        //        SkillRuntime.Instance.SavingMagicPowerTime += Time.deltaTime;
        //    }
        //}
    }

    public void KeyUp(int key)
    {
        //从按键对应数据对象中获取该键位对应的按键数组（选取条件为选择技能）
        //KeyContactStruct[] keyContactStructs =
        //   KeyContactData.Instance.GetKeyContactStruct(key, temp => temp.keyContactType == EnumKeyContactType.Skill);
        //if (keyContactStructs.Length > 0)
        //{
        //    //只处理其中的一个
        //    KeyContactStruct keyContactStruct = keyContactStructs[0];
        //    if (keyContactStruct.id > (int)EnumSkillType.EndMagic && keyContactStruct.id < (int)EnumSkillType.EndMagic)
        //    {
        //        bool addResult = SkillRuntime.Instance.SetSkill(keyContactStruct.id);
        //        if (!addResult)
        //            Debug.Log("无法使用的组合");
        //    }
        //    //释放魔法  如果读条完成则可以释放
        //    if (keyContactStruct.id == (int)EnumSkillType.MagicRelease)
        //    {
        //        //通知魔法技能处理模块可以释放魔法了
        //        SkillDealHandle.Instance.EndCombineSkill(SkillRuntime.Instance.SavingMagicPowerTime);
        //    }
        //}
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
