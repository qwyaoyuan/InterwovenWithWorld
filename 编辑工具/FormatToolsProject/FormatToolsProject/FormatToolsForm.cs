using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FormatToolsProject
{
    public partial class FormatToolsForm : Form
    {
        public FormatToolsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 导入配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 导入配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "文本文件|*.txt";
            DialogResult dr = openFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                TextBox_Config.Text = File.ReadAllText(filePath);
                InitDataGrid();
            }
        }

        /// <summary>
        /// 导出配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 导出配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "文本文件|*.txt";
            DialogResult dr = saveFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                File.WriteAllText(filePath, TextBox_Config.Text, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 初始化表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 初始化表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitDataGrid();
        }

        /// <summary>
        /// 根据当前的内容初始化表
        /// </summary>
        private void InitDataGrid()
        {
            string config = TextBox_Config.Text;
            DataGridView_Data.Columns.Clear();
            DataGridView_Data.Rows.Clear();
            if (config.Length == 0)
            {
                return;
            }
            List<string> mustReplaceList = new List<string>();
            int index = 0;
            while (index >= 0 || index < config.Length)
            {
                index = config.IndexOf('{', index);
                if (index < 0)
                    break;
                int findIndex = config.IndexOf('&', index);
                if (findIndex - index == 1 && index > -1)
                {
                    int endIndex = config.IndexOf('}', findIndex);
                    string mustReplace = config.Substring(index, endIndex - index + 1);
                    if (!mustReplaceList.Contains(mustReplace))
                        mustReplaceList.Add(mustReplace);
                    index = endIndex + 1;
                }
                else index++;
            }
            foreach (string mustReplace in mustReplaceList)
            {
                DataGridView_Data.Columns.Add(mustReplace, mustReplace);
            }
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 生成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\Result.txt";
            string sourceText = TextBox_Config.Text;
            int rowsLength = DataGridView_Data.Rows.Count;
            int columnLength = DataGridView_Data.Columns.Count;
            if (rowsLength > 0 && columnLength > 0)
            {
                string[,] replaceArrays = new string[rowsLength, columnLength];
                string[] replaceFlag = new string[columnLength];
                for (int i = 0; i < columnLength; i++)
                {
                    replaceFlag[i] = DataGridView_Data.Columns[i].Name;
                }
                for (int i = 0; i < rowsLength; i++)
                {
                    for (int j = 0; j < columnLength; j++)
                    {
                        replaceArrays[i, j] = DataGridView_Data.Rows[i].Cells[j].Value?.ToString();
                    }
                }
                string endString = "";
                for (int i = 0; i < rowsLength - 1; i++)
                {
                    string tempStr = sourceText;
                    for (int j = 0; j < columnLength; j++)
                    {
                        string replaceStr = replaceArrays[i, j];
                        if (replaceStr == null)
                            replaceStr = "";
                        tempStr = tempStr.Replace(replaceFlag[j], replaceStr);
                    }
                    endString += tempStr + "\r\n\r\n";
                }
                File.WriteAllText(filePath, endString, Encoding.UTF8);
                Process.Start(filePath);
            }

        }


    }
}
