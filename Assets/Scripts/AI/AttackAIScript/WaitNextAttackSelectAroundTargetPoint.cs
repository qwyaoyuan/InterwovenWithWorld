using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions
{
    /// <summary>
    /// 在等待下一次攻击时,选择一个绕着目标移动的点
    /// </summary>
    [Name("在等待下一次攻击时,选择一个绕着目标移动的点")]
    [Category("MyTools/AttackAI")]
    public class WaitNextAttackSelectAroundTargetPoint : ActionTask<Transform>
    {
        /// <summary>
        /// 目标
        /// </summary>
        public BBParameter<GameObject> target;
        /// <summary>
        /// 寻路组件
        /// </summary>
        public BBParameter<NavMeshAgent> navMeshAgent;
        /// <summary>
        /// 寻找点的距离
        /// </summary>
        public BBParameter<float> findPointDistance;
        /// <summary>
        /// 寻找到的点
        /// </summary>
        public BBParameter<Vector3> nextPoint;
        /// <summary>
        /// 是否忽略Y轴
        /// </summary>
        public bool ignoreY;

        protected override string info
        {
            get
            {
                return "在等待下一次攻击时\r\n选择一个绕着" + target.ToString() + "移动的点"
                    + "\r\n将点设置到" + nextPoint.ToString() + "中"
                    + "\r\n如果自身不存在寻路组件则使用" + navMeshAgent.ToString();
            }
        }

        protected override void OnExecute()
        {
            NavMeshAgent navMeshAgent = agent.GetComponent<NavMeshAgent>();
            if (navMeshAgent == null)
                navMeshAgent = this.navMeshAgent.value;
            else
                this.navMeshAgent.value = navMeshAgent;
            if (navMeshAgent)
            {
                Vector3 getPoint = GetRandomPoint();
                NavMeshPath navMeshPath = new NavMeshPath();
                if (navMeshAgent.CalculatePath(getPoint, navMeshPath))
                {
                    if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                    {
                        nextPoint.value = getPoint;
                        EndAction();
                        return;
                    }
                }
            }
            nextPoint.value = agent.position;
            EndAction();
        }

        /// <summary>
        /// 获取一个随机点
        /// </summary>
        /// <returns></returns>
        private Vector3 GetRandomPoint()
        {
            Vector3 targetPos = target.value.transform.position;
            Vector3 thisPos = agent.position;
            if (ignoreY)
            {
                targetPos.y = 0;
                thisPos.y = 0;
            }
            float nowDistance = Vector3.Distance(targetPos, thisPos);
            for (int i = 0; i < 10; i++)
            {
                float x = Random.Range(agent.position.x + findPointDistance.value, agent.position.x - findPointDistance.value);
                float z = Random.Range(agent.position.z + findPointDistance.value, agent.position.z - findPointDistance.value);
                Vector3 selectPos = new Vector3(x, 0, z);
                if (!ignoreY)
                {
                    selectPos.y = (target.value.transform.position.y + agent.position.y) / 2;
                }
                float selectDistance = Vector3.Distance(targetPos, selectPos);
                if (selectDistance + findPointDistance.value / 4 < nowDistance)
                    continue;
                else
                {
                    selectPos.y = (target.value.transform.position.y + agent.position.y) / 2;
                    //射线
                    Ray ray = new Ray(selectPos + Vector3.up * 500, Vector3.down);
                    RaycastHit rch = Physics.RaycastAll(ray).Where(temp => string.Equals(temp.transform.tag, "Terrain")).FirstOrDefault();
                    if (rch.collider != null)
                        return rch.point;
                    else
                        continue;
                }
            }
            return agent.position;
        }
    }
}
