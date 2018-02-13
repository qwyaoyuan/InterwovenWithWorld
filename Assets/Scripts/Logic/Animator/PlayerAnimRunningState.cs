using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 动画的运行时状态
/// </summary>
public class PlayerAnimRunningState : StateMachineBehaviour
{
    /// <summary>
    /// 动画剪辑的类型
    /// </summary>
    [SerializeField]
    private EnumAnimationClipType AnimationClipType;

    /// <summary>
    /// 动画运行时的状态数据
    /// </summary>
    [SerializeField]
    private List<PlayerAnimRunningState_Struct> DataStructs;

    /// <summary>
    /// 动画层
    /// </summary>
    [SerializeField]
    private int animLayer;

    /// <summary>
    /// 触发当前运行时状态后的事件
    /// </summary>
    public event Action<EnumAnimationClipType, EnumAnimationClipTimeType, float> AnimRunningStateHandle;

    /// <summary>
    /// 当前状态
    /// </summary>
    PlayerAnimRunningState_Struct NowDataStruct;

    private void Awake()
    {
        DataStructs.RemoveAll(temp => temp.AnimationClipTimeType == EnumAnimationClipTimeType.In || temp.AnimationClipTimeType == EnumAnimationClipTimeType.Out);
        DataStructs.Add(new PlayerAnimRunningState_Struct() { AnimationClipTimeType = EnumAnimationClipTimeType.In, Time = 0 });
        DataStructs.Add(new PlayerAnimRunningState_Struct() { AnimationClipTimeType = EnumAnimationClipTimeType.Out, Time = 1 });
        DataStructs = DataStructs.OrderBy(temp => temp.Time).ToList();
    }


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NowDataStruct = DataStructs.FirstOrDefault(temp => temp.AnimationClipTimeType == EnumAnimationClipTimeType.In);
        if (AnimRunningStateHandle != null)
        {
            AnimRunningStateHandle(AnimationClipType, EnumAnimationClipTimeType.In, 0);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NowDataStruct = DataStructs.FirstOrDefault(temp => temp.AnimationClipTimeType == EnumAnimationClipTimeType.In);
        if (AnimRunningStateHandle != null)
        {
            AnimRunningStateHandle(AnimationClipType, EnumAnimationClipTimeType.Out, 1);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerAnimRunningState_Struct _NowDataStruct = DataStructs.LastOrDefault(temp => temp.AnimationClipTimeType != EnumAnimationClipTimeType.Out && temp.AnimationClipTimeType != EnumAnimationClipTimeType.In && temp.Time < stateInfo.normalizedTime);
        if (_NowDataStruct != null)
        {
            if (_NowDataStruct != NowDataStruct)
            {
                NowDataStruct = _NowDataStruct;
                if (AnimRunningStateHandle != null)
                {
                    AnimRunningStateHandle(AnimationClipType, _NowDataStruct.AnimationClipTimeType, stateInfo.normalizedTime);
                }
            }
        }
        else
        {
            _NowDataStruct = DataStructs.FirstOrDefault(temp => temp.AnimationClipTimeType == EnumAnimationClipTimeType.In);
            if (AnimRunningStateHandle != null)
            {
                AnimRunningStateHandle(AnimationClipType, _NowDataStruct.AnimationClipTimeType, stateInfo.normalizedTime);
            }
        }
    }
}

/// <summary>
/// 动画的运行时状态数据
/// </summary>
[Serializable]
public class PlayerAnimRunningState_Struct
{
    /// <summary>
    /// 运行时间
    /// </summary>
    [Range(0, 1)]
    public float Time;
    /// <summary>
    /// 动画剪辑的时间状态类型
    /// </summary>
    public EnumAnimationClipTimeType AnimationClipTimeType;
}