using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// 功能交互数据
/// </summary>
public class ActionInteractiveData : ILoadable<StuffData>
{
    /// <summary>
    /// 保存功能交互数据的路径
    /// </summary>
    public static string dataDirectoryPath = "Data/ActionInteractiveData";

    /// <summary>
    /// 功能交互信息集合数组
    /// </summary>
    ActionInteractiveDataInfoCollection[] actionInteractiveDataInfoCollections;

    public void Load()
    {
        TextAsset[] allTextAssets = Resources.LoadAll<TextAsset>(ActionInteractiveData.dataDirectoryPath);//加载所有场景的功能交互信息
        List<ActionInteractiveDataInfoCollection> actionInteractiveDataInfoCollectionList = new List<ActionInteractiveDataInfoCollection>();
        foreach (TextAsset textAsset in allTextAssets)
        {
            string assetName = textAsset.name;
            string assetText = Encoding.UTF8.GetString(textAsset.bytes);
            ActionInteractiveDataInfoCollection actionInteractiveDataInfoCollection = DeSerializeNow<ActionInteractiveDataInfoCollection>(assetText);
            if (actionInteractiveDataInfoCollection != null)
            {
                actionInteractiveDataInfoCollectionList.Add(actionInteractiveDataInfoCollection);
            }
        }
        actionInteractiveDataInfoCollections = actionInteractiveDataInfoCollectionList.ToArray();
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
    /// 通过场景名和功能交互对象id获取功能交互对象信息
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="ID"></param>
    /// <returns></returns>
    public ActionInteractiveDataInfo GetActionInteractiveDataInfo(string sceneName, int ID)
    {
        ActionInteractiveDataInfoCollection actionInteractiveDataInfoCollection = actionInteractiveDataInfoCollections.FirstOrDefault(temp => string.Equals(temp.sceneName, sceneName));
        if (actionInteractiveDataInfoCollection != null)
            return actionInteractiveDataInfoCollection.ActionInteractiveDataInfos.FirstOrDefault(temp => temp.ActionInteractiveID == ID);
        return null;
    }

    /// <summary>
    /// 通过场景名获取该场景的所有功能信息交互对象
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public ActionInteractiveDataInfo[] GetActionInteractiveDataInfos(string sceneName)
    {
        ActionInteractiveDataInfoCollection actionInteractiveDataInfoCollection = actionInteractiveDataInfoCollections.FirstOrDefault(temp => string.Equals(temp.sceneName, sceneName));
        if (actionInteractiveDataInfoCollection != null && actionInteractiveDataInfoCollection.ActionInteractiveDataInfos != null)
            return actionInteractiveDataInfoCollection.ActionInteractiveDataInfos.ToArray();
        return new ActionInteractiveDataInfo[0];
    }
}

/// <summary>
/// 功能交互信息集合(要给场景一个该对象)
/// </summary>
[Serializable]
public class ActionInteractiveDataInfoCollection
{
    /// <summary>
    /// 功能交互信息集合
    /// </summary>
    public List<ActionInteractiveDataInfo> ActionInteractiveDataInfos;
    /// <summary>
    /// 场景名 
    /// </summary>
    public string sceneName;

    public ActionInteractiveDataInfoCollection()
    {
        ActionInteractiveDataInfos = new List<ActionInteractiveDataInfo>();
    }
}

/// <summary>
///  单个功能交互信息
/// </summary>
[Serializable]
public class ActionInteractiveDataInfo
{
    /// <summary>
    /// 保存交互信息预设体的路径
    /// </summary>
    [JsonIgnore]
    public static string prefabDirectoryPath = "Prefabs/ActionInteractive";

    /// <summary>
    /// 功能交互对象的ID
    /// </summary>
    public int ActionInteractiveID;

    /// <summary>
    /// 功能交互对象的名字 
    /// </summary>
    public string ActionInteractiveName;

    /// <summary>
    /// 功能交互的类型
    /// </summary>
    public EnumActionInteractiveType ActionInteractiveType;

    /// <summary>
    /// 功能交互对象的位置(分量x)
    /// </summary>
    [JsonProperty]
    private float locationX;
    /// <summary>
    /// 功能交互对象的位置(分量y)
    /// </summary>
    [JsonProperty]
    private float locationY;
    /// <summary>
    /// 功能交互对象的位置(分量z)
    /// </summary>
    [JsonProperty]
    private float locationZ;
    /// <summary>
    /// 功能交互对象的位置
    /// </summary>
    [JsonIgnore]
    public Vector3 ActionInteractiveLocation
    {
        get
        {
            return new Vector3(locationX, locationY, locationZ);
        }
        set
        {
            locationX = value.x;
            locationY = value.y;
            locationZ = value.z;
        }
    }
    /// <summary>
    /// 功能交互对象的角度(分量x)
    /// </summary>
    [JsonProperty]
    private float angleX;
    /// <summary>
    /// 功能交互对象的角度(分量y)
    /// </summary>
    [JsonProperty]
    private float angleY;
    /// <summary>
    /// 功能交互对象的角度(分量z)
    /// </summary>
    [JsonProperty]
    private float angleZ;
    /// <summary>
    /// 功能交互对象的角度
    /// </summary>
    [JsonIgnore]
    public Vector3 ActionInteractiveAngle
    {
        get
        {
            return new Vector3(angleX, angleY, angleZ);
        }
        set
        {
            angleX = value.x;
            angleY = value.y;
            angleZ = value.z;
        }
    }
    /// <summary>
    /// 功能交互对象所在场景
    /// </summary>
    public string SceneName;
    /// <summary>
    /// 功能交互对象的预设提名字
    /// </summary>
    public string actionInteractivePrefabName;
    /// <summary>
    /// 功能交互对象的预设提对象
    /// </summary>
    [JsonIgnore]
    private GameObject ActionInteractiveObjPrefab;
    /// <summary>
    /// 游戏对象
    /// </summary>
    [JsonIgnore]
    public GameObject ActionInteractiveObj { get; set; }
    /// <summary>
    /// 附加的数据
    /// </summary>
    public object OtherValue;

