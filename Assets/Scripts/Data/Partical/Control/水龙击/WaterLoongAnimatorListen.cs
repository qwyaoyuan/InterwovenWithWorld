using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 监听水龙动画
/// </summary>
public class WaterLoongAnimatorListen : StateMachineBehaviour
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public List<float> startTimeList;
    /// <summary>
    /// 结束时间
    /// </summary>
    public List<float> endTimeList;

    /// <summary>
    /// 上一帧的时间
    /// </summary>
    float lastTime;

    /// <summary>
    /// 状态回调
    /// true表示进入 false表示离开
    /// </summary>
    public Action<bool> StateCallBack;

    /// <summary>
    /// 运行时计算的开始时间
    /// </summary>
    List<float> startTimeArray;
    /// <summary>
    /// 运行时计算的结束时间
    /// </summary>
    List<float> endTimeArray;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lastTime = 0;
        startTimeArray = startTimeList.OrderBy<float, float>(temp => temp).ToList();
        endTimeArray = endTimeList.OrderBy<float, float>(temp => temp).ToList();
    }

    private void OnEnable()
    {
        lastTime = 0;
        startTimeArray = startTimeList.OrderBy<float, float>(temp => temp).ToList();
        endTimeArray = endTimeList.OrderBy<float, float>(temp => temp).ToList();
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float tempTime = lastTime;
        lastTime = stateInfo.normalizedTime;
        if (startTimeArray != null && startTimeArray.Count > 0)
        {
            float first = startTimeArray[0];
            bool isEnter = tempTime < first && stateInfo.normalizedTime > first;
            if (isEnter)
            {
                startTimeArray.RemoveAt(0);
                if (StateCallBack != null)
                {
                    StateCallBack(true);
                }
            }
        }
        if (endTimeArray != null && endTimeArray.Count > 0)
        {
            float first = endTimeArray[0];
            bool isLeave = tempTime < first && stateInfo.normalizedTime > first;
            if (isLeave)
            {
                endTimeArray.RemoveAt(0);
                if (StateCallBack != null)
                {
                    StateCallBack(false);
                }
            }
        }

    }

}
