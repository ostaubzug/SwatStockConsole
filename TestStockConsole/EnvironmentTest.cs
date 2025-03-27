using StockConsole;

namespace TestStockConsole;

[TestClass]
public class EnvironmentTest
{
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    public void TestEnvVariablesd()
    {
        DotNetEnv.Env.Load();
        var apiKey = Environment.GetEnvironmentVariable("ALPHA_API_KEY") ?? throw new ArgumentException("Api Key not found");
        var apiUrl = Environment.GetEnvironmentVariable("ALPHA_API_URL") ?? throw new ArgumentException("Api url not found");
    }
    
    [TestCategory("Manual")]
    [TestMethod]
    public async Task FetchApi()
    {
        DotNetEnv.Env.Load();
        var apiKey = Environment.GetEnvironmentVariable("ALPHA_API_KEY") ?? throw new ArgumentException("Api Key not found");
        
        using var client = new HttpClient();
        string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=AAPL&apikey={apiKey}";
        var response = await client.GetAsync(url);
    
        var content = await response.Content.ReadAsStringAsync();
    
        Console.WriteLine($"API Response: {content}");
        Assert.IsTrue(content.Contains("Global Quote"), "Response doesn't contain expected data");
    }
    
}