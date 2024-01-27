using Turbo_Download_Manager.Helpers;

namespace Turbo_Download_Manager
{
    public partial class TurboMgr : Form
    {
        public TurboMgr()
        {
            InitializeComponent();
        }

        private void downloadAFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDownload addDownload = new AddDownload();
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
    }
}
