namespace StockConsole;

public class APIService
{
    private string apiKey;
    public APIService()
    {
        DotNetEnv.Env.Load();
        apiKey = Environment.GetEnvironmentVariable("ALPHA_API_KEY") ?? throw new ArgumentException();
    }
    
    public void ServiceMethod()
    {
        Console.WriteLine("HelloWorld");
    }
}