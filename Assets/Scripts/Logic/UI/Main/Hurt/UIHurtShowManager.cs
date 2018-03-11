using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 显示伤害管理器
/// </summary>
public class UIHurtShowManager : MonoBehaviour
{
    /// <summary>
    /// 例子预设体
    /// </summary>
    [SerializeField]
    private GameObject ExplanObjPrefab;

    /// <summary>
    /// 父控件
    /// </summary>
    [SerializeField]
    private RectTransform ParentRectTrans;

    /// <summary>
    /// Y轴移动对应时间的曲线
    /// </summary>
    [SerializeField]
    private AnimationCurve YMoveToTimeCurve;
    /// <summary>
    /// Y轴移动曲线的比例
    /// </summary>
    [SerializeField]
    private float YMoveScale;

    /// <summary>
    /// 字体大小对应时间的曲线
    /// </summary>
    [SerializeField]
    private AnimationCurve SizeToTimeCurve;
    /// <summary>
    /// 字体大小曲线的比例
    /// </summary>
    [SerializeField]
    private float SizeScale;

    /// <summary>
    /// 透明度对应时间的曲线
    /// </summary>
    [SerializeField]
    private AnimationCurve AphlaToTimeCurve;
    /// <summary>
    /// 透明度曲线的比例
    /// </summary>
    [SerializeField]
    private float AphlaScale;

    /// <summary>
    /// 显示的总时长
    /// </summary>
    [SerializeField]
    [Range(1, 5)]
    private float ShowAllTime = 3;

    /// <summary>
    /// 当前显示的结构
    /// </summary>
    Dictionary<GameObject, List<ShowData>> showListDic;

    IPlayerState iPlayerState;

    private void Start()
    {
        if (ShowAllTime <= 0)
            ShowAllTime = 3;
        showListDic = new Dictionary<GameObject, List<ShowData>>();
        if (ParentRectTrans == null)
            ParentRectTrans = GetComponent<RectTransform>();
        iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        GameState.Instance.Registor<IGameState>(IGameState_Changed);
    }

    private void OnDisable()
    {
        GameState.Instance.UnRegistor<IGameState>(IGameState_Changed);
    }

