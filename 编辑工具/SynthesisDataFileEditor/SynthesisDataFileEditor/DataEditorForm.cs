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
            Type itemTypeType = typeof(EnumItemType);
            Dictionary<EnumItemType, FieldExplanAttribute> tempExplanDic = itemTypeType.GetFields().Where(temp => !temp.Name.Equals("value__"))
                .ToDictionary(temp => (EnumItemType)Enum.Parse(typeof(EnumItemType), temp.Name), temp => temp.GetCustomAttributes(typeof(FieldExplanAttribute), false).FirstOrDefault())
                .Where(temp => temp.Value != null)
                .ToDictionary(temp => temp.Key, temp => temp.Value as FieldExplanAttribute);

            var tempDataStruct = Enum.GetValues(typeof(EnumItemType)).OfType<EnumItemType>()
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
                        for (int l = 1;  l < layer3TempDataStruct.Length;  l++)
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


        public DataEditorForm(TreeNode treeNode)
        {
            InitializeComponent();
            this.treeNode = treeNode;
            if (itemTreeNodeArray == null)
                itemTreeNodeArray = GetItemTypeTreeNodes();
        }
    }
}
