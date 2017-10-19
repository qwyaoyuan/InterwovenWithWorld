using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DialogueDataFileEditor
{
    /// <summary>
    /// 展开收起控件
    /// </summary>
    public interface IOpenStop
    {
        /// <summary>
        /// 控件是否展开
        /// </summary>
        bool OpenStopState { get; }
        /// <summary>
        /// 展开控件
        /// </summary>
        /// <param name="location">控件位置</param>
        void OpenControl(System.Drawing.Point location);
        /// <summary>
        /// 收起控件 
        /// </summary>
        /// <param name="location">控件位置 </param>
        void StopControl(System.Drawing.Point location);

        /// <summary>
        /// 展开控件后的控件大小
        /// </summary>
        System.Drawing.Size OpenSize { get; }
        /// <summary>
        /// 收起控件后的控件大小
        /// </summary>
        System.Drawing.Size StopSize{ get; }

        /// <summary>
        /// 注册对象的展开收起发生变化事件
        /// </summary>
        /// <param name="OpenStopChanged"></param>
        void SetLiestenOpenStop(Action<IOpenStop, bool> OpenStopChanged);
    }
}
