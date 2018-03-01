using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// UI树节点
/// </summary>
public class UITreeNode : MonoBehaviour, IEnumerable<UITreeNode>
{
    /// <summary>
    /// 展开时所用的图片
    /// </summary>
    [SerializeField]
    Image expandImage;
    /// <summary>
    /// 收起时所用的图片
    /// </summary>
    [SerializeField]
    Image packUpImage;
    /// <summary>
    /// 被选中时所用的图片
    /// </summary>
    [SerializeField]
    Image selectImage;
    /// <summary>
    /// 显示的文字
    /// </summary>
    [SerializeField]
    Text explanText;

    /// <summary>
    /// 父节点
    /// </summary>
    public UITreeNode Parent
    {
        get; set;
    }

    private List<UITreeNode> _childNodes;

    /// <summary>
    /// 子节点集合
    /// </summary>
    private List<UITreeNode> ChildNodes
    {
        get
        {
            if (_childNodes == null)
                _childNodes = new List<UITreeNode>();
            return _childNodes;
        }
        set { _childNodes = value; }
    }

    /// <summary>
    /// 删除节点(游戏对象)事件
    /// </summary>
    public event Action<UITreeNode> DeleteNodeHandle;
    /// <summary>
    /// 选择(取消选择)节点事件
    /// </summary>
    public event Action<UITreeNode> SelectNodeHandle;
    /// <summary>
    /// 状态发生变化事件 
    /// </summary>
    public event Action StateChangedHandle;

    /// <summary>
    /// 节点数据
    /// </summary>
    public object value;

    /// <summary>
    /// 设置或获取要显示的文字
    /// </summary>
    public string ExplanText
    {
        get { return explanText.text; }
        set { explanText.text = value; }
    }

    /// <summary>
    /// 是否显示
    /// </summary>
    private bool _isDisplay;
    /// <summary>
    /// 是否显示
    /// </summary>
    public bool IsDisplay
    {
        get { return _isDisplay; }
        set
        {
            bool oldState = _isDisplay;
            _isDisplay = value;
            if (oldState != _isDisplay)
            {
                if (StateChangedHandle != null)
                    StateChangedHandle();
            }
        }
    }


    /// <summary>
    /// 是否展开节点
    /// </summary>
    private bool _isExpand;
    /// <summary>
    /// 是否展开节点
    /// </summary>
    public bool IsExpand
    {
        get { return _isExpand; }
        set
        {
            bool oldState = _isExpand;
            _isExpand = value;
            if (_isExpand || Count == 0)//展开状态
            {
                packUpImage.enabled = true;
                expandImage.enabled = false;
            }
            else//收起状态
            {
                packUpImage.enabled = false;
                expandImage.enabled = true;
            }
            if (oldState != IsExpand)
            {
                if (StateChangedHandle != null)
                    StateChangedHandle();
            }
        }
    }

    /// <summary>
    /// 是否正在被选择
    /// </summary>
    public bool _isSelect;
    /// <summary>
    /// 是否正在被选择
    /// </summary>
    public bool IsSelect
    {
        get { return _isSelect; }
        set
        {
            _isSelect = value;
            selectImage.enabled = IsSelect;
            if (_isSelect)
            {
                if (SelectNodeHandle != null)
                    SelectNodeHandle(this);
            }
        }
    }

    private void Awake()
    {
        IsExpand = _isExpand;
        selectImage.enabled = false;
        //添加点击展开按钮事件
        EventTrigger expandClickTrigger = expandImage.gameObject.AddComponent<EventTrigger>();
        expandClickTrigger.triggers = new List<EventTrigger.Entry>();
        EventTrigger.Entry expandClickEntry = new EventTrigger.Entry();
        expandClickEntry.eventID = EventTriggerType.PointerClick;
        expandClickEntry.callback = new EventTrigger.TriggerEvent();
        expandClickEntry.callback.AddListener(ExpandImage_Click);
        expandClickTrigger.triggers.Add(expandClickEntry);
        //添加点击树事件
        EventTrigger itemClickTrigger = explanText.gameObject.AddComponent<EventTrigger>();
        itemClickTrigger.triggers = new List<EventTrigger.Entry>();
        EventTrigger.Entry itemClickEntry = new EventTrigger.Entry();
        itemClickEntry.eventID = EventTriggerType.PointerClick;
        itemClickEntry.callback = new EventTrigger.TriggerEvent();
        itemClickEntry.callback.AddListener(Item_Click);
        itemClickTrigger.triggers.Add(itemClickEntry);
    }

