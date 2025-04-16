using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using StockConsole.Model;
using StockConsole.Services;
using TestStockConsole.TestData;

namespace TestStockConsole.ServicesTest;


[TestClass]
[TestSubject(typeof(IAlphaVantageApiService))]
public class AlphaVantageApiServiceTest
{

    private static readonly Dictionary<string, string> ConfigurationValues = new Dictionary<string, string>
    {
        {"ALPHA_API_KEY", "test-api-key"},
        {"ALPHA_API_URL", "https://www.alphavantage.co/query?function={function}&symbol={symbol}&apikey={apiKey}"}
    };
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(ConfigurationValues!)
        .Build();
    
    private IHttpClientFactory CreateMockHttpClientFactory()
    {
        return HttpClientFactoryMock.CreateMockFactory(request =>
        {
            var requestUri = request.RequestUri?.ToString() ?? string.Empty;
            if (requestUri.Contains("function=GLOBAL_QUOTE"))
            {
                return ApiData.ValidQuoteJson;
            }
            else if (requestUri.Contains("function=TIME_SERIES_DAILY"))
            {
                return ApiData.ValidTimeSeriesJson;
            }
            return "{}";
        });
    }

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    public async Task GetMostRecentPrice_ReturnsCorrectPrice()
    {
        var mockFactory = CreateMockHttpClientFactory();
        var service = new AlphaVantageApiService(mockFactory, Configuration);
        var price = await service.GetMostRecentPrice("AAPL");
        Assert.AreEqual(221.53m, price);
    }

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    public async Task GetMostRecentPrice_CheckSentRequest()
    {
        var mockFactory = CreateMockHttpClientFactory();
        var service = new AlphaVantageApiService(mockFactory, Configuration);
        await service.GetMostRecentPrice("AAPL");
        
        var request = HttpClientFactoryMock.ReceivedRequests[0];

        Assert.IsTrue(HttpClientFactoryMock.ReceivedRequests.Count > 0, "No requests were captured");
        Assert.IsTrue(request.RequestUri != null && request.RequestUri.ToString().Contains("AAPL"), "Request URL should contain the symbol");
        Assert.IsTrue(request.RequestUri != null && request.RequestUri.ToString().Contains("GLOBAL_QUOTE"), "Request URL should contain the function GLOBAL_QUOTE");
    }
    

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    public async Task GetLastWeekPrices_CheckSentRequest()
    {
        var mockFactory = CreateMockHttpClientFactory();
        var service = new AlphaVantageApiService(mockFactory, Configuration);
        await service.GetTimeSeries("AAPL",7);
        
        var request = HttpClientFactoryMock.ReceivedRequests[0];

        Assert.IsTrue(HttpClientFactoryMock.ReceivedRequests.Count > 0, "No requests were captured");
        Assert.IsTrue(request.RequestUri != null && request.RequestUri.ToString().Contains("AAPL"), "Request URL should contain the symbol");
        Assert.IsTrue(request.RequestUri != null && request.RequestUri.ToString().Contains("TIME_SERIES_DAILY"), "Request URL should contain the function TIME_SERIES_DAILY");
    }

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    [ExpectedException(typeof(ArgumentException), "Api Key not found")]
    public void Constructor_ThrowsException_WhenApiKeyMissing()
    {
        var emptyConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"ALPHA_API_URL", "https://www.alphavantage.co/query?function={function}&symbol={symbol}&apikey={apiKey}"}
            }!)
            .Build();
            
        _ = new AlphaVantageApiService(CreateMockHttpClientFactory(), emptyConfig);
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
            
        _ = new AlphaVantageApiService(CreateMockHttpClientFactory(), emptyConfig);
    }

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    [ExpectedException(typeof(ArgumentException), "Api Key not found")]
    public void Constructor_ThrowsException_WhenNoConfigValuesProvided()
    {
        var emptyConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>()!)
            .Build();
            
        _ = new AlphaVantageApiService(CreateMockHttpClientFactory(), emptyConfig);
    }

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    public async Task GetTimeSeriesAllPrices_CheckSentRequest()
    {
        var mockFactory = CreateMockHttpClientFactory();
        var service = new AlphaVantageApiService(mockFactory, Configuration);
        await service.GetTimeSeries("AAPL", 3);
        
        var request = HttpClientFactoryMock.ReceivedRequests[0];

        Assert.IsTrue(HttpClientFactoryMock.ReceivedRequests.Count > 0, "No requests were captured");
        Assert.IsTrue(request.RequestUri != null && request.RequestUri.ToString().Contains("AAPL"), "Request URL should contain the symbol");
        Assert.IsTrue(request.RequestUri != null && request.RequestUri.ToString().Contains("TIME_SERIES_DAILY"), "Request URL should contain the function TIME_SERIES_DAILY");
    }
    
    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    public async Task GetTimeSeries_ReturnsDataWithCorrectStructureAndValues()
    {
        var mockFactory = CreateMockHttpClientFactory();
        var service = new AlphaVantageApiService(mockFactory, Configuration);
        
        var stockData = await service.GetTimeSeries("AAPL", 3);
        
        Assert.IsNotNull(stockData, "GetTimeSeries should not return null");
        Assert.AreEqual(3, stockData.Count, "Should return exactly 3 days of data");
        
        // Day 1 (2025-03-26)
        Assert.AreEqual(223.51m, stockData[0].Open, "Day 1 Open price should match");
        Assert.AreEqual(225.02m, stockData[0].High, "Day 1 High price should match");
        Assert.AreEqual(220.47m, stockData[0].Low, "Day 1 Low price should match");
        Assert.AreEqual(221.53m, stockData[0].Close, "Day 1 Close price should match");
        
        // Day 2 (2025-03-25)
        Assert.AreEqual(220.77m, stockData[1].Open, "Day 2 Open price should match");
        Assert.AreEqual(224.10m, stockData[1].High, "Day 2 High price should match");
        Assert.AreEqual(220.08m, stockData[1].Low, "Day 2 Low price should match");
        Assert.AreEqual(223.75m, stockData[1].Close, "Day 2 Close price should match");
        
        // Day 3 (2025-03-24)
        Assert.AreEqual(221.00m, stockData[2].Open, "Day 3 Open price should match");
        Assert.AreEqual(221.48m, stockData[2].High, "Day 3 High price should match");
        Assert.AreEqual(218.58m, stockData[2].Low, "Day 3 Low price should match");
        Assert.AreEqual(220.73m, stockData[2].Close, "Day 3 Close price should match");
        
        // Verify data type structure
        var firstDay = stockData[0];
        Assert.IsTrue(firstDay.GetType() == typeof(DailyPriceData), "Result should be of type DailyPriceData");
        Assert.IsTrue(firstDay.GetType().GetProperties().Length == 4, "DailyPriceData should have exactly 4 properties");
        Assert.IsTrue(firstDay.GetType().GetProperty("Open") != null, "DailyPriceData should have Open property");
        Assert.IsTrue(firstDay.GetType().GetProperty("High") != null, "DailyPriceData should have High property");
        Assert.IsTrue(firstDay.GetType().GetProperty("Low") != null, "DailyPriceData should have Low property");
        Assert.IsTrue(firstDay.GetType().GetProperty("Close") != null, "DailyPriceData should have Close property");
    }
}