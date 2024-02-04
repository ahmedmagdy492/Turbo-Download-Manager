namespace Turbo_Download_Manager
{
    partial class AddDownload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddDownload));
            label1 = new Label();
            txtDownloadLink = new TextBox();
            btnDownload = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(22, 16);
            label1.Name = "label1";
            label1.Size = new Size(155, 30);
            label1.TabIndex = 0;
            label1.Text = "Download Link";
            // 
            // txtDownloadLink
            // 
            txtDownloadLink.Location = new Point(26, 50);
            txtDownloadLink.Name = "txtDownloadLink";
            txtDownloadLink.Size = new Size(440, 36);
            txtDownloadLink.TabIndex = 1;
            // 
            // btnDownload
            // 
            btnDownload.Location = new Point(479, 45);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(139, 46);
            btnDownload.TabIndex = 2;
            btnDownload.Text = "Download";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // AddDownload
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(630, 112);
            Controls.Add(btnDownload);
            Controls.Add(txtDownloadLink);
            Controls.Add(label1);
            Font = new Font("Segoe UI", 13F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddDownload";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Download a File";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtDownloadLink;
        private Button btnDownload;
    }
}