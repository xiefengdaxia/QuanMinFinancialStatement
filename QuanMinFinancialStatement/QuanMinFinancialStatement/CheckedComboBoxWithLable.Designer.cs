namespace QuanMinFinancialStatement
{
    partial class CheckedComboBoxWithLable
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkedComboBox1 = new QuanMinFinancialStatement.CheckedComboBox();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(177, 9);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(48, 16);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "全选";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "操作员:";
            // 
            // checkedComboBox1
            // 
            this.checkedComboBox1.CheckOnClick = true;
            this.checkedComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.checkedComboBox1.DropDownHeight = 1;
            this.checkedComboBox1.FormattingEnabled = true;
            this.checkedComboBox1.IntegralHeight = false;
            this.checkedComboBox1.Location = new System.Drawing.Point(50, 5);
            this.checkedComboBox1.Name = "checkedComboBox1";
            this.checkedComboBox1.Size = new System.Drawing.Size(121, 22);
            this.checkedComboBox1.TabIndex = 0;
            this.checkedComboBox1.ValueSeparator = ",";
            // 
            // CheckedComboBoxWithLable
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.checkedComboBox1);
            this.Name = "CheckedComboBoxWithLable";
            this.Size = new System.Drawing.Size(226, 35);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckedComboBox checkedComboBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
    }
}
