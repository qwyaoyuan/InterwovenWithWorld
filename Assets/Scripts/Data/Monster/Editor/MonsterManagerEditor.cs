using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using ReflectEncapsulation;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Reflection;

/// <summary>
/// 怪物编辑器
/// </summary>
public class MonsterManagerEditor : EditorWindow
{
    /// <summary>
    /// 保存怪物信息的完整路径
    /// </summary>
    public string dataDirectoryPath = @"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\Monster";

    [MenuItem("小工具/怪物编辑器(类型场景与位置)")]
    static void AddWindown()
    {
        MonsterManagerEditor monsterManagerEditor = EditorWindow.GetWindow<MonsterManagerEditor>();
        monsterManagerEditor.Show();
    }

    /// <summary>
    /// 怪物信息集合数组 
    /// </summary>
    MonsterDataInfoCollection[] monsterDataInfoCollections;

    /// <summary>
    /// 怪物预设提字典
    /// </summary>
    private Dictionary<string, GameObject> monsterPrefabDic;

    /// <summary>
    /// 怪物类型对应说明的字典
    /// </summary>
    private Dictionary<EnumMonsterType, string> monsterTypeToFieldNameDic;

    /// <summary>
    /// 物品类型对应说明的字典
    /// </summary>
    private Dictionary<EnumGoodsType, string> goodsTypeToFieldNameDic;

    /// <summary>
    /// 怪物AI类型对应说明的字典
    /// </summary>
    private Dictionary<EnumMonsterAIType, string> monsterAITypeToFieldNameDic;

    /// <summary>
    /// 用于显示范围的游戏对象
    /// </summary>
    private GameObject rangeObj;

    private void Awake()
    {

        if (!Directory.Exists(dataDirectoryPath))
            Directory.CreateDirectory(dataDirectoryPath);
        if (!File.Exists(dataDirectoryPath + "/Monster.txt"))
        {
            File.Create(dataDirectoryPath + "/Monster.txt").Close();
            monsterDataInfoCollections = new MonsterDataInfoCollection[0];
            string valueText = SerializeNow(monsterDataInfoCollections);
            File.WriteAllText(dataDirectoryPath + "/Monster.txt", valueText, Encoding.UTF8);
        }
        else
        {
            string valueText = File.ReadAllText(dataDirectoryPath + "/Monster.txt", Encoding.UTF8);
            monsterDataInfoCollections = DeSerializeNow<MonsterDataInfoCollection[]>(valueText);
            if (monsterDataInfoCollections == null)
                monsterDataInfoCollections = new MonsterDataInfoCollection[0];
        }
        //加载预设提
        monsterPrefabDic = new Dictionary<string, GameObject>();
        GameObject[] allPrefabs = Resources.LoadAll<GameObject>(MonsterDataInfo.monsterPrefabDirectoryPath);
        foreach (GameObject prefab in allPrefabs)
        {
            monsterPrefabDic.Add(prefab.name, prefab);
        }
        //怪物类型名字典
        monsterTypeToFieldNameDic = new Dictionary<EnumMonsterType, string>();
        Type monsterTypeType = typeof(EnumMonsterType);
        EnumMonsterType[] monsterTypeArray = Enum.GetValues(typeof(EnumMonsterType)).OfType<EnumMonsterType>().ToArray();
        foreach (EnumMonsterType monsterType in monsterTypeArray)
        {
            FieldInfo fieldInfo = monsterTypeType.GetField(monsterType.ToString());
            if (fieldInfo == null)
                continue;
            FieldExplanAttribute fieldExplanAttribute = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
            if (fieldExplanAttribute == null)
                continue;
            monsterTypeToFieldNameDic.Add(monsterType, fieldExplanAttribute.GetExplan());
        }
        //物品类型名字典 
        goodsTypeToFieldNameDic = new Dictionary<EnumGoodsType, string>();
        Type goodsTypeType = typeof(EnumGoodsType);
        EnumGoodsType[] goodsTypeArray = Enum.GetValues(typeof(EnumGoodsType)).OfType<EnumGoodsType>().ToArray();
        foreach (EnumGoodsType goodsType in goodsTypeArray)
        {
            int goodsTypeID = (int)goodsType;
            int temp1 = goodsTypeID % 1000;
            if (temp1 == 0)
                continue;
            FieldInfo fieldInfo = goodsTypeType.GetField(goodsType.ToString());
            if (fieldInfo == null)
                continue;
            FieldExplanAttribute fieldExplanAttribute = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
            if (fieldExplanAttribute == null)
                continue;
            goodsTypeToFieldNameDic.Add(goodsType, fieldExplanAttribute.GetExplan());
        }
        //怪物ai类型名字典
        monsterAITypeToFieldNameDic = new Dictionary<EnumMonsterAIType, string>();
        Type monsterAITypeType = typeof(EnumMonsterAIType);
        EnumMonsterAIType[] monsterAITypeArray = Enum.GetValues(typeof(EnumMonsterAIType)).OfType<EnumMonsterAIType>().ToArray();
        foreach (EnumMonsterAIType monsterAIType in monsterAITypeArray)
        {
            FieldInfo fieldInfo = monsterAITypeType.GetField(monsterAIType.ToString());
            if (fieldInfo == null)
                continue;
            FieldExplanAttribute fieldExplanAttribute = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
            if (fieldExplanAttribute == null)
                continue;
            monsterAITypeToFieldNameDic.Add(monsterAIType, fieldExplanAttribute.GetExplan());
        }

        //表示范围的游戏对象
        rangeObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Shader shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
        Material material = new Material(shader);
        material.SetColor("_Color", new Color(1, 0, 0, 0.35f));
        rangeObj.GetComponent<MeshRenderer>().material = material;
        rangeObj.name = "中心以及区域";
    }

