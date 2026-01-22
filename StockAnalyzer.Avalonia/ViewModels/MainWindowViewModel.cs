using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StockAnalyzer.Avalonia.Core.Models;
using StockAnalyzer.Avalonia.Core.Services;

namespace StockAnalyzer.Avalonia.ViewModels
{
    /// <summary>
    /// Main ViewModel that connects the StockAnalysisService to the View.
    /// Follows MVVM pattern for clean separation of concerns.
    /// </summary>
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IStockAnalysisService _analysisService;

        [ObservableProperty]
        private string _title = "Stock Analyzer - Avalonia";

        [ObservableProperty]
        private string _statusMessage = "Load a CSV file to begin";

        [ObservableProperty]
        private string? _loadedFilePath;

        [ObservableProperty]
        private DateTimeOffset? _startDate = DateTimeOffset.Now.AddYears(-1);

        [ObservableProperty]
        private DateTimeOffset? _endDate = DateTimeOffset.Now;

        [ObservableProperty]
        private ObservableCollection<Candlestick> _candlesticks = new();

        [ObservableProperty]
        private ObservableCollection<Wave> _upWaves = new();

        [ObservableProperty]
        private ObservableCollection<Wave> _downWaves = new();

        [ObservableProperty]
        private Wave? _selectedWave;

        [ObservableProperty]
        private ObservableCollection<FibonacciLevel> _fibonacciLevels = new();

        [ObservableProperty]
        private string _priceRangeText = "";

        [ObservableProperty]
        private int _confirmationCount;

        [ObservableProperty]
        private double _chartMinY;

        [ObservableProperty]
        private double _chartMaxY;

        // Event to notify View that chart needs updating
        public event Action? ChartUpdateRequested;

        public MainWindowViewModel() : this(new StockAnalysisService())
        {
        }

        public MainWindowViewModel(IStockAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        /// <summary>
        /// Loads and processes data from a CSV file.
        /// </summary>
        public void LoadFile(string filePath)
        {
            try
            {
                LoadedFilePath = filePath;
                var allData = _analysisService.LoadFromCsv(filePath);

                if (allData.Count == 0)
                {
                    StatusMessage = "No valid data found in file";
                    return;
                }

                // Update date range to match data
                var minDate = allData.Min(c => c.Date);
                var maxDate = allData.Max(c => c.Date);
                StartDate = new DateTimeOffset(minDate);
                EndDate = new DateTimeOffset(maxDate);

                ApplyFilter(allData);
                StatusMessage = $"Loaded {allData.Count} candlesticks from {System.IO.Path.GetFileName(filePath)}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading file: {ex.Message}";
            }
        }

        /// <summary>
        /// Applies date filter and reprocesses data.
        /// </summary>
        [RelayCommand]
        private void ApplyDateFilter()
        {
            if (string.IsNullOrEmpty(LoadedFilePath))
                return;

            var allData = _analysisService.LoadFromCsv(LoadedFilePath);
            ApplyFilter(allData);
        }

        private void ApplyFilter(System.Collections.Generic.List<Candlestick> allData)
        {
            var filtered = _analysisService.FilterByDateRange(
                allData, 
                StartDate?.DateTime ?? DateTime.MinValue, 
                EndDate?.DateTime ?? DateTime.MaxValue);

            if (filtered.Count == 0)
            {
                StatusMessage = "No data in selected date range";
                return;
            }

            // Update candlesticks
            Candlesticks = new ObservableCollection<Candlestick>(filtered);

            // Get price range for chart
            var range = _analysisService.GetPriceRange(filtered);
            ChartMinY = range.Min;
            ChartMaxY = range.Max;

            // Detect waves
            var waveResult = _analysisService.DetectWaves(filtered);
            UpWaves = new ObservableCollection<Wave>(waveResult.UpWaves);
            DownWaves = new ObservableCollection<Wave>(waveResult.DownWaves);

            // Clear selection
            SelectedWave = null;
            FibonacciLevels.Clear();
            PriceRangeText = "";
            ConfirmationCount = 0;

            // Notify view to update chart
            ChartUpdateRequested?.Invoke();
        }

        /// <summary>
        /// Called when a wave is selected - calculates Fibonacci levels.
        /// </summary>
        partial void OnSelectedWaveChanged(Wave? value)
        {
            if (value == null)
            {
                FibonacciLevels.Clear();
                PriceRangeText = "";
                ConfirmationCount = 0;
                return;
            }

            var levels = _analysisService.CalculateFibonacciLevels(value, Candlesticks.ToList());
            FibonacciLevels = new ObservableCollection<FibonacciLevel>(levels);

            double low = Math.Min((double)value.StartPrice, (double)value.EndPrice);
            double high = Math.Max((double)value.StartPrice, (double)value.EndPrice);
            PriceRangeText = $"${low:F2} – ${high:F2}";
            ConfirmationCount = levels.Sum(l => l.Confirmations);

            // Notify view to update chart with wave annotations
            ChartUpdateRequested?.Invoke();
        }
    }
}
