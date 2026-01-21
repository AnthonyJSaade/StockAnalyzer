using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockAnalyzer
{
    /// <summary>
    /// Start form that allows users to select date range and load stock CSV files.
    /// </summary>
    public partial class Form_Start : Form
    {
        public Form_Start()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog_LoadTicker.ShowDialog();   // Opens file dialog
        }

        private void openFileDialog_LoadTicker_FileOk(object sender, CancelEventArgs e)
        {
            foreach (var filename in openFileDialog_LoadTicker.FileNames)   // Loop to handle multiple selections
            {
                Form_ChartDisplay f = new Form_ChartDisplay(filename, dateTimePicker_StartDate.Value, dateTimePicker_EndDate.Value);
                f.Text = filename;   // Adds name as title
                f.Show();            // Shows form
            }
        }
    }
}

