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
/// 功能交互编辑器
/// </summary>
public class ActionInteractiveDataEditor : EditorWindow
{

    [MenuItem("小工具/功能交互编辑器")]
    static void AddWindow()
    {
        ActionInteractiveDataEditor actionInteractiveDataEditor = EditorWindow.GetWindow<ActionInteractiveDataEditor>();
        actionInteractiveDataEditor.Show();
    }

    /// <summary>
    /// 保存功能交互信息的完整路径
    /// </summary>
    public string dataAllPath = "";//@"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\ActionInteractiveData";

    /// <summary>
    /// 数据字典
    /// </summary>
    private Dictionary<string, TextAsset> dataDic;
    /// <summary>
    /// 功能交互预设体字典
    /// </summary>
    private Dictionary<string, GameObject> prefabDataDic;
    /// <summary>
    /// 当前场景
    /// </summary>
    private string nowSceneName;
    /// <summary>
    /// 当前的功能交互信息集合
    /// </summary>
    ActionInteractiveDataInfoCollection nowActionInteractiveDataInfoCollection;

    /// <summary>
    /// 滑动条位置
    /// </summary>
    Vector2 scrollPostion;

    /// <summary>
    /// 交互功能类型对应显示名键值对集合
    /// </summary>
    public static List<KeyValuePair<EnumActionInteractiveType, string>> actionInteractiveTypeToNameList;

    private void Awake()
    {
        base.titleContent = new GUIContent("功能交互编辑器");

        //重置路径
        dataAllPath = Application.dataPath + @"\Scripts\Data\Resources\Data\ActionInteractiveData";

        dataDic = new Dictionary<string, TextAsset>();
        prefabDataDic = new Dictionary<string, GameObject>();

        TextAsset[] allTextAssets = Resources.LoadAll<TextAsset>(ActionInteractiveData.dataDirectoryPath);
        foreach (TextAsset textAsset in allTextAssets)
        {
            dataDic.Add(textAsset.name, textAsset);
        }
        GameObject[] allPrefabs = Resources.LoadAll<GameObject>(ActionInteractiveDataInfo.prefabDirectoryPath);
        foreach (GameObject prefab in allPrefabs)
        {
            prefabDataDic.Add(prefab.name, prefab);
        }
        actionInteractiveTypeToNameList = new List<KeyValuePair<EnumActionInteractiveType, string>>();
        FieldExplanAttribute.SetEnumExplanDic(actionInteractiveTypeToNameList);
    }

