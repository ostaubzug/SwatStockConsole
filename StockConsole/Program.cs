using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StockConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
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

      
    }
}
