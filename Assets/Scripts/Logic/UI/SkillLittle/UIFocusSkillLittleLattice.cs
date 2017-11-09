using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFocusSkillLittleLattice : UIFocus
{
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
                uiAddNum.SetLeftHandleState(settingState);
                uiAddNum.SetRightHandleState(settingState);
                //如果是true则需要判断是否可以进行增加或减少（根据剩余点数与当前已经加点的数值，以及根据当前的技能等级与最大最小值的关系）
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
        //需要通过数据进行验证，然后更改显示
    }

    /// <summary>
    /// 左侧手柄被点击
    /// </summary>
    private void UiAddNum_ClickLeftHandle()
    {
        //需要通过数据进行验证，然后更改显示
    }

    /// <summary>
    /// 初始化显示
    /// </summary>
    public void InitSkillShow()
    {
        if (uiAddNum)
        {
            //查询出技能id然后显示
            //uiAddNum.Value = ?;
        }
    }
}
