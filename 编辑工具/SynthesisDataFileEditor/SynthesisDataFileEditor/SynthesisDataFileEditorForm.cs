using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            synthesisDataAnalysis = new SynthesisDataAnalysis();
        }

        /// <summary>
        /// 其实id
        /// </summary>
        int startID;

        /// <summary>
        /// 文件名
        /// </summary>
        private string fileName;
        /// <summary>
        /// 合成数据解析对象
        /// </summary>
        private SynthesisDataAnalysis synthesisDataAnalysis;

        /// <summary>
        /// 设置TreeView的基础节点
        /// </summary>
        private void InitTreeView()
        {
            TreeView_Main.Nodes.Clear();
            //顶层节点，表示是炼金还是打造
            Type synthesisTypeType = typeof(EnumSynthesisType);
            foreach (FieldInfo synthesisTypeFieldInfo in synthesisTypeType.GetFields())
            {
                if (synthesisTypeFieldInfo.Name.Equals("value__"))
                    continue;
                TreeNode synthesisTypeNode = new TreeNode(synthesisTypeFieldInfo.Name);
                FieldExplanAttribute fieldExplanAttribute_Type = synthesisTypeFieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).Select(temp => temp as FieldExplanAttribute).FirstOrDefault();
                if (fieldExplanAttribute_Type != null)
                {
                    synthesisTypeNode.Text = fieldExplanAttribute_Type.GetExplan();
                }
                synthesisTypeNode.Tag = synthesisTypeFieldInfo.Name;
                synthesisTypeNode.Name = synthesisTypeFieldInfo.Name;
                TreeView_Main.Nodes.Add(synthesisTypeNode);

                //添加子层节点，具体条目，和熟练度相关的类型
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
                    synthesisItemNode.Name = synthesisItemFieldInfo.Name;
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
            startID = 0;
            this.fileName = fileName;
            InitTreeView();
            //读取数据
            string valueStr = File.ReadAllText(this.fileName);
            synthesisDataAnalysis.ReadData(valueStr);
            int[] ids = synthesisDataAnalysis.IDArray;
            if (ids.Length > 0)
                startID = ids.Max() + 1;
            SynthesisDataStruct[] synthesisDataStructs = ids.Select(temp => synthesisDataAnalysis.GetDataByID(temp)).ToArray();
            foreach (SynthesisDataStruct synthesisDataStruct in synthesisDataStructs)
            {
                TreeNode synthesisTypeNode = TreeView_Main.Nodes.Find(synthesisDataStruct.synthesisType.ToString(), false).FirstOrDefault();
                if (synthesisTypeNode != null)
                {
                    TreeNode synthesisItemNode = synthesisTypeNode.Nodes.Find(synthesisDataStruct.synthesisItem.ToString(), false).FirstOrDefault();
                    if (synthesisItemNode != null)
                    {
                        TreeNode dataNode = new TreeNode();
                        dataNode.Name = synthesisDataStruct.id.ToString();
                        dataNode.Text = synthesisDataStruct.ToStringSimple();
                        dataNode.Tag = synthesisDataStruct;
                        synthesisItemNode.Nodes.Add(dataNode);
                    }
                }
            }
            //使控件可用
            添加节点ToolStripMenuItem.Enabled = true;
            删除节点ToolStripMenuItem.Enabled = true;
            保存方案ToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// 保存方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存方案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string valueStr = synthesisDataAnalysis.GetData();
            File.WriteAllText(fileName, valueStr, Encoding.UTF8);
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 添加节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectNode = TreeView_Main.SelectedNode;
            if (selectNode != null)
            {
                Stack<TreeNode> treeNodeStack = new Stack<TreeNode>();
                Action<TreeNode> selectParentAction = null;
                selectParentAction = (thisNode) =>
                {
                    treeNodeStack.Push(thisNode);
                    if (thisNode.Parent != null)
                        selectParentAction(thisNode.Parent);
                };
                selectParentAction(selectNode);
                if (treeNodeStack.Count >= 2)
                {
                    TreeNode synthesisTypeNode = treeNodeStack.Pop();
                    TreeNode synthesisItemNode = treeNodeStack.Pop();
                    EnumSynthesisType synthesisType = (EnumSynthesisType)Enum.Parse(typeof(EnumSynthesisType), synthesisTypeNode.Tag.ToString());
                    EnumSynthesisItem synthesisItem = (EnumSynthesisItem)Enum.Parse(typeof(EnumSynthesisItem), synthesisItemNode.Tag.ToString());
                    int id = startID++;
                    SynthesisDataStruct synthesisDataStruct = new SynthesisDataStruct();
                    synthesisDataStruct.id = id;
                    synthesisDataStruct.name = "";
                    synthesisDataStruct.time = 1;
                    synthesisDataStruct.synthesisType = synthesisType;
                    synthesisDataStruct.synthesisItem = synthesisItem;
                    synthesisDataStruct.inputStruct = new SynthesisDataStruct.SynthesisItemStruct[0];
                    synthesisDataAnalysis.AddSynthesisDataStruct(synthesisDataStruct);
                    TreeNode treeNode = new TreeNode();
                    treeNode.Name = id.ToString();
                    treeNode.Tag = synthesisDataStruct;
                    treeNode.Text = synthesisDataStruct.ToStringSimple();
                    synthesisItemNode.Nodes.Add(treeNode);
                }
            }
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectNode = TreeView_Main.SelectedNode;
            if (selectNode != null)
            {
                if (selectNode.Tag != null && selectNode.Tag.GetType().Equals(typeof(SynthesisDataStruct)))
                {
                    synthesisDataAnalysis.RemoveSynthesisDataStruct(selectNode.Tag as SynthesisDataStruct);
                    selectNode.Parent.Nodes.Remove(selectNode);
                }
            }
        }

        /// <summary>
        /// 双击控件时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_Main_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null && e.Node.Tag != null && e.Node.Tag.GetType().Equals(typeof(SynthesisDataStruct)))
            {
                DataEditorForm dataEditorForm = new DataEditorForm(e.Node);
                dataEditorForm.ShowDialog();
            }
        }
    }
}
