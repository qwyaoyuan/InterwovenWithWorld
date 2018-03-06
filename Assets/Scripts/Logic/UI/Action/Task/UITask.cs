using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI任务界面
/// </summary>
public class UITask : MonoBehaviour
{
    /// <summary>
    /// UI的任务集合对象
    /// </summary>
    [SerializeField]
    private UIList uiTaskList;

    /// <summary>
    /// 当前的任务
    /// </summary>
    private UIListItem nowTaskItem;

    /// <summary>
    /// 显示UI文字的对象
    /// </summary>
    [SerializeField]
    private Text taskExplanText;

    /// <summary>
    /// 运行时任务
    /// </summary>
    TaskMap.RunTimeTaskData runtimeTaskData;

    /// <summary>
    /// 地图的状态对象 
    /// </summary>
    IMapState iMapState;

    /// <summary>
    /// 当前任务状态
    /// </summary>
    INowTaskState iNowTaskState;

    private void Awake()
    {
        uiTaskList.ItemClickHandle += UiTaskList_ItemClickHandle;
    }

    /// <summary>
    /// 点击任务列表
    /// </summary>
    /// <param name="mouseType"></param>
    /// <param name="target"></param>
    private void UiTaskList_ItemClickHandle(UIList.ItemClickMouseType mouseType, UIListItem target)
    {
        if (nowTaskItem && nowTaskItem.childImage)
            nowTaskItem.childImage.enabled = false;
        nowTaskItem = target;
        nowTaskItem.childImage.enabled = true;
        if (mouseType == UIList.ItemClickMouseType.Right)//标记任务
            MarkTaskToMap();
        ExplanNowItem();
    }

    private void OnEnable()
    {
        iMapState = GameState.Instance.GetEntity<IMapState>();
        iNowTaskState = GameState.Instance.GetEntity<INowTaskState>();
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;

        uiTaskList.Init();
        //填充任务列表
        TaskMap.RunTimeTaskInfo[] runTimeTaskInfoArray = iNowTaskState.GetStartTask(null); //runtimeTaskData.GetAllToDoList().Where(temp => temp.IsStart == true).ToArray();
        foreach (TaskMap.RunTimeTaskInfo runTimeTaskInfo in runTimeTaskInfoArray)
        {
            TaskMap.TaskInfoStruct taskInfoStruct = runTimeTaskInfo.TaskInfoStruct;
            UIListItem uiListItem = uiTaskList.NewItem();
            uiListItem.value = runTimeTaskInfo;
            uiListItem.childText.text = taskInfoStruct.TaskTitile;
        }
        uiTaskList.UpdateUI();
        nowTaskItem = uiTaskList.FirstShowItem();
        if (nowTaskItem)
            uiTaskList.ShowItem(nowTaskItem);
        ExplanNowItem();
    }

    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
    }

    /// <summary>
    /// 按键检测
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        Action<int> MoveListAction = (addOffset) =>
        {
            UIListItem[] tempArrays = uiTaskList.GetAllImtes();
            if (tempArrays.Length == 0)
                return;
            int index = 0;
            if (nowTaskItem)
                index = tempArrays.ToList().IndexOf(nowTaskItem);
            if (index < 0)
                index = 0;
            index += addOffset;
            index = Mathf.Clamp(index, 0, tempArrays.Length - 1);
            if (index < tempArrays.Length)
            {
                uiTaskList.ShowItem(tempArrays[index]);
                if (nowTaskItem && nowTaskItem.childImage)
                    nowTaskItem.childImage.enabled = false;
                nowTaskItem = tempArrays[index];
                if (nowTaskItem && nowTaskItem.childImage)
                    nowTaskItem.childImage.enabled = true;
                ExplanNowItem();
            }
        };
        switch (keyType)
        {
            case UIManager.KeyType.Y://任务追踪
                MarkTaskToMap();
                break;
            case UIManager.KeyType.UP:
                MoveListAction(-1);
                break;
            case UIManager.KeyType.DOWN:
                MoveListAction(1);
                break;
        }
    }

    /// <summary>
    /// 标记当前任务
    /// </summary>
    private void MarkTaskToMap()
    {
        if (nowTaskItem)
        {
            TaskMap.RunTimeTaskInfo runTimeTaskInfo = nowTaskItem.value as TaskMap.RunTimeTaskInfo;
            if (runTimeTaskInfo != null)
            {
                if (iMapState.MarkTaskID == runTimeTaskInfo.ID)//如果相等则取消标记
                    iMapState.MarkTaskID = -1;
                else//如果不想等则重新标记
                    iMapState.MarkTaskID = runTimeTaskInfo.ID;
            }
        }
    }

    /// <summary>
    /// 展示当前任务的说明与进度
    /// </summary>
    private void ExplanNowItem()
    {
        if (!nowTaskItem)
            taskExplanText.text = "";
        else
        {
            //显示任务
            TaskMap.RunTimeTaskInfo runTimeTaskInfo = nowTaskItem.value as TaskMap.RunTimeTaskInfo;
            if (runTimeTaskInfo != null)
            {
                taskExplanText.text = runTimeTaskInfo.TaskInfoStruct.TaskExplain;
                //根据类型以及其他信息显示完善的数据信息
                //throw new Exception("未完善");
            }
            else
                taskExplanText.text = "";
        }
    }

}
