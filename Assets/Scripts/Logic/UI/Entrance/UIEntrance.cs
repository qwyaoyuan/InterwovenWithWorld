using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 入口界面UI
/// </summary>
public class UIEntrance : MonoBehaviour
{
    /// <summary>
    /// 继续游戏
    /// </summary>
    public RectTransform continueRect;
    /// <summary>
    /// 新游戏
    /// </summary>
    public RectTransform newGameRect;
    /// <summary>
    /// 设置
    /// </summary>
    public RectTransform optionRect;

    /// <summary>
    /// 遮罩图片
    /// </summary>
    public Image maskImage;
    /// <summary>
    /// 过渡时间
    /// </summary>
    public float crossoverTime;

    /// <summary>
    /// 设置面板
    /// </summary>
    private Canvas settingCanvas;

    /// <summary>
    /// ui路径
    /// </summary>
    UIFocusPath uiFocusPath;
    /// <summary>
    /// 当前的焦点
    /// </summary>
    UIFocus nowFocus;
    /// <summary>
    /// 自身是否可以使用输入
    /// </summary>
    private bool CanGetInput
    {
        get
        {
            if (settingCanvas)
            {
                return !settingCanvas.gameObject.activeSelf;
            }
            else return true;
        }
    }

    void Start()
    {
        uiFocusPath = GetComponent<UIFocusPath>();
        GameObject settingCanvasObj = GameObject.Find("SettingCanvas");
        if (settingCanvasObj)
        {
            settingCanvas = settingCanvas.GetComponent<Canvas>();
        }
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        StartCoroutine(CrossoverMaskImageAlpha(1));

        if (nowFocus == null && uiFocusPath)
        {
            nowFocus = uiFocusPath.GetFirstFocus();
            if (nowFocus)
                nowFocus.SetForcus();
        }
    }

    private void Update()
    {
        if (!CanGetInput)
            return;

        if (nowFocus == null && uiFocusPath)
        {
            nowFocus = uiFocusPath.GetFirstFocus();
            if (nowFocus)
                nowFocus.SetForcus();
        }

        if (uiFocusPath)
        {
            Action<UIFocusPath.MoveType> ThisAction = (moveType) =>
            {
                UIFocus next = uiFocusPath.GetNextFocus(nowFocus, moveType, true);
                if (next)
                {
                    nowFocus = next;
                    nowFocus.SetForcus();
                }
            };
            if (Input.GetKeyUp(KeyCode.A))
                ThisAction(UIFocusPath.MoveType.LEFT);
            if (Input.GetKeyUp(KeyCode.W))
                ThisAction(UIFocusPath.MoveType.UP);
            if (Input.GetKeyUp(KeyCode.D))
                ThisAction(UIFocusPath.MoveType.RIGHT);
            if (Input.GetKeyUp(KeyCode.S))
                ThisAction(UIFocusPath.MoveType.DOWN);
            if (Input.GetKeyUp(KeyCode.X))
            {
                if (nowFocus)
                {
                    Button button = nowFocus.GetComponent<Button>();
                    button.onClick.Invoke();
                }
            }
        }
    }

    void OnDestroy()
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
        if (!CanGetInput)
            return;
        if (nowFocus == null && uiFocusPath)
        {
            nowFocus = uiFocusPath.GetFirstFocus();
            if (nowFocus)
                nowFocus.SetForcus();
        }
        if (uiFocusPath)
        {
            //判断键位
            Action<UIFocusPath.MoveType> MoveFocusAction = (moveType) =>
            {
                UIFocus next = uiFocusPath.GetNextFocus(nowFocus, moveType, true);
                if (next)
                {
                    nowFocus = next;
                    nowFocus.SetForcus();
                }
            };
            switch (keyType)
            {
                case UIManager.KeyType.A:
                    if (nowFocus)
                    {
                        Button nowButton = nowFocus.GetComponent<Button>();
                        if (nowButton)
                        {
                            nowButton.onClick.Invoke();
                        }
                    }
                    break;
                case UIManager.KeyType.LEFT:
                    MoveFocusAction(UIFocusPath.MoveType.LEFT);
                    break;
                case UIManager.KeyType.RIGHT:
                    MoveFocusAction(UIFocusPath.MoveType.RIGHT);
                    break;
                case UIManager.KeyType.UP:
                    MoveFocusAction(UIFocusPath.MoveType.UP);
                    break;
                case UIManager.KeyType.DOWN:
                    MoveFocusAction(UIFocusPath.MoveType.DOWN);
                    break;
            }
        }
    }

    /// <summary>
    /// 过渡携程
    /// </summary>
    /// <param name="value">要过渡到的值</param>
    /// <param name="callBack">回调</param>
    /// <returns></returns>
    IEnumerator CrossoverMaskImageAlpha(int value, Action callBack = null)
    {
        value = Mathf.Clamp(value, 0, 1);
        float startValue = maskImage.color.a;
        float offset = value - startValue;
        if (crossoverTime > 0)
        {
            float interval = offset / crossoverTime;
            Color nowColor = maskImage.color;
            float tempCrossoverTime = crossoverTime;
            while ((tempCrossoverTime -= Time.deltaTime) >= 0)
            {
                nowColor.a += interval;
                maskImage.color = nowColor;
                yield return null;
            }
            nowColor.a = value;
            maskImage.color = nowColor;
        }
        if (callBack != null)
            callBack();
    }

    /// <summary>
    /// 切换到指定场景
    /// </summary>
    /// <param name="sceneName">场景名</param>
    private void ChangeScene(string sceneName)
    {
        StartCoroutine(CrossoverMaskImageAlpha(0, () => { }));
    }

    #region ui上的事件
    /// <summary>
    /// 继续按钮按下事件
    /// </summary>
    public void ContinueButtonClick()
    {
        //切换场景
        nowFocus = EventSystem.current.currentSelectedGameObject.GetComponent<UIFocus>();
    }
    /// <summary>
    /// 新游戏按钮按下事件
    /// </summary>
    public void NewGameButtonClick()
    {
        //切换场景
        nowFocus = EventSystem.current.currentSelectedGameObject.GetComponent<UIFocus>();
    }
    /// <summary>
    /// 设置按钮按下事件
    /// </summary>
    public void SettingButtonClick()
    {
        nowFocus = EventSystem.current.currentSelectedGameObject.GetComponent<UIFocus>();
        //显示设置面板
        if (settingCanvas)
        {
            settingCanvas.gameObject.SetActive(true);
        }
    }
    #endregion
}
