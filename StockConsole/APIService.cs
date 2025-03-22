using System.Text.Json;

namespace StockConsole;

public class APIService
{
    private string _apiKey;
    public APIService()
    {
        DotNetEnv.Env.Load();
        _apiKey = Environment.GetEnvironmentVariable("ALPHA_API_KEY") ?? throw new ArgumentException();
    }
    
    public async Task<decimal> GetLastPrice(string symbol)
    {
        string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}";

        using HttpClient client = new HttpClient();
        
        HttpResponseMessage response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var price = GetPriceProperty(json);
        return decimal.Parse(price);
    }

    private string GetPriceProperty(string json)
    {
        string price = string.Empty;
        using (JsonDocument document = JsonDocument.Parse(json))
        {
            if (document.RootElement.TryGetProperty("Global Quote", out JsonElement globalQuote))
            {
                if (globalQuote.TryGetProperty("05. price", out JsonElement priceElement))
                {
                    price = priceElement.GetString();
                }
            }
        }
        return price;
    }
}