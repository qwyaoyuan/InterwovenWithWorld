using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoodsEditor
{
    public partial class GoodsEnumNatrues : UserControl
    {

        public event Action<GoodsEnumNatrues> Deleted;

        public GoodsEnumNatrues()
        {
            InitializeComponent();
            Init();
        }






        /// <summary>
        /// 初始化读取下拉菜单内容
        /// </summary>
        private void Init()
        {
            foreach (EnumGoodsAbility suit in Enum.GetValues(typeof(EnumGoodsAbility)))
            {
                comboBox1.Items.Add((int)suit + "~" + suit + "~" + suit.Display());
            }
        }

        /// <summary>
        /// 物品能力
        /// </summary>
        public GoodsAbility GoodsAbility
        {

            get
            {

                try
                {
                    var natrueType = (EnumGoodsAbility)int.Parse(comboBox1.Text.Split('~')[0]);
                    var number = int.Parse(this.textBox1.Text);
                    return new GoodsAbility(natrueType, number, natrueType.Display());
                }

                catch
                {
                    return new GoodsAbility(EnumGoodsAbility.None, 0, string.Empty);
                }
            }
            set
            {
                int index = 0;
                foreach (EnumGoodsAbility suit in Enum.GetValues(typeof(EnumGoodsAbility)))
                {
                    if (value.AbilibityKind == suit)
                        break;
                    index++;
                }

                comboBox1.SelectedIndex = index;
                this.textBox1.Text = value.Value.ToString();
                this.textBox2.Text = value.Explain;

            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = GoodsDictionarys.Instance().GetGoodsNatrue(int.Parse(comboBox1.Text.Split('~')[0]));
        }

        #region  辅助方法

        #endregion


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Deleted != null)
            {
                Deleted(this);
            }
        }
    }

    /// <summary>
    /// 物品能力
    /// </summary>
    public class GoodsAbility
    {
        /// <summary>
        /// 能力种类
        /// </summary>
        public EnumGoodsAbility AbilibityKind { get; set; }

        /// <summary>
        /// 能力值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Explain { get; set; }


        public GoodsAbility()
        {

        }

        public GoodsAbility(EnumGoodsAbility abilityKind, int val, string explain)
        {
            this.AbilibityKind = abilityKind;
            this.Value = val;
            this.Explain = explain;
        }
    }
}
