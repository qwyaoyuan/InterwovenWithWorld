using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 实现了ISkillState接口的GameState类的一个分支实体
/// </summary> 
public partial class GameState
{
    partial void Load_IPlayerState_ISkillState()
    {
        _CombineSkills = new SkillBaseStruct[4];
        isSkillStartHolding = false;
        skillStartHoldingTime = 0;
    }

    /// <summary>
    /// 组合技能数组
    /// </summary>
    SkillBaseStruct[] _CombineSkills;

    /// <summary>
    /// 是否开始蓄力
    /// </summary>
    bool isSkillStartHolding;

    /// <summary>
    /// 技能蓄力时间
    /// </summary>
    float skillStartHoldingTime;

    /// <summary>
    /// 获取或设置组合技能
    /// 设置:如果传入的长度为0或空,则清空,否则如果可以组合则附加,如果无法组合则替换;如果处于蓄力阶段,则无法设置新的技能
    /// 获取:返回当前组合的技能数组
    /// </summary>
    public SkillBaseStruct[] CombineSkills
    {
        get { if (_CombineSkills == null) return new SkillBaseStruct[0]; else return _CombineSkills.Where(temp => temp != null).ToArray(); }
        set
        {
            if (isSkillStartHolding)
                return;
            if (_CombineSkills == null)
                _CombineSkills = new SkillBaseStruct[4];
            if (value == null && value.Count(temp => temp == null) == 0)
            {
                SkillBaseStruct[] tempCombineSkills = _CombineSkills;
                _CombineSkills = new SkillBaseStruct[4];
                if (tempCombineSkills.Count(temp => temp == null) != 4)
                    Call<ISkillState, SkillBaseStruct[]>(temp => temp.CombineSkills);
            }
            else
            {
                SkillBaseStruct firstSkillBaseStruct = _CombineSkills[0];
                SkillBaseStruct secondSkillBaseStruct = _CombineSkills[1];
                SkillBaseStruct thirdSkillBaseStruct = _CombineSkills[2];
                SkillBaseStruct fourthSkillBaseStruct = _CombineSkills[3];
                SkillBaseStruct _firstSkillBaseStruct = value.FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel1Start && temp.skillType < EnumSkillType.MagicCombinedLevel1End);
                SkillBaseStruct _secondSkillBaseStruct = value.FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel2Start && temp.skillType < EnumSkillType.MagicCombinedLevel2End);
                SkillBaseStruct _thirdSkillBaseStruct = value.FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel3Start && temp.skillType < EnumSkillType.MagicCombinedLevel3End);
                SkillBaseStruct _fourthSkillBaseStruct = value.FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel4Start && temp.skillType < EnumSkillType.MagicCombinedLevel4End);
                SkillBaseStruct combineFirstSkillBaseStruct = _firstSkillBaseStruct != null ? _firstSkillBaseStruct : firstSkillBaseStruct;
                SkillBaseStruct combineSecondSkillBaseStruct = _secondSkillBaseStruct != null ? _secondSkillBaseStruct : firstSkillBaseStruct;
                SkillBaseStruct combineThirdSkillBaseStruct = _thirdSkillBaseStruct != null ? _thirdSkillBaseStruct : firstSkillBaseStruct;
                SkillBaseStruct combineFourthSkillBaseStruct = _fourthSkillBaseStruct != null ? _fourthSkillBaseStruct : firstSkillBaseStruct;
                bool canUseThisCombine = SkillCombineStaticTools.GetCanCombineSkills(
                    combineFirstSkillBaseStruct != null ? combineFirstSkillBaseStruct.skillType : EnumSkillType.None,
                    combineSecondSkillBaseStruct != null ? combineSecondSkillBaseStruct.skillType : EnumSkillType.None,
                    combineThirdSkillBaseStruct != null ? combineThirdSkillBaseStruct.skillType : EnumSkillType.None,
                    combineFourthSkillBaseStruct != null ? combineFourthSkillBaseStruct.skillType : EnumSkillType.None);
                if (canUseThisCombine)
                {
                    _CombineSkills[0] = combineFirstSkillBaseStruct;
                    _CombineSkills[1] = combineSecondSkillBaseStruct;
                    _CombineSkills[2] = combineThirdSkillBaseStruct;
                    _CombineSkills[3] = combineFourthSkillBaseStruct;
                    if (firstSkillBaseStruct != combineFirstSkillBaseStruct ||
                        secondSkillBaseStruct != combineSecondSkillBaseStruct ||
                        thirdSkillBaseStruct != combineThirdSkillBaseStruct ||
                        fourthSkillBaseStruct != combineFourthSkillBaseStruct)
                        Call<ISkillState, SkillBaseStruct[]>(temp => temp.CombineSkills);
                }
                else
                {
                    _CombineSkills[0] = _firstSkillBaseStruct;
                    _CombineSkills[1] = _secondSkillBaseStruct;
                    _CombineSkills[2] = _thirdSkillBaseStruct;
                    _CombineSkills[3] = _fourthSkillBaseStruct;
                    if (firstSkillBaseStruct != _firstSkillBaseStruct ||
                        secondSkillBaseStruct != _secondSkillBaseStruct ||
                        thirdSkillBaseStruct != _thirdSkillBaseStruct ||
                        fourthSkillBaseStruct != _fourthSkillBaseStruct)
                        Call<ISkillState, SkillBaseStruct[]>(temp => temp.CombineSkills);
                }
            }
        }
    }

    partial void Update_IPlayerState_ISkillState()
    {
        if (isSkillStartHolding)
        {
            skillStartHoldingTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// 开始按住释放魔法键(用于初始化计时)
    /// </summary>
    /// <returns>是否可以释放该技能</returns>
    public bool StartCombineSkillRelease()
    {
        bool canRelease = _CombineSkills != null
            && _CombineSkills.Count(temp => temp != null) > 0
            && SkillCombineStaticTools.GetCanCombineSkills(_CombineSkills.Select(temp=>temp!=null?temp.skillType:EnumSkillType.None).ToArray());
        if (canRelease)
        {
            isSkillStartHolding = true;
            skillStartHoldingTime = 0;
        }
        return canRelease;
    }

    /// <summary>
    /// 结束按住(松开)释放魔法键(用于结束计时并释放)
    /// </summary>
    /// <returns>是否可以释放该技能</returns>
    public bool EndCombineSkillRelease()
    {
        if (isSkillStartHolding)
        {
            bool canRelease = _CombineSkills != null
                && _CombineSkills.Count(temp => temp != null) > 0
                && SkillCombineStaticTools.GetCanCombineSkills(_CombineSkills.Select(temp => temp != null ? temp.skillType : EnumSkillType.None).ToArray());
            //处理技能伤害数据以及粒子(粒子上包含技能伤害判定的回调函数)
            //初始化
            isSkillStartHolding = false;
            skillStartHoldingTime = 0;
            CombineSkills = null;
            return canRelease;
        }
        return false;
    }

    /// <summary>
    /// 释放普通技能,如果正在释放其他技能则无法释放 
    /// </summary>
    /// <param name="skillBaseStruct"></param>
    public bool ReleaseNormalSkill(SkillBaseStruct skillBaseStruct)
    {
        if (isSkillStartHolding)
        {
            //处理技能伤害数据以及粒子(粒子上包含技能伤害判定的回调函数)
            return true;
        }
        return false;
    }
}

