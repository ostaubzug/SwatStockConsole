using JetBrains.Annotations;
using StockConsole;

namespace TestStockConsole;


[TestClass]
[TestSubject(typeof(StockPriceService))]
public class StockPriceApiTest
{
    [TestCategory("Manual")]
    [TestMethod]
    public async Task TestGetPrice()
    {
        //todo api Client Mock
        var service = new StockPriceService(new AlphaVantageApiService());
        var price = await service.GetMostRecentPrice("AAPL");
        Console.WriteLine(price);
        Assert.IsNotNull(price);
    }

    
    
}