using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System;
using System.Linq;
using Newtonsoft.Json;
using MapStruct;

/// <summary>
/// 词条编辑器
/// </summary>
public class EntryDataEditor : EditorWindow
{
    /// <summary>
    /// 保存词条信息的完整路径
    /// </summary>
    public string dataFilePath = @"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\Entry\Entry.json";

    /// <summary>
    /// 数据的图形结构
    /// 默认顶层什么都没有且id为0
    /// </summary>
    Map<EntryDataInfo> dataMap;

    /// <summary>
    /// 选择的目标
    /// </summary>
    IMapElement<EntryDataInfo> selectTarget;

    /// <summary>
    /// 滑动条位置
    /// </summary>
    Vector2 scrollPostion;

    /// <summary>
    /// 选中按钮的样式
    /// </summary>
    GUIStyle buttonSelectStyle;

    /// <summary>
    /// 词条类型枚举对应显示的名字
    /// </summary>
    List<KeyValuePair<EntryDataInfo.EnumEntryValueType, string>> entryValueTypeToExplanList;
    /// <summary>
    /// 词条解锁条件枚举对应显示的名字
    /// </summary>
    List<KeyValuePair<EntryDataInfo.EnumEntryUnlockType, string>> entryUnlockTypeToExplanList;
    /// <summary>
    /// 怪物类型对应显示名字
    /// </summary>
    List<KeyValuePair<EnumMonsterType, string>> monsterTypeToExplanList;

    /// <summary>
    /// 当前选中的词条内容类型
    /// </summary>
    EntryDataInfo.EnumEntryValueType selectEntryValueType;
    /// <summary>
    /// 当前选中的词条解锁条件类型
    /// </summary>
    EntryDataInfo.EnumEntryUnlockType selectEntryUnlockType;

    [MenuItem("小工具/词条编辑器")]
    static void AddWindow()
    {
        EntryDataEditor entryDataEditor = EditorWindow.GetWindow<EntryDataEditor>();
        entryDataEditor.Show();
    }

    private void Awake()
    {
        dataMap = new Map<EntryDataInfo>();
        if (!File.Exists(dataFilePath))
        {
            File.Create(dataFilePath).Close();
            MapElement<EntryDataInfo> root = dataMap.CreateMapElement();
            root.Deep = -1;
            root.Value = new EntryDataInfo() { ID = root.ID };
            string jsonStr = dataMap.Save();
            File.WriteAllText(dataFilePath, jsonStr, Encoding.UTF8);
        }
        else
        {
            string jsonStr = File.ReadAllText(dataFilePath, Encoding.UTF8);
            dataMap.Load(jsonStr);
        }
        entryValueTypeToExplanList = new List<KeyValuePair<EntryDataInfo.EnumEntryValueType, string>>();
        FieldExplanAttribute.SetEnumExplanDic(entryValueTypeToExplanList);
        entryUnlockTypeToExplanList = new List<KeyValuePair<EntryDataInfo.EnumEntryUnlockType, string>>();
        FieldExplanAttribute.SetEnumExplanDic(entryUnlockTypeToExplanList);
        monsterTypeToExplanList = new List<KeyValuePair<EnumMonsterType, string>>();
        FieldExplanAttribute.SetEnumExplanDic(monsterTypeToExplanList);

        buttonSelectStyle = new GUIStyle();
        buttonSelectStyle.fontSize = 10;  //字体大小
        buttonSelectStyle.alignment = TextAnchor.MiddleCenter;//文字位置上下左右居中，
        buttonSelectStyle.normal.background = Resources.Load<Texture2D>("Task/Blue");//背景.
        buttonSelectStyle.normal.textColor = Color.yellow;//文字颜色。
    }

    void OnInspectorUpdate()
    {
        //Debug.Log("窗口面板的更新");
        //这里开启窗口的重绘，不然窗口信息不会刷新
        this.Repaint();
    }

    private void OnGUI()
    {
        if (dataMap == null)
            return;
        EditorGUILayout.BeginHorizontal();
        //显示列表
        EditorGUILayout.BeginVertical(GUILayout.Width(300));
        if (GUILayout.Button("保存数据"))
        {
            string jsonStr = dataMap.Save();
            File.WriteAllText(dataFilePath, jsonStr, Encoding.UTF8);
            EditorUtility.DisplayDialog("保存数据", "保存成功!", "确认");
        }
        if (GUILayout.Button("新建跟节点"))
        {
            MapElement<EntryDataInfo> root = dataMap.FirstElement;
            MapElement<EntryDataInfo> node = dataMap.CreateMapElement();
            node.Deep = 0;
            node.Value = new EntryDataInfo() { ID = node.ID, Name = "[None]" };
            root.CorrelativesNode.Add(node);
        }
        if (GUILayout.Button("新建子节点"))
        {
            if (selectTarget != null)
            {
                MapElement<EntryDataInfo> node = dataMap.CreateMapElement();
                node.Deep = selectTarget.Deep + 1;
                node.Value = new EntryDataInfo() { ID = node.ID, Name = "[None]" };
                selectTarget.CorrelativesNode.Add(node);
            }
            else EditorUtility.DisplayDialog("建立子节点失败", "请先选择一个节点,若当前没有节点请建立一个根节点!", "确认");
        }
        if (selectTarget != null && GUILayout.Button("删除选中节点") && EditorUtility.DisplayDialog("请再次确定!", "是否删除选中节点?", "确定删除", "取消"))
        {
            dataMap.Remove(selectTarget as MapElement<EntryDataInfo>);
            selectTarget = null;
        }
        GUILayout.Space(selectTarget != null ? 10 : 30);
        //使用递归在左侧构建树
        scrollPostion = EditorGUILayout.BeginScrollView(scrollPostion);
        {
            MapElement<EntryDataInfo> root = dataMap.FirstElement;
            IMapElement<EntryDataInfo>[] childs = root.Next(EnumMapTraversalModel.More);
            UpdateItemList(childs);
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        //显示选择的项
        EditorGUILayout.BeginVertical();
        if (selectTarget != null)
        {
            EditorGUILayout.LabelField("ID:" + selectTarget.ID);
            EditorGUILayout.LabelField("-----------------解锁条件-------------------");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("解锁条件");
            List<EntryDataInfo.EnumEntryUnlockType> entryUnlockTypeValues = entryUnlockTypeToExplanList.Select(temp => temp.Key).ToList();
            string[] entryUnlockTypeExplans = entryUnlockTypeToExplanList.Select(temp => temp.Value).ToArray();
            int selectEntryUnlockIndex = entryUnlockTypeValues.IndexOf(selectEntryUnlockType);
            selectEntryUnlockIndex = EditorGUILayout.Popup(selectEntryUnlockIndex, entryUnlockTypeExplans);
            if (selectEntryUnlockIndex > 0)
                selectEntryUnlockType = entryUnlockTypeValues[selectEntryUnlockIndex];
            if (GUILayout.Button("添加") && !selectTarget.Value.UnlockDick.ContainsKey(selectEntryUnlockType))
            {
                selectTarget.Value.UnlockDick.Add(selectEntryUnlockType, 0);
            }
            EditorGUILayout.EndHorizontal();
            var entryUnlocks = selectTarget.Value.UnlockDick.Select(temp => temp.Key).ToArray();
            foreach (var entryUnlock in entryUnlocks)
            {
                GUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("×", GUILayout.Width(20)) && EditorUtility.DisplayDialog("请再次确认!", "是否删除该解锁条件", "确认删除", "取消"))
                {
                    selectTarget.Value.UnlockDick.Remove(entryUnlock);
                    continue;
                }
                switch (entryUnlock)
                {
                    case EntryDataInfo.EnumEntryUnlockType.OverTask:
                        EditorGUILayout.LabelField("解锁条件:完成任务!    任务ID:");
                        selectTarget.Value.UnlockDick[entryUnlock] = EditorGUILayout.IntField(selectTarget.Value.UnlockDick[entryUnlock]);
                        break;
                    case EntryDataInfo.EnumEntryUnlockType.KillMonster:
                        EditorGUILayout.LabelField("解锁条件:杀死怪物!    怪物类型:");
                        EnumMonsterType monsterType = default(EnumMonsterType);
                        try
                        {
                            monsterType = (EnumMonsterType)Enum.Parse(typeof(EnumMonsterType), selectTarget.Value.UnlockDick[entryUnlock].ToString());
                        }
                        catch { }
                        List<EnumMonsterType> monsterTypeValues = monsterTypeToExplanList.Select(temp => temp.Key).ToList();
                        string[] monsterTypeExplans = monsterTypeToExplanList.Select(temp => temp.Value).ToArray();
                        int monsterTypeIndex = monsterTypeValues.IndexOf(monsterType);
                        monsterTypeIndex = EditorGUILayout.Popup(monsterTypeIndex, monsterTypeExplans);
                        if (monsterTypeIndex > 0)
                            monsterType = monsterTypeValues[monsterTypeIndex];
                        selectTarget.Value.UnlockDick[entryUnlock] = (int)monsterType;
                        break;
                    case EntryDataInfo.EnumEntryUnlockType.ClickNPC:
                        EditorGUILayout.LabelField("解锁条件:点击NPC!    NPC ID :");
                        selectTarget.Value.UnlockDick[entryUnlock] = EditorGUILayout.IntField(selectTarget.Value.UnlockDick[entryUnlock]);
                        break;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.LabelField("-----------------内容编辑-------------------");
            EditorGUILayout.BeginHorizontal();
            selectTarget.Value.Name = EditorGUILayout.TextField("要显示在列表中的内容", selectTarget.Value.Name);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("词条内容类型");
            List<EntryDataInfo.EnumEntryValueType> entryValueTypeValues = entryValueTypeToExplanList.Select(temp => temp.Key).ToList();
            string[] entryValueTypeExplans = entryValueTypeToExplanList.Select(temp => temp.Value).ToArray();
            int selectEntryValueIndex = entryValueTypeValues.IndexOf(selectEntryValueType);
            selectEntryValueIndex = EditorGUILayout.Popup(selectEntryValueIndex, entryValueTypeExplans);
            if (selectEntryValueIndex >= 0)
                selectEntryValueType = entryValueTypeValues[selectEntryValueIndex];
            if (GUILayout.Button("添加"))
            {
                selectTarget.Value.Datas.Add(new EntryDataInfo.EntryValue() { EntryValueType = selectEntryValueType, Data = "" });
            }
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < selectTarget.Value.Datas.Count; i++)
            {
                GUILayout.Space(15);
                EditorGUILayout.BeginHorizontal();
                int selectIndex = entryValueTypeValues.IndexOf(selectTarget.Value.Datas[i].EntryValueType);
                EditorGUILayout.LabelField("词条内容类型:" + entryValueTypeExplans[selectIndex], GUILayout.Width(120));
                if (GUILayout.Button("↑", GUILayout.Width(20)))
                {
                    if (i > 0)
                    {
                        EntryDataInfo.EntryValue entryValue = selectTarget.Value.Datas[i];
                        selectTarget.Value.Datas.Remove(entryValue);
                        selectTarget.Value.Datas.Insert(i - 1, entryValue);
                    }
                }
                if (GUILayout.Button("↓", GUILayout.Width(20)))
                {
                    if (i < selectTarget.Value.Datas.Count - 1)
                    {
                        EntryDataInfo.EntryValue entryValue = selectTarget.Value.Datas[i];
                        selectTarget.Value.Datas.Remove(entryValue);
                        if (i >= selectTarget.Value.Datas.Count - 1)
                        {
                            selectTarget.Value.Datas.Add(entryValue);
                        }
                        else
                        {
                            selectTarget.Value.Datas.Insert(i + 1, entryValue);
                        }
                    }
                }
                if (GUILayout.Button("×", GUILayout.Width(20)) && EditorUtility.DisplayDialog("请再次确定!", "是否删除该词条内容?", "确定删除", "取消"))
                {
                    selectTarget.Value.Datas.RemoveAt(i);
                    i--;
                    EditorGUILayout.EndHorizontal();
                    break;
                }
                EditorGUILayout.EndHorizontal();
                {
                    EntryDataInfo.EntryValue entryValue = selectTarget.Value.Datas[i];
                    switch (entryValue.EntryValueType)
                    {
                        case EntryDataInfo.EnumEntryValueType.Title:
                            entryValue.Data = EditorGUILayout.TextField(entryValue.Data);
                            break;
                        case EntryDataInfo.EnumEntryValueType.Text:
                            entryValue.Data = EditorGUILayout.TextArea(entryValue.Data, GUILayout.Height(100));
                            break;
                        case EntryDataInfo.EnumEntryValueType.Image:
                            Sprite sprite = SpriteManager.GetSrpite(entryValue.Data);
                            sprite = (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), true, GUILayout.Width(100), GUILayout.Height(100));
                            entryValue.Data = SpriteManager.GetName(sprite);
                            break;
                    }
                }
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 更新列表的显示(递归)
    /// </summary>
    /// <param name="items"></param>
    private void UpdateItemList(IMapElement<EntryDataInfo>[] items)
    {
        foreach (IMapElement<EntryDataInfo> item in items)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15 * item.Deep);
            if (selectTarget != null && selectTarget.ID == item.ID)
            {
                if (GUILayout.Button(item.ID + ":" + item.Value.Name, buttonSelectStyle))
                { selectTarget = item; selectEntryValueType = EntryDataInfo.EnumEntryValueType.Title; }
            }
            else
            {
                if (GUILayout.Button(item.ID + ":" + item.Value.Name))
                { selectTarget = item; selectEntryValueType = EntryDataInfo.EnumEntryValueType.Title; }
            }
            EditorGUILayout.EndHorizontal();
            //便利子节点
            IMapElement<EntryDataInfo>[] childs = item.Next(EnumMapTraversalModel.More);
            if (childs.Length > 0)
            {
                UpdateItemList(childs);
            }
        }
    }
}
