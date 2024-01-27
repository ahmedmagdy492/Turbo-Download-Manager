using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Turbo_Download_Manager
{
    public partial class DownloadComplete : Form
    {
        private readonly string _downloadFolder;
        private readonly string _fileName;

        public DownloadComplete(string downloadFolder, string fileName)
        {
            InitializeComponent();
            _downloadFolder = downloadFolder;
            _fileName = fileName;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = _downloadFolder,
                UseShellExecute = false,
            });
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DownloadComplete_Load(object sender, EventArgs e)
        {
            txtDownloadPath.Text = Path.Combine(_downloadFolder, _fileName);
        }
    }
}
