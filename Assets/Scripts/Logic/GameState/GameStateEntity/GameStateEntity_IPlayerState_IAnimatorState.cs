using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现了IPlayerState->IAnimatorState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState
{
    #region IAnimatorState 动画状态
    #region 外部设置
    /// <summary>
    /// 当前的魔法动画类型
    /// </summary>
    EnumMagicAnimatorType _MagicAnimatorType;
    /// <summary>
    /// 当前的魔法动画类型
    /// </summary>
    public EnumMagicAnimatorType MagicAnimatorType
    {
        get { return _MagicAnimatorType; }
        set
        {
            EnumMagicAnimatorType tempMagicAnimatorType = _MagicAnimatorType;
            _MagicAnimatorType = value;
            if (tempMagicAnimatorType != _MagicAnimatorType)
                Call<IAnimatorState, EnumMagicAnimatorType>(temp => temp.MagicAnimatorType);
        }
    }

    /// <summary>
    /// 当前的物理动画类型
    /// </summary>
    EnumPhysicAnimatorType _PhysicAnimatorType;
    /// <summary>
    /// 当前的物理动画类型
    /// </summary>
    public EnumPhysicAnimatorType PhysicAnimatorType
    {
        get { return _PhysicAnimatorType; }
        set
        {
            EnumPhysicAnimatorType tempPhysicAnimatorType = _PhysicAnimatorType;
            _PhysicAnimatorType = value;
            if (tempPhysicAnimatorType != _PhysicAnimatorType || _PhysicAnimatorType == EnumPhysicAnimatorType.Normal)
                Call<IAnimatorState, EnumPhysicAnimatorType>(temp => temp.PhysicAnimatorType);
        }
    }

    /// <summary>
    /// 当前的物理技能类型,用于检索动画 
    /// </summary>
    EnumSkillType _PhysicAnimatorSkillType;
    /// <summary>
    /// 当前的物理技能类型,用于检索动画
    /// </summary>
    public EnumSkillType PhysicAnimatorSkillType
    {
        get
        {
            return _PhysicAnimatorSkillType;
        }
        set
        {
            EnumSkillType tempPhysicAnimatorSkillType = _PhysicAnimatorSkillType;
            _PhysicAnimatorSkillType = value;
            //检测该技能是不是物理技能
            int skillNum = (int)_PhysicAnimatorSkillType;
            if (!(_PhysicAnimatorSkillType > EnumSkillType.SpecialPhysicActionReleaseStart && _PhysicAnimatorSkillType < EnumSkillType.SpecialPhysicReleaseEnd))
                _PhysicAnimatorSkillType = tempPhysicAnimatorSkillType;
            if (_PhysicAnimatorSkillType != tempPhysicAnimatorSkillType)
                Call<IAnimatorState, EnumSkillType>(temp => temp.PhysicAnimatorSkillType);
        }
    }

    /// <summary>
    /// 当前的交互动画类型
    /// </summary>
    EnumMutualAnimatorType _MutualAnimatorType;
    /// <summary>
    /// 当前的交互动画类型
    /// </summary>
    public EnumMutualAnimatorType MutualAnimatorType
    {
        get { return _MutualAnimatorType; }
        set
        {
            EnumMutualAnimatorType tempMutualAnimatorType = _MutualAnimatorType;
            _MutualAnimatorType = value;
            if (tempMutualAnimatorType != _MutualAnimatorType)
                Call<IAnimatorState, EnumMutualAnimatorType>(temp => temp.MutualAnimatorType);
        }
    }

    /// <summary>
    /// 动画的移动速度(0-1)
    /// </summary>
    float _AnimatorMoveSpeed;
    /// <summary>
    /// 动画的移动速度(0-1)
    /// </summary>
    public float AnimatorMoveSpeed
    {
        get { return _AnimatorMoveSpeed; }
        set
        {
            float tempAnimatorMoveSpeed = _AnimatorMoveSpeed;
            _AnimatorMoveSpeed = value;
            if (tempAnimatorMoveSpeed != _AnimatorMoveSpeed)
                Call<IAnimatorState, float>(temp => temp.AnimatorMoveSpeed);
        }
    }

    /// <summary>
    /// 当前的移动方向类型
    /// </summary>
    int _MoveAnimatorVectorType;
    /// <summary>
    /// 当前的移动方向类型
    /// </summary>
    public int MoveAnimatorVectorType
    {
        get { return _MoveAnimatorVectorType; }
        set
        {
            int tempMoveAnimatorVectorType = _MoveAnimatorVectorType;
            _MoveAnimatorVectorType = value;
            if (tempMoveAnimatorVectorType != _MoveAnimatorVectorType)
                Call<IAnimatorState, int>(temp => temp.MoveAnimatorVectorType);
        }
    }

    /// <summary>
    /// 翻滚动画
    /// </summary>
    bool _RollAnimator;
    /// <summary>
    /// 翻滚动画
    /// </summary>
    public bool RollAnimator
    {
        get { return _RollAnimator; }
        set
        {
            bool tempRollAnimator = _RollAnimator;
            _RollAnimator = value;
            if (tempRollAnimator != _RollAnimator)
                Call<IAnimatorState, bool>(temp => temp.RollAnimator);
        }
    }

    /// <summary>
    /// 技能动作是否持续
    /// </summary>
    bool _SkillSustainable;
    /// <summary>
    /// 技能动作是否持续
    /// </summary>
    public bool SkillSustainable
    {
        get { return _SkillSustainable; }
        set
        {
            bool tempSkillSustainable = _SkillSustainable;
            _SkillSustainable = value;
            if (tempSkillSustainable != _SkillSustainable)
                Call<IAnimatorState, bool>(temp => temp.SkillSustainable);
        }
    }
    #endregion
    #region 内部设置
    /// <summary>
    /// 是否正在进行魔法动作
    /// </summary>
    bool _IsMagicActionState;
    /// <summary>
    ///  是否正在进行魔法动作
    /// </summary>
    public bool IsMagicActionState
    {
        get { return _IsMagicActionState; }
        set
        {
            bool tempIsMagicActionState = _IsMagicActionState;
            _IsMagicActionState = value;
            if (!_IsMagicActionState)
                MagicAnimatorType = EnumMagicAnimatorType.None;
            if (tempIsMagicActionState != _IsMagicActionState)
                Call<IAnimatorState, bool>(temp => temp.IsMagicActionState);
        }
    }

    /// <summary>
    /// 是否正在进行物理动作
    /// </summary>
    bool _IsPhycisActionState;
    /// <summary>
    /// 是否正在进行物理动作
    /// </summary>
    public bool IsPhycisActionState
    {
        get { return _IsPhycisActionState; }
        set
        {
            bool tempIsPhycisActionState = _IsPhycisActionState;
            _IsPhycisActionState = value;
            if (!_IsPhycisActionState)
                PhysicAnimatorType = EnumPhysicAnimatorType.None;
            if (tempIsPhycisActionState != _IsPhycisActionState)
                Call<IAnimatorState, bool>(temp => temp.IsPhycisActionState);
        }
    }

    /// <summary>
    /// 当前物理动作(普通攻击)的阶段
    /// </summary>
    int _PhycisActionNowType;
    /// <summary>
    /// 当前物理动作(普通攻击)的阶段
    /// </summary>
    public int PhycisActionNowType
    {
        get { return _PhycisActionNowType; }
        set
        {
            int tempPhycisActionNowType = _PhycisActionNowType;
            _PhycisActionNowType = value;
            if (tempPhycisActionNowType != _PhycisActionNowType)
                Call<IAnimatorState, int>(temp => temp.PhycisActionNowType);
        }
    }

    /// <summary>
    /// 是否正在进行技能动作
    /// </summary>
    bool _IsSkillState;
    /// <summary>
    /// 是否正在进行技能动作
    /// </summary>
    public bool IsSkillState
    {
        get { return _IsSkillState; }
        set
        {
            bool tempIsSkillState = _IsSkillState;
            _IsSkillState = value;
            if (!_IsSkillState)
                PhysicAnimatorType = EnumPhysicAnimatorType.None;
            if (tempIsSkillState != _IsSkillState)
                Call<IAnimatorState, bool>(temp => temp.IsSkillState);
        }
    }
    #endregion

    #endregion

}