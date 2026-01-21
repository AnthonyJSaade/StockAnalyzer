using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer
{
    /// <summary>
    /// Represents a price wave between two peak/valley extremes.
    /// </summary>
    public class Wave
    {
        /// <summary>Gets or sets the starting peak or valley.</summary>
        public PeakValley Start { get; set; }
        
        /// <summary>Gets or sets the ending peak or valley.</summary>
        public PeakValley End { get; set; }

        /// <summary>Returns true if this is an upward wave (valley to peak).</summary>
        public bool IsUpWave => Start.IsValley && End.IsPeak;
        
        /// <summary>Returns true if this is a downward wave (peak to valley).</summary>
        public bool IsDownWave => Start.IsPeak && End.IsValley;

        /// <summary>
        /// Returns a string representation showing the date range.
        /// </summary>
        public override string ToString()
        {
            return $"{Start.Candlestick.Date:MM/dd/yyyy} – {End.Candlestick.Date:MM/dd/yyyy}";
        }
    }
}

