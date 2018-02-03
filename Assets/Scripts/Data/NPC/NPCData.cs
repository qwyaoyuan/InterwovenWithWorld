using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// NPC数据
/// </summary>
public class NPCData : ILoadable<NPCData>
{
    /// <summary>
    /// 保存NPC信息的路径
    /// </summary>
    public static string dataDirectoryPath = "Data/NPC";

    /// <summary>
    /// npc信息集合数组
    /// </summary>
    NPCDataInfoCollection[] npcDataInfoCollections;

    public void Load()
    {
        TextAsset[] allTextAssets = Resources.LoadAll<TextAsset>(NPCData.dataDirectoryPath);//加载所有场景的npc信息
        List<NPCDataInfoCollection> npcDataInfoCollectionList = new List<NPCDataInfoCollection>();
        foreach (TextAsset textAsset in allTextAssets)
        {
            string assetName = textAsset.name;
            string assetText = Encoding.UTF8.GetString(textAsset.bytes);
            NPCDataInfoCollection npcDataInfoCollection = DeSerializeNow<NPCDataInfoCollection>(assetText);
            if (npcDataInfoCollection != null)
            {
                npcDataInfoCollectionList.Add(npcDataInfoCollection);
            }
        }
        npcDataInfoCollections = npcDataInfoCollectionList.ToArray();
        //重新设置一遍他们各自所在的场景
        foreach (NPCDataInfoCollection npcDataInfoCollection in npcDataInfoCollections)
        {
            foreach (NPCDataInfo npcDataInfo in npcDataInfoCollection.NPCDataInfos)
            {
                if (string.IsNullOrEmpty(npcDataInfo.SceneName))
                {
                    npcDataInfo.SceneName = npcDataInfoCollection.sceneName;
                }
            }
        }
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T">反序列化后的类型</typeparam>
    /// <param name="value">字符串</param>
    /// <returns>对象</returns>
    private T DeSerializeNow<T>(string value) where T : class
    {
        try
        {
            T target = JsonConvert.DeserializeObject<T>(value);
            return target;
        }
        catch { }
        return null;
    }

    /// <summary>
    /// 通过npc id获取npc信息
    /// </summary>
    /// <param name="npcID">npc id</param>
    /// <returns></returns>
    public NPCDataInfo GetNPCDataInfo(string sceneName, int npcID)
    {
        NPCDataInfoCollection npcDataInfoCollection = npcDataInfoCollections.FirstOrDefault(temp => string.Equals(temp.sceneName, sceneName));
        if (npcDataInfoCollection != null)
            return npcDataInfoCollection.NPCDataInfos.Where(temp => temp.NPCID == npcID).FirstOrDefault();
        return null;
    }

    /// <summary>
    /// 通过指定的检索条件获取所有复合条件的npc信息
    /// </summary>
    /// <param name="CheckFunc"></param>
    /// <returns></returns>
    public NPCDataInfo[] GetNPCDataInfo(Func<NPCDataInfo, bool> CheckFunc = null)
    {
        if (CheckFunc != null)
        {
            return npcDataInfoCollections.Select(temp => temp.NPCDataInfos).Aggregate((temp1, temp2) => temp1.Concat(temp2).ToList()).Where(temp => CheckFunc(temp)).ToArray();
        }
        return npcDataInfoCollections.Select(temp => temp.NPCDataInfos).Aggregate((temp1, temp2) => temp1.Concat(temp2).ToList()).ToArray();

    }

    /// <summary>
    /// 通过场景名获取该场景的所有NPC信息
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public NPCDataInfo[] GetNPCDataInfos(string sceneName)
    {
        NPCDataInfoCollection npcDataInfoCollection = npcDataInfoCollections.FirstOrDefault(temp => string.Equals(temp.sceneName, sceneName));
        if (npcDataInfoCollection != null && npcDataInfoCollection.NPCDataInfos != null)
            return npcDataInfoCollection.NPCDataInfos.ToArray();
        return new NPCDataInfo[0];
    }

}

/// <summary>
/// npc信息集合(一个场景一个该对象)
/// </summary>
[Serializable]
public class NPCDataInfoCollection
{
    /// <summary>
    /// NPC信息
    /// </summary>
    public List<NPCDataInfo> NPCDataInfos;

