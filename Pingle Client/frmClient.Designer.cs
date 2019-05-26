namespace Pingle
{
    partial class frmClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClient));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbNetworkAvailablity = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tsTxtIpAddress = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.tsTxtIpPort = new System.Windows.Forms.ToolStripTextBox();
            this.tsBtnRunTest = new System.Windows.Forms.ToolStripButton();
            this.tsBtnStopTest = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tsTxtPingsNum = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.tsTxtPigsPerMinuts = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnSendReport = new System.Windows.Forms.ToolStripButton();
            this.tblLayoutContainer = new System.Windows.Forms.TableLayoutPanel();
            this.chartInboundConnection = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.txtReport = new System.Windows.Forms.TextBox();
            this.lvResults = new System.Windows.Forms.ListView();
            this.chartNetBandWidth = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartOutboundConnection = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelLoading = new System.Windows.Forms.Panel();
            this.lbCancelProcess = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnAbout = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tblLayoutContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartInboundConnection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartNetBandWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartOutboundConnection)).BeginInit();
            this.panelLoading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbNetworkAvailablity});
            this.statusStrip1.Location = new System.Drawing.Point(0, 619);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(966, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbNetworkAvailablity
            // 
            this.lbNetworkAvailablity.Name = "lbNetworkAvailablity";
            this.lbNetworkAvailablity.Size = new System.Drawing.Size(136, 17);
            this.lbNetworkAvailablity.Text = "Network is not available!";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tsTxtIpAddress,
            this.toolStripLabel3,
            this.tsTxtIpPort,
            this.tsBtnRunTest,
            this.tsBtnStopTest,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.tsTxtPingsNum,
            this.toolStripSeparator2,
            this.toolStripLabel4,
            this.tsTxtPigsPerMinuts,
            this.toolStripSeparator3,
            this.tsBtnSendReport,
            this.toolStripSeparator4,
            this.tsBtnAbout});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(966, 25);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(20, 22);
            this.toolStripLabel1.Text = "IP:";
            // 
            // tsTxtIpAddress
            // 
            this.tsTxtIpAddress.Name = "tsTxtIpAddress";
            this.tsTxtIpAddress.Size = new System.Drawing.Size(100, 25);
            this.tsTxtIpAddress.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel3.Text = "Port:";
            // 
            // tsTxtIpPort
            // 
            this.tsTxtIpPort.Name = "tsTxtIpPort";
            this.tsTxtIpPort.Size = new System.Drawing.Size(100, 25);
            this.tsTxtIpPort.Text = "80";
            this.tsTxtIpPort.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tsTxtIpPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tsTxtIpPort_KeyPress);
            this.tsTxtIpPort.TextChanged += new System.EventHandler(this.tsTxtIpPort_TextChanged);
            // 
            // tsBtnRunTest
            // 
            this.tsBtnRunTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnRunTest.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnRunTest.Image")));
            this.tsBtnRunTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnRunTest.Name = "tsBtnRunTest";
            this.tsBtnRunTest.Size = new System.Drawing.Size(23, 22);
            this.tsBtnRunTest.Text = "Run Test";
            this.tsBtnRunTest.Click += new System.EventHandler(this.tsBtnRunTest_ClickAsync);
            // 
            // tsBtnStopTest
            // 
            this.tsBtnStopTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnStopTest.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnStopTest.Image")));
            this.tsBtnStopTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnStopTest.Name = "tsBtnStopTest";
            this.tsBtnStopTest.Size = new System.Drawing.Size(23, 22);
            this.tsBtnStopTest.Text = "Stop test";
            this.tsBtnStopTest.Click += new System.EventHandler(this.tsBtnStopTest_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(100, 22);
            this.toolStripLabel2.Text = "Number of Pings:";
            // 
            // tsTxtPingsNum
            // 
            this.tsTxtPingsNum.Name = "tsTxtPingsNum";
            this.tsTxtPingsNum.Size = new System.Drawing.Size(100, 25);
            this.tsTxtPingsNum.Text = "60";
            this.tsTxtPingsNum.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tsTxtPingsNum.ToolTipText = "Number of pings.";
            this.tsTxtPingsNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tsTxtPingsNum_KeyPress);
            this.tsTxtPingsNum.TextChanged += new System.EventHandler(this.tsTxtPingsNum_TextChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(102, 22);
            this.toolStripLabel4.Text = "Pings per minutes";
            // 
            // tsTxtPigsPerMinuts
            // 
            this.tsTxtPigsPerMinuts.Name = "tsTxtPigsPerMinuts";
            this.tsTxtPigsPerMinuts.Size = new System.Drawing.Size(100, 25);
            this.tsTxtPigsPerMinuts.Text = "60";
            this.tsTxtPigsPerMinuts.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tsTxtPigsPerMinuts.ToolTipText = "Pings per minuts";
            this.tsTxtPigsPerMinuts.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tsTxtPigsPerMinuts_KeyPress);
            this.tsTxtPigsPerMinuts.TextChanged += new System.EventHandler(this.tsTxtPigsPerMinuts_TextChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsBtnSendReport
            // 
            this.tsBtnSendReport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnSendReport.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnSendReport.Image")));
            this.tsBtnSendReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnSendReport.Name = "tsBtnSendReport";
            this.tsBtnSendReport.Size = new System.Drawing.Size(23, 22);
            this.tsBtnSendReport.Text = "Create Report";
            this.tsBtnSendReport.Click += new System.EventHandler(this.tsBtnSendReport_Click);
            // 
            // tblLayoutContainer
            // 
            this.tblLayoutContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblLayoutContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblLayoutContainer.ColumnCount = 2;
            this.tblLayoutContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.30615F));
            this.tblLayoutContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.69385F));
            this.tblLayoutContainer.Controls.Add(this.chartInboundConnection, 0, 0);
            this.tblLayoutContainer.Controls.Add(this.txtReport, 0, 4);
            this.tblLayoutContainer.Controls.Add(this.lvResults, 0, 3);
            this.tblLayoutContainer.Controls.Add(this.chartNetBandWidth, 0, 1);
            this.tblLayoutContainer.Controls.Add(this.chartOutboundConnection, 0, 2);
            this.tblLayoutContainer.Location = new System.Drawing.Point(0, 27);
            this.tblLayoutContainer.Name = "tblLayoutContainer";
            this.tblLayoutContainer.RowCount = 5;
            this.tblLayoutContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblLayoutContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblLayoutContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblLayoutContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblLayoutContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblLayoutContainer.Size = new System.Drawing.Size(966, 594);
            this.tblLayoutContainer.TabIndex = 6;
            // 
            // chartInboundConnection
            // 
            this.chartInboundConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chartInboundConnection.ChartAreas.Add(chartArea1);
            this.tblLayoutContainer.SetColumnSpan(this.chartInboundConnection, 2);
            legend1.Name = "Legend1";
            this.chartInboundConnection.Legends.Add(legend1);
            this.chartInboundConnection.Location = new System.Drawing.Point(3, 3);
            this.chartInboundConnection.Name = "chartInboundConnection";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartInboundConnection.Series.Add(series1);
            this.chartInboundConnection.Size = new System.Drawing.Size(960, 112);
            this.chartInboundConnection.TabIndex = 7;
            this.chartInboundConnection.Text = "Inbound Connection";
            // 
            // txtReport
            // 
            this.tblLayoutContainer.SetColumnSpan(this.txtReport, 2);
            this.txtReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReport.Location = new System.Drawing.Point(3, 475);
            this.txtReport.Multiline = true;
            this.txtReport.Name = "txtReport";
            this.txtReport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReport.Size = new System.Drawing.Size(960, 116);
            this.txtReport.TabIndex = 11;
            // 
            // lvResults
            // 
            this.lvResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblLayoutContainer.SetColumnSpan(this.lvResults, 2);
            this.lvResults.Location = new System.Drawing.Point(3, 357);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(960, 112);
            this.lvResults.TabIndex = 13;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = System.Windows.Forms.View.Details;
            // 
            // chartNetBandWidth
            // 
            this.chartNetBandWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.Name = "ChartArea1";
            this.chartNetBandWidth.ChartAreas.Add(chartArea2);
            this.tblLayoutContainer.SetColumnSpan(this.chartNetBandWidth, 2);
            legend2.Name = "Legend1";
            this.chartNetBandWidth.Legends.Add(legend2);
            this.chartNetBandWidth.Location = new System.Drawing.Point(3, 121);
            this.chartNetBandWidth.Name = "chartNetBandWidth";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartNetBandWidth.Series.Add(series2);
            this.chartNetBandWidth.Size = new System.Drawing.Size(960, 112);
            this.chartNetBandWidth.TabIndex = 12;
            this.chartNetBandWidth.Text = "Bandwidth";
            // 
            // chartOutboundConnection
            // 
            this.chartOutboundConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea3.Name = "ChartArea1";
            this.chartOutboundConnection.ChartAreas.Add(chartArea3);
            this.tblLayoutContainer.SetColumnSpan(this.chartOutboundConnection, 2);
            legend3.Name = "Legend1";
            this.chartOutboundConnection.Legends.Add(legend3);
            this.chartOutboundConnection.Location = new System.Drawing.Point(3, 239);
            this.chartOutboundConnection.Name = "chartOutboundConnection";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartOutboundConnection.Series.Add(series3);
            this.chartOutboundConnection.Size = new System.Drawing.Size(960, 112);
            this.chartOutboundConnection.TabIndex = 8;
            this.chartOutboundConnection.Text = "Outbound Connection";
            // 
            // panelLoading
            // 
            this.panelLoading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLoading.Controls.Add(this.lbCancelProcess);
            this.panelLoading.Controls.Add(this.label1);
            this.panelLoading.Controls.Add(this.pictureBox1);
            this.panelLoading.Location = new System.Drawing.Point(593, 440);
            this.panelLoading.Name = "panelLoading";
            this.panelLoading.Size = new System.Drawing.Size(226, 63);
            this.panelLoading.TabIndex = 10;
            // 
            // lbCancelProcess
            // 
            this.lbCancelProcess.AutoSize = true;
            this.lbCancelProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCancelProcess.LinkColor = System.Drawing.Color.DimGray;
            this.lbCancelProcess.Location = new System.Drawing.Point(83, 45);
            this.lbCancelProcess.Name = "lbCancelProcess";
            this.lbCancelProcess.Size = new System.Drawing.Size(46, 13);
            this.lbCancelProcess.TabIndex = 5;
            this.lbCancelProcess.TabStop = true;
            this.lbCancelProcess.Text = "Cancel";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(97)))), ((int)(((byte)(141)))));
            this.label1.Location = new System.Drawing.Point(60, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Please wait...";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PingleClient.Properties.Resources.big_loader;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(221, 25);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsBtnAbout
            // 
            this.tsBtnAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnAbout.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnAbout.Image")));
            this.tsBtnAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnAbout.Name = "tsBtnAbout";
            this.tsBtnAbout.Size = new System.Drawing.Size(23, 22);
            this.tsBtnAbout.Text = "About";
            this.tsBtnAbout.Click += new System.EventHandler(this.tsBtnAbout_Click);
            // 
            // frmClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 641);
            this.Controls.Add(this.panelLoading);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tblLayoutContainer);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmClient";
            this.Text = "Pingle Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmClient_FormClosing);
            this.Load += new System.EventHandler(this.frmClient_Load);
            this.Resize += new System.EventHandler(this.frmClient_Resize);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tblLayoutContainer.ResumeLayout(false);
            this.tblLayoutContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartInboundConnection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartNetBandWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartOutboundConnection)).EndInit();
            this.panelLoading.ResumeLayout(false);
            this.panelLoading.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbNetworkAvailablity;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox tsTxtIpAddress;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton tsBtnRunTest;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TableLayoutPanel tblLayoutContainer;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartOutboundConnection;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartInboundConnection;
        private System.Windows.Forms.Panel panelLoading;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripButton tsBtnStopTest;
        private System.Windows.Forms.TextBox txtReport;
        private System.Windows.Forms.LinkLabel lbCancelProcess;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripTextBox tsTxtPigsPerMinuts;
        private System.Windows.Forms.ToolStripTextBox tsTxtPingsNum;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsBtnSendReport;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartNetBandWidth;
        private System.Windows.Forms.ListView lvResults;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox tsTxtIpPort;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsBtnAbout;
    }
}

