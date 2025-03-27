namespace StockConsole;

public class AlphaVantageApiService : IAlphaVantageApiService
{
    private readonly string _apiKey;
    private readonly string _apiUrl;
    private readonly IHttpClientFactory _httpClientFactory;

    public AlphaVantageApiService(IHttpClientFactory clientFactory)
    {
        DotNetEnv.Env.Load();
        _apiKey = Environment.GetEnvironmentVariable("ALPHA_API_KEY") ?? throw new ArgumentException("Api Key not found");
        _apiUrl = Environment.GetEnvironmentVariable("ALPHA_API_URL") ?? throw new ArgumentException("Api url not found");
        _httpClientFactory = clientFactory;
    }
    
    public async Task<decimal> GetMostRecentPrice(string symbol)
    {
        var json = await FetchApi(_apiUrl, _apiKey, symbol);
        return JsonUtility.ExtractLatestPrice(json);
    }
    
    private async Task<string> FetchApi(string url, string apiKey, string symbol)
    {
        var requestUrl = url.Replace("{symbol}", symbol).Replace("{_apiKey}", apiKey);
        var client = _httpClientFactory.CreateClient("AlphaVantage");
        return await client.GetStringAsync(requestUrl);
    }
}