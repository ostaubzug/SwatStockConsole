using StockConsole.Model;

namespace StockConsole.Services;

public interface IAlphaVantageApiService
{
    /// <summary>
    /// Returns the most recent price available.
    /// </summary>
    /// <param name="symbol"></param>
    /// <returns></returns>
    public Task<decimal> GetMostRecentPrice(string symbol);
    
    /// <summary>
    /// Gets the Time Series for the daily Price.
    /// </summary>
    /// <param name="symbol"></param>
    /// <param name="days"></param>
    /// <returns></returns>
    public Task<List<DailyPriceData>> GetTimeSeries(string symbol, int days);
}