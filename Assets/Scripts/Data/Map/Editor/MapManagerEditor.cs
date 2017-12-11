using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using ReflectEncapsulation;

/// <summary>
/// 地图信息编辑器(地图的图片 场景大小等)
/// </summary>
public class MapManagerEditor : EditorWindow
{
    /// <summary>
    /// 保存地图配置信息的文件夹路径
    /// </summary>
    public string dataDirectoryPath = @"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\Map";

    /// <summary>
    /// 场景对应地图数组字典
    /// </summary>
    Dictionary<string, MapDataInfo> sceneToMapDataDic;

    [MenuItem("小工具/Map数据(图片、场景)编辑器")]
    static void AddWindow()
    {
        MapManagerEditor mapManagerEditor = EditorWindow.GetWindow<MapManagerEditor>();
        mapManagerEditor.Show();
    }

    private void Awake()
    {
        if (!Directory.Exists(dataDirectoryPath))
            Directory.CreateDirectory(dataDirectoryPath);
        if (!File.Exists(dataDirectoryPath + "/Map.txt"))
        {
            sceneToMapDataDic = new Dictionary<string, MapDataInfo>();
            File.Create(dataDirectoryPath + "/Map.txt").Close();
            string valueText = SerializeNow(sceneToMapDataDic);
            File.WriteAllText(dataDirectoryPath + "/Map.txt", valueText, Encoding.UTF8);
        }
        else
        {
            string valueText = File.ReadAllText(dataDirectoryPath + "/Map.txt", Encoding.UTF8);
            sceneToMapDataDic = DeSerializeNow<Dictionary<string, MapDataInfo>>(valueText);
            if (sceneToMapDataDic == null)
                sceneToMapDataDic = new Dictionary<string, MapDataInfo>();
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
    /// 临时显示的场景名
    /// </summary>
    string tempSceneName;

    /// <summary>
    /// 目标对象
    /// </summary>
    MapDataInfo tempMapDataInfo;

    /// <summary>
    /// 左侧的滑动条值
    /// </summary>
    Vector2 leftScroll;

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        //左侧具体的场景
        EditorGUILayout.BeginVertical(GUILayout.Width(135));
        if (GUILayout.Button("保存"))
        {
            string valueText = SerializeNow(sceneToMapDataDic);
            File.WriteAllText(dataDirectoryPath + "/Map.txt", valueText, Encoding.UTF8);
            EditorUtility.DisplayDialog("保存数据", "保存成功!", "确认");
        }
        EditorGUILayout.LabelField("场景名");
        tempSceneName = EditorGUILayout.TextField(tempSceneName);
        if (!string.IsNullOrEmpty(tempSceneName))
        {
            if (!sceneToMapDataDic.ContainsKey(tempSceneName))
            {
                if (GUILayout.Button("添加"))
                {
                    if (EditorUtility.DisplayDialog("提示!", "是否添加该场景的数据", "确认添加", "取消添加"))
                    {
                        sceneToMapDataDic.Add(tempSceneName, new MapDataInfo());
                    }
                }
            }
        }
        EditorGUILayout.Space();
        leftScroll = EditorGUILayout.BeginScrollView(leftScroll);
        KeyValuePair<string, MapDataInfo>[] values = new KeyValuePair<string, MapDataInfo>[0];
        if (string.IsNullOrEmpty(tempSceneName))
            values = sceneToMapDataDic.OfType<KeyValuePair<string, MapDataInfo>>().ToArray();
        else
            values = sceneToMapDataDic.Where(temp => temp.Key.Contains(tempSceneName)).ToArray();
        foreach (KeyValuePair<string, MapDataInfo> value in values)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("×", GUILayout.Width(20)))
            {
                if (EditorUtility.DisplayDialog("警告!", "是否删除该场景名的数据", "确认删除", "取消删除"))
                {
                    sceneToMapDataDic.Remove(value.Key);
                }
            }
            string newKey = EditorGUILayout.TextField(value.Key, GUILayout.Width(80));
            if (!string.Equals(value.Key, newKey) && !string.IsNullOrEmpty(newKey))
            {
                if (sceneToMapDataDic.ContainsKey(value.Key) && !sceneToMapDataDic.ContainsKey(newKey))
                {
                    sceneToMapDataDic.Remove(value.Key);
                    sceneToMapDataDic.Add(newKey, value.Value);
                }
            }
            bool isCheck = object.Equals(tempMapDataInfo, value.Value);
            isCheck = EditorGUILayout.Toggle(isCheck, GUILayout.Width(25));
            if (isCheck)
                tempMapDataInfo = value.Value;
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        //游戏详细的设置
        EditorGUILayout.BeginVertical();
        if (tempMapDataInfo != null && sceneToMapDataDic.ContainsValue(tempMapDataInfo))
        {
            string thisSceneName = sceneToMapDataDic.Where(temp => object.Equals(temp.Value, tempMapDataInfo)).Select(temp => temp.Key).FirstOrDefault();
            if (!string.IsNullOrEmpty(thisSceneName))
            {
                ReflectUnit<MapDataInfo> mapDataInfoUnit = Entry.On(tempMapDataInfo);
                //设置场景名
                if (!string.Equals(tempMapDataInfo.SceneName, thisSceneName))
                    mapDataInfoUnit.Field("sceneName", thisSceneName).End();
                EditorGUILayout.LabelField("场景名:" + tempMapDataInfo.SceneName);
                //设置边界 
                Rect sceneRect = EditorGUILayout.RectField("场景边界:", tempMapDataInfo.SceneRect);
                if (!Rect.Equals(sceneRect, tempMapDataInfo.SceneRect))
                {
                    mapDataInfoUnit.Set("SceneRect", sceneRect).End();
                }
                //设置地图图片
                tempMapDataInfo.Load();
                Sprite mapSprite = (Sprite)EditorGUILayout.ObjectField("场景的地图:", tempMapDataInfo.MapSprite, typeof(Sprite), true);
                if (!Sprite.Equals(mapSprite, tempMapDataInfo.MapSprite) && mapSprite != null)
                {
                    string mapSpriteID = SpriteManager.GetName(mapSprite);
                    mapDataInfoUnit.Field("mapSpriteID", mapSpriteID).End();
                }
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}
