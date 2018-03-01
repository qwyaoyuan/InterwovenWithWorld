using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MapStruct;

/// <summary>
/// 词条数据
/// </summary>
public class EntryData : ILoadable<EntryData>
{


    /// <summary>
    /// 保存词条默认信息的路径
    /// </summary>
    public static string dataFilePath = "Data/Entry/Entry";

    /// <summary>
    /// 数据的图形结构
    /// 默认顶层什么都没有且id为0
    /// </summary>
    Map<EntryDataInfo> dataMap;

    public void Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(dataFilePath);
        string assetText = Encoding.UTF8.GetString(textAsset.bytes);
        dataMap = new Map<EntryDataInfo>();
        dataMap.Load(assetText);
    }

    /// <summary>
    /// 获取顶层的数据
    /// </summary>
    /// <returns></returns>
    public EntryDataInfo[] GetTops()
    {
        MapElement<EntryDataInfo> root = dataMap.FirstElement;
        if (root != null)
            return GetNexts(root.Value);//id0表示的是最上层的根节点(此节点不用于显示)
        return null;
    }

    /// <summary>
    /// 向下遍历数据
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public EntryDataInfo[] GetNexts(EntryDataInfo parent)
    {
        MapElement<EntryDataInfo> root = dataMap.GetElement(parent.ID);
        if (root == null)
            return null;
        return root.Next(EnumMapTraversalModel.More).Select(temp => temp.Value).ToArray();
    }

}

/// <summary>
/// 具体的词条
/// </summary>
public class EntryDataInfo
{
    /// <summary>
    /// 词条ID
    /// </summary>
    public int ID;

    /// <summary>
    /// 词条显示的名字
    /// </summary>
    public string Name;

    /// <summary>
    /// 数据集合
    /// </summary>
    public List<EntryValue> Datas;

    /// <summary>
    /// 词条解锁条件字典
    /// </summary>
    public Dictionary<EnumEntryUnlockType, int> UnlockDick;

    /// <summary>
    /// 词条内容的类型
    /// </summary>
    public enum EnumEntryValueType
    {
        /// <summary>
        /// 标题
        /// </summary>
        [FieldExplan("标题")]
        Title,
        /// <summary>
        /// 文字内容
        /// </summary>
        [FieldExplan("文字内容")]
        Text,
        /// <summary>
        /// 图片内容
        /// </summary>
        [FieldExplan("图片内容")]
        Image,
    }

    /// <summary>
    /// 词条内容
    /// </summary>
    public class EntryValue
    {
        /// <summary>
        /// 词条类型
        /// </summary>
        public EnumEntryValueType EntryValueType;
        /// <summary>
        /// 词条内容(除了图片类型都是保存的原始数据,图片类型保存的是图片名)
        /// </summary>
        public string Data;

    }

    /// <summary>
    /// 词条的解锁条件
    /// </summary>
    public enum EnumEntryUnlockType
    {
        /// <summary>
        /// 完成任务
        /// </summary>
        [FieldExplan("完成任务")]
        OverTask,
        /// <summary>
        /// 杀死怪物
        /// </summary>
        [FieldExplan("杀死怪物")]
        KillMonster,
        /// <summary>
        /// 点击NPC
        /// </summary>
        [FieldExplan("点击NPC")]
        ClickNPC,
    }


    public EntryDataInfo()
    {
        Datas = new List<EntryValue>();
        UnlockDick = new Dictionary<EnumEntryUnlockType, int>();
    }
}
