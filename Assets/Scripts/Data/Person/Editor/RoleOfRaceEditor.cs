using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System;
using System.Reflection;
using System.Linq;

/// <summary>
/// 种族数据编辑器
/// </summary>
public class RoleOfRaceEditor : EditorWindow
{
    /// <summary>
    /// 保存种族信息的完整路径
    /// </summary>
    public string dataFilePath = "";//@"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\RoleOfRaceData\RoleOfRaceData.json";


    [MenuItem("小工具/种族数据编辑器")]
    static void AddWindow()
    {
        RoleOfRaceEditor roleOfRateEditor = EditorWindow.GetWindow<RoleOfRaceEditor>();
        roleOfRateEditor.Show();
    }

    RoleOfRaceEditor()
    {
        this.titleContent = new GUIContent("种族数据编辑器");
    }

    /// <summary>
    /// 每个种族的数据数组
    /// </summary>
    private RoleOfRaceInfoStruct[] roleOfRaceInfoStructArray;

    /// <summary>
    /// 节点显示树
    /// </summary>
    Tree<KeyValuePair<RoleOfRace, Rect>> roleOfRaceShowTree;
    /// <summary>
    /// 枚举对应的显示集合
    /// </summary>
    List<KeyValuePair<RoleOfRace, string>> roleOfRaceExplanList;

    private void Awake()
    {
        //重置路径
        dataFilePath = Application.dataPath + @"\Scripts\Data\Resources\Data\RoleOfRaceData\RoleOfRaceData.json";

        if (!File.Exists(dataFilePath))
        {
            File.Create(dataFilePath).Close();
            roleOfRaceInfoStructArray = new RoleOfRaceInfoStruct[0];
            string valueText = SerializeNow(roleOfRaceInfoStructArray);
            File.WriteAllText(dataFilePath, valueText, Encoding.UTF8);
        }
        else
        {
            string valueText = File.ReadAllText(dataFilePath, Encoding.UTF8);
            roleOfRaceInfoStructArray = DeSerializeNow<RoleOfRaceInfoStruct[]>(valueText);
            if (roleOfRaceInfoStructArray == null)
                roleOfRaceInfoStructArray = new RoleOfRaceInfoStruct[0];
        }
        Type roleOfRaceType = typeof(RoleOfRace);
        FieldInfo[] fieldInfos = roleOfRaceType.GetFields(BindingFlags.Public | BindingFlags.Static);
        RoleOfRace[] roleOfRaces = fieldInfos.Select(temp => (int)temp.GetValue(null)).Select(temp => (RoleOfRace)temp).ToArray();
        List<RoleOfRaceInfoStruct> roleOfRaceInfoStructList = new List<RoleOfRaceInfoStruct>(roleOfRaceInfoStructArray);
        foreach (RoleOfRace roleOfRace in roleOfRaces)
        {
            if (roleOfRaceInfoStructList.Count(temp => temp.roleOfRace == roleOfRace) == 0)
            {
                roleOfRaceInfoStructList.Add(new RoleOfRaceInfoStruct() { roleOfRace = roleOfRace });
            }
        }
        roleOfRaceInfoStructArray = roleOfRaceInfoStructList.ToArray();
        //重构节点显示树
        roleOfRaceShowTree = new Tree<KeyValuePair<RoleOfRace, Rect>>();
        Tree<RoleOfRace> roleOfRaceTree = RoleOfRaceHelper.roleOfRaceTree;
        roleOfRaceShowTree.TopNode = new TreeNode<KeyValuePair<RoleOfRace, Rect>>(new KeyValuePair<RoleOfRace, Rect>(roleOfRaceTree.TopNode.Data, Rect.zero));
        TreeNode<KeyValuePair<RoleOfRace, Rect>> tempShowNode = roleOfRaceShowTree.TopNode;
        TreeNode<RoleOfRace> tempNode = roleOfRaceTree.TopNode;
        //设置显示树的节点
        SetShowTreeChildNode(tempNode, tempShowNode);
        //查找树的最大深度和最大广度
        Dictionary<RoleOfRace, KeyValuePair<int, int>> deepExtentDic = new Dictionary<RoleOfRace, KeyValuePair<int, int>>();
        CheckShowTreeDeepAndExtent(tempNode, 1, 1, deepExtentDic);
        //计算最大的深度和广度
        int maxDeep = deepExtentDic.Max(temp => temp.Value.Key);
        int maxExtent = deepExtentDic.Max(temp => temp.Value.Value);
        //根据该深度和广度计算总大小
        maxHeight = maxDeep * 150;//50+100 高50 间距100
        maxWidth = maxExtent * 110;//100+10 宽100 间距10
        SetShowTreeRect(tempShowNode, deepExtentDic, 100, 50, 10, 100, maxWidth);
        //设置枚举对应的显示文字
        roleOfRaceExplanList = new List<KeyValuePair<RoleOfRace, string>>();
        FieldExplanAttribute.SetEnumExplanDic(roleOfRaceExplanList);
    }

