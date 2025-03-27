using JetBrains.Annotations;
using StockConsole;

namespace TestStockConsole;


[TestClass]
[TestSubject(typeof(ApiService))]
public class ApiServiceTest
{
    [TestCategory("Manual")]
    [TestMethod]
    public async Task TestGetPrice()
    {
        //todo api Client Mock
        var service = new ApiService(new HttpClient());
        var price = await service.GetMostRecentPrice("AAPL");
        Console.WriteLine(price);
        Assert.IsNotNull(price);
    }

    
    
}