using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanMinFinancialStatement
{
    public partial class CheckedComboBoxWithLable :UserControl
    {
        public CheckedComboBoxWithLable()
        {
            InitializeComponent();
        }
        public string comboxText
        {
            get { return checkedComboBox1.Text; }
            set { checkedComboBox1.Text = value; }
        }
        public string LableText
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                for (int i = 0; i < checkedComboBox1.Items.Count; i++)
                {
                    checkedComboBox1.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < checkedComboBox1.Items.Count; i++)
                {
                    checkedComboBox1.SetItemChecked(i, false);
                }
            }
        }
        public void Add_CB_Item(string itemName)
        {
            checkedComboBox1.Items.Add(itemName);
        }
        public void ClearItem()
        {
            checkedComboBox1.Items.Clear();
        }
        /// <summary>
        /// 将自定义控件恢复默认设置
        /// </summary>
        public void reset()
        {
            for (int i = 0; i < checkedComboBox1.Items.Count; i++)
            {
                checkedComboBox1.SetItemChecked(i, false);
            }
            checkBox1.Checked = false;
        }
    }
}
