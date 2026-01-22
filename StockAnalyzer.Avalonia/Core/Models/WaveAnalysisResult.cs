using System.Collections.Generic;

namespace StockAnalyzer.Avalonia.Core.Models
{
    /// <summary>
    /// Contains results from wave analysis including all detected peaks, valleys, and waves.
    /// </summary>
    public class WaveAnalysisResult
    {
        public List<PeakValley> PeaksAndValleys { get; set; } = new();
        public List<Wave> UpWaves { get; set; } = new();
        public List<Wave> DownWaves { get; set; } = new();
    }
}
