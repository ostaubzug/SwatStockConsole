using StockConsole.Model;

namespace StockConsole.Services;

public interface IAlphaVantageApiService
{
    public Task<decimal> GetMostRecentPrice(string symbol);
    
    public Task<List<DailyPriceData>> GetTimeSeries(string symbol, int days);
}