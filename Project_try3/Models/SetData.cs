using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace Project_try3.Models
{//select用
    public class SetData
    {
        //1.建立資料庫連線物件( SqlConnection)
        
        static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Project_V3Connection"].ConnectionString);
        //2.建立SQL命令物件(SqlCommand)
             SqlCommand cmd = new SqlCommand("", conn);
        //3.建立存取方法
        //沒有傳參數
        public void executeSql(string Sql)
        {
            cmd.CommandText = Sql;
            conn.Open();
            //寫入資料，如果讀取是ExecuteReader
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
        }
        //有傳參數，但因為不能確定參數有多少個所以使用list泛型存放
        /// <summary>
        /// 必須傳入SqlParameter List泛型參數
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="list"></param>
        public void executeSql(string Sql,List<SqlParameter>list)
        {
            cmd.CommandText = Sql;
            foreach (var p in list)
            {
                cmd.Parameters.Add(p);   
            }
           
            conn.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
        }
        
    }
}