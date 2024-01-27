using Turbo_Download_Manager.Database;
using Turbo_Download_Manager.Helpers;
using Turbo_Download_Manager.Repository;

namespace Turbo_Download_Manager
{
    public partial class TurboMgr : Form
    {
        private readonly AppDBContext _appDBContext;
        private readonly IFileDownloadRepository _fileDownloadRepository;

        public TurboMgr()
        {
            InitializeComponent();
            _appDBContext = new AppDBContext();
            _fileDownloadRepository = new FileDownloadRepository(_appDBContext);
        }

        private void downloadAFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDownload addDownload = new AddDownload(_fileDownloadRepository);
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
            notifyIcon1.ShowBalloonTip(20);
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
            downloads.DataSource = (await _fileDownloadRepository.GetDownloadsBy()).Select(f => new { f.FileName, f.DownloadUrl, f.SavePath, f.StartDownloadDateTime }).ToList();
        }
    }
}
