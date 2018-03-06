using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 保存与退出处理
/// </summary>
public class UIDataManager : MonoBehaviour
{
    /// <summary>
    /// 标签路径
    /// </summary>
    UIFocusPath uiFocusPath;
    /// <summary>
    /// 当前选中的对象
    /// </summary>
    UIFocus nowUIFocus;

    /// <summary>
    /// 保存中图片
    /// </summary>
    public Image saveingImage;

    /// <summary>
    /// 开始时第一次的按键抬起(此时不可以使用)
    /// </summary>
    bool fisrtKeyUP;


    private void OnEnable()
    {
        fisrtKeyUP = false;
        uiFocusPath = GetComponent<UIFocusPath>();
        nowUIFocus = uiFocusPath.GetFirstFocus();
        saveingImage.gameObject.SetActive(false);
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
    }

    private void OnDisable()
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
        if (!fisrtKeyUP)
        {
            fisrtKeyUP = true;
            return;
        }
        if (UIAction.isSaving)
            return;
        Action<UIFocusPath.MoveType> MoveNextEndAction = (moveType) =>
        {
            if (nowUIFocus)
            {
                UIFocus uiFocus = uiFocusPath.GetNewNextFocus(nowUIFocus, moveType);
                if (uiFocus)
                {
                    nowUIFocus.LostForcus();
                    nowUIFocus = uiFocus;
                    nowUIFocus.SetForcus();
                }
            }
            else
            {
                nowUIFocus = uiFocusPath.GetFirstFocus();
                nowUIFocus.SetForcus();
            }
        };
        switch (keyType)
        {
            case UIManager.KeyType.A:
                UIFocusButton uiFocusButton = nowUIFocus as UIFocusButton;
                if (uiFocusButton)
                {
                    uiFocusButton.ClickThisButton();
                }
                break;
            case UIManager.KeyType.UP:
                MoveNextEndAction(UIFocusPath.MoveType.UP);
                break;
            case UIManager.KeyType.DOWN:
                MoveNextEndAction(UIFocusPath.MoveType.DOWN);
                break;
        }
    }

    /// <summary>
    /// 按键功能
    /// </summary>
    private void ButtonAction()
    {
        if (nowUIFocus)
        {
            switch (nowUIFocus.Tag)
            {
                case "Save":
                    //保存
                    SavingData();
                    break;
                case "Exit":
                    Application.Quit();//退出
                    break;
            }
        }
    }

    /// <summary>
    /// 保存当前数据
    /// </summary>
    private void SavingData()
    {
        StartCoroutine("SavingEnumerator");
    }

    /// <summary>
    /// 保存数据的携程
    /// </summary>
    /// <returns></returns>
    IEnumerator SavingEnumerator()
    {
        UIAction.isSaving = true;
        saveingImage.gameObject.SetActive(true);
        float savingTime = 0;
        IGameState iGameState = GameState.Instance.GetEntity<IGameState>();
        IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
        playerState.Scene = iGameState.SceneName;
        playerState.Location = iPlayerState.PlayerObj.transform.position + Vector3.up * 0.2f;
        //保存原来地形的遮罩图
        if (!string.IsNullOrEmpty(iGameState.SceneName))
            playerState.SaveGetSceneMapMaskData(iGameState.SceneName);
        DataCenter.Instance.Save(1);
        while (savingTime < 3)
        {
            savingTime += Time.deltaTime;
            yield return null;
            saveingImage.transform.localEulerAngles = new Vector3
                (
                saveingImage.transform.localEulerAngles.x,
                saveingImage.transform.localEulerAngles.y,
                saveingImage.transform.localEulerAngles.z + 180 * Time.deltaTime
                );
        }
        UIAction.isSaving = false;
        saveingImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 点击按钮
    /// </summary>
    /// <param name="uiFocus"></param>
    public void ClickButtong(UIFocus uiFocus)
    {
        if (UIAction.isSaving)
            return;
        if (nowUIFocus != uiFocus)
        {
            if (nowUIFocus != null)
                nowUIFocus.LostForcus();
            nowUIFocus = uiFocus;
            nowUIFocus.SetForcus();
        }
        ButtonAction();
    }
}
