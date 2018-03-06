using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;
using ReflectEncapsulation;

/// <summary>
/// 状态(buff debuff 特殊状态)编辑器
/// </summary>
public class StatusManagerEditor : EditorWindow
{

    /// <summary>
    /// 保存状态信息的完整目录路径
    /// </summary>
    public string dataDirecotryPath = "";//@"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\Status";

    /// <summary>
    /// 数字字典
    /// </summary>
    Dictionary<EnumStatusEffect, StatusDataInfo> dataDic;

    [MenuItem("小工具/Status(状态)数据编辑器")]
    static void AddWindow()
    {
        StatusManagerEditor statusManagerEditor = EditorWindow.GetWindow<StatusManagerEditor>();
        statusManagerEditor.Show();
    }

    private void Awake()
    {
        //重置路径
        dataDirecotryPath = Application.dataPath + @"\Scripts\Data\Resources\Data\Status";

        assembly = typeof(StatusActionDataInfo_Base).Assembly;
        if (!Directory.Exists(dataDirecotryPath))
            Directory.CreateDirectory(dataDirecotryPath);
        if (!File.Exists(dataDirecotryPath + "/Status.txt"))
        {
            dataDic = new Dictionary<EnumStatusEffect, StatusDataInfo>();
            File.Create(dataDirecotryPath + "/Status.txt").Close();
            string valueText = SerializeNow(dataDic);
            File.WriteAllText(dataDirecotryPath + "/Status.txt", valueText, Encoding.UTF8);
        }
        else
        {
            string valueText = File.ReadAllText(dataDirecotryPath + "/Status.txt", Encoding.UTF8);
            dataDic = DeSerializeNow<Dictionary<EnumStatusEffect, StatusDataInfo>>(valueText);
            if (dataDic == null)
                dataDic = new Dictionary<EnumStatusEffect, StatusDataInfo>();
        }
        //检测字典中的是否存在指定状态,如果不存在则添加,如果多余则剔除
        IEnumerable<EnumStatusEffect> CheckStatusEffect = dataDic.Keys.OfType<EnumStatusEffect>();
        IEnumerable<EnumStatusEffect> defaultStatusEffect = Enum.GetValues(typeof(EnumStatusEffect)).OfType<EnumStatusEffect>();
        foreach (EnumStatusEffect enumStatusEffect in CheckStatusEffect)
        {
            if (!defaultStatusEffect.Contains(enumStatusEffect))
            {
                dataDic.Remove(enumStatusEffect);
            }
        }
        foreach (EnumStatusEffect enumStatusEffect in defaultStatusEffect)
        {
            if (!dataDic.ContainsKey(enumStatusEffect))
            {
                dataDic.Add(enumStatusEffect, new StatusDataInfo());
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
        string value = JsonConvert.SerializeObject(target,
            new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
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
        T target = JsonConvert.DeserializeObject<T> (value,
            new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
        return target;
    }

    void OnInspectorUpdate()
    {
        //Debug.Log("窗口面板的更新");
        //这里开启窗口的重绘，不然窗口信息不会刷新
        this.Repaint();
    }

    /// <summary>
    /// 选择的枚举
    /// </summary>
    EnumStatusEffect selectEnumStatusEffect;

    /// <summary>
    /// 选择的等级
    /// </summary>
    int selectLevel;

    /// <summary>
    /// 右侧的滑动条值
    /// </summary>
    Vector2 rightScroll;

    /// <summary>
    /// 程序集
    /// </summary>
    Assembly assembly;

    /// <summary>
    /// 重新开始
    /// </summary>
    bool restart;

    private void OnGUI()
    {
        if (restart)
        {
            EditorGUILayout.LabelField("出现错误请重新打开");
            return;
        }
        try
        {
            EditorGUILayout.BeginHorizontal();
            //左侧的选择状态类型与保存面板
            EditorGUILayout.BeginVertical(GUILayout.Width(100));
            if (GUILayout.Button("保存", GUILayout.Width(95)))
            {
                string valueText = SerializeNow(dataDic);
                File.WriteAllText(dataDirecotryPath + "/Status.txt", valueText, Encoding.UTF8);
                EditorUtility.DisplayDialog("保存数据", "保存成功!", "确认");
            }
            EditorGUILayout.LabelField("状态类型");
            selectEnumStatusEffect = (EnumStatusEffect)EditorGUILayout.EnumPopup(selectEnumStatusEffect, GUILayout.Width(95));
            FieldInfo fieldInfo = typeof(EnumStatusEffect).GetField(selectEnumStatusEffect.ToString());
            FieldExplanAttribute fieldExplane = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
            if (fieldExplane != null)
            {
                EditorGUILayout.LabelField(fieldExplane.GetExplan(), GUILayout.Width(95));
            }
            EditorGUILayout.EndVertical();

            //右侧显示详细信息面板
            EditorGUILayout.BeginVertical();
            rightScroll = EditorGUILayout.BeginScrollView(rightScroll);
            if (dataDic.ContainsKey(selectEnumStatusEffect))
            {
                StatusDataInfo statusDataInfo = dataDic[selectEnumStatusEffect];
                ReflectUnit<StatusDataInfo> statusDataInfoUnit = Entry.On(statusDataInfo);
                EditorGUILayout.BeginHorizontal();
                //说明
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("状态简要说明");
                string statusExplane = EditorGUILayout.TextArea(statusDataInfo.StatusExplane, GUILayout.Width(200), GUILayout.Height(100));
                if (!string.Equals(statusExplane, statusDataInfo.StatusExplane))
                    statusDataInfoUnit.Field("statusExplane", statusExplane);
                EditorGUILayout.EndVertical();
                //图片
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("状态的图标");
                statusDataInfo.Load();
                Sprite statusSprite = (Sprite)EditorGUILayout.ObjectField(statusDataInfo.StatusSprite, typeof(Sprite), true, GUILayout.Width(100), GUILayout.Height(100));
                if (!Sprite.Equals(statusSprite, statusDataInfo.StatusSprite) && statusSprite != null)
                {
                    string statusSpriteID = SpriteManager.GetName(statusSprite);
                    statusDataInfoUnit.Field("statusSpriteID", statusSpriteID).End();
                    statusDataInfoUnit.Field("statusSprite", null);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                //不同等级的具体状态
                Dictionary<int, StatusDataInfo.StatusLevelDataInfo> levelToDataDic = statusDataInfoUnit.Field<Dictionary<int, StatusDataInfo.StatusLevelDataInfo>>("levelToDataDic").Element;
                //等级
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("最大等级:" + (levelToDataDic.Count - 1), GUILayout.Width(70));
                if (levelToDataDic.Count > 0 && GUILayout.Button("-", GUILayout.Width(20)))
                {
                    if (EditorUtility.DisplayDialog("警告!", "将会减去最后一个等级的数据!", "确认", "取消"))
                        levelToDataDic.Remove(levelToDataDic.Count - 1);
                }
                if (GUILayout.Button("+", GUILayout.Width(20)))
                {
                    levelToDataDic.Add(levelToDataDic.Count, new StatusDataInfo.StatusLevelDataInfo());
                }
                EditorGUILayout.LabelField("当前选择:" + selectLevel, GUILayout.Width(70));
                if (selectLevel > 0 && GUILayout.Button("-", GUILayout.Width(20)))
                    selectLevel--;
                if (selectLevel < levelToDataDic.Count - 1 && GUILayout.Button("+", GUILayout.Width(20)))
                    selectLevel++;
                selectLevel = Mathf.Clamp(selectLevel, 0, levelToDataDic.Count - 1);
                EditorGUILayout.EndHorizontal();
                //具体状态
                if (selectLevel >= 0 && selectLevel < levelToDataDic.Count)
                {
                    StatusDataInfo.StatusLevelDataInfo statusLeveDataInfo = levelToDataDic[selectLevel];
                    EditorGUILayout.LabelField("具体说明");
                    statusLeveDataInfo.LevelExplane = EditorGUILayout.TextArea(statusLeveDataInfo.LevelExplane, GUILayout.Height(60));
                    //该等级的耗魔对应基础持续时间曲线
                    EditorGUILayout.LabelField("持续时间曲线设置:");
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("最小魔力:",GUILayout.Width(75));
                    statusLeveDataInfo.MinMana= EditorGUILayout.IntField(statusLeveDataInfo.MinMana, GUILayout.Width(75));
                    statusLeveDataInfo.DurationCuvre = EditorGUILayout.CurveField(statusLeveDataInfo.DurationCuvre, GUILayout.Width(150));
                    EditorGUILayout.LabelField("最大魔力:", GUILayout.Width(75));
                    statusLeveDataInfo.MaxMana = EditorGUILayout.IntField(statusLeveDataInfo.MaxMana, GUILayout.Width(75));
                    EditorGUILayout.EndHorizontal();
                    //设置具体的数据 
                    EditorGUILayout.LabelField("具体数据设置:");
                    StatusActionAttribute statusAction = fieldInfo.GetCustomAttributes(typeof(StatusActionAttribute), false).OfType<StatusActionAttribute>().FirstOrDefault();
                    if (statusAction != null)
                    {
                        EnumStatusAction[] enumStatusActions = statusAction.GetStatusActions();
                        //如果不存在该项则添加
                        foreach (EnumStatusAction enumStatusAction in enumStatusActions)
                        {
                            if (!statusLeveDataInfo.StatusActionDataInfoDic.ContainsKey(enumStatusAction))
                            {
                                Type t = assembly.GetType("StatusActionDataInfo_" + enumStatusAction.ToString());
                                if (t != null)
                                {
                                    StatusActionDataInfo_Base sb = Activator.CreateInstance(t) as StatusActionDataInfo_Base;
                                    statusLeveDataInfo.StatusActionDataInfoDic.Add(enumStatusAction, sb);
                                }
                            }
                        }
                        //如果多余则删除
                        IEnumerable<EnumStatusAction> checkEnumStatusActions = statusLeveDataInfo.StatusActionDataInfoDic.Keys.OfType<EnumStatusAction>();
                        foreach (EnumStatusAction item in checkEnumStatusActions)
                        {
                            if (!enumStatusActions.Contains(item))
                            {
                                statusLeveDataInfo.StatusActionDataInfoDic.Remove(item);
                            }
                        }
                        //循环
                        Type enumStatusActionType = typeof(EnumStatusAction);
                        foreach (KeyValuePair<EnumStatusAction, StatusActionDataInfo_Base> item in statusLeveDataInfo.StatusActionDataInfoDic)
                        {
                            FieldExplanAttribute fieldExplanAttribute = enumStatusActionType.GetField(item.Key.ToString()).GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().First();
                            if (fieldExplanAttribute != null)
                            {
                                EditorGUILayout.Space();
                                EditorGUILayout.LabelField(fieldExplanAttribute.GetExplan());//显示该状态效果的名字
                                Type statusActionDataInfoBaseType = item.Value.GetType();// assembly.GetType("StatusActionDataInfo_" + item.Key.ToString());
                                if (statusActionDataInfoBaseType != null)
                                {
                                    FieldInfo[] statusActionDataInfoBaseInfos = statusActionDataInfoBaseType.GetFields();
                                    //便利该效果对应的数据对象并显示
                                    foreach (FieldInfo statusActionDataInfoBaseInfo in statusActionDataInfoBaseInfos)
                                    {
                                        object innerValueData = statusActionDataInfoBaseInfo.GetValue(item.Value);
                                        FieldExplanAttribute innerAttribute = statusActionDataInfoBaseInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
                                        if (innerAttribute != null)
                                        {
                                            EditorGUILayout.BeginHorizontal();
                                            EditorGUILayout.LabelField(innerAttribute.GetExplan());
                                            if (statusActionDataInfoBaseInfo.FieldType.Equals(typeof(int)))
                                            {
                                                int innerValue = EditorGUILayout.IntField((int)innerValueData);
                                                statusActionDataInfoBaseInfo.SetValue(item.Value, innerValue);
                                            }
                                            else if (statusActionDataInfoBaseInfo.FieldType.Equals(typeof(float)))
                                            {
                                                float innerValue = EditorGUILayout.FloatField((float)innerValueData);
                                                statusActionDataInfoBaseInfo.SetValue(item.Value, innerValue);
                                            }
                                            else if (statusActionDataInfoBaseInfo.FieldType.Equals(typeof(string)))
                                            {
                                                string innerValue = EditorGUILayout.TextField((string)innerValueData);
                                                statusActionDataInfoBaseInfo.SetValue(item.Value, innerValue);
                                            }
                                            else if (statusActionDataInfoBaseInfo.FieldType.Equals(typeof(bool)))
                                            {
                                                bool innerValue = EditorGUILayout.Toggle((bool)innerValueData);
                                                statusActionDataInfoBaseInfo.SetValue(item.Value, innerValue);
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        catch
        {
            restart = false;
        }
    }
}
