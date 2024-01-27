using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Turbo_Download_Manager
{
    public partial class AddDownload : Form
    {
        public AddDownload()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            string downloadLink = txtDownloadLink.Text;

            if(!string.IsNullOrWhiteSpace(downloadLink))
            {
                try
                {
                    var url = new Uri(downloadLink);
                    Downloader downloader = new Downloader(url);
                    this.Close();
                    downloader.Show();
                }
                catch(UriFormatException)
                {
                    if(!downloadLink.StartsWith("http"))
                    {
                        MessageBox.Show("Turbo Download Manager only supports http protocol");
                    }
                    else
                    {
                        MessageBox.Show("Invalid Link Provided");
                    }
                }
            }
        }
    }
}
