using System;
using System.Collections;
using System.Collections.Generic;
using TaskMap;
using UnityEngine;

/// <summary>
/// 场景状态--火炬控制门的打开状态
/// </summary>
public class SceneState_TorchLightToDoor : MonoBehaviour
{

    /// <summary>
    /// 该状态的ID
    /// </summary>
    [SerializeField]
    private int StateID;

    /// <summary>
    /// 监听触发任务的ID
    /// </summary>
    [SerializeField]
    private int ListenTriggerTaskID;

    /// <summary>
    /// 场景状态
    /// </summary>
    SceneStateDatas sceneStateData;
    /// <summary>
    /// 游戏状态
    /// </summary>
    IGameState iGameState;
    /// <summary>
    /// 任务状态事件
    /// </summary>
    INowTaskStateEvent iNowTaskStateEvent;
    /// <summary>
    /// 门的状态
    /// </summary>
    bool doorState;

    /// <summary>
    /// 开门状态的角度
    /// </summary>
    public Vector3 openAngle;

    void Start()
    {
        sceneStateData = DataCenter.Instance.GetEntity<SceneStateDatas>();
        iGameState = GameState.Instance.GetEntity<IGameState>();
        iNowTaskStateEvent = GameState.Instance.GetEntity<INowTaskStateEvent>();
        object data = sceneStateData.GetData(StateID, iGameState.SceneName);
        if (data == null || !data.GetType().Equals(typeof(bool)))
        {
            doorState = false;
        }
        else
        {
            doorState = (bool)data;
        }
        //注册
        iNowTaskStateEvent.RegistTaskEvent(TaskMap.Enums.EnumTaskEventType.Trigger, EventTriggerCallBack);
        if (doorState)
            OpenDoor();
    }

    /// <summary>
    /// Trigger型的触发任务
    /// </summary>
    /// <param name="taskEventData"></param>
    private void EventTriggerCallBack(TaskEventData taskEventData)
    {
        if (doorState)
            return;
        if (taskEventData.EventType == TaskMap.Enums.EnumTaskEventType.Trigger && taskEventData.TaskID == ListenTriggerTaskID)
        {
            //保存状态并播放开门动画
            doorState = true;
            sceneStateData.SetData(StateID, iGameState.SceneName, true);
            OpenDoor();
        }
    }

    /// <summary>
    /// 开门
    /// </summary>
    private void OpenDoor()
    {
        transform.eulerAngles = openAngle;
    }
    
}
