using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Enums
{

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
    /// 任务进度枚举
    /// </summary>
    public enum EnumTaskProgress
    {
        /// <summary>
        /// 失败
        /// </summary>
        Failed,

        /// <summary>
        /// 已成功
        /// </summary>
        Sucessed,

        /// <summary>
        /// 已开始
        /// </summary>
        Started,


        /// <summary>
        /// 未接取
        /// </summary>
        NoTake,



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

        /// <summary>
        /// 到达的中心位置
        /// </summary>
        public Vector3 ArrivedCenterPos { get; set; }


        /// <summary>
        /// 半径
        /// </summary>
        public int Radius { get; set; }
    }
}

