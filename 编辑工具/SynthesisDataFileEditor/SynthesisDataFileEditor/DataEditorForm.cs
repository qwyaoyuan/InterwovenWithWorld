using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SynthesisDataFileEditor
{
    public partial class DataEditorForm : Form
    {

        /// <summary>
        /// 物品树节点
        /// </summary>
        static TreeNode[] itemTreeNodeArray;

        /// <summary>
        /// 获取物品类型构造的树节点
        /// </summary>
        /// <returns></returns>
        static TreeNode[] GetItemTypeTreeNodes()
        {
            int layer1 = 1000000;
            int layer2 = 100000;
            int layer3 = 1000;
            Type itemTypeType = typeof(EnumGoodsType);
            Dictionary<EnumGoodsType, FieldExplanAttribute> tempExplanDic = itemTypeType.GetFields().Where(temp => !temp.Name.Equals("value__"))
                .ToDictionary(temp => (EnumGoodsType)Enum.Parse(typeof(EnumGoodsType), temp.Name), temp => temp.GetCustomAttributes(typeof(FieldExplanAttribute), false).FirstOrDefault())
                .Where(temp => temp.Value != null)
                .ToDictionary(temp => temp.Key, temp => temp.Value as FieldExplanAttribute);

            var tempDataStruct = Enum.GetValues(typeof(EnumGoodsType)).OfType<EnumGoodsType>()
                .Select(temp => new
                {
                    type = temp,
                    value = (int)temp,
                    name = tempExplanDic.ContainsKey(temp) ? tempExplanDic[temp].GetExplan() : ""
                })
                .OrderBy(temp => temp.value)
                .ToArray();
            List<TreeNode> itemTreeNodeList = new List<TreeNode>();
            for (int i = 1; i < 10; i++)//第一层
            {
                #region 第一层
                int layer1Min = layer1 * i;
                int layer1Max = layer1 * (i + 1);
                var layer1TempDataStruct = tempDataStruct.Where(temp => temp.value >= layer1Min && temp.value < layer1Max)
                    .Select(temp => new { type = temp.type, value = temp.value % layer1, name = temp.name })
                    .ToArray();
                if (layer1TempDataStruct.Length == 0)
                    continue;
                TreeNode layer1TreeNode = new TreeNode();
                layer1TreeNode.Text = layer1TempDataStruct[0].name;
                layer1TreeNode.Tag = layer1TempDataStruct[0].type;
                itemTreeNodeList.Add(layer1TreeNode);
                #endregion
                for (int j = 1; j < 10; j++)//第二层
                {
                    #region 第二层
                    int layer2Min = layer2 * j;
                    int layer2Max = layer2 * (j + 1);
                    var layer2TempDataStruct = layer1TempDataStruct.Where(temp => temp.value >= layer2Min && temp.value < layer2Max)
                        .Select(temp => new { type = temp.type, value = temp.value % layer2, name = temp.name })
                        .ToArray();
                    if (layer2TempDataStruct.Length == 0)
                        continue;
                    TreeNode layer2TreeNode = new TreeNode();
                    layer2TreeNode.Text = layer2TempDataStruct[0].name;
                    layer2TreeNode.Tag = layer2TempDataStruct[0].type;
                    layer1TreeNode.Nodes.Add(layer2TreeNode);
                    #endregion
                    for (int k = 1; k < 100; k++)//第三层
                    {
                        #region 第三层
                        int layer3Min = layer3 * k;
                        int layer3Max = layer3 * (k + 1);
                        var layer3TempDataStruct = layer2TempDataStruct.Where(temp => temp.value >= layer3Min && temp.value < layer3Max)
                            .Select(temp => new { type = temp.type, value = temp.value % layer3, name = temp.name })
                            .ToArray();
                        if (layer3TempDataStruct.Length == 0)
                            continue;
                        TreeNode layer3TreeNode = new TreeNode();
                        layer3TreeNode.Text = layer3TempDataStruct[0].name;
                        layer3TreeNode.Tag = layer3TempDataStruct[0].type;
                        layer2TreeNode.Nodes.Add(layer3TreeNode);
                        #endregion
                        for (int l = 1; l < layer3TempDataStruct.Length; l++)
                        {
                            TreeNode layer4TreeNode = new TreeNode();
                            layer4TreeNode.Text = layer3TempDataStruct[l].name;
                            layer4TreeNode.Tag = layer3TempDataStruct[l].type;
                            layer4TreeNode.ForeColor = Color.Red;
                            layer3TreeNode.Nodes.Add(layer4TreeNode);
                        }
                    }
                }
            }
            return itemTreeNodeList.ToArray();
        }

        /// <summary>
        /// 编辑的目标节点
        /// </summary>
        TreeNode treeNode;

        /// <summary>
        /// 合成数据对象
        /// </summary>
        SynthesisDataStruct synthesisDataStruct;

        public DataEditorForm(TreeNode treeNode)
        {
            InitializeComponent();
            this.treeNode = treeNode;
            this.synthesisDataStruct = treeNode.Tag as SynthesisDataStruct;
            if (itemTreeNodeArray == null)
                itemTreeNodeArray = GetItemTypeTreeNodes();
            TreeView_ItemType.Nodes.AddRange(itemTreeNodeArray);
            Init();

            GroupBox_From.AllowDrop = true;
            GroupBox_To.AllowDrop = true;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            TextBox_ID.Text = synthesisDataStruct.id.ToString();
            TextBox_Name.Text = synthesisDataStruct.name == null ? "" : synthesisDataStruct.name;
            TextBox_Name.TextChanged += (sender, e) => { synthesisDataStruct.name = TextBox_Name.Text.Trim(); };
            TextBox_SynthesisType.Text = treeNode.Parent.Parent.Text;
            TextBox_SynthesisItem.Text = treeNode.Parent.Text;
            NumericUpDown_Time.Value = synthesisDataStruct.time;
            NumericUpDown_Time.ValueChanged += (sender, e) => { synthesisDataStruct.time = (int)NumericUpDown_Time.Value; };
            NumericUpDown_Level.Value = synthesisDataStruct.level;
            NumericUpDown_Level.ValueChanged += (sender, e) => { synthesisDataStruct.level = (int)NumericUpDown_Level.Value; };
            if (synthesisDataStruct.inputStruct == null)
                synthesisDataStruct.inputStruct = new SynthesisDataStruct.SynthesisItemStruct[0];
            if (synthesisDataStruct.inputStruct.Length > 0)
            {
                foreach (SynthesisDataStruct.SynthesisItemStruct itemStruct in synthesisDataStruct.inputStruct)
                {
                    AddDataToGroupBox_From(itemStruct);
                }
            }
            if (synthesisDataStruct.outputStruct != null)
                ChangedDataToGroupBox_To(synthesisDataStruct.outputStruct);
        }

        /// <summary>
        /// 给GroupBox_To控件更换一条数据
        /// </summary>
        /// <param name="itemStruct"></param>
        private void ChangedDataToGroupBox_To(SynthesisDataStruct.SynthesisItemStruct itemStruct)
        {
            GroupBox_To.Controls.Clear();
            SynthesisItemControl synthesisItemControl = new SynthesisItemControl(itemStruct);
            GroupBox_To.Controls.Add(synthesisItemControl);
            synthesisItemControl.Location = new Point(3, 23);
            synthesisItemControl.Size = new Size(GroupBox_From.Size.Width - 6, 30);
            synthesisItemControl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
        }

        /// <summary>
        /// 给GroupBox_From控件添加一条数据
        /// </summary>
        /// <param name="itemStruct"></param>
        private void AddDataToGroupBox_From(SynthesisDataStruct.SynthesisItemStruct itemStruct)
        {
            SynthesisDataStruct.SynthesisItemStruct _itemStruct = itemStruct;
            SynthesisItemControl synthesisItemControl = new SynthesisItemControl(_itemStruct);
            Button closeBtn = new Button();
            closeBtn.Text = "×";
            closeBtn.Click += (sender, e) =>
            {
                GroupBox_From.Controls.Remove(synthesisItemControl);
                GroupBox_From.Controls.Remove(closeBtn);
                synthesisDataStruct.inputStruct = synthesisDataStruct.inputStruct.Where(temp => !temp.Equals(_itemStruct)).ToArray();
                UpdateGroupBox_From();
            };
            synthesisItemControl.Tag = closeBtn;
            closeBtn.Tag = synthesisItemControl;
            GroupBox_From.Controls.Add(synthesisItemControl);
            GroupBox_From.Controls.Add(closeBtn);
            UpdateGroupBox_From();
        }

        /// <summary>
        /// 刷新GroupBox_From界面
        /// </summary>
        private void UpdateGroupBox_From()
        {
            SynthesisItemControl[] synthesisItemControls = GroupBox_From.Controls.OfType<SynthesisItemControl>().ToArray();
            int height = 20;
            foreach (SynthesisItemControl synthesisItemControl in synthesisItemControls)
            {
                height += 3;
                synthesisItemControl.Location = new Point(3, height);
                synthesisItemControl.Size = new Size(GroupBox_From.Size.Width - (9 + 30), 30);
                synthesisItemControl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                Button btn = synthesisItemControl.Tag as Button;
                btn.Location = new Point(GroupBox_From.Size.Width - (3 + 30), height);
                btn.Size = new Size(30, 30);
                btn.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                height += 30;
            }
        }

        /// <summary>
        /// 关闭窗口
        /// 清空树并刷新节点的显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TreeView_ItemType.Nodes.Clear();
            treeNode.Text = synthesisDataStruct.ToStringSimple();
        }

        /// <summary>
        /// 点击给From控件添加一条数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_GiveFrom_Click(object sender, EventArgs e)
        {
            TreeNode selectItemTypeNode = TreeView_ItemType.SelectedNode;
            AddNewItemStructToGirveFromByTreeNode(selectItemTypeNode);
        }

        /// <summary>
        /// 通过一个选中节点添加一个输入结构
        /// </summary>
        /// <param name="selectItemTypeNode">选中节点</param>
        private void AddNewItemStructToGirveFromByTreeNode(TreeNode selectItemTypeNode)
        {
            if (selectItemTypeNode != null && selectItemTypeNode.Tag.GetType().Equals(typeof(EnumGoodsType)) && Color.Equals(selectItemTypeNode.ForeColor, Color.Red))
            {
                SynthesisDataStruct.SynthesisItemStruct itemStruct = new SynthesisDataStruct.SynthesisItemStruct();
                itemStruct.itemType = (EnumGoodsType)selectItemTypeNode.Tag;
                itemStruct.num = 1;
                itemStruct.minQuality = EnumQualityType.White;
                itemStruct.maxQuality = EnumQualityType.White;
                if (synthesisDataStruct.inputStruct == null)
                {
                    synthesisDataStruct.inputStruct = new SynthesisDataStruct.SynthesisItemStruct[1];
                    synthesisDataStruct.inputStruct[0] = itemStruct;
                }
                else
                {
                    //如果已经存在相同的材料则直接返回
                    if (synthesisDataStruct.inputStruct.Count(temp => temp.itemType == itemStruct.itemType) > 0)
                    {
                        MessageBox.Show("已经存在该材料");
                        return;
                    }
                    else
                    {
                        SynthesisDataStruct.SynthesisItemStruct[] tempArray = new SynthesisDataStruct.SynthesisItemStruct[synthesisDataStruct.inputStruct.Length + 1];
                        Array.Copy(synthesisDataStruct.inputStruct, tempArray, synthesisDataStruct.inputStruct.Length);
                        tempArray[tempArray.Length - 1] = itemStruct;
                        synthesisDataStruct.inputStruct = tempArray;
                    }
                }
                AddDataToGroupBox_From(itemStruct);
            }
        }

        /// <summary>
        /// 点击给To控件替换或者添加一条数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_GiveTo_Click(object sender, EventArgs e)
        {
            TreeNode selectItemTypeNode = TreeView_ItemType.SelectedNode;
            ChangedItemStructToGirveToByTreeNode(selectItemTypeNode);
        }

        /// <summary>
        /// 通过一个选中节点替换或添加一个输出结构
        /// </summary>
        /// <param name="selectItemTypeNode"></param>
        private void ChangedItemStructToGirveToByTreeNode(TreeNode selectItemTypeNode)
        {
            if (selectItemTypeNode != null && selectItemTypeNode.Tag.GetType().Equals(typeof(EnumGoodsType)) && Color.Equals(selectItemTypeNode.ForeColor, Color.Red))
            {
                SynthesisDataStruct.SynthesisItemStruct itemStruct = new SynthesisDataStruct.SynthesisItemStruct();
                itemStruct.itemType = (EnumGoodsType)selectItemTypeNode.Tag;
                itemStruct.num = 1;
                itemStruct.minQuality = EnumQualityType.White;
                itemStruct.maxQuality = EnumQualityType.White;
                if (synthesisDataStruct.outputStruct == null)
                    synthesisDataStruct.outputStruct = itemStruct;
                else
                {
                    if (synthesisDataStruct.outputStruct.itemType == itemStruct.itemType)
                    {
                        MessageBox.Show("已经存在该材料");
                        return;
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show("是否替换材料？", "警告！", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.No)
                            return;
                        synthesisDataStruct.outputStruct = itemStruct;
                    }
                }
                ChangedDataToGroupBox_To(itemStruct);
            }
        }


        #region 拖拽操作
        /// <summary>
        /// 开始拖拽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_ItemType_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode selectItemTypeNode = TreeView_ItemType.SelectedNode;
            if (selectItemTypeNode != null && selectItemTypeNode.Tag.GetType().Equals(typeof(EnumGoodsType)) && Color.Equals(selectItemTypeNode.ForeColor, Color.Red))
            {
                TreeView_ItemType.DoDragDrop(selectItemTypeNode, DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// 数据拖拽进输入数据控件中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupBox_From_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                DataObject dataObject = e.Data as DataObject;
                if (dataObject != null)
                {
                    object value = dataObject.GetData(typeof(TreeNode));
                    if (value != null)
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                }
            }
            catch { }
        }


        /// <summary>
        /// 拖拽操作在输入控件中完成时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupBox_From_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                DataObject dataObject = e.Data as DataObject;
                if (dataObject != null)
                {
                    object value = dataObject.GetData(typeof(TreeNode));
                    if (value != null)
                    {
                        TreeNode treeNode = value as TreeNode;
                        AddNewItemStructToGirveFromByTreeNode(treeNode);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 数据拖拽进输入数据控件中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupBox_To_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                DataObject dataObject = e.Data as DataObject;
                if (dataObject != null)
                {
                    object value = dataObject.GetData(typeof(TreeNode));
                    if (value != null)
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 拖拽操作在输入控件中完成时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupBox_To_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                DataObject dataObject = e.Data as DataObject;
                if (dataObject != null)
                {
                    object value = dataObject.GetData(typeof(TreeNode));
                    if (value != null)
                    {
                        TreeNode treeNode = value as TreeNode;
                        ChangedItemStructToGirveToByTreeNode(treeNode);
                    }
                }
            }
            catch { }
        }
        #endregion


    }
}
