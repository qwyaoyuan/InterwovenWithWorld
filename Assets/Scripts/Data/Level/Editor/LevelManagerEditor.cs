using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using ReflectEncapsulation;

/// <summary>
/// 等级编辑器
/// </summary>
public class LevelManagerEditor : EditorWindow
{
    /// <summary>
    /// 保存等级信息的完整目录路径
    /// </summary>
    public string dataDirecotryPath = @"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\Level";

    /// <summary>
    /// 等级与数据对应的字典
    /// </summary>
    Dictionary<int, LevelDataInfo> levelToDataDic;

    [MenuItem("小工具/Level数据编辑器")]
    static void AddWindow()
    {
        LevelManagerEditor levelManagerEditor = EditorWindow.GetWindow<LevelManagerEditor>();
        levelManagerEditor.Show();
    }

    private void Awake()
    {
        if (!Directory.Exists(dataDirecotryPath))
            Directory.CreateDirectory(dataDirecotryPath);
        if (!File.Exists(dataDirecotryPath + "/Level.txt"))
        {
            levelToDataDic = new Dictionary<int, LevelDataInfo>();
            File.Create(dataDirecotryPath + "/Level.txt").Close();
            string valueText = SerializeNow(levelToDataDic);
            File.WriteAllText(dataDirecotryPath + "/Level.txt", valueText, Encoding.UTF8);
        }
        else
        {
            string valueText = File.ReadAllText(dataDirecotryPath + "/Level.txt", Encoding.UTF8);
            levelToDataDic = DeSerializeNow<Dictionary<int, LevelDataInfo>>(valueText);
            if (levelToDataDic == null)
                levelToDataDic = new Dictionary<int, LevelDataInfo>();
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

    void OnInspectorUpdate()
    {
        //Debug.Log("窗口面板的更新");
        //这里开启窗口的重绘，不然窗口信息不会刷新
        this.Repaint();
    }

    /// <summary>
    /// 左侧的滑动条值
    /// </summary>
    Vector2 leftScroll;
    /// <summary>
    /// 选择的项
    /// </summary>
    int selectKey;

    private void OnGUI()
    {
        if (levelToDataDic == null)
            return;
        EditorGUILayout.BeginHorizontal();
        //左侧的选择等级按钮面板
        EditorGUILayout.BeginVertical(GUILayout.Width(83));
        if (GUILayout.Button("保存"))
        {
            string valueText = SerializeNow(levelToDataDic);
            File.WriteAllText(dataDirecotryPath + "/Level.txt", valueText, Encoding.UTF8);
            EditorUtility.DisplayDialog("保存数据", "保存成功!", "确认");
        }
        leftScroll = EditorGUILayout.BeginScrollView(leftScroll);
        EditorGUILayout.BeginHorizontal();
        int count = levelToDataDic.Count;
        if (GUILayout.Button("-", GUILayout.Width(18)))
            if (EditorUtility.DisplayDialog("警告!", "将会减去最后一个等级的数据!", "确认", "取消"))
                count--;
        EditorGUILayout.IntField(count, GUILayout.Width(18));
        if (GUILayout.Button("+", GUILayout.Width(18)))
            count++;
        if (count != levelToDataDic.Count && count >= 0)
        {
            if (count > levelToDataDic.Count)
                levelToDataDic.Add(levelToDataDic.Count, new LevelDataInfo());
            else
                levelToDataDic.Remove(levelToDataDic.Count - 1);
        }
        EditorGUILayout.EndHorizontal();
        selectKey = Mathf.Clamp(selectKey, -1, levelToDataDic.Count - 1);
        for (int i = 0; i < levelToDataDic.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (selectKey == i)
                GUILayout.Space(20);
            if (GUILayout.Button(i.ToString(), GUILayout.Width(40)))
            {
                if (selectKey == i)
                    selectKey = -1;
                else selectKey = i;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        //右侧的当前等级对应数据面板
        EditorGUILayout.BeginVertical();
        if (selectKey > -1 && selectKey < levelToDataDic.Count)
        {
            //取出该等级的数据并构建反射单元
            LevelDataInfo levelDataInfo = levelToDataDic[selectKey];
            ReflectUnit<LevelDataInfo> levelDataInfoUnit = Entry.On(levelDataInfo);
            //设置该等级
            if (levelDataInfo.Level != selectKey)
                levelDataInfoUnit.Field("level", selectKey).End();
            EditorGUILayout.LabelField("等级:" + levelDataInfo.Level);
            //设置升级所需经验
            int experience = EditorGUILayout.IntField("升级所需经验:", levelDataInfo.Experience);
            if (experience != levelDataInfo.Experience)
                levelDataInfoUnit.Field("experience", experience).End();
            //设置该等级附加的力量
            int strength = EditorGUILayout.IntField("本等级附加的力量:", levelDataInfo.Strength);
            if (strength != levelDataInfo.Strength)
                levelDataInfoUnit.Field("strength", strength).End();
            //设置该等级附加的精神
            int spirit = EditorGUILayout.IntField("本等级附加的精神", levelDataInfo.Spirit);
            if (spirit != levelDataInfo.Spirit)
                levelDataInfoUnit.Field("spirit", spirit).End();
            //设置该等级附加的敏捷
            int agility = EditorGUILayout.IntField("本等级附加的敏捷", levelDataInfo.Agility);
            if (agility != levelDataInfo.Agility)
                levelDataInfoUnit.Field("agility", agility).End();
            //设置该等级附加的专注
            int concentration = EditorGUILayout.IntField("本等级附加的专注", levelDataInfo.Concentration);
            if (concentration != levelDataInfo.Concentration)
                levelDataInfoUnit.Field("concentration", concentration).End();
            //设置该等级附加的自由点数
            int freedomPoint = EditorGUILayout.IntField("本等级附加的自由点数", levelDataInfo.FreedomPoint);
            if (freedomPoint != levelDataInfo.FreedomPoint)
                levelDataInfoUnit.Field("freedomPoint", freedomPoint).End();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}
