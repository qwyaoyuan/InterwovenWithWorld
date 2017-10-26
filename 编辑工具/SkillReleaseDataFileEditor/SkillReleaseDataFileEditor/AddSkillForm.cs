using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SkillDataFileEditor
{
    public partial class AddSkillForm : Form
    {
        public AddSkillForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 技能名
        /// </summary>
        public string skillName = "";
        /// <summary>
        /// 技能id
        /// </summary>
        public string skillID = "";

        private void Button_OK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_SkillID.Text.Trim())||string.IsNullOrEmpty(TextBox_SkillName.Text.Trim()))
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            skillName = TextBox_SkillName.Text.Trim();
            skillID = TextBox_SkillID.Text.Trim();
            DialogResult = DialogResult.OK;
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
