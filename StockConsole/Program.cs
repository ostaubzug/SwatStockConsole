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
            // Load environment variables first
            DotNetEnv.Env.Load();
            
            // Configure and build the host
            using IHost host = CreateHostBuilder(args).Build();
            
            // Run your console application logic
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            Client.StartConsoleApplication(services);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, loggerConfiguration) => 
                {
                    loggerConfiguration.WriteTo.File("/var/logs/stockconsole.log");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddSingleton<IAlphaVantageApiService, AlphaVantageApiService>();
                    services.AddSingleton<IChartService, ChartService>();
                });
    }
}