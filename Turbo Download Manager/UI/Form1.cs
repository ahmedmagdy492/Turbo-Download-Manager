using SQLitePCL;
using System.Windows.Forms;
using Turbo_Download_Manager.Database;
using Turbo_Download_Manager.Helpers;
using Turbo_Download_Manager.Repository;

namespace Turbo_Download_Manager
{
    public partial class TurboMgr : Form
    {
        private readonly IFileDownloadRepository _fileDownloadRepository;

        public TurboMgr()
        {
            InitializeComponent();
            UnitOfWork unitOfWork = new UnitOfWork();
            _fileDownloadRepository = unitOfWork.FileDownloadRepository;
        }

        private async Task GetAndAppendFileDownloadEntries()
        {
            var fileDownloads = (await _fileDownloadRepository.GetDownloadsBy(1, 100)).ToList();

            downloads.Columns.Clear();

            downloads.Columns.Add("FileID", "File Id");
            downloads.Columns.Add("FileName", "File Name");
            downloads.Columns.Add("SavePath", "Save Location");
            downloads.Columns.Add("HasCompleted", "Status");
            downloads.Columns.Add("StartDownloadDateTime", "Download Date");

            downloads.Columns[0].Visible = false;

            foreach (var file in fileDownloads)
            {
                var row = new DataGridViewRow();
                row.CreateCells(downloads);

                row.Cells[0].Value = file.FileId;
                row.Cells[1].Value = file.FileName;
                row.Cells[2].Value = file.SavePath;
                row.Cells[3].Value = file.HasCompleted ? "Completed" : "Not Completed";
                row.Cells[4].Value = $"{file.StartDownloadDateTime.ToShortDateString()} {file.StartDownloadDateTime.ToShortTimeString()}";

                downloads.Rows.Add(row);
            }

            downloads.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void downloadAFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDownload addDownload = new AddDownload();
            addDownload._onAddingDownload += () =>
            {
                downloads.Invoke(new Action(async () =>
                {
                    await GetAndAppendFileDownloadEntries();
                }));
            };
            addDownload.Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            notifyIcon1.Text = "Turbo Download Manager";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipText = "Turbo Download Manager is working in the background";
            notifyIcon1.BalloonTipTitle = "Turbo Download Manager";
            notifyIcon1.Icon = this.Icon;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void showTurboToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private async void TurboMgr_Load(object sender, EventArgs e)
        {
            await GetAndAppendFileDownloadEntries();
        }

        private async void copyDownloadLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (downloads.SelectedRows.Count == 1)
            {
                var fileId = downloads.SelectedRows[0].Cells[0].Value.ToString();
                var fileDownloadEntry = await _fileDownloadRepository.SearchByFileId(fileId);

                if (fileDownloadEntry != null)
                {
                    Clipboard.SetText(fileDownloadEntry.DownloadUrl);
                }
            }
        }

        private async void restartDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (downloads.SelectedRows.Count == 1)
            {
                var fileId = downloads.SelectedRows[0].Cells[0].Value.ToString();
                var fileDownloadEntry = await _fileDownloadRepository.SearchByFileId(fileId);

                if (fileDownloadEntry != null)
                {
                    Downloader downloader = new Downloader(new Uri(fileDownloadEntry.DownloadUrl), fileDownloadEntry);
                    downloader.Show();
                }
            }
        }

        private void contextMenuStrip2_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(downloads.SelectedRows.Count > 0)
            {
                copyDownloadLinkToolStripMenuItem.Enabled = true;
                restartDownloadToolStripMenuItem.Enabled = true;
            }
            else
            {
                copyDownloadLinkToolStripMenuItem.Enabled = false;
                restartDownloadToolStripMenuItem.Enabled = false;
            }
        }
    }
}
