namespace StockConsole;

public interface IApiService
{
    public Task<string> GetMostRecentPrice(string url, string apiKey, string symbol);

}