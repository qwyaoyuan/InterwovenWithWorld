using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

public class TestDataCenter : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {


        List<Archive> allArchives = DataCenter.Instance.GetAllArchive();

        //不存在id为1的存档就来一份吧~
        if (allArchives.All(a => a.ID != 1))
        {
            DataCenter.Instance.Save(1, "firstTest", "testIntro");
        }

        //加载存档1
        DataCenter.Instance.LoadArchive(1);

        //测试任务
        MetaTasksData metaTaskDataSystem = DataCenter.Instance.GetMetaData<MetaTasksData>();


        RuntimeTasksData runtimeTaskData = DataCenter.Instance.GetEntity<RuntimeTasksData>();

        List<RunTimeTaskInfo> allCanDotask = runtimeTaskData.GetAllToDoList();

        RunTimeTaskInfo firstTask = allCanDotask[0];

        //根据runtimetask的id去metaTask里拿Task的所有信息
        MetaTaskInfo firstTaskMetaInfo = metaTaskDataSystem[firstTask.ID];
        Debug.Log(firstTaskMetaInfo.MetaTaskNode.TaskLocation);
        Debug.Log(firstTaskMetaInfo.MetaTaskNode.TaskTitile);
        Debug.Log(firstTaskMetaInfo.MetaTaskNode.TaskType);

        //******任务具体逻辑************

        //任务完成
        firstTask.IsOver = true;
        //获取新任务继续
        var newTaskList = runtimeTaskData.GetAllToDoList();

        //获取按键数据
        KeyContactData keyContactData = DataCenter.Instance.GetEntity<KeyContactData>();
        keyContactData.SetKeyContactStruct(1, new KeyContactStruct() { id = 1, key = 2, keyContactType = EnumKeyContactType.Action });
        var keyDataStructs = keyContactData.GetKeyContactStruct(1);

        //获取 PlayerState
        PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
        playerState.Agility = 1;
        playerState.Concentration = 2;
        playerState.Level = 20;



        

        //写入数据
        DataCenter.Instance.Save(2, "test2", "test2");
        Debug.Log("存档成功");
    }



    // Update is called once per frame
    void Update()
    {

    }
}