    private void Update()
    {
        List<GameObject> mustRemoveList = new List<GameObject>();
        //首先移除
        foreach (var item in showListDic)
        {
            //判断需要移除到时的
            var tempDataArray = item.Value.ToArray();
            foreach (var tempData in tempDataArray)
            {
                if (tempData.time > ShowAllTime)
                {
                    GameObject.Destroy(tempData.showPanel.gameObject);
                    item.Value.Remove(tempData);
                }
            }
            //判断需要移除游戏对象已经消失的
            if (item.Key == null)
            {
                foreach (var tempData in item.Value)
                {
                    GameObject.Destroy(tempData.showPanel.gameObject);
                }
                item.Value.Clear();
            }
            //添加到列表中等待移除
            if (item.Value.Count == 0)
            {
                mustRemoveList.Add(item.Key);
            }
        }
        //具体的移除
        foreach (var item in mustRemoveList)
        {
            showListDic.Remove(item);
        }
        //然后创建
        if (ExplanObjPrefab != null && ParentRectTrans != null && iPlayerState != null && iPlayerState.PlayerCamera != null)
        {
            foreach (var item in showListDic)
            {
                foreach (var tempData in item.Value)
                {
                    if (tempData.showPanel == null && tempData.hurtFontStruct.TargetObj != null)
                    {
                        //换算屏幕坐标
                        Vector3 worldVec = tempData.hurtFontStruct.TargetObj.transform.position + Vector3.up * tempData.hurtFontStruct.Offset;
                        Vector3 sceneVec = iPlayerState.PlayerCamera.WorldToScreenPoint(worldVec);//屏幕坐标
                        Rect cameraRect = iPlayerState.PlayerCamera.pixelRect;//屏幕尺寸(左下角为0,0)
                        //换算anchor位置
                        Vector2 anchorVec = new Vector2(sceneVec.x / cameraRect.width, sceneVec.y / cameraRect.height);
                        tempData.startAnchor = anchorVec;
                        //创建
                        GameObject createObj = GameObject.Instantiate<GameObject>(ExplanObjPrefab);
                        createObj.SetActive(true);
                        //设置位置
                        createObj.transform.SetParent(ParentRectTrans);
                        RectTransform rectTrans = createObj.GetComponent<RectTransform>();
                        rectTrans.anchorMin = anchorVec;
                        rectTrans.anchorMax = anchorVec;
                        rectTrans.anchoredPosition = Vector2.zero;
                        tempData.showPanel = rectTrans;
                        //设置文字
                        Text text = createObj.GetComponent<Text>();
                        text.text = ((int)tempData.hurtFontStruct.Hurt).ToString();
                        tempData.text = text;
                        //设置颜色
                        if (tempData.hurtFontStruct.IsCrit)
                        {
                            text.color = Color.red;
                        }
                        else
                        {
                            text.color = Color.white;
                        }

                    }
                }
            }
        }
        //根据当前的时间设置状态
        foreach (var item in showListDic)
        {
            foreach (var tempData in item.Value)
            {
                if (tempData.showPanel == null || tempData.text == null || iPlayerState == null || iPlayerState.PlayerCamera == null)
                    continue;
                float nowTime = tempData.time / ShowAllTime;
                float yMoveValue = YMoveToTimeCurve.Evaluate(nowTime) * YMoveScale;
                float sizeValue = SizeToTimeCurve.Evaluate(nowTime) * SizeScale;
                float aphlaValue = AphlaToTimeCurve.Evaluate(nowTime) * AphlaScale;
                //换算屏幕坐标
                Vector3 worldVec = tempData.hurtFontStruct.TargetObj.transform.position + Vector3.up * tempData.hurtFontStruct.Offset;
                Vector3 sceneVec = iPlayerState.PlayerCamera.WorldToScreenPoint(worldVec);//屏幕坐标
                Rect cameraRect = iPlayerState.PlayerCamera.pixelRect;//屏幕尺寸(左下角为0,0)
                //换算anchor位置
                Vector2 anchorVec = new Vector2(sceneVec.x / cameraRect.width, sceneVec.y / cameraRect.height);
                tempData.startAnchor = anchorVec;
                tempData.showPanel.anchorMin = anchorVec;
                tempData.showPanel.anchorMax = anchorVec;
                tempData.showPanel.anchoredPosition = new Vector2(0, yMoveValue);
                tempData.text.fontSize = (int)sizeValue;
                tempData.text.color = new Color(tempData.text.color.r, tempData.text.color.g, tempData.text.color.b, aphlaValue);
            }
        }
        //设置时间进度
        foreach (var item in showListDic)
        {
            foreach (var tempData in item.Value)
            {
                tempData.time += Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// 游戏状态发生变化时触发(主要是显示伤害)
    /// </summary>
    /// <param name="iGameState"></param>
    /// <param name="fieldName"></param>
    private void IGameState_Changed(IGameState iGameState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.GetFieldNameStatic<IGameState, HurtFontStruct>(temp => temp.ShowHurtFont)))
        {
            HurtFontStruct hurtFontStruct = iGameState.ShowHurtFont;
            ShowData showData = new ShowData()
            {
                hurtFontStruct = hurtFontStruct
            };
            if (!showListDic.ContainsKey(hurtFontStruct.TargetObj))
                showListDic.Add(hurtFontStruct.TargetObj, new List<ShowData>());
            showListDic[hurtFontStruct.TargetObj].Add(showData);
        }
    }

    /// <summary>
    /// 显示结构
    /// </summary>
    public class ShowData
    {
        /// <summary>
        /// 数据
        /// </summary>
        public HurtFontStruct hurtFontStruct;

        /// <summary>
        /// 剩余时间
        /// </summary>
        public float time;

        /// <summary>
        /// 显示的面板
        /// </summary>
        public RectTransform showPanel;

        /// <summary>
        /// 创建时的其实锚点
        /// </summary>
        public Vector2 startAnchor;

        /// <summary>
        /// 显示文字的控件
        /// </summary>
        public Text text;
    }
}



