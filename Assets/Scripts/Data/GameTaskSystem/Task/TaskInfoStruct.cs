using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaskMap
{
    /// <summary>
    /// 任务结构
    /// </summary>
    public class TaskInfoStruct
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [JsonProperty]
        public int ID { get; private set; }
        /// <summary>
        /// 任务标题
        /// </summary>
        public string TaskTitile { get; set; }
        /// <summary>
        /// 任务说明
        /// </summary>
        public string TaskExplain { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public Enums.EnumTaskType TaskType { get; set; }

        #region 任务接取限制
        /// <summary>
        /// 等级限制,>=此等级可以开始任务
        /// </summary>
        public int LevelLimit { get; set; }
        /// <summary>
        /// 性格倾向
        /// </summary>
        public Enums.CharacterTendency ChaTendency { get; set; }
        /// <summary>
        /// 接取任务的npc的id(如果这里有值则使用接取任务地点的场景,如果这里没有值(小于0)则到达接取任务地点后直接接取)
        /// </summary>
        public int ReceiveTaskNpcId { get; set; }
        /// <summary>
        /// 接取任务地点
        /// </summary>
        public Enums.TaskLocation ReceiveTaskLocation { get; set; }
        /// <summary>
        /// 直接接取任务是否需要显示对话栏,如果不展示则直接接取
        /// </summary>
        public bool NeedShowTalk { get; set; }
        /// <summary>
        /// 直接接取任务是否需要显示图片提示栏,如果展示则直接接取
        /// </summary>
        public bool NeedShowImageTip { get; set; }
        /// <summary>
        /// 如果要显示图片提示栏,则显示的图片提示栏的文件夹名
        /// </summary>
        public string ShowImageTipDirectoryName { get; set; }
        /// <summary>
        /// 需要的声望
        /// </summary>
        public int NeedReputation { get; set; }
        #endregion

        #region 奖励
        /// <summary>
        /// 奖励物品
        /// </summary>
        public Dictionary<EnumGoodsType, int> AwardGoods { get; set; }
        /// <summary>
        /// 奖励经验
        /// </summary>
        public int AwardExperience { get; set; }
        /// <summary>
        /// 奖励技能点
        /// </summary>
        public int AwardSkillPoint { get; set; }
        /// <summary>
        /// 奖励的声望
        /// </summary>
        public int AwardReputation { get; set; }
        #endregion

        #region 任务交付条件
        /// <summary>
        /// 交付任务的npc的id(如果这里有值则使用交付任务地点的场景,如果这里没有值(小于0)则到达交付任务地点后直接交付)
        /// </summary>
        public int DeliveryTaskNpcId { get; set; }
        /// <summary>
        /// 交付任务地点
        /// </summary>
        public Enums.TaskLocation DeliveryTaskLocation { get; set; }
        #endregion

        #region 其他判断任务完成条件
        /// <summary>
        /// 需要杀死的怪物数量
        /// </summary>
        public Dictionary<EnumMonsterType, int> NeedKillMonsterCount { get; set; }
        /// <summary>
        /// 需要获取物品的数量
        /// </summary>
        public Dictionary<EnumGoodsType, int> NeedGetGoodsCount { get; set; }
        /// <summary>
        /// 特殊状态检测
        /// </summary>
        public  Enums.EnumTaskSpecialCheck NeedSpecialCheck;
        /// <summary>
        /// 达到足够的声望
        /// </summary>
        public int NeedGetReputation { get; set; }
        /// <summary>
        /// 经过指定的游戏时间
        /// </summary>
        public int TimeLimit { get; set; }

        #endregion

        #region 编辑器所用数据
        /// <summary>
        /// 节点在编辑器中的位置
        /// </summary>
        public float X_Editor;
        /// <summary>
        /// 节点在编辑器中的位置
        /// </summary>
        public float Y_Editor;
        #endregion

        #region 游戏中所用的数据
        /// <summary>
        /// 游戏中达到指定时间(秒)
        /// </summary>
        public int GameTimeLimit { get; set; }
        /// <summary>
        /// 游戏中杀死的怪物数量
        /// </summary>
        public Dictionary<EnumMonsterType, int> GameKillMonsterCount { get; set; }
        /// <summary>
        /// 任务进度
        /// </summary>
        [JsonProperty]
        internal Enums.EnumTaskProgress TaskProgress { get; set; }
        /// <summary>
        /// 游戏中获得的物品数量
        /// </summary>
        public Dictionary<EnumGoodsType, int> GameGetGoodsCount { get; set; }
        #endregion

        #region 任务状态触发的事件
        /// <summary>
        /// 任务时间触发字典
        /// </summary>
        public Dictionary<Enums.EnumTaskProgress, List<TaskEventData>> TaskEventTriggerDic;
        #endregion

        public TaskInfoStruct()
        { }

        public TaskInfoStruct(int id)
        {
            this.ID = id;
        }

    }

    /// <summary>
    /// 任务事件数据
    /// </summary>
    public class TaskEventData
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public Enums.EnumTaskEventType EventType;
        /// <summary>
        /// 事件数据
        /// </summary>
        public object EventData;

        /// <summary>
        /// 返回深拷贝
        /// </summary>
        /// <returns></returns>
        public TaskEventData Clone()
        {
            return new TaskEventData() { EventType = EventType, EventData = EventData };
        }
    }

}
