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
        
        decimal price = JsonUtility.ExtractLatestPrice(validJson);
        
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
        JsonUtility.ExtractLatestPrice(invalidJson);
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
        JsonUtility.ExtractLatestPrice(invalidJson);
    }
    
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    public void ExtractPriceLastXDays_ValidJson_ReturnsCorrectPrices()
    {
        string validJson = @"{
            ""Meta Data"": {
                ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
                ""2. Symbol"": ""AAPL"",
                ""3. Last Refreshed"": ""2025-03-26"",
                ""4. Output Size"": ""Compact"",
                ""5. Time Zone"": ""US/Eastern""
            },
            ""Time Series (Daily)"": {
                ""2025-03-26"": {
                    ""1. open"": ""223.5100"",
                    ""2. high"": ""225.0200"",
                    ""3. low"": ""220.4700"",
                    ""4. close"": ""221.5300"",
                    ""5. volume"": ""34532656""
                },
                ""2025-03-25"": {
                    ""1. open"": ""220.7700"",
                    ""2. high"": ""224.1000"",
                    ""3. low"": ""220.0800"",
                    ""4. close"": ""223.7500"",
                    ""5. volume"": ""34493583""
                },
                ""2025-03-24"": {
                    ""1. open"": ""221.0000"",
                    ""2. high"": ""221.4800"",
                    ""3. low"": ""218.5800"",
                    ""4. close"": ""220.7300"",
                    ""5. volume"": ""44299483""
                }
            }
        }";
        
        var prices = JsonUtility.ExtractPriceLastXDays(validJson, 3);
        
        Assert.AreEqual(3, prices.Count);
        Assert.AreEqual(221.53m, prices[0]);
        Assert.AreEqual(223.75m, prices[1]);
        Assert.AreEqual(220.73m, prices[2]);
    }
    
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    public void ExtractPriceLastXDays_RequestMoreDaysThanAvailable_ReturnsAllAvailableDays()
    {
        string validJson = @"{
            ""Meta Data"": {
                ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
                ""2. Symbol"": ""AAPL"",
                ""3. Last Refreshed"": ""2025-03-26"",
                ""4. Output Size"": ""Compact"",
                ""5. Time Zone"": ""US/Eastern""
            },
            ""Time Series (Daily)"": {
                ""2025-03-26"": {
                    ""1. open"": ""223.5100"",
                    ""2. high"": ""225.0200"",
                    ""3. low"": ""220.4700"",
                    ""4. close"": ""221.5300"",
                    ""5. volume"": ""34532656""
                },
                ""2025-03-25"": {
                    ""1. open"": ""220.7700"",
                    ""2. high"": ""224.1000"",
                    ""3. low"": ""220.0800"",
                    ""4. close"": ""223.7500"",
                    ""5. volume"": ""34493583""
                }
            }
        }";
        
        var prices = JsonUtility.ExtractPriceLastXDays(validJson, 5);
        
        Assert.AreEqual(2, prices.Count);
        Assert.AreEqual(221.53m, prices[0]);
        Assert.AreEqual(223.75m, prices[1]);
    }
    
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    [ExpectedException(typeof(JsonException), "Time Series (Daily) property not found in API response")]
    public void ExtractPriceLastXDays_MissingTimeSeriesProperty_ThrowsJsonException()
    {
        string invalidJson = @"{
            ""Meta Data"": {
                ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
                ""2. Symbol"": ""AAPL"",
                ""3. Last Refreshed"": ""2025-03-26""
            },
            ""Wrong Property"": {}
        }";
        
        JsonUtility.ExtractPriceLastXDays(invalidJson, 3);
    }
    
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    public void ExtractAllPricesLastXDays_ValidJson_ReturnsCorrectPrices()
    {
        string validJson = @"{
            ""Meta Data"": {
                ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
                ""2. Symbol"": ""AAPL"",
                ""3. Last Refreshed"": ""2025-03-26"",
                ""4. Output Size"": ""Compact"",
                ""5. Time Zone"": ""US/Eastern""
            },
            ""Time Series (Daily)"": {
                ""2025-03-26"": {
                    ""1. open"": ""223.5100"",
                    ""2. high"": ""225.0200"",
                    ""3. low"": ""220.4700"",
                    ""4. close"": ""221.5300"",
                    ""5. volume"": ""34532656""
                },
                ""2025-03-25"": {
                    ""1. open"": ""220.7700"",
                    ""2. high"": ""224.1000"",
                    ""3. low"": ""220.0800"",
                    ""4. close"": ""223.7500"",
                    ""5. volume"": ""34493583""
                },
                ""2025-03-24"": {
                    ""1. open"": ""221.0000"",
                    ""2. high"": ""221.4800"",
                    ""3. low"": ""218.5800"",
                    ""4. close"": ""220.7300"",
                    ""5. volume"": ""44299483""
                }
            }
        }";
        
        var priceDataList = JsonUtility.ExtractAllPricesLastXDays(validJson, 3);
        
        Assert.AreEqual(3, priceDataList.Count);
        
        // Check first day's values
        Assert.AreEqual(223.51m, priceDataList[0].Open);
        Assert.AreEqual(225.02m, priceDataList[0].High);
        Assert.AreEqual(220.47m, priceDataList[0].Low);
        Assert.AreEqual(221.53m, priceDataList[0].Close);
        
        // Check second day's values
        Assert.AreEqual(220.77m, priceDataList[1].Open);
        Assert.AreEqual(224.10m, priceDataList[1].High);
        Assert.AreEqual(220.08m, priceDataList[1].Low);
        Assert.AreEqual(223.75m, priceDataList[1].Close);
        
        // Check third day's values
        Assert.AreEqual(221.00m, priceDataList[2].Open);
        Assert.AreEqual(221.48m, priceDataList[2].High);
        Assert.AreEqual(218.58m, priceDataList[2].Low);
        Assert.AreEqual(220.73m, priceDataList[2].Close);
    }
    
    [TestCategory("ContinuousIntegration")]
    [TestMethod]
    [ExpectedException(typeof(JsonException), "Time Series (Daily) property not found in API response")]
    public void ExtractAllPricesLastXDays_MissingTimeSeriesProperty_ThrowsJsonException()
    {
        string invalidJson = @"{
            ""Meta Data"": {
                ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
                ""2. Symbol"": ""AAPL"",
                ""3. Last Refreshed"": ""2025-03-26""
            },
            ""Wrong Property"": {}
        }";
        
        JsonUtility.ExtractAllPricesLastXDays(invalidJson, 3);
    }
}