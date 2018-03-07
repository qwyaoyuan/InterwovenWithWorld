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
using System.Reflection;

/// <summary>
/// 采集点编辑器
/// </summary>
public class StuffManagerEditor : EditorWindow
{
    /// <summary>
    /// 保存采集点信息的完整路径
    /// </summary>
    public string dataAllPath = "";//@"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\Stuff";

    /// <summary>
    /// 数据字典
    /// </summary>
    private Dictionary<string, TextAsset> dataDic;
    /// <summary>
    /// 采集点预设体字典
    /// </summary>
    private Dictionary<string, GameObject> stuffDataDic;
    /// <summary>
    /// 当前场景
    /// </summary>
    private string nowSceneName;

    /// <summary>
    /// 当前的采集点信息集合
    /// </summary>
    StuffDataInfoCollection nowStuffDataInfoCollection;

    /// <summary>
    /// 滑动条位置
    /// </summary>
    Vector2 scrollPostion;

    /// <summary>
    /// 物品类型对应名字字典
    /// </summary>
    Dictionary<int, string> goodsTypeToNameDic;

    [MenuItem("小工具/采集点编辑器")]
    static void AddWindow()
    {
        StuffManagerEditor stuffManagerEditor = EditorWindow.GetWindow<StuffManagerEditor>();
        stuffManagerEditor.Show();
    }

    private void Awake()
    {
        //重置路径
        dataAllPath = Application.dataPath + @"\Scripts\Data\Resources\Data\Stuff";

        dataDic = new Dictionary<string, TextAsset>();
        stuffDataDic = new Dictionary<string, GameObject>();

        TextAsset[] allTextAssets = Resources.LoadAll<TextAsset>(StuffData.dataDirectoryPath);
        foreach (TextAsset textAsset in allTextAssets)
        {
            dataDic.Add(textAsset.name, textAsset);
        }
        GameObject[] allPrefabs = Resources.LoadAll<GameObject>(StuffDataInfo.stuffPrefabDirectoryPath);
        foreach (GameObject prefab in allPrefabs)
        {
            stuffDataDic.Add(prefab.name, prefab);
        }
        goodsTypeToNameDic = new Dictionary<int, string>();
        //只放入材料类
        Type goodsType = typeof(EnumGoodsType);
        EnumGoodsType[] enumGoodsTypes = Enum.GetValues(typeof(EnumGoodsType)).OfType<EnumGoodsType>().
            Where(temp => temp > EnumGoodsType.MineralBig && temp < EnumGoodsType.IngotCasting).
            Where(temp => ((int)temp) % 1000 != 0).ToArray();
        foreach (EnumGoodsType enumGoodsType in enumGoodsTypes)
        {
            FieldInfo fieldInfo = goodsType.GetField(enumGoodsType.ToString());
            if (fieldInfo == null)
                continue;
            FieldExplanAttribute fieldExplanAttribute = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
            if (fieldExplanAttribute != null)
            {
                string explanName = fieldExplanAttribute.GetExplan();
                if (goodsTypeToNameDic.ContainsKey((int)enumGoodsType))
                    continue;
                goodsTypeToNameDic.Add((int)enumGoodsType, explanName);
            }
        }
    }

    private void OnDestroy()
    {
        if (nowStuffDataInfoCollection != null)
        {
            if (nowStuffDataInfoCollection.StuffDataInfos != null)
            {
                foreach (StuffDataInfo stuffDataInfo in nowStuffDataInfoCollection.StuffDataInfos)
                {
                    if (stuffDataInfo != null)
                    {
                        GameObject.DestroyImmediate(stuffDataInfo.StuffObj);
                    }
                }
            }
        }
        nowStuffDataInfoCollection = null;
    }

    private void OnInspectorUpdate()
    {
        this.Repaint();
    }

