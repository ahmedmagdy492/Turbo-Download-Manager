namespace Turbo_Download_Manager
{
    partial class TurboMgr
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TurboMgr));
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            downloadAFileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem1 = new ToolStripMenuItem();
            downloads = new DataGridView();
            contextMenuStrip2 = new ContextMenuStrip(components);
            restartDownloadToolStripMenuItem = new ToolStripMenuItem();
            copyDownloadLinkToolStripMenuItem = new ToolStripMenuItem();
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            showTurboToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)downloads).BeginInit();
            contextMenuStrip2.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(928, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { downloadAFileToolStripMenuItem, exitToolStripMenuItem1 });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // downloadAFileToolStripMenuItem
            // 
            downloadAFileToolStripMenuItem.Name = "downloadAFileToolStripMenuItem";
            downloadAFileToolStripMenuItem.Size = new Size(200, 26);
            downloadAFileToolStripMenuItem.Text = "Download a File";
            downloadAFileToolStripMenuItem.Click += downloadAFileToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem1
            // 
            exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            exitToolStripMenuItem1.Size = new Size(200, 26);
            exitToolStripMenuItem1.Text = "Exit";
            exitToolStripMenuItem1.Click += exitToolStripMenuItem1_Click;
            // 
            // downloads
            // 
            downloads.AllowUserToAddRows = false;
            downloads.BackgroundColor = SystemColors.ButtonHighlight;
            downloads.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            downloads.ContextMenuStrip = contextMenuStrip2;
            downloads.Dock = DockStyle.Fill;
            downloads.Location = new Point(0, 28);
            downloads.Name = "downloads";
            downloads.ReadOnly = true;
            downloads.RowHeadersWidth = 51;
            downloads.Size = new Size(928, 405);
            downloads.TabIndex = 1;
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.ImageScalingSize = new Size(20, 20);
            contextMenuStrip2.Items.AddRange(new ToolStripItem[] { restartDownloadToolStripMenuItem, copyDownloadLinkToolStripMenuItem });
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new Size(216, 52);
            contextMenuStrip2.Opening += contextMenuStrip2_Opening;
            // 
            // restartDownloadToolStripMenuItem
            // 
            restartDownloadToolStripMenuItem.Name = "restartDownloadToolStripMenuItem";
            restartDownloadToolStripMenuItem.Size = new Size(215, 24);
            restartDownloadToolStripMenuItem.Text = "Restart Download";
            restartDownloadToolStripMenuItem.Click += restartDownloadToolStripMenuItem_Click;
            // 
            // copyDownloadLinkToolStripMenuItem
            // 
            copyDownloadLinkToolStripMenuItem.Name = "copyDownloadLinkToolStripMenuItem";
            copyDownloadLinkToolStripMenuItem.Size = new Size(215, 24);
            copyDownloadLinkToolStripMenuItem.Text = "Copy Download Link";
            copyDownloadLinkToolStripMenuItem.Click += copyDownloadLinkToolStripMenuItem_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipText = "Turbo is running in the background";
            notifyIcon1.BalloonTipTitle = "Turbo Download Manager";
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Text = "Turbo Download Manager";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { showTurboToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(158, 58);
            // 
            // showTurboToolStripMenuItem
            // 
            showTurboToolStripMenuItem.Name = "showTurboToolStripMenuItem";
            showTurboToolStripMenuItem.Size = new Size(157, 24);
            showTurboToolStripMenuItem.Text = "Show Turbo";
            showTurboToolStripMenuItem.Click += showTurboToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(154, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(157, 24);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // TurboMgr
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(928, 433);
            Controls.Add(downloads);
            Controls.Add(menuStrip1);
            Font = new Font("Segoe UI", 13F);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4);
            Name = "TurboMgr";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Turbo Download Manager";
            Load += TurboMgr_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)downloads).EndInit();
            contextMenuStrip2.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private DataGridView downloads;
        private ToolStripMenuItem downloadAFileToolStripMenuItem;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem showTurboToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem1;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem restartDownloadToolStripMenuItem;
        private ToolStripMenuItem copyDownloadLinkToolStripMenuItem;
    }
}
