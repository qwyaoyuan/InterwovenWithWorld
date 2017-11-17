using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoodsEditor
{
    public partial class GoodsEditor : Form
    {
        public event Action<int, List<GoodsAbility>> GoodsAbilitysChanged;

        List<GoodsAbility> GoodsAbilitys = new List<GoodsAbility>();

        public int ID { get; set; }

        public GoodsEditor(int ID)
        {
            InitializeComponent();

            this.FormClosed += GoodsEditor_FormClosed;
            this.ID = ID;
            if (GoodsControl.allGoodsAbilitys.ContainsKey(ID))
            {
                this.GoodsAbilitys = GoodsControl.allGoodsAbilitys[ID];
            }

            foreach (var item in GoodsAbilitys)
            {
                AddAbilityAuto(item);
            }
        }



        private void GoodsEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            //待修改ZXW
            //序列化ID对应的物品信息，并保存到对应的文件中
            GoodsAbilitys.Clear();
            foreach (var item in EnumNatures)
            {
                if (item.GoodsAbility.AbilibityKind != EnumGoodsAbility.None)
                {
                    GoodsAbilitys.Add(item.GoodsAbility);
                }
            }
            if (GoodsAbilitysChanged != null)
            {
                GoodsAbilitysChanged(this.ID, GoodsAbilitys);
            }

        }


        /// <summary>
        /// 自定义控件存储数组
        /// </summary>
        private List<GoodsEnumNatrues> EnumNatures = new List<GoodsEnumNatrues>();





        /// <summary>
        /// 自动添加
        /// </summary>
        /// <param name="goodsAbility"></param>

        private void AddAbilityAuto(GoodsAbility goodsAbility)
        {
            var goodsEnumNatrues = new GoodsEnumNatrues();
            goodsEnumNatrues.GoodsAbility = goodsAbility;
            goodsEnumNatrues.Deleted += GoodsEnumNatrues_Deleted;
            this.flowLayoutPanel1.Controls.Add(goodsEnumNatrues);
            this.flowLayoutPanel1.Height += goodsEnumNatrues.Height + 5;
            EnumNatures.Add(goodsEnumNatrues);
            RefreshScrollBar();

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="obj"></param>
        private void GoodsEnumNatrues_Deleted(GoodsEnumNatrues obj)
        {
            EnumNatures.Remove(obj);
            this.flowLayoutPanel1.Controls.Remove(obj);
        }


        private void 添加属性ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var goodsEnumNatrues = new GoodsEnumNatrues();
            goodsEnumNatrues.Deleted += GoodsEnumNatrues_Deleted;
            this.flowLayoutPanel1.Controls.Add(goodsEnumNatrues);
            this.flowLayoutPanel1.Height += goodsEnumNatrues.Height + 5;
            EnumNatures.Add(goodsEnumNatrues);
            RefreshScrollBar();
        }





        private void RefreshScrollBar()
        {
            this.vScrollBar1.Maximum = this.flowLayoutPanel1.Height - this.panel1.Height > 0 ? this.flowLayoutPanel1.Height - this.panel1.Height : 0;

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

            this.flowLayoutPanel1.Location = new Point(this.flowLayoutPanel1.Location.X, -this.vScrollBar1.Value);
        }
    }
}
