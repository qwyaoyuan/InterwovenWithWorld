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

/// <summary>
/// 过场编辑器
/// </summary>
public class InterludesDataEditor : EditorWindow
{

    /// <summary>
    /// 保存过场信息的文件路径
    /// </summary>
    public string dataInterludesFilePath = @"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\Interludes\InterludesData.json";
    /// <summary>
    /// 保存摄像机动画的预设体的文件夹路径
    /// </summary>
    public string cameraPathDirectoryPath = @"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\Interludes\CameraPath";

    /// <summary>
    /// 摄像机动画显示选择路径
    /// </summary>
    public string cameraPathDirectoryShowWindowPath = "Assets/Scripts/Data/Resources/Data/Interludes/CameraPath";

    [MenuItem("小工具/过场编辑器")]
    static void AddWindow()
    {
        InterludesDataEditor interludesDataEditor = EditorWindow.GetWindow<InterludesDataEditor>();
        interludesDataEditor.Show();
    }

    /// <summary>
    /// 过场数据
    /// </summary>
    List<InterludesItemStruct> interludesItemStructList;

    /// <summary>
    /// 过场对于任务来说是开始还是结束显示说明字典
    /// </summary>
    List<KeyValuePair<InterludesItemStruct.EnumInterludesShowTime, string>> interludesShowTimeExplanList;

    /// <summary>
    /// 数据片段类型对应显示文本的字典
    /// </summary>
    Dictionary<Type, string> itemDataTypeToExplanDic;