    private void Update()
    {
        Scene nowScene = SceneManager.GetActiveScene();
        if (!string.Equals(nowScene.name, this.nowSceneName))
        {
            this.nowSceneName = nowScene.name;
            //清理之前创建的采集点对象并和从新加载新的
            if (nowStuffDataInfoCollection != null)
            {
                if (nowStuffDataInfoCollection.StuffDataInfos != null)
                {
                    foreach (StuffDataInfo stuffDataInfo in nowStuffDataInfoCollection.StuffDataInfos)
                    {
                        if (stuffDataInfo != null)
                        {
                            GameObject.DestroyImmediate(stuffDataInfo.StuffObj);
                        }
                    }
                }
            }
            nowStuffDataInfoCollection = null;
            LoadNowAssetsObj();
        }
    }

    /// <summary>
    /// 加载当前场景的采集点对象
    /// </summary>
    private void LoadNowAssetsObj()
    {
        if (dataDic.ContainsKey(this.nowSceneName))
        {
            TextAsset nowSceneTextAsset = dataDic[this.nowSceneName];
            string assetText = Encoding.UTF8.GetString(nowSceneTextAsset.bytes);
            StuffDataInfoCollection stuffDataInfoCollection = DeSerializeNow<StuffDataInfoCollection>(assetText);
            if (stuffDataInfoCollection != null)
            {
                nowStuffDataInfoCollection = stuffDataInfoCollection;
                foreach (StuffDataInfo stuffDataInfo in nowStuffDataInfoCollection.StuffDataInfos)
                {
                    if (string.IsNullOrEmpty(stuffDataInfo.stuffPrefabName))
                        continue;
                    if (!stuffDataDic.ContainsKey(stuffDataInfo.stuffPrefabName))
                        continue;
                    stuffDataInfo.Load();
                }
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

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("路径信息请在StuffManagerEditor内自行设置");
        EditorGUILayout.LabelField("当前场景:" + nowSceneName);
        if (nowStuffDataInfoCollection == null)//如果当前场景不存在数据
        {
            if (GUILayout.Button("创建该场景的采集点信息数据"))
            {
                nowStuffDataInfoCollection = new StuffDataInfoCollection();
                nowStuffDataInfoCollection.sceneName = nowSceneName;
                if (File.Exists(dataAllPath + "\\" + nowSceneName + ".txt"))
                {
                    if (EditorUtility.DisplayDialog("警告!", "存在相同的文件" + nowSceneName + ".txt\r\n是否覆盖?", "确认覆盖", "取消覆盖"))
                    {
                        string valueText = SerializeNow<StuffDataInfoCollection>(nowStuffDataInfoCollection);
                        File.WriteAllText(dataAllPath + "\\" + nowSceneName + ".txt", valueText, Encoding.UTF8);
                    }
                    else
                    {
                        nowStuffDataInfoCollection = null;
                    }
                }
                else
                {
                    File.Create(dataAllPath + "\\" + nowSceneName + ".txt").Close();
                    string valueText = SerializeNow<StuffDataInfoCollection>(nowStuffDataInfoCollection);
                    File.WriteAllText(dataAllPath + "\\" + nowSceneName + ".txt", valueText, Encoding.UTF8);
                }
            }
        }
        else//如果当前场景存在数据
        {
            if (GUILayout.Button("保存数据"))
            {
                if (nowStuffDataInfoCollection != null && File.Exists(dataAllPath + "\\" + nowStuffDataInfoCollection.sceneName + ".txt"))
                {
                    foreach (StuffDataInfo tempStuffdataInfo in nowStuffDataInfoCollection.StuffDataInfos)
                    {
                        if (tempStuffdataInfo.StuffObj != null)
                        {
                            tempStuffdataInfo.StuffLocation = tempStuffdataInfo.StuffObj.transform.position;
                            tempStuffdataInfo.StuffAngle = tempStuffdataInfo.StuffObj.transform.eulerAngles;
                        }
                    }
                    string valueText = SerializeNow<StuffDataInfoCollection>(nowStuffDataInfoCollection);
                    File.WriteAllText(dataAllPath + "\\" + nowStuffDataInfoCollection.sceneName + ".txt", valueText, Encoding.UTF8);
                    EditorUtility.DisplayDialog("保存数据", "保存成功!", "确认");
                }
            }
            scrollPostion = EditorGUILayout.BeginScrollView(scrollPostion);
            StuffDataInfo[] stuffDataInfos = nowStuffDataInfoCollection.StuffDataInfos.ToArray();
            string[] showGoodsTypeNames = goodsTypeToNameDic.Values.ToArray();
            int[] showGoodsTypeIDs = goodsTypeToNameDic.Keys.ToArray();
            foreach (StuffDataInfo stuffDataInfo in stuffDataInfos)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("ID:", GUILayout.Width(20));
                stuffDataInfo.StuffID = EditorGUILayout.IntField(stuffDataInfo.StuffID, GUILayout.Width(30));
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Name:", GUILayout.Width(35));
                stuffDataInfo.StuffName = EditorGUILayout.TextField(stuffDataInfo.StuffName, GUILayout.Width(100));
                if (stuffDataInfo.StuffObj)
                {
                    stuffDataInfo.StuffObj.name = stuffDataInfo.StuffName;
                }
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Target:", GUILayout.Width(40));
                if (!string.IsNullOrEmpty(stuffDataInfo.stuffPrefabName) && stuffDataDic.ContainsKey(stuffDataInfo.stuffPrefabName))//如果存在该预设提则实例化并显示
                {
                    EditorGUILayout.ObjectField(stuffDataInfo.StuffObj, typeof(GameObject), true, GUILayout.Width(100));
                    if (GUILayout.Button("×", GUILayout.Width(25)))
                    {
                        if (EditorUtility.DisplayDialog("警告!", "是否删除该游戏对象(重新选择预设提)", "是", "否"))
                        {
                            stuffDataInfo.stuffPrefabName = "";
                            if (stuffDataInfo.StuffObj != null)
                                GameObject.DestroyImmediate(stuffDataInfo.StuffObj);
                            stuffDataInfo.StuffObj = null;
                            stuffDataInfo.Load();
                        }
                    }
                }
                else
                {
                    List<string> names = stuffDataDic.Keys.ToList();
                    int index = names.IndexOf(stuffDataInfo.stuffPrefabName);
                    index = EditorGUILayout.Popup(index, names.ToArray(), GUILayout.Width(100));
                    if (index >= 0)
                    {
                        stuffDataInfo.stuffPrefabName = names[index];
                        stuffDataInfo.Load();
                    }
                }
                EditorGUILayout.Space();
                int nowID = (int)stuffDataInfo.StuffType;
                EditorGUILayout.LabelField("Type:", GUILayout.Width(35));
                nowID = EditorGUILayout.IntPopup(nowID, showGoodsTypeNames, showGoodsTypeIDs);
                stuffDataInfo.StuffType = (EnumGoodsType)nowID;
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Count:", GUILayout.Width(40));
                stuffDataInfo.StuffCount = EditorGUILayout.IntField(stuffDataInfo.StuffCount, GUILayout.Width(30));
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Cooling:", GUILayout.Width(50));
                stuffDataInfo.CoolingTime = EditorGUILayout.FloatField(stuffDataInfo.CoolingTime, GUILayout.Width(35));
                EditorGUILayout.Space();
                if (GUILayout.Button("删除"))
                {
                    if (EditorUtility.DisplayDialog("警告!", "请再次确认是否删除?", "确认", "取消"))
                    {
                        if (stuffDataInfo != null)
                        {
                            if (stuffDataInfo.StuffObj != null)
                                GameObject.DestroyImmediate(stuffDataInfo.StuffObj);
                            nowStuffDataInfoCollection.StuffDataInfos.Remove(stuffDataInfo);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            if (GUILayout.Button("添加"))
            {
                nowStuffDataInfoCollection.StuffDataInfos.Add(
                    new StuffDataInfo()
                    {
                        StuffID = nowStuffDataInfoCollection.StuffDataInfos.Count() > 0 ? (nowStuffDataInfoCollection.StuffDataInfos.Max(temp => temp.StuffID) + 1) : 0
                    });
            }
        }
        EditorGUILayout.EndVertical();
    }
}

