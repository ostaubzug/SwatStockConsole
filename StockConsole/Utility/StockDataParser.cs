using System.Text.Json;
using StockConsole.Model;

namespace StockConsole.Utility;

public static class StockDataParser
{
    public static decimal GetCurrentPrice(string json)
    {
        using JsonDocument document = JsonDocument.Parse(json);

        if (!document.RootElement.TryGetProperty("Global Quote", out JsonElement globalQuote))
            throw new JsonException("Global Quote property not found in API response");

        if (!globalQuote.TryGetProperty("05. price", out JsonElement priceProperty))
            throw new JsonException("Price property not found in API response");

        var priceString = priceProperty.GetString()!;
        return decimal.Parse(priceString);
    }

    public static List<DailyPriceData> GetRecentDailyPrices(string json, int numberOfDays)
    {
        var result = new List<DailyPriceData>();
        using JsonDocument document = JsonDocument.Parse(json);

        document.RootElement.TryGetProperty("Time Series (Daily)", out var timeSeriesDaily);

        var dates = timeSeriesDaily.EnumerateObject()
            .Select(p => p.Name)
            .OrderByDescending(date => date)
            .Take(numberOfDays);

        foreach (var date in dates)
        {
            if (timeSeriesDaily.TryGetProperty(date, out var dailyData))
            {
                var priceData = new DailyPriceData();

                if (dailyData.TryGetProperty("1. open", out var openElement) &&
                    decimal.TryParse(openElement.GetString(), out var openPrice))
                {
                    priceData.Open = openPrice;
                }

                if (dailyData.TryGetProperty("2. high", out var highElement) &&
                    decimal.TryParse(highElement.GetString(), out var highPrice))
                {
                    priceData.High = highPrice;
                }

                if (dailyData.TryGetProperty("3. low", out var lowElement) &&
                    decimal.TryParse(lowElement.GetString(), out var lowPrice))
                {
                    priceData.Low = lowPrice;
                }

                if (dailyData.TryGetProperty("4. close", out var closeElement) &&
                    decimal.TryParse(closeElement.GetString(), out var closePrice))
                {
                    priceData.Close = closePrice;
                }

                result.Add(priceData);
            }
        }

        return result;
    }
}