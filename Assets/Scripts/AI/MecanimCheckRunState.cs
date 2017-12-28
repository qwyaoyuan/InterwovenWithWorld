using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{
    /// <summary>
    /// 检测动画是否在指定的状态内
    /// </summary>
    [Name("Check Mecanim Runing State")]
    [Category("Animator")]
    [Description("你可以用它来检测动画是否处于该状态")]
    public class MecanimCheckRunState : ConditionTask<Animator>
    {
        /// <summary>
        /// 动画层
        /// </summary>
        public BBParameter<int> layerIndex;

        /// <summary>
        /// 动画状态名
        /// </summary>
        public BBParameter<string> mecanimState;


        protected override string info
        {
            get
            {
                return string.Format("Mec.CheckState int {0} is {1} running", layerIndex.value, string.IsNullOrEmpty(mecanimState.value) ? "" : mecanimState.ToString());
            }
        }

        protected override bool OnCheck()
        {
            if (string.IsNullOrEmpty(mecanimState.value) || agent == null)
                return false;
            return agent.GetCurrentAnimatorStateInfo(layerIndex.value).IsName(mecanimState.value);
        }

    }
}
