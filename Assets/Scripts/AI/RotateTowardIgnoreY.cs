using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [Category("MyTools")]
    public class RotateTowardIgnoreY : ActionTask<Transform>
    {
        [RequiredField]
        public BBParameter<GameObject> target;
        public BBParameter<float> speed = 2;
        [SliderField(1, 180)]
        public BBParameter<float> angleDifference = 5;
        public bool repeat;

        /// <summary>
        /// 是否忽略Y轴
        /// </summary>
        public bool ignoreY;

        protected override void OnExecute() { Rotate(); }
        protected override void OnUpdate() { Rotate(); }

        void Rotate()
        {
            Vector3 targetVec = target.value.transform.position;
            Vector3 thisVec = agent.position;
            Vector3 thisForward = agent.forward;
            if (ignoreY)
            {
                targetVec.y = 0;
                thisVec.y = 0;
                thisForward.y = 0;
            }
            if (Vector3.Angle(targetVec - thisVec, thisForward) > angleDifference.value)
            {
                var dir = targetVec - thisVec;
                agent.rotation = Quaternion.LookRotation(Vector3.RotateTowards(agent.forward, dir, speed.value * Time.deltaTime, 0));
            }
            else
            {
                if (!repeat)
                    EndAction();
            }
        }

    }
}
