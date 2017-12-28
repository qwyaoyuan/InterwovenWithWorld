using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    /// <summary>
    /// 执行攻击逻辑
    /// </summary>
    [Name("执行")]
    [Category("MyTools/AttackAI")]
    public class ImplementationAttackAiDataStruct : ActionTask<Transform>
    {
        /// <summary>
        /// 初始化的数据
        /// </summary>
        public BBParameter<AttackAIDataStruct> target;
        /// <summary>
        /// 消息名
        /// </summary>
        public BBParameter<string> messageName;

        /// <summary>
        /// 本次攻击后的等待时间 
        /// </summary>
        public BBParameter<float> thisAttackNextWaitTime;

        /// <summary>
        /// 等待时间
        /// </summary>
        float waitTime;

        protected override string info
        {
            get
            {
                return "将" + target.ToString() + "发送给" + messageName.ToString() + "函数";
            }
        }

        protected override void OnExecute()
        {
            if (!string.IsNullOrEmpty(messageName.value))
            {
                try
                {
                    agent.SendMessage(messageName.value, target.value);
                }
                catch
                {
                    Debug.Log("没有该攻击函数:" + messageName.value);
                }
            }
            waitTime = target.value.AttackDurationTime;
        }

        protected override void OnUpdate()
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                EndAction();
            }
        }

        protected override void OnStop()
        {
            waitTime = 0;
            thisAttackNextWaitTime.value = target.value.ThisAttackNextWaitTime;//设置下次选择技能的等待时间
        }

        protected override void OnPause()
        {
            OnStop();
        }
    }
}
