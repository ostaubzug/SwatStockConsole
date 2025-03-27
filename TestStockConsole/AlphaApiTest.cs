using StockConsole;

namespace TestStockConsole;

[TestClass]
public class AlphaApiTest
{
    private string? _apiKey;

    [TestInitialize]
    public void GetApiKey()
    {
        DotNetEnv.Env.Load();
        _apiKey = Environment.GetEnvironmentVariable("ALPHA_API_KEY");
    }

    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    public void TestEnvVariablesd()
    {
        //todo Konstanten einführen
        var apiKey = Environment.GetEnvironmentVariable("ALPHA_API_KEY") ?? throw new ArgumentException("Api Key not found");
        var apiUrl = Environment.GetEnvironmentVariable("ALPHA_API_URL") ?? throw new ArgumentException("Api url not found");
    
    }
    
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    public void TestGetApiKey()
    {
        Assert.IsNotNull(_apiKey);
        Assert.IsTrue(_apiKey.Length > 0);
    }
    
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    public async Task TestAlphaVantageApiKeyWorks()
    {
        using var client = new HttpClient();
        
        string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=AAPL&apikey={_apiKey}";
        var response = await client.GetAsync(url);
    
        var content = await response.Content.ReadAsStringAsync();
    
        Assert.IsTrue(content.Contains("Global Quote"), "Response doesn't contain expected data");
        Console.WriteLine($"API Response: {content}");
    }
    
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    public async Task TestGetPrice()
    {
        //todo api Client Mock
        var service = new ApiService(new HttpClient());
        var price = await service.GetMostRecentPrice("AAPL");
        Console.WriteLine(price);
        Assert.IsNotNull(price);
    }
    
    
}