    /// <summary>
    /// 场景名
    /// </summary>
    public string sceneName;

    public NPCDataInfoCollection()
    {
        NPCDataInfos = new List<NPCDataInfo>();
    }
}

/// <summary>
/// 单个NPC信息
/// </summary>
[Serializable]
public class NPCDataInfo
{
    /// <summary>
    /// 保存npc预设提的路径
    /// </summary>
    [JsonIgnore]
    public static string npcPrefabDirectoryPath = "Prefabs/NPC";

    /// <summary>
    /// npc的类型
    /// </summary>
    public EnumNPCType NPCType;
    /// <summary>
    /// npc的id
    /// </summary>
    public int NPCID;
    /// <summary>
    /// npc的名字
    /// </summary>
    public string NPCName;
    /// <summary>
    /// npc的位置(分量x)
    /// </summary>
    [JsonProperty]
    private float locationX;
    /// <summary>
    /// npc的位置(分量y)
    /// </summary>
    [JsonProperty]
    private float locationY;
    /// <summary>
    /// npc的位置(分量z)
    /// </summary>
    [JsonProperty]
    private float locationZ;
    /// <summary>
    /// npc的位置
    /// </summary>
    [JsonIgnore]
    public Vector3 NPCLocation
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
    /// npc的角度(分量x)
    /// </summary>
    [JsonProperty]
    private float angleX;
    /// <summary>
    /// npc的角度(分量y)
    /// </summary>
    [JsonProperty]
    private float angleY;
    /// <summary>
    /// npc的角度(分量z)
    /// </summary>
    [JsonProperty]
    private float angleZ;
    /// <summary>
    /// npc的角度
    /// </summary>
    [JsonIgnore]
    public Vector3 NPCAngle
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
    /// npc对话相对中心的偏差(分量x)
    /// </summary>
    [JsonProperty]
    private float talkShowOffsetX;
    /// <summary>
    /// npc对话相对中心的偏差(分量y)
    /// </summary>
    [JsonProperty]
    private float talkShowOffsetY;
    /// <summary>
    /// npc对话相对中心的偏差(分量z)
    /// </summary>
    [JsonProperty]
    private float talkShowOffsetZ;
    /// <summary>
    /// npc对话相对中心的偏差
    /// </summary>
    [JsonIgnore]
    public Vector3 TalkShowOffset
    {
        get
        {
            return new Vector3(talkShowOffsetX, talkShowOffsetY, talkShowOffsetZ);
        }
        set
        {
            talkShowOffsetX = value.x;
            talkShowOffsetY = value.y;
            talkShowOffsetZ = value.z;
        }
    }
    /// <summary>
    /// 用于查找npc图标的唯一id
    /// </summary>
    public string npcSpriteID;
    /// <summary>
    /// npc在地图上的图标
    /// </summary>
    [JsonIgnore]
    public Sprite NPCSprite { get; set; }
    /// <summary>
    /// npc所在场景
    /// </summary>
    public string SceneName;
    /// <summary>
    /// npc的预设提名字
    /// </summary>
    public string npcPrefabName;
    /// <summary>
    /// npc的预设提对象
    /// </summary>
    [JsonIgnore]
    private GameObject npcObjPrefab;
    /// <summary>
    /// 游戏对象
    /// </summary>
    [JsonIgnore]
    public GameObject NPCObj { get; set; }
    /// <summary>
    /// 附加的数据
    /// </summary>
    public string OtherValue;
    /// <summary>
    /// NPC的显示条件,如果为null表示一直显示,如果不为null则判断
    /// </summary>
    public NPCShowCondition NPCShowCondition { get; set; }
#if UNITY_EDITOR
    /// <summary>
    /// 将NPCObjPrefab设置为false
    /// </summary>
    public void InitNPCObjPrefab()
    {
        npcObjPrefab = null;
    }
#endif

