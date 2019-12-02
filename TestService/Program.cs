using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace TestService
{
    public class Program
    {

        public static NLog.Logger Logger { get; set; }
        public static IConfiguration Cnf { get; set; }

        public static void Main(string[] args)
        {
            Logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                Logger.Info("Запуск тестового сервиса");
                Cnf = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

                CreateWebHostBuilder(args).Build().Run();
            }
            catch(Exception ex)
            {
                Logger.Error(ex, "Тестовый сервис остановлен");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                                            .ConfigureLogging(logging =>
                                            {
                                                logging.ClearProviders();
                                                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                                            })
                .UseNLog()
                .UseUrls(Cnf["ip"])
                .UseStartup<Startup>();
    }
}
