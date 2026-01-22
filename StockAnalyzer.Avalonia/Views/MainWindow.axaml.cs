using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Plottables;
using ScottPlot.TickGenerators;
using StockAnalyzer.Avalonia.Core.Models;
using StockAnalyzer.Avalonia.ViewModels;

namespace StockAnalyzer.Avalonia.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext!;
        private AvaPlot Chart => CandlestickChart;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            // Wire up events
            LoadButton.Click += OnLoadButtonClick;
            ViewModel.ChartUpdateRequested += UpdateChart;

            // Configure chart appearance
            ConfigureChart();
        }

        private void ConfigureChart()
        {
            // ScottPlot 5 styling - dark theme
            Chart.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#1E1E1E");
            Chart.Plot.DataBackground.Color = ScottPlot.Color.FromHex("#252526");
            
            Chart.Plot.Axes.Color(ScottPlot.Color.FromHex("#888888"));
            Chart.Plot.Grid.MajorLineColor = ScottPlot.Color.FromHex("#333333");

            Chart.Plot.Axes.Left.Label.Text = "Price ($)";
            Chart.Plot.Axes.Left.Label.ForeColor = ScottPlot.Color.FromHex("#CCCCCC");
        }

        private async void OnLoadButtonClick(object? sender, RoutedEventArgs e)
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select Stock CSV File",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("CSV Files") { Patterns = new[] { "*.csv" } },
                    new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
                }
            });

            if (files.Count > 0)
            {
                var path = files[0].Path.LocalPath;
                ViewModel.LoadFile(path);
            }
        }

        private void UpdateChart()
        {
            Chart.Plot.Clear();

            var candlesticks = ViewModel.Candlesticks.ToList();
            if (candlesticks.Count == 0)
            {
                Chart.Refresh();
                return;
            }

            // Use SEQUENTIAL mode - plot candles at integer positions to get proper width
            // This avoids gaps and makes candlestick bodies visible
            var ohlcs = new List<OHLC>();
            for (int i = 0; i < candlesticks.Count; i++)
            {
                var c = candlesticks[i];
                // Use index as position, with TimeSpan of 1 day for proper width
                ohlcs.Add(new OHLC(
                    (double)c.Open,
                    (double)c.High,
                    (double)c.Low,
                    (double)c.Close,
                    DateTime.FromOADate(i),  // Use sequential index
                    TimeSpan.FromDays(0.8)   // 80% width for spacing
                ));
            }

            // Add candlestick chart
            var candlePlot = Chart.Plot.Add.Candlestick(ohlcs);
            candlePlot.RisingColor = ScottPlot.Color.FromHex("#26A69A");   // Green
            candlePlot.FallingColor = ScottPlot.Color.FromHex("#EF5350"); // Red

            // Create custom tick labels showing actual dates
            var tickPositions = new List<double>();
            var tickLabels = new List<string>();
            
            // Show ~10 ticks evenly spaced
            int tickCount = Math.Min(10, candlesticks.Count);
            int step = Math.Max(1, candlesticks.Count / tickCount);
            
            for (int i = 0; i < candlesticks.Count; i += step)
            {
                tickPositions.Add(i);
                tickLabels.Add(candlesticks[i].Date.ToString("MM/dd"));
            }

            Chart.Plot.Axes.Bottom.TickGenerator = new NumericManual(
                tickPositions.ToArray(), 
                tickLabels.ToArray()
            );
            Chart.Plot.Axes.Bottom.Label.Text = "Date";
            Chart.Plot.Axes.Bottom.Label.ForeColor = ScottPlot.Color.FromHex("#CCCCCC");

            // Set axis limits
            Chart.Plot.Axes.SetLimitsY(ViewModel.ChartMinY, ViewModel.ChartMaxY);
            Chart.Plot.Axes.SetLimitsX(-1, candlesticks.Count);

            // Draw wave annotations if a wave is selected
            var selectedWave = ViewModel.SelectedWave;
            if (selectedWave != null)
            {
                DrawWaveAnnotations(selectedWave, candlesticks);
            }

            Chart.Refresh();
        }

        private void DrawWaveAnnotations(Wave wave, List<Candlestick> candlesticks)
        {
            // Find indices using sequential position
            int startIdx = candlesticks.FindIndex(c => c.Date == wave.Start.Candlestick.Date);
            int endIdx = candlesticks.FindIndex(c => c.Date == wave.End.Candlestick.Date);

            if (startIdx < 0 || endIdx < 0) return;

            // Use sequential indices for X coordinates
            double startX = startIdx;
            double endX = endIdx;
            double startY = (double)wave.StartPrice;
            double endY = (double)wave.EndPrice;
            double yLow = Math.Min(startY, endY);
            double yHigh = Math.Max(startY, endY);

            var waveColor = wave.IsUpWave
                ? ScottPlot.Color.FromHex("#26A69A")
                : ScottPlot.Color.FromHex("#EF5350");

            // Draw bounding rectangle
            var rect = Chart.Plot.Add.Rectangle(startX - 0.5, endX + 0.5, yLow, yHigh);
            rect.LineColor = waveColor;
            rect.LineWidth = 2;
            rect.FillColor = waveColor.WithAlpha(30);

            // Draw diagonal line
            var line = Chart.Plot.Add.Line(startX, startY, endX, endY);
            line.Color = waveColor;
            line.LineWidth = 2;

            // Draw Fibonacci levels
            double range = yHigh - yLow;
            double[] fibs = { 0.0, 0.236, 0.382, 0.5, 0.618, 1.0 };

            foreach (double f in fibs)
            {
                double level = yLow + range * f;
                var fibLine = Chart.Plot.Add.HorizontalLine(level);
                fibLine.Color = waveColor.WithAlpha(150);
                fibLine.LineWidth = 1;
                fibLine.LinePattern = LinePattern.Dashed;

                // Add label at the end of the wave
                var label = Chart.Plot.Add.Text($"{f * 100:F1}%", endX + 1, level);
                label.LabelFontColor = waveColor;
                label.LabelFontSize = 10;
            }
        }
    }
}
