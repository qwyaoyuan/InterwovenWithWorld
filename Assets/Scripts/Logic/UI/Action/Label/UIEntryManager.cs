using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 词条展示界面
/// </summary>
public class UIEntryManager : MonoBehaviour
{
    /// <summary>
    /// 属性列表控件
    /// </summary>
    [SerializeField]
    UITree uiTree;

    /// <summary>
    /// 用于显示的父面板
    /// </summary>
    [SerializeField]
    RectTransform showPanel;

    /// <summary>
    /// 竖直滑动条
    /// </summary>
    [SerializeField]
    Scrollbar verticalScrollbar;

    /// <summary>
    /// 例子文本框
    /// </summary>
    [SerializeField]
    Text explanText;

    /// <summary>
    /// 例子图片
    /// </summary>
    [SerializeField]
    Image explanImage;

    /// <summary>
    /// 玩家存档状态
    /// </summary>
    PlayerState playerState;

    /// <summary>
    /// 词条原始数据
    /// </summary>
    EntryData entryData;

    /// <summary>
    /// 词条id对应节点字典
    /// </summary>
    Dictionary<int, UITreeNode> entryIDToNodeDic;

    private void Awake()
    {
        uiTree.SelectNodeChangedHandle += UiTree_SelectNodeChangedHandle;
    }

