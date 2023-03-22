using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Project_try3.Models
{
    public class GetData
    {
        //1.建立資料庫連線物件( SqlConnection)
        //把web.config當資料庫，connectionString當欄位,讀取NorthwindConnection裡的資料
        //要建立成static方便各Action取用 
        static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Project_V3Connection"].ConnectionString);
        //2.建立SQL命令物件(SqlCommand)
        //第一個參數利用下面方法傳值
        SqlCommand cmd = new SqlCommand("", conn);
        //3.建立資料讀取物件
        //SqlDataReader rd;

        //SqlDataAdapter只能讀取select無法做增刪修，要做增刪修只能使用SqlCommand
        SqlDataAdapter adp = new SqlDataAdapter("", conn);
        DataSet ds = new DataSet();
        //要讀取table
        DataTable dt = new DataTable();

        public DataTable TableQuery(string sql)
        {
            //sql = "select * from Orders";
            adp.SelectCommand.CommandText = sql;//指定Select Command
            adp.Fill(ds);//把取得的table放到dataset
            dt = ds.Tables[0];
            return dt;
        }
        //多載：以相同的方法名稱呼叫，在參數個數不同或參數內容不同的情況下可使用不同的方法
        public DataTable TableQuery(string sql, List<SqlParameter> para)
        {
            //sql = "select * from Orders";
            adp.SelectCommand.CommandText = sql;//指定Select Command
            foreach (SqlParameter p in para)
            {
                adp.SelectCommand.Parameters.Add(p);
            }
            adp.Fill(ds);//把取得的table放到dataset
            dt = ds.Tables[0];
            return dt;
        }
        //使用SQL 預存函式來寫：要多寫commandType
        public DataTable TableQueryBySP(string sql)
        {
            //sql = "select * from Orders";
            adp.SelectCommand.CommandText = sql;//指定Select Command
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;//使用資料庫的StoredProcedure
            adp.Fill(ds);//把取得的table放到dataset
            dt = ds.Tables[0];
            return dt;
        }
        public DataTable TableQueryBySP(string sql, List<SqlParameter> para)
        {
            //sql = "select * from Orders";
            adp.SelectCommand.CommandText = sql;//指定Select Command
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;//使用資料庫的StoredProcedure
            foreach (SqlParameter p in para)
            {
                adp.SelectCommand.Parameters.Add(p);
            }
            adp.Fill(ds);//把取得的table放到dataset
            if (ds.Tables.Count == 0)//如果dataset讀不到值
                                     //"回應一個狀態沒有資料內容"
                return dt;
            dt = ds.Tables[0];
            return dt;
        }


    }
}