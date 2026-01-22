namespace StockAnalyzer.Avalonia.Core.Models
{
    /// <summary>
    /// Represents a price wave between two peak/valley extremes.
    /// </summary>
    public class Wave
    {
        public PeakValley Start { get; set; } = null!;
        public PeakValley End { get; set; } = null!;

        /// <summary>True if this wave goes from valley to peak (price increase).</summary>
        public bool IsUpWave => Start.IsValley && End.IsPeak;
        
        /// <summary>True if this wave goes from peak to valley (price decrease).</summary>
        public bool IsDownWave => Start.IsPeak && End.IsValley;

        /// <summary>The price at the start of the wave.</summary>
        public decimal StartPrice => IsUpWave ? Start.Candlestick.Low : Start.Candlestick.High;
        
        /// <summary>The price at the end of the wave.</summary>
        public decimal EndPrice => IsUpWave ? End.Candlestick.High : End.Candlestick.Low;

        public override string ToString() => 
            $"{Start.Candlestick.Date:MM/dd/yyyy} – {End.Candlestick.Date:MM/dd/yyyy}";
    }
}