    public void Load()
    {
        if (string.IsNullOrEmpty(npcPrefabName))
            npcObjPrefab = null;
        if (npcObjPrefab == null)
            if (!string.IsNullOrEmpty(npcPrefabName))
                npcObjPrefab = Resources.Load<GameObject>(npcPrefabDirectoryPath + "/" + npcPrefabName);
        if (NPCObj == null && npcObjPrefab != null)
        {
            NPCObj = GameObject.Instantiate<GameObject>(npcObjPrefab);
            NPCObj.transform.position = NPCLocation;
            NPCObj.transform.eulerAngles = NPCAngle;
            NPCObj.name = NPCName;
            NPCDataInfoMono npcDaatInfoMono = NPCObj.AddComponent<NPCDataInfoMono>();
            npcDaatInfoMono.NPCDataInfo = this;
        }
        if (NPCSprite == null)
            if (!string.IsNullOrEmpty(npcSpriteID))
                NPCSprite = SpriteManager.GetSrpite(npcSpriteID);

    }
}

/// <summary>
/// NPC的显示条件(可以分为时间和任务范围)
/// </summary>
[Serializable]
public class NPCShowCondition
{
    [JsonProperty] private float timeRange_x;
    [JsonProperty] private float timeRange_y;
    /// <summary>
    /// 时间范围(范围内显示该对象)
    /// 
    /// (注意如果第一个数字大于第二个数字表示回环,如23-1表示晚上23点到第二天的早上1点)
    /// 
    /// (如果该数值为0,0表示不受时间范围约束)
    /// </summary>
    [JsonIgnore]
    public Vector2 TimeRange
    {
        get { return new Vector2(timeRange_x, timeRange_y); }
        set { timeRange_x = value.x; timeRange_y = value.y; }
    }
    /// <summary>
    /// 任务的条件(隐藏)
    /// 
    /// 注意这里判断的是隐藏条件,如果有任何一条满足则该NPC隐藏(优先级大于显示)
    /// </summary>
    public TaskCondition[] TaskConditionsHide;
    /// <summary>
    /// 任务的条件(显示)
    /// 
    /// 注意这里判断的是显示条件,如果有任何一条满足则该NPC可以显示(优先级小于隐藏)
    /// </summary>
    public TaskCondition[] TaskConditionShow;

    /// <summary>
    /// 时间检测是否通过(通过表示可以显示,否则必须隐藏)
    /// </summary>
    [JsonIgnore]
    public bool TimeRangePath;
    /// <summary>
    /// 隐藏条件是否通过(通过表示必须隐藏,否则可以显示)
    /// </summary>
    [JsonIgnore]
    public bool TaskCoditionHidePath;
    /// <summary>
    /// 显示条件是否通过(通过表示可以显示,否则必须隐藏)
    /// </summary>
    [JsonIgnore]
    public bool TaskCoditionShowPath;

    /// <summary>
    /// 任务的条件
    /// </summary>
    [Serializable]
    public class TaskCondition
    {
        /// <summary>
        /// 任务id
        /// </summary>
        public int TaskID;
        /// <summary>
        /// 任务的状态
        /// </summary>
        public TaskMap.Enums.EnumTaskProgress TaskState;
    }
}


/// <summary>
/// npc的数据(用于挂在在对象身上)
/// </summary>
public class NPCDataInfoMono : DataInfoType<NPCDataInfoMono>
{
    /// <summary>
    /// npc的数据
    /// </summary>
    public NPCDataInfo NPCDataInfo;

    /// <summary>
    /// 任务结构对象
    /// </summary>
    TaskMap.RunTimeTaskData runTimeTaskData;

    private void Start()
    {
        if (GameState.Instance != null)
        {
            GameState.Instance.Registor<INowTaskState>(CheckTask);
            CheckTask(GameState.Instance.GetEntity<INowTaskState>());
            runTimeTaskData = DataCenter.Instance.GetEntity<TaskMap.RunTimeTaskData>();
        }
        if (NPCDataInfo != null)
        {
            if (NPCDataInfo.NPCShowCondition != null)
            {
                NPCDataInfo.NPCShowCondition.TimeRangePath = true;
                NPCDataInfo.NPCShowCondition.TaskCoditionHidePath = false;
                NPCDataInfo.NPCShowCondition.TaskCoditionShowPath = true;
            }
        }
    }


