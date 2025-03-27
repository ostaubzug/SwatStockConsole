using JetBrains.Annotations;
using StockConsole;
using System.Text.Json;

namespace TestStockConsole;

[TestClass]
[TestSubject(typeof(JsonUtility))]
public class JsonUtilityTest
{
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    public void ExtractMostRecentPrice_ValidJson_ReturnsPrice()
    {
        string validJson = @"{
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
        
        decimal price = JsonUtility.ExtractMostRecentPrice(validJson);
        
        Assert.AreEqual(221.53m, price);
    }
    
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    [ExpectedException(typeof(JsonException), "Global Quote property not found in API response")]
    public void ExtractMostRecentPrice_MissingGlobalQuote_ThrowsJsonException()
    {
        string invalidJson = @"{
            ""WrongProperty"": {}
        }";
        JsonUtility.ExtractMostRecentPrice(invalidJson);
    }
    
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    [ExpectedException(typeof(JsonException), "Price property not found in API response")]
    public void ExtractMostRecentPrice_MissingPriceProperty_ThrowsJsonException()
    {
        string invalidJson = @"{
            ""Global Quote"": {
                ""01. symbol"": ""AAPL"",
                ""02. open"": ""223.5100"",
                ""missing price property"": ""value""
            }
        }";
        JsonUtility.ExtractMostRecentPrice(invalidJson);
    }
}