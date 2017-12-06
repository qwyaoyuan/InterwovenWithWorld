using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主界面的UI
/// </summary>
public class UIMain : MonoBehaviour
{
    /// <summary>
    /// 主线过渡对话面板
    /// </summary>
    [SerializeField]
    GameObject interludePanel;
    /// <summary>
    /// 支线分支过渡对话面板
    /// </summary>
    [SerializeField]
    GameObject queryPanel;

    /// <summary>
    /// 合成面板
    /// </summary>
    [SerializeField]
    GameObject synthesisPanel;

    /// <summary>
    /// 设置面板预设提
    /// </summary>
    [SerializeField]
    GameObject settingPanelPrefab;

    /// <summary>
    /// 设置面板
    /// </summary>
    Canvas settingPanel;

    /// <summary>
    /// 交互对象
    /// </summary>
    IInteractiveState iInteractiveState;

    private void Start()
    {
        iInteractiveState = GameState.Instance.GetEntity<IInteractiveState>();
        iInteractiveState.InterludeObj = interludePanel;
        iInteractiveState.QueryObj = queryPanel;

        GameObject settingPanelObj = GameObject.Instantiate<GameObject>(settingPanelPrefab);
        settingPanel = settingPanelObj.GetComponent<Canvas>();
        settingPanel.gameObject.SetActive(false);
    }

}
