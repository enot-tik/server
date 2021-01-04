using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("_________------------====================------------_________");
            Thread thread2 = new Thread(CreateHostBuilder(args).Build().Run);
            Thread thread1 = new Thread(Handler.VKSide.Init);
            Config.Data.Init();
            Console.WriteLine(Config.Data.cfg.DBHost);
            thread2.Start();
            thread1.Start();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
