using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanMinFinancialStatement
{
    public partial class Form1 : DevComponents.DotNetBar.RibbonForm
    {
        //private string[] coloursArr = { "Red", "Green", "Black", "White", "Orange", "Yellow", "Blue", "Maroon", "Pink", "Purple" };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //for (int i = 0; i < coloursArr.Length; i++)
            //{
            //    CCBoxItem item = new CCBoxItem(coloursArr[i], i);
            //    checkedComboBox1.Items.Add(item);
            //}
            //把时间控件设为当月1号
            DTbegin.Text = DTbegin.Text.Substring(0, 8) + "1";
            setCombo("select user_namec from sys_user", checkedComboBox1);
            //checkedComboBox1.MaxDropDownItems = 50;
            //checkedComboBox1.DisplayMember = "Name";
            checkedComboBox1.ValueSeparator = ",";
        }
        #region 设置combox的值方法，参数(sql,cmb)
        //设置combox的值--------------
        private void setCombo(string sql, CheckedComboBox cmb)
        {
            //清空下拉列表
            cmb.Items.Clear();
            //cmb.Items.Add("%");
            //cmb.Text = "%";
            //实例化一个类
            getLxerpConn v = new getLxerpConn();
            //获取类里面的sql连接到数据库的字符串
            v.getsqlconn();
            //  string connStr = "Data Source=.\\sql2008;Initial Catalog=books;Integrated Security=True"; //连接到数据库的字符串
            SqlConnection cn = new SqlConnection(v.Sqlconn);
            //打开链接
            cn.Open();
            SqlCommand cmd = cn.CreateCommand();
            cmd.CommandText = sql;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cmb.Items.Add(dr.GetValue(0).ToString().Trim());
            }
            cn.Close();
            cn.Dispose();
        }
        #endregion

        #region 根据sql显示报表
        public void baobiao(string cmdsql, string reportName)
        {
            getLxerpConn v = new getLxerpConn();
            v.getsqlconn();
            //声明连接、命令对象及其他相关对象
            SqlConnection conReport = new SqlConnection(v.Sqlconn);
            SqlCommand cmdReport = new SqlCommand();
            SqlDataReader drReport;
            DataSet dsReport = new dsQuan();

            try
            {
                //打开连接
                conReport.Open();

                //准备连接对象以把获取的数据放入数据集

                cmdReport.CommandType = CommandType.Text;
                cmdReport.Connection = conReport;
                cmdReport.CommandText = cmdsql;

                //从命令对象中读取数据
                drReport = cmdReport.ExecuteReader();

                //有了ADO.NET，可把读取来的数据直接加载到数据集中

                dsReport.Tables[0].Load(drReport);

                //关闭读取及连接
                drReport.Close();
                conReport.Close();

                //为查看器提供本地报表数据
                reportViewer1.LocalReport.ReportEmbeddedResource = reportName;
                //准备报表数据源
                ReportDataSource rds = new ReportDataSource();
                rds.Name = "dsQuan";
                rds.Value = dsReport.Tables[0];


                //clear非常重要，不加没数据，坑死人
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
                ReportParameter canshu = new ReportParameter("F_date_to_date", "【从】" +DTbegin.Text + "【到】" +DTend.Text);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { canshu });

                //加载报表查看器
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                //显示错误信息
                MessageBox.Show(ex.Message+"\n"+ex.StackTrace);
            }
            finally
            {
                //检查连接是否仍然打开，如果是，关闭它。
                if (conReport.State == ConnectionState.Open)
                {
                    conReport.Close();
                }
            }
        }

        #endregion

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string sql = @"select  c.name as 会籍种类 ,count(*) as 数量 ,sum(a.totalpayable) as 合计金额 from pos_bills a 
                left join mem_member b on a.memno=b.m_id 
                left join mem_kind c on c.memberkind=b.memberkind
                left join sys_user d on a.openuserid=d.empid
                left join sys_workshop e on a.billshop=e.shop_code
                right join (select distinct billcode from pos_sales where itemcode  in('99995','99987','99996') and rec_status=1) f on f.billcode=a.billcode 
                where 1=1 and a.paycode in('$91','$93','$95')  and d.user_namec like '%%%' and e.namec like '%%%' and a.closedate>='2015-01-01' and  a.closedate <= '2016-01-18' and a.bill_status = 1 group by c.name  ";
            string rpName = "QuanMinFinancialStatement.业务出纳表.rdlc";
            baobiao(sql, rpName);
        }
    }
}
