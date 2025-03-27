using Microsoft.Extensions.DependencyInjection;

namespace StockConsole;

public static class Client
{
    public static void StartConsoleApplication(ServiceProvider serviceProvider)
    {
        var stockPriceService = serviceProvider.GetRequiredService<IAlphaVantageApiService>();

        Console.WriteLine("Which stock do you want to know the price of?");
        var stockSymbol = Console.ReadLine()!;
        var price = stockPriceService.GetMostRecentPrice(stockSymbol);
        Console.WriteLine($"The last price is {price}");
    }
    
}