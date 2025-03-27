using Microsoft.Extensions.DependencyInjection;

namespace StockConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddSingleton<IAlphaVantageApiService, AlphaVantageApiService>();
            
            var serviceProvider = services.BuildServiceProvider();
            Client.StartConsoleApplication(serviceProvider);
            
        }

      
    }
}
