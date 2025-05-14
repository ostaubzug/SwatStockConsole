using JetBrains.Annotations;
using StockConsole.Services;
using StockConsole.Utility;
using TestStockConsole.TestData;

namespace TestStockConsole.ServicesTest;

[TestClass]
[TestSubject(typeof(ChartService))]
public class ChartServiceTest
{

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    public void CreateChart()
    {
        var service = new ChartService();
        var priceData = StockDataParser.GetRecentDailyPrices(ApiData.ValidTimeSeriesJson, 7);
        var result = service.RenderCandlestickChart(priceData);
        Assert.AreEqual(File.ReadAllText("ServicesTest/chart.txt"),result);
    }
    
}