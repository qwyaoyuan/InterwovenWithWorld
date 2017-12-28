using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions
{
    [Name("检测两个点之间的距离")]
    [Category("MyTools")]
    public class Check2Vector3Dis : ConditionTask
    {
        public BBParameter<Vector3> first;
        public CompareMethod checkType = CompareMethod.LessThan;
        public BBParameter<Vector3> second;

        public BBParameter<float> distance = 10;

        public bool ignoreX;
        public bool ignoreY;
        public bool ignoreZ;

        [SliderField(0, 0.1f)]
        public float floatingPoint = 0.05f;

        protected override string info
        {
            get
            {
                return "Distance " + distance + " from " + first.ToString() + " to " + second.ToString();
            }
        }

        protected override bool OnCheck()
        {
            Vector3 _first = first.value;
            Vector3 _second = second.value;
            if (ignoreX)
            {
                _first.x = 0;
                _second.x = 0;
            }
            if (ignoreY)
            {
                _first.y = 0;
                _second.y = 0;
            }
            if (ignoreZ)
            {
                _first.z = 0;
                _second.z = 0;
            }
            return OperationTools.Compare(Vector3.Distance(_first, _second), distance.value, checkType, floatingPoint);
        }
    }
}
