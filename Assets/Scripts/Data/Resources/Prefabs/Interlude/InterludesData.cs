using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 过场数据
/// </summary>
public class InterludesData : ILoadable<InterludesData>
{
    /// <summary>
    /// 过场的数据
    /// </summary>
    InterludesItemStruct[] interludesItemStructs;

    public void Load()
    {
        interludesItemStructs = new InterludesItemStruct[0];
    }

    /// <summary>
    /// 通过任务id获取该任务的过场动画
    /// </summary>
    /// <param name="taskID"></param>
    /// <returns></returns>
    public InterludesItemStruct GetInterludesItemStructByTaskID(int taskID, InterludesItemStruct.EnumInterludesShowTime interludesShowTime = InterludesItemStruct.EnumInterludesShowTime.Start)
    {
        return interludesItemStructs.Where(temp => temp.TaskID == taskID && temp.InterludesShowTime == interludesShowTime).FirstOrDefault();
    }

}


/// <summary>
/// 每条过场的数据
/// </summary>
public class InterludesItemStruct
{
    /// <summary>
    /// 接收该任务的时候触发过场
    /// </summary>
    public int TaskID;

    /// <summary>
    /// 过场动画的显示时间
    /// </summary>
    public EnumInterludesShowTime InterludesShowTime;

    /// <summary>
    /// 过场的显示时间
    /// </summary>
    public enum EnumInterludesShowTime
    {
        /// <summary>
        /// 接收任务时显示
        /// </summary>
        Start,
        /// <summary>
        /// 完成任务时显示
        /// </summary>
        Over
    }
}
