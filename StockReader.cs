using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StockAnalyzer
{
    /// <summary>
    /// Reads stock data from CSV files and converts it to Candlestick objects.
    /// </summary>
    public class StockReader
    {
        /// <summary>
        /// Reads candlestick data from a CSV file.
        /// </summary>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <returns>A list of Candlestick objects parsed from the file.</returns>
        public List<Candlestick> ReadCandlesticksFromCsv(string filePath)
        {
            var candlesticks = new List<Candlestick>();
            
            using (var reader = new StreamReader(filePath))
            {
                var header = reader.ReadLine();   // Skip the header line of the file

                int lineNumber = 1;
                while (!reader.EndOfStream)
                {
                    lineNumber++;
                    var line = reader.ReadLine();
                    
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    try
                    {
                        var candlestick = new Candlestick(line);
                        candlesticks.Add(candlestick);
                    }
                    catch (Exception ex)
                    {
                        // Skip malformed lines but log the issue
                        System.Diagnostics.Debug.WriteLine($"Warning: Skipped line {lineNumber} - {ex.Message}");
                    }
                }
            }
            
            return candlesticks;
        }
    }
}

