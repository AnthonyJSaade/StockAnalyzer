using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace StockAnalyzer
{
    public partial class Form_ChartDisplay : Form
    {
        public string filePath;      // Holds file path to CSV file
        List<Candlestick> candlesticks;   // Creates the list of candlesticks
        private List<Candlestick> displayedCandlesticks;
        StockReader stockReader;          // Creates StockReader variable
        string filename;             // Holds the name of the CSV file
        List<Wave> upWaves = new List<Wave>();        // List of upward waves
        List<Wave> downWaves = new List<Wave>();     // List of downward waves
        List<PeakValley> peaksAndValleys = new List<PeakValley>();  // List of peaks and valleys (initialized to prevent null) 
        DateTime start;    //start date
        DateTime end;         //end date
       
        private bool isRubberBanding = false;     // Rubber-band selection boolean
        private PeakValley rubberStartPV;      // Starting candlestick for rubber-banding
        private RectangleAnnotation rubberRect;     // Rectangle annotation
        private Wave selectedWave;           // Selected wave from boxes
        private double simulatedEndY;         // Ending price of simulation
       
        private double percentRange = 0.1;   // Initialize percent range before user input
        private int numSteps = 10;      // Initialize number of steps before user input

        private bool isSimulating = false;       // Initialize boolean for simulating
        private int currentStep = 0;         // Counts which step we're on
        private double simulationBottom;     // Holds the simulation bottom
        private double simulationTop;        // Holds the simulation top

        /// <summary>
        /// Default constructor - initializes the form with all required event handlers.
        /// </summary>
        public Form_ChartDisplay()
        {
            InitializeComponent();
            stockReader = new StockReader();  // Initialize stockReader to prevent null reference
            
            // Event handlers for rubber-band and selection
            chart_OHLC.MouseDown += Chart_OHLC_MouseDown;
            chart_OHLC.MouseMove += Chart_OHLC_MouseMove;
            chart_OHLC.MouseUp += Chart_OHLC_MouseUp;
            
            // Wire each ListBox to its handler
            listBox_UpWaves.SelectedIndexChanged += listBox_UpWaves_SelectedIndexChanged;
            listBox_DownWaves.SelectedIndexChanged += listBox_DownWaves_SelectedIndexChanged;
            hScrollBar_Range.Scroll += hScrollBar_Range_Scroll;
            hScrollBar_Step.Scroll += hScrollBar_Step_Scroll;
        }

        /// <summary>
        /// Constructor that loads and displays stock data from a CSV file.
        /// </summary>
        /// <param name="originalFilePath">Path to the CSV file containing stock data.</param>
        /// <param name="startDate">Start date for filtering the data.</param>
        /// <param name="endDate">End date for filtering the data.</param>
        public Form_ChartDisplay(string originalFilePath, DateTime startDate, DateTime endDate)
        {
            InitializeComponent();

            stockReader = new StockReader();       // Creates StockReader variable

            filePath = originalFilePath;        // Assigns the file path
            start = startDate;      // Sets start date for chart
            end = endDate;         // Sets end date for chart

            // Wire event handlers (same as default constructor)
            chart_OHLC.MouseDown += Chart_OHLC_MouseDown;
            chart_OHLC.MouseMove += Chart_OHLC_MouseMove;
            chart_OHLC.MouseUp += Chart_OHLC_MouseUp;
            listBox_UpWaves.SelectedIndexChanged += listBox_UpWaves_SelectedIndexChanged;
            listBox_DownWaves.SelectedIndexChanged += listBox_DownWaves_SelectedIndexChanged;
            hScrollBar_Range.Scroll += hScrollBar_Range_Scroll;
            hScrollBar_Step.Scroll += hScrollBar_Step_Scroll;

            loadAndDisplay();          // Loads and displays the chart

            Show();               // Displays form
        }

        /// <summary>
        /// Loads candlestick data from a CSV file and ensures chronological order.
        /// </summary>
        private List<Candlestick> loadTicker(string filename)
        {
            List<Candlestick> listOfCandlesticks = stockReader.ReadCandlesticksFromCsv(filename);  // Reads candlestick data from CSV
            
            if (listOfCandlesticks == null || listOfCandlesticks.Count < 2)
            {
                return listOfCandlesticks ?? new List<Candlestick>();
            }
            
            Candlestick firstCandlestick = listOfCandlesticks[0];       // Gets first candlestick
            Candlestick secondCandlestick = listOfCandlesticks[1];      // Gets second candlestick
            if (firstCandlestick.Date > secondCandlestick.Date)
            {
                listOfCandlesticks.Reverse();     // Reverse the list to chronological order
            }
            return listOfCandlesticks;
        }

        public void loadAndDisplay()
        {
            Text = filePath;                //title = file path
            candlesticks = loadTicker(filePath);           //loads candlestick ist from file

            if (candlesticks == null || candlesticks.Count == 0)
            {
                MessageBox.Show("No valid data found in the selected file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<Candlestick> listOfCandlesticks = FilterCandlesticksByDate(candlesticks, start, end); //calls function to filtercnadlesticks and stores returned list in the variable
            displayedCandlesticks = listOfCandlesticks;
            if (listOfCandlesticks.Count == 0)
            {
                MessageBox.Show("No data matches the selected date range.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            displayStock(listOfCandlesticks);        //shows the chart
            InitializeWaveDetection(listOfCandlesticks);      //finds waves
        }

        public List<Candlestick> FilterCandlesticksByDate(List<Candlestick> candlesticks, DateTime startDate, DateTime endDate)   //filter candlesticks to contain the candlestick data within the provided date range
        {
            return candlesticks.Where(c => c.Date >= startDate && c.Date <= endDate).ToList(); //returns the filter candlesticks between the provided start and end date
        }
        private void normalizeChart(List<Candlestick> listOfCandlesticks)        //method that makes the chart bound by the min and max value of the stock
        {
            if (listOfCandlesticks == null || listOfCandlesticks.Count == 0)
            {
                MessageBox.Show("No candlestick data available to display.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            decimal minValue = listOfCandlesticks.Min(c => c.Low);        //finds the minimum value of the stock and sets minValue equal to that number
            decimal maxValue = listOfCandlesticks.Max(c => c.High);      //finds the maximum value of the stock and sets maxValue equal to that number

            decimal padding = (maxValue - minValue) * 0.05m;   // Adds a 5% padding
            decimal minY = minValue - padding;             // Makes the minimum y the chart reaches to be the lowest the stock reaches minus 5% padding
            decimal maxY = maxValue + padding;           // Makes the maximum y the chart reaches to be the highest the stock reaches plus 5% padding

            chart_OHLC.ChartAreas[0].AxisY.Minimum = (double)minY;    // Assigns the chart minimum y value the y-axis reaches to the computed variable minY
            chart_OHLC.ChartAreas[0].AxisY.Maximum = (double)maxY;    // Assigns the chart maximum y value to the computed variable maxY

            chart_OHLC.ChartAreas[0].AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;   //spaces the dates on the x-axis

        }

        public void displayStock(List<Candlestick> listOfCandlesticks)   //method that displays the stock data by binding data to table and chart
        {
            listOfCandlesticks = listOfCandlesticks.OrderBy(c => c.Date).ToList();     //orders the list in acsending chronological order

            candlestickBindingSource.DataSource = listOfCandlesticks;             //binds the candlestick data to the table
            normalizeChart(listOfCandlesticks);                      //calles function to arrange the chart y-axis

            chart_OHLC.DataSource = listOfCandlesticks;              //binds candlestick data to chart
            chart_OHLC.DataBind();               //displays the data and refreshes the chart
        }



        private void InitializeWaveDetection(List<Candlestick> list)
        {
            int margin = 1;              //default margin
            peaksAndValleys = PeakValleyDetector.FindPeaksAndValleys(list, margin);        //calls function to find peaks and valleys

            upWaves = new List<Wave>();         //initializes variables for waves
            downWaves = new List<Wave>();
            for (int i = 0; i < peaksAndValleys.Count - 1; i++)              //finds waves
            {
                var wave = new Wave { Start = peaksAndValleys[i], End = peaksAndValleys[i + 1] };
                if (wave.IsUpWave) upWaves.Add(wave);           //adds to upwave list
                if (wave.IsDownWave) downWaves.Add(wave);          //adds to downwave list
            }

            listBox_UpWaves.DataSource = upWaves;          //puts upwaves in box
            listBox_DownWaves.DataSource = downWaves;     //puts downwaves in box
        }

        private void ListBox_Waves_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAnnotations();     //clears any old annotations
            Wave wave = null;
            if (sender == listBox_UpWaves && listBox_UpWaves.SelectedItem is Wave uw)  //checks if upwave is selected 
                wave = uw;
            else if (sender == listBox_DownWaves && listBox_DownWaves.SelectedItem is Wave dw)    //checks if downwave is selected 
                wave = dw;
            if (wave != null) {
                // initialize simulatedEndY to the wave’strue extreme
                simulatedEndY = wave.IsUpWave
                              ? (double)wave.End.Candlestick.High   //peak for an upwave
                              : (double)wave.End.Candlestick.Low;   //valley for a dow‐wave

                DrawWaveAnnotations(wave, simulatedEndY);       //draws thw wave selected
            }
        }

        private void Chart_OHLC_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (peaksAndValleys == null || peaksAndValleys.Count == 0) return;  // Guard against null/empty
            
            var ca = chart_OHLC.ChartAreas[0];            // Initializes chart area 
            double xVal = ca.AxisX.PixelPositionToValue(e.X);    // Initializes x value of mouse
            
            rubberStartPV = peaksAndValleys.OrderBy(pv => Math.Abs(pv.Candlestick.Date.ToOADate() - xVal)).First();   // Find nearest PeakValley
            isRubberBanding = true;        // Sets boolean to true
            rubberRect = new RectangleAnnotation       // Creates rectangle
            {
                AxisX = ca.AxisX,
                AxisY = ca.AxisY,
                ClipToChartArea = ca.Name,
                IsSizeAlwaysRelative = false,
                LineColor = rubberStartPV.IsValley ? Color.Green : Color.Red
            };
            chart_OHLC.Annotations.Add(rubberRect);   // Draws rectangle
        }

        private void Chart_OHLC_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isRubberBanding || rubberRect == null) return;
            var ca = chart_OHLC.ChartAreas[0];
            double x0 = rubberStartPV.Candlestick.Date.ToOADate();
            double y0 = rubberStartPV.IsPeak
                            ? (double)rubberStartPV.Candlestick.High
                            : (double)rubberStartPV.Candlestick.Low; double x1 = ca.AxisX.PixelPositionToValue(e.X);
            // find nearest candlestick date to x1
            var endPV = peaksAndValleys.OrderBy(pv => Math.Abs(pv.Candlestick.Date.ToOADate() - x1)).First();
            double y1 = endPV.IsPeak
                            ? (double)endPV.Candlestick.High
                            : (double)endPV.Candlestick.Low;            //updates rectangle
            rubberRect.X = x0;
            rubberRect.Y = isRubberBanding && rubberStartPV.IsPeak ? Math.Max(y0, y1) : Math.Min(y0, y1);
            rubberRect.Width = endPV.Candlestick.Date.ToOADate() - x0;
            rubberRect.Height = (isRubberBanding && rubberStartPV.IsPeak)
                ? Math.Min(y0, y1) - Math.Max(y0, y1)
                : Math.Max(y0, y1) - Math.Min(y0, y1);
        }

        private void Chart_OHLC_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isRubberBanding) return;        //stops the rubberbanding
            isRubberBanding = false;
           
            var ca = chart_OHLC.ChartAreas[0];
            double x1 = ca.AxisX.PixelPositionToValue(e.X);
            var endPV = peaksAndValleys.OrderBy(pv => Math.Abs(pv.Candlestick.Date.ToOADate() - x1)).First();    //determine end PV
            var wave = new Wave { Start = rubberStartPV, End = endPV };
            simulatedEndY = wave.IsUpWave
                  ? (double)wave.End.Candlestick.High   //peak for upwave
                  : (double)wave.End.Candlestick.Low;   //valley for downwave

            if (wave.IsUpWave || wave.IsDownWave)
            {
                ClearAnnotations();
                DrawWaveAnnotations(wave, simulatedEndY);
                //update selections
                if (wave.IsUpWave) listBox_UpWaves.SelectedItem = wave;
                else listBox_DownWaves.SelectedItem = wave;
            }
            else
            {
                MessageBox.Show("Invalid wave selection. Please start at an extreme and end at the opposite extreme.", "Invalid Wave", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ClearAnnotations();
            }
        
            chart_OHLC.Annotations.Remove(rubberRect);             //remove rubberRect
            rubberRect = null;
        }

        private void DrawWaveAnnotations(Wave wave, double overrideEndY)
        {
            var ca = chart_OHLC.ChartAreas[0];

            //find start/end indexes in the displayed slice
            int startIdx = displayedCandlesticks
                .FindIndex(c => c.Date == wave.Start.Candlestick.Date);
            int endIdx = displayedCandlesticks
                .FindIndex(c => c.Date == wave.End.Candlestick.Date);

            //determine the two extreme prices
            double startY = wave.IsUpWave
                ? (double)wave.Start.Candlestick.Low   //valley
                : (double)wave.Start.Candlestick.High; //peak

            // use the user value here instead of the original extreme
            double endY = overrideEndY;

            //track min/max for fibs & labels
            double yLow = Math.Min(startY, endY);
            double yHigh = Math.Max(startY, endY);

            //clear previous annotations
            chart_OHLC.Annotations.Clear();

            //bounding rectangle
            var rect = new RectangleAnnotation
            {
                AxisX = ca.AxisX,
                AxisY = ca.AxisY,
                ClipToChartArea = ca.Name,
                IsSizeAlwaysRelative = false,

                AnchorX = startIdx,
                AnchorY = yHigh,
                AnchorAlignment = ContentAlignment.TopLeft,

                Width = (endIdx - startIdx) + 1,
                Height = yLow - yHigh,    //negative draws downward

                LineColor = wave.IsUpWave ? Color.Green : Color.Red,
                LineWidth = 2,
                BackColor = Color.Transparent
            };
            chart_OHLC.Annotations.Add(rect);

            //diagonal from (startIdx, startY) → (endIdx, endY)
            var diag = new LineAnnotation
            {
                AxisX = ca.AxisX,
                AxisY = ca.AxisY,
                ClipToChartArea = ca.Name,
                IsSizeAlwaysRelative = false,

                X = startIdx,
                Y = startY,
                Width = (endIdx - startIdx) + 1,  //cover full span
                Height = endY - startY,            //positive up, negative down

                LineColor = rect.LineColor,
                LineWidth = 2
            };
            chart_OHLC.Annotations.Add(diag);

            //Fibonacci lines & confirmation counts
            double range = yHigh - yLow;
            double[] fibs = { 0.0, 0.236, 0.382, 0.5, 0.618, 1.0 };
            int confirms = 0;
            double tol = range * 0.005;

            foreach (double f in fibs)
            {
                double level = yLow + range * f;
                var fibLine = new HorizontalLineAnnotation
                {
                    AxisY = ca.AxisY,
                    ClipToChartArea = ca.Name,
                    IsInfinitive = true,
                    Y = level,
                    LineDashStyle = ChartDashStyle.Dash,
                    LineWidth = 1,
                    LineColor = rect.LineColor
                };
                chart_OHLC.Annotations.Add(fibLine);

                //count any bar whose High>=level & Low<=level
                var between = displayedCandlesticks
                    .Where(c => c.Date > wave.Start.Candlestick.Date
                             && c.Date < wave.End.Candlestick.Date);
                if (between.Any(c =>
                    (double)c.High >= level &&
                    (double)c.Low <= level))
                {
                    confirms++;
                }


                
            }

            //update labels
            label_Price.Text = $"{yLow:C} – {yHigh:C}";
            label_Confirmations.Text = confirms.ToString();
        }





        private void ClearAnnotations()
        {
            chart_OHLC.Annotations.Clear();
        }

        private void listBox_UpWaves_SelectedIndexChanged(object sender, EventArgs e)
        {
            //get the selected up-wave
            var wave = listBox_UpWaves.SelectedItem as Wave;
            if (wave == null) return;

            // clear old annotations
            chart_OHLC.Annotations.Clear();
            selectedWave = wave;
            simulatedEndY = wave.IsUpWave
                  ? (double)wave.End.Candlestick.High   //peak for upwave
                  : (double)wave.End.Candlestick.Low;   //valley for downwave

            //draw the rectangle + Fibonacci lines + confirmations
            DrawWaveAnnotations(wave, simulatedEndY);
        }

        private void listBox_DownWaves_SelectedIndexChanged(object sender, EventArgs e)
        {
            //get the selected dowmwave
            var wave = listBox_DownWaves.SelectedItem as Wave;
            if (wave == null) return;

            //clear old annotations
            chart_OHLC.Annotations.Clear();
            selectedWave = wave;
            simulatedEndY = wave.IsUpWave
                  ? (double)wave.End.Candlestick.High   // peak for upwave
                  : (double)wave.End.Candlestick.Low;   // valley for downwave

            //draw the rectangle + Fibonacci lines + confirmations
            DrawWaveAnnotations(wave, simulatedEndY);
        }


  

        private void button_Plus_Click(object sender, EventArgs e)
        {
            if (selectedWave == null) return;

            //recompute the wave range
            double yLow = selectedWave.IsUpWave
                           ? (double)selectedWave.Start.Candlestick.Low
                           : (double)selectedWave.End.Candlestick.Low;
            double yHigh = selectedWave.IsUpWave
                           ? (double)selectedWave.End.Candlestick.High
                           : (double)selectedWave.Start.Candlestick.High;
            double range = yHigh - yLow;

            //new step size = 2 * percentRange * range
            double stepSize = 2 * percentRange * range;

            //move the simulation end‐price up by that step
            simulatedEndY += stepSize;

            //redraw with the adjusted endprice
            DrawWaveAnnotations(selectedWave, simulatedEndY);
        }

        private void button_Minus_Click(object sender, EventArgs e)
        {
            if (selectedWave == null) return;

            double yLow = selectedWave.IsUpWave
                           ? (double)selectedWave.Start.Candlestick.Low
                           : (double)selectedWave.End.Candlestick.Low;
            double yHigh = selectedWave.IsUpWave
                           ? (double)selectedWave.End.Candlestick.High
                           : (double)selectedWave.Start.Candlestick.High;
            double range = yHigh - yLow;

            double stepSize = 2 * percentRange * range;

            //move the simulation endprice down by that step
            simulatedEndY -= stepSize;

            DrawWaveAnnotations(selectedWave, simulatedEndY);      //redraw annotations
        }

        private void hScrollBar_Range_Scroll(object sender, ScrollEventArgs e)
        {
            percentRange = e.NewValue / 100.0;            //turn 0–100 into 0.0–1.0
            label_Percent.Text = $"{e.NewValue}%";        // show percentage
        }

        private void hScrollBar_Step_Scroll(object sender, ScrollEventArgs e)
        {
            numSteps = e.NewValue;                        //store steps
            label_Steps.Text = numSteps.ToString();       //show steps
        }

        private void button_StartStop_Click(object sender, EventArgs e)
        {
            if (!isSimulating)
            {
                if (selectedWave == null) return;

                //guard against an accidental zero
                if (numSteps < 1) numSteps = 1;

                //compute bottom and top of your percent band
                double originalEnd = selectedWave.IsUpWave
                    ? (double)selectedWave.End.Candlestick.High
                    : (double)selectedWave.End.Candlestick.Low;

                double yLow = selectedWave.IsUpWave
                    ? (double)selectedWave.Start.Candlestick.Low
                    : (double)selectedWave.End.Candlestick.Low;
                double yHigh = selectedWave.IsUpWave
                    ? (double)selectedWave.End.Candlestick.High
                    : (double)selectedWave.Start.Candlestick.High;
                double range = yHigh - yLow;
                double delta = percentRange * range;

                simulationBottom = originalEnd - delta;
                simulationTop = originalEnd + delta;

                //reset step to zero
                currentStep = 0;
                simulatedEndY = simulationBottom;

                DrawWaveAnnotations(selectedWave, simulatedEndY);        //redraw annotations

                isSimulating = true;            //set boolean to true
                button_StartStop.Text = "Stop";     //set button text to stop
                button_Plus.Enabled = false;        //disable +- buttons
                button_Minus.Enabled = false; 

                timer1.Start();            //start timer
            }
            else
            {
                timer1.Stop();                   //stop the timer
                isSimulating = false;  //clear our running flag
                button_StartStop.Text = "Start";    //set button text back to start
                button_Plus.Enabled = true;      //re-enable +- buttons
                button_Minus.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!isSimulating) return;

            //advance the step counter
            currentStep++;

            //stop if exceeded numSteps
            if (currentStep > numSteps)
            {
                timer1.Stop();                   //stop the timer
                isSimulating = false;  //clear our running flag
                button_StartStop.Text = "Start";
                button_Plus.Enabled = true;
                button_Minus.Enabled = true;
                return;
            }

            //compute the new endprice
            double totalSpan = simulationTop - simulationBottom;
            double stepSize = totalSpan / numSteps;
            simulatedEndY = simulationBottom + currentStep * stepSize;

            //redraw with the updated overrideEndY
            DrawWaveAnnotations(selectedWave, simulatedEndY);
        }
    }
}

 