using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDataCenter : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        List<Archive> allArchives = DataCenter.Instance.GetAllArchive();
        Debug.Log("所有存档为：");
        if (allArchives.Count == 0)
        {
            Debug.Log("暂无任何存档");
        }
        else
        {
            int i = 0;
            foreach (var item in allArchives)
            {
                Debug.Log(item);
                if (i == 0)
                    DataCenter.Instance.LoadArchive(item.ID);
                i++;
            }
        }

        //测试元数据
        Debug.Log("所有任务为：");
        Tasks TaskSystem = DataCenter.Instance.GetMetaData<Tasks>();

        foreach (var item in TaskSystem.GetAllToDoList())
        {
            Debug.Log(item.ID);
        }

        //设置玩家数据

        //PlayerState playState = DataCenter.Instance.GetEntity<PlayerState>();
        //playState.BloodVolume = 100;
        //playState.Level = 20;
        //playState.RoleOfRace = RoleOfRace.Amnesiac;



        //写入数据
        DataCenter.Instance.Save(1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
