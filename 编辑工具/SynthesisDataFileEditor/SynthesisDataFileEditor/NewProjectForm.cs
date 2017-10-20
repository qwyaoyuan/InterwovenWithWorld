using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SynthesisDataFileEditor
{
    public partial class NewProjectForm : Form
    {
        /// <summary>
        /// 上次选择的路径
        /// </summary>
        public static string lastPath = @"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\编辑工具\SkillData";

        /// <summary>
        /// 新建文件的路径
        /// </summary>
        public string newFilePath = "";

        public NewProjectForm()
        {
            InitializeComponent();
            FolderBrowserDialog_Select.SelectedPath = lastPath;
            TextBox_Folder.Text = lastPath;
        }

        /// <summary>
        /// 选择文件夹 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_SelectFolder_Click(object sender, EventArgs e)
        {
            DialogResult dr = FolderBrowserDialog_Select.ShowDialog();
            if (dr == DialogResult.OK)
            {
                lastPath = FolderBrowserDialog_Select.SelectedPath;
                TextBox_Folder.Text = lastPath;
            }
        }

        private void Button_OK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_ProjectName.Text) || string.IsNullOrEmpty(TextBox_Folder.Text))
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            newFilePath = TextBox_Folder.Text + "\\" + TextBox_ProjectName.Text + ".SynthesisEditor";
            if (File.Exists(newFilePath))
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            File.Create(newFilePath).Close();
            DialogResult = DialogResult.OK;
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
