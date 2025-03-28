using Microsoft.Extensions.DependencyInjection;
using StockConsole.Services;

namespace StockConsole;

public static class Client
{
    public static void StartConsoleApplication(ServiceProvider serviceProvider)
    {
        var stockPriceService = serviceProvider.GetRequiredService<IAlphaVantageApiService>();
        var chartService = serviceProvider.GetRequiredService<IChartService>();

        Console.WriteLine("Enter stock symbol:");
        var stockSymbol = Console.ReadLine()?.ToUpper() ?? "AAPL";
        
        Console.WriteLine($"Fetching latest price for {stockSymbol}...");
        var price = stockPriceService.GetMostRecentPrice(stockSymbol).Result;
        Console.WriteLine($"Latest price: ${price}");
        
        Console.WriteLine($"\nFetching historical data for {stockSymbol}...");
        var days = 10;
        var timeSeries = stockPriceService.GetTimeSeries(stockSymbol, days).Result;
        
        Console.WriteLine($"\nCandlestick chart for {stockSymbol} (last {days} days):");
        var chart = chartService.RenderCandlestickChart(timeSeries);
        Console.WriteLine(chart);
    }
}