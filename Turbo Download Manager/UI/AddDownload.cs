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
        public Action _onAddingDownload = null;

        public AddDownload()
        {
            InitializeComponent();
            UnitOfWork unitOfWork = new UnitOfWork();
            _fileDownloadRepository = unitOfWork.FileDownloadRepository;
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            string downloadLink = txtDownloadLink.Text;

            if(!string.IsNullOrWhiteSpace(downloadLink))
            {
                try
                {
                    var url = new Uri(downloadLink);
                    var fileDownloadEntry = _fileDownloadRepository.CreateFileDownloadEntry(new Models.FileDownloadEntry
                    {
                        FileName = Utils.GetFileName(url),
                        StartDownloadDateTime = DateTime.Now,
                        DownloadUrl = downloadLink,
                        SavePath = Constants.FinalDownloadDirectory
                    });
                    var result = await _fileDownloadRepository.SaveChanges();
                    if(_onAddingDownload != null)
                    {
                        _onAddingDownload();
                    }
                    this.Close();
                    Downloader downloader = new Downloader(url, fileDownloadEntry);
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
