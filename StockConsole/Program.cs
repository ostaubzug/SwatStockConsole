using System;

namespace StockConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var httpClient = new HttpClient();
            var service = new StockPriceService(new AlphaVantageApiService());
            Console.WriteLine("Which stock do you want to know the price of?");

            string stockSymbol = Console.ReadLine()!;
            var price = await service.GetMostRecentPrice(stockSymbol);

            Console.WriteLine($"The last price is {price}");
        }
    }
}
