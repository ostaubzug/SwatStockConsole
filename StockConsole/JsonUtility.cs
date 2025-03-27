using System.Text.Json;

namespace StockConsole;

public static class JsonUtility
{
    public static decimal ExtractLatestPrice(string json)
    {
        using JsonDocument document = JsonDocument.Parse(json);
        
        if (!document.RootElement.TryGetProperty("Global Quote", out JsonElement globalQuote))
            throw new JsonException("Global Quote property not found in API response");
            
        if (!globalQuote.TryGetProperty("05. price", out JsonElement priceProperty))
            throw new JsonException("Price property not found in API response");
            
        var priceString = priceProperty.GetString()!;
        return decimal.Parse(priceString);
    }
}