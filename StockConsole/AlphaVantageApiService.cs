namespace StockConsole;

public class AlphaVantageApiService : IApiService
{
    private static readonly HttpClient HttpClient = new();
    
    public Task<string> GetMostRecentPrice(string url, string apiKey, string symbol)
    {
        var requestUrl = url.Replace("{symbol}", symbol).Replace("{_apiKey}", apiKey);
        return HttpClient.GetStringAsync(requestUrl);
    }
}