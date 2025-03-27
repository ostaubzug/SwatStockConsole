using Microsoft.Extensions.Configuration;

namespace StockConsole;

public class AlphaVantageApiService(IHttpClientFactory clientFactory, IConfiguration configuration)
    : IAlphaVantageApiService
{
    private readonly string _apiKey = configuration["ALPHA_API_KEY"] ?? throw new ArgumentException("Api Key not found");
    private readonly string _apiUrl = configuration["ALPHA_API_URL"] ?? throw new ArgumentException("Api url not found");

    public async Task<decimal> GetMostRecentPrice(string symbol)
    {
        var json = await FetchApi(_apiUrl,"GLOBAL_QUOTE", _apiKey, symbol);
        return JsonUtility.ExtractLatestPrice(json);
    }
    
    public async Task<List<DailyPriceData>> GetTimeSeries(string symbol, int days)
    {
        var json = await FetchApi(_apiUrl,"TIME_SERIES_DAILY", _apiKey, symbol);
        return JsonUtility.ExtractAllPricesLastXDays(json,days);
    }

    private async Task<string> FetchApi(string url, string function, string apiKey, string symbol)
    {
        var requestUrl = url
            .Replace("{function}", function)
            .Replace("{symbol}", symbol)
            .Replace("{apiKey}", apiKey);
            
        var client = clientFactory.CreateClient("AlphaVantage");
        return await client.GetStringAsync(requestUrl);
    }
}