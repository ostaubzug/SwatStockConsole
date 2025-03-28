using StockConsole.Model;

namespace StockConsole.Services;

public interface IChartService
{
    string RenderCandlestickChart(List<DailyPriceData> timeSeries);
}