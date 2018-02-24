using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AINew
{
    /// <summary>
    /// AI的基础逻辑
    /// </summary>
    public class AIBaseLogic : AILogic
    {
        public override void Init(IAIState aiState)
        {
            base.Init(aiState);
            if (aiState.MonsterData != null)
            {
                switch (aiState.MonsterData.AIType)//根据怪物的状态初始化
                {
                    case EnumMonsterAIType.Trigger://直接就是追踪状态
                        aiState.SetState(EnumAILogicState.ZJ, EnumAIActionState.Wait, null);
                        break;
                    case EnumMonsterAIType.GoOnPatrol://先是巡逻状态
                        aiState.SetState(EnumAILogicState.XL, EnumAIActionState.Wait, null);
                        break;
                    case EnumMonsterAIType.Boss://固定型
                        aiState.SetState(EnumAILogicState.GD, EnumAIActionState.Wait, null);
                        break;
                }
            }
        }

        public override void Update()
        {
            if (!AIState.NowAIState)
                return;

            switch (AIState.AILogicState)
            {
                case EnumAILogicState.ZJ:
                    Logic_ZJ();
                    break;
                case EnumAILogicState.XL:
                    Logic_XL();
                    break;
                case EnumAILogicState.GD:
                    Logic_GD();
                    break;
            }
        }

        /// <summary>
        /// 追踪逻辑
        /// </summary>
        protected virtual void Logic_ZJ()
        {
            if (AIState.Target == null)
            {
                AIState.SetState(EnumAILogicState.XL, EnumAIActionState.Wait, null);//切换为巡逻逻辑
                return;
            }
        }

        /// <summary>
        /// 巡逻逻辑
        /// </summary>
        protected virtual void Logic_XL()
        {
            if(AIState.Target == null)
            {
                //继续巡逻,且开始查找对象 
            }
        }

        /// <summary>
        /// 固定逻辑
        /// </summary>
        protected virtual void Logic_GD() { }
    }
}
