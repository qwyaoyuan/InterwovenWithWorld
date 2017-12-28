using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions
{
    [Name("检测是否必须防御")]
    [Category("MyTools")]
    public class CheckMustDefense : ConditionTask
    {
        /// <summary>
        /// 目标造成的伤害
        /// </summary>
        public BBParameter<float> targetHurt;
        /// <summary>
        /// 预计承受的伤害
        /// </summary>
        public BBParameter<float> minHurt;
        /// <summary>
        /// 浮动值
        /// </summary>
        public BBParameter<float> floating = 10;

        protected override string info
        {
            get
            {
                return "承受" + targetHurt + "攻击,如大于" + minHurt + "则防御(在" + (-floating.value) + "和" + floating.value + ")间浮动";
            }
        }

        protected override bool OnCheck()
        {
            float tempFloat = Random.Range(-floating.value, floating.value);
            tempFloat += minHurt.value;
            if (targetHurt.value > tempFloat)
                return true;
            return false;
        }
    }
}
