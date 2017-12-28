using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [Category("✫ Blackboard")]
    [Description("Set a blackboard gameObject variable")]
    public class SetGameObject : ActionTask
    {
        /// <summary>
        /// 目标
        /// </summary>
        public BBParameter<GameObject> target;

        [BlackboardOnly]
        public BBParameter<GameObject> objVariable;

        protected override string info
        {
            get
            {
                return "Set " + target.ToString()  + " to " + objVariable.ToString();
            }
        }

        protected override void OnExecute()
        {
            objVariable.value = target.value;
            EndAction();
        }
    }
}
