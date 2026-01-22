namespace StockAnalyzer.Avalonia.Core.Models
{
    /// <summary>
    /// Represents a Fibonacci retracement level with confirmation count.
    /// </summary>
    public record FibonacciLevel(
        double Ratio,           // e.g., 0.236, 0.382, 0.5, 0.618
        double Price,           // The price at this level
        int Confirmations       // Number of candlesticks that touched this level
    )
    {
        public string Label => $"{Ratio * 100:F1}%";
    }
}
