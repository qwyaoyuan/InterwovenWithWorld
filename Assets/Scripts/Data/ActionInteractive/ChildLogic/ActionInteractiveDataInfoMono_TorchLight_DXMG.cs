using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理地下迷宫的火炬功能
/// </summary>
public class ActionInteractiveDataInfoMono_TorchLight_DXMG : ActionInteractiveDataInfoMono
{
    /// <summary>
    /// 火炬的状态
    /// </summary>
    public bool torchLightState;

    /// <summary>
    /// 表示火炬的下标
    /// </summary>
    private int torchIndex;

    /// <summary>
    /// 与火炬开关相关的对象
    /// </summary>
    private GameObject targetObj;

    protected override void InnerAwake()
    {
        base.InnerAwake();
        targetObj = transform.GetChild(0).gameObject;
    }

    public override void LoadData(object obj, Action<object> SaveDataAction)
    {
        base.LoadData(obj, SaveDataAction);
        if (obj != null && obj.GetType().Equals(typeof(bool)))
        {
            torchLightState = (bool)obj;
        }
        ActionInteractiveDataInfo.OtherDataStruct otherDataStruct = ActionInteractiveDataInfo.OtherValue as ActionInteractiveDataInfo.OtherDataStruct;
        if (otherDataStruct != null&& !string.IsNullOrEmpty( otherDataStruct.Data))
        {
            int index = 0;
            if (int.TryParse(otherDataStruct.Data.Trim(), out index))
            {
                //每一个代表不同的火炬,通过下标获取相关的对象
                switch (index)
                {
                    case 1:
                        torchIndex = 1;
                        break;
                    case 2:
                        torchIndex = 2;
                        break;
                    case 3:
                        torchIndex = 3;
                        break;
                    case 4:
                        torchIndex = 4;
                        break;
                }
            }
        }
        if (targetObj)
            targetObj.SetActive(torchLightState);
    }

    public override void ActionTrigger()
    {
        base.ActionTrigger();
        if (!torchLightState)
        {
            //触发机关
            torchLightState = true;
            if (targetObj)
                targetObj.SetActive(true);
            //保存状态
            SaveDataAction(true);
            //设置任务状态
            INowTaskState iNowTaskState = GameState.Instance.GetEntity<INowTaskState>();
            switch (torchIndex)
            {
                case 1:
                    iNowTaskState.CheckNowTask(EnumCheckTaskType.Special, (int)TaskMap.Enums.EnumTaskSpecialCheck.DXMG_TorchLight1);
                    break;
                case 2:
                    iNowTaskState.CheckNowTask(EnumCheckTaskType.Special, (int)TaskMap.Enums.EnumTaskSpecialCheck.DXMG_TorchLight2);
                    break;
                case 3:
                    iNowTaskState.CheckNowTask(EnumCheckTaskType.Special, (int)TaskMap.Enums.EnumTaskSpecialCheck.DXMG_TorchLight3);
                    break;
                case 4:
                    iNowTaskState.CheckNowTask(EnumCheckTaskType.Special, (int)TaskMap.Enums.EnumTaskSpecialCheck.DXMG_TorchLight4);
                    break;
            }
      
        }
    }
}
