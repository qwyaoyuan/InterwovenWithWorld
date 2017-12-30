using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private SkillManager()
    {
        GameState.Instance.Registor<IGameState>(GameStateChanged);
    }

    /// <summary>
    /// 游戏状态发生变化
    /// </summary>
    /// <param name="iGameState"></param>
    /// <param name="fieldName"></param>
    private void GameStateChanged(IGameState iGameState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<GameState, Action>(temp => temp.LoadArchive)))
        {
            InitDataTarget();
        }
    }

    /// <summary>
    /// 按键对应数据对象
    /// </summary>
    KeyContactData keyContactData;

    /// <summary>
    /// 技能状态对象
    /// </summary>
    ISkillState iSkillState;

    /// <summary>
    /// 技能数据管理对象
    /// </summary>
    SkillStructData skillStructData;

    /// <summary>
    /// 游戏状态
    /// </summary>
    IGameState iGameState;

    /// <summary>
    /// 初始化数据对象
    /// </summary>
    private void InitDataTarget()
    {
        keyContactData = DataCenter.Instance.GetEntity<KeyContactData>();
        skillStructData = DataCenter.Instance.GetMetaData<SkillStructData>();
        iSkillState = GameState.Instance.GetEntity<ISkillState>();
        iGameState = GameState.Instance.GetEntity<IGameState>();
    }



    public void KeyDown(int key)
    {
        if (keyContactData == null)
            return;
        if (iGameState.GameRunType != EnumGameRunType.Safe && iGameState.GameRunType != EnumGameRunType.Unsafa)
            return;
        KeyContactStruct[] keyContactStructs = keyContactData.GetKeyContactStruct(key, temp => temp.keyContactType == EnumKeyContactType.Skill);
        if (keyContactStructs.Length > 0)
        {
            foreach (KeyContactStruct keyContactStruct in keyContactStructs)//便利所有按键对象
            {
                int skillID = keyContactStruct.id;
                //组合技能放入技能组合盘
                if (skillID > (int)EnumSkillType.MagicCombinedStart)
                {
                    EnumSkillType[] enumSkillTypes = SkillCombineStaticTools.GetCombineSkills(skillID);
                    SkillBaseStruct[] combineSkills = skillStructData.SearchSkillDatas(temp => enumSkillTypes.Contains(temp.skillType));
                    iSkillState.CombineSkills = combineSkills;
                }
                else if (skillID > (int)EnumSkillType.MagicCombinedLevel1Start && skillID < (int)EnumSkillType.MagicCombinedLevel4End)
                {
                    SkillBaseStruct[] singleCombineSkills = skillStructData.SearchSkillDatas(temp => (EnumSkillType)skillID == temp.skillType);
                    iSkillState.CombineSkills = singleCombineSkills;
                }
                //如果按下魔法释放键,则释放组合盘中的魔法
                else if (skillID == (int)EnumSkillType.MagicRelease)
                {
                    iSkillState.StartCombineSkillRelease();
                }
                else//非组合技能直接释放
                {
                    iSkillState.ReleaseNormalSkill(skillStructData.SearchSkillDatas(temp => (int)temp.skillType == skillID).FirstOrDefault());
                }
            }
        }
        else if (keyContactData.GetKeyContactStruct(key, temp => temp.keyContactType == EnumKeyContactType.None).Count() == 1)//如果输入了空的按键则
        {
            iSkillState.CombineSkills = null;
        }
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
        if (keyContactData == null)
            return;
        if (iGameState.GameRunType != EnumGameRunType.Safe && iGameState.GameRunType != EnumGameRunType.Unsafa)
            return;
        KeyContactStruct[] keyContactStructs = keyContactData.GetKeyContactStruct(key, temp => temp.keyContactType == EnumKeyContactType.Skill);
        if (keyContactStructs.Length > 0)
        {
            foreach (KeyContactStruct keyContactStruct in keyContactStructs)//便利所有按键对象
            {
                int skillID = keyContactStruct.id;
                if (skillID == (int)EnumSkillType.MagicRelease)//松开释放按钮
                {
                    iSkillState.EndCombineSkillRelease();
                }
            }
        }
    }

    public void Move(Vector2 forward)
    {

    }

    public void View(Vector2 view)
    {

    }


}
