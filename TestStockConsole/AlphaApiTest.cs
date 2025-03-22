namespace TestStockConsole;

[TestClass]
public class AlphaApiTest
{
    private string apiKey;

    [TestInitialize]
    public void GetApiKey()
    {
        DotNetEnv.Env.Load();
        apiKey = Environment.GetEnvironmentVariable("ALPHA_API_KEY");
    }
    
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    public void TestGetApiKey()
    {
        Assert.IsNotNull(apiKey);
        Assert.IsTrue(apiKey.Length > 0);
    }
    
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    public async Task TestAlphaVantageApiKeyWorks()
    {
        using var client = new HttpClient();
        
        string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=AAPL&apikey={apiKey}";
        var response = await client.GetAsync(url);
    
        var content = await response.Content.ReadAsStringAsync();
    
        Assert.IsTrue(content.Contains("Global Quote"), "Response doesn't contain expected data");
        Console.WriteLine($"API Response: {content}");
    }
    
    
}