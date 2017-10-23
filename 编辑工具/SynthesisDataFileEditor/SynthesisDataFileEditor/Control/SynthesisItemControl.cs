using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace SynthesisDataFileEditor
{
    /// <summary>
    /// 合成所需条目数据控件
    /// </summary>
    public partial class SynthesisItemControl : UserControl
    {
        /// <summary>
        /// 数据对象
        /// </summary>
        SynthesisDataStruct.SynthesisItemStruct synthesisItemStruct;

        public SynthesisItemControl(SynthesisDataStruct.SynthesisItemStruct synthesisItemStruct)
        {
            InitializeComponent();
            this.synthesisItemStruct = synthesisItemStruct;
            Init();
        }

        private void Init()
        {
            TextBox_SynthesisItem.Text = SynthesisDataStruct.GetEnumExplan( synthesisItemStruct.itemType);//材料或成品类型
            if (synthesisItemStruct.num <= 0)
                synthesisItemStruct.num = 1;
            else if (synthesisItemStruct.num > 100)
                synthesisItemStruct.num = 100;
            NumericUpDown_Num.Value = synthesisItemStruct.num;
            NumericUpDown_Num.ValueChanged += (sender, e) => { synthesisItemStruct.num = (int)NumericUpDown_Num.Value; };
            Action<ComboBox, Type> InitComboBoxAction = (cbo, type) =>
            {
                FieldInfo[] fieldInfo = type.GetFields().Where(temp => !temp.Name.Equals("value__")).ToArray();
                var tempDic = fieldInfo.Select(temp => new { name = temp.Name, attr = temp.GetCustomAttributes(typeof(FieldExplanAttribute), false).Select(_temp => _temp as FieldExplanAttribute).FirstOrDefault() });
                DataTable dt = new DataTable();
                dt.Columns.Add("Value");
                dt.Columns.Add("Display");
                foreach (var tempKeyValue in tempDic)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = tempKeyValue.name;
                    dr[1] = tempKeyValue.attr != null ? tempKeyValue.attr.GetExplan() : tempKeyValue.name;
                    dt.Rows.Add(dr);
                }
                cbo.DataSource = dt;
                cbo.ValueMember = "Value";
                cbo.DisplayMember = "Display";
            };
            InitComboBoxAction(ComboBox_MinQuality, typeof(EnumQualityType));
            InitComboBoxAction(ComboBox_MaxQuality, typeof(EnumQualityType));
            ComboBox_MinQuality.SelectedValue = synthesisItemStruct.minQuality.ToString();
            ComboBox_MaxQuality.SelectedValue = synthesisItemStruct.maxQuality.ToString();
            ComboBox_MinQuality.SelectedIndexChanged += (sender, e) =>
            {
                try { synthesisItemStruct.minQuality = (EnumQualityType)Enum.Parse(typeof(EnumQualityType), ComboBox_MinQuality.SelectedValue.ToString()); } catch { }
            };
            ComboBox_MaxQuality.SelectedIndexChanged += (sender, e) =>
            {
                try { synthesisItemStruct.maxQuality = (EnumQualityType)Enum.Parse(typeof(EnumQualityType), ComboBox_MaxQuality.SelectedValue.ToString()); } catch { }
            };
        }

    }
}