    private void OnEnable()
    {
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        entryData = DataCenter.Instance.GetMetaData<EntryData>();
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        UIManager.Instance.KeyPressHandle += Instance_KeyPressHandle;
        INowTaskState iNowTaskState = GameState.Instance.GetEntity<INowTaskState>();
        iNowTaskState.CheckNowTask(EnumCheckTaskType.Special, (int)TaskMap.Enums.EnumTaskSpecialCheck.OpenEntryUI);
        InitShow();
    }



    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        UIManager.Instance.KeyPressHandle -= Instance_KeyPressHandle;

    }

    /// <summary>
    /// 初始化显示
    /// </summary>
    private void InitShow()
    {
        entryIDToNodeDic = new Dictionary<int, UITreeNode>();
        uiTree.Clear();
        EntryDataInfo[] entryDataInfos = entryData.GetTops();//最上层节点
        if (entryDataInfos != null)
            foreach (EntryDataInfo entryDataInfo in entryDataInfos)
            {
                CreateNodeByEntryDataInfo(entryDataInfo);
            }
        uiTree.UpdateRenderer();
    }

    /// <summary>
    /// 通过数据创建节点
    /// </summary>
    /// <param name="target">数据</param>
    /// <param name="parent">父节点</param>
    private void CreateNodeByEntryDataInfo(EntryDataInfo target, UITreeNode parent = null)
    {
        UITreeNode uiTreeNode = uiTree.CreateTreeNode();
        if (parent == null)
            uiTree.AddTreeNode(uiTreeNode);
        else
            parent.Add(uiTreeNode);
        uiTreeNode.value = target;
        if (playerState.EntryEnableList.Contains(target.ID))
            uiTreeNode.ExplanText = target.Name;
        else uiTreeNode.ExplanText = "????????????????";
        uiTreeNode.IsDisplay = true;
        uiTreeNode.IsExpand = false;
        if (!entryIDToNodeDic.ContainsKey(target.ID))//添加到字典中
            entryIDToNodeDic.Add(target.ID, uiTreeNode);
        EntryDataInfo[] entryDataInfos = entryData.GetNexts(target);
        if (entryDataInfos != null)
            foreach (EntryDataInfo entryDataInfo in entryDataInfos)
            {
                CreateNodeByEntryDataInfo(entryDataInfo, uiTreeNode);
            }
    }

    /// <summary>
    /// 按键检测(松开)
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        switch (keyType)
        {
            case UIManager.KeyType.A://展开或收起节点
                if (uiTree.SelectNode != null)
                {
                    uiTree.SelectNode.IsExpand = !uiTree.SelectNode.IsExpand;
                }
                break;
            case UIManager.KeyType.UP:
                if (uiTree.SelectNode)
                    uiTree.SelectUPTreeNode();
                else if (uiTree.Count > 0) uiTree[0].IsSelect = true;
                break;
            case UIManager.KeyType.DOWN:
                if (uiTree.SelectNode)
                    uiTree.SelectDownTreeNode();
                else if (uiTree.Count > 0) uiTree[0].IsSelect = true;
                break;
        }
    }

    /// <summary>
    /// 按键检测(按住)
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyPressHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        switch (keyType)
        {
            case UIManager.KeyType.RIGHT_ROCKER:
                verticalScrollbar.value += rockValue.y * Time.deltaTime * 0.5f;
                break;
        }

    }

    /// <summary>
    /// 树节点选择项发生变化事件
    /// </summary>
    /// <param name="obj"></param>
    private void UiTree_SelectNodeChangedHandle(UITreeNode obj)
    {
        ShowNowSelectTreeNodeExplan();
    }

    /// <summary>
    /// 显示当前选择节点的说明
    /// </summary>
    private void ShowNowSelectTreeNodeExplan()
    {
        //删除之前的
        Transform[] childTranses = Enumerable.Range(0, showPanel.childCount).Select(temp => showPanel.GetChild(temp)).ToArray();
        foreach (var childTrans in childTranses)
        {
            GameObject.Destroy(childTrans.gameObject);
        }
        //显示现在的
        UITreeNode selectNode = uiTree.SelectNode;
        if (selectNode == null)
            return;
        EntryDataInfo entryDataInfo = selectNode.value as EntryDataInfo;
        if (entryDataInfo == null)
            return;
        if (!playerState.EntryEnableList.Contains(entryDataInfo.ID))
            return;
        foreach (EntryDataInfo.EntryValue entryValue in entryDataInfo.Datas)
        {
            switch (entryValue.EntryValueType)
            {
                case EntryDataInfo.EnumEntryValueType.Title:
                    AddTitle(entryValue.Data);
                    break;
                case EntryDataInfo.EnumEntryValueType.Text:
                    Text textValue = AddText(entryValue.Data);
                    LinkImageText linkImageText = textValue as LinkImageText;
                    if (linkImageText)
                    {
                        linkImageText.onHrefClick.AddListener(ClickLink);
                    }
                    break;
                case EntryDataInfo.EnumEntryValueType.Image:
                    AddSprite(entryValue.Data);
                    break;
            }
        }

    }

    /// <summary>
    /// 点击连接
    /// </summary>
    /// <param name="link"></param>
    private void ClickLink(string link)
    {
        int id;
        if (entryIDToNodeDic != null && int.TryParse(link, out id) && entryIDToNodeDic.ContainsKey(id))
        {
            Action<UITreeNode> SetTreeNodeExplan = null;
            SetTreeNodeExplan = (temp) =>
            {
                if (temp == null)
                    return;
                temp.IsExpand = true;
                SetTreeNodeExplan(temp.Parent);
            };
            UITreeNode uiTreeNode = entryIDToNodeDic[id];
            SetTreeNodeExplan(uiTreeNode);
            uiTreeNode.IsSelect = true;
        }
    }

    /// <summary>
    /// 初始化UI的宽和高
    /// </summary>
    /// <param name="targetRect">对象</param>
    /// <param name="height">高(如果为0则使用默认)</param>
    private void InitUIWidth(RectTransform targetRect, float height = 0)
    {
        float width = showPanel.rect.width;
        Rect tempRect = targetRect.rect;
        targetRect.anchorMin = new Vector2(0, 1);
        targetRect.anchorMax = new Vector2(0, 1);
        targetRect.offsetMin = new Vector2(0, height >= 0 ? -height : -targetRect.rect.height);
        targetRect.offsetMax = new Vector2(tempRect.width, 0);
    }

    /// <summary>
    /// 添加一个标题
    /// </summary>
    /// <param name="title"></param>
    private void AddTitle(string title)
    {
        GameObject createObj = GameObject.Instantiate(explanText.gameObject);
        createObj.SetActive(true);
        RectTransform rectTrans = createObj.GetComponent<RectTransform>();
        rectTrans.SetParent(showPanel);
        InitUIWidth(rectTrans);
        //设置文字
        Text text = createObj.GetComponent<Text>();
        text.alignment = TextAnchor.MiddleCenter;
        text.fontStyle = FontStyle.Bold;
        text.fontSize = 35;
        text.text = title;
    }

    /// <summary>
    /// 添加一个文本
    /// </summary>
    /// <param name="explan"></param>
    private Text AddText(string explan)
    {
        GameObject createObj = GameObject.Instantiate(explanText.gameObject);
        createObj.SetActive(true);
        RectTransform rectTrans = createObj.GetComponent<RectTransform>();
        rectTrans.SetParent(showPanel);
        InitUIWidth(rectTrans);
        //设置文字
        Text text = createObj.GetComponent<Text>();
        text.alignment = TextAnchor.MiddleLeft;
        text.fontSize = 30;
        text.text = explan;
        return text;
    }

    /// <summary>
    /// 添加一个精灵
    /// </summary>
    /// <param name="spriteName"></param>
    private void AddSprite(string spriteName)
    {
        Sprite sprite = SpriteManager.GetSrpite(spriteName);
        if (sprite == null)
            return;
        float bili = sprite.rect.height / sprite.rect.width;//高与宽的比例
        GameObject createObj = GameObject.Instantiate(explanImage.gameObject);
        createObj.SetActive(true);
        RectTransform rectTrans = createObj.GetComponent<RectTransform>();
        rectTrans.SetParent(showPanel);
        //根据宽高比设置高
        float height = bili * rectTrans.rect.width;
        InitUIWidth(rectTrans, height);
        //设置图片
        Image image = createObj.GetComponent<Image>();
        image.sprite = sprite;
    }


}

