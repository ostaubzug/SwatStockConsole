using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StockConsole;
using StockConsole.Model;
using StockConsole.Services;

namespace TestStockConsole;

[TestClass]
[TestSubject(typeof(Client))]
public class ClientTest
{
    private Mock<IAlphaVantageApiService>? _mockStockPriceService;
    private Mock<IChartService>? _mockChartService;
    private ServiceProvider? _serviceProvider;

    [TestInitialize]
    public void Setup()
    {
        _mockStockPriceService = new Mock<IAlphaVantageApiService>();
        _mockChartService = new Mock<IChartService>();

        var services = new ServiceCollection();
        services.AddSingleton(_mockStockPriceService.Object);
        services.AddSingleton(_mockChartService.Object);
        _serviceProvider = services.BuildServiceProvider();
    }

    [TestMethod]
    [TestCategory("ContinuousIntegration")]
    public void StartConsoleApplication_WithValidInput_DisplaysCorrectOutput()
    {
        const string testSymbol = "AAPL";
        const decimal testPrice = 150.25m;
        const int testDays = 10;
        var testTimeSeries = new List<DailyPriceData>
        {
            new() { Open = 150m, High = 151m, Low = 149m, Close = 150.25m }
        };
        const string testChart = "Test Chart";

        _mockStockPriceService!.Setup(s => s.GetMostRecentPrice(testSymbol))
            .ReturnsAsync(testPrice);
        _mockStockPriceService.Setup(s => s.GetTimeSeries(testSymbol, testDays))
            .ReturnsAsync(testTimeSeries);
        _mockChartService!.Setup(s => s.RenderCandlestickChart(testTimeSeries))
            .Returns(testChart);

        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        Console.SetIn(new StringReader(testSymbol));

        Client.StartConsoleApplication(_serviceProvider!);

        var output = stringWriter.ToString();
        Assert.IsTrue(output.Contains($"Fetching latest price for {testSymbol}..."));
        Assert.IsTrue(output.Contains($"Latest price: ${testPrice}"));
        Assert.IsTrue(output.Contains($"Fetching historical data for {testSymbol}..."));
        Assert.IsTrue(output.Contains($"Candlestick chart for {testSymbol} (last {testDays} days):"));
        Assert.IsTrue(output.Contains(testChart));
    }
}