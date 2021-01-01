using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;

namespace server.Handler
{
    class Connect
    {
        public int id { get; set; }
        public string token { get; set; }
        public DateTime accept { get; set; }

        public Connect(int id, string token, DateTime accept)
        {
            this.id = id;
            this.token = token;
            this.accept = accept;
        }
    }

    [ApiController]
    [Route("api")]
    public class Main : ControllerBase
    {
        private Query query = new Query();
        private static Random rnd = new Random();
        private static List<Connect> connects = new List<Connect> { };


        [HttpGet]
        public string Welcome()
        {
            return "hello";
        }

        [HttpPost("auth/{password}")]
        public string auth(string password)
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
                        if (connects.FirstOrDefault(x => x.id == id) != null) return "session exists";
                        byte length = Convert.ToByte((Convert.ToInt32(Row["level"]) == 0) ? rnd.Next(30, 40) : rnd.Next(42, 65));
                        string chars = "qwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASDFGHJKLZXCVBNM_$%";
                        StringBuilder builder = new StringBuilder(length);
                        for (int i = 0; i < length; ++i) 
                            builder.Append(chars[rnd.Next(chars.Length)]);
                        connects.Add(new Connect(id, builder.ToString(), DateTime.Now));
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
