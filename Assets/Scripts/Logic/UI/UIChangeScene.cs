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
            LoadResult(asyncOperation!=null);
    }

}
