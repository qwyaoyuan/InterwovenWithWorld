using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoodsEditor
{
    public partial class GoodsControl : Form
    {
        public GoodsControl()
        {
            InitializeComponent();

            itemInformationform = new GoodsInfoMation();
            InitialGoodsAbilitys();
            itemInformationform.Parent = this.panel1;
            this.panel1.Width = itemInformationform.Width;
            this.Width = this.panel1.Width;
            itemInformationform.ShowGoodsInfo(allGoodsInfo);
        }


        /// <summary>
        /// 初始化已保存的物品能力
        /// </summary>
        private void InitialGoodsAbilitys()
        {
            if (File.Exists("allGoodsMetaInfo.txt"))
            {
                allGoodsInfo = JsonConvert.DeserializeObject<Dictionary<int, Goods>>(File.ReadAllText("allGoodsMetaInfo.txt"));
                foreach (var item in allGoodsInfo)
                {
                    allGoodsAbilitys.Add(item.Key, item.Value.goodsAbilities);
                }

            }
        }

        public static Dictionary<int, Goods> allGoodsInfo = new Dictionary<int, Goods>();

        GoodsInfoMation itemInformationform = null;

        /// <summary>
        /// 所有物品的能力
        /// </summary>
        public static Dictionary<int, List<GoodsAbility>> allGoodsAbilitys = new Dictionary<int, List<GoodsAbility>>();



        private void 加载文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = System.Environment.CurrentDirectory;
            var result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                itemInformationform.ShowGoodsInfo(allGoodsInfo);
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var allGoodsBaseInfo = itemInformationform.GetAllGoodsInfoNoAbilitys();
            if (allGoodsBaseInfo == null) return;
            foreach (var goodsBaseInfo in allGoodsBaseInfo)
            {
                if (allGoodsAbilitys.Keys.Contains(goodsBaseInfo.Key))
                    goodsBaseInfo.Value.goodsAbilities = allGoodsAbilitys[goodsBaseInfo.Key];
            }
            //保存数据

            string output = JsonConvert.SerializeObject(allGoodsBaseInfo);
            File.WriteAllText("allGoodsMetaInfo.txt", output);

            GenerateEnumClass();
            MessageBox.Show(string.Format("ok"));
        }

        private void GenerateEnumClass()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("public enum EnumGoodsType");
            sb.AppendLine("{");
            //     /// <summary>
            //     /// 素材类的起始
            //     /// </summary>
            //[FieldExplan("素材")]
            //SourceMaterial = 1000000,

  
            Dictionary<string, int> enumDic = new Dictionary<string, int>();
            //模板
            var allEnumFileds = itemInformationform.GetAllNeedGenerateEnumFileds();
            foreach (var enumField in allEnumFileds)
            {
                sb.AppendLine("/// <summary>");
                sb.AppendLine("///" + enumField.Value.Key);
                sb.AppendLine("/// </summary>");
                sb.AppendLine("[FieldExplan(" + "\"" + enumField.Value.Key + "\"" + ")]");
                if (enumDic.ContainsKey(enumField.Value.Value))
                    enumDic[enumField.Value.Value]++;
                else
                    enumDic.Add(enumField.Value.Value, 0);
                if (enumDic[enumField.Value.Value] == 0)
                    sb.AppendLine(enumField.Value.Value + "=" + enumField.Key  + ",");
                else
                    sb.AppendLine(enumField.Value.Value + enumDic[enumField.Value.Value] + "=" + enumField.Key  + ",");


            }
            sb.AppendLine("}");
            string writePath = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "bin" }, StringSplitOptions.RemoveEmptyEntries)[0];
            File.WriteAllText(writePath + "EnumGoodsType.cs", sb.ToString());
        }


        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DataGridView datadridView = this.panel1.Controls[0].Controls[0] as DataGridView;
            if (datadridView[0, datadridView.CurrentCell.RowIndex].Value != null)
            {
                GoodsEditor editor = new GoodsEditor(int.Parse(datadridView[0, datadridView.CurrentCell.RowIndex].Value.ToString()));
                editor.GoodsAbilitysChanged += Editor_GoodsAbilitysChanged;
                editor.ShowDialog();
            }
        }

        private void Editor_GoodsAbilitysChanged(int id, List<GoodsAbility> abilitys)
        {
            if (allGoodsAbilitys.ContainsKey(id))
            {
                allGoodsAbilitys[id] = abilitys;

            }
            else
            {
                allGoodsAbilitys.Add(id, abilitys);
            }
        }
    }
}
