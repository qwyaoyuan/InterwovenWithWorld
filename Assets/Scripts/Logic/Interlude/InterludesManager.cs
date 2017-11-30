using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 过场控制器
/// </summary>
public class InterludesManager : IEntrance
{
    public void OnDestroy()
    {
        
    }

    public void Start()
    {
        GameState.Instance.Registor<INowTaskState>(INowTaskStateChanged);
    }

    private void INowTaskStateChanged(INowTaskState iNowTaskState, string targetName)
    {
        //接受任务时调用
        if (string.Equals(targetName, GameState.Instance.GetFieldName<INowTaskState, int>(temp => temp.StartTask)))
        {
            InterludesData interludesData = DataCenter.Instance.GetMetaData<InterludesData>();
            InterludesItemStruct interludesItemStruct = interludesData.GetInterludesItemStructByTaskID(iNowTaskState.StartTask);
            if (interludesItemStruct != null)
            {
                throw new Exception("未实现,需要开始进行过场动画");
            }
            else
            {
                Debug.Log("该任务没有过场动画");
            }
        }
    }

    public void Update()
    {
        
    }
}
