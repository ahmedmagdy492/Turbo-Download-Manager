using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turbo_Download_Manager.Helpers;
using Turbo_Download_Manager.Repository;

namespace Turbo_Download_Manager
{
    public partial class AddDownload : Form
    {
        private readonly IFileDownloadRepository _fileDownloadRepository;
        public AddDownload(IFileDownloadRepository fileDownloadRepository)
        {
            InitializeComponent();
            _fileDownloadRepository = fileDownloadRepository;
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            string downloadLink = txtDownloadLink.Text;

            if(!string.IsNullOrWhiteSpace(downloadLink))
            {
                try
                {
                    var url = new Uri(downloadLink);
                    Downloader downloader = new Downloader(url);
                    _fileDownloadRepository.CreateFileDownloadEntry(new Models.FileDownloadEntry
                    {
                        FileName = Utils.GetFileName(url),
                        StartDownloadDateTime = DateTime.Now,
                        DownloadUrl = downloadLink,
                        SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
                    });
                    var result = await _fileDownloadRepository.SaveChanges();
                    this.Close();
                    downloader.Show();
                }
                catch(UriFormatException)
                {
                    if(!downloadLink.StartsWith("http"))
                    {
                        MessageBox.Show("Turbo Download Manager only supports http protocol", "Error Occcured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Invalid Link Provided", "Error Occcured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Error Occured: {ex.Message}", "Error Occcured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
