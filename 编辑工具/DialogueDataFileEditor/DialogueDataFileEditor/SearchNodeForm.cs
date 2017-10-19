using DialogueDataFileEditor.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DialogueDataFileEditor
{
    public partial class SearchNodeForm : Form
    {
        public SearchNodeForm()
        {
            InitializeComponent();
            ToolStripComboBox_DialogueType.SelectedIndex = 0;
            ToolStripTextBox_Leave(ToolStripTextBox_TopDialogueID, null);
            ToolStripTextBox_Leave(ToolStripTextBox_Keyword, null);
            ToolStripTextBox_Leave(ToolStripTextBox_NPCID, null);
            this.Activated += SearchNodeForm_Activated;
        }

        /// <summary>
        /// 是否需要关闭
        /// </summary>
        bool mustClose = false;

        /// <summary>
        /// 是否自动更新
        /// </summary>
        bool autoUpdate = false;

        /// <summary>
        /// 基础父控件
        /// </summary>
        public FlowLayoutPanel BasePanel { get; set; }

        /// <summary>
        /// 将选择的控件移动到父控件指定位置以显示控件
        /// </summary>
        public Action<Panel> ShowSearchPanel;

        /// <summary>
        /// 节点发生了改变
        /// </summary>
        public bool NodeChanged { get; set; }

        /// <summary>
        /// 当进入输入文本框时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripTextBox_Enter(object sender, EventArgs e)
        {
            ToolStripTextBox toolStripTextBox = sender as ToolStripTextBox;
            string defaultStr = toolStripTextBox.Tag.ToString();
            if (string.Equals(toolStripTextBox.Text.Trim(), defaultStr.Trim()))
            {
                toolStripTextBox.Text = "";
                toolStripTextBox.ForeColor = Color.FromArgb(0, 0, 0);
            }
        }

        /// <summary>
        /// 当离开文本输入框时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripTextBox_Leave(object sender, EventArgs e)
        {
            ToolStripTextBox toolStripTextBox = sender as ToolStripTextBox;
            string defaultStr = toolStripTextBox.Tag.ToString();
            if (string.IsNullOrEmpty(toolStripTextBox.Text.Trim()) || string.Equals(toolStripTextBox.Text.Trim(), defaultStr.Trim()))
            {
                toolStripTextBox.Text = defaultStr;
                toolStripTextBox.ForeColor = Color.FromArgb(100, 100, 100);
            }
            else
            {
                toolStripTextBox.ForeColor = Color.FromArgb(0, 0, 0);
            }
        }

        private void ToolStripTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter)
                ToolStripTextBox_Leave(sender, e);
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchNodeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!mustClose)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        /// <summary>
        /// 必须关闭
        /// </summary>
        public void MustClose()
        {
            mustClose = true;
            this.Close();
        }

        /// <summary>
        /// 清理搜索条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Clear_Click(object sender, EventArgs e)
        {
            ToolStripTextBox_TopDialogueID.Text = "";
            ToolStripTextBox_Leave(ToolStripTextBox_TopDialogueID, null);
            ToolStripTextBox_Keyword.Text = "";
            ToolStripTextBox_Leave(ToolStripTextBox_Keyword, null);
            ToolStripComboBox_DialogueType.SelectedIndex = 0;
            ToolStripMenuItem_Search_Click(null, null);
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Search_Click(object sender, EventArgs e)
        {
            TreeView_Main.Nodes.Clear();
            string searchID = ToolStripTextBox_TopDialogueID.Text.Trim();
            if (string.IsNullOrEmpty(ToolStripTextBox_TopDialogueID.Text.Trim()) ||
               string.Equals(ToolStripTextBox_TopDialogueID.Text.Trim(), ToolStripTextBox_TopDialogueID.Tag.ToString().Trim()))
                searchID = "";
            string searchKeyword = ToolStripTextBox_Keyword.Text.Trim();
            if (string.IsNullOrEmpty(ToolStripTextBox_Keyword.Text.Trim()) ||
               string.Equals(ToolStripTextBox_Keyword.Text.Trim(), ToolStripTextBox_Keyword.Tag.ToString().Trim()))
                searchKeyword = "";
            string searchNPCID = ToolStripTextBox_NPCID.Text.Trim();
            if (string.IsNullOrEmpty(ToolStripTextBox_NPCID.Text.Trim()) ||
                string.Equals(ToolStripTextBox_NPCID.Text.Trim(), ToolStripTextBox_NPCID.Tag.ToString().Trim()))
                searchNPCID = "";
            string searchType = ToolStripComboBox_DialogueType.SelectedItem.ToString();
            if (string.IsNullOrEmpty(searchType) || searchType.Trim().Equals("None"))
                searchType = "";
            if (BasePanel == null)
                return;
            var searchValues = BasePanel.Controls.OfType<Panel>().
                Select(temp => new
                {
                    panel = temp,
                    dialogueConditionControl = temp.Controls.OfType<DialogueConditionControl>().FirstOrDefault()
                }).
                Select(temp => new
                {
                    panel = temp.panel,
                    dialogueConditionControl = temp.dialogueConditionControl,
                    dialoguePointControl =
                        (temp.dialogueConditionControl != null && temp.dialogueConditionControl.Tag != null && (temp.dialogueConditionControl.Tag as List<Control>) != null && (temp.dialogueConditionControl.Tag as List<Control>).Count > 0) ?
                        (temp.dialogueConditionControl.Tag as List<Control>)[0] as DialoguePointControl :
                        null
                }).
                Where(temp => searchID.Equals("") ? true :
                    (temp.dialoguePointControl == null ? false :
                        (temp.dialoguePointControl.GetDialogueValue().dialogueID.ToString().Equals(searchID) ? true : false))).
                Where(temp => searchKeyword.Equals("") ? true :
                    (temp.dialogueConditionControl == null ? false :
                        (temp.dialogueConditionControl.GetDialogueCondition().text.Contains(searchKeyword) ? true : false))).
                Where(temp => searchNPCID.Equals("") ? true :
                    (temp.dialogueConditionControl == null ? false :
                        (temp.dialogueConditionControl.GetDialogueCondition().touchNPCID.ToString().Equals(searchNPCID) ? true : false))).
                Where(temp => searchType.Equals("") ? true :
                    (temp.dialogueConditionControl == null ? false :
                        (temp.dialogueConditionControl.GetDialogueCondition().enumDialogueType.ToString().Equals(searchType) ? true : false)));
            foreach (var searchValue in searchValues)
            {
                TreeNode treeNode = new TreeNode();
                TreeView_Main.Nodes.Add(treeNode);
                SetPanelToTreeNode(treeNode, searchValue.panel);
            }
        }

        /// <summary>
        /// 将panel的数据设置到TreeNode中
        /// </summary>
        /// <param name="treeNode">树节点</param>
        /// <param name="panel">对应的panel</param>
        private void SetPanelToTreeNode(TreeNode treeNode, Panel panel)
        {
            treeNode.Tag = panel;
            DialogueConditionControl dialogueConditionControl = panel.Controls.OfType<DialogueConditionControl>().FirstOrDefault();
            DialogueCondition dialogueCondition = dialogueConditionControl.GetDialogueCondition();
            treeNode.Text = "Text:[" + dialogueCondition.text + "];Type:[" + dialogueCondition.enumDialogueType + "]";
            if (dialogueConditionControl.Tag != null && (dialogueConditionControl.Tag as List<Control>) != null && (dialogueConditionControl.Tag as List<Control>).Count > 0)
            {
                SetControlToTreeNode(treeNode, (dialogueConditionControl.Tag as List<Control>).ToArray());
            }
        }

        /// <summary>
        /// 将controls的数据设置到TreeNode中 
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="controls"></param>
        private void SetControlToTreeNode(TreeNode treeNode, params Control[] controls)
        {
            foreach (Control control in controls)
            {
                DialoguePointControl dialoguePointControl = control as DialoguePointControl;
                if (dialoguePointControl != null)
                {
                    DialogueValue dialogueValue = dialoguePointControl.GetDialogueValue();
                    TreeNode childTreeNode = new TreeNode();
                    treeNode.Nodes.Add(childTreeNode);
                    childTreeNode.Tag = dialogueValue;
                    childTreeNode.Text = "ID:[" + dialogueValue.dialogueID +
                        "];NPC ID:[" + dialogueValue.npcID +
                        "];Title:[" + dialogueValue.titleValue +
                        "];Value:[" + dialogueValue.showValue + "]";
                    if (dialoguePointControl.Tag != null && (dialoguePointControl.Tag as List<Control>) != null && (dialoguePointControl.Tag as List<Control>).Count > 0)
                    {
                        SetControlToTreeNode(childTreeNode, (dialoguePointControl.Tag as List<Control>).ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// 是否自动更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Auto_Click(object sender, EventArgs e)
        {
            autoUpdate = !autoUpdate;
            ToolStripMenuItem_Auto.Image = autoUpdate ? Resources.Checked : Resources.Empty;
        }

        /// <summary>
        /// 当窗体获取焦点时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchNodeForm_Activated(object sender, EventArgs e)
        {
            if (autoUpdate && BasePanel != null && NodeChanged)
            {
                NodeChanged = false;
                ToolStripMenuItem_Search_Click(null, null);
            }
        }

        /// <summary>
        /// 双击节点时主窗体更新位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_Main_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Func<TreeNode, TreeNode> GetRoot = null;
            GetRoot = (parent) =>
            {
                if (parent.Parent != null)
                    return GetRoot(parent.Parent);
                return parent;
            };
            TreeNode root = GetRoot(e.Node);
            Panel panel = root.Tag as Panel;
            ShowSearchPanel?.Invoke(panel);
        }
    }
}
