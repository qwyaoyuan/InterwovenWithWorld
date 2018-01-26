using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// npc对话的控制
/// </summary>
public class UITalkShowControl : MonoBehaviour
{
    /// <summary>
    /// 例子对象(包含一个面板,面板内有一个text对象)
    /// </summary>
    public GameObject ExplanObj;

    /// <summary>
    /// 显示文本游戏对象结构集合
    /// </summary>
    List<TalkShowObjStruct> talkShowObjStructList;

    /// <summary>
    /// 自身的canvas
    /// </summary>
    Canvas thisCanvas;

    /// <summary>
    /// 背景面板
    /// </summary>
    RectTransform backRect;

    private void Awake()
    {
        talkShowObjStructList = new List<TalkShowObjStruct>();
        thisCanvas = GetComponent<Canvas>();
        backRect = transform.GetChild(0).GetComponent<RectTransform>();
        thisCanvas.worldCamera = Camera.main;
    }

    private void Update()
    {
        float nowHeight = 0;
        //更新位置
        for (int i = 0; i < talkShowObjStructList.Count; i++)
        {
            TalkShowObjStruct talkShowObjStruct = talkShowObjStructList[i];
            talkShowObjStruct.TimeRemaining -= Time.deltaTime;
            if (talkShowObjStruct.TimeRemaining > 1)
            {
                if (talkShowObjStruct.CanvasGroup.alpha < 1)
                    talkShowObjStruct.CanvasGroup.alpha += Time.deltaTime;
            }
            else if (talkShowObjStruct.TimeRemaining >= 0)
                talkShowObjStruct.CanvasGroup.alpha = talkShowObjStruct.TimeRemaining;
            else
            {
                GameObject.Destroy(talkShowObjStruct.Panel.gameObject);
                talkShowObjStructList.Remove(talkShowObjStruct);
                continue;
            }
            //查看此时的text高度
            float tempHeight = talkShowObjStruct.Text.GetComponent<RectTransform>().rect.height;
            //不需要设置面板高度,因为面板是被上层控制大小的
            nowHeight += tempHeight + 15;
        }
        //Rect canvasRect = thisCanvas.pixelRect;
        //canvasRect.height = nowHeight;
        RectTransform rectTrans = thisCanvas.GetComponent<RectTransform>();
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, nowHeight);
        //根据摄像机更新方向
        Camera mainCamera = Camera.main;
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        thisCanvas.transform.forward = cameraForward;
    }

    /// <summary>
    /// 添加新的对话
    /// </summary>
    /// <param name="talkValue">对话内容</param>
    /// <param name="timeDuration">对话的持续时间</param>
    public void AddNewTalk(string talkValue, float timeDuration)
    {
        if (talkShowObjStructList == null || thisCanvas == null || ExplanObj == null)
            return;
        GameObject createObj = GameObject.Instantiate<GameObject>(ExplanObj);
        createObj.transform.SetParent(backRect);
        createObj.SetActive(true);
        RectTransform panel = createObj.GetComponent<RectTransform>();
        panel.localScale = Vector3.one;
        panel.localEulerAngles = Vector2.zero;
        panel.localPosition = new Vector3(panel.localPosition.x, panel.localPosition.y, 0);
        Text text = panel.GetChild(1).GetComponent<Text>();
        text.text = talkValue;
        TalkShowObjStruct talkShowObjStruct = new TalkShowObjStruct()
        {
            TimeRemaining = timeDuration,
            Panel = panel,
            Text = text,
            CanvasGroup = panel.GetComponent<CanvasGroup>()
        };
        talkShowObjStructList.Add(talkShowObjStruct);
    }

    /// <summary>
    /// 对话显示的游戏对象结构
    /// </summary>
    class TalkShowObjStruct
    {
        /// <summary>
        /// 剩余时间
        /// </summary>
        public float TimeRemaining;
        /// <summary>
        /// 面板对象
        /// </summary>
        public RectTransform Panel;
        /// <summary>
        /// 文本对象
        /// </summary>
        public Text Text;
        /// <summary>
        /// 用于整体显示或隐藏的对象
        /// </summary>
        public CanvasGroup CanvasGroup;
    }
}

