using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SkillDataFileEditor
{
    public partial class SkillDataFileEditorForm : Form
    {
        public SkillDataFileEditorForm()
        {
            InitializeComponent();
            projectFilePath = "";
            skillAnalysisData = new SkillAnalysisData();
        }

        /// <summary>
        /// 工程文件路径
        /// 工程文件仅仅保存指定id的技能对应的文件名
        /// </summary>
        string projectFilePath;
        /// <summary>
        /// 技能数据解析对象
        /// </summary>
        SkillAnalysisData skillAnalysisData;

        /// <summary>
        /// 新建一个项目文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_NewProject_Click(object sender, EventArgs e)
        {
            NewProjectForm newProjectForm = new NewProjectForm();
            DialogResult dr = newProjectForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                projectFilePath = newProjectForm.newFilePath;
                OpenProject();
            }
        }

        /// <summary>
        /// 打开一个项目文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_LoadProject_Click(object sender, EventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();
            opd.InitialDirectory = NewProjectForm.lastPath;
            opd.Filter = "技能编辑文件|*.SkillEditor";
            DialogResult dr = opd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                projectFilePath = opd.FileName;
                OpenProject();
            }
        }

        /// <summary>
        /// 清理数据与控件
        /// </summary>
        private void ClearData()
        {
            TreeView_Skills.Nodes.Clear();
            skillAnalysisData.Clear();
        }

        /// <summary>
        /// 打开工程
        /// </summary>
        private void OpenProject()
        {
            ClearData();
            string[] splits = new string[] { "^^^" };
            using (StreamReader fs = new StreamReader(projectFilePath, Encoding.UTF8))
            {
                string readLine = null;
                while ((readLine = fs.ReadLine()) != null)
                {
                    readLine = readLine.Trim();
                    if (string.IsNullOrEmpty(readLine))
                        continue;
                    // id 技能名 文件名
                    string[] idToDatas = readLine.Split(splits, StringSplitOptions.RemoveEmptyEntries);
                    if (idToDatas.Length != 3)
                        continue;
                    TreeNode treeNode = new TreeNode(idToDatas[1]);
                    treeNode.Name = idToDatas[0];
                    treeNode.Tag = idToDatas;
                    TreeView_Skills.Nodes.Add(treeNode);
                }
            }
            List<string> values = new List<string>();
            string folderPath = Path.GetDirectoryName(projectFilePath);
            foreach (TreeNode treeNode in TreeView_Skills.Nodes)
            {
                string[] idToDatas = treeNode.Tag as string[];
                if (idToDatas != null)
                {
                    string value = File.ReadAllText(folderPath + "\\" + idToDatas[2], Encoding.UTF8);
                    values.Add(value);
                }
            }
            skillAnalysisData.AnalysisData(values.ToArray());

            Button_SaveProject.Enabled = true;
            Button_AddSkill.Enabled = false;
            Button_DeleteSkill.Enabled = false;
        }

        /// <summary>
        /// 添加技能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_AddSkill_Click(object sender, EventArgs e)
        {
            AddSkillForm addSkillForm = new AddSkillForm();
            DialogResult dr = addSkillForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string skillID = addSkillForm.skillID;
                string skillName = addSkillForm.skillName;
                string skillFileName = Tools.GetRandomString(30, true, true, true, false, "");
                string folderPath = Path.GetDirectoryName(projectFilePath);
                TreeNode[] checkNodesByID = TreeView_Skills.Nodes.Find(skillID, false);
                int selectCount = TreeView_Skills.Nodes.OfType<TreeNode>().Where(
                    temp =>
                    {
                        string[] tags = (string[])temp.Tag;
                        return string.Equals(tags[2], skillFileName);
                    }).Count();
                if (checkNodesByID.Length > 0 || selectCount > 0)
                {
                    return;
                }
                skillAnalysisData.AddID(skillID);
                skillAnalysisData.SetValue(skillID, "skillID", skillID);
                skillAnalysisData.SetValue(skillID, "skillName", skillName);
                TreeNode treeNode = new TreeNode(skillName);
                treeNode.Name = skillID;
                treeNode.Tag = new string[] { skillID, skillName, skillFileName };
                TreeView_Skills.Nodes.Add(treeNode);
                TreeView_Skills.SelectedNode = treeNode;
            }
        }

        /// <summary>
        /// 删除技能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_DeleteSkill_Click(object sender, EventArgs e)
        {
            TreeNode selectNode = TreeView_Skills.SelectedNode;
            if (selectNode != null)
            {
                DialogResult dr = MessageBox.Show("是否删除选中技能?", "警告！", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    TreeView_Skills.Nodes.Remove(selectNode);
                    skillAnalysisData.RemoveID(selectNode.Name);
                    string folderPath = Path.GetDirectoryName(projectFilePath);
                    string fileName = ((string[])selectNode.Tag)[2];
                    if (File.Exists(folderPath + "\\" + fileName))
                        File.Delete(folderPath + "\\" + fileName);
                }

            }
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_SaveProject_Click(object sender, EventArgs e)
        {
            if (File.Exists(projectFilePath))
            {
                string projectValue = "";
                string[] splits = new string[] { "^^^" };
                string folderPath = Path.GetDirectoryName(projectFilePath);
                foreach (TreeNode treeNode in TreeView_Skills.Nodes)
                {
                    string[] tags = (string[])treeNode.Tag;
                    foreach (string tag in tags)
                    {
                        projectValue += tag + splits[0];
                    }
                    projectValue += "\r\n";
                    //每个文件的数据
                    string value = skillAnalysisData.Disanalysis(tags[0]);
                    File.WriteAllText(folderPath + "\\" + tags[2], value, Encoding.UTF8);
                }
                File.WriteAllText(projectFilePath, projectValue, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 保存当前修改到内存中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_SaveSkillToMemory_Click(object sender, EventArgs e)
        {

        }
    }
}
