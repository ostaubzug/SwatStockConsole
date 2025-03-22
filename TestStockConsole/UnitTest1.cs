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

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    public void TestMethod2()
    {
        var api = new APIService();
        Assert.IsTrue(true);
    }
}