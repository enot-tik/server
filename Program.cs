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
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            Thread thread2 = new Thread(CreateHostBuilder(args).Build().Run);
            Thread thread1 = new Thread(Handler.VKSide.Init);

            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "logi.log" };
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logfile);
            NLog.LogManager.Configuration = config;
            Logger.Info("System start work");

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
