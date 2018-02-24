using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AINew
{

    /// <summary>
    /// AI的状态,所有的行动逻辑和动画都是基于当前的AI状态的
    /// </summary>
    public class AIState : MonoBehaviour, IAIState
    {
        /// <summary>
        /// 当前的AI状态(开关)
        /// </summary>
        public bool NowAIState { get; set; }

        /// <summary>
        /// 当前的逻辑状态
        /// </summary>
        public EnumAILogicState AILogicState {  get;private set; }
        /// <summary>
        /// 当前的行动状态
        /// </summary>
        public EnumAIActionState AIActionState { get;private set; }
        /// <summary>
        /// 当前行动所附加的数据
        /// </summary>
        public object AIActionData { get; private set; }
        /// <summary>
        /// 自身
        /// </summary>
        public GameObject Self { get; private set; }
        /// <summary>
        /// 目标
        /// </summary>
        public GameObject Target { get; private set; }
        /// <summary>
        /// 怪物的数据
        /// </summary>
        public MonsterDataInfo MonsterData { get;private set; }

        public MonsterDataInfo _TestMonsterData;

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="aiLogicState"></param>
        /// <param name="aiActionState"></param>
        /// <param name="aiActionData"></param>
        public void SetState(EnumAILogicState aiLogicState, EnumAIActionState aiActionState, object aiActionData)
        {
            this.AILogicState = aiLogicState;
            this.AIActionState = aiActionState;
            this.AIActionData = aiActionData;
        }

        /// <summary>
        /// AI行动方式处理类的名字
        /// </summary>
        public string AIActionClass;
        /// <summary>
        /// AI行动方式类
        /// </summary>
        AIAction AIAction;

        /// <summary>
        /// AI逻辑类的名字
        /// </summary>
        public string AILogicClass;
        /// <summary>
        /// AI逻辑类
        /// </summary>
        AILogic AILogic;

        /// <summary>
        /// AI动画类的名字
        /// </summary>
        public string AIAnimatorClass;
        /// <summary>
        /// AI动画类
        /// </summary>
        AIAnimator AIAnimator;

        private void Start()
        {
            Self = gameObject;
            MonsterData = _TestMonsterData;
            try
            {
                AIAction = (AIAction)Activator.CreateInstance(Type.GetType(AIActionClass));
                AIAction.Init(this);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            try
            {
                AILogic = (AILogic)Activator.CreateInstance(Type.GetType(AILogicClass));
                AILogic.Init(this);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            try
            {
                AIAnimator = (AIAnimator)Activator.CreateInstance(Type.GetType(AIAnimatorClass));
                AIAnimator.Init(this);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        private void Update()
        {
            if (AILogic != null)
            {
                AILogic.Update();
            }
            if (AIAction != null)
            {
                AILogic.Update();
            }
            if (AIAnimator != null)
            {
                AILogic.Update();
            }
        }
    }

    /// <summary>
    /// AI的逻辑状态
    /// </summary>
    public enum EnumAILogicState
    {
        /// <summary>
        /// 追踪
        /// </summary>
        ZJ,
        /// <summary>
        /// 巡逻
        /// </summary>
        XL,
        /// <summary>
        /// 固定(一般是boss)
        /// </summary>
        GD
    }

    /// <summary>
    /// AI的行动状态
    /// </summary>
    public enum EnumAIActionState
    {
        /// <summary>
        /// 等待
        /// </summary>
        Wait,
        /// <summary>
        /// 移动
        /// </summary>
        Move,
        /// <summary>
        /// 收到攻击
        /// </summary>
        GetHit,
        /// <summary>
        /// 攻击
        /// </summary>
        Hit,
        /// <summary>
        /// 眩晕
        /// </summary>
        Dizzy,
        /// <summary>
        /// 死亡
        /// </summary>
        Death,
    }
}


