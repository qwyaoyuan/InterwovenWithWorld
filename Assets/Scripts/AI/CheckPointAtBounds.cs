using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using System;

namespace NodeCanvas.Tasks.Conditions
{
    /// <summary>
    /// 检测是否在区域内
    /// </summary>
    [Name("检测是否在区域内部或外部")]
    [Category("MyTools")]
    public class CheckPointAtBounds : ConditionTask<Transform>
    {
        /// <summary>
        /// 位置关系
        /// </summary>
        public enum PositionRelationship
        {
            /// <summary>
            /// 内部
            /// </summary>
            Inner,
            /// <summary>
            /// 外部
            /// </summary>
            Outside

        }

        /// <summary>
        /// 区域范围
        /// </summary>
        public BBParameter<Bounds> bounds;
        /// <summary>
        /// 内部还是外部
        /// </summary>
        public PositionRelationship checkType = PositionRelationship.Inner;

        protected override string info
        {
            get
            {
                return "对象是否在" + bounds.ToString() + (checkType == PositionRelationship.Inner ? "内部" : "外部");
            }
        }

        protected override bool OnCheck()
        {
            bool isInner = bounds.value.Contains(agent.position);
            return checkType == PositionRelationship.Inner ? isInner : !isInner;
        }
    }
}