    private void CheckTask(INowTaskState iNowTaskState, string fieldName)
    {
        CheckTask(iNowTaskState);
    }

    /// <summary>
    /// 判断任务情况
    /// </summary>
    /// <param name="iNowTaskState"></param>
    /// <param name="fieldName"></param>
    private void CheckTask(INowTaskState iNowTaskState)
    {
        if (iNowTaskState != null && NPCDataInfo != null && NPCDataInfo.NPCShowCondition != null && runTimeTaskData != null)
        {
            NPCDataInfo.NPCShowCondition.TaskCoditionHidePath = false;
            if (NPCDataInfo.NPCShowCondition.TaskConditionsHide != null && NPCDataInfo.NPCShowCondition.TaskConditionsHide.Length > 0)
            {
                foreach (var item in NPCDataInfo.NPCShowCondition.TaskConditionsHide)
                {
                    TaskMap.RunTimeTaskInfo runtimeTaskInfo = runTimeTaskData.GetTasksWithID(item.TaskID, false);
                    if (runtimeTaskInfo != null && runtimeTaskInfo.TaskProgress == item.TaskState)
                    {
                        NPCDataInfo.NPCShowCondition.TaskCoditionHidePath = true;
                        break;
                    }
                }
            }
            NPCDataInfo.NPCShowCondition.TaskCoditionShowPath = false;
            if (NPCDataInfo.NPCShowCondition.TaskConditionShow != null && NPCDataInfo.NPCShowCondition.TaskConditionShow.Length > 0)
            {
                foreach (var item in NPCDataInfo.NPCShowCondition.TaskConditionShow)
                {
                    TaskMap.RunTimeTaskInfo runtimeTaskInfo = runTimeTaskData.GetTasksWithID(item.TaskID, false);
                    if (runtimeTaskInfo != null && runtimeTaskInfo.TaskProgress == item.TaskState)
                    {
                        NPCDataInfo.NPCShowCondition.TaskCoditionShowPath = true;
                        break;
                    }
                }
            }
            else NPCDataInfo.NPCShowCondition.TaskCoditionShowPath = true;
        }
    }

    /// <summary>
    /// 在Update里面判断是否可以显示(游戏内部时间)
    /// </summary>
    void Update()
    {
        if (GameState.Instance != null)
        {
            if (NPCDataInfo != null && NPCDataInfo.NPCShowCondition != null)
            {
                if (NPCDataInfo.NPCShowCondition.TimeRange.Equals(Vector2.zero))
                    NPCDataInfo.NPCShowCondition.TimeRangePath = true;
                else
                {
                    //暂时没有时间系统
                    NPCDataInfo.NPCShowCondition.TimeRangePath = true;
                }
                UpdateShow();
            }
        }
    }

    /// <summary>
    /// 更新显示
    /// </summary>
    void UpdateShow()
    {
        if (NPCDataInfo != null)
        {
            if (NPCDataInfo.NPCShowCondition != null)
            {
                bool state = (!NPCDataInfo.NPCShowCondition.TaskCoditionHidePath) && NPCDataInfo.NPCShowCondition.TaskCoditionShowPath && NPCDataInfo.NPCShowCondition.TimeRangePath;
                if (gameObject.activeSelf != state)
                    gameObject.SetActive(state);
            }
        }
    }

    private void OnDestroy()
    {
        if (GameState.Instance != null)
        {
            GameState.Instance.UnRegistor<INowTaskState>(CheckTask);
        }
    }
}

/// <summary>
/// npc类型
/// </summary>
public enum EnumNPCType
{
    /// <summary>
    /// 普通(有可能是场景道具)
    /// </summary>
    Normal,
    /// <summary>
    /// 商人 
    /// </summary>
    Businessman,
    /// <summary>
    /// 合成人
    /// </summary>
    Synthesiser,
    /// <summary>
    /// 打造人
    /// </summary>
    Forge,
    /// <summary>
    /// 佣兵提交
    /// </summary>
    Mercenarier,
    /// <summary>
    /// 路牌
    /// </summary>
    Street,
}