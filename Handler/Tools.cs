using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Handler
{
    public class Tools
    {
        private static Random rnd = new Random();
        public static Query query = new Query();
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static string GenerationToken(string password)
        {
            DataTable res = query.GetDataTable("SELECT * FROM workers");
            if (res == null) return "w/o workers";
            foreach (DataRow Row in res.Rows)
            {
                string pas = Convert.ToString(Row["password"]);
                try
                {
                    if (password == pas)
                    {
                        int id = Convert.ToInt32(Row["id"]);
                        if (Main.connects.FirstOrDefault(x => x.id == id) != null) return "session exists";
                        byte length = Convert.ToByte((Convert.ToInt32(Row["level"]) == 0) ? rnd.Next(30, 40) : rnd.Next(42, 65));
                        string chars = "qwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASDFGHJKLZXCVBNM_$%";
                        StringBuilder builder = new StringBuilder(length);
                        for (int i = 0; i < length; ++i)
                            builder.Append(chars[rnd.Next(chars.Length)]);
                        Main.connects.Add(new Connect(id, builder.ToString(), DateTime.Now));
                        return builder.ToString();
                    }

                }
                catch
                {
                    // TODO: log system
                }
            }

            return "noncorrect";
        }
    }
}
