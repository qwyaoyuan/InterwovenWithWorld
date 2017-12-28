using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{

    [Name("Wait Time")]
    [Category("MyTools")]
    public class WaitTimeTask : ActionTask
    {
        /// <summary>
        /// 等待的时间
        /// </summary>
        public BBParameter<float> waitTime = 1;

        /// <summary>
        /// 当前时间
        /// </summary>
        float nowTime;

        protected override string info
        {
            get { return "等待时间: " + waitTime.ToString(); }
        }

        protected override void OnExecute()
        {
            nowTime = 0;
        }

        protected override void OnUpdate()
        {
            nowTime += Time.deltaTime;
            Go();
        }

        void Go()
        {
            if (nowTime > waitTime.value)
            {
                EndAction(true);
            }
        }

        protected override void OnStop()
        {
            nowTime = 0;
        }

        protected override void OnPause()
        {
            OnStop();
        }
    }
}
