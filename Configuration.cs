using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace server.Config 
{
    public class Cfg
    {
        public string DBHost;
        public string vkToken;
    }
    public class Data
    {
        public  static Cfg cfg = new Cfg();
        public static void Init()
        {
            cfg = JsonConvert.DeserializeObject<Cfg>(File.ReadAllText("json.json"));
            
        }

    }
}
