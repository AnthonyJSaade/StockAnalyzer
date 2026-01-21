using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StockAnalyzer
{
    /// <summary>
    /// Represents a peak or valley point in the candlestick data.
    /// </summary>
    public class PeakValley
    {
        /// <summary>Gets or sets the candlestick at this peak/valley.</summary>
        public Candlestick Candlestick { get; set; }
        
        /// <summary>Gets or sets whether this point is a peak (local maximum).</summary>
        public bool IsPeak { get; set; }
        
        /// <summary>Gets or sets whether this point is a valley (local minimum).</summary>
        public bool IsValley { get; set; }
        
        /// <summary>Gets or sets the left margin used for detection.</summary>
        public int LeftMargin { get; set; }
        
        /// <summary>Gets or sets the right margin used for detection.</summary>
        public int RightMargin { get; set; }

        /// <summary>
        /// Initializes a new PeakValley instance.
        /// </summary>
        public PeakValley(Candlestick candlestick, bool isPeak, bool isValley, int leftMargin, int rightMargin)
        {
            Candlestick = candlestick;
            IsPeak = isPeak;
            IsValley = isValley;
            LeftMargin = leftMargin;
            RightMargin = rightMargin;
        }
    }

    /// <summary>
    /// Detects peaks and valleys in candlestick data.
    /// </summary>
    public static class PeakValleyDetector
    {
        /// <summary>
        /// Finds all peaks and valleys in the given candlestick list.
        /// </summary>
        /// <param name="candlesticks">The list of candlesticks to analyze.</param>
        /// <param name="margin">The number of neighboring candlesticks to compare on each side.</param>
        /// <returns>A list of detected peaks and valleys.</returns>
        public static List<PeakValley> FindPeaksAndValleys(List<Candlestick> candlesticks, int margin)
        {
            List<PeakValley> peaksAndValleys = new List<PeakValley>();

            for (int i = margin; i < candlesticks.Count - margin; i++)
            {
                bool isPeak = true;
                bool isValley = true;

                // Check for peak or valley by comparing with neighbors
                for (int j = 1; j <= margin; j++)
                {
                    if (candlesticks[i].High <= candlesticks[i - j].High || candlesticks[i].High <= candlesticks[i + j].High)
                    {
                        isPeak = false;   // If any neighbor is higher, not a peak
                    }
                    if (candlesticks[i].Low >= candlesticks[i - j].Low || candlesticks[i].Low >= candlesticks[i + j].Low)
                    {
                        isValley = false;   // If any neighbor is lower, not a valley
                    }
                }

                if (isPeak || isValley)
                {
                    peaksAndValleys.Add(new PeakValley(
                        candlesticks[i],  // The current candlestick
                        isPeak,           // Whether it's a peak
                        isValley,         // Whether it's a valley
                        margin,           // Left margin
                        margin            // Right margin
                    ));
                }
            }

            return peaksAndValleys;
        }
    }
}
