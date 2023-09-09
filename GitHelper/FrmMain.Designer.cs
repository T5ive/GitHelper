namespace GitHelper
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtLogs = new RichTextBox();
            btnRun = new Button();
            menuStrip1 = new MenuStrip();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // txtLogs
            // 
            txtLogs.BackColor = Color.FromArgb(60, 63, 65);
            txtLogs.Dock = DockStyle.Fill;
            txtLogs.ForeColor = Color.Gainsboro;
            txtLogs.Location = new Point(0, 24);
            txtLogs.Name = "txtLogs";
            txtLogs.Size = new Size(734, 407);
            txtLogs.TabIndex = 0;
            txtLogs.Text = "";
            txtLogs.LinkClicked += txtLogs_LinkClicked;
            // 
            // btnRun
            // 
            btnRun.BackColor = Color.FromArgb(60, 63, 65);
            btnRun.Dock = DockStyle.Bottom;
            btnRun.FlatStyle = FlatStyle.Flat;
            btnRun.ForeColor = SystemColors.Control;
            btnRun.Location = new Point(0, 431);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(734, 30);
            btnRun.TabIndex = 1;
            btnRun.Text = "Fetch && Pull";
            btnRun.UseVisualStyleBackColor = false;
            btnRun.Click += btnRun_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(734, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(60, 63, 65);
            ClientSize = new Size(734, 461);
            Controls.Add(txtLogs);
            Controls.Add(btnRun);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Git Helper";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox txtLogs;
        private Button btnRun;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}