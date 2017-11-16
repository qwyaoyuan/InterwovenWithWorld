using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI树控件
/// </summary>
public class UITree : MonoBehaviour
{
    /// <summary>
    /// 渲染窗体
    /// </summary>
    [SerializeField]
    RectTransform rendererPanel;
    /// <summary>
    /// 样例节点(用于生成节点)
    /// </summary>
    [SerializeField]
    GameObject exampleNode;

    /// <summary>
    /// 缩进像素
    /// </summary>
    public float indent = 20;

    /// <summary>
    /// 根节点集合
    /// </summary>
    List<UITreeNode> roots;

    /// <summary>
    /// 所有树节点的一个集合
    /// </summary>
    List<UITreeNode> allTreeNode;

    /// <summary>
    /// 需要更形渲染
    /// </summary>
    int mustUpdateRenderer;

    /// <summary>
    /// 当前选择的节点
    /// </summary>
    UITreeNode selectNode;

    private void Awake()
    {
        roots = new List<UITreeNode>();
        allTreeNode = new List<UITreeNode>();
    }

    private void Start()
    {
        UITreeNode uiTreeNode = CreateTreeNode();
        AddTreeNode(uiTreeNode);
        uiTreeNode.IsDisplay = true;
        uiTreeNode.IsExpand = false;
        UITreeNode uitreeNode1 = CreateTreeNode();
        uiTreeNode.Add(uitreeNode1);
        uitreeNode1.IsDisplay = true;
        UpdateRenderer();
    }

    private void LateUpdate()
    {
        if (mustUpdateRenderer > 0)
        {
            mustUpdateRenderer--;
            _UpdateRenderer();
        }
    }

    /// <summary>
    /// 更新面板(调用后不会立即更新会在本次调用的LateUpdate函数中更新)
    /// </summary>
    public void UpdateRenderer()
    {
        mustUpdateRenderer = 2;
    }

    /// <summary>
    /// 更新渲染
    /// </summary>
    private void _UpdateRenderer()
    {
        roots.RemoveAll(temp => temp == null);//清空空项
        #region 设置显示或隐藏项
        Action<UITreeNode, bool> SetNodeDisplay = null;
        SetNodeDisplay = (parentNode, display) =>
         {
             parentNode.gameObject.SetActive(false);
             foreach (var item in parentNode)
             {
                 SetNodeDisplay(item, display);
             }
         };
        Action<UITreeNode> CheckAction = null;
        CheckAction = (parentNode) =>
        {
            if (parentNode.IsDisplay)
            {
                parentNode.gameObject.SetActive(true);
                if (parentNode.IsExpand)
                {
                    foreach (var item in parentNode)
                    {
                        CheckAction(item);
                    }
                }
                else
                    foreach (var item in parentNode)
                    {
                        SetNodeDisplay(item, false);
                    }
            }
            else
                SetNodeDisplay(parentNode, false);
        };
        foreach (UITreeNode root in roots)
        {
            CheckAction(root);
        }
        #endregion
        #region 将所有的对象的大小更新一下 
        Action<UITreeNode> UpdateNodeSize = null;
        UpdateNodeSize = (parentNode) =>
        {
            if (parentNode.gameObject.activeSelf)
            {
                HorizontalLayoutGroup horizonTalLayoutGroup = parentNode.GetComponent<HorizontalLayoutGroup>();
                if (horizonTalLayoutGroup)
                {
                    horizonTalLayoutGroup.SetLayoutHorizontal();
                }
                ContentSizeFitter contentSizeFitter = parentNode.GetComponent<ContentSizeFitter>();
                contentSizeFitter.SetLayoutHorizontal();
                contentSizeFitter.SetLayoutVertical();
                foreach (var item in parentNode)
                {
                    UpdateNodeSize(item);
                }
            }
        };
        foreach (UITreeNode root in roots)
        {
            UpdateNodeSize(root);
        }
        #endregion
        #region 设置位置
        float xPos = 0;
        float yPos = 0;
        float maxXPos = 0;
        Action<UITreeNode> SetNodePos = null;
        SetNodePos = (parentNode) =>
        {
            if (parentNode.IsDisplay)
            {
                //设置自身位置
                RectTransform rectTransform = parentNode.GetComponent<RectTransform>();
                rectTransform.localPosition = new Vector2(xPos, -yPos);
                float thisWidth = rectTransform.rect.width;
                if (xPos + thisWidth > maxXPos)
                    maxXPos = xPos + thisWidth;
                yPos += rectTransform.rect.height;
                //设置子节点位置 
                if (parentNode.IsExpand)
                {
                    xPos += indent;
                    foreach (var item in parentNode)
                    {
                        try
                        {
                            SetNodePos(item);
                        }
                        catch { }
                    }
                    xPos -= indent;
                }
            }
        };
        foreach (UITreeNode root in roots)
        {
            SetNodePos(root);
        }
        Vector2 offsetMin = rendererPanel.offsetMin;
        Vector2 offsetMax = rendererPanel.offsetMax;
        float width = rendererPanel.rect.width;
        float height = rendererPanel.rect.height;
        float offsetWidth = maxXPos - width;
        float offsetHeight = yPos - height;
        offsetMax.x += offsetWidth;
        offsetMin.y -= offsetHeight;
        rendererPanel.offsetMin = offsetMin;
        rendererPanel.offsetMax = offsetMax;
        #endregion
    }

