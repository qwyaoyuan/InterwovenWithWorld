using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TaskMap
{
    /// <summary>
    /// 所有的枚举
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public enum EnumTaskType
        {
            /// <summary>
            /// 主线
            /// </summary>
            [FieldExplan("主线")]
            Main,
            /// <summary>
            /// 支线
            /// </summary>
            [FieldExplan("支线")]
            Spur,
            /// <summary>
            /// 重复
            /// </summary>
            [FieldExplan("重复")]
            Repeat,
            /// <summary>
            /// 随机
            /// </summary>
            [FieldExplan("随机")]
            Random
        }

        /// <summary>
        /// 性格倾向
        /// </summary>
        public enum CharacterTendency
        {
            [FieldExplan("无")]
            None,
            /// <summary>
            /// 好杀戮的
            /// </summary>
            [FieldExplan("好杀戮的")]
            Slaughterous,
            /// <summary>
            /// 和平的
            /// </summary>
            [FieldExplan("和平的")]
            Peaceable,
        }

        /// <summary>
        /// 任务进度枚举
        /// </summary>
        public enum EnumTaskProgress
        {
            /// <summary>
            /// 未接取(任务还未接取)
            /// </summary>
            NoTake,
            /// <summary>
            /// 失败(一般用于互斥任务)
            /// </summary>
            Failed,
            /// <summary>
            /// 已成功(任务完成后)
            /// </summary>
            Sucessed,
            /// <summary>
            /// 已开始(任务正在执行)
            /// </summary>
            Started,
        }

        /// <summary>
        /// 任务地点
        /// </summary>
        public class TaskLocation
        {
            /// <summary>
            /// 场景名
            /// </summary>
            public string SceneName { get; set; }

            [JsonProperty]
            private float x;
            [JsonProperty]
            private float y;
            [JsonProperty]
            private float z;
            /// <summary>
            /// 到达的中心位置
            /// </summary>
            [JsonIgnore]
            public Vector3 ArrivedCenterPos
            {
                get
                {
                    return new Vector3(x, y, z);
                }
                set
                {
                    x = value.x;
                    y = value.y;
                    z = value.z;
                }
            }
            /// <summary>
            /// 半径
            /// </summary>
            public int Radius { get; set; }
        }
    }
}
