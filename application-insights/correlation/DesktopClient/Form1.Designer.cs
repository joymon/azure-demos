namespace DesktopClient
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
            this.LogCorrelatedEventsFromChainedTasksbutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.RadiustextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LogstextBox = new System.Windows.Forms.TextBox();
            this.FindAreaViaWebServiceButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LogCorrelatedEventsFromChainedTasksbutton
            // 
            this.LogCorrelatedEventsFromChainedTasksbutton.Location = new System.Drawing.Point(166, 13);
            this.LogCorrelatedEventsFromChainedTasksbutton.Name = "LogCorrelatedEventsFromChainedTasksbutton";
            this.LogCorrelatedEventsFromChainedTasksbutton.Size = new System.Drawing.Size(107, 20);
            this.LogCorrelatedEventsFromChainedTasksbutton.TabIndex = 0;
            this.LogCorrelatedEventsFromChainedTasksbutton.Text = "Find Area Local";
            this.LogCorrelatedEventsFromChainedTasksbutton.UseVisualStyleBackColor = true;
            this.LogCorrelatedEventsFromChainedTasksbutton.Click += new System.EventHandler(this.LogCorrelatedEventsFromChainedTasksbutton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Radius";
            // 
            // RadiustextBox
            // 
            this.RadiustextBox.Location = new System.Drawing.Point(60, 13);
            this.RadiustextBox.Name = "RadiustextBox";
            this.RadiustextBox.Size = new System.Drawing.Size(100, 20);
            this.RadiustextBox.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LogstextBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 341);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Logs";
            // 
            // LogstextBox
            // 
            this.LogstextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogstextBox.Location = new System.Drawing.Point(3, 16);
            this.LogstextBox.Multiline = true;
            this.LogstextBox.Name = "LogstextBox";
            this.LogstextBox.Size = new System.Drawing.Size(794, 322);
            this.LogstextBox.TabIndex = 0;
            // 
            // FindAreaViaWebServiceButton
            // 
            this.FindAreaViaWebServiceButton.Location = new System.Drawing.Point(279, 13);
            this.FindAreaViaWebServiceButton.Name = "FindAreaViaWebServiceButton";
            this.FindAreaViaWebServiceButton.Size = new System.Drawing.Size(124, 20);
            this.FindAreaViaWebServiceButton.TabIndex = 4;
            this.FindAreaViaWebServiceButton.Text = "Find Area WebService";
            this.FindAreaViaWebServiceButton.UseVisualStyleBackColor = true;
            this.FindAreaViaWebServiceButton.Click += new System.EventHandler(this.FindAreaViaWebServiceButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.FindAreaViaWebServiceButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.RadiustextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LogCorrelatedEventsFromChainedTasksbutton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LogCorrelatedEventsFromChainedTasksbutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox RadiustextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox LogstextBox;
        private System.Windows.Forms.Button FindAreaViaWebServiceButton;
    }
}

