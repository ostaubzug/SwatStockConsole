using StockConsole;

namespace TestStockConsole;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    public void TestMethod1()
    {
        Assert.IsTrue(true);
    }

    public void TestMethod2()
    {
        var api = new APIService();
        api.ServiceMethod();
        Assert.IsTrue(true);
    }
}