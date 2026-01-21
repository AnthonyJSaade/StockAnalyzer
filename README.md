# Stock Analyzer

A Windows Forms application for technical stock analysis, featuring candlestick charts, peak/valley detection, wave analysis, and Fibonacci retracement levels.

## Features

- **CSV Data Import**: Load stock data from CSV files in OHLCV format (Date, Open, High, Low, Close, Volume)
- **Candlestick Charts**: Beautiful candlestick visualization with customizable date ranges
- **Peak & Valley Detection**: Automatic detection of local price extremes using a configurable margin
- **Wave Analysis**: Identifies upward and downward price waves between detected extremes
- **Fibonacci Retracement**: Displays key Fibonacci levels (0%, 23.6%, 38.2%, 50%, 61.8%, 100%)
- **Interactive Selection**: Rubber-band selection to manually define wave regions
- **Price Simulation**: Animate and simulate price movements within a specified range

## Screenshots

*Coming soon*

## Requirements

- Windows OS
- .NET Framework 4.7.2 or higher
- Visual Studio 2019 or later (for development)

## Getting Started

### Running the Application

1. Open `StockAnalyzer.sln` in Visual Studio
2. Build the solution (Ctrl+Shift+B)
3. Run the application (F5)

### Loading Stock Data

1. Click "Load Stock" on the start screen
2. Select a CSV file containing stock data
3. Adjust the Start Date and End Date to filter the data range
4. The candlestick chart will display with detected waves

### CSV Format

Your CSV files should have the following format:

```csv
Date,Open,High,Low,Close,Volume
2024-01-02,185.53,186.88,183.62,185.24,28987123
2024-01-03,184.22,185.01,183.26,184.12,25000000
...
```

- **Date**: YYYY-MM-DD format
- **Open/High/Low/Close**: Decimal prices
- **Volume**: Integer trading volume

### Using the Chart

- **Up Waves / Down Waves**: Lists of detected price waves. Click to select and highlight.
- **Rubber-band Selection**: Click and drag on the chart to manually select a wave region
- **+/- Buttons**: Manually adjust the end price of the selected wave
- **% of Range**: Slider to set the simulation range percentage
- **# of Steps**: Slider to set the number of animation steps
- **Start/Stop**: Animate through the price simulation

## Technical Overview

### Architecture

| File | Description |
|------|-------------|
| `Candlestick.cs` | OHLCV data model for individual candlesticks |
| `StockReader.cs` | CSV file parser that creates Candlestick lists |
| `PeakValley.cs` | Peak/valley detection algorithm |
| `Wave.cs` | Wave model representing price movement between extremes |
| `Form_Start.cs` | Entry form for date selection and file loading |
| `Form_ChartDisplay.cs` | Main chart display with all technical analysis features |

### Key Algorithms

- **Peak Detection**: A candlestick is a peak if its High is greater than the High of all neighbors within the margin
- **Valley Detection**: A candlestick is a valley if its Low is less than the Low of all neighbors within the margin
- **Wave Classification**: Up waves go from valley to peak; down waves go from peak to valley

## License

MIT License - See [LICENSE](LICENSE) for details.

## Author

Built as a class project demonstrating Windows Forms development and technical stock analysis concepts.
