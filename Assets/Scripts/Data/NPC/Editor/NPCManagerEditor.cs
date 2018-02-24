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

/// <summary>
/// NPC编辑器
/// </summary>
public class NPCManagerEditor : EditorWindow
{
    /// <summary>
    /// 保存NPC信息的完整路径
    /// </summary>
    public string dataAllPath = @"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\NPC";

    /// <summary>
    /// 数据字典
    /// </summary>
    private Dictionary<string, TextAsset> dataDic;

    /// <summary>
    /// NPC预设提字典
    /// </summary>
    private Dictionary<string, GameObject> npcDataDic;

    /// <summary>
    /// 当前的npc信息集合
    /// </summary>
    NPCDataInfoCollection nowNPCDataInfoCollection;

    /// <summary>
    /// 当前场景
    /// </summary>
    private string nowSceneName;

    /// <summary>
    /// 滑动条位置
    /// </summary>
    Vector2 scrollPostion;

    /// <summary>
    /// 是否正在弹出其他的窗口
    /// </summary>
    bool showNextWindow;

    [MenuItem("小工具/NPC编辑器")]
    static void AddWindow()
    {
        NPCManagerEditor npcManagerEditor = EditorWindow.GetWindow<NPCManagerEditor>();
        npcManagerEditor.Show();
    }

    private void Awake()
    {
        dataDic = new Dictionary<string, TextAsset>();
        npcDataDic = new Dictionary<string, GameObject>();

        TextAsset[] allTextAssets = Resources.LoadAll<TextAsset>(NPCData.dataDirectoryPath);
        foreach (TextAsset textAsset in allTextAssets)
        {
            dataDic.Add(textAsset.name, textAsset);
        }

        GameObject[] allPrefabs = Resources.LoadAll<GameObject>(NPCDataInfo.npcPrefabDirectoryPath);
        foreach (GameObject prefab in allPrefabs)
        {
            npcDataDic.Add(prefab.name, prefab);
        }

    }

    private void OnDestroy()
    {
        if (nowNPCDataInfoCollection != null)
        {
            if (nowNPCDataInfoCollection.NPCDataInfos != null)
            {
                foreach (NPCDataInfo npcDataInfo in nowNPCDataInfoCollection.NPCDataInfos)
                {
                    if (npcDataInfo != null)
                    {
                        GameObject.DestroyImmediate(npcDataInfo.NPCObj);
                    }
                }
            }
        }
        nowNPCDataInfoCollection = null;
    }

    void OnInspectorUpdate()
    {
        //Debug.Log("窗口面板的更新");
        //这里开启窗口的重绘，不然窗口信息不会刷新
        this.Repaint();
    }

    private void Update()
    {
        Scene nowScene = SceneManager.GetActiveScene();
        if (!string.Equals(nowScene.name, this.nowSceneName))
        {
            this.nowSceneName = nowScene.name;
            //清理之前创建的npc对象并重新加载新的
            if (nowNPCDataInfoCollection != null)
            {
                foreach (NPCDataInfo npcDataInfo in nowNPCDataInfoCollection.NPCDataInfos)
                {
                    if (npcDataInfo != null)
                    {
                        GameObject.DestroyImmediate(npcDataInfo.NPCObj);
                    }
                }
            }
            nowNPCDataInfoCollection = null;
            LoadNowAssetObj();
        }
    }