    private void OnDestroy()
    {
        if (nowActionInteractiveDataInfoCollection != null)
        {
            if (nowActionInteractiveDataInfoCollection.ActionInteractiveDataInfos != null)
            {
                foreach (ActionInteractiveDataInfo actionInteractiveDataInfo in nowActionInteractiveDataInfoCollection.ActionInteractiveDataInfos)
                {
                    if (actionInteractiveDataInfo != null)
                    {
                        GameObject.DestroyImmediate(actionInteractiveDataInfo.ActionInteractiveObj);
                    }
                }
            }
        }
        nowActionInteractiveDataInfoCollection = null;
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
            //清理之前创建的对象并重新加载新的
            if (nowActionInteractiveDataInfoCollection != null)
            {
                if (nowActionInteractiveDataInfoCollection.ActionInteractiveDataInfos != null)
                {
                    foreach (ActionInteractiveDataInfo actionInteractiveDataInfo in nowActionInteractiveDataInfoCollection.ActionInteractiveDataInfos)
                    {
                        if (actionInteractiveDataInfo != null)
                        {
                            GameObject.DestroyImmediate(actionInteractiveDataInfo.ActionInteractiveObj);
                        }
                    }
                }
            }
            nowActionInteractiveDataInfoCollection = null;
            LoadNowAssetsObj();
        }
    }

    /// <summary>
    /// 加载当前场景的功能交互信息
    /// </summary>
    private void LoadNowAssetsObj()
    {
        if (dataDic.ContainsKey(this.nowSceneName))
        {
            TextAsset nowSceneTextAsset = dataDic[this.nowSceneName];
            string assetText = Encoding.UTF8.GetString(nowSceneTextAsset.bytes);
            ActionInteractiveDataInfoCollection actionInteractiveDataInfoCollection = DeSerializeNow<ActionInteractiveDataInfoCollection>(assetText);
            if (actionInteractiveDataInfoCollection != null)
            {
                nowActionInteractiveDataInfoCollection = actionInteractiveDataInfoCollection;
                foreach (ActionInteractiveDataInfo actionInteractiveDataInfo in nowActionInteractiveDataInfoCollection.ActionInteractiveDataInfos)
                {
                    if (string.IsNullOrEmpty(actionInteractiveDataInfo.actionInteractivePrefabName))
                        continue;
                    if (!prefabDataDic.ContainsKey(actionInteractiveDataInfo.actionInteractivePrefabName))
                        continue;
                    actionInteractiveDataInfo.Load(true);
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
        EditorGUILayout.LabelField("路径信息请在ActionInteractiveDataEditor内自行设置");
        EditorGUILayout.LabelField("当前场景:" + nowSceneName);
        if (nowActionInteractiveDataInfoCollection == null)//如果当前场景不存在数据
        {
            if (GUILayout.Button("创建该场景的功能交互信息数据"))
            {
                nowActionInteractiveDataInfoCollection = new ActionInteractiveDataInfoCollection();
                nowActionInteractiveDataInfoCollection.sceneName = nowSceneName;
                if (File.Exists(dataAllPath + "\\" + nowSceneName + ".txt"))
                {
                    if (EditorUtility.DisplayDialog("警告!", "存在相同的文件" + nowSceneName + ".txt\r\n是否覆盖?", "确认覆盖", "取消覆盖"))
                    {
                        string valueText = SerializeNow<ActionInteractiveDataInfoCollection>(nowActionInteractiveDataInfoCollection);
                        File.WriteAllText(dataAllPath + "\\" + nowSceneName + ".txt", valueText, Encoding.UTF8);
                    }
                    else
                    {
                        nowActionInteractiveDataInfoCollection = null;
                    }
                }
                else
                {
                    File.Create(dataAllPath + "\\" + nowSceneName + ".txt").Close();
                    string valueText = SerializeNow<ActionInteractiveDataInfoCollection>(nowActionInteractiveDataInfoCollection);
                    File.WriteAllText(dataAllPath + "\\" + nowSceneName + ".txt", valueText, Encoding.UTF8);
                }
            }
        }
        else//如果当前场景存在数据
        {
            if (GUILayout.Button("保存数据"))
            {
                if (nowActionInteractiveDataInfoCollection != null && File.Exists(dataAllPath + "\\" + nowActionInteractiveDataInfoCollection.sceneName + ".txt"))
                {
                    foreach (ActionInteractiveDataInfo tempActionInteractiveDataInfo in nowActionInteractiveDataInfoCollection.ActionInteractiveDataInfos)
                    {
                        if (tempActionInteractiveDataInfo.ActionInteractiveObj != null)
                        {
                            tempActionInteractiveDataInfo.ActionInteractiveLocation = tempActionInteractiveDataInfo.ActionInteractiveObj.transform.position;
                            tempActionInteractiveDataInfo.ActionInteractiveAngle = tempActionInteractiveDataInfo.ActionInteractiveObj.transform.eulerAngles;
                        }
                    }
                    string valueText = SerializeNow<ActionInteractiveDataInfoCollection>(nowActionInteractiveDataInfoCollection);
                    File.WriteAllText(dataAllPath + "\\" + nowActionInteractiveDataInfoCollection.sceneName + ".txt", valueText, Encoding.UTF8);
                    EditorUtility.DisplayDialog("保存数据", "保存成功!", "确认");
                }
            }

            scrollPostion = EditorGUILayout.BeginScrollView(scrollPostion);
            ActionInteractiveDataInfo[] actionInteractiveDataInfos = nowActionInteractiveDataInfoCollection.ActionInteractiveDataInfos.ToArray();
            List<EnumActionInteractiveType> actionInteractiveTypeValues = actionInteractiveTypeToNameList.Select(temp => temp.Key).ToList();
            string[] actionInteractiveTypeExplans = actionInteractiveTypeToNameList.Select(temp => temp.Value).ToArray();
            foreach (ActionInteractiveDataInfo actionInteractiveDataInfo in actionInteractiveDataInfos)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("ID:", GUILayout.Width(20));
                actionInteractiveDataInfo.ActionInteractiveID = EditorGUILayout.IntField(actionInteractiveDataInfo.ActionInteractiveID, GUILayout.Width(30));
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Name:", GUILayout.Width(35));
                actionInteractiveDataInfo.ActionInteractiveName = EditorGUILayout.TextField(actionInteractiveDataInfo.ActionInteractiveName, GUILayout.Width(100));
                if (actionInteractiveDataInfo.ActionInteractiveObj)
                {
                    actionInteractiveDataInfo.ActionInteractiveObj.name = actionInteractiveDataInfo.ActionInteractiveName;
                }
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Target:", GUILayout.Width(40));
                if (!string.IsNullOrEmpty(actionInteractiveDataInfo.actionInteractivePrefabName) && prefabDataDic.ContainsKey(actionInteractiveDataInfo.actionInteractivePrefabName))//如果存在该预设提则实例化并显示
                {
                    EditorGUILayout.ObjectField(actionInteractiveDataInfo.ActionInteractiveObj, typeof(GameObject), true, GUILayout.Width(100));
                    if (GUILayout.Button("×", GUILayout.Width(25)))
                    {
                        if (EditorUtility.DisplayDialog("警告!", "是否删除该游戏对象(重新选择预设提)", "是", "否"))
                        {
                            actionInteractiveDataInfo.actionInteractivePrefabName = "";
                            if (actionInteractiveDataInfo.ActionInteractiveObj != null)
                                GameObject.DestroyImmediate(actionInteractiveDataInfo.ActionInteractiveObj);
                            actionInteractiveDataInfo.ActionInteractiveObj = null;
                            actionInteractiveDataInfo.Load(true);
                        }
                    }
                }
                else
                {
                    List<string> names = prefabDataDic.Keys.ToList();
                    int index = names.IndexOf(actionInteractiveDataInfo.actionInteractivePrefabName);
                    index = EditorGUILayout.Popup(index, names.ToArray(), GUILayout.Width(100));
                    if (index >= 0)
                    {
                        actionInteractiveDataInfo.actionInteractivePrefabName = names[index];
                        actionInteractiveDataInfo.Load(true);
                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("DataType:", GUILayout.Width(50));
                int actionInteractiveTypeIndex = actionInteractiveTypeValues.IndexOf(actionInteractiveDataInfo.ActionInteractiveType);
                int _actionInteractiveTypeIndex = EditorGUILayout.Popup(actionInteractiveTypeIndex, actionInteractiveTypeExplans, GUILayout.Width(120));
                if (_actionInteractiveTypeIndex >= 0 && _actionInteractiveTypeIndex != actionInteractiveTypeIndex)
                {
                    actionInteractiveDataInfo.ActionInteractiveType = actionInteractiveTypeValues[_actionInteractiveTypeIndex];
                    switch (actionInteractiveDataInfo.ActionInteractiveType)
                    {
                        case EnumActionInteractiveType.None:
                            actionInteractiveDataInfo.OtherValue = null;
                            break;
                        case EnumActionInteractiveType.TreasureBox:
                            actionInteractiveDataInfo.OtherValue = new Dictionary<EnumGoodsType,  ActionInteractiveDataInfo.TreasureBoxStruct>();
                            break;
                        case EnumActionInteractiveType.Other:
                            actionInteractiveDataInfo.OtherValue = new ActionInteractiveDataInfo.OtherDataStruct();
                            break;
                    }
                }
                switch (actionInteractiveDataInfo.ActionInteractiveType)
                {
                    case EnumActionInteractiveType.TreasureBox:
                    case EnumActionInteractiveType.Other:
                        if (GUILayout.Button("编辑", GUILayout.Width(35)))
                        {
                            ActionInteractiveDataStructEditor actionInteractiveDataStructEditor = EditorWindow.GetWindow<ActionInteractiveDataStructEditor>();
                            actionInteractiveDataStructEditor.Show();
                            actionInteractiveDataStructEditor.actionInteractiveDataInfo = actionInteractiveDataInfo;
                        }
                        break;
                }
                EditorGUILayout.Space();
                if (GUILayout.Button("删除"))
                {
                    if (EditorUtility.DisplayDialog("警告!", "请再次确认是否删除?", "确认", "取消"))
                    {
                        if (actionInteractiveDataInfo != null)
                        {
                            if (actionInteractiveDataInfo.ActionInteractiveObj != null)
                                GameObject.DestroyImmediate(actionInteractiveDataInfo.ActionInteractiveObj);
                            nowActionInteractiveDataInfoCollection.ActionInteractiveDataInfos.Remove(actionInteractiveDataInfo);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            if (GUILayout.Button("添加"))
            {
                nowActionInteractiveDataInfoCollection.ActionInteractiveDataInfos.Add(
                    new ActionInteractiveDataInfo()
                    {
                        ActionInteractiveID = nowActionInteractiveDataInfoCollection.ActionInteractiveDataInfos.Count() > 0 ? (nowActionInteractiveDataInfoCollection.ActionInteractiveDataInfos.Max(temp => temp.ActionInteractiveID) + 1) : 0,
                        SceneName = nowActionInteractiveDataInfoCollection.sceneName
                    });
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}

/// <summary>
/// 功能交互对象的附加数据数据编辑器
/// </summary>
public class ActionInteractiveDataStructEditor : EditorWindow
{
    /// <summary>
    /// 要编辑的目标
    /// </summary>
    public ActionInteractiveDataInfo actionInteractiveDataInfo;

    /// <summary>
    /// 物品对应说明键值对集合
    /// </summary>
    List<KeyValuePair<EnumGoodsType, string>> goodsTypeToExplanList;

    /// <summary>
    /// 品质对应说明键值对集合
    /// </summary>
    List<KeyValuePair<EnumQualityType, string>> qualityTypeToExplanList;

    /// <summary>
    /// 其他类型的类型集合
    /// </summary>
    List<Type> otherDataTypes;

    private void Awake()
    {
        base.titleContent = new GUIContent("功能交互编辑器-->附加数据");
        goodsTypeToExplanList = new List<KeyValuePair<EnumGoodsType, string>>();
        FieldExplanAttribute.SetEnumExplanDic(goodsTypeToExplanList, 0, temp => ((int)temp) % 1000 != 0);

        qualityTypeToExplanList = new List<KeyValuePair<EnumQualityType, string>>();
        FieldExplanAttribute.SetEnumExplanDic(qualityTypeToExplanList);

        Type[] tempOtherDataTypes = typeof(ActionInteractiveDataInfoMono).Assembly.GetTypes();
        otherDataTypes = tempOtherDataTypes.Where(temp => temp.IsSubclassOf(typeof(ActionInteractiveDataInfoMono)) && !temp.Equals(typeof(ActionInteractiveDataInfoMono_TreasureBox))).ToList();
    }

    private void OnInspectorUpdate()
    {
        base.Repaint();
    }

    /// <summary>
    /// 选择商品的下标
    /// </summary>
    int selectGoodsIndex;

    private void OnGUI()
    {
        if (actionInteractiveDataInfo != null && ActionInteractiveDataEditor.actionInteractiveTypeToNameList != null)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            List<EnumActionInteractiveType> actionInteractiveTypeValues = ActionInteractiveDataEditor.actionInteractiveTypeToNameList.Select(temp => temp.Key).ToList();
            string[] actionInteractiveTypeExplans = ActionInteractiveDataEditor.actionInteractiveTypeToNameList.Select(temp => temp.Value).ToArray();
            int actionInteractiveTypeIndex = actionInteractiveTypeValues.IndexOf(actionInteractiveDataInfo.ActionInteractiveType);
            EditorGUILayout.Popup(actionInteractiveTypeIndex, actionInteractiveTypeExplans, GUILayout.Width(120));
            EditorGUILayout.EndHorizontal();
            switch (actionInteractiveDataInfo.ActionInteractiveType)
            {
                case EnumActionInteractiveType.TreasureBox:
                    Dictionary<EnumGoodsType, ActionInteractiveDataInfo.TreasureBoxStruct> treasureBoxData = actionInteractiveDataInfo.OtherValue as Dictionary<EnumGoodsType,  ActionInteractiveDataInfo.TreasureBoxStruct>;
                    if (treasureBoxData == null)
                        break;
                    List<EnumGoodsType> goodsTypeValues = goodsTypeToExplanList.Select(temp => temp.Key).ToList();
                    string[] goodsTypeExplans = goodsTypeToExplanList.Select(temp => temp.Value).ToArray();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("商品类型", GUILayout.Width(45));
                    selectGoodsIndex = EditorGUILayout.Popup(selectGoodsIndex, goodsTypeExplans, GUILayout.Width(150));
                    if (selectGoodsIndex >= 0)
                    {
                        if (!treasureBoxData.ContainsKey(goodsTypeValues[selectGoodsIndex]) && GUILayout.Button("添加", GUILayout.Width(35)))
                        {
                            treasureBoxData.Add(goodsTypeValues[selectGoodsIndex], new ActionInteractiveDataInfo.TreasureBoxStruct());
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    List<EnumQualityType> qualityTypeValues = qualityTypeToExplanList.Select(temp => temp.Key).ToList();
                    string[] qualityTypeExplans = qualityTypeToExplanList.Select(temp => temp.Value).ToArray();
                    EnumGoodsType[] showGoodsTypeArray = treasureBoxData.Select(temp => temp.Key).ToArray();
                    foreach (EnumGoodsType showGoodsType in showGoodsTypeArray)
                    {
                        EditorGUILayout.BeginHorizontal();
                        int thisIndex = goodsTypeValues.IndexOf(showGoodsType);
                        GUILayout.Button(goodsTypeExplans[thisIndex], GUILayout.Width(50));
                        EditorGUILayout.LabelField("数量:", GUILayout.Width(35));
                        treasureBoxData[showGoodsType].Count = EditorGUILayout.IntField(treasureBoxData[showGoodsType].Count, GUILayout.Width(35));
                        EditorGUILayout.LabelField("最小品质:", GUILayout.Width(50));
                        int minIndex = qualityTypeValues.IndexOf(treasureBoxData[showGoodsType].MinQualityType);
                        minIndex = EditorGUILayout.Popup(minIndex, qualityTypeExplans,GUILayout.Width(65));
                        if (minIndex >= 0)
                            treasureBoxData[showGoodsType].MinQualityType = qualityTypeValues[minIndex];
                        EditorGUILayout.LabelField("最大品质:", GUILayout.Width(50));
                        int maxIndex = qualityTypeValues.IndexOf(treasureBoxData[showGoodsType].MaxQualityType);
                        maxIndex = EditorGUILayout.Popup(maxIndex, qualityTypeExplans, GUILayout.Width(65));
                        if (maxIndex >= 0)
                            treasureBoxData[showGoodsType].MaxQualityType = qualityTypeValues[maxIndex];
                        if (GUILayout.Button("×", GUILayout.Width(20)) && EditorUtility.DisplayDialog("请再次确认!", "是否从宝箱中移除该道具?", "确认", "取消"))
                        {
                            treasureBoxData.Remove(showGoodsType);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    break;
                case EnumActionInteractiveType.Other:
                    ActionInteractiveDataInfo.OtherDataStruct otherDataStruct = actionInteractiveDataInfo.OtherValue as ActionInteractiveDataInfo.OtherDataStruct;
                    if (otherDataStruct == null)
                        break;
                    string[] otherDataTypeExplans = otherDataTypes.Select(temp => temp.Name).ToArray();
                    int otherDataTypeIndex = otherDataTypes.IndexOf(otherDataStruct.ActionInterativeClass);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("处理类型:");
                    otherDataTypeIndex = EditorGUILayout.Popup(otherDataTypeIndex, otherDataTypeExplans);
                    if (otherDataTypeIndex >= 0)
                    {
                        otherDataStruct.ActionInterativeClass = otherDataTypes[otherDataTypeIndex];
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("数据:");
                    otherDataStruct.Data = EditorGUILayout.TextField(otherDataStruct.Data);
                    EditorGUILayout.EndHorizontal();
                    break;

            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
