namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.AzCopyPathTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.NInParallelButton = new System.Windows.Forms.Button();
            this.NumberOfDownloadsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.GenerateAzCopyButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.AnalysisIdTextBox = new System.Windows.Forms.TextBox();
            this.AzCopyCommandTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SourceFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SourceFolderTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BrowseFolderButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfDownloadsNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(228, 90);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Download single";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AzCopyPathTextBox
            // 
            this.AzCopyPathTextBox.Location = new System.Drawing.Point(73, 13);
            this.AzCopyPathTextBox.Name = "AzCopyPathTextBox";
            this.AzCopyPathTextBox.Size = new System.Drawing.Size(498, 20);
            this.AzCopyPathTextBox.TabIndex = 1;
            this.AzCopyPathTextBox.Text = "C:\\Program Files (x86)\\Microsoft SDKs\\Azure\\AzCopy";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "AzCopy path";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(330, 90);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(126, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Download 2 in parallel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // NInParallelButton
            // 
            this.NInParallelButton.Location = new System.Drawing.Point(384, 131);
            this.NInParallelButton.Name = "NInParallelButton";
            this.NInParallelButton.Size = new System.Drawing.Size(72, 23);
            this.NInParallelButton.TabIndex = 4;
            this.NInParallelButton.Text = "in parallel";
            this.NInParallelButton.UseVisualStyleBackColor = true;
            this.NInParallelButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // NumberOfDownloadsNumericUpDown
            // 
            this.NumberOfDownloadsNumericUpDown.Location = new System.Drawing.Point(330, 131);
            this.NumberOfDownloadsNumericUpDown.Name = "NumberOfDownloadsNumericUpDown";
            this.NumberOfDownloadsNumericUpDown.Size = new System.Drawing.Size(48, 20);
            this.NumberOfDownloadsNumericUpDown.TabIndex = 5;
            this.NumberOfDownloadsNumericUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(269, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Download";
            // 
            // GenerateAzCopyButton
            // 
            this.GenerateAzCopyButton.Location = new System.Drawing.Point(430, 24);
            this.GenerateAzCopyButton.Name = "GenerateAzCopyButton";
            this.GenerateAzCopyButton.Size = new System.Drawing.Size(148, 23);
            this.GenerateAzCopyButton.TabIndex = 7;
            this.GenerateAzCopyButton.Text = "Generate AzCopy Command";
            this.GenerateAzCopyButton.UseVisualStyleBackColor = true;
            this.GenerateAzCopyButton.Click += new System.EventHandler(this.GenerateAzCopyButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Analysis Id";
            // 
            // AnalysisIdTextBox
            // 
            this.AnalysisIdTextBox.Location = new System.Drawing.Point(69, 24);
            this.AnalysisIdTextBox.Name = "AnalysisIdTextBox";
            this.AnalysisIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.AnalysisIdTextBox.TabIndex = 8;
            this.AnalysisIdTextBox.Text = "TestAnalysis";
            // 
            // AzCopyCommandTextBox
            // 
            this.AzCopyCommandTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AzCopyCommandTextBox.Location = new System.Drawing.Point(3, 60);
            this.AzCopyCommandTextBox.Multiline = true;
            this.AzCopyCommandTextBox.Name = "AzCopyCommandTextBox";
            this.AzCopyCommandTextBox.Size = new System.Drawing.Size(577, 93);
            this.AzCopyCommandTextBox.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BrowseFolderButton);
            this.groupBox1.Controls.Add(this.SourceFolderTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.GenerateAzCopyButton);
            this.groupBox1.Controls.Add(this.AzCopyCommandTextBox);
            this.groupBox1.Controls.Add(this.AnalysisIdTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 160);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(583, 156);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Generation of AzCopy upload command with SAS";
            // 
            // SourceFolderTextBox
            // 
            this.SourceFolderTextBox.Location = new System.Drawing.Point(247, 24);
            this.SourceFolderTextBox.Name = "SourceFolderTextBox";
            this.SourceFolderTextBox.ReadOnly = true;
            this.SourceFolderTextBox.Size = new System.Drawing.Size(147, 20);
            this.SourceFolderTextBox.TabIndex = 11;
            this.SourceFolderTextBox.Text = "c:\\temp\\source\\";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(175, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Source Folder";
            // 
            // BrowseFolderButton
            // 
            this.BrowseFolderButton.Location = new System.Drawing.Point(400, 24);
            this.BrowseFolderButton.Name = "BrowseFolderButton";
            this.BrowseFolderButton.Size = new System.Drawing.Size(25, 23);
            this.BrowseFolderButton.TabIndex = 13;
            this.BrowseFolderButton.Text = "...";
            this.BrowseFolderButton.UseVisualStyleBackColor = true;
            this.BrowseFolderButton.Click += new System.EventHandler(this.BrowseFolderButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 316);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NumberOfDownloadsNumericUpDown);
            this.Controls.Add(this.NInParallelButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AzCopyPathTextBox);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfDownloadsNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox AzCopyPathTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button NInParallelButton;
        private System.Windows.Forms.NumericUpDown NumberOfDownloadsNumericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button GenerateAzCopyButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox AnalysisIdTextBox;
        private System.Windows.Forms.TextBox AzCopyCommandTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BrowseFolderButton;
        private System.Windows.Forms.TextBox SourceFolderTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FolderBrowserDialog SourceFolderBrowserDialog;
    }
}

