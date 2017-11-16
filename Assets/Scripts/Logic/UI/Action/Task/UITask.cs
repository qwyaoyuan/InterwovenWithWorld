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
        MarkTaskToMap();
    }

    private void OnEnable()
    {
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        uiTaskList.Init();
        //填充任务列表
        //----------------------//
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
        if (!nowTaskItem)
        {
            //只是地图跟踪当前任务
            //-------------------//
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
            //--------------------------//
        }
    }

}