    /// <summary>
    /// 加载当前场景的NPC对象
    /// </summary>
    private void LoadNowAssetObj()
    {
        if (dataDic.ContainsKey(this.nowSceneName))
        {
            TextAsset nowSceneTextAsset = dataDic[this.nowSceneName];
            string assetText = Encoding.UTF8.GetString(nowSceneTextAsset.bytes);
            NPCDataInfoCollection npcDataInfoCollection = DeSerializeNow<NPCDataInfoCollection>(assetText);
            if (npcDataInfoCollection != null)
            {
                nowNPCDataInfoCollection = npcDataInfoCollection;
                foreach (NPCDataInfo npcDataInfo in nowNPCDataInfoCollection.NPCDataInfos)
                {
                    if (string.IsNullOrEmpty(npcDataInfo.npcPrefabName))
                        continue;
                    if (!npcDataDic.ContainsKey(npcDataInfo.npcPrefabName))
                        continue;
                    npcDataInfo.Load();
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

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("路径信息请在NPCManagerEditor内自行设置");
        EditorGUILayout.LabelField("当前场景:" + nowSceneName);
        if (nowNPCDataInfoCollection == null)//如果当前场景不存在数据
        {
            if (GUILayout.Button("创建该场景的NPC信息数据"))
            {
                nowNPCDataInfoCollection = new NPCDataInfoCollection();
                nowNPCDataInfoCollection.sceneName = nowSceneName;
                if (File.Exists(dataAllPath + "\\" + nowSceneName + ".txt"))
                {
                    if (EditorUtility.DisplayDialog("警告!", "存在相同的文件" + nowSceneName + ".txt\r\n是否覆盖?", "确认覆盖", "取消覆盖"))
                    {
                        string valueText = SerializeNow<NPCDataInfoCollection>(nowNPCDataInfoCollection);
                        File.WriteAllText(dataAllPath + "\\" + nowSceneName + ".txt", valueText, Encoding.UTF8);
                    }
                    else
                    {
                        nowNPCDataInfoCollection = null;
                    }
                }
                else
                {
                    File.Create(dataAllPath + "\\" + nowSceneName + ".txt").Close();
                    string valueText = SerializeNow<NPCDataInfoCollection>(nowNPCDataInfoCollection);
                    File.WriteAllText(dataAllPath + "\\" + nowSceneName + ".txt", valueText, Encoding.UTF8);
                }
            }
        }
        else//如果当前场景存在数据
        {
            if (showNextWindow)
                EditorGUILayout.LabelField("请先关闭(处理完成)弹出窗体后再继续");
            if (GUILayout.Button("保存数据") && !showNextWindow)
            {
                if (nowNPCDataInfoCollection != null && File.Exists(dataAllPath + "\\" + nowNPCDataInfoCollection.sceneName + ".txt"))
                {
                    foreach (NPCDataInfo tempNPCDataInfo in nowNPCDataInfoCollection.NPCDataInfos)
                    {
                        if (tempNPCDataInfo.NPCObj != null)
                        {
                            tempNPCDataInfo.NPCLocation = tempNPCDataInfo.NPCObj.transform.position;
                            tempNPCDataInfo.NPCAngle = tempNPCDataInfo.NPCObj.transform.eulerAngles;
                        }
                    }
                    string valueText = SerializeNow<NPCDataInfoCollection>(nowNPCDataInfoCollection);
                    File.WriteAllText(dataAllPath + "\\" + nowNPCDataInfoCollection.sceneName + ".txt", valueText, Encoding.UTF8);
                    EditorUtility.DisplayDialog("保存数据", "保存成功!", "确认");
                }
            }
            scrollPostion = EditorGUILayout.BeginScrollView(scrollPostion);
            NPCDataInfo[] npcDataInfos = nowNPCDataInfoCollection.NPCDataInfos.ToArray();
            foreach (NPCDataInfo npcDataInfo in npcDataInfos)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("ID:", GUILayout.Width(20));
                npcDataInfo.NPCID = EditorGUILayout.IntField(npcDataInfo.NPCID, GUILayout.Width(30));
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Name:", GUILayout.Width(30));
                npcDataInfo.NPCName = EditorGUILayout.TextField(npcDataInfo.NPCName);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Target:", GUILayout.Width(30));
                EditorGUILayout.ObjectField(npcDataInfo.NPCObj, typeof(GameObject), true);
                EditorGUILayout.Space();
                if (GUILayout.Button("编辑") && !showNextWindow)
                {
                    EditorNPCDataInfoWindow changeNPCDataInfoWindow = EditorWindow.GetWindow<EditorNPCDataInfoWindow>(true, "修改NPC信息", true);
                    showNextWindow = true;
                    changeNPCDataInfoWindow.ResultHandle += (_npcDataInfo) => { showNextWindow = false; };
                    changeNPCDataInfoWindow.npcDataDic = npcDataDic;
                    changeNPCDataInfoWindow.nowIDList = nowNPCDataInfoCollection.NPCDataInfos.Select(temp => temp.NPCID).Where(temp => temp != npcDataInfo.NPCID).ToList();
                    changeNPCDataInfoWindow.SetNPCDataInfo(npcDataInfo);
                    changeNPCDataInfoWindow.Show();
                }
                if (GUILayout.Button("删除") && !showNextWindow)
                {
                    if (EditorUtility.DisplayDialog("警告!", "请再次确认是否删除?", "确认删除", "取消删除"))
                    {
                        if (npcDataInfo != null)
                        {
                            if (npcDataInfo.NPCObj != null)
                                GameObject.DestroyImmediate(npcDataInfo.NPCObj);
                            nowNPCDataInfoCollection.NPCDataInfos.Remove(npcDataInfo);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("添加") && !showNextWindow)
            {
                EditorNPCDataInfoWindow newNPCDataInfoWindow = EditorWindow.GetWindow<EditorNPCDataInfoWindow>(true, "添加NPC信息", true);
                showNextWindow = true;
                NPCDataInfoCollection tempNowNpcDataInfoCollection = nowNPCDataInfoCollection;
                newNPCDataInfoWindow.ResultHandle += (npcDataInfo) =>
                {
                    showNextWindow = false;
                    if (npcDataInfo != null && object.Equals(tempNowNpcDataInfoCollection, nowNPCDataInfoCollection))
                    {
                        tempNowNpcDataInfoCollection.NPCDataInfos.Add(npcDataInfo);
                    }
                };
                newNPCDataInfoWindow.npcDataDic = npcDataDic;
                newNPCDataInfoWindow.nowIDList = nowNPCDataInfoCollection.NPCDataInfos.Select(temp => temp.NPCID).ToList();
                newNPCDataInfoWindow.Show();
            }
            EditorGUILayout.EndScrollView();
        }

        EditorGUILayout.EndVertical();
    }
}

/// <summary>
/// 添加新的NPC信息所用的窗体
/// </summary>
public class EditorNPCDataInfoWindow : EditorWindow
{
    /// <summary>
    /// 结果
    /// </summary>
    public event Action<NPCDataInfo> ResultHandle;

    /// <summary>
    /// npc预设字典
    /// </summary>
    public Dictionary<string, GameObject> npcDataDic;

    /// <summary>
    /// 当前的id集合
    /// </summary>
    public List<int> nowIDList;

    /// <summary>
    /// 要创建的NPC数据
    /// </summary>
    NPCDataInfo tempNPCDataInfo;

    /// <summary>
    /// 是否创建了
    /// </summary>
    bool isCreate;

    /// <summary>
    /// 存储所有静态道具的类
    /// </summary>
    GoodsMetaInfoMations goodsMetaInfoMations;
    /// <summary>
    /// 如果是商人则存在的数据是该对象
    /// </summary>
    BusinessmanDataInfo businessmanDataInfo;
    /// <summary>
    /// 商人编辑列表的滑动条
    /// </summary>
    Vector2 businessmanScroll;
    /// <summary>
    /// 物品对应说明字典
    /// </summary>
    List<KeyValuePair<EnumGoodsType, string>> goodsTypeToExplanList;
    /// <summary>
    /// 物品品质对应说明字典
    /// </summary>
    List<KeyValuePair<EnumQualityType, string>> goodsQualityTypeToExplanList;
    /// <summary>
    /// 任务状态对应说明字典
    /// </summary>
    List<KeyValuePair<TaskMap.Enums.EnumTaskProgress, string>> taskStateTypeToExplanList;
    /// <summary>
    /// NPC类型对应说明字典
    /// </summary>
    List<KeyValuePair<EnumNPCType, string>> npcTypeToExplanList;
    /// <summary>
    /// 物品类型选择添加下标
    /// </summary>
    int goodsTypeIndex;

    /// <summary>
    /// 滑动条
    /// </summary>
    Vector2 scroll;

    public void SetNPCDataInfo(NPCDataInfo npcDataInfo)
    {
        tempNPCDataInfo = npcDataInfo;
        if (tempNPCDataInfo != null && tempNPCDataInfo.NPCObj != null)
            isCreate = true;
    }

    private void OnInspectorUpdate()
    {
        this.Repaint();
    }

    private void Update()
    {
        if (isCreate && tempNPCDataInfo != null && tempNPCDataInfo.NPCObj != null)
        {
            tempNPCDataInfo.NPCLocation = tempNPCDataInfo.NPCObj.transform.position;
            tempNPCDataInfo.NPCAngle = tempNPCDataInfo.NPCObj.transform.eulerAngles;
            tempNPCDataInfo.NPCObj.name = tempNPCDataInfo.NPCName;
        }
    }

    private void OnGUI()
    {
        if (npcDataDic == null)
            return;
        if (tempNPCDataInfo == null)
        {
            tempNPCDataInfo = new NPCDataInfo();
            if (nowIDList.Count > 0)
                tempNPCDataInfo.NPCID = nowIDList.Max() + 1;
            else
                tempNPCDataInfo.NPCID = 0;
            tempNPCDataInfo.NPCName = "Name";
            tempNPCDataInfo.npcPrefabName = "";
            tempNPCDataInfo.NPCType = EnumNPCType.Normal;

        }
        EditorGUILayout.BeginVertical();

        scroll = EditorGUILayout.BeginScrollView(scroll);

        List<string> names = npcDataDic.Keys.OfType<string>().ToList();
        int index = names.IndexOf(tempNPCDataInfo.npcPrefabName);
        if (tempNPCDataInfo.NPCObj)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(tempNPCDataInfo.npcPrefabName);
            if (GUILayout.Button("×", GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog("警告!", "是否重新选择预设提?", "是", "否"))
                {
                    tempNPCDataInfo.npcPrefabName = "";
                    tempNPCDataInfo.InitNPCObjPrefab();
                    GameObject.DestroyImmediate(tempNPCDataInfo.NPCObj);
                    tempNPCDataInfo.NPCObj = null;
                    index = -1;
                    isCreate = false;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            index = EditorGUILayout.Popup(index, names.ToArray());
            if (index >= 0)
            {
                tempNPCDataInfo.npcPrefabName = names[index];
            }
        }

        if (index >= 0)
            EditorGUILayout.ObjectField(npcDataDic[tempNPCDataInfo.npcPrefabName], typeof(GameObject), true);
        int id = EditorGUILayout.IntField("NPC ID:", tempNPCDataInfo.NPCID);
        if (!nowIDList.Contains(id))
            tempNPCDataInfo.NPCID = id;
        tempNPCDataInfo.NPCName = EditorGUILayout.TextField("NPC Name:", tempNPCDataInfo.NPCName);
        if (npcTypeToExplanList == null)
        {
            npcTypeToExplanList = new List<KeyValuePair<EnumNPCType, string>>();
            FieldExplanAttribute.SetEnumExplanDic(npcTypeToExplanList);
        }
        List<EnumNPCType> npcTypeValues = npcTypeToExplanList.Select(temp => temp.Key).ToList();
        string[] npcTypeExplans = npcTypeToExplanList.Select(temp => temp.Value).ToArray();
        int npcTypeIndex = npcTypeValues.IndexOf(tempNPCDataInfo.NPCType);
        npcTypeIndex = EditorGUILayout.Popup("NPC Type:", npcTypeIndex, npcTypeExplans);
        if (npcTypeIndex >= 0)
        {
            tempNPCDataInfo.NPCType = npcTypeValues[npcTypeIndex];
        }
        tempNPCDataInfo.OtherValue = EditorGUILayout.TextField("Other Data:", tempNPCDataInfo.OtherValue);
        if (isCreate)
        {
            EditorGUILayout.ObjectField("NPC Object:", tempNPCDataInfo.NPCObj, typeof(GameObject), true);
        }
        if (((!isCreate && index >= 0) || (tempNPCDataInfo.NPCObj == null && index >= 0))
            && GUILayout.Button("Create NPC GameObject"))
        {
            GameObject createObj = GameObject.Instantiate<GameObject>(npcDataDic[tempNPCDataInfo.npcPrefabName]);
            tempNPCDataInfo.NPCObj = createObj;
            isCreate = true;
        }
        if (tempNPCDataInfo.NPCObj != null)
        {
            Selection.activeGameObject = tempNPCDataInfo.NPCObj;
            if (tempNPCDataInfo.NPCObj.GetComponent<TalkShowPosition>() == null)
                tempNPCDataInfo.NPCObj.AddComponent<TalkShowPosition>();
            tempNPCDataInfo.NPCObj.GetComponent<TalkShowPosition>().tempNPCDataInfo = tempNPCDataInfo;
            tempNPCDataInfo.TalkShowOffset = EditorGUILayout.Vector3Field("Talk Show Offset:", tempNPCDataInfo.TalkShowOffset);
            Vector3 talkShowWorldVec = tempNPCDataInfo.TalkShowOffset + tempNPCDataInfo.NPCObj.transform.position;
        }
        Sprite tempSprite = (Sprite)EditorGUILayout.ObjectField("NPC Sprite:", tempNPCDataInfo.NPCSprite, typeof(Sprite), false);
        if (tempSprite != tempNPCDataInfo.NPCSprite && tempSprite != null)
        {
            tempNPCDataInfo.npcSpriteID = SpriteManager.GetName(tempSprite);
            tempNPCDataInfo.NPCSprite = tempSprite;
        }
        EditorGUILayout.LabelField("------------------显示条件------------------");
        if (tempNPCDataInfo.NPCShowCondition == null && GUILayout.Button("创建显示条件"))
        {
            tempNPCDataInfo.NPCShowCondition = new NPCShowCondition();
        }
        if (tempNPCDataInfo.NPCShowCondition != null)
        {
            if (GUILayout.Button("删除显示条件"))
            {
                if (EditorUtility.DisplayDialog("请再次确认!", "是否要删除显示条件?", "删除", "取消"))
                {
                    tempNPCDataInfo.NPCShowCondition = null;
                }
            }
        }
        if (tempNPCDataInfo.NPCShowCondition != null)
        {
            if (taskStateTypeToExplanList == null)
            {
                taskStateTypeToExplanList = new List<KeyValuePair<TaskMap.Enums.EnumTaskProgress, string>>();
                FieldExplanAttribute.SetEnumExplanDic(taskStateTypeToExplanList);
            }
            List<TaskMap.Enums.EnumTaskProgress> taskProgressValueList = taskStateTypeToExplanList.Select(temp => temp.Key).ToList();
            string[] taskProgressExplanArray = taskStateTypeToExplanList.Select(temp => temp.Value).ToArray();
            //显示与任务条件相关的函数
            Func<NPCShowCondition.TaskCondition[], NPCShowCondition.TaskCondition[]> ShowAbourtTaskConditionFunc = (source) =>
            {
                if (source == null)
                    source = new NPCShowCondition.TaskCondition[0];
                if (GUILayout.Button("添加", GUILayout.Width(50)))
                {
                    NPCShowCondition.TaskCondition[] tempSource = new NPCShowCondition.TaskCondition[source.Length + 1];
                    Array.Copy(source, tempSource, source.Length);
                    tempSource[source.Length] = new NPCShowCondition.TaskCondition();
                    source = tempSource;
                }
                List<NPCShowCondition.TaskCondition> removeList = new List<NPCShowCondition.TaskCondition>();//需要移除的列表
                foreach (NPCShowCondition.TaskCondition taskCondition in source)
                {
                    if (taskCondition == null)
                        continue;
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("×", GUILayout.Width(20)))//删除
                    {
                        if (EditorUtility.DisplayDialog("请再次确认!", "是否删除该条数据?", "删除", "取消"))
                        {
                            removeList.Add(taskCondition);
                        }
                    }
                    EditorGUILayout.LabelField("任务ID:", GUILayout.Width(50));
                    taskCondition.TaskID = EditorGUILayout.IntField(taskCondition.TaskID, GUILayout.Width(20));

                    EditorGUILayout.LabelField("任务状态:", GUILayout.Width(60));
                    int taskStateIndex = taskProgressValueList.IndexOf(taskCondition.TaskState);
                    taskStateIndex = EditorGUILayout.Popup(taskStateIndex, taskProgressExplanArray, GUILayout.Width(100));
                    if (taskStateIndex > -1)
                    {
                        TaskMap.Enums.EnumTaskProgress tskProgress = taskProgressValueList[taskStateIndex];
                        taskCondition.TaskState = tskProgress;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                if (removeList.Count > 0)
                {
                    List<NPCShowCondition.TaskCondition> tempSource = new List<NPCShowCondition.TaskCondition>(source);
                    foreach (NPCShowCondition.TaskCondition taskCondition in removeList)
                    {
                        tempSource.Remove(taskCondition);
                    }
                    source = tempSource.ToArray();
                }
                return source;
            };
            tempNPCDataInfo.NPCShowCondition.TimeRange = EditorGUILayout.Vector2Field("在该时间范围内显示(都为0表示不受该项影响)", tempNPCDataInfo.NPCShowCondition.TimeRange);
            EditorGUILayout.LabelField("NPC的隐藏条件(满足任何一条则必须隐藏):");
            tempNPCDataInfo.NPCShowCondition.TaskConditionsHide = ShowAbourtTaskConditionFunc(tempNPCDataInfo.NPCShowCondition.TaskConditionsHide);
            EditorGUILayout.LabelField("NPC的显示条件(满足任何一条则允许显示):");
            tempNPCDataInfo.NPCShowCondition.TaskConditionShow = ShowAbourtTaskConditionFunc(tempNPCDataInfo.NPCShowCondition.TaskConditionShow);
        }
        EditorGUILayout.LabelField("------------------其他数据------------------");
        switch (tempNPCDataInfo.NPCType)
        {
            case EnumNPCType.Businessman://如果是商人,则otherValue的数据是BusinessmanDataInfo类型的数据
                if (businessmanDataInfo == null)
                {
                    businessmanDataInfo = BusinessmanDataInfo.DeSerializeNow<BusinessmanDataInfo>(tempNPCDataInfo.OtherValue);
                    if (businessmanDataInfo == null)
                        businessmanDataInfo = new BusinessmanDataInfo();
                }
                if (goodsMetaInfoMations == null)
                {
                    goodsMetaInfoMations = new GoodsMetaInfoMations();
                    goodsMetaInfoMations.Load();
                }
                if (goodsTypeToExplanList == null)
                {
                    goodsTypeToExplanList = new List<KeyValuePair<EnumGoodsType, string>>();
                    FieldExplanAttribute.SetEnumExplanDic(goodsTypeToExplanList, 0, temp => ((int)temp) % 1000 != 0);
                }
                if (goodsQualityTypeToExplanList == null)
                {
                    goodsQualityTypeToExplanList = new List<KeyValuePair<EnumQualityType, string>>();
                    FieldExplanAttribute.SetEnumExplanDic(goodsQualityTypeToExplanList, 0);
                }
                //显示商人应该显示的列表
                businessmanScroll = EditorGUILayout.BeginScrollView(businessmanScroll);
                {
                    List<EnumGoodsType> enumGoodsTypes = goodsTypeToExplanList.Select(temp => temp.Key).ToList();
                    string[] enumGoodsExplans = goodsTypeToExplanList.Select(temp => temp.Value).ToArray();
                    EditorGUILayout.BeginHorizontal();
                    goodsTypeIndex = EditorGUILayout.Popup(goodsTypeIndex, enumGoodsExplans);
                    if (GUILayout.Button("添加该物品"))
                    {
                        if (goodsTypeIndex > -1
                            && goodsTypeIndex < enumGoodsExplans.Length
                            && !businessmanDataInfo.GoodsDic.ContainsKey(enumGoodsTypes[goodsTypeIndex]))
                        {
                            businessmanDataInfo.GoodsDic.Add(enumGoodsTypes[goodsTypeIndex], new BusinessmanDataInfo.GoodsDataInfoInner());
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("提示!", "添加失败!", "确定");
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    List<EnumQualityType> qualityTypes = goodsQualityTypeToExplanList.Select(temp => temp.Key).ToList();
                    string[] qualityExplans = goodsQualityTypeToExplanList.Select(temp => temp.Value).ToArray();
                    List<EnumGoodsType> removeGoodsTypes = new List<EnumGoodsType>();
                    foreach (KeyValuePair<EnumGoodsType, BusinessmanDataInfo.GoodsDataInfoInner> item in businessmanDataInfo.GoodsDic)
                    {
                        EditorGUILayout.BeginHorizontal();
                        int goodsTypeIndex = enumGoodsTypes.IndexOf(item.Key);
                        if (goodsTypeIndex > -1)
                        {
                            EditorGUILayout.LabelField(enumGoodsExplans[goodsTypeIndex], GUILayout.Width(150));
                            int _qualityIndex_min = qualityTypes.IndexOf(item.Value.MinQualityType);
                            int qualityIndex_min = EditorGUILayout.Popup(_qualityIndex_min, qualityExplans, GUILayout.Width(150));
                            if (_qualityIndex_min != qualityIndex_min && qualityIndex_min > -1)
                                item.Value.MinQualityType = qualityTypes[qualityIndex_min];
                            int _qualityIndex_Max = qualityTypes.IndexOf(item.Value.MaxQualityType);
                            int qualityIndex_max = EditorGUILayout.Popup(_qualityIndex_Max, qualityExplans, GUILayout.Width(150));
                            if (_qualityIndex_Max != qualityIndex_max && qualityIndex_max > -1)
                                item.Value.MaxQualityType = qualityTypes[qualityIndex_max];
                            if (GUILayout.Button("×", GUILayout.Width(25)))
                                if (EditorUtility.DisplayDialog("请再次确认!", "是否删除?", "确定", "取消"))
                                    removeGoodsTypes.Add(item.Key);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    removeGoodsTypes.ForEach(temp => businessmanDataInfo.GoodsDic.Remove(temp));
                }
                EditorGUILayout.EndScrollView();
                break;
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void OnDestroy()
    {
        if (ResultHandle != null)
        {
            if (tempNPCDataInfo.NPCObj != null)
            {
                TalkShowPosition talkShowPosition = tempNPCDataInfo.NPCObj.GetComponent<TalkShowPosition>();
                if (talkShowPosition != null)
                    GameObject.DestroyImmediate(talkShowPosition);
            }
            if (nowIDList.Contains(tempNPCDataInfo.NPCID) ||
                string.IsNullOrEmpty(tempNPCDataInfo.npcPrefabName) ||
                !isCreate)
            {
                if (tempNPCDataInfo != null && tempNPCDataInfo.NPCObj)
                    GameObject.DestroyImmediate(tempNPCDataInfo.NPCObj);
                tempNPCDataInfo = null;
            }
            //如果对象是商人,则将心有的数据序列化进去
            if (tempNPCDataInfo != null && tempNPCDataInfo.NPCType == EnumNPCType.Businessman)
                tempNPCDataInfo.OtherValue = BusinessmanDataInfo.SerializeNow(businessmanDataInfo);
            ResultHandle(tempNPCDataInfo);
        }
    }

}

/// <summary>
/// 用于显示对话位置控制柄(脚本)
/// </summary>
public class TalkShowPosition : MonoBehaviour
{
    /// <summary>
    /// 要创建的NPC数据
    /// </summary>
    public NPCDataInfo tempNPCDataInfo;
}

/// <summary>
/// 用于显示对话位置控制柄(编辑器)
/// </summary>
[CustomEditor(typeof(TalkShowPosition))]
public class TalkShowPositionEditor : Editor
{
    TalkShowPosition targetObj;

    private void Awake()
    {
        targetObj = target as TalkShowPosition;
    }

    private void OnSceneGUI()
    {
        if (targetObj != null && targetObj.tempNPCDataInfo != null)
        {
            Vector3 vec = Handles.PositionHandle(targetObj.transform.position + targetObj.tempNPCDataInfo.TalkShowOffset, targetObj.transform.rotation);
            targetObj.tempNPCDataInfo.TalkShowOffset = vec - targetObj.transform.position;
        }
    }
}

