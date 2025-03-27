using System.Text;

namespace StockConsole;



public class ChartService : IChartService
{
    private const int ChartHeight = 15;
    private const char UpBodyChar = '█';    // Full block for up days
    private const char DownBodyChar = '░';  // Light shade for down days - better distinction
    private const char WickChar = '│';      // Box drawings light vertical
    private const char AxisChar = '│';
    private const char HorizontalLineChar = '─';
    
    public string RenderCandlestickChart(List<DailyPriceData> timeSeries)
    {
        if (timeSeries == null || timeSeries.Count == 0)
            return "No data to display";
            
        var sb = new StringBuilder();
        
        // Calculate chart dimensions
        decimal minPrice = timeSeries.Min(d => d.Low);
        decimal maxPrice = timeSeries.Max(d => d.High);
        decimal priceRange = maxPrice - minPrice;
        
        // Add some padding (10%) to the price range for better visualization
        decimal padding = priceRange * 0.1m;
        minPrice -= padding;
        maxPrice += padding;
        priceRange = maxPrice - minPrice;
        
        // If all prices are the same, create a small range to avoid division by zero
        if (priceRange == 0)
            priceRange = 1;
            
        // Add title and header
        sb.AppendLine($"Candlestick Chart (Last {timeSeries.Count} days)");
        sb.AppendLine(new string('-', timeSeries.Count * 2 + 15));
        
        // Add price scale on the left side
        sb.AppendLine($"{maxPrice:F2} {AxisChar}");
        
        // Create chart rows
        for (int row = 0; row < ChartHeight; row++)
        {
            // Calculate the price level for this row
            decimal rowPrice = maxPrice - (row * priceRange / ChartHeight);
            
            // Start with price label and axis
            sb.Append($"{rowPrice:F2} {AxisChar}");
            
            // Draw candles in each column
            for (int i = 0; i < timeSeries.Count; i++)
            {
                var data = timeSeries[i];
                var isUpDay = data.Close >= data.Open;
                
                // Calculate normalized positions (0 to ChartHeight-1)
                int highPos = NormalizePrice(data.High, minPrice, maxPrice, priceRange);
                int lowPos = NormalizePrice(data.Low, minPrice, maxPrice, priceRange);
                int openPos = NormalizePrice(data.Open, minPrice, maxPrice, priceRange);
                int closePos = NormalizePrice(data.Close, minPrice, maxPrice, priceRange);
                
                // Current row position (0 at top, ChartHeight-1 at bottom)
                int currentPos = row;
                
                // Determine what to draw at this position
                char charToDraw = ' ';
                
                if (currentPos >= highPos && currentPos <= lowPos)
                {
                    // We're within the high-low range for this candle
                    if (currentPos >= Math.Min(openPos, closePos) && currentPos <= Math.Max(openPos, closePos))
                    {
                        // We're within the body range (between open and close)
                        charToDraw = isUpDay ? UpBodyChar : DownBodyChar;
                    }
                    else
                    {
                        // We're in the wick range (between high/low and open/close)
                        charToDraw = WickChar;
                    }
                }
                
                // Add a space after each character to improve spacing in terminals
                sb.Append(charToDraw + " ");
            }
            
            sb.AppendLine();
        }
        
        // Add bottom price scale
        sb.Append($"{minPrice:F2} {AxisChar}");
        sb.AppendLine(new string(HorizontalLineChar, timeSeries.Count * 2));
        
        // Add day markers at bottom with better spacing
        sb.Append("       ");
        for (int i = 0; i < timeSeries.Count; i++)
        {
            if (i < 9)
                sb.Append($"{i+1} ");
            else
                sb.Append($"{i+1}");
            
            if (i < timeSeries.Count - 1 && i >= 9)
                sb.Append(" ");
        }
        
        // Add legend
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine($"Legend: {UpBodyChar} Up day   {DownBodyChar} Down day   {WickChar} Price range/wick");
        
        return sb.ToString();
    }
    
    private int NormalizePrice(decimal price, decimal minPrice, decimal maxPrice, decimal priceRange)
    {
        // Convert price to position in chart (0 = top/maxPrice, ChartHeight-1 = bottom/minPrice)
        return (int)Math.Round((maxPrice - price) / priceRange * ChartHeight);
    }
}