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
    /// 功能面板预设提
    /// </summary>
    [SerializeField]
    GameObject actionPanelPrefab;

    /// <summary>
    /// 设置面板
    /// </summary>
    Canvas settingPanel;

    /// <summary>
    /// 功能面板
    /// </summary>
    Canvas actionPanel;

    /// <summary>
    /// 交互对象
    /// </summary>
    IInteractiveState iInteractiveState;

    /// <summary>
    /// 游戏状态
    /// </summary>
    IGameState iGameState;

    private void Start()
    {
        iInteractiveState = GameState.Instance.GetEntity<IInteractiveState>();
        iInteractiveState.InterludeObj = interludePanel;
        iInteractiveState.QueryObj = queryPanel;

        iGameState = GameState.Instance.GetEntity<IGameState>();

        GameObject settingPanelObj = GameObject.Instantiate<GameObject>(settingPanelPrefab);
        settingPanel = settingPanelObj.GetComponent<Canvas>();
        settingPanel.gameObject.SetActive(false);

        GameObject actionPanelObj = GameObject.Instantiate<GameObject>(actionPanelPrefab);
        actionPanel = actionPanelObj.GetComponent<Canvas>();
        actionPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
    }

    /// <summary>
    /// 抬起按键
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType arg1, Vector2 arg2)
    {
        if (iGameState.GameRunType == EnumGameRunType.Safe ||
            iGameState.GameRunType == EnumGameRunType.Unsafa)
            switch (arg1)
            {
                case UIManager.KeyType.START://开启功能界面
                    actionPanel.gameObject.SetActive(true);
                    break;
                case UIManager.KeyType.Back://开启设置界面
                    settingPanel.gameObject.SetActive(false);
                    break;
            }
    }

    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
    }

}
