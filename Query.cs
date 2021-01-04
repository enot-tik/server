using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace server
{
    public class Query
    {
        public MySqlConnection mycon;
        public MySqlCommand mycom;
        public DataSet ds;

        public Query()
        {
            string constr = $"Server={Config.Data.cfg.DBHost};" +
                                                "Database=tik;" +
                                                "User=root;";
            mycon = new MySqlConnection(constr);
            Console.WriteLine("Database open");
            mycon.Open();
        }

        public string GetString(string sql) => new MySqlCommand(sql, mycon).ExecuteScalar().ToString();
        public DataTable GetDataTable(string sql) {
            DataTable res = new DataTable();
            res.Load(new MySqlCommand(sql, mycon).ExecuteReader());
            return res;
        }
        public void Send(string sql)
        {
            new MySqlCommand(sql, mycon).ExecuteNonQuery();
            
        }

    }
}