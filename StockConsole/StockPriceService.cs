namespace StockConsole;

public class StockPriceService
{
    private readonly string _apiKey;
    private readonly string _apiUrl;
    private readonly IApiService _apiService;
    
    public StockPriceService(IApiService apiService)
    {
        DotNetEnv.Env.Load();
        _apiKey = Environment.GetEnvironmentVariable("ALPHA_API_KEY") ?? throw new ArgumentException("Api Key not found");
        _apiUrl = Environment.GetEnvironmentVariable("ALPHA_API_URL") ?? throw new ArgumentException("Api url not found");
        _apiService = apiService;
    }
    

    public async Task<decimal> GetMostRecentPrice(string symbol)
    {
        var json = await _apiService.GetMostRecentPrice(_apiUrl, _apiKey, symbol);
        return JsonUtility.ExtractMostRecentPrice(json);
    }
}