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
    static void AddWindown()
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

        List<string> names = npcDataDic.Keys.OfType<string>().ToList();
        int index = names.IndexOf(tempNPCDataInfo.npcPrefabName);
        if (tempNPCDataInfo.NPCObj)
            EditorGUILayout.LabelField(tempNPCDataInfo.npcPrefabName);
        else
        {
            index = EditorGUILayout.Popup(index, names.ToArray());
            if (index >= 0)
                tempNPCDataInfo.npcPrefabName = names[index];
        }

        if (index >= 0)
            EditorGUILayout.ObjectField(npcDataDic[tempNPCDataInfo.npcPrefabName], typeof(GameObject), true);
        int id = EditorGUILayout.IntField("NPC ID:", tempNPCDataInfo.NPCID);
        if (!nowIDList.Contains(id))
            tempNPCDataInfo.NPCID = id;
        tempNPCDataInfo.NPCName = EditorGUILayout.TextField("NPC Name:", tempNPCDataInfo.NPCName);
        tempNPCDataInfo.NPCType = (EnumNPCType)EditorGUILayout.EnumPopup("NPC Type:", tempNPCDataInfo.NPCType);
        tempNPCDataInfo.OtherValue = EditorGUILayout.TextField("Other Data:", tempNPCDataInfo.OtherValue);
        if (isCreate)
        {
            EditorGUILayout.ObjectField("NPC Object:", tempNPCDataInfo.NPCObj, typeof(GameObject), true);
        }
        if (!isCreate && index >= 0 && GUILayout.Button("Create NPC GameObject"))
        {
            GameObject createObj = GameObject.Instantiate<GameObject>(npcDataDic[tempNPCDataInfo.npcPrefabName]);
            tempNPCDataInfo.NPCObj = createObj;
            isCreate = true;
        }
        Sprite tempSprite = (Sprite)EditorGUILayout.ObjectField("NPC Sprite:", tempNPCDataInfo.NPCSprite, typeof(Sprite), false);
        if (tempSprite != tempNPCDataInfo.NPCSprite)
        {
            tempNPCDataInfo.npcSpriteID = SpriteManager.GetName(tempSprite);
            tempNPCDataInfo.NPCSprite = tempSprite;
        }

        EditorGUILayout.EndVertical();
    }

    private void OnDestroy()
    {
        if (ResultHandle != null)
        {
            if (nowIDList.Contains(tempNPCDataInfo.NPCID) ||
                string.IsNullOrEmpty(tempNPCDataInfo.npcPrefabName) ||
                !isCreate)
            {
                if (tempNPCDataInfo != null && tempNPCDataInfo.NPCObj)
                    GameObject.DestroyImmediate(tempNPCDataInfo.NPCObj);
                tempNPCDataInfo = null;
            }
            ResultHandle(tempNPCDataInfo);
        }
    }
}

