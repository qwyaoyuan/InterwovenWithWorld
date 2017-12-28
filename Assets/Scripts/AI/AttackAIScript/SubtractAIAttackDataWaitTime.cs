using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    /// <summary>
    /// 持续减去攻击等待时间,直到该时间为0返回false
    /// </summary>
    [Name("持续减去攻击等待时间直到为0")]
    [Category("MyTools/AttackAI")]
    public class SubtractAIAttackDataWaitTime : ActionTask
    {
        /// <summary>
        /// 攻击数据
        /// </summary>
        public BBParameter<AttackAIDataStruct> attackData;

        /// <summary>
        /// 返回状态
        /// </summary>
        public bool returnState = false;

        protected override string info
        {
            get
            {
                return "直到攻击等待时间为0,返回" + returnState;
            }
        }

        protected override void OnUpdate()
        {
            AttackAIDataStruct tempData = attackData.value;
            tempData.TempMaxWaitTime -= Time.deltaTime;
            attackData.value = tempData;
            if (tempData.TempMaxWaitTime <= 0)
                EndAction(false);
        }

    }
}
