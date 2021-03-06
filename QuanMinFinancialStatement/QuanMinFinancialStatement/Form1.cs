﻿using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace QuanMinFinancialStatement
{
    public partial class Form1 : DevComponents.DotNetBar.RibbonForm
    {
        //private string[] coloursArr = { "Red", "Green", "Black", "White", "Orange", "Yellow", "Blue", "Maroon", "Pink", "Purple" };

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            //circularProgress1.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //把时间控件设为当月1号
            DTbegin.Text = DTbegin.Text.Substring(0, 8) + "1";


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
        private void setCombo(string sql, ComboBox cmb)
        {
            //清空下拉列表
            cmb.Items.Clear();
            cmb.Items.Add("%");
            cmb.Text = "%";
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
        private void setCombo(string sql, CheckedComboBoxWithLable cmb)
        {
            //清空下拉列表
            cmb.ClearItem();
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
                cmb.Add_CB_Item(dr.GetValue(0).ToString().Trim());
            }
            cn.Close();
            cn.Dispose();
        }
        #endregion

        #region 根据sql显示报表
        public void baobiao(string cmdsql, string reportName)
        {
            circularProgress1.Visible = true;
            circularProgress1.IsRunning = true;
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
                ReportParameter canshu = new ReportParameter("F_date_to_date", "【从】" + DTbegin.Text + "【到】" + DTend.Text);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { canshu });
                //加载报表查看器
                reportViewer1.RefreshReport();

            }
            catch (Exception ex)
            {
                //显示错误信息
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                //检查连接是否仍然打开，如果是，关闭它。
                if (conReport.State == ConnectionState.Open)
                {
                    conReport.Close();
                }
                circularProgress1.IsRunning = false;
                circularProgress1.Visible = false;
            }
        }

        #endregion

        private void buttonX1_Click(object sender, EventArgs e)
        {
            Thread worker = new Thread(delegate()
            {
                try
                {
                    int ischecked = 0;
                    //检查是否有选中的报表
                    foreach (Control c in groupPanel1.Controls)
                    {
                        if (c is RadioButton && ((RadioButton)c).Checked == true)
                        {
                            ischecked++;
                        }
                    }
                    if (ischecked == 0)
                    {
                        MessageBox.Show("请选择一个报表!", "友情提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (radioButton5.Checked == true)
                    {
                        string sql = "";
                        sql +=
                        @"select  c.name as 会籍种类 ,count(*) as 数量 ,sum(a.totalpayable) as 合计金额 from pos_bills a 
                left join mem_member b on a.memno=b.m_id 
                left join mem_kind c on c.memberkind=b.memberkind
                left join sys_user d on a.openuserid=d.empid
                left join sys_workshop e on a.billshop=e.shop_code
                right join (select distinct billcode from pos_sales where itemcode  in ('99995','99987','99996') and rec_status=1) f on f.billcode=a.billcode 
                where 1=1 and a.paycode in('$91','$93','$95') ";
                        sql += "	AND d.user_namec IN " + comboxTosqlString(CheckedComboBoxWithLable);
                        sql += "AND (c.name is null or c.name IN " + comboxTosqlString(checkedComboBoxWithLable1);
                        sql += ")	AND e.namec LIKE '%" + cb.Text + "%'";
                        sql += "	AND a.closedate >= '" + DTbegin.Text + "'";
                        sql += "	AND a.closedate < '" + EndDatePlus1Day(DTend.Text) + "'";
                        sql += "	AND a.bill_status = 1 GROUP BY c.name";
                        string rpName = "QuanMinFinancialStatement.业务出纳表.rdlc";
                        baobiao(sql, rpName);
                    }
                    if (radioButton2.Checked == true)
                    {
                        string sql = @" select b.NAMEC 营业点, c.user_namec 收银员,sum(case when payamount>0 then payamount else 0 end)收入金额,sum(case when payamount<0 then payamount else 0 end) 退款金额,SUM(payamount) 合计金额 
                from pos_bill_payment a
                left join sys_workshop b on a.payshop=b.SHOP_CODE
                left join sys_user c on a.userid=c.empid
                right join (select distinct billcode from pos_sales where (itemcode  in('99995','99987','99996')or parent_kinds = '$76' )and rec_status=1 and pay_status=1) f on f.billcode=a.billcode ";
                        sql += "where 1=1 and a.payparent in ('0001','0002') and b.NAMEC like '%" + cb.Text + "%'";
                        sql += "	AND a.paydate>='" + DTbegin.Text + "' and a.paydate<'" + EndDatePlus1Day(DTend.Text) + "'";
                        sql += "   AND c.USER_NAMEC in " + comboxTosqlString(CheckedComboBoxWithLable);
                        sql += "group by b.NAMEC,c.user_namec order by b.NAMEC";
                        string rpName = "QuanMinFinancialStatement.各营业点账务统计表.rdlc";
                        baobiao(sql, rpName);
                    }
                    if (radioButton4.Checked == true)
                    {
                        string sql = @"select 
                c.namec as 营业点, 
                paymethod as 付款编码, 
                b.name_c as 付款方式, 
                sum(payamount) as 合计 
                from 
                pos_bill_payment a, 
                sys_payment b, 
                sys_workshop c 
                where 1=1 and
                a.paymethod = b.code 
                and a.payshop = c.shop_code ";
                        sql += "and a.paydate >= '" + DTbegin.Text + "' and a.paydate < '" + EndDatePlus1Day(DTend.Text) + "' ";
                        sql += "and c.namec LIKE '%" + cb.Text + "%'";
                        sql += @"and a.rec_status = 1 
                and a.payamount<>0
                group by 
                a.paymethod, 
                b.name_c, 
                c.namec, 
                a.payparent 
                having 
                payparent = '0006'
                order by c.namec,b.name_c";
                        string rpName = "QuanMinFinancialStatement.各营业点充值卡消费统计表.rdlc";
                        baobiao(sql, rpName);
                    }
                    if (radioButton3.Checked == true)
                    {
                        string sql = @"select 
                          m_id 卡号, 
                          (
                            select 
                              name 
                            from 
                              mem_kind 
                            where 
                              memberkind = a.memberkind
                          ) 会籍种类, 
                          namec 姓名, 
                          (
                            select 
                              sum(totalpayable) 
                            from 
                              pos_bills 
                            where 
                              paycode = '$93' 
                              and rec_status = 1 
                              and pay_status=1
                              and memno = a.m_id
                          ) 充值金额 ,
                          (
                            select 
                              sum(totalpayable) 
                            from 
                              pos_bills 
                            where 
                              paycode != '$93' 
                              and rec_status = 1 
                               and pay_status=1
                              and memno = a.m_id
                              and left(paymethod,1)='6'
                          ) 消费金额 ,
                          a.cardbalance 余额,
                          a.joindate 办卡日期
                        from 
                            mem_member a left join mem_kind b on a.memberkind=b.memberkind where a.joindate >= '" + DTbegin.Text + "' and a.joindate < '" + EndDatePlus1Day(DTend.Text) + "' and a.m_id like '%" + txtBox.Text + "%'";
                        sql += "AND b.name IN " + comboxTosqlString(CheckedComboBoxWithLable);
                        string rpName = "QuanMinFinancialStatement.充值卡收支平衡报表.rdlc";
                        baobiao(sql, rpName);
                    }
                    if (radioButton6.Checked == true)
                    {
                        string sql = @"select m_id 卡号,namec 姓名,(b.name)卡种,
                    (select SUM(time) from memcardlog where type=2 and memid=a.q_id) 充次,
                    (select SUM(time) from memcardlog where type=5 and memid=a.q_id) 扣次,
                     c.time 余次,  DATEDIFF ( d , a.joindate , a.closedate ) 天数,
                     convert(varchar,a.joindate,23) 办卡日期,convert(varchar,a.closedate,23) 截止日期,
                     (case when a.closedate>GETDATE() then '未过期' else '已过期' end) 是否过期 
                     from mem_member  a
                     inner join memcard c on a.q_id = c.memid 
                     left join mem_kind b on a.memberkind=b.memberkind where b.name IN " + comboxTosqlString(CheckedComboBoxWithLable);
                        sql += " and  a.joindate>='" + DTbegin.Text + "' and a.joindate<'" + EndDatePlus1Day(DTend.Text)+"'";
                        switch (cb.Text)
                        {
                            case "未过期": sql += "and a.closedate>GETDATE()"; break;
                            case "已过期": sql += "and a.closedate<=GETDATE()"; break;
                            default: ; ; break;
                        }
                        sql+="order by 卡种";
                        string rpName = "QuanMinFinancialStatement.次卡统计表.rdlc";
                        baobiao(sql, rpName);
                    }
                    if (radioButton7.Checked == true)
                    {
                        string sql = @" select m_id 卡号,namec 姓名,b.name 卡种, (select count (*) from fitness_checkin where cardno=a.m_id)来场次数, DATEDIFF ( d , a.joindate , a.closedate ) 天数,
                    convert(varchar,a.joindate,23) 办卡日期,convert(varchar,a.closedate,23) 截止日期,
                     (case when a.closedate>GETDATE() then '未过期' else '已过期' end) 是否过期  from  mem_member a
                     left join mem_kind b on a.memberkind=b.memberkind where b.name IN " + comboxTosqlString(CheckedComboBoxWithLable);
                        sql += "and  a.joindate>='" + DTbegin.Text + "' and a.joindate<'" + EndDatePlus1Day(DTend.Text) + "'";
                        switch (cb.Text)
                        {
                            case "未过期": sql += "and a.closedate>GETDATE()"; break;
                            case "已过期": sql += "and a.closedate<=GETDATE()"; break;
                            default: ; ; break;
                        }
                        sql+="order by 来场次数 desc";
                        string rpName = "QuanMinFinancialStatement.期限卡统计表.rdlc";
                        baobiao(sql, rpName);
                    }
                    if (radioButton8.Checked == true)
                    {
                        string sql = @" select e.NAMEC,f.USER_NAMEC,cardno 卡号,a.name 姓名,c.name 卡种,(select top 1 timebalance from memcardlog where cardno=a.cardno order by usertime desc) 余次, case when (DATEDIFF ( d , GETDATE() , b.closedate )<0 )then 0 else DATEDIFF ( d , GETDATE() , b.closedate ) end 天数,totalbalance 卡内余额,timebalance 退卡金额,usertime 退卡时间 from t_fsl_memquit a
                         left join mem_member b on a.cardno=b.m_id
                         left join mem_kind c on b.memberkind=c.memberkind
                         left join memcard d on b.q_id = d.memid 
                         left join sys_workshop e on a.workshop=e.SHOP_CODE
                         left join SYS_USER f on a.usercode=f.empid";
                        sql += " where a.usertime>='" + DTbegin.Text + "' and a.usertime<'" + EndDatePlus1Day(DTend.Text) + "'";
                        sql += " and c.name IN " + comboxTosqlString(CheckedComboBoxWithLable);
                        sql += " and f.USER_NAMEC in " + comboxTosqlString(checkedComboBoxWithLable1);
                        sql += " and e.NAMEC like '%"+cb.Text+"%'";
                        sql += " and a.cardno like '%" + txtBox.Text + "%'";
                        sql+=" order by usertime ";
                        string rpName = "QuanMinFinancialStatement.退卡记录表.rdlc";
                        baobiao(sql, rpName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                }

            });
            worker.Start();


        }

        DateTime dt;
        /// <summary>
        /// 原有日期加一天用作截止日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string EndDatePlus1Day(string date)
        {
            dt = Convert.ToDateTime(date).AddDays(1);
            return dt.ToString("yyyy-MM-dd");
        }

        private CheckedComboBox Add_CheckedComboBox(CheckedComboBox ccb, Control above_crl, string sql, string lableText)
        {
            ccb = new CheckedComboBox();
            setCombo(sql, ccb);
            ccb.Top = above_crl.Top + above_crl.Height + 13;
            ccb.Left = above_crl.Left;
            groupPanel2.Controls.Add(ccb);

            l = new Label();
            l.Text = lableText;
            l.Top = above_crl.Top + above_crl.Height + 16;
            l.Left = label1.Left;
            l.BackColor = Color.Transparent;
            groupPanel2.Controls.Add(l);

            return ccb;
        }
        private TextBox add_TextBox(TextBox txtbox, Control above_crl, string lableText)
        {
            txtbox = new TextBox();
            txtbox.Top = above_crl.Top + above_crl.Height + 13;
            txtbox.Left = DTend.Left;
            txtbox.Width = 119;
            txtbox.Text = "%";
            groupPanel2.Controls.Add(txtbox);
            l = new Label();
            l.Text = lableText;
            l.Top = above_crl.Top + above_crl.Height + 16;
            l.Left = label1.Left;
            l.BackColor = Color.Transparent;
            groupPanel2.Controls.Add(l);
            return txtbox;
        }
        //void l_DoubleClick(object sender, EventArgs e)
        //{
        //    for (int i = 0; i < ccb1.Items.Count; i++)
        //    {
        //        ccb1.SetItemChecked(i, true);
        //    }
        //}

        private ComboBox Add_ComboBox(ComboBox cb, Control above_crl, string sql, string lableText)
        {
            cb = new ComboBox();
            if (sql != null)
            {
                setCombo(sql, cb);
            }
            else
            {
                //cb.Text = "%";
                cb.Items.Clear();
                cb.Items.Add("%");
                cb.Items.Add("未过期");
                cb.Items.Add("已过期");
                cb.DropDownStyle =ComboBoxStyle.DropDownList;
                cb.SelectedIndex = 0;
            }
            cb.Top = above_crl.Top + above_crl.Height + 13;
            cb.Left = above_crl.Left;
            groupPanel2.Controls.Add(cb);

            l = new Label();
            l.Text = lableText;
            l.Top = above_crl.Top + above_crl.Height + 16;
            l.Left = label1.Left;
            l.BackColor = Color.Transparent;
            groupPanel2.Controls.Add(l);
            return cb;
        }
        private string comboxTosqlString(CheckedComboBoxWithLable cbwl)
        {
            //('99995','99987','99996')
            string sql_plus = "('" + cbwl.comboxText.Replace(",", "','") + "')";

            return sql_plus;
        }

        /// <summary>
        /// 清除非默认需要的控件
        /// </summary>
        private void clearControl()
        {
            groupPanel2.Controls.Remove(cb);
            groupPanel2.Controls.Remove(txtBox);
            groupPanel2.Controls.Remove(l);
            CheckedComboBoxWithLable.reset();
            checkedComboBoxWithLable1.reset();
            foreach (Control ctl in groupPanel2.Controls)
            {
                if (ctl is System.Windows.Forms.Label)
                {
                    if (!ctl.Text.Contains("日期"))
                    {
                        this.groupPanel2.Controls.Remove(ctl);
                    }
                }
                //if (ctl is System.Windows.Forms.TextBox || ctl is System.Windows.Forms.ComboBox || ctl is QuanMinFinancialStatement.CheckedComboBox)
                //{
                //    this.groupPanel2.Controls.Remove(ctl);
                //}
            }
            groupPanel2.Refresh();
        }
        //CheckedComboBox ccb1;
        //CheckedComboBox ccb2;
        ComboBox cb;
        TextBox txtBox;
        Label l;
        private void radioButton5_Click(object sender, EventArgs e)
        {
            clearControl();
            cb = Add_ComboBox(cb, DTend, "select NAMEC from sys_workshop", "营业点:");
            //ccb1 = Add_CheckedComboBox(ccb1, cb, "select user_namec from sys_user", "操作员:");
            //ccb2 = Add_CheckedComboBox(ccb2, ccb1, "select mem_kind.name from mem_kind", "卡 种:");
            CheckedComboBoxWithLable.LableText = "操作员:";
            setCombo("select user_namec from sys_user", CheckedComboBoxWithLable);

            checkedComboBoxWithLable1.LableText = "卡 种:";
            setCombo("select mem_kind.name from mem_kind", checkedComboBoxWithLable1);

            //cb.Visible = true;
            CheckedComboBoxWithLable.Visible = true;
            checkedComboBoxWithLable1.Visible = true;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            clearControl();
            cb = Add_ComboBox(cb, DTend, "select NAMEC from sys_workshop", "营业点:");
            //cb.Visible = true;
            CheckedComboBoxWithLable.Visible = true;
            checkedComboBoxWithLable1.Visible = false;
            CheckedComboBoxWithLable.LableText = "操作员:";
            setCombo("select user_namec from sys_user", CheckedComboBoxWithLable);
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            clearControl();
            cb = Add_ComboBox(cb, DTend, "select NAMEC from sys_workshop", "营业点:");
            //cb.Visible = true;
            CheckedComboBoxWithLable.Visible = false;
            checkedComboBoxWithLable1.Visible = false;
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            clearControl();
            CheckedComboBoxWithLable.LableText = "卡 种:";
            setCombo("select mem_kind.name from mem_kind", CheckedComboBoxWithLable);
            CheckedComboBoxWithLable.Visible = true;
            checkedComboBoxWithLable1.Visible = false;
            txtBox = add_TextBox(txtBox, CheckedComboBoxWithLable, "卡 号:");
        }

        private void radioButton6_Click(object sender, EventArgs e)
        {
            clearControl();
            CheckedComboBoxWithLable.LableText = "卡 种:";
            setCombo("select mem_kind.name from mem_kind", CheckedComboBoxWithLable);
            CheckedComboBoxWithLable.Visible = true;
            checkedComboBoxWithLable1.Visible = false;
            txtBox = add_TextBox(txtBox, CheckedComboBoxWithLable, "卡 号:");
            cb = Add_ComboBox(cb, txtBox, null, "过期?:");
        }

        private void radioButton7_Click(object sender, EventArgs e)
        {
            clearControl();
            CheckedComboBoxWithLable.LableText = "卡 种:";
            setCombo("select mem_kind.name from mem_kind", CheckedComboBoxWithLable);
            CheckedComboBoxWithLable.Visible = true;
            checkedComboBoxWithLable1.Visible = false;
            txtBox = add_TextBox(txtBox, CheckedComboBoxWithLable, "卡 号:");
            cb = Add_ComboBox(cb, txtBox, null, "过期?:");
        }

        private void radioButton8_Click(object sender, EventArgs e)
        {
            clearControl();
            CheckedComboBoxWithLable.LableText = "卡 种:";
            setCombo("select mem_kind.name from mem_kind", CheckedComboBoxWithLable);
            checkedComboBoxWithLable1.LableText = "操作员:";
            setCombo("select user_namec from sys_user", checkedComboBoxWithLable1);
            CheckedComboBoxWithLable.Visible = true;
            checkedComboBoxWithLable1.Visible = true;
            txtBox = add_TextBox(txtBox, checkedComboBoxWithLable1, "卡 号:");
            cb = Add_ComboBox(cb, DTend, "select NAMEC from sys_workshop", "营业点:");
        }

    }
}
