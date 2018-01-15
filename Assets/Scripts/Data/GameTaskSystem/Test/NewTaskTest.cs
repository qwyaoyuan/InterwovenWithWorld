using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaskMap;
using System.Runtime.Serialization;

public class NewTaskText : MonoBehaviour
{


    TaskMap.RunTimeTaskData runTimeTaskData;
    // Use this for initialization
    void Start()
    {
        runTimeTaskData = new RunTimeTaskData();
        runTimeTaskData.OnDeserializedMethod(default(StreamingContext));
        List<TaskMap.RunTimeTaskInfo> runtimeTaskInfos = runTimeTaskData.GetAllToDoList();
        foreach (var item in runtimeTaskInfos)
        {
            item.IsStart = true;
            item.IsOver = true;
        }
        runtimeTaskInfos = runTimeTaskData.GetAllToDoList();
        foreach (var item in runtimeTaskInfos)
        {
            item.IsStart = true;
            item.IsOver = true;
        }
        runtimeTaskInfos = runTimeTaskData.GetAllToDoList();
        foreach (var item in runtimeTaskInfos)
        {
            if (item.ID == 4)
            {
                item.IsStart = true;
                item.IsOver = true;
            }
        }
        DebugNowState();
    }

    private void DebugNowState()
    {
        List<TaskMap.RunTimeTaskInfo> runtimeTaskInfos = runTimeTaskData.GetAllToDoList();
        foreach (var item in runtimeTaskInfos)
        {
            Debug.Log(item.ID);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
