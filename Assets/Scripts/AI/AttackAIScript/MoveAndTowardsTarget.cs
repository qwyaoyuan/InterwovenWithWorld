using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions
{
    /// <summary>
    /// 移动并且朝向目标
    /// </summary>
    [Name("移动并且朝向目标")]
    [Category("MyTools/AttackAI")]
    public class MoveAndTowardsTarget : ActionTask<Transform>
    {
        /// <summary>
        /// 目标
        /// </summary>
        public BBParameter<GameObject> target;
        /// <summary>
        /// 目标的朝向(0表示前 -1和1表示后  0.5表示右 -0.5表示左)
        /// </summary>
        public BBParameter<float> animatorForward;
        /// <summary>
        /// 寻路组件,如果Transform存在则不使用这个
        /// </summary>
        public BBParameter<NavMeshAgent> navMeshAgent;
        /// <summary>
        /// 要移动到的点
        /// </summary>
        public BBParameter<Vector3> moveToPoint;
        /// <summary>
        /// 速度
        /// </summary>
        public BBParameter<float> speed = 3;
        /// <summary>
        /// 保持距离
        /// </summary>
        public BBParameter<float> keepDistance = 0.1f;
        /// <summary>
        /// 移动点队列
        /// </summary>
        Queue<Vector3> moveToQueue;

        protected override string info
        {
            get
            {
                return "移动到" + moveToPoint.ToString()
                    + "\r\n朝向设置到" + animatorForward.ToString();
            }
        }

        protected override void OnExecute()
        {
            moveToQueue = new Queue<Vector3>();
            NavMeshAgent tempNavMeshAgent = agent.GetComponent<NavMeshAgent>();
            if (tempNavMeshAgent == null)
                tempNavMeshAgent = navMeshAgent.value;
            else
                navMeshAgent.value = tempNavMeshAgent;
            if (tempNavMeshAgent != null)
            {
                NavMeshPath navMeshPath = new NavMeshPath();
                if (tempNavMeshAgent.CalculatePath(moveToPoint.value, navMeshPath))
                {
                    if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                    {
                        if (navMeshPath.corners != null)
                            navMeshPath.corners.ToList().ForEach(temp => moveToQueue.Enqueue(temp));
                    }
                }
            }
            Move();
        }

        protected override void OnUpdate()
        {
            Move();
        }

        void Move()
        {
            if (moveToQueue.Count > 0)
            {
                Vector3 nowNextPoint = moveToQueue.Peek();
                float nowDistance = GetDistanceIgnoreY(nowNextPoint, agent.position);
                Vector3 moveForward = (GetIgnoreY(nowNextPoint) - GetIgnoreY(agent.position)).normalized;
                if (nowDistance > keepDistance.value)//继续移动
                {
                    float moveDis = speed.value * Time.deltaTime;
                    if (moveDis > nowDistance)
                    {
                        moveDis = nowDistance;
                    }

                    agent.position += moveForward * moveDis;
                }
                else //移除当前目标点
                {
                    moveToQueue.Dequeue();
                }
                //朝向目标
                Vector3 targetForward = (GetIgnoreY(target.value.transform.position) - GetIgnoreY(agent.position)).normalized;
                Quaternion lookAtRot = Quaternion.LookRotation(targetForward);
                agent.rotation = Quaternion.Lerp(agent.rotation, lookAtRot, 0.1f);
                //计算移动方向 
                Vector3 nowForward = GetIgnoreY(agent.forward);
                float angle = Vector3.Angle(nowForward, moveForward);
                Vector3 crossVec = Vector3.Cross(nowForward, moveForward);
                float forwardFloat = 0;
                if (crossVec.y >= 0)//移动方向在角色右侧
                {
                    forwardFloat = angle / 180;
                }
                else//移动方向在角色左侧
                {
                    forwardFloat = -angle / 180;
                }
                animatorForward.value = forwardFloat;
            }
            else
                EndAction();
        }

        /// <summary>
        /// 获取忽略Y轴的点
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        Vector3 GetIgnoreY(Vector3 target)
        {
            target.y = 0;
            return target;
        }

        /// <summary>
        /// 计算距离忽略Y轴
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        float GetDistanceIgnoreY(Vector3 first, Vector3 second)
        {
            first.y = 0;
            second.y = 0;
            return Vector3.Distance(first, second);
        }

        protected override void OnStop()
        {
            animatorForward.value = 0;
        }

        protected override void OnPause()
        {
            OnStop();
        }
    }
}
