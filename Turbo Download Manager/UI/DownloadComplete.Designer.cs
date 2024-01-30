namespace Turbo_Download_Manager
{
    partial class DownloadComplete
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadComplete));
            label1 = new Label();
            btnOk = new Button();
            btnClose = new Button();
            txtDownloadPath = new TextBox();
            btnOpenFile = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(201, 49);
            label1.Name = "label1";
            label1.Size = new Size(224, 30);
            label1.TabIndex = 0;
            label1.Text = "Download Compelete";
            // 
            // btnOk
            // 
            btnOk.Location = new Point(45, 157);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(172, 49);
            btnOk.TabIndex = 1;
            btnOk.Text = "Open Folder";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(410, 157);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(172, 49);
            btnClose.TabIndex = 2;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // txtDownloadPath
            // 
            txtDownloadPath.Location = new Point(48, 95);
            txtDownloadPath.Name = "txtDownloadPath";
            txtDownloadPath.ReadOnly = true;
            txtDownloadPath.Size = new Size(534, 36);
            txtDownloadPath.TabIndex = 3;
            // 
            // btnOpenFile
            // 
            btnOpenFile.Location = new Point(232, 157);
            btnOpenFile.Name = "btnOpenFile";
            btnOpenFile.Size = new Size(172, 49);
            btnOpenFile.TabIndex = 4;
            btnOpenFile.Text = "Open File";
            btnOpenFile.UseVisualStyleBackColor = true;
            btnOpenFile.Click += btnOpenFile_Click;
            // 
            // DownloadComplete
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(627, 254);
            Controls.Add(btnOpenFile);
            Controls.Add(txtDownloadPath);
            Controls.Add(btnClose);
            Controls.Add(btnOk);
            Controls.Add(label1);
            Font = new Font("Segoe UI", 13F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DownloadComplete";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Download Complete";
            TopMost = true;
            Load += DownloadComplete_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnOk;
        private Button btnClose;
        private TextBox txtDownloadPath;
        private Button btnOpenFile;
    }
}