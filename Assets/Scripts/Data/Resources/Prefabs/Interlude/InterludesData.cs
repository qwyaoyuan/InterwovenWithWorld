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
    public InterludesItemStruct GetInterludesItemStructByTaskID(int taskID)
    {
        return interludesItemStructs.Where(temp => temp.TaskID == taskID).FirstOrDefault();
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
}