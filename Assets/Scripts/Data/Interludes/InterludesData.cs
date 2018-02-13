using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 过场数据
/// </summary>
public class InterludesData : ILoadable<InterludesData>
{
    /// <summary>
    /// 保存过场信息的文件路径
    /// </summary>
    public static string dataInterludesFilePath = "Data/Interludes/InterludesData";
    /// <summary>
    /// 保存摄像机动画的预设体的文件夹路径
    /// </summary>
    public static string cameraPathDirectoryPath = "Data/Interludes/CameraPath";

    /// <summary>
    /// 过场的数据
    /// </summary>
    InterludesItemStruct[] interludesItemStructs;

    public void Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(InterludesData.dataInterludesFilePath);//加载过场的配置文件
        string assetText = Encoding.UTF8.GetString(textAsset.bytes);
        List<InterludesItemStruct> interludesItemStructList = DeSerializeNow<List<InterludesItemStruct>>(assetText);
        if (interludesItemStructList == null)
            interludesItemStructs = new InterludesItemStruct[0];
        else interludesItemStructs = interludesItemStructList.ToArray();

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


    /// <summary>
    /// 通过任务id获取该任务的过场动画
    /// </summary>
    /// <param name="taskID"></param>
    /// <returns></returns>
    public InterludesItemStruct GetInterludesItemStructByTaskID(int taskID, InterludesItemStruct.EnumInterludesShowTime interludesShowTime = InterludesItemStruct.EnumInterludesShowTime.Start)
    {
        return interludesItemStructs.Where(temp => temp.TaskID == taskID && temp.InterludesShowTime == interludesShowTime).FirstOrDefault();
    }

    /// <summary>
    /// 通过过长的ID获取该任务的过场动画
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public InterludesItemStruct GetInterludesItemStructByID(int id)
    {
        return interludesItemStructs.Where(temp => temp.ID == id).FirstOrDefault();
    }

}


/// <summary>
/// 每条过场的数据
/// </summary>
[Serializable]
public class InterludesItemStruct
{
    /// <summary>
    /// 过场ID
    /// </summary>
    public int ID;

    /// <summary>
    /// 接收该任务的时候触发过场
    /// </summary>
    public int TaskID;

    /// <summary>
    /// 过场动画的显示时间
    /// </summary>
    public EnumInterludesShowTime InterludesShowTime;

    /// <summary>
    /// 过场的数据
    /// </summary>
    public InterludesDataInfo InterludesDataInfo;

    /// <summary>
    /// 过场的显示时间
    /// </summary>
    public enum EnumInterludesShowTime
    {
        /// <summary>
        /// 接收任务时显示
        /// </summary>
        [FieldExplan("任务开始时")]
        Start,
        /// <summary>
        /// 完成任务时显示
        /// </summary>
        [FieldExplan("任务结束时")]
        Over
    }
}

/// <summary>
/// 过场的数据
/// </summary>
[Serializable]
public class InterludesDataInfo
{
    /// <summary>
    /// 数据
    /// </summary>
    public List<ItemData> Datas;

    public InterludesDataInfo()
    {
        Datas = new List<ItemData>();
    }

    /// <summary>
    /// 每一个条目的数据
    /// </summary>
    [Serializable]
    public class ItemData
    {
        /// <summary>
        /// 该段结构的大概的持续时间
        /// </summary>
        public float baseKeepTime;
    }

    /// <summary>
    /// 对话条目
    /// </summary>
    [Serializable]
    public class ItemData_Talk : ItemData
    {
        /// <summary>
        /// 对话的ID
        /// </summary>
        public int TalkID;
    }

    /// <summary>
    /// 摄像机动画条目
    /// </summary>
    [Serializable]
    public class ItemData_CameraPathAnimation: ItemData
    {
        /// <summary>
        /// 保存摄像机动画信息预设体的名字
        /// </summary>
        [JsonProperty]
        private string _ObjPrefabName;
        /// <summary>
        /// 保存摄像机动画信息的预设体
        /// </summary>
        [JsonIgnore]
        private GameObject _ObjPrefab;
        /// <summary>
        /// 保存摄像机动画信息的预设体
        /// </summary>
        [JsonIgnore]
        public GameObject ObjPrefab
        {
            get
            {
                if (!_ObjPrefab && !string.IsNullOrEmpty( _ObjPrefabName))
                {
                    _ObjPrefab = Resources.Load<GameObject>(InterludesData.cameraPathDirectoryPath + "/" + _ObjPrefabName);
                }
                return _ObjPrefab;
            }
            set
            {
                _ObjPrefab = value;
                if (value)
                    _ObjPrefabName = value.name;
            }
        }
    }
}
