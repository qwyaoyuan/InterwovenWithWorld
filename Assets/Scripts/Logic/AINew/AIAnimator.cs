﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AINew
{
    /// <summary>
    /// AI的动画
    /// </summary>
    public abstract class AIAnimator : IAIInterface
    {
        /// <summary>
        /// AI的状态
        /// </summary>
        protected IAIState AIState;

        public virtual void Init(IAIState aiState)
        {
            this.AIState = aiState;
        }

        public abstract void Update();
    }
}
