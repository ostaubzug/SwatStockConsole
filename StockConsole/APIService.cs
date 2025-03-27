using System.Text.Json;

namespace StockConsole;

public class ApiService
{
    private string _apiKey;
    private string _apiUrl;
    private readonly HttpClient _client;
    
    public ApiService(HttpClient client)
    {
        _client = client;
        LoadEnvVariables();
    }

    private void LoadEnvVariables()
    {
        DotNetEnv.Env.Load();
        _apiKey = Environment.GetEnvironmentVariable("ALPHA_API_KEY") ?? throw new ArgumentException("Api Key not found");
        _apiUrl = Environment.GetEnvironmentVariable("ALPHA_API_URL") ?? throw new ArgumentException("Api url not found");
    }

    public async Task<decimal> GetMostRecentPrice(string symbol)
    {
        //todo dependency injection für tests vom client / reuse
        string urlWithSymbol = _apiUrl.Replace("{symbol}", symbol).Replace("{_apiKey}", _apiKey!);
        
        HttpResponseMessage response = await _client.GetAsync(urlWithSymbol);
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