    /// <summary>
    /// 创建一个树节点
    /// </summary>
    /// <returns></returns>
    public UITreeNode CreateTreeNode()
    {
        GameObject createNodeObj = Instantiate(exampleNode);
        UITreeNode createNode = createNodeObj.GetComponent<UITreeNode>();
        allTreeNode.Add(createNode);
        createNode.DeleteNodeHandle += (uiTreeNode) =>
        {
            allTreeNode.Remove(uiTreeNode);
        };
        createNode.SelectNodeHandle += (uiTreeNode) => 
        {
            if (selectNode && !object.Equals(selectNode, uiTreeNode))
            {
                selectNode.IsSelect = false;
            }
            selectNode = uiTreeNode;
        };
        createNode.StateChangedHandle += () => 
        {
            UpdateRenderer();
        };
        createNodeObj.transform.SetParent(rendererPanel);
        createNodeObj.SetActive(false);
        return createNode;
    }

    /// <summary>
    /// 移除指定位置的项,并返回移除的项 
    /// 该方法不会删除子节点
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public UITreeNode RemoveAt(int index)
    {
        UITreeNode removeTarget = roots[index];
        roots.RemoveAt(index);
        return removeTarget;
    }

    /// <summary>
    /// 移除节点
    /// 注意该操作不会删除节点
    /// </summary>
    /// <param name="uiTreeNode"></param>
    public bool RemoveTreeNode(UITreeNode uiTreeNode)
    {
        if (roots.Contains(uiTreeNode))
            return roots.Remove(uiTreeNode);
        return false;
    }

    public int Count { get { return roots.Count; } }

    /// <summary>
    /// 添加节点
    /// </summary>
    /// <param name="uiTreeNode"></param>
    public void AddTreeNode(UITreeNode uiTreeNode)
    {
        if (!roots.Contains(uiTreeNode))
        {
            roots.Add(uiTreeNode);
            uiTreeNode.Parent = null;
        }
    }

    /// <summary>
    /// 将指定项插入到指定位置
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    public void Insert(int index, UITreeNode item)
    {
        roots.Insert(index, item);
        item.Parent = null;
    }

    /// <summary>
    /// 获取指定索引的项
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public UITreeNode this[int index]
    {
        get
        {
            if (roots.Count < index && index >= 0)
            {
                return roots[index];
            }
            throw new Exception("下标超出数组范围");
        }
    }

    /// <summary>
    /// 清空所有子项 
    ///  注意该方法会删除子节点
    /// </summary>
    public void Clear()
    {
        foreach (var item in roots)
        {
            item.DeleteObject();
        }
        roots.Clear();
        allTreeNode.Clear();
    }

}
