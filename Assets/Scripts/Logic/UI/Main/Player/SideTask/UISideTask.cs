using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 边框任务栏
/// </summary>
public class UISideTask : MonoBehaviour
{
    /// <summary>
    /// 边框任务栏的标题
    /// </summary>
    public GameObject taskTitleObj;
    /// <summary>
    /// 边框任务栏的内容
    /// </summary>
    public GameObject taskValueObj;

    /// <summary>
    /// 显示标题的文本控件
    /// </summary>
    public Text taskTitleText;

    /// <summary>
    /// 显示内容的文本控件
    /// </summary>
    public Text taskValueText;

    /// <summary>
    /// 自动排列组件
    /// </summary>
    VerticalLayoutGroup verticalLayoutGroup;

    /// <summary>
    /// 当前任务状态
    /// </summary>
    INowTaskState iNowTaskState;

    /// <summary>
    /// 需要在地图上标记的任务
    /// </summary>
    IMapState iMapState;

    /// <summary>
    /// 任务对象
    /// </summary>
    RuntimeTasksData runTimeTasksData;

    void Start()
    {
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        iNowTaskState = GameState.Instance.GetEntity<INowTaskState>();
        iMapState = GameState.Instance.GetEntity<IMapState>();
        GameState.Instance.Registor<INowTaskState>(INowTaskStateChanged);
        GameState.Instance.Registor<IMapState>(IMapStateChanged);
        runTimeTasksData = DataCenter.Instance.GetEntity<RuntimeTasksData>();
        ShowNowTask();
    }

    private void INowTaskStateChanged(INowTaskState iNowTaskState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<INowTaskState, int>(temp => temp.StartTask)))
        {
            ShowNowTask();
        }
    }

    private void IMapStateChanged(IMapState iMapState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IMapState, int>(temp => temp.MarkTaskID)))
        {
            ShowNowTask();
        }
    }

    /// <summary>
    /// 显示当前任务
    /// </summary>
    private void ShowNowTask()
    {
        RunTimeTaskInfo runTimeTaskInfo = null;
        //显示地图标记的任务
        if (iMapState.MarkTaskID > -1)
        {
            runTimeTaskInfo  = runTimeTasksData.GetAllToDoList().Where(temp => temp.ID == iMapState.MarkTaskID).FirstOrDefault();
        }
        //如果地图没有标记或者标记的任务已经被完成了,则显示当前的第一个主线任务
        if (runTimeTaskInfo==null)
        {
            runTimeTaskInfo = runTimeTasksData.GetAllToDoList().Where(temp => temp.RunTimeTaskNode.TaskType == Enums.TaskType.PrincipalLine).FirstOrDefault();
        }
        //如果主线已经做完了,则选取第一个任务
        if (runTimeTaskInfo == null)
        {
            runTimeTaskInfo = runTimeTasksData.GetAllToDoList().Where(temp => temp.RunTimeTaskNode.TaskType != Enums.TaskType.PrincipalLine).FirstOrDefault();
        }
        //如果当前存在任务则显示
        if (runTimeTaskInfo != null)
        {
            taskTitleObj.SetActive(true);
            taskValueObj.SetActive(true);
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            taskTitleText.text = runTimeTaskInfo.RunTimeTaskNode.TaskTitile;
            taskTitleText.text = runTimeTaskInfo.RunTimeTaskNode.TaskExplain;
            verticalLayoutGroup.SetLayoutHorizontal();
            verticalLayoutGroup.SetLayoutVertical();
        }
        //如果当前不存在任务则隐藏
        else
        {
            taskTitleObj.SetActive(false);
            taskValueObj.SetActive(false);
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            taskTitleText.text = "";
            taskTitleText.text = "";

        }
    }

}
