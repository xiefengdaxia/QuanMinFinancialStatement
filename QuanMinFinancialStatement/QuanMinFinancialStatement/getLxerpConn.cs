using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace QuanMinFinancialStatement
{
    class getLxerpConn
    {
        //public string sql="select  a.cardno ,a.cname as 姓名 , case a.status when '7' then '取消' when '9' then '已付款' when '0' then '预定' end as 状态,a.res_date ,a.res_time,c.cname,gym_item.item_name ,a.groups ,a.person ,b.USER_NAMEC,a.op_time,a.memo   FROM gym_item , gym_reserve as a,sys_user  as b ,gym_kind as c where  gym_item.item_code = a.res_item and a.operator=b.empid and c.code=a.time_mode ";
        private string sql = "select a.sid, a.cardno ,a.cname as 姓名 , case a.status when '7' then '取消' when '9' then '已付款' when '0' then '预定' end as 状态,a.res_date ,a.res_time,c.cname,gym_item.item_name ,a.groups ,a.person ,b.USER_NAMEC,a.op_time,a.memo   FROM gym_item , gym_reserve as a,sys_user  as b ,gym_kind as c where  gym_item.item_code = a.res_item and a.operator=b.empid and c.code=a.time_mode ";

        public string Sql
        {
            get { return sql; }
            set { sql = value; }
        }

        private string sqlconn;

        public string Sqlconn
        {
            get { return sqlconn; }
            set { sqlconn = value; }
        }

        #region "声明变量"

        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        private string strFilePath = Application.StartupPath + "\\lxerp.ini";//获取INI文件路径
        private string strSec = ""; //INI文件名

        #endregion

        #region "自定义读取INI文件中的内容方法"
        /// <summary>
        /// 自定义读取INI文件中的内容方法
        /// </summary>
        /// <param name="Section">键</param>
        /// <param name="key">值</param>
        /// <returns></returns>
        private string ContentValue(string Section, string key)
        {

            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, strFilePath);
            return temp.ToString();
        }
        #endregion

        #region "获取sqlconn连接参数"
        public void getsqlconn()
        {
            if (File.Exists(strFilePath))//读取时先要判读INI文件是否存在
            {

                //strSec = Path.GetFileNameWithoutExtension(strFilePath);
                strSec = "Connection";
                string Database = ContentValue(strSec, "Database");
                string ServerName = ContentValue(strSec, "ServerName");
                string LogId = ContentValue(strSec, "LogId");
                string Logpass = ContentValue(strSec, "Logpass");
                sqlconn = @"server=" + ServerName + ";database = " + Database + "; uid =" + LogId + "; pwd = " + Logpass + ";";
            }
        }
        #endregion
    }
}
