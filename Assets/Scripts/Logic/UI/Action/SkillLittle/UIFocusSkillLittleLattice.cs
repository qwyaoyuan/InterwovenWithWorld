using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIFocusSkillLittleLattice : UIFocus
{
    /// <summary>
    /// 临时使用的技能点
    /// </summary>
    public static int tempUseSkillPoint;

    /// <summary>
    /// UI加减控件
    /// </summary>
    UIAddNum uiAddNum;

    /// <summary>
    /// 设置状态
    /// </summary>
    bool settingState;

    /// <summary>
    /// 技能id
    /// </summary>
    public int skillID;

    /// <summary>
    /// 临时的技能加点
    /// </summary>
    public int TempPoint
    {
        get
        {
            if (uiAddNum)
                return uiAddNum.Value;
            return 0;
        }
    }

    /// <summary>
    /// 玩家存档
    /// </summary>
    PlayerState playerState;

    /// <summary>
    /// 技能元数据
    /// </summary>
    SkillStructData skillStructData;

    /// <summary>
    /// 更新前置条件状态
    /// </summary>
    Action UpdatePreconditionStateAction;

    /// <summary>
    /// 所有组的当前加点情况 (这个数据用于计算技能加点时,判断当前的状态是否复合前置条件)
    /// </summary>
    public static Dictionary<EnumSkillZone,int> zonePointDic;
    /// <summary>
    /// 所有技能的当前加点情况(这个数据用于计算技能加点时,判断当前的状态是否复合前置条件)
    /// </summary>
    public static Dictionary<EnumSkillType, int> skillPointDic;

    /// <summary>
    /// 所有技能对于组点数的需求情况,根据技能计算出他们的需求(这个数据用于计算技能减点时,判断是否可以减去一点)
    /// </summary>
    public static Dictionary<EnumSkillZone, int> zoneMustPointDic;
    /// <summary>
    /// 所有技能对于前置点数的需求情况,根据技能计算出他们的需求(这个数据用于计算技能减点时,判断是否可以减去一点)
    /// </summary>
    public static Dictionary<EnumSkillType, int> skillMustPointDic;

    private void Awake()
    {
        uiAddNum = GetComponent<UIAddNum>();
        uiAddNum.ClickLeftHandle += UiAddNum_ClickLeftHandle;
        uiAddNum.ClickRightHandle += UIAddNum_ClickRightHandle;
    }   

    /// <summary>
    /// 设置焦点
    /// </summary>
    public override void SetForcus()
    {
        if (uiAddNum)
            uiAddNum.SetImageState(true);
    }

    /// <summary>
    /// 失去焦点
    /// </summary>
    public override void LostForcus()
    {
        if (uiAddNum)
            uiAddNum.SetImageState(false);
        SkillLittleSettingState = false;
    }

    /// <summary>
    /// 获取和设置技能设置状态
    /// </summary>
    public bool SkillLittleSettingState
    {
        get { return settingState; }
        set
        {
            settingState = value;
            if (uiAddNum)
            {
                //如果是false则直接隐藏
                if (settingState)
                {
                    uiAddNum.SetLeftHandleState(settingState);
                    uiAddNum.SetRightHandleState(settingState);
                }
                //如果是true则需要判断是否可以进行增加或减少（根据剩余点数与当前已经加点的数值，以及根据当前的技能等级与最大最小值的关系）
                //如果是true则需要判断是否可以点击该技能,比如技能的前置要求等
                else
                {
                    EnumSkillType enumSkillType = (EnumSkillType)skillID;
                    if (CheckSkillPrecondition(enumSkillType))//检查技能的前置条件
                    {
                        if (CheckMinHandleCanUse(enumSkillType))//检查技能是否可以点击减按钮
                            uiAddNum.SetLeftHandleState(true);
                        if (CheckMaxHandleCanUse(enumSkillType))//检查技能是否可以点击加按钮
                            uiAddNum.SetRightHandleState(true);
                    }
                    else
                    {
                        uiAddNum.SetLeftHandleState(false);
                        uiAddNum.SetRightHandleState(false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 是否可以移动到下一个
    /// 点击A键可以确认和返回
    /// </summary>
    /// <param name="moveType"></param>
    /// <returns></returns>
    public override bool CanMoveNext(UIFocusPath.MoveType moveType)
    {
        switch (moveType)
        {
            case UIFocusPath.MoveType.LEFT:
            case UIFocusPath.MoveType.RIGHT:
            case UIFocusPath.MoveType.UP:
            case UIFocusPath.MoveType.DOWN:
                return !SkillLittleSettingState;
            case UIFocusPath.MoveType.OK:
                return false;
        }
        return true;
    }

    /// <summary>
    /// 移动子节点（技能）
    /// </summary>
    /// <param name="moveType"></param>
    public override void MoveChild(UIFocusPath.MoveType moveType)
    {
        switch (moveType)
        {
            case UIFocusPath.MoveType.LEFT:
            case UIFocusPath.MoveType.DOWN:
                UIAddNum_ClickRightHandle();
                break;
            case UIFocusPath.MoveType.RIGHT:
            case UIFocusPath.MoveType.UP:
                UIAddNum_ClickRightHandle();
                break;
            case UIFocusPath.MoveType.OK:
                SkillLittleSettingState = !SkillLittleSettingState;
                break;
        }
    }

    /// <summary>
    /// 右侧手柄被点击
    /// </summary>
    private void UIAddNum_ClickRightHandle()
    {
        EnumSkillType enumSkillType = (EnumSkillType)skillID;
        //需要通过数据进行验证，然后更改显示
        if (CheckSkillPrecondition(enumSkillType))
        {
            if (CheckMaxHandleCanUse(enumSkillType))
                if (uiAddNum)
                {
                    tempUseSkillPoint++;
                    uiAddNum.Value++;
                }
        }
        if (UpdatePreconditionStateAction!=null)
            UpdatePreconditionStateAction();
        SkillLittleSettingState = true;
    }

    /// <summary>
    /// 左侧手柄被点击
    /// </summary>
    private void UiAddNum_ClickLeftHandle()
    {
        EnumSkillType enumSkillType = (EnumSkillType)skillID;
        //需要通过数据进行验证，然后更改显示
        if (CheckSkillPrecondition(enumSkillType))
        {
            if (CheckMinHandleCanUse(enumSkillType))
                if (uiAddNum)
                {
                    tempUseSkillPoint--;
                    uiAddNum.Value--;
                }
        }
        if (UpdatePreconditionStateAction != null)
            UpdatePreconditionStateAction();
        SkillLittleSettingState = true;
    }

    /// <summary>
    /// 初始化显示
    /// </summary>
    /// <param name="UpdatePreconditionStateAction">更新组加点状态(使用的是临时的加点状态计算的)</param>
    public void InitSkillShow(Action UpdatePreconditionStateAction)
    {
        this.UpdatePreconditionStateAction = UpdatePreconditionStateAction;
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        skillStructData = DataCenter.Instance.GetMetaData<SkillStructData>();
        EnumSkillType enumSkillType = (EnumSkillType)skillID;
        if (uiAddNum)
        {
            //设置UI显示 
            //设置技能等级范围
            SkillBaseStruct skillBaseStruct = skillStructData.SearchSkillDatas(temp => temp.skillType == enumSkillType).FirstOrDefault();
            if (skillBaseStruct != null)
            {
                uiAddNum.Min = 0;
                uiAddNum.Max = skillBaseStruct.maxLevel;
            }
            //设置当前的技能等级 
            int skillPoint;
            if (playerState.SkillPoint.TryGetValue(enumSkillType, out skillPoint))
            {
                uiAddNum.Value = skillPoint;
            }
            else
            {
                uiAddNum.Value = 0;
            }
        }
    }

    /// <summary>
    /// 检查技能前置条件
    /// </summary>
    /// <param name="enumSkillType"></param>
    /// <returns></returns>
    private bool CheckSkillPrecondition(EnumSkillType enumSkillType)
    {
        SkillBaseStruct skillBaseStruct = skillStructData.SearchSkillDatas(temp => temp.skillType == enumSkillType).FirstOrDefault();
        if (skillBaseStruct != null)
        {
            //需求技能组的加点
            KeyValuePair<EnumSkillZone, int>[] skillMustZoneDic = skillBaseStruct.skillPrecondition.mustSkillZonePointDic.OfType<KeyValuePair<EnumSkillZone, int>>().ToArray();
            foreach (var item in skillMustZoneDic)
            {
                int nowZonePoint = 0;
                zonePointDic.TryGetValue(item.Key, out nowZonePoint);//当前组的加点
                if (nowZonePoint < item.Value)//如果当前的组加点小于必需的组加点
                    return false;
            }
            //需求前置技能的加点 
            KeyValuePair<EnumSkillType, int>[] skillMustTypeDic = skillBaseStruct.skillPrecondition.mustSkillPointDic.OfType<KeyValuePair<EnumSkillType, int>>().ToArray();
            foreach (var item in skillMustTypeDic)
            {
                int nowSkillPoint = 0;
                skillPointDic.TryGetValue(item.Key, out nowSkillPoint);//当前的技能加点 
                if (nowSkillPoint < item.Value)//如果当前的加点小于必须的加点
                    return false;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检查技能的减按钮是否可以使用
    /// </summary>
    /// <param name="enumSkillType"></param>
    /// <returns></returns>
    private bool CheckMinHandleCanUse(EnumSkillType enumSkillType)
    {
        int oldPoint;
        if (playerState.SkillPoint.TryGetValue(enumSkillType, out oldPoint))
        {
            if (oldPoint <= 0)//技能如果已经小于等于0了,则不能在减了
                return false;
            if (uiAddNum.Value <= oldPoint)//如果技能小于等于之前的加点数据,则不能再减了
                return false;
            if (uiAddNum.Value == 1)//需要判断如果该技能等0(减去1后)时,是否会影响后置技能(组加点以及前置加点)
            {
                SkillBaseStruct skillBaseStruct = skillStructData.SearchSkillDatas(temp => temp.skillType == enumSkillType).FirstOrDefault();
                if (skillBaseStruct!=null)
                {
                    EnumSkillZone skillZone = skillBaseStruct.skillZone;//当前技能所在的组 
                    int nowSkillZonePoint = 0;
                    zonePointDic.TryGetValue(skillZone,out nowSkillZonePoint);//获取当前技能组的点数 
                    int mustSkillZonePoint = 0;
                    zoneMustPointDic.TryGetValue(skillZone,out mustSkillZonePoint);//获取需求技能组的点数
                    return mustSkillZonePoint <= nowSkillZonePoint;
                }
                return false;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检查技能的加按钮是否可以使用
    /// </summary>
    /// <param name="enumSkillType"></param>
    /// <returns></returns>
    private bool CheckMaxHandleCanUse(EnumSkillType enumSkillType)
    {
        int oldPoint;
        SkillBaseStruct skillBaseStruct = skillStructData.SearchSkillDatas(temp => temp.skillType == enumSkillType).FirstOrDefault();
        if (playerState.SkillPoint.TryGetValue(enumSkillType, out oldPoint)&& skillBaseStruct!=null)
        {
            if (uiAddNum.Value >= skillBaseStruct.maxLevel)//技能如果已经到达最大了,则不能再加了
                return false;
            if (playerState.FreedomPoint <= tempUseSkillPoint)//如果剩余技能点已经全部被使用,则不能再加了
                return false;
            return true;
        }
        return false;
    }
}
