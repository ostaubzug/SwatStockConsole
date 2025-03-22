using System;

namespace StockConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var service = new APIService();
            Console.WriteLine("Which stock do you want to know the price of?");

            string stockSymbol = Console.ReadLine();
            var price = await service.GetLastPrice(stockSymbol);

            Console.WriteLine($"The last price is {price}");
        }
    }
}
