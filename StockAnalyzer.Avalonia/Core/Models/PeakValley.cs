namespace StockAnalyzer.Avalonia.Core.Models
{
    /// <summary>
    /// Represents a peak or valley point in the candlestick data.
    /// </summary>
    public class PeakValley
    {
        public Candlestick Candlestick { get; set; } = null!;
        public bool IsPeak { get; set; }
        public bool IsValley { get; set; }
        public int Margin { get; set; }

        public PeakValley() { }

        public PeakValley(Candlestick candlestick, bool isPeak, bool isValley, int margin)
        {
            Candlestick = candlestick;
            IsPeak = isPeak;
            IsValley = isValley;
            Margin = margin;
        }

        public override string ToString() => 
            $"{Candlestick.Date:MM/dd/yyyy} ({(IsPeak ? "Peak" : "Valley")})";
    }
}
