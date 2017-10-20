using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SynthesisDataFileEditor
{
    public partial class SynthesisDataFileEditorForm : Form
    {
        public SynthesisDataFileEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置TreeView的基础节点
        /// </summary>
        private void InitTreeView()
        {
            TreeView_Main.Nodes.Clear();
            //顶层节点，表示是炼金还是打造
            Type synthesisTypeType = typeof(EnumSynthesisType);
            foreach (FieldInfo  synthesisTypeFieldInfo in synthesisTypeType.GetFields())
            {
                if (synthesisTypeFieldInfo.Name.Equals("value__"))
                    continue;
                TreeNode synthesisTypeNode = new TreeNode(synthesisTypeFieldInfo.Name);
                FieldExplanAttribute fieldExplanAttribute_Type = synthesisTypeFieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).Select(temp =>temp as FieldExplanAttribute).FirstOrDefault();
                if (fieldExplanAttribute_Type != null)
                {
                    synthesisTypeNode.Text = fieldExplanAttribute_Type.GetExplan();
                }
                synthesisTypeNode.Tag = synthesisTypeFieldInfo.Name;
                TreeView_Main.Nodes.Add(synthesisTypeNode);

                //添加子层节点
                Type synthesisItemType = typeof(EnumSynthesisItem);
                foreach (FieldInfo synthesisItemFieldInfo in synthesisItemType.GetFields())
                {
                    if (synthesisItemFieldInfo.Name.Equals("value__"))
                        continue;
                    TreeNode synthesisItemNode = new TreeNode(synthesisItemFieldInfo.Name);
                    FieldExplanAttribute fieldExplanAttribute_Item = synthesisItemFieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).Select(temp => temp as FieldExplanAttribute).FirstOrDefault();
                    if (fieldExplanAttribute_Item != null)
                    {
                        synthesisItemNode.Text = fieldExplanAttribute_Item.GetExplan();
                    }
                    synthesisItemNode.Tag = synthesisItemFieldInfo.Name;
                    synthesisTypeNode.Nodes.Add(synthesisItemNode);
                }
            }
        }

        /// <summary>
        /// 打开方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 打开方案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
#if DEBUG
            ofd.InitialDirectory = @"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\编辑工具\SynthesisData";
#endif
            ofd.Filter = "合成结构文件|*.SynthesisEditor";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                OpenProject(ofd.FileName);
            }
        }

        /// <summary>
        /// 新建方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新建方案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProjectForm newProjectForm = new NewProjectForm();
            DialogResult dr = newProjectForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                OpenProject(newProjectForm.newFilePath);
            }
        }

        /// <summary>
        /// 打开方案
        /// </summary>
        /// <param name="fileName"></param>
        private void OpenProject(string fileName)
        {
            InitTreeView();
        }

        private void 保存方案ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }




    }
}
