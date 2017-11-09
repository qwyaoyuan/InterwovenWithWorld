using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 开始界面开始等待时的UI
/// </summary>
public class UIStartWait : MonoBehaviour
{

    /// <summary>
    /// 依次播放的对象数据
    /// </summary>
    [SerializeField]
    public StartPlayData[] startPlayDatas;

    /// <summary>
    /// 当播放完成后实例化的UI
    /// </summary>
    public GameObject nextUIPrefab;

    /// <summary>
    /// 当前播放的下标
    /// </summary>
    int playCurrent;

    /// <summary>
    /// 播放的Image
    /// </summary>
    public Image playImage;

    /// <summary>
    /// 过度时间
    /// </summary>
    public float crossoverTime;

    /// <summary>
    /// 是否可以跳过
    /// </summary>
    bool canPause;

    /// <summary>
    /// 音频播放组件
    /// </summary>
    AudioSource audioSource;

    void Start()
    {
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// 检测按键
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType arg1, Vector2 arg2)
    {
        if (canPause)
        {
            ++playCurrent;
            SetPlayCurrent(playCurrent);
            canPause = false;
        }
    }

    void OnDestroy()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
    }

    void OnEnable()
    {
        SetPlayCurrent(0);
    }

    /// <summary>
    /// 设置播放的下标
    /// </summary>
    /// <param name="current"></param>
    private void SetPlayCurrent(int current)
    {
        StopCoroutine("Crossover");
        StopCoroutine("Play");
        playCurrent = current;
        StartCoroutine("Crossover");
    }

    /// <summary>
    /// 播放
    /// </summary>
    /// <returns></returns>
    IEnumerator Play()
    {
        int index = 0;
        StartPlayData current = startPlayDatas[playCurrent];
        if (audioSource != null && current.audioClip != null)
        {
            audioSource.PlayOneShot(current.audioClip);
        }
        float waitTime = current.playTime;
        while ((waitTime -= Time.deltaTime) > 0)
        {
            if (playImage)
            {
                if (current.sprites != null)
                {
                    if (current.sprites.Length > index)
                        playImage.sprite = current.sprites[index];
                    else playImage.sprite = current.sprites[current.sprites.Length - 1];
                }
            }
            index++;
            yield return null;
        }
        playCurrent++;
        StopCoroutine("Crossover");
        StartCoroutine("Crossover");
    }

    /// <summary>
    /// 过渡
    /// </summary>
    /// <returns></returns>
    IEnumerator Crossover()
    {
        float halfTime = crossoverTime / 2;
        Color defaultColor = playImage == null ? Color.white : playImage.color;
        float offset = 1 / halfTime;
        while ((halfTime -= Time.deltaTime) > 0 && (defaultColor.a > 0.001))//渐隐
        {
            float alpha = defaultColor.a - offset * Time.deltaTime;
            alpha = Mathf.Clamp(alpha, 0, 1);
            defaultColor.a = alpha;
            if (playImage)
                playImage.color = defaultColor;
            if (audioSource)
                audioSource.volume = alpha;
            yield return null;
        }
        //判断是否存在当前帧
        if (playCurrent >= startPlayDatas.Length)
        {
            //实例化新的UI并销毁自己
            if (nextUIPrefab)
                Instantiate(nextUIPrefab);
            DestroyImmediate(transform.root != null ? transform.root.gameObject : transform.gameObject);
        }
        else//开启播放
        {
            StopCoroutine("Play");
            StartCoroutine("Play");
            //渐显
            halfTime = crossoverTime / 2;
            while ((halfTime += Time.deltaTime) > 0 && defaultColor.a < 0.999f)
            {
                float alpha = defaultColor.a + offset * Time.deltaTime;
                alpha = Mathf.Clamp(alpha, 0, 1);
                defaultColor.a = alpha;
                if (playImage)
                    playImage.color = defaultColor;
                if (audioSource)
                    audioSource.volume = alpha;
                yield return null;
            }
            //只有完全显示了才可以进行跳过
            canPause = true;
        }

    }

    /// <summary>
    /// 开始画面播放的动画
    /// </summary>
    [Serializable]
    public class StartPlayData
    {
        /// <summary>
        /// 精灵图片
        /// </summary>
        public Sprite[] sprites;

        /// <summary>
        /// 播放总时间
        /// </summary>
        public float playTime;

        /// <summary>
        /// 音频
        /// </summary>
        public AudioClip audioClip;
    }
}
