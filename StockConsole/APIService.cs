namespace StockConsole;

public class ApiService
{
    private string _apiKey;
    private string _apiUrl;
    private readonly HttpClient _httpClient;
    
    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
        var requestUrl = _apiUrl.Replace("{symbol}", symbol).Replace("{_apiKey}", _apiKey!);
        var json = await _httpClient.GetStringAsync(requestUrl);
        return JsonUtility.ExtractMostRecentPrice(json);
    }
}