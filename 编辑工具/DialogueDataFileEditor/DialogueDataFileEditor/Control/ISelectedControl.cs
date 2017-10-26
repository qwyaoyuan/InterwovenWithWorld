using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DialogueDataFileEditor
{
    /// <summary>
    /// 选中控件
    /// </summary>
    public interface ISelectedControl
    {
        /// <summary>
        /// 注册监听控件被选中方法
        /// </summary>
        /// <param name="ListenSelectedControl">委托函数</param>
        void SetListenControlSelected(Action<ISelectedControl> ListenSelectedControl);

        /// <summary>
        /// 取消选中控件
        /// </summary>
        void NoSelectedControl();

        /// <summary>
        /// 选中控件
        /// </summary>
        void SelectedControl();

        /// <summary>
        /// 获取父控件
        /// </summary>
        /// <returns></returns>
        System.Windows.Forms.Control GetParent();
    }
}
