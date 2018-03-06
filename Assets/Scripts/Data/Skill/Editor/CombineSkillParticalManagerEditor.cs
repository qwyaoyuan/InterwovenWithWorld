using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using ReflectEncapsulation;
using System.Linq;
using System;
using System.Reflection;

/// <summary>
/// 组合技能粒子管理器
/// </summary>
public class CombineSkillParticalManagerEditor : EditorWindow
{
    /// <summary>
    /// 保存组合技能粒子信息的完整目录路径
    /// </summary>
    public string dataDirecotryPath = "";//@"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\Skill\CombinePartical";

    /// <summary>
    /// 组合技能与粒子对应的字典
    /// </summary>
    Dictionary<int, string> skillTypeToParticalNameDic;

    [MenuItem("小工具/技能粒子选择器")]
    static void AddWindow()
    {
        CombineSkillParticalManagerEditor combineSkillParticalManagerEditor = EditorWindow.GetWindow<CombineSkillParticalManagerEditor>();
        combineSkillParticalManagerEditor.Show();
    }

    private void Awake()
    {
        //重置路径
        dataDirecotryPath = Application.dataPath + @"\Scripts\Data\Resources\Data\Skill\CombinePartical";

        allSkillTypes = Enum.GetValues(typeof(EnumSkillType)).OfType<EnumSkillType>().ToArray();
        if (!Directory.Exists(dataDirecotryPath))
            Directory.CreateDirectory(dataDirecotryPath);
        if (!File.Exists(dataDirecotryPath + "/CombinePartical.txt"))
        {
            skillTypeToParticalNameDic = new Dictionary<int, string>();
            File.Create(dataDirecotryPath + "/CombinePartical.txt").Close();
            string valueText = SerializeNow(skillTypeToParticalNameDic);
            File.WriteAllText(dataDirecotryPath + "/CombinePartical.txt", valueText, Encoding.UTF8);
        }
        else
        {
            string valueText = File.ReadAllText(dataDirecotryPath + "/CombinePartical.txt", Encoding.UTF8);
            skillTypeToParticalNameDic = DeSerializeNow<Dictionary<int, string>>(valueText);
            if (skillTypeToParticalNameDic == null)
                skillTypeToParticalNameDic = new Dictionary<int, string>();
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
        string value = JsonConvert.SerializeObject(target, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
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
        T target = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
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
    int selectKey = -1;

    /// <summary>
    /// 当前选择项的技能组合类型数组
    /// </summary>
    EnumSkillType[] nowSelectSkillTyps;

    /// <summary>
    /// 所有的技能类型 
    /// </summary>
    EnumSkillType[] allSkillTypes;

    private void OnGUI()
    {
        if (skillTypeToParticalNameDic == null)
            return;
        EditorGUILayout.BeginHorizontal();
        //左侧的选择配置项按钮面板
        EditorGUILayout.BeginVertical(GUILayout.Width(250));
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("保存", GUILayout.Width(35)))
        {
            string valueText = SerializeNow(skillTypeToParticalNameDic);
            File.WriteAllText(dataDirecotryPath + "/CombinePartical.txt", valueText, Encoding.UTF8);
            EditorUtility.DisplayDialog("保存数据", "保存成功!", "确认");
        }
        GUILayout.Space(100);
        if (GUILayout.Button("增量加载资源", GUILayout.Width(100)))
        {
            ParticalManager.IncrementalLoad();
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("添加", GUILayout.Width(35)))
        {
            if (!skillTypeToParticalNameDic.ContainsKey(-1))
                skillTypeToParticalNameDic.Add(-1, "None");
            else
                EditorUtility.DisplayDialog("提示", "请先编辑之前添加的数据!", "确认");
        }
        leftScroll = EditorGUILayout.BeginScrollView(leftScroll);
        KeyValuePair<int, string>[] tempValues = skillTypeToParticalNameDic.ToArray();
        Type enumType = typeof(EnumSkillType);
        foreach (KeyValuePair<int, string> tempValue in tempValues)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("×", GUILayout.Width(15)))
            {
                if (EditorUtility.DisplayDialog("警告!", "将会减去该数据!", "确认", "取消"))
                {
                    skillTypeToParticalNameDic.Remove(tempValue.Key);
                }
            }
            if (object.Equals(tempValue.Key, selectKey))
                GUILayout.Space(30);
            string[] showNames = new string[0];
            EnumSkillType[] thisSkillTyps = SkillCombineStaticTools.GetCombineSkills(tempValue.Key);
            if (thisSkillTyps != null)
                showNames = thisSkillTyps.Select<EnumSkillType, string>(temp =>
                  {
                      FieldInfo fieldInfo = enumType.GetField(temp.ToString());
                      if (fieldInfo == null)
                          return "";
                      FieldExplanAttribute fieldExplanAttribute = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).Select(innerTemp => innerTemp as FieldExplanAttribute).Where(innerTemp => innerTemp != null).FirstOrDefault();
                      if (fieldExplanAttribute == null)
                          return "";
                      return fieldExplanAttribute.GetExplan();
                  }).Where(temp => !string.IsNullOrEmpty(temp)).ToArray();
            string showName = showNames.Length > 0 ? string.Join("+", showNames) : "None";
            if (GUILayout.Button(showName))
            {
                if (object.Equals(tempValue.Key, selectKey))
                    selectKey = 0;
                else
                {
                    selectKey = tempValue.Key;
                    nowSelectSkillTyps = thisSkillTyps;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        //第一个技能
        EditorGUILayout.BeginVertical();
        if (nowSelectSkillTyps == null || nowSelectSkillTyps.Length < 1)
            nowSelectSkillTyps = new EnumSkillType[] { EnumSkillType.None };
        var enumSkillTypes_First = allSkillTypes.Select(temp =>
        {
            FieldInfo fieldInfo = enumType.GetField(temp.ToString());
            if (fieldInfo == null)
                return new { type = temp, str = "" };
            FieldExplanAttribute fieldExplanAttriubte = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().Where(innerTemp => innerTemp != null).FirstOrDefault();
            if (fieldExplanAttriubte == null)
                return new { type = temp, str = "" };
            return new { type = temp, str = fieldExplanAttriubte.GetExplan() };
        }).Where(temp => !string.IsNullOrEmpty(temp.str)).Where(temp => temp.type < EnumSkillType.MagicCombinedLevel1End).Reverse();
        int index_First = enumSkillTypes_First.Select(temp => temp.type).ToList().IndexOf(nowSelectSkillTyps[0]);
        int nowIndex_First = EditorGUILayout.Popup(index_First, enumSkillTypes_First.Select(temp => temp.str).ToArray());
        if (nowIndex_First > -1 && nowIndex_First < enumSkillTypes_First.Count())
        {
            EnumSkillType selectEnumSkillType = enumSkillTypes_First.ToArray()[nowIndex_First].type;
            nowSelectSkillTyps[0] = selectEnumSkillType;
        }
        if (nowSelectSkillTyps[0] < EnumSkillType.MagicCombinedLevel1Start)
        {
            EnumSkillType[] tempArray = nowSelectSkillTyps;
            nowSelectSkillTyps = new EnumSkillType[1];
            Array.Copy(tempArray, nowSelectSkillTyps, 1);
        }
        //第二个技能
        if (nowSelectSkillTyps.Length > 0 && nowSelectSkillTyps[0] > EnumSkillType.MagicCombinedLevel1Start)
        {
            if (nowSelectSkillTyps.Length < 2)
            {
                EnumSkillType[] tempArray = nowSelectSkillTyps;
                nowSelectSkillTyps = new EnumSkillType[2];
                Array.Copy(tempArray, nowSelectSkillTyps, 1);
            }
            var enumSkillTypes_Second = allSkillTypes.Select(temp =>
            {
                FieldInfo fieldInfo = enumType.GetField(temp.ToString());
                if (fieldInfo == null)
                    return new { type = temp, str = "" };
                FieldExplanAttribute fieldExplanAttriubte = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().Where(innerTemp => innerTemp != null).FirstOrDefault();
                if (fieldExplanAttriubte == null)
                    return new { type = temp, str = "" };
                return new { type = temp, str = fieldExplanAttriubte.GetExplan() };
            }).Where(temp => !string.IsNullOrEmpty(temp.str)).Where(temp => temp.type < EnumSkillType.MagicCombinedLevel2End && temp.type > EnumSkillType.MagicCombinedLevel2Start);
            int index_Second = enumSkillTypes_Second.Select(temp => temp.type).ToList().IndexOf(nowSelectSkillTyps[1]);
            int nowIndex_Second = EditorGUILayout.Popup(index_Second, enumSkillTypes_Second.Select(temp => temp.str).ToArray());
            if (nowIndex_Second > -1 && nowIndex_Second < enumSkillTypes_Second.Count())
            {
                EnumSkillType selectEnumSkillType = enumSkillTypes_Second.ToArray()[nowIndex_Second].type;
                nowSelectSkillTyps[1] = selectEnumSkillType;
            }
        }
        //第三个技能
        if (nowSelectSkillTyps.Length > 1 && nowSelectSkillTyps[1] > EnumSkillType.MagicCombinedLevel2Start)
        {
            if (nowSelectSkillTyps.Length < 3)
            {
                EnumSkillType[] tempArray = nowSelectSkillTyps;
                nowSelectSkillTyps = new EnumSkillType[3];
                Array.Copy(tempArray, nowSelectSkillTyps, 2);
            }
            var enumSkillTypes_Third = allSkillTypes.Select(temp =>
            {
                FieldInfo fieldInfo = enumType.GetField(temp.ToString());
                if (fieldInfo == null)
                    return new { type = temp, str = "" };
                FieldExplanAttribute fieldExplanAttriubte = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().Where(innerTemp => innerTemp != null).FirstOrDefault();
                if (fieldExplanAttriubte == null)
                    return new { type = temp, str = "" };
                return new { type = temp, str = fieldExplanAttriubte.GetExplan() };
            }).Where(temp => !string.IsNullOrEmpty(temp.str)).Where(temp => temp.type < EnumSkillType.MagicCombinedLevel3End && temp.type > EnumSkillType.MagicCombinedLevel3Start);
            int index_Third = enumSkillTypes_Third.Select(temp => temp.type).ToList().IndexOf(nowSelectSkillTyps[2]);
            int nowIndex_Third = EditorGUILayout.Popup(index_Third, enumSkillTypes_Third.Select(temp => temp.str).ToArray());
            if (nowIndex_Third > -1 && nowIndex_Third < enumSkillTypes_Third.Count())
            {
                EnumSkillType selectEnumSkillType = enumSkillTypes_Third.ToArray()[nowIndex_Third].type;
                nowSelectSkillTyps[2] = selectEnumSkillType;
            }
        }
        //第四个技能
        if (nowSelectSkillTyps.Length > 2 && nowSelectSkillTyps[2] > EnumSkillType.MagicCombinedLevel3Start)
        {
            if (nowSelectSkillTyps.Length < 4)
            {
                EnumSkillType[] tempArray = nowSelectSkillTyps;
                nowSelectSkillTyps = new EnumSkillType[4];
                Array.Copy(tempArray, nowSelectSkillTyps, 3);
            }
            var enumSkillTypes_Fourth = allSkillTypes.Select(temp =>
            {
                FieldInfo fieldInfo = enumType.GetField(temp.ToString());
                if (fieldInfo == null)
                    return new { type = temp, str = "" };
                FieldExplanAttribute fieldExplanAttriubte = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().Where(innerTemp => innerTemp != null).FirstOrDefault();
                if (fieldExplanAttriubte == null)
                    return new { type = temp, str = "" };
                return new { type = temp, str = fieldExplanAttriubte.GetExplan() };
            }).Where(temp => !string.IsNullOrEmpty(temp.str)).Where(temp => temp.type < EnumSkillType.MagicCombinedLevel4End && temp.type > EnumSkillType.MagicCombinedLevel4Start);
            int index_Fourth = enumSkillTypes_Fourth.Select(temp => temp.type).ToList().IndexOf(nowSelectSkillTyps[3]);
            int nowIndex_Fourth = EditorGUILayout.Popup(index_Fourth, enumSkillTypes_Fourth.Select(temp => temp.str).ToArray());
            if (nowIndex_Fourth > -1 && nowIndex_Fourth < enumSkillTypes_Fourth.Count())
            {
                EnumSkillType selectEnumSkillType = enumSkillTypes_Fourth.ToArray()[nowIndex_Fourth].type;
                nowSelectSkillTyps[3] = selectEnumSkillType;
            }
        }
        //技能粒子对象
        if (skillTypeToParticalNameDic.ContainsKey(selectKey))
        {
            GameObject particalObj = (GameObject)EditorGUILayout.ObjectField(ParticalManager.GetPartical(skillTypeToParticalNameDic[selectKey]), typeof(GameObject), true);
            skillTypeToParticalNameDic[selectKey] = ParticalManager.GetName(particalObj);
        }
        if (GUILayout.Button("保存", GUILayout.Width(35)))
        {
            int key = SkillCombineStaticTools.GetCombineSkillKey(nowSelectSkillTyps);
            if (key == 0 || (key != selectKey && skillTypeToParticalNameDic.ContainsKey(key)))
            {
                EditorUtility.DisplayDialog("提示", "无法保存该数据!", "确认");
            }
            else
            {
                string particalName = "";
                if (skillTypeToParticalNameDic.ContainsKey(selectKey))
                {
                    particalName = skillTypeToParticalNameDic[selectKey];
                    skillTypeToParticalNameDic.Remove(selectKey);
                }
                skillTypeToParticalNameDic.Add(key, particalName);
                selectKey = key;
            }
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}
