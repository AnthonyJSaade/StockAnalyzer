using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StockAnalyzer.Avalonia.Core.Models;

namespace StockAnalyzer.Avalonia.Core.Services
{
    /// <summary>
    /// Pure business logic for stock analysis. Has zero UI dependencies.
    /// Extracted from the original WinForms Form_ChartDisplay.cs.
    /// </summary>
    public class StockAnalysisService : IStockAnalysisService
    {
        private static readonly double[] FibonacciRatios = { 0.0, 0.236, 0.382, 0.5, 0.618, 1.0 };

        /// <inheritdoc />
        public List<Candlestick> LoadFromCsv(string filePath)
        {
            var candlesticks = new List<Candlestick>();

            using var reader = new StreamReader(filePath);
            reader.ReadLine(); // Skip header

            int lineNumber = 1;
            while (!reader.EndOfStream)
            {
                lineNumber++;
                var line = reader.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    candlesticks.Add(new Candlestick(line));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Warning: Skipped line {lineNumber} - {ex.Message}");
                }
            }

            // Ensure chronological order
            if (candlesticks.Count >= 2 && candlesticks[0].Date > candlesticks[1].Date)
            {
                candlesticks.Reverse();
            }

            return candlesticks;
        }

        /// <inheritdoc />
        public List<Candlestick> FilterByDateRange(List<Candlestick> data, DateTime startDate, DateTime endDate)
        {
            return data
                .Where(c => c.Date >= startDate && c.Date <= endDate)
                .OrderBy(c => c.Date)
                .ToList();
        }

        /// <inheritdoc />
        public WaveAnalysisResult DetectWaves(List<Candlestick> data, int margin = 1)
        {
            var result = new WaveAnalysisResult();

            if (data == null || data.Count < (margin * 2) + 1)
                return result;

            // Find peaks and valleys
            for (int i = margin; i < data.Count - margin; i++)
            {
                bool isPeak = true;
                bool isValley = true;

                for (int j = 1; j <= margin; j++)
                {
                    if (data[i].High <= data[i - j].High || data[i].High <= data[i + j].High)
                        isPeak = false;

                    if (data[i].Low >= data[i - j].Low || data[i].Low >= data[i + j].Low)
                        isValley = false;
                }

                if (isPeak || isValley)
                {
                    result.PeaksAndValleys.Add(new PeakValley(data[i], isPeak, isValley, margin));
                }
            }

            // Build waves from consecutive peaks/valleys
            for (int i = 0; i < result.PeaksAndValleys.Count - 1; i++)
            {
                var wave = new Wave
                {
                    Start = result.PeaksAndValleys[i],
                    End = result.PeaksAndValleys[i + 1]
                };

                if (wave.IsUpWave)
                    result.UpWaves.Add(wave);
                else if (wave.IsDownWave)
                    result.DownWaves.Add(wave);
            }

            return result;
        }

        /// <inheritdoc />
        public List<FibonacciLevel> CalculateFibonacciLevels(Wave wave, List<Candlestick> data)
        {
            var levels = new List<FibonacciLevel>();

            if (wave == null || data == null || data.Count == 0)
                return levels;

            double yLow = Math.Min((double)wave.StartPrice, (double)wave.EndPrice);
            double yHigh = Math.Max((double)wave.StartPrice, (double)wave.EndPrice);
            double range = yHigh - yLow;

            // Get candlesticks between wave start and end
            var between = data
                .Where(c => c.Date > wave.Start.Candlestick.Date && c.Date < wave.End.Candlestick.Date)
                .ToList();

            foreach (double ratio in FibonacciRatios)
            {
                double price = yLow + range * ratio;

                // Count confirmations: bars whose High >= level && Low <= level
                int confirmations = between.Count(c =>
                    (double)c.High >= price && (double)c.Low <= price);

                levels.Add(new FibonacciLevel(ratio, price, confirmations));
            }

            return levels;
        }

        /// <inheritdoc />
        public (double Min, double Max) GetPriceRange(List<Candlestick> data, double paddingPercent = 0.05)
        {
            if (data == null || data.Count == 0)
                return (0, 100);

            double minValue = (double)data.Min(c => c.Low);
            double maxValue = (double)data.Max(c => c.High);

            double padding = (maxValue - minValue) * paddingPercent;

            return (minValue - padding, maxValue + padding);
        }
    }
}
