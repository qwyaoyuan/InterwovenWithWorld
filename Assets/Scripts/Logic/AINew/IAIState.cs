using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AINew
{
    /// <summary>
    /// ai状态接口
    /// </summary>
    public interface IAIState
    {
        /// <summary>
        /// 当前的AI状态(开关)
        /// </summary>
        bool NowAIState { get; set; }
        /// <summary>
        /// 逻辑状态
        /// </summary>
        EnumAILogicState AILogicState { get; }
        /// <summary>
        /// 行动状态
        /// </summary>
        EnumAIActionState AIActionState { get;  }
        /// <summary>
        /// 当前行动所附加的数据
        /// </summary>
        object AIActionData { get;  }
        /// <summary>
        /// 自身
        /// </summary>
        GameObject Self { get; }
        /// <summary>
        /// 目标
        /// </summary>
        GameObject Target { get; }
        /// <summary>
        /// 怪物数据
        /// </summary>
        MonsterDataInfo MonsterData { get; }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="aiLogicState"></param>
        /// <param name="aiActionState"></param>
        /// <param name="aiActionData"></param>
        void SetState(EnumAILogicState aiLogicState, EnumAIActionState aiActionState, object aiActionData);
    }
}
