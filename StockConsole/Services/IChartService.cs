namespace StockConsole;

public interface IChartService
{
    string RenderCandlestickChart(List<DailyPriceData> timeSeries);
}