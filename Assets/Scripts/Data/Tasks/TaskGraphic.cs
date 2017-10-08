using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Data.Tasks
{
    public class TaskGraphic
    {
       
    }


    /// <summary>
    /// 任务节点
    /// </summary>
    public class TaskNode
    {
        /// <summary>
        /// 任务id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 等级限制,>=此等级可以开始任务
        /// </summary>
        public int LevelLimit { get; set; }

        /// <summary>
        /// 性格倾向
        /// </summary>
        public CharacterTendency ChaTendency { get; set; }

    }

    /// <summary>
    /// 任务类型
    /// </summary>
    public enum TaskType
    {
        /// <summary>
        /// 主线任务
        /// </summary>
        PrincipalLine,

        /// <summary>
        /// 支线任务
        /// </summary>
        BranchLine,

        /// <summary>
        /// 重复性任务
        /// </summary>
        Repeat,

        /// <summary>
        /// 随机任务
        /// </summary>
        Random
    }

    /// <summary>
    /// 性格倾向
    /// </summary>
    public enum CharacterTendency
    {

        None,
        /// <summary>
        /// 好杀戮的
        /// </summary>
        Slaughterous,

        /// <summary>
        /// 和平的
        /// </summary>
        Peaceable,
    }


    /// <summary>
    /// 角色种族枚举
    /// </summary>
    public enum RoleOfRace
    {
        

    }
}
