namespace GoodsEditor
{
    partial class GoodsInfoMation
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsBaseTypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.GoodsFuncTypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column1,
            this.Column6,
            this.Column7,
            this.GoodsBaseTypeColumn,
            this.GoodsFuncTypeColumn,
            this.Column5,
            this.Column8});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(1200, 600);
            this.dataGridView1.TabIndex = 0;
            // 
            // Column2
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column2.HeaderText = "物品ID";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "物品名称";
            this.Column1.Name = "Column1";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "基础重量";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "基础价格";
            this.Column7.Name = "Column7";
            // 
            // GoodsBaseTypeColumn
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            this.GoodsBaseTypeColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.GoodsBaseTypeColumn.DropDownWidth = 100;
            this.GoodsBaseTypeColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GoodsBaseTypeColumn.HeaderText = "物品基础类型";
            this.GoodsBaseTypeColumn.Items.AddRange(new object[] {
            "1",
            "2"});
            this.GoodsBaseTypeColumn.Name = "GoodsBaseTypeColumn";
            this.GoodsBaseTypeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.GoodsBaseTypeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.GoodsBaseTypeColumn.Width = 130;
            // 
            // GoodsFuncTypeColumn
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            this.GoodsFuncTypeColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.GoodsFuncTypeColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GoodsFuncTypeColumn.HeaderText = "物品功能类型";
            this.GoodsFuncTypeColumn.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.GoodsFuncTypeColumn.Name = "GoodsFuncTypeColumn";
            this.GoodsFuncTypeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.GoodsFuncTypeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.GoodsFuncTypeColumn.Width = 130;
            // 
            // Column5
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Column5.HeaderText = "物品具体类型";
            this.Column5.Name = "Column5";
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column5.Width = 130;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "物品说明";
            this.Column8.Name = "Column8";
            this.Column8.Width = 400;
            // 
            // GoodsInfoMation
            // 
            this.Controls.Add(this.dataGridView1);
            this.Name = "GoodsInfoMation";
            this.Size = new System.Drawing.Size(1200, 600);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewComboBoxColumn GoodsBaseTypeColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn GoodsFuncTypeColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}
