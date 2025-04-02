namespace TestStockConsole.TestData;

public class ApiData
{
    
    public const string ValidQuoteJson = @"{
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
    
    public const string ValidTimeSeriesJson = @"{
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
                },
                ""2025-03-23"": {
                    ""1. open"": ""219.5000"",
                    ""2. high"": ""222.1200"",
                    ""3. low"": ""219.1500"",
                    ""4. close"": ""221.0800"",
                    ""5. volume"": ""38765432""
                },
                ""2025-03-22"": {
                    ""1. open"": ""218.2500"",
                    ""2. high"": ""220.5000"",
                    ""3. low"": ""217.8800"",
                    ""4. close"": ""219.5700"",
                    ""5. volume"": ""35678923""
                },
                ""2025-03-21"": {
                    ""1. open"": ""217.1000"",
                    ""2. high"": ""219.2500"",
                    ""3. low"": ""216.7500"",
                    ""4. close"": ""218.3100"",
                    ""5. volume"": ""36543210""
                },
                ""2025-03-20"": {
                    ""1. open"": ""215.8000"",
                    ""2. high"": ""218.1000"",
                    ""3. low"": ""215.5000"",
                    ""4. close"": ""217.2500"",
                    ""5. volume"": ""37654320""
                }
            }
        }";
}