    /// <summary>
    /// 设置显示树的子节点
    /// </summary>
    /// <param name="node"></param>
    /// <param name="showNode"></param>
    private void SetShowTreeChildNode(TreeNode<RoleOfRace> node, TreeNode<KeyValuePair<RoleOfRace, Rect>> showNode)
    {
        if (node.Children != null && node.Children.Count > 0)
        {
            foreach (TreeNode<RoleOfRace> childNode in node.Children)
            {
                TreeNode<KeyValuePair<RoleOfRace, Rect>> showChildNode = new TreeNode<KeyValuePair<RoleOfRace, Rect>>(new KeyValuePair<RoleOfRace, Rect>(childNode.Data, Rect.zero));
                showNode.AddChild(showChildNode);
                SetShowTreeChildNode(childNode, showChildNode);

            }
        }
    }

    /// <summary>
    /// 检测树的深度和广度
    /// </summary>
    /// <param name="node">节点</param>
    /// <param name="deep">深度</param>
    /// <param name="extent">广度</param>
    /// <param name="resultDic">每个节点的所在的深度和广度</param>
    private void CheckShowTreeDeepAndExtent(TreeNode<RoleOfRace> node, int deep, int extent, Dictionary<RoleOfRace, KeyValuePair<int, int>> resultDic)
    {
        if (!resultDic.ContainsKey(node.Data))
            resultDic.Add(node.Data, new KeyValuePair<int, int>(deep, extent));

        if (node.Children != null && node.Children.Count > 0)
        {
            int nextDeep = deep + 1;//下一层的深度
            //计算下一层节点的起始广度
            var nextDeepNodes = resultDic.Where(temp => temp.Value.Key == nextDeep);
            int nextDeepExtent = 0;
            if (nextDeepNodes.Count() > 0)
                nextDeepExtent = nextDeepNodes.Max(temp => temp.Value.Value);
            foreach (TreeNode<RoleOfRace> childNode in node.Children)
            {
                if (!resultDic.ContainsKey(childNode.Data))
                {
                    nextDeepExtent += 1;
                }
                CheckShowTreeDeepAndExtent(childNode, nextDeep, nextDeepExtent, resultDic);
            }
        }
    }

