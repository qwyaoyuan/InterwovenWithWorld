using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    /// <summary>
    /// 初始化AI攻击结构
    /// </summary>
    [Name("初始化")]
    [Category("MyTools/AttackAI")]
    public class InitAttackAIDataStruct : ActionTask
    {
        /// <summary>
        /// 初始化的目标
        /// </summary>
        public BBParameter<AttackAIDataStruct> target;

        /// <summary>
        /// 用于设置的攻击角度
        /// </summary>
        public BBParameter<float> setAttackAngle;

        protected override string info
        {
            get
            {
                return "对" + target.ToString() + "进行初始化\r\n"+
                    "设置攻击角度到"+setAttackAngle.ToString();
            }
        }

        protected override void OnExecute()
        {
            AttackAIDataStruct tempData = target.value;
            tempData.TempCoolTime = tempData.CoolTime;
            tempData.TempMaxWaitTime = tempData.MaxWaitTime;
            target.value = tempData;
            EndAction();
        }

    }
}
