using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 设置事件系统
/// </summary>
public class EventSystemSetting : MonoBehaviour {

    StandaloneInputModule standaloneInputModule;

    private void Awake()
    {
        standaloneInputModule = GetComponent<StandaloneInputModule>();
    }

    private void OnEnable()
    {
        Invoke("DisIt", 1);
    }

    void DisIt()
    {
        if (standaloneInputModule != null)
        {
            standaloneInputModule.enabled = false;
        }
    }
}
