using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using StockConsole.Services;

namespace StockConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            
            var logger = new LoggerConfiguration().WriteTo.File("/var/logs/stockconsole.log").CreateLogger();
            
            CreateHostBuilder(args, logger).Build().Run();
            
            DotNetEnv.Env.Load();
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
            
            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<IAlphaVantageApiService, AlphaVantageApiService>();
            services.AddSingleton<IChartService, ChartService>();
            
            var serviceProvider = services.BuildServiceProvider();
            Client.StartConsoleApplication(serviceProvider);
            
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args, ILogger logger) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog(logger)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<StartupBase>();
                });

      
    }
}