    /// <summary>
    /// 点击展开按钮
    /// </summary>
    /// <param name="e"></param>
    private void ExpandImage_Click(BaseEventData e)
    {
        IsSelect = true;
        IsExpand = !IsExpand;
    }

    private void Item_Click(BaseEventData e)
    {
        IsSelect = true;
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
            if (index < ChildNodes.Count && index >= 0)
            {
                return ChildNodes[index];
            }
            throw new Exception("下标超出数组范围");
        }
        //set
        //{
        //    if (childNodes.Count < index && index > 0)
        //    {
        //        if (!CheckNodeSafe(value))
        //            throw new Exception("传入参数不是UITreeNode类型");
        //        childNodes[index] = value;
        //    }
        //    throw new Exception("下标超出数组范围");
        //}
    }

    /// <summary>
    /// 查找指定项的位置
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int IndexOf(UITreeNode item)
    {
        return ChildNodes.IndexOf(item);
    }

    /// <summary>
    /// 将指定项插入到指定位置
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    public bool Insert(int index, UITreeNode item)
    {
        if (!CheckNodeRelation(item))//要添加的节点是本届点的父节点
            return false;
        ChildNodes.Insert(index, item);
        item.Parent = this;
        IsExpand = _isExpand;
        if (StateChangedHandle != null)
            StateChangedHandle();
        return true;
    }

    /// <summary>
    /// 移除指定位置的项,并返回移除的项
    /// 该方法不会删除子节点
    /// </summary>
    /// <param name="index">指定位置</param>
    /// <remarks></remarks>
    public UITreeNode RemoveAt(int index)
    {
        UITreeNode removeTarget = ChildNodes[index];
        ChildNodes.RemoveAt(index);
        removeTarget.Parent = null;
        IsExpand = _isExpand;
        if (StateChangedHandle != null)
            StateChangedHandle();
        return removeTarget;
    }

    public int Count { get { return ChildNodes.Count; } }

    public bool Add(UITreeNode value)
    {
        if (!CheckNodeSafe(value))
            throw new Exception("只能添加UITreeNode类型的对象");
        if (ChildNodes.Contains(value))
            return false;
        if (!CheckNodeRelation(value))//要添加的节点是本届点的父节点
            return false;
        else
        {
            if (value.Parent)
            {
                value.Parent.Remove(value);
            }
            ChildNodes.Add(value);
            value.Parent = this;
            IsExpand = _isExpand;
            if (StateChangedHandle != null)
                StateChangedHandle();
            return true;
        }
    }

    /// <summary>
    /// 清空所有子项
    /// 注意该方法会删除子节点
    /// </summary>
    public void Clear()
    {
        foreach (var item in ChildNodes)
        {
            item.DeleteObject();
        }
        ChildNodes.Clear();
        IsExpand = _isExpand;
        if (StateChangedHandle != null)
            StateChangedHandle();
    }

    /// <summary>
    /// 判断是否存在该子节点
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(UITreeNode value)
    {
        if (!CheckNodeSafe(value))
            return false;
        return ChildNodes.Contains(value);
    }

    /// <summary>
    /// 移除指定的项
    /// 该方法不会删除子节点
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Remove(UITreeNode item)
    {
        bool result = ChildNodes.Remove(item);
        if (result)
        {
            item.Parent = null;
            IsExpand = _isExpand;
        }
        return result;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ChildNodes.GetEnumerator();
    }

    public IEnumerator<UITreeNode> GetEnumerator()
    {
        return ChildNodes.GetEnumerator();
    }

    /// <summary>
    /// 删除对象
    /// </summary>
    public void DeleteObject()
    {
        if (Parent)
        {
            Parent.Remove(this);
        }
        var tempChildNodes = ChildNodes.ToArray();
        foreach (var item in tempChildNodes)
        {
            item.DeleteObject();
        }
        if (DeleteNodeHandle != null)
        {
            try
            {
                DeleteNodeHandle(this);
            }
            catch { }
        }
        DestroyImmediate(gameObject);
    }

    /// <summary>
    /// 检测节点合法性
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private bool CheckNodeSafe(object value)
    {
        if (value == null || !(value.GetType().IsSubclassOf(typeof(UITreeNode)) || value.GetType().Equals(typeof(UITreeNode))))
            return false;
        return true;
    }

    /// <summary>
    /// 检查节点关系
    /// 如果要插入的节点是本届点的上层节点则不可以插入
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool CheckNodeRelation(UITreeNode target)
    {
        UITreeNode parentNode = Parent;
        while (parentNode)
        {
            if (parentNode != target)
            {
                parentNode = parentNode.Parent;
            }
            else return false;
        }
        return true;
    }
}
