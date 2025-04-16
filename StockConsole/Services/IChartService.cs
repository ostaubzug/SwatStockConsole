using StockConsole.Model;

namespace StockConsole.Services;

public interface IChartService
{
    
    /// <summary>
    /// Renders the CandleStick chart visible.
    /// </summary>
    /// <param name="timeSeries"></param>
    /// <returns></returns>
    string RenderCandlestickChart(List<DailyPriceData> timeSeries);
}