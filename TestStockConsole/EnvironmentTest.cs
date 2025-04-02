using Microsoft.Extensions.Configuration;
using StockConsole;

namespace TestStockConsole;

[TestClass]
public class EnvironmentTest
{
    private static readonly Dictionary<string, string> ConfigurationValues = new Dictionary<string, string>
    {
        {"ALPHA_API_KEY", "test-api-key"},
        {"ALPHA_API_URL", "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}"}
    };
    
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(ConfigurationValues!)
        .Build();
    
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    public void TestEnvVariablesd()
    {
        var apiKey = Configuration["ALPHA_API_KEY"] ?? throw new ArgumentException("Api Key not found");
        var apiUrl = Configuration["ALPHA_API_URL"] ?? throw new ArgumentException("Api url not found");
        Assert.IsNotNull(apiKey, apiUrl);
    }
    
    [TestCategory("Manual")]
    [TestMethod]
    public async Task FetchApi()
    {
        var apiKey = Configuration["ALPHA_API_KEY"] ?? throw new ArgumentException("Api Key not found");
        
        using var client = new HttpClient();
        string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=AAPL&apikey={apiKey}";
        var response = await client.GetAsync(url);
    
        var content = await response.Content.ReadAsStringAsync();
    
        Console.WriteLine($"API Response: {content}");
        Assert.IsTrue(content.Contains("Global Quote"), "Response doesn't contain expected data");
    }
}