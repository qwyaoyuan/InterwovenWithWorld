using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
    /// <summary>
    /// 判断攻击距离
    /// </summary>
    [Name("判断攻击距离和阻碍物")]
    [Category("MyTools/AttackAI")]
    public class CheckAttackDistanceAndHinder : ConditionTask<Transform>
    {
        /// <summary>
        /// 攻击目标
        /// </summary>
        public BBParameter<GameObject> target;

        /// <summary>
        /// 用于判断的数据
        /// </summary>
        public BBParameter<AttackAIDataStruct> attackData;

        /// <summary>
        /// 偏差值
        /// </summary>
        [SliderField(-5, 5f)]
        public float differenceThreshold = 1f;

        /// <summary>
        /// 判断阻碍的偏差值
        /// 在中心点和中心点依据该偏差值上下进行三条射线检测,如果检测到障碍则无法攻击
        /// </summary>
        public BBParameter<float> hinderOffset;
        /// <summary>
        /// 射线检测的阻碍物的层
        /// </summary>
        public LayerMask rayCastMask;

        protected override string info
        {
            get
            {
                return "判断与" + target.ToString() + "的距离足够攻击\r\n" +
                    "在" + hinderOffset.ToString() + "的偏差下没有全部碰撞到障碍层";
            }
        }

        protected override bool OnCheck()
        {
            //计算距离
            float distance = Vector3.Distance(agent.position, target.value.transform.position);
            bool distanceResult = distance + differenceThreshold < attackData.value.AttackDistance;
            //计算射线
            Vector3 firstRayForward = (target.value.transform.position - agent.position).normalized;
            float firstRayDistance = Vector3.Distance(target.value.transform.position, agent.position);
            bool firstRayTest = RayTest(agent.position, firstRayForward, firstRayDistance);

            Vector3 secondRayForward = (target.value.transform.position - (agent.position - Vector3.up * hinderOffset.value)).normalized;
            float secondRayDistance = Vector3.Distance(target.value.transform.position, (agent.position - Vector3.up * hinderOffset.value));
            bool secondRayTest = RayTest(agent.position - Vector3.up * hinderOffset.value, secondRayForward, secondRayDistance);

            Vector3 thirdRayForward = (target.value.transform.position - (agent.position + Vector3.up * hinderOffset.value)).normalized;
            float thirdRayDistance = Vector3.Distance(target.value.transform.position, (agent.position + Vector3.up * hinderOffset.value));
            bool thirdRayTest = RayTest(agent.position + Vector3.up * hinderOffset.value, thirdRayForward, thirdRayDistance);

            bool rayResult = !firstRayTest || !secondRayTest || !thirdRayTest;
            return rayResult && distanceResult;
        }

        /// <summary>
        /// 射线检测
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="forward"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private bool RayTest(Vector3 startPoint, Vector3 forward, float distance)
        {
            Ray ray = new Ray(startPoint, forward);
            return Physics.RaycastAll(ray, distance, rayCastMask)
                .Count(temp=>
                    !GameObject.Equals(temp.transform.gameObject,agent.gameObject)
                    && !GameObject.Equals(temp.transform.gameObject, target.value)) >0;
        }
    }
}