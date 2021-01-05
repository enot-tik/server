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
    public class Connect
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
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static List<Connect> connects = new List<Connect> { };


        [HttpGet]
        public string Welcome()
        {
            return "hello";
        }

        public bool accept(string token)
        {
            Connect con = connects.FirstOrDefault(x => x.token == token);
            if (con == null) return false;
            if (con.accept.AddHours(10) > DateTime.Now) {
                connects.Remove(con);
                return false;
            }
            return true;
        }

        [HttpPost("auth/{password}")]
        public string auth(string password) => Tools.GenerationToken(password);

    }
}