    void Awake()
    {
        interludesShowTimeExplanList = new List<KeyValuePair<InterludesItemStruct.EnumInterludesShowTime, string>>();
        FieldExplanAttribute.SetEnumExplanDic(interludesShowTimeExplanList);

        itemDataTypeToExplanDic = new Dictionary<Type, string>();
        itemDataTypeToExplanDic.Add(typeof(InterludesDataInfo.ItemData), "空");
        itemDataTypeToExplanDic.Add(typeof(InterludesDataInfo.ItemData_Talk), "显示对话");
        itemDataTypeToExplanDic.Add(typeof(InterludesDataInfo.ItemData_CameraPathAnimation), "镜头动画");

        if (!File.Exists(dataInterludesFilePath))
        {
            File.Create(dataInterludesFilePath).Close();
            interludesItemStructList = new List<InterludesItemStruct>();
            string assetStr = SerializeNow(interludesItemStructList);
            File.WriteAllText(dataInterludesFilePath, assetStr, Encoding.UTF8);
        }
        else
        {
            string assetStr = File.ReadAllText(dataInterludesFilePath, Encoding.UTF8);
            interludesItemStructList = DeSerializeNow<List<InterludesItemStruct>>(assetStr);
            if (interludesItemStructList == null)
                interludesItemStructList = new List<InterludesItemStruct>();
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
        T target = JsonConvert.DeserializeObject<T>(value,
            new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
        return target;
    }

    private void OnInspectorUpdate()
    {
        base.Repaint();
    }

    /// <summary>
    /// 列表的滑动条
    /// </summary>
    Vector2 listScroll;
    /// <summary>
    /// 设置值时的滑动条
    /// </summary>
    Vector2 valueScroll;

    /// <summary>
    /// 当前选择的过场数据
    /// </summary>
    InterludesItemStruct selectInterludesItemStruct;

    private void OnGUI()
    {
        if (interludesItemStructList == null)
            return;
        if (itemDataTypeToExplanDic == null)
            return;
        EditorGUILayout.BeginHorizontal();
        //显示列表
        EditorGUILayout.BeginVertical(GUILayout.Width(200));
        if (GUILayout.Button("保存"))
        {
            string assetStr = SerializeNow(interludesItemStructList);
            File.WriteAllText(dataInterludesFilePath, assetStr, Encoding.UTF8);
            EditorUtility.DisplayDialog("提示!", "保存成功!", "确定");
        }
        if (GUILayout.Button("添加"))
        {
            int nextID = GetNextID();
            interludesItemStructList.Add(new InterludesItemStruct() { ID = nextID, TaskID = -1, InterludesShowTime = InterludesItemStruct.EnumInterludesShowTime.Start, InterludesDataInfo = new InterludesDataInfo() { Datas = new List<InterludesDataInfo.ItemData>() } });
        }
        listScroll = EditorGUILayout.BeginScrollView(listScroll);
        InterludesItemStruct[] tempInterludesItemStructArray = interludesItemStructList.ToArray();
        foreach (InterludesItemStruct tempInterludesItemStruct in tempInterludesItemStructArray)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("删除") && EditorUtility.DisplayDialog("警告!", "请再次确认是否删除?", "确认", "取消 "))
            {
                interludesItemStructList.Remove(tempInterludesItemStruct);
                if (selectInterludesItemStruct == tempInterludesItemStruct)
                {
                    selectInterludesItemStruct = interludesItemStructList.FirstOrDefault();
                }
            }
            if (selectInterludesItemStruct == tempInterludesItemStruct)
            {
                GUILayout.Space(10);
            }
            string buttonShow = "[" + tempInterludesItemStruct.ID + "]任务ID:" + tempInterludesItemStruct.TaskID + "--" + tempInterludesItemStruct.InterludesShowTime;
            if (GUILayout.Button(buttonShow))
            {
                selectInterludesItemStruct = tempInterludesItemStruct;
            }
            if (selectInterludesItemStruct != tempInterludesItemStruct)
            {
                GUILayout.Space(10);
            }
            EditorGUILayout.EndHorizontal();
        }


        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        //显示数据
        EditorGUILayout.BeginVertical();
        valueScroll = EditorGUILayout.BeginScrollView(valueScroll);
        if (selectInterludesItemStruct != null)
        {
            EditorGUILayout.LabelField("ID:" + selectInterludesItemStruct.ID);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("任务ID(输入-1则表示不适用于任务)");
            selectInterludesItemStruct.TaskID = EditorGUILayout.IntField(selectInterludesItemStruct.TaskID);
            EditorGUILayout.EndHorizontal();
            if (selectInterludesItemStruct.TaskID >= 0)
            {
                EditorGUILayout.BeginHorizontal();
                List<InterludesItemStruct.EnumInterludesShowTime> interludesShowTimeValues = interludesShowTimeExplanList.Select(temp => temp.Key).ToList();
                string[] interludesShowTimeExplans = interludesShowTimeExplanList.Select(temp => temp.Value).ToArray();
                int tempIndex = interludesShowTimeValues.IndexOf(selectInterludesItemStruct.InterludesShowTime);
                EditorGUILayout.LabelField("触发条件:");
                tempIndex = EditorGUILayout.Popup(tempIndex, interludesShowTimeExplans);
                if (tempIndex > -1)
                    selectInterludesItemStruct.InterludesShowTime = interludesShowTimeValues[tempIndex];
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("添加数据"))
            {
                selectInterludesItemStruct.InterludesDataInfo.Datas.Add(new InterludesDataInfo.ItemData());
            }

            List<Type> itemDataValues = itemDataTypeToExplanDic.Select(temp => temp.Key).ToList();
            string[] itemDataExplans = itemDataTypeToExplanDic.Select(temp => temp.Value).ToArray();
            InterludesDataInfo.ItemData[] itemDatas = selectInterludesItemStruct.InterludesDataInfo.Datas.ToArray();
            foreach (InterludesDataInfo.ItemData itemData in itemDatas)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("×", GUILayout.Width(20)) && EditorUtility.DisplayDialog("警告!", "请再次确认是否删除?", "确认", "取消"))
                {
                    selectInterludesItemStruct.InterludesDataInfo.Datas.Remove(itemData);
                }
                int tempIndex = selectInterludesItemStruct.InterludesDataInfo.Datas.IndexOf(itemData);
                if (GUILayout.Button("↑", GUILayout.Width(20)))
                {
                    if (tempIndex > 0)
                    {
                        selectInterludesItemStruct.InterludesDataInfo.Datas.Remove(itemData);
                        selectInterludesItemStruct.InterludesDataInfo.Datas.Insert(tempIndex - 1, itemData);
                    }
                }
                if (GUILayout.Button("↓", GUILayout.Width(20)))
                {
                    if (tempIndex < selectInterludesItemStruct.InterludesDataInfo.Datas.Count - 1)
                    {
                        selectInterludesItemStruct.InterludesDataInfo.Datas.Remove(itemData);
                        selectInterludesItemStruct.InterludesDataInfo.Datas.Insert(tempIndex + 1, itemData);
                    }
                }
                EditorGUILayout.EndHorizontal();
                if (itemData == null)
                {
                    EditorGUILayout.LabelField("该项为空");
                    continue;
                }
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("持续时间:", GUILayout.Width(50));
                itemData.baseKeepTime = EditorGUILayout.FloatField(itemData.baseKeepTime);
                EditorGUILayout.EndHorizontal();
                //切换类型
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("数据类型:", GUILayout.Width(50));
                var varItemDataValues = Enumerable.Range(0, itemDataValues.Count).Select(temp => new { _index = temp, _type = itemDataValues[temp] });
                var varItemDataValue = varItemDataValues.FirstOrDefault(temp => string.Equals(itemData.GetType().Name, temp._type.Name));
                int tempTypeIndex = varItemDataValue._index;
                int changeTempTypeIndex = EditorGUILayout.Popup(tempTypeIndex, itemDataExplans,GUILayout.Width(120));
                if (changeTempTypeIndex > -1 && changeTempTypeIndex != tempTypeIndex)// 切换了类型
                {
                    if (EditorUtility.DisplayDialog("提示!", "是否切换类型?注意切换类型后会丢失原有类型的数据!", "确认", "取消"))
                    {
                        Type itemDataType = itemDataValues[changeTempTypeIndex];
                        int nowIndex = selectInterludesItemStruct.InterludesDataInfo.Datas.IndexOf(itemData);
                        if (nowIndex > -1)
                        {
                            selectInterludesItemStruct.InterludesDataInfo.Datas[nowIndex] = Activator.CreateInstance(itemDataType) as InterludesDataInfo.ItemData;
                            if (selectInterludesItemStruct.InterludesDataInfo.Datas[nowIndex] == null)
                            {
                                selectInterludesItemStruct.InterludesDataInfo.Datas[nowIndex] = itemData;
                            }
                            selectInterludesItemStruct.InterludesDataInfo.Datas[nowIndex].baseKeepTime = itemData.baseKeepTime;
                        }
                    }
                }
                //显示不同类型的面板
                switch (itemData.GetType().Name)
                {
                    case "ItemData_Talk":
                        InterludesDataInfo.ItemData_Talk itemData_Talk = itemData as InterludesDataInfo.ItemData_Talk;
                        if (itemData_Talk != null)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("对话的ID:", GUILayout.Width(50));
                            itemData_Talk.TalkID = EditorGUILayout.IntField(itemData_Talk.TalkID);
                            EditorGUILayout.EndHorizontal();
                        }
                        break;
                    case "ItemData_CameraPathAnimation":
                        InterludesDataInfo.ItemData_CameraPathAnimation itemData_CameraPathAnimation = itemData as InterludesDataInfo.ItemData_CameraPathAnimation;
                        if (itemData_CameraPathAnimation != null)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("摄像机动画预设体:", GUILayout.Width(100));
                            EditorGUILayout.ObjectField(itemData_CameraPathAnimation.ObjPrefab, typeof(GameObject), false, GUILayout.Width(150));
                            if (GUILayout.Button("S", GUILayout.Width(20)))
                                ObjectSelectorWindow.get.Show(itemData_CameraPathAnimation.ObjPrefab, typeof(GameObject), null, resultObj => itemData_CameraPathAnimation.ObjPrefab = (GameObject)resultObj, cameraPathDirectoryShowWindowPath);
                            if (GUILayout.Button("创建"))
                            {
                                string prefabName = "Interludes_CameraPathAnimation_" + System.DateTime.Now.ToBinary();
                                //创建预设体
                                GameObject obj = new GameObject();
                                obj.transform.position = Vector3.forward;
                                obj.AddComponent<CameraPath>();
                                obj.AddComponent<CameraPathAnimator>();
                                UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab(cameraPathDirectoryShowWindowPath+"/"+ prefabName+".prefab");
                                //关联并设置到对象
                                itemData_CameraPathAnimation.ObjPrefab = PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
                                //然后删除对象
                                GameObject.DestroyImmediate(obj);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        break;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    IEnumerator WaiCreateSetTo(Func<bool> LoadFunc)
    {
        float runTime = 0;
        while (runTime < 5)
        {
            runTime += Time.deltaTime;
            yield return new WaitForSeconds(1);
            if (LoadFunc())
            {
                break;
            }
        }
    }

    /// <summary>
    /// 获取下一个可用ID
    /// </summary>
    /// <returns></returns>
    private int GetNextID()
    {
        int id = 0;
        foreach (InterludesItemStruct item in interludesItemStructList)
        {
            if (item.ID == id)
            {
                id++;
            }
            else
            {
                break;
            }
        }
        return id;
    }
}
