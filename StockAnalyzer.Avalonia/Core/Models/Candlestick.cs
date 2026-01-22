using System;
using System.Globalization;

namespace StockAnalyzer.Avalonia.Core.Models
{
    /// <summary>
    /// Represents a single candlestick containing OHLCV data for a specific date.
    /// </summary>
    public class Candlestick
    {
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public ulong Volume { get; set; }

        public Candlestick() { }

        public Candlestick(DateTime date, decimal open, decimal high, decimal low, decimal close, ulong volume)
        {
            Date = date;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        /// <summary>
        /// Creates a candlestick by parsing a CSV line.
        /// </summary>
        public Candlestick(string csvLine)
        {
            var separators = new char[] { ',', '"' };
            var values = csvLine.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length != 6)
                throw new ArgumentException("Expected 6 values: Date,Open,High,Low,Close,Volume");

            Date = DateTime.ParseExact(values[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Open = Math.Round(decimal.Parse(values[1], CultureInfo.InvariantCulture), 2);
            High = Math.Round(decimal.Parse(values[2], CultureInfo.InvariantCulture), 2);
            Low = Math.Round(decimal.Parse(values[3], CultureInfo.InvariantCulture), 2);
            Close = Math.Round(decimal.Parse(values[4], CultureInfo.InvariantCulture), 2);
            Volume = ulong.Parse(values[5], CultureInfo.InvariantCulture);
        }

        public override string ToString() => $"{Date:yyyy-MM-dd} O:{Open} H:{High} L:{Low} C:{Close}";
    }
}
