namespace StockConsole;

public interface IAlphaVantageApiService
{
    public Task<decimal> GetMostRecentPrice(string symbol);

}