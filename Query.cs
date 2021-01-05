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
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public MySqlConnection mycon;
        public MySqlCommand mycom;
        public DataSet ds;

        public Query()
        {
            string constr = $"Server={Config.Data.cfg.DBHost};" +
                                                "Database=tik;" +
                                                "User=root;";
            mycon = new MySqlConnection(constr);
            mycon.Open();
        }

        public string GetString(string sql) 
        {
            try
            {
                return new MySqlCommand(sql, mycon).ExecuteScalar().ToString();
            }
            catch (Exception e) { Logger.Error($"EXCEPTION AT 'GetString'({sql}):\n{e.ToString()}"); }
            return null;
        }
        public DataTable GetDataTable(string sql) {
            try 
            { 
                DataTable res = new DataTable();
                res.Load(new MySqlCommand(sql, mycon).ExecuteReader());
                return res;
            }
            catch (Exception e) { Logger.Error($"EXCEPTION AT 'GetDataTable'({sql}):\n{e.ToString()}"); }
            return null;
        }
        public void Send(string sql)
        {
            try
            { 
                new MySqlCommand(sql, mycon).ExecuteNonQuery();
            }
            catch (Exception e) { Logger.Error($"EXCEPTION AT 'Send'({sql}):\n{e.ToString()}"); }
            
        }

    }
}