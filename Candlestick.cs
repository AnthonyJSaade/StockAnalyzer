using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StockAnalyzer
{
    /// <summary>
    /// Represents a single candlestick containing OHLCV (Open, High, Low, Close, Volume) data for a specific date.
    /// </summary>
    public class Candlestick
    {
        /// <summary>Gets or sets the date of the trading period.</summary>
        public DateTime Date { get; set; }
        
        /// <summary>Gets or sets the opening price of the stock.</summary>
        public decimal Open { get; set; }
        
        /// <summary>Gets or sets the highest price of the stock during the trading period.</summary>
        public decimal High { get; set; }
        
        /// <summary>Gets or sets the lowest price of the stock during the trading period.</summary>
        public decimal Low { get; set; }
        
        /// <summary>Gets or sets the closing price of the stock.</summary>
        public decimal Close { get; set; }
        
        /// <summary>Gets or sets the trading volume.</summary>
        public ulong Volume { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Candlestick()
        {
        }

        /// <summary>
        /// Initializes a candlestick with the specified OHLCV values.
        /// </summary>
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
        /// <param name="data">A comma-separated string containing Date,Open,High,Low,Close,Volume.</param>
        /// <exception cref="ArgumentException">Thrown when the data format is invalid.</exception>
        public Candlestick(string data)
        {
            var separators = new char[] { ',', '\"' };
            var values = data.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length != 6)
            {
                throw new ArgumentException("Invalid data format. Expected 6 values separated by commas (Date, Open, High, Low, Close, Volume).");
            }

            Date = DateTime.ParseExact(values[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Open = Math.Round(decimal.Parse(values[1], CultureInfo.InvariantCulture), 2);
            High = Math.Round(decimal.Parse(values[2], CultureInfo.InvariantCulture), 2);
            Low = Math.Round(decimal.Parse(values[3], CultureInfo.InvariantCulture), 2);
            Close = Math.Round(decimal.Parse(values[4], CultureInfo.InvariantCulture), 2);
            Volume = ulong.Parse(values[5], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a string representation of the candlestick.
        /// </summary>
        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd}, Open: {Open}, High: {High}, Low: {Low}, Close: {Close}, Volume: {Volume}";
        }
    }
}

