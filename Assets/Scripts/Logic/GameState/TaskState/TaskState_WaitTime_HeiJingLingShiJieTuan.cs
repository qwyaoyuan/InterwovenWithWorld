using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑精灵使节团-计时对象
/// </summary>
public class TaskState_WaitTime_HeiJingLingShiJieTuan : MonoBehaviour
{

	
	void Start ()
    {
        GameState.Instance.Registor<INowTaskState>(INowTaskStateChanged);
	}

    private void INowTaskStateChanged(INowTaskState iNowTaskState, string fieldName)
    {
     
    }

    void Update ()
    {
		
	}
}
