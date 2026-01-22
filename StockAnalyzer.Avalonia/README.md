# Stock Analyzer - Avalonia (Cross-Platform)

A cross-platform port of the Windows Forms Stock Analyzer application, built with Avalonia UI and ScottPlot 5.

## Features

- **Cross-Platform**: Runs natively on macOS, Windows, and Linux
- **Candlestick Charts**: Beautiful OHLC visualization using ScottPlot 5
- **Wave Detection**: Automatic detection of up/down price waves
- **Fibonacci Levels**: Display key retracement levels (0%, 23.6%, 38.2%, 50%, 61.8%, 100%)
- **MVVM Architecture**: Clean separation of concerns for testability

## Architecture

```
StockAnalyzer.Avalonia/
├── Core/                    # Pure business logic (no UI dependencies)
│   ├── Models/              # Candlestick, Wave, PeakValley, etc.
│   └── Services/            # StockAnalysisService
├── ViewModels/              # MVVM ViewModels
└── Views/                   # Avalonia XAML UI
```

## Requirements

- .NET 8.0 SDK
- macOS 10.15+ / Windows 10+ / Linux

## Running the App

```bash
cd StockAnalyzer.Avalonia
dotnet restore
dotnet run
```

## Usage

1. Click **"Load CSV"** to select a stock data file
2. Adjust the **Start/End dates** to filter the data range
3. Click **"Apply Filter"** to update the chart
4. Select a wave from **Up Waves** or **Down Waves** lists
5. View **Fibonacci levels** and **confirmations** in the sidebar

## CSV Format

```csv
Date,Open,High,Low,Close,Volume
2024-01-02,185.53,186.88,183.62,185.24,28987123
```

## Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| Avalonia | 11.2.3 | Cross-platform UI framework |
| ScottPlot.Avalonia | 5.0.47 | Charting (candlesticks) |
| CommunityToolkit.Mvvm | 8.3.2 | MVVM source generators |

## License

MIT License
