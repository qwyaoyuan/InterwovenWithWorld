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
        /// 特殊检测
        /// </summary>
        public enum EnumTaskSpecialCheck
        {
            /// <summary>
            /// 不存在特殊检测 
            /// </summary>
            [FieldExplan("不存在特殊检测")]
            None,
            /// <summary>
            /// 打开菜单
            /// </summary>
            [FieldExplan("打开菜单")]
            OpenMenuUI,
            /// <summary>
            /// 进入属性展示界面
            /// </summary>
            [FieldExplan("进入属性展示界面")]
            OpenAttributeUI,
            /// <summary>
            /// 进入词条展示界面
            /// </summary>
            [FieldExplan("进入词条展示界面")]
            OpenEntryUI,
            /// <summary>
            /// 进入道具展示界面
            /// </summary>
            [FieldExplan("进入道具展示界面")]
            OpenItemUI,
            /// <summary>
            /// 学习火元素技能
            /// </summary>
            [FieldExplan("学习火元素技能")]
            LearnFireSkill,
            /// <summary>
            /// 将火元素技能放入技能格子
            /// </summary>
            [FieldExplan("将火球术技能放入技能格子")]
            SetFireSkillToLattice,
            /// <summary>
            /// 使用了传讯魔法卷轴
            /// </summary>
            [FieldExplan("使用了传讯魔法卷轴")]
            SummonsScrollMagic,
            /// <summary>
            /// 等待一定时间不适用传讯卷轴被发现(或离开一定的位置)
            /// </summary>
            [FieldExplan("等待一定时间不适用传讯卷轴被发现(或离开一定的位置)")]
            WaiTimeNotSummonsScroll,
            /// <summary>
            /// 地下迷宫火炬1
            /// </summary>
            [FieldExplan("地下迷宫火炬1")]
            DXMG_TorchLight1,
            /// <summary>
            /// 地下迷宫火炬2
            /// </summary>
            [FieldExplan("地下迷宫火炬2")]
            DXMG_TorchLight2,
            /// <summary>
            /// 地下迷宫火炬3
            /// </summary>
            [FieldExplan("地下迷宫火炬3")]
            DXMG_TorchLight3,
            /// <summary>
            /// 地下迷宫火炬4
            /// </summary>
            [FieldExplan("地下迷宫火炬4")]
            DXMG_TorchLight4,

        }

        /// <summary>
        /// 任务进度枚举
        /// </summary>
        public enum EnumTaskProgress
        {
            /// <summary>
            /// 未接取(任务还未接取)
            /// </summary>
            [FieldExplan("未接取")]
            NoTake,
            /// <summary>
            /// 失败(一般用于互斥任务)
            /// </summary>
            [FieldExplan("失败")]
            Failed,
            /// <summary>
            /// 已成功(任务完成后)
            /// </summary>
            [FieldExplan("已成功")]
            Sucessed,
            /// <summary>
            /// 已开始(任务正在执行)
            /// </summary>
            [FieldExplan("已开始")]
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

        /// <summary>
        /// 任务事件类型
        /// 当任务处于某种状态时,可能会导致的某种事件
        /// </summary>
        public enum EnumTaskEventType
        {
            /// <summary>
            /// 空
            /// </summary>
            [FieldExplan("空")]
            [TargetTypeExplan(TargetTypeExplanAttribute.EnumTargetType.Null)]
            None,
            /// <summary>
            /// 大地图功能是否可以使用
            /// </summary>
            [FieldExplan("大地图功能是否可以使用")]
            [TargetTypeExplan(TargetTypeExplanAttribute.EnumTargetType.Bool)]
            CanBigMap,
            /// <summary>
            /// 触发状态
            /// </summary>
            [FieldExplan("触发状态")]
            [TargetTypeExplan(TargetTypeExplanAttribute.EnumTargetType.Int)]
            Trigger,
        }
    }
}
