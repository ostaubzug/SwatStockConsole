using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using StockConsole;

namespace TestStockConsole;

[TestClass]
[TestSubject(typeof(IAlphaVantageApiService))]
public class AlphaVantageApiServiceTest
{
    private const string ValidJson = @"{
            ""Global Quote"": {
                ""01. symbol"": ""AAPL"",
                ""02. open"": ""223.5100"",
                ""03. high"": ""225.0200"",
                ""04. low"": ""220.4700"",
                ""05. price"": ""221.5300"",
                ""06. volume"": ""34532656"",
                ""07. latest trading day"": ""2025-03-26"",
                ""08. previous close"": ""223.7500"",
                ""09. change"": ""-2.2200"",
                ""10. change percent"": ""-0.9922%""
            }
        }";
    
    private static readonly Dictionary<string, string> ConfigurationValues = new Dictionary<string, string>
    {
        {"ALPHA_API_KEY", "test-api-key"},
        {"ALPHA_API_URL", "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}"}
    };
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(ConfigurationValues!)
        .Build();
    private readonly IHttpClientFactory _mockFactory = HttpClientFactoryMock.CreateMockFactory(ValidJson);
    

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    public async Task GetMostRecentPrice_ReturnsCorrectPrice()
    {
        var service = new AlphaVantageApiService(_mockFactory, Configuration);
        var price = await service.GetMostRecentPrice("AAPL");
        Assert.AreEqual(221.53m, price);
        
    }

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    public async Task GetMostRecentPrice_CheckSentRequest()
    {
        var service = new AlphaVantageApiService(_mockFactory,Configuration);
        await service.GetMostRecentPrice("AAPL");
        
        var request = HttpClientFactoryMock.ReceivedRequests[0];

        Assert.IsTrue(HttpClientFactoryMock.ReceivedRequests.Count > 0, "No requests were captured");
        Assert.IsTrue(request.RequestUri != null && request.RequestUri.ToString().Contains("AAPL"), "Request URL should contain the symbol");
    }

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    [ExpectedException(typeof(ArgumentException), "Api Key not found")]
    public void Constructor_ThrowsException_WhenApiKeyMissing()
    {
        var emptyConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"ALPHA_API_URL", "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}"}
            }!)
            .Build();
            
        _ = new AlphaVantageApiService(_mockFactory, emptyConfig);
    }
    
    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    [ExpectedException(typeof(ArgumentException), "Api url not found")]
    public void Constructor_ThrowsException_WhenApiUrlMissing()
    {
        var emptyConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"ALPHA_API_KEY", "test-api-key"}
            }!)
            .Build();
            
        _ = new AlphaVantageApiService(_mockFactory, emptyConfig);
    }

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    [ExpectedException(typeof(ArgumentException), "Api Key not found")]
    public void Constructor_ThrowsException_WhenNoConfigValuesProvided()
    {
        var emptyConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>()!)
            .Build();
            
        _ = new AlphaVantageApiService(_mockFactory, emptyConfig);
    }
}