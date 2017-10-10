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
            //通过对象中的技能id在技能结构对象中获取技能结构对象
            SkillBaseStruct skillBaseStruct = SkillStructData.Instance.GetSkillBaseStruct(keyContactStruct.id);
            switch (skillBaseStruct.releaseMode)
            {
                case EnumReleaseMode.Magic_Action:
                    //魔法指令技能
                    SkillMagicBaseStruct skillMagicBaseStruct = skillBaseStruct as SkillMagicBaseStruct;
                    //如果技能是释放魔法
                    if (skillMagicBaseStruct.skillType == EnumSkillType.MagicRelease)
                    {
                        //通过查询条件从技能结构数据对象中获取技能结构对象
                        SkillBaseStruct selectSkillBaseStruct = SkillStructData.Instance.GetSkillBaseStruct(temp =>
                        {
                            //该技能是魔法技能且技能是组合技能
                            SkillMagicBaseStruct _skillMagicBaseStruct = temp as SkillMagicBaseStruct;
                            if (_skillMagicBaseStruct != null && _skillMagicBaseStruct.combinSkillTypes.Length > 0)
                            {

                                int min = SkillMagicBaseStruct.GetCombinSkillTypesLength( _skillMagicBaseStruct);
                                int nullIndex = SkillRuntime.Instance.GetNullIndexByCombineSkills();
                                //检测该技能的组合技能元素和当前的组合技能长度是否一致
                                if (min == nullIndex)
                                {
                                    //每一个组合技能元素都必须和当前的组合技能元素一致
                                    for (int i = 0; i < min; i++)
                                    {
                                        if (SkillRuntime.Instance.combineSkills[i].id != (int)_skillMagicBaseStruct.combinSkillTypes[i])
                                        {
                                            return false;
                                        }
                                    }
                                    return true;
                                }
                                return false;
                            }
                            return false;
                        });
                        //如果查询出的技能结构对象不为空
                        if (selectSkillBaseStruct != null)
                        {
                            //清空组合技能栏，将组合后的技能放入组合技能位
                            SkillRuntime.Instance.combineSkill_End = (SkillBaseStruct)skillMagicBaseStruct.Clone();
                            SkillRuntime.Instance.ClearCombineSkills();
                            SkillMagicBaseStruct skillMagicBseStruct = SkillRuntime.Instance.combineSkill_End as SkillMagicBaseStruct;
                            if (skillMagicBseStruct != null)
                                skillMagicBseStruct.powerRate = 0;
                            //交给技能处理模块处理
                        }
                    }
                    break;
            }

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
            //通过对象中的技能id在技能结构对象中获取技能结构对象
            SkillBaseStruct skillBaseStruct = SkillStructData.Instance.GetSkillBaseStruct(keyContactStruct.id);
            switch (skillBaseStruct.releaseMode)
            {
                case EnumReleaseMode.Magic_Action:
                    //魔法指令技能
                    if (SkillRuntime.Instance.combineSkill_End != null)
                    {
                        if (SkillRuntime.Instance.combineSkill_End != null)
                        {
                            //如果这是魔法技能
                            SkillMagicBaseStruct skillMagicBaseStruct = SkillRuntime.Instance.combineSkill_End as SkillMagicBaseStruct;
                            if (skillMagicBaseStruct != null&& skillMagicBaseStruct.combinSkillTypes.Length>0)
                            {
                                //提高技能消魔
                                skillMagicBaseStruct.powerRate += Time.deltaTime;
                            }
                        }
                    }
                    break;
            }
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
            switch (skillBaseStruct.releaseMode)
            {
                case EnumReleaseMode.Magic_Bullet:
                case EnumReleaseMode.Magic_Vibrate:
                case EnumReleaseMode.Magic_Barrier:
                case EnumReleaseMode.Magic_Point:
                case EnumReleaseMode.Magic_Pulse:
                case EnumReleaseMode.Magic_Buff:
                case EnumReleaseMode.Magic_Call:
                    //魔法技能
                    SkillMagicBaseStruct skillMagicBaseStruct = skillBaseStruct as SkillMagicBaseStruct;
                    //如果技能是常规直接释放技能
                    if (skillMagicBaseStruct.skillType > EnumSkillType.MagicNormalStart &&
                        skillMagicBaseStruct.skillType < EnumSkillType.MagicNormalEnd)
                    {
                        //交给技能处理模块处理
                    }
                    //如过技能是组合技能后的技能
                    else if (skillMagicBaseStruct.skillType > EnumSkillType.MagicCombinedStart &&
                        skillMagicBaseStruct.skillType < EnumSkillType.MagicCombinedEnd)
                    {
                        //交给技能处理模块处理
                    }
                    //如果技能是组合技能的组合元素
                    else if (skillMagicBaseStruct.skillType > EnumSkillType.MagicCombinedLevel1Start &&
                        skillMagicBaseStruct.skillType < EnumSkillType.MagicCombinedLevel4End)
                    {
                        //获取当前可以放入的组合技能栏下标
                        int nullIndex = SkillRuntime.Instance.GetNullIndexByCombineSkills();
                        bool skillLevelRight = false;//该技能的阶段是否可以放入到组合技能栏里
                        switch (nullIndex)
                        {
                            case 0:
                                skillLevelRight = skillMagicBaseStruct.skillType > EnumSkillType.MagicCombinedLevel1Start &&
                                    skillMagicBaseStruct.skillType < EnumSkillType.MagicCombinedLevel1End;
                                break;
                            case 1:
                                skillLevelRight = skillMagicBaseStruct.skillType > EnumSkillType.MagicCombinedLevel2Start &&
                                    skillMagicBaseStruct.skillType < EnumSkillType.MagicCombinedLevel2End;
                                break;
                            case 2:
                                skillLevelRight = skillMagicBaseStruct.skillType > EnumSkillType.MagicCombinedLevel3Start &&
                                    skillMagicBaseStruct.skillType < EnumSkillType.MagicCombinedLevel3End;
                                break;
                            case 3:
                                skillLevelRight = skillMagicBaseStruct.skillType > EnumSkillType.MagicCombinedLevel4Start &&
                                    skillMagicBaseStruct.skillType < EnumSkillType.MagicCombinedLevel4End;
                                break;
                        }
                        if (skillLevelRight)
                        {
                            //通过查询条件从技能结构数据对象中获取技能结构对象
                            SkillBaseStruct selectSkillBaseStruct = SkillStructData.Instance.GetSkillBaseStruct(temp =>
                            {
                                //该技能是魔法技能且技能是组合技能
                                SkillMagicBaseStruct _skillMagicBaseStruct = temp as SkillMagicBaseStruct;
                                if (_skillMagicBaseStruct != null && _skillMagicBaseStruct.combinSkillTypes.Length > 0)
                                {
                                    int min = SkillMagicBaseStruct.GetCombinSkillTypesLength(_skillMagicBaseStruct);
                                    min = SkillRuntime.SkillLevelMax < min ? SkillRuntime.SkillLevelMax : min;
                                    //便利最小长度的元素，如果有不同的元素则返回false，如果一样则范围true
                                    for (int i = 0; i < min; i++)
                                    {
                                        if (SkillRuntime.Instance.combineSkills[i] != null && SkillRuntime.Instance.combineSkills[i].id != (int)_skillMagicBaseStruct.combinSkillTypes[i])
                                        {
                                            return false;
                                        }
                                    }
                                    return true;
                                }
                                return false;
                            });
                            if (selectSkillBaseStruct != null)
                                SkillRuntime.Instance.SetToCombineSkill(skillMagicBaseStruct);
                        }
                    }
                    break;
                case EnumReleaseMode.Magic_Action:
                    //魔法指令技能
                    SkillMagicBaseStruct skillMagicBaseStruct_Action = skillBaseStruct as SkillMagicBaseStruct;
                    //如果技能是释放魔法
                    if (skillMagicBaseStruct_Action.skillType == EnumSkillType.MagicRelease)
                    {
                        SkillRuntime.Instance.combineSkill_End = null;
                    }
                    break;
                case EnumReleaseMode.Physics_Buff:
                case EnumReleaseMode.Physics_Attack:
                    break;
            }
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