    /// <summary>
    /// 加载
    /// </summary>
    /// <param name="isEditor">是否是编辑器</param>
    public void Load(bool isEditor = false)
    {
        if (string.IsNullOrEmpty(actionInteractivePrefabName))
            ActionInteractiveObjPrefab = null;
        if (ActionInteractiveObjPrefab == null)
            if (!string.IsNullOrEmpty(actionInteractivePrefabName))
                ActionInteractiveObjPrefab = Resources.Load<GameObject>(prefabDirectoryPath + "/" + actionInteractivePrefabName);
        if (ActionInteractiveObj == null && ActionInteractiveObjPrefab != null)
        {
            ActionInteractiveObj = GameObject.Instantiate<GameObject>(ActionInteractiveObjPrefab, ActionInteractiveLocation, Quaternion.identity);
            ActionInteractiveObj.transform.eulerAngles = ActionInteractiveAngle;
            ActionInteractiveObj.name = ActionInteractiveName;
            if (isEditor)//如果是编辑器直接返回
                return;
            ActionInteractiveDataInfoMono addScript = null;
            switch (ActionInteractiveType)
            {
                case EnumActionInteractiveType.None://此时的otherValue为null
                    addScript = ActionInteractiveObj.AddComponent<ActionInteractiveDataInfoMono>();
                    break;
                case EnumActionInteractiveType.TreasureBox://此时的otherValue为物品与数量之间的字典
                    addScript = ActionInteractiveObj.AddComponent<ActionInteractiveDataInfoMono_TreasureBox>();
                    break;
                case EnumActionInteractiveType.Other://此时的otherValue为OtherDataStruct数据
                    if (OtherValue != null)
                    {
                        OtherDataStruct otherDataStruct = OtherValue as OtherDataStruct;
                        if (otherDataStruct != null && otherDataStruct.ActionInterativeClass != null)
                        {
                            Component component = ActionInteractiveObj.AddComponent(otherDataStruct.ActionInterativeClass);
                            addScript = component as ActionInteractiveDataInfoMono;
                        }
                    }
                    break;
                default:
                    break;
            }
            if (addScript == null)
                return;
            addScript.ActionInteractiveDataInfo = this;
            ActionInteractiveStateData actionInteractiveStateData = DataCenter.Instance.GetEntity<ActionInteractiveStateData>();
            if (actionInteractiveStateData != null)
            {
                if (!actionInteractiveStateData.SceneDic.ContainsKey(SceneName))
                    actionInteractiveStateData.SceneDic.Add(SceneName, new ActionInteractiveStateData.SceneData());
                ActionInteractiveStateData.SceneData sceneData = actionInteractiveStateData.SceneDic[SceneName];
                if (!sceneData.IDDic.ContainsKey(ActionInteractiveID))
                    sceneData.IDDic.Add(ActionInteractiveID, null);
                addScript.LoadData(sceneData.IDDic[ActionInteractiveID], temp => sceneData.IDDic[ActionInteractiveID] = temp);
            }
        }
    }

    /// <summary>
    /// 如果是其他类型时,附加数据的数据
    /// </summary>
    [Serializable]
    public class OtherDataStruct
    {
        /// <summary>
        /// 用于处理的类型
        /// </summary>
        public Type ActionInterativeClass;
        /// <summary>
        /// 数据
        /// </summary>
        public string Data;
    }

    /// <summary>
    /// 如果是宝箱类型时,附加数据的数据
    /// </summary>
    [Serializable]
    public class TreasureBoxStruct
    {
        /// <summary>
        /// 最小品质
        /// </summary>
        public EnumQualityType MinQualityType;
        /// <summary>
        /// 最大品质
        /// </summary>
        public EnumQualityType MaxQualityType;
        /// <summary>
        /// 数量
        /// </summary>
        public int Count;
    }
}

/// <summary>
/// 功能交互对象的类型
/// </summary>
public enum EnumActionInteractiveType
{
    /// <summary>
    /// 基础类型(ActionInteractiveDataInfoMono类型)
    /// </summary>
    [FieldExplan("基础类型")]
    None,
    /// <summary>
    /// 宝箱(ActionInteractiveDataInfoMono类型)
    /// </summary>
    [FieldExplan("宝箱")]
    TreasureBox,
    /// <summary>
    /// 其他类型(继承自ActionInteractiveDataInfoMono的类型)
    /// </summary>
    [FieldExplan("其他类型")]
    Other,
}

/// <summary>
/// 功能交互的数据(用于挂在在对象身上)
/// </summary>
public class ActionInteractiveDataInfoMono : DataInfoType<ActionInteractiveDataInfoMono>
{
    public ActionInteractiveDataInfo ActionInteractiveDataInfo;

    /// <summary>
    /// 保存数据委托
    /// </summary>
    protected Action<object> SaveDataAction;

    /// <summary>
    /// 加载数据
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="SaveDataAction"></param>
    public virtual void LoadData(object obj,Action<object> SaveDataAction)
    {
        this.SaveDataAction = SaveDataAction;
    }

    /// <summary>
    /// 触发功能
    /// </summary>
    public virtual void ActionTrigger() { }
}