    private void OnDestroy()
    {
        if (rangeObj != null)
            try
            {
                GameObject.DestroyImmediate(rangeObj);
            }
            catch { }
    }

    private void Update()
    {
        string thisSceneName = SceneManager.GetActiveScene().name;
        if (!string.Equals(thisSceneName, selectScene))
        {
            selectMonsterID = -1;
        }
        selectScene = thisSceneName;
    }

    void OnInspectorUpdate()
    {
        //Debug.Log("窗口面板的更新");
        //这里开启窗口的重绘，不然窗口信息不会刷新
        this.Repaint();
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

    /// <summary>
    /// 保存 场景 等按钮页面的滑动条位置
    /// </summary>
    Vector2 saveSceneScrollPostion;
    /// <summary>
    /// 选择id页面的滑动条位置 
    /// </summary>
    Vector2 monsterIDScrollPostion;
    /// <summary>
    /// 基础数据页面的滑动条位置 
    /// </summary>
    Vector2 baseDataScrollPostion;
    /// <summary>
    /// 怪物ai页面的滑动条位置
    /// </summary>
    Vector2 aiDataScrollPostion;

    /// <summary>
    /// 当前的场景
    /// </summary>
    string selectScene;
    /// <summary>
    /// 选中的怪物id
    /// </summary>
    int selectMonsterID;

    void OnGUI()
    {
        if (monsterDataInfoCollections == null || monsterTypeToFieldNameDic == null || monsterAITypeToFieldNameDic == null)
            return;
        //获取当前场景对象
        MonsterDataInfoCollection monsterDataInfoCollection = monsterDataInfoCollections.FirstOrDefault(temp => string.Equals(temp.sceneName, selectScene));

        EditorGUILayout.BeginHorizontal();
        #region 保存 场景 等按钮
        EditorGUILayout.BeginVertical(GUILayout.Width(200));
        if (GUILayout.Button("保存", GUILayout.Width(100)))
        {
            string valueText = SerializeNow(monsterDataInfoCollections);
            File.WriteAllText(dataDirectoryPath + "/Monster.txt", valueText, Encoding.UTF8);
            EditorUtility.DisplayDialog("提示!", "保存成功", "确认");
        }
        if (monsterDataInfoCollection == null && GUILayout.Button("添加本场景", GUILayout.Width(100)))
        {
            monsterDataInfoCollection = new MonsterDataInfoCollection() { sceneName = selectScene };
            MonsterDataInfoCollection[] tempArray = new MonsterDataInfoCollection[monsterDataInfoCollections.Length + 1];
            Array.Copy(monsterDataInfoCollections, tempArray, monsterDataInfoCollections.Length);
            tempArray[tempArray.Length - 1] = monsterDataInfoCollection;
            monsterDataInfoCollections = tempArray;
        }
        saveSceneScrollPostion = EditorGUILayout.BeginScrollView(saveSceneScrollPostion);
        #region  显示所有的场景按钮
        bool changedScene = false;
        List<MonsterDataInfoCollection> monsterDataInfoCollectionList = new List<MonsterDataInfoCollection>(monsterDataInfoCollections);
        for (int i = 0; i < monsterDataInfoCollections.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("删除", GUILayout.Width(35)))
            {
                if (EditorUtility.DisplayDialog("警告!", "请再次确认是否删除该场景的怪物配置信息?", "确认删除", "取消删除"))
                {
                    monsterDataInfoCollectionList.Remove(monsterDataInfoCollections[i]);
                    changedScene = true;
                }
            }
            if (monsterDataInfoCollections[i] == monsterDataInfoCollection)
                EditorGUILayout.Toggle(true, GUILayout.Width(20));
            GUILayout.Button(monsterDataInfoCollections[i].sceneName);
            EditorGUILayout.EndHorizontal();
        }
        if (changedScene)
        {
            monsterDataInfoCollections = monsterDataInfoCollectionList.ToArray();
            if (!monsterDataInfoCollectionList.Contains(monsterDataInfoCollection))
            {
                monsterDataInfoCollection = null;
                selectMonsterID = -1;
            }
        }
        #endregion
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        #endregion
        #region 选择删除添加怪物id
        EditorGUILayout.BeginVertical(GUILayout.Width(175));
        if (monsterDataInfoCollection != null)
        {
            if (GUILayout.Button("添加新策略", GUILayout.Width(75)))
            {

                int id = 0;
                if (monsterDataInfoCollection.MonsterDataInofs.Count > 0)
                    id = monsterDataInfoCollection.MonsterDataInofs.Max(temp => temp.ID) + 1;
                monsterDataInfoCollection.MonsterDataInofs.Add(new MonsterDataInfo() { ID = id, Briefly = "空" });
            }
            monsterIDScrollPostion = EditorGUILayout.BeginScrollView(monsterIDScrollPostion, GUILayout.Width(170));
            List<MonsterDataInfo> tempMonsterDataInfos = new List<MonsterDataInfo>(monsterDataInfoCollection.MonsterDataInofs);
            foreach (MonsterDataInfo monsterDataInfo in tempMonsterDataInfos)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("删除", GUILayout.Width(35)))
                {
                    if (EditorUtility.DisplayDialog("警告!", "请再次确认是否删除怪物配置信息?", "确认删除", "取消删除"))
                    {
                        monsterDataInfoCollection.MonsterDataInofs.Remove(monsterDataInfo);
                    }
                }
                if (monsterDataInfo.ID == selectMonsterID)
                    EditorGUILayout.Toggle(true, GUILayout.Width(20));
                if (GUILayout.Button(monsterDataInfo.Briefly))
                {
                    selectMonsterID = monsterDataInfo.ID;
                    //在这里重新赋值区域位置
                    if (rangeObj != null)
                    {
                        rangeObj.transform.position = monsterDataInfo.Center;
                        rangeObj.transform.localScale = new Vector3(monsterDataInfo.Range * 2, monsterDataInfo.Range * 2, monsterDataInfo.Range * 2);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
        #endregion
        //集合不等于0 选择的id大于等于0 并且存在该id 
        if (monsterDataInfoCollection != null && selectMonsterID >= 0 && monsterDataInfoCollection.MonsterDataInofs.Count(temp => temp.ID == selectMonsterID) > 0)
        {
            MonsterDataInfo monsterDataInfo = monsterDataInfoCollection.MonsterDataInofs.FirstOrDefault(temp => temp.ID == selectMonsterID);
            #region 怪物的基本信息
            EditorGUILayout.BeginVertical(GUILayout.Width(350));
            baseDataScrollPostion = EditorGUILayout.BeginScrollView(baseDataScrollPostion);
            monsterDataInfo.Briefly = EditorGUILayout.TextField("配置:", monsterDataInfo.Briefly);
            monsterDataInfo.Explane = EditorGUILayout.TextField("说明:", monsterDataInfo.Explane, GUILayout.Width(300), GUILayout.Height(150));
            string[] monsterTypeExplans = monsterTypeToFieldNameDic.Values.ToArray();
            int[] monsterTypeValues = monsterTypeToFieldNameDic.Keys.Select(temp => (int)temp).ToArray();
            int monsterTypeValue = EditorGUILayout.IntPopup("怪物类型:", (int)monsterDataInfo.MonsterType, monsterTypeExplans, monsterTypeValues);
            monsterDataInfo.MonsterType = (EnumMonsterType)monsterTypeValue;
            if (rangeObj != null)
            {
                rangeObj.transform.forward = Vector3.forward;
                monsterDataInfo.Center = rangeObj.transform.position;
                Vector3 scale = rangeObj.transform.localScale;
                float minRange = scale.x;
                minRange = scale.y < minRange ? scale.y : minRange;
                minRange = scale.z < minRange ? scale.z : minRange;
                monsterDataInfo.Range = minRange / 2;
            }
            monsterDataInfo.Center = EditorGUILayout.Vector3Field("区域中心:", monsterDataInfo.Center);
            monsterDataInfo.Range = EditorGUILayout.FloatField("区域范围:", monsterDataInfo.Range);
            if (rangeObj != null)
            {
                rangeObj.transform.position = monsterDataInfo.Center;
                rangeObj.transform.localScale = new Vector3(monsterDataInfo.Range * 2, monsterDataInfo.Range * 2, monsterDataInfo.Range * 2);
            }
            monsterDataInfo.Offset = EditorGUILayout.FloatField("高度偏差值:", monsterDataInfo.Offset);
            monsterDataInfo.Experience = EditorGUILayout.IntField("经验值:", monsterDataInfo.Experience);
            GameObject monsterPrefab = EditorGUILayout.ObjectField("怪物预设体:", monsterDataInfo.MonsterPrefab, typeof(GameObject), false) as GameObject;
            if (monsterPrefab == null)
                monsterDataInfo.monsterPrefabName = "";
            else if (!string.Equals(monsterPrefab.name, monsterDataInfo.monsterPrefabName))
            {
                string monsterPrefabName = monsterPrefab.name;
                if (monsterPrefabDic.ContainsKey(monsterPrefabName))
                    monsterDataInfo.monsterPrefabName = monsterPrefabName;
            }
            if (monsterDataInfo.ItemDropRates == null)
                monsterDataInfo.ItemDropRates = new float[0];
            if (monsterDataInfo.ItemDropTypes == null)
                monsterDataInfo.ItemDropTypes = new EnumGoodsType[0];
            int dropCount = monsterDataInfo.ItemDropRates.Length > monsterDataInfo.ItemDropTypes.Length ? monsterDataInfo.ItemDropTypes.Length : monsterDataInfo.ItemDropRates.Length;
            int nowDropCount = EditorGUILayout.IntField("掉落物品种类数量(类型->概率):", dropCount);
            if (dropCount != nowDropCount)
            {
                monsterDataInfo.ItemDropRates = ChangedArrayLength(monsterDataInfo.ItemDropRates, nowDropCount);
                monsterDataInfo.ItemDropTypes = ChangedArrayLength(monsterDataInfo.ItemDropTypes, nowDropCount);
            }
            string[] goodsTypeExplans = goodsTypeToFieldNameDic.Values.ToArray();
            int[] goodsTypeValues = goodsTypeToFieldNameDic.Keys.Select(temp => (int)temp).ToArray();
            for (int i = 0; i < nowDropCount; i++)
            {
                EditorGUILayout.BeginHorizontal();
                int goodsTypeValue = EditorGUILayout.IntPopup("物品类型", (int)monsterDataInfo.ItemDropTypes[i], goodsTypeExplans, goodsTypeValues);
                monsterDataInfo.ItemDropTypes[i] = (EnumGoodsType)goodsTypeValue;
                float rate = EditorGUILayout.FloatField(monsterDataInfo.ItemDropRates[i]);
                rate = Mathf.Clamp(rate, 0, 1);
                monsterDataInfo.ItemDropRates[i] = rate;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            #endregion
            #region 怪物的AI信息
            EditorGUILayout.BeginVertical(GUILayout.Width(350));
            string[] monsterAITypeExplans = monsterAITypeToFieldNameDic.Values.ToArray();
            int[] monsterAITypeValues = monsterAITypeToFieldNameDic.Keys.Select(temp => (int)temp).ToArray();
            int monsterAITypeValue = EditorGUILayout.IntPopup("怪物AI类型:", (int)monsterDataInfo.AIType, monsterAITypeExplans, monsterAITypeValues);
            if ((int)monsterDataInfo.AIType != monsterAITypeValue || monsterDataInfo.AIData == null)
            {
                monsterDataInfo.AIType = (EnumMonsterAIType)monsterAITypeValue;
                MonsterAIDataStruct monsterAIDataStruct = monsterDataInfo.AIData;
                try
                {
                    monsterDataInfo.AIData = (MonsterAIDataStruct)Activator.CreateInstance(typeof(MonsterAIDataStruct).Assembly.GetType("MonsterAIData_" + monsterDataInfo.AIType.ToString()));
                }
                catch { }
                if (monsterAIDataStruct != null && monsterDataInfo.AIData != null)
                {
                    monsterDataInfo.AIData.FollowDistance = monsterAIDataStruct.FollowDistance;
                    monsterDataInfo.AIData.UpdateTime = monsterAIDataStruct.UpdateTime;
                }
            }
            aiDataScrollPostion = EditorGUILayout.BeginScrollView(aiDataScrollPostion);
            if (monsterDataInfo.AIData != null)
            {
                monsterDataInfo.AIData.UpdateTime = EditorGUILayout.FloatField("怪物刷新时间:", monsterDataInfo.AIData.UpdateTime);
                monsterDataInfo.AIData.FollowDistance = EditorGUILayout.FloatField("跟随的最远距离:", monsterDataInfo.AIData.FollowDistance);
                //反射出不同类型的数据
                Type thisAIDataType = typeof(MonsterAIDataStruct).Assembly.GetType("MonsterAIData_" + monsterDataInfo.AIType.ToString());
                FieldInfo[] fieldInfos = thisAIDataType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    FieldExplanAttribute fieldExplan = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
                    if (fieldExplan != null)
                    {
                        if (fieldInfo.FieldType.Equals(typeof(float)))
                        {
                            float value = EditorGUILayout.FloatField(fieldExplan.GetExplan(), (float)fieldInfo.GetValue(monsterDataInfo.AIData));
                            fieldInfo.SetValue(monsterDataInfo.AIData, value);
                        }
                        else if (fieldInfo.FieldType.Equals(typeof(int)))
                        {
                            int value = EditorGUILayout.IntField(fieldExplan.GetExplan(), (int)fieldInfo.GetValue(monsterDataInfo.AIData));
                            fieldInfo.SetValue(monsterDataInfo.AIData, value);
                        }
                        else if (fieldInfo.FieldType.Equals(typeof(bool)))
                        {
                            bool value = EditorGUILayout.Toggle(fieldExplan.GetExplan(), (bool)fieldInfo.GetValue(monsterDataInfo.AIData));
                            fieldInfo.SetValue(monsterDataInfo.AIData, value);
                        }
                        //还有其他的在后面添加.....
                    }
                }

            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            #endregion
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 修改数组长度
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    private T[] ChangedArrayLength<T>(T[] source, int length)
    {
        T[] result = new T[length];
        if (source == null)
            return result;
        if (source.Length > length)
            Array.Copy(source, result, length);
        else
            Array.Copy(source, result, source.Length);
        return result;
    }
}
