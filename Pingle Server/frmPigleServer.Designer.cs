namespace Pingle_Server
{
    partial class frmPigleServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPigleServer));
            this.lbShowIP = new System.Windows.Forms.Label();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbNetworkAvailablity = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtReport = new System.Windows.Forms.TextBox();
            this.lbMessageToUser = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.numericFileSize = new System.Windows.Forms.NumericUpDown();
            this.lbFileSize = new System.Windows.Forms.Label();
            this.cmbIPList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericFileSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbShowIP
            // 
            this.lbShowIP.AutoSize = true;
            this.lbShowIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbShowIP.Location = new System.Drawing.Point(2, 2);
            this.lbShowIP.Name = "lbShowIP";
            this.lbShowIP.Size = new System.Drawing.Size(420, 61);
            this.lbShowIP.TabIndex = 0;
            this.lbShowIP.Text = "999.999.999.999";
            // 
            // btnStartServer
            // 
            this.btnStartServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartServer.Location = new System.Drawing.Point(457, 143);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(98, 98);
            this.btnStartServer.TabIndex = 1;
            this.btnStartServer.Text = "Start\r\nServer";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbNetworkAvailablity});
            this.statusStrip1.Location = new System.Drawing.Point(0, 244);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(564, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbNetworkAvailablity
            // 
            this.lbNetworkAvailablity.Name = "lbNetworkAvailablity";
            this.lbNetworkAvailablity.Size = new System.Drawing.Size(136, 17);
            this.lbNetworkAvailablity.Text = "Network is not available!";
            // 
            // txtReport
            // 
            this.txtReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReport.Location = new System.Drawing.Point(6, 143);
            this.txtReport.Multiline = true;
            this.txtReport.Name = "txtReport";
            this.txtReport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReport.Size = new System.Drawing.Size(444, 98);
            this.txtReport.TabIndex = 12;
            // 
            // lbMessageToUser
            // 
            this.lbMessageToUser.AutoSize = true;
            this.lbMessageToUser.Location = new System.Drawing.Point(12, 65);
            this.lbMessageToUser.Name = "lbMessageToUser";
            this.lbMessageToUser.Size = new System.Drawing.Size(35, 13);
            this.lbMessageToUser.TabIndex = 13;
            this.lbMessageToUser.Text = "label1";
            // 
            // txtPort
            // 
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.Location = new System.Drawing.Point(413, 7);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(124, 60);
            this.txtPort.TabIndex = 14;
            // 
            // numericFileSize
            // 
            this.numericFileSize.Location = new System.Drawing.Point(506, 93);
            this.numericFileSize.Name = "numericFileSize";
            this.numericFileSize.Size = new System.Drawing.Size(49, 20);
            this.numericFileSize.TabIndex = 15;
            // 
            // lbFileSize
            // 
            this.lbFileSize.AutoSize = true;
            this.lbFileSize.Location = new System.Drawing.Point(416, 95);
            this.lbFileSize.Name = "lbFileSize";
            this.lbFileSize.Size = new System.Drawing.Size(92, 13);
            this.lbFileSize.TabIndex = 16;
            this.lbFileSize.Text = "Upload Size (MB):";
            // 
            // cmbIPList
            // 
            this.cmbIPList.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbIPList.FormattingEnabled = true;
            this.cmbIPList.Location = new System.Drawing.Point(6, 116);
            this.cmbIPList.Name = "cmbIPList";
            this.cmbIPList.Size = new System.Drawing.Size(549, 21);
            this.cmbIPList.TabIndex = 17;
            this.cmbIPList.SelectionChangeCommitted += new System.EventHandler(this.cmbIPList_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(280, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Limit Bandwidth:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(361, 93);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(49, 20);
            this.numericUpDown1.TabIndex = 15;
            // 
            // frmPigleServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 266);
            this.Controls.Add(this.cmbIPList);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericFileSize);
            this.Controls.Add(this.lbFileSize);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lbMessageToUser);
            this.Controls.Add(this.txtReport);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.lbShowIP);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPigleServer";
            this.Text = "Pingle Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPigleServer_FormClosing);
            this.Load += new System.EventHandler(this.frmPigleServer_Load);
            this.Shown += new System.EventHandler(this.frmPigleServer_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericFileSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbShowIP;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbNetworkAvailablity;
        private System.Windows.Forms.TextBox txtReport;
        private System.Windows.Forms.Label lbMessageToUser;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.NumericUpDown numericFileSize;
        private System.Windows.Forms.Label lbFileSize;
        private System.Windows.Forms.ComboBox cmbIPList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}