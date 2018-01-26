using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 切换场景
/// </summary>
public class UIChangeScene : MonoBehaviour
{
    public static UIChangeScene Instance;

    /// <summary>
    /// 所有子物体
    /// </summary>
    public GameObject[] childObjs;
    /// <summary>
    /// 背景图片对象
    /// </summary>
    public Image BGImage;

    /// <summary>
    /// 进度条图片
    /// </summary>
    public Image ScheduleImage;

    /// <summary>
    /// 进度条文字
    /// </summary>
    public Text ScheduleText;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

    }


    /// <summary>
    /// 加载场景携程
    /// </summary>
    Coroutine loadSceneCoroutine;

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="LoadResult"></param>
    public void LoadScene(string sceneName, Action<bool> LoadResult = null)
    {
        if (loadSceneCoroutine == null)
        {
            SetObjState(true);
            loadSceneCoroutine = StartCoroutine(LoadSceneAsync(sceneName,
                 (result) =>
                 {
                     loadSceneCoroutine = null;
                     if (LoadResult != null)
                         try
                         {
                             LoadResult(result);
                         }
                         catch (Exception ex)
                         {
                             Debug.Log(ex);
                         }
                     SetObjState(false);
                 }
             ));
        }
    }

    /// <summary>
    /// 移动对象携程
    /// </summary>
    Coroutine movePlayerCoroutine;

    /// <summary>
    /// 移动角色
    /// </summary>
    /// <param name="waitTime">等待时间</param>
    /// <param name="MoveResult"></param>
    public void MovePlayer(float waitTime, Action<bool> MoveResult = null)
    {
        if (movePlayerCoroutine != null)
        {
            StopCoroutine(movePlayerCoroutine);
            movePlayerCoroutine = null;
        }
        SetObjState(true);
        movePlayerCoroutine = StartCoroutine(MovePlayerAsync(waitTime,
            (result) =>
            {
                movePlayerCoroutine = null;
                if (MoveResult != null)
                    try
                    {
                        MoveResult(result);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex);
                    }
                SetObjState(false);
            }));
    }

    /// <summary>
    /// 设置对象的显示状态
    /// </summary>
    /// <param name="state"></param>
    private void SetObjState(bool state)
    {
        foreach (GameObject chidObj in childObjs)
        {
            if (chidObj)
                chidObj.SetActive(state);
        }
    }

    /// <summary>
    /// 使用携程加载场景
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <returns></returns>
    IEnumerator LoadSceneAsync(string sceneName, Action<bool> LoadResult = null)
    {
        AsyncOperation asyncOperation = null;
        try
        {
            asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        }
        catch
        {
            asyncOperation = null;
        }
        while (asyncOperation != null && !asyncOperation.isDone)
        {
            ScheduleImage.fillAmount = asyncOperation.progress;
            ScheduleText.text = ((int)(asyncOperation.progress * 100)).ToString();
            yield return null;
        }
        if (LoadResult != null)
            LoadResult(asyncOperation != null);
    }

    /// <summary>
    /// 使用携程移动对象
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="MoveResult"></param>
    /// <returns></returns>
    IEnumerator MovePlayerAsync(float waitTime, Action<bool> MoveResult = null)
    {
        float nowTime = 0;
        while (nowTime < waitTime)
        {
            ScheduleImage.fillAmount = waitTime > 0 ? (nowTime / waitTime) : 1;
            ScheduleText.text = ((int)(ScheduleImage.fillAmount * 100)).ToString();
            yield return null;
            nowTime += Time.deltaTime;
        }
        if (MoveResult != null)
            MoveResult(true);
    }

}
