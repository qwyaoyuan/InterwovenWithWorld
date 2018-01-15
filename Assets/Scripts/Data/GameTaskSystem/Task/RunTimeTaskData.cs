using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;
using MapStruct;


namespace TaskMap
{
    /// <summary>
    /// 运行时的任务数据
    /// </summary>
    public class RunTimeTaskData
    {
        /// <summary>
        /// 默认的路径
        /// </summary>
        const string JsonValueResourcePath = "Data/Task/Task";

        /// <summary>
        /// 任务json字符串
        /// </summary>
        [JsonProperty]
        private string TaskDataJson;

        /// <summary>
        /// 任务对象
        /// </summary>
        [JsonIgnore]
        private TaskMap taskMap;

        public RunTimeTaskData()
        {

        }

        /// <summary>
        /// 序列化完成后挂事件
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            LoadTasks();
        }

        /// <summary>
        /// 通过配置文件加载任务
        /// </summary>
        private void LoadTasks()
        {
            if (TaskDataJson == null || TaskDataJson == "")
            {
                TextAsset textAsset = Resources.Load<TextAsset>(JsonValueResourcePath);
                TaskDataJson = Encoding.UTF8.GetString(textAsset.bytes);
            }
            taskMap = new TaskMap();
            taskMap.Load(TaskDataJson);
        }

        [OnSerializing]
        public void Save(StreamingContext context)
        {
            if (taskMap != null)
                TaskDataJson = taskMap.Save();
        }

        /// <summary>
        /// 获取当前所有可做任务
        /// </summary>
        /// <returns></returns>
        public List<RunTimeTaskInfo> GetAllToDoList()
        {
            List<RunTimeTaskInfo> resultList = taskMap.GetLastFrameNodes().Select(temp =>
                new RunTimeTaskInfo()
                {
                    ID = temp.ID,
                    TaskInfoStruct = temp.Value,
                    TaskInfoNode = temp,
                    TaskMap = taskMap
                }).ToList();
            return resultList;
        }
    }

    /// <summary>
    /// 运行时的任务对象
    /// </summary>
    public class RunTimeTaskInfo
    {
        /// <summary>
        /// 任务的ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 任务数据
        /// </summary>
        [JsonIgnore]
        public TaskInfoStruct TaskInfoStruct { get; internal set; }

        /// <summary>
        /// 任务图
        /// </summary>
        [JsonIgnore]
        internal TaskMap TaskMap { get; set; }

        /// <summary>
        /// 任务节点
        /// </summary>
        [JsonIgnore]
        internal MapElement<TaskInfoStruct> TaskInfoNode { get; set; }

        /// <summary>
        /// 当前的任务状态
        /// </summary>
        [JsonIgnore]
        public Enums.EnumTaskProgress TaskProgress { get { return TaskInfoStruct.TaskProgress; } }

        /// <summary>
        /// 设置是接取任务或放弃任务
        /// 获取该任务是否已经接取
        /// </summary>
        [JsonIgnore]
        public bool IsStart
        {
            get
            {
                return TaskInfoStruct.TaskProgress == Enums.EnumTaskProgress.Started;
            }
            set
            {
                if (value)
                    TaskMap.SetTaskState(TaskInfoNode, Enums.EnumTaskProgress.Started);
                else
                    TaskMap.SetTaskState(TaskInfoNode, Enums.EnumTaskProgress.NoTake);
            }
        }

        /// <summary>
        /// 当前任务是否完成
        /// 失败和完成都是返回true
        /// 只能设置完成,放弃请使用IsStart设置为false
        /// </summary>
        public bool IsOver
        {
            get
            {
                return TaskInfoStruct.TaskProgress == Enums.EnumTaskProgress.Failed ||
                  TaskInfoStruct.TaskProgress == Enums.EnumTaskProgress.Sucessed;
            }
            set
            {
                if (value)
                {
                    TaskMap.SetTaskState(TaskInfoNode, Enums.EnumTaskProgress.Sucessed);
                }
            }
        }
    }
}