    /// <summary>
    /// 设置显示树的具体显示位置和大小
    /// </summary>
    /// <param name="showNode">显示数节点</param>
    /// <param name="deepExtentDic">深度与广度字典</param>
    /// <param name="selfWidth">树节点的显示宽度</param>
    /// <param name="selfHeight">树节点的显示高度</param>
    /// <param name="intervalWidth">间隔宽度</param>
    /// <param name="intervalHeight">间隔高度</param>
    /// <param name="maxWidth">最大的宽度</param>
    private void SetShowTreeRect(TreeNode<KeyValuePair<RoleOfRace, Rect>> showNode, Dictionary<RoleOfRace, KeyValuePair<int, int>> deepExtentDic, int selfWidth, int selfHeight, int intervalWidth, int intervalHeight, int maxWidth)
    {
        KeyValuePair<int, int> thisNodeDeepAndExtent = deepExtentDic[showNode.Data.Key];//当前节点的深度和广度
        //当前深度的最大广度
        int thisDeepMaxExtent = deepExtentDic.Where(temp => temp.Value.Key == thisNodeDeepAndExtent.Key).Max(temp => temp.Value.Value);
        //根据最大广度计算每一个格子的宽度
        int everyWidth = maxWidth / thisDeepMaxExtent;
        //该节点的起始x轴位置
        int startX = everyWidth * (thisNodeDeepAndExtent.Value - 1) + everyWidth / 2 - (selfWidth + intervalWidth) / 2;
        //该节点的其实y轴位置
        int startY = (selfHeight + intervalHeight) * (thisNodeDeepAndExtent.Key - 1) + intervalHeight / 2;
        KeyValuePair<RoleOfRace, Rect> tempShowNodeData = showNode.Data;
        showNode.Data = new KeyValuePair<RoleOfRace, Rect>(tempShowNodeData.Key, new Rect(startX, startY, selfWidth, selfHeight));
        //便利子节点
        if (showNode.Children != null && showNode.Children.Count > 0)
        {
            foreach (TreeNode<KeyValuePair<RoleOfRace, Rect>> item in showNode.Children)
            {
                SetShowTreeRect(item, deepExtentDic, selfWidth, selfHeight, intervalWidth, intervalHeight, maxWidth);
            }
        }
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="target">对象</param>
    /// <returns>返回的字符串</returns>
    public string SerializeNow<T>(T target) where T : class
    {
        if (target == null)
            return "";
        string value = JsonConvert.SerializeObject(target);
        return value;
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T">反序列化后的类型</typeparam>
    /// <param name="value">字符串</param>
    /// <returns>对象</returns>
    public T DeSerializeNow<T>(string value) where T : class
    {
        T target = JsonConvert.DeserializeObject<T>(value);
        return target;
    }

    private void OnInspectorUpdate()
    {
        this.Repaint();
    }

    /// <summary>
    /// 滑动条
    /// </summary>
    Vector2 scroll;
    /// <summary>
    /// 最大高度
    /// </summary>
    int maxHeight;
    /// <summary>
    /// 最大宽度
    /// </summary>
    int maxWidth;

    private void OnGUI()
    {
        if (roleOfRaceShowTree == null)
            return;
        float width = position.width;
        float height = position.height;
        EditorGUILayout.BeginHorizontal();
        scroll = GUI.BeginScrollView(new Rect(0, 0, width, height), scroll, new Rect(0, 0, maxWidth, maxHeight));
        //绘制连线
        Handles.color = Color.red;
        ShowNodeLine(roleOfRaceShowTree.TopNode);
        //绘制节点
        List<int> valueList = roleOfRaceExplanList.Select(temp => (int)temp.Key).ToList();
        string[] explanList = roleOfRaceExplanList.Select(temp => temp.Value).ToArray();
        ShowNodeButton(roleOfRaceShowTree.TopNode, valueList, explanList);
        //绘制保存按钮
        if (GUI.Button(new Rect(scroll.x, scroll.y, 100, 30), "保存"))
        {
            string valueText = SerializeNow(roleOfRaceInfoStructArray);
            File.WriteAllText(dataFilePath, valueText, Encoding.UTF8);
            EditorUtility.DisplayDialog("提示!", "保存成功", "是");
        }
        GUI.EndScrollView();
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 显示节点的按钮
    /// </summary>
    /// <param name="showNode"></param>
    ///<param name="explanList"></param>
    ///<param name="valueList"></param>
    private void ShowNodeButton(TreeNode<KeyValuePair<RoleOfRace, Rect>> showNode, List<int> valueList, string[] explanList)
    {
        int index = valueList.IndexOf((int)showNode.Data.Key);
        string explan = "None";
        if (index >= 0)
        {
            explan = explanList[index];
        }
        if (GUI.Button(showNode.Data.Value, explan))
        {
            RoleOfRaceInfoEditor roleOfRaceInfoEditor = EditorWindow.GetWindow<RoleOfRaceInfoEditor>();
            roleOfRaceInfoEditor.roleOfRaceInfoStruct = roleOfRaceInfoStructArray.FirstOrDefault(temp => temp.roleOfRace == showNode.Data.Key);
            roleOfRaceInfoEditor.RoleOfRaceString = explan;
            roleOfRaceInfoEditor.Show();
        }
        if (showNode.Children != null && showNode.Children.Count > 0)
            foreach (TreeNode<KeyValuePair<RoleOfRace, Rect>> childNode in showNode.Children)
            {
                ShowNodeButton(childNode, valueList, explanList);
            }
    }

    /// <summary>
    /// 显示节点的连线
    /// </summary>
    /// <param name="parentShowNode"></param>
    private void ShowNodeLine(TreeNode<KeyValuePair<RoleOfRace, Rect>> parentShowNode)
    {
        if (parentShowNode.Children != null && parentShowNode.Children.Count > 0)
        {
            Vector2 startPoint = parentShowNode.Data.Value.center;
            foreach (TreeNode<KeyValuePair<RoleOfRace, Rect>> childNode in parentShowNode.Children)
            {
                Vector2 endPoint = childNode.Data.Value.center;
                Handles.DrawLine(startPoint, endPoint);
                ShowNodeLine(childNode);
            }
        }
    }

}

/// <summary>
/// 种族的详细数据编辑
/// </summary>
public class RoleOfRaceInfoEditor : EditorWindow
{
    RoleOfRaceInfoEditor()
    {
        this.titleContent = new GUIContent("种族数据详细设置:");
    }

    /// <summary>
    /// 该种组的数据
    /// </summary>
    public RoleOfRaceInfoStruct roleOfRaceInfoStruct;

    private string roleOfRaceString;
    /// <summary>
    /// 种族名
    /// </summary>
    public string RoleOfRaceString
    {
        get { return roleOfRaceString; }
        set
        {
            roleOfRaceString = value;
            this.titleContent.text = value;
        }
    }

    /// <summary>
    /// 类的字段数组
    /// </summary>
    FieldInfo[] fieldInfos;
    /// <summary>
    /// 类的字段与字段说明字典
    /// </summary>
    Dictionary<FieldInfo, FieldExplanAttribute> fieldInfoToExplanDic;

    private void Awake()
    {
        Type type = typeof(RoleOfRaceInfoStruct);
        fieldInfos = type.GetFields(BindingFlags.Public|BindingFlags.Instance);
        fieldInfoToExplanDic = new Dictionary<FieldInfo, FieldExplanAttribute>();
        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            FieldExplanAttribute fieldExplanAttribute = FieldExplanAttribute.GetFieldInfoExplan(fieldInfo);
            if (fieldExplanAttribute != null)
            {
                fieldInfoToExplanDic.Add(fieldInfo, fieldExplanAttribute);
            }
        }
    }

    private void OnInspectorUpdate()
    {
        base.Repaint();
    }

    Vector2 scroll;

    private void OnGUI()
    {
        if (roleOfRaceInfoStruct == null)
            return;
        EditorGUILayout.BeginVertical();
        scroll = EditorGUILayout.BeginScrollView(scroll);
        foreach (KeyValuePair<FieldInfo,FieldExplanAttribute> fieldInfoToExplan in fieldInfoToExplanDic)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(fieldInfoToExplan.Value.GetExplan());
            object value = fieldInfoToExplan.Key.GetValue(roleOfRaceInfoStruct);
            if (fieldInfoToExplan.Key.FieldType.Equals(typeof(int)))
            {
                int valueInt = (int)value;
                value = EditorGUILayout.IntField(valueInt);
            }
            else if (fieldInfoToExplan.Key.FieldType.Equals(typeof(float)))
            {
                float valueFloat = (float)value;
                value = EditorGUILayout.FloatField(valueFloat);
            }
            fieldInfoToExplan.Key.SetValue(roleOfRaceInfoStruct, value);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

}
