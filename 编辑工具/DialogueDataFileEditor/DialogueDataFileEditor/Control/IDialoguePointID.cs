using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DialogueDataFileEditor
{
    /// <summary>
    /// 节点ID接口
    /// </summary>
    public interface IDialoguePointID
    {
        /// <summary>
        /// 添加一个子节点
        /// </summary>
        /// <param name="child">子节点对象</param>
        /// <returns></returns>
        void AddNextPointID(IDialoguePointID child);

        /// <summary>
        /// 移除一个子节点
        /// </summary>
        /// <param name="child">子节点对象</param>
        /// <returns></returns>
        void RemovNextPointID(IDialoguePointID child);

        /// <summary>
        /// 获取该节点的子节点数组
        /// </summary>
        IDialoguePointID[] GetDialogueNextPointID { get; }
    }
}
