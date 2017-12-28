using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
    [Name("检测对象到目标点的距离")]
    [Category("MyTools")]
    public class CheckDistanceToPoint: ConditionTask<Transform>
    {
        public BBParameter<Vector3> point;
        public CompareMethod checkType = CompareMethod.LessThan;
        public BBParameter<float> distance = 10;
        [SliderField(0, 0.1f)]
        public float floatingPoint = 0.05f;

        protected override string info
        {
            get { return "Distance " +  distance + " to " + point.ToString(); }
        }

        protected override bool OnCheck()
        {
            return OperationTools.Compare(Vector3.Distance(agent.position, point.value), distance.value, checkType, floatingPoint);
        }

        public override void OnDrawGizmosSelected()
        {
            if (agent != null)
            {
                Gizmos.DrawWireSphere(agent.position, distance.value);
            }
        }
    }
}
