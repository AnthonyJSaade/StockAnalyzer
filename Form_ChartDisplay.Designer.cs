namespace StockAnalyzer
{
    partial class Form_ChartDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart_OHLC = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.candlestickBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button_StartStop = new System.Windows.Forms.Button();
            this.button_Plus = new System.Windows.Forms.Button();
            this.button_Minus = new System.Windows.Forms.Button();
            this.hScrollBar_Range = new System.Windows.Forms.HScrollBar();
            this.hScrollBar_Step = new System.Windows.Forms.HScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listBox_UpWaves = new System.Windows.Forms.ListBox();
            this.listBox_DownWaves = new System.Windows.Forms.ListBox();
            this.label_Confirmations = new System.Windows.Forms.Label();
            this.label_Price = new System.Windows.Forms.Label();
            this.label_Percent = new System.Windows.Forms.Label();
            this.label_Steps = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.candlestickBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chart_OHLC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.candlestickBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.candlestickBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart_OHLC
            // 
            chartArea1.Name = "ChartArea_OHLC";
            this.chart_OHLC.ChartAreas.Add(chartArea1);
            this.chart_OHLC.DataSource = this.candlestickBindingSource1;
            legend1.Name = "Legend1";
            this.chart_OHLC.Legends.Add(legend1);
            this.chart_OHLC.Location = new System.Drawing.Point(38, 36);
            this.chart_OHLC.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chart_OHLC.Name = "chart_OHLC";
            series1.ChartArea = "ChartArea_OHLC";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.CustomProperties = "PriceDownColor=OrangeRed, PriceUpColor=Lime";
            series1.IsXValueIndexed = true;
            series1.Legend = "Legend1";
            series1.Name = "Series_OHLC";
            series1.XValueMember = "Date";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            series1.YValueMembers = "High, Low, Open, Close";
            series1.YValuesPerPoint = 4;
            this.chart_OHLC.Series.Add(series1);
            this.chart_OHLC.Size = new System.Drawing.Size(957, 396);
            this.chart_OHLC.TabIndex = 0;
            this.chart_OHLC.Text = "chart1";
            this.chart_OHLC.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Chart_OHLC_MouseDown);
            this.chart_OHLC.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Chart_OHLC_MouseMove);
            this.chart_OHLC.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Chart_OHLC_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button_StartStop
            // 
            this.button_StartStop.Location = new System.Drawing.Point(253, 478);
            this.button_StartStop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_StartStop.Name = "button_StartStop";
            this.button_StartStop.Size = new System.Drawing.Size(143, 40);
            this.button_StartStop.TabIndex = 1;
            this.button_StartStop.Text = "Start";
            this.button_StartStop.UseVisualStyleBackColor = true;
            this.button_StartStop.Click += new System.EventHandler(this.button_StartStop_Click);
            // 
            // button_Plus
            // 
            this.button_Plus.Location = new System.Drawing.Point(457, 480);
            this.button_Plus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_Plus.Name = "button_Plus";
            this.button_Plus.Size = new System.Drawing.Size(71, 36);
            this.button_Plus.TabIndex = 2;
            this.button_Plus.Text = "+";
            this.button_Plus.UseVisualStyleBackColor = true;
            this.button_Plus.Click += new System.EventHandler(this.button_Plus_Click);
            // 
            // button_Minus
            // 
            this.button_Minus.Location = new System.Drawing.Point(550, 480);
            this.button_Minus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_Minus.Name = "button_Minus";
            this.button_Minus.Size = new System.Drawing.Size(67, 36);
            this.button_Minus.TabIndex = 3;
            this.button_Minus.Text = "-";
            this.button_Minus.UseVisualStyleBackColor = true;
            this.button_Minus.Click += new System.EventHandler(this.button_Minus_Click);
            // 
            // hScrollBar_Range
            // 
            this.hScrollBar_Range.Location = new System.Drawing.Point(155, 563);
            this.hScrollBar_Range.Minimum = 2;
            this.hScrollBar_Range.Name = "hScrollBar_Range";
            this.hScrollBar_Range.Size = new System.Drawing.Size(228, 34);
            this.hScrollBar_Range.TabIndex = 4;
            this.hScrollBar_Range.Value = 10;
            this.hScrollBar_Range.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Range_Scroll);
            // 
            // hScrollBar_Step
            // 
            this.hScrollBar_Step.Location = new System.Drawing.Point(155, 607);
            this.hScrollBar_Step.Minimum = 1;
            this.hScrollBar_Step.Name = "hScrollBar_Step";
            this.hScrollBar_Step.Size = new System.Drawing.Size(228, 36);
            this.hScrollBar_Step.TabIndex = 5;
            this.hScrollBar_Step.Value = 2;
            this.hScrollBar_Step.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Step_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 562);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "% of Range";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 607);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "# of Steps";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1043, 59);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "# Confirmations";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1042, 189);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Price";
            // 
            // listBox_UpWaves
            // 
            this.listBox_UpWaves.FormattingEnabled = true;
            this.listBox_UpWaves.ItemHeight = 16;
            this.listBox_UpWaves.Location = new System.Drawing.Point(701, 486);
            this.listBox_UpWaves.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listBox_UpWaves.Name = "listBox_UpWaves";
            this.listBox_UpWaves.Size = new System.Drawing.Size(156, 132);
            this.listBox_UpWaves.TabIndex = 10;
            this.listBox_UpWaves.SelectedIndexChanged += new System.EventHandler(this.listBox_UpWaves_SelectedIndexChanged);
            // 
            // listBox_DownWaves
            // 
            this.listBox_DownWaves.FormattingEnabled = true;
            this.listBox_DownWaves.ItemHeight = 16;
            this.listBox_DownWaves.Location = new System.Drawing.Point(890, 484);
            this.listBox_DownWaves.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listBox_DownWaves.Name = "listBox_DownWaves";
            this.listBox_DownWaves.Size = new System.Drawing.Size(158, 132);
            this.listBox_DownWaves.TabIndex = 11;
            this.listBox_DownWaves.SelectedIndexChanged += new System.EventHandler(this.listBox_DownWaves_SelectedIndexChanged);
            // 
            // label_Confirmations
            // 
            this.label_Confirmations.AutoSize = true;
            this.label_Confirmations.Location = new System.Drawing.Point(1039, 92);
            this.label_Confirmations.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Confirmations.Name = "label_Confirmations";
            this.label_Confirmations.Size = new System.Drawing.Size(14, 16);
            this.label_Confirmations.TabIndex = 12;
            this.label_Confirmations.Text = "0";
            // 
            // label_Price
            // 
            this.label_Price.AutoSize = true;
            this.label_Price.Location = new System.Drawing.Point(1045, 225);
            this.label_Price.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Price.Name = "label_Price";
            this.label_Price.Size = new System.Drawing.Size(14, 16);
            this.label_Price.TabIndex = 13;
            this.label_Price.Text = "0";
            // 
            // label_Percent
            // 
            this.label_Percent.AutoSize = true;
            this.label_Percent.Location = new System.Drawing.Point(410, 563);
            this.label_Percent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Percent.Name = "label_Percent";
            this.label_Percent.Size = new System.Drawing.Size(33, 16);
            this.label_Percent.TabIndex = 14;
            this.label_Percent.Text = "10%";
            // 
            // label_Steps
            // 
            this.label_Steps.AutoSize = true;
            this.label_Steps.Location = new System.Drawing.Point(414, 609);
            this.label_Steps.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Steps.Name = "label_Steps";
            this.label_Steps.Size = new System.Drawing.Size(14, 16);
            this.label_Steps.TabIndex = 15;
            this.label_Steps.Text = "2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(703, 462);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "Up Waves:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(890, 461);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "Down Waves:";
            // 
            // candlestickBindingSource1
            // 
            this.candlestickBindingSource1.DataSource = typeof(Project_3.Candlestick);
            // 
            // Form_ChartDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1283, 675);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label_Steps);
            this.Controls.Add(this.label_Percent);
            this.Controls.Add(this.label_Price);
            this.Controls.Add(this.label_Confirmations);
            this.Controls.Add(this.listBox_DownWaves);
            this.Controls.Add(this.listBox_UpWaves);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hScrollBar_Step);
            this.Controls.Add(this.hScrollBar_Range);
            this.Controls.Add(this.button_Minus);
            this.Controls.Add(this.button_Plus);
            this.Controls.Add(this.button_StartStop);
            this.Controls.Add(this.chart_OHLC);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form_ChartDisplay";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.chart_OHLC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.candlestickBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.candlestickBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart_OHLC;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button_StartStop;
        private System.Windows.Forms.Button button_Plus;
        private System.Windows.Forms.Button button_Minus;
        private System.Windows.Forms.HScrollBar hScrollBar_Range;
        private System.Windows.Forms.HScrollBar hScrollBar_Step;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.BindingSource candlestickBindingSource;
        private System.Windows.Forms.ListBox listBox_UpWaves;
        private System.Windows.Forms.ListBox listBox_DownWaves;
        private System.Windows.Forms.Label label_Confirmations;
        private System.Windows.Forms.Label label_Price;
        private System.Windows.Forms.Label label_Percent;
        private System.Windows.Forms.Label label_Steps;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.BindingSource candlestickBindingSource1;
    }
}
