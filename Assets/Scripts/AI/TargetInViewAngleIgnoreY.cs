using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
    [Name("Target In View Angle")]
    [Category("MyTools")]
    [Description("Checks whether the target is in the view angle of the agent")]
    public class TargetInViewAngleIgnoreY : ConditionTask<Transform>
    {
        [RequiredField]
        public BBParameter<GameObject> checkTarget;
        [SliderField(1, 180)]
        public BBParameter<float> viewAngle = 70f;
        /// <summary>
        /// 是否忽略Y轴
        /// </summary>
        public bool ignoreY;

        protected override string info
        {
            get { return checkTarget + " in view angle"; }
        }

        protected override bool OnCheck()
        {
            Vector3 targetVec = checkTarget.value.transform.position;
            Vector3 thisVec = agent.position;
            Vector3 thisForward = agent.forward;
            if (ignoreY)
            {
                targetVec.y = 0;
                thisVec.y = 0;
                thisForward.y = 0;
            }
            return Vector3.Angle(targetVec - thisVec, thisForward) < viewAngle.value;
        }

        public override void OnDrawGizmosSelected()
        {
            if (agent != null)
            {
                Gizmos.matrix = Matrix4x4.TRS(agent.position, agent.rotation, Vector3.one);
                Gizmos.DrawFrustum(Vector3.zero, viewAngle.value, 5, 0, 1f);
            }
        }
    }
}
