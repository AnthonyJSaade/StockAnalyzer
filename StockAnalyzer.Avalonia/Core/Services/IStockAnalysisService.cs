using System;
using System.Collections.Generic;
using StockAnalyzer.Avalonia.Core.Models;

namespace StockAnalyzer.Avalonia.Core.Services
{
    /// <summary>
    /// Interface for stock analysis operations. Enables dependency injection and unit testing.
    /// </summary>
    public interface IStockAnalysisService
    {
        /// <summary>
        /// Loads candlestick data from a CSV file.
        /// </summary>
        List<Candlestick> LoadFromCsv(string filePath);

        /// <summary>
        /// Filters candlesticks to only include those within the specified date range.
        /// </summary>
        List<Candlestick> FilterByDateRange(List<Candlestick> data, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Detects peaks, valleys, and waves in the candlestick data.
        /// </summary>
        WaveAnalysisResult DetectWaves(List<Candlestick> data, int margin = 1);

        /// <summary>
        /// Calculates Fibonacci retracement levels for a given wave.
        /// </summary>
        List<FibonacciLevel> CalculateFibonacciLevels(Wave wave, List<Candlestick> data);

        /// <summary>
        /// Gets the price range (min/max) with optional padding percentage.
        /// </summary>
        (double Min, double Max) GetPriceRange(List<Candlestick> data, double paddingPercent = 0.05);
    }
}
