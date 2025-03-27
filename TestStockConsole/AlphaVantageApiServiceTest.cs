using JetBrains.Annotations;
using StockConsole;

namespace TestStockConsole;

[TestClass]
[TestSubject(typeof(IAlphaVantageApiService))]
public class AlphaVantageApiServiceTest
{
    private readonly string _validJson = @"{
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

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    public async Task GetMostRecentPrice_ReturnsCorrectPrice()
    {
        var mockFactory = HttpClientFactoryMock.CreateMockFactory(_validJson);
        var service = new AlphaVantageApiService(mockFactory);

        var price = await service.GetMostRecentPrice("AAPL");

        Assert.AreEqual(221.53m, price);
        
    }

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    public async Task GetMostRecentPrice_CheckSentRequest()
    {
        var mockFactory = HttpClientFactoryMock.CreateMockFactory(_validJson);
        var service = new AlphaVantageApiService(mockFactory);
        await service.GetMostRecentPrice("AAPL");
        
        var request = HttpClientFactoryMock.ReceivedRequests[0];

        Assert.IsTrue(HttpClientFactoryMock.ReceivedRequests.Count > 0, "No requests were captured");
        Assert.IsTrue(request.RequestUri != null && request.RequestUri.ToString().Contains("AAPL"), "Request URL should contain the symbol");
    }
}