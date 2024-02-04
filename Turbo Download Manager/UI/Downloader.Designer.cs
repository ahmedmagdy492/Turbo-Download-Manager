namespace Turbo_Download_Manager
{
    partial class Downloader
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Downloader));
            lblDownloadPrgrs = new Label();
            downloadProgressBar = new ProgressBar();
            btnCancel = new Button();
            downloadTasksTracker = new System.Windows.Forms.Timer(components);
            lblThreadsCount = new Label();
            overallProgress = new ProgressBar();
            SuspendLayout();
            // 
            // lblDownloadPrgrs
            // 
            lblDownloadPrgrs.AutoSize = true;
            lblDownloadPrgrs.Location = new Point(12, 41);
            lblDownloadPrgrs.Name = "lblDownloadPrgrs";
            lblDownloadPrgrs.Size = new Size(177, 30);
            lblDownloadPrgrs.TabIndex = 0;
            lblDownloadPrgrs.Text = "Downloading 0%";
            // 
            // downloadProgressBar
            // 
            downloadProgressBar.Location = new Point(12, 153);
            downloadProgressBar.Name = "downloadProgressBar";
            downloadProgressBar.Size = new Size(755, 29);
            downloadProgressBar.TabIndex = 1;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(680, 210);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 38);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // downloadTasksTracker
            // 
            downloadTasksTracker.Interval = 1000;
            downloadTasksTracker.Tick += downloadTasksTracker_Tick;
            // 
            // lblThreadsCount
            // 
            lblThreadsCount.AutoSize = true;
            lblThreadsCount.Font = new Font("Segoe UI", 11F);
            lblThreadsCount.Location = new Point(12, 185);
            lblThreadsCount.Name = "lblThreadsCount";
            lblThreadsCount.Size = new Size(110, 25);
            lblThreadsCount.TabIndex = 4;
            lblThreadsCount.Text = "0 MB/ 0 MB";
            // 
            // overallProgress
            // 
            overallProgress.Location = new Point(12, 88);
            overallProgress.Name = "overallProgress";
            overallProgress.Size = new Size(755, 29);
            overallProgress.TabIndex = 5;
            // 
            // Downloader
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(786, 260);
            Controls.Add(overallProgress);
            Controls.Add(lblThreadsCount);
            Controls.Add(btnCancel);
            Controls.Add(downloadProgressBar);
            Controls.Add(lblDownloadPrgrs);
            Font = new Font("Segoe UI", 13F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            MaximizeBox = false;
            Name = "Downloader";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Turbo Download Manager";
            Load += Downloader_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblDownloadPrgrs;
        private ProgressBar downloadProgressBar;
        private Button btnCancel;
        private System.Windows.Forms.Timer downloadTasksTracker;
        private Label lblThreadsCount;
        private ProgressBar overallProgress;
    }
}