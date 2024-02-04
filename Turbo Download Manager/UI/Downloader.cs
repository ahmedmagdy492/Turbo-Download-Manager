using System.Data;
using Turbo_Download_Manager.Helpers;
using Turbo_Download_Manager.Models;
using Turbo_Download_Manager.Repository;

namespace Turbo_Download_Manager
{
    public partial class Downloader : Form
    {
        private readonly Uri _downloadUri;
        private readonly HttpClient _httpClient;
        private readonly List<DownloadJob> _downloadJobs;
        private readonly string _tempFilesParentFolder = Environment.GetEnvironmentVariable("TEMP");
        private readonly string _finalFileParentFolder = Constants.FinalDownloadDirectory;
        private string _fileExtension = string.Empty;
        private bool _isTickEventProcessing = false;
        private readonly IFileDownloadRepository _fileDownloadRepository;
        private FileDownloadEntry _fileDownloadEntry;
        private string curMediaType;

        private readonly DownloadManager _downloadManager;

        private double CalcOverallProgress(double curByte, long totalFileSize)
        {
            return curByte / (double)totalFileSize * 100;
        }

        public Downloader(Uri downloadUri, FileDownloadEntry fileDownloadEntry)
        {
            InitializeComponent();

            _downloadManager = DownloadManager.CreateDownloadManager();
            _downloadManager.SubscribeToDownloadCancel(new Action<DownloadCancelInfo>(async (cancelInfo) =>
            {
                try
                {
                    _fileDownloadEntry.HasCompleted = false;
                    _fileDownloadEntry.ProgressPercent = $"{cancelInfo.Progress}%";
                    _fileDownloadRepository.UpdateFileDownloadEntry(_fileDownloadEntry);
                    await _fileDownloadRepository.SaveChanges();
                    Invoke(new Action(() =>
                    {
                        this.Close();
                    }));
                }
                catch { }
            }));
            _downloadManager.SubscribeToProgressUpdate(new Action<DownloadProgressInfo>((progressInfo) =>
            {
                try
                {
                    curMediaType = progressInfo.CurrentDownloadMediaType;
                    Invoke(new Action(() =>
                    {
                        Text = $"Downloaded {Math.Round(progressInfo.Progress, 2)}% of {Utils.GetFileName(_downloadUri)}";
                    }));

                    lblDownloadPrgrs.Invoke(new Action(() =>
                    {
                        lblDownloadPrgrs.Text = $"Downloaded {Math.Floor(CalcOverallProgress(progressInfo.CurrentByte, progressInfo.TotalDownloadLength))}%";
                    }));
                    overallProgress.Invoke(new Action(() =>
                    {
                        overallProgress.Value = (int)Math.Floor(CalcOverallProgress(progressInfo.CurrentByte, progressInfo.TotalDownloadLength));
                    }));
                    downloadProgressBar.Invoke(new Action(() =>
                    {
                        downloadProgressBar.Value = (int)Math.Round(progressInfo.Progress, 2);
                    }));
                    lblThreadsCount.Invoke(new Action(() =>
                    {
                        lblThreadsCount.Text = $"{progressInfo.CurrentByte/1024/1024} MB/{progressInfo.FileSize/1024/1024} MB";
                    }));
                }
                catch { }
            }));

            _downloadManager.SubscribeToDownloadEnded(new Action(async () =>
            {
                try
                {
                    Invoke(new Action(() =>
                    {
                        string fileName = Utils.GetFileName(_downloadUri);
                        string fullPath = Path.Combine(Constants.FinalDownloadDirectory, fileName + Utils.GetAccurateExtension(Utils.GetExtensionFromMimeType(curMediaType), fileName));
                        DownloadComplete downloadComplete = new DownloadComplete(Constants.FinalDownloadDirectory, fullPath);
                        this.Close();
                        downloadComplete.Show();
                    }));
                }
                catch { }

                _fileDownloadEntry.HasCompleted = true;
                _fileDownloadRepository.UpdateFileDownloadEntry(_fileDownloadEntry);
                await _fileDownloadRepository.SaveChanges();
            }));

            _downloadUri = downloadUri;
            _httpClient = new HttpClient
            {
                BaseAddress = _downloadUri
            };
            _downloadJobs = new List<DownloadJob>();
            UnitOfWork unitOfWork = new UnitOfWork();
            _fileDownloadRepository = unitOfWork.FileDownloadRepository;
            _fileDownloadEntry = fileDownloadEntry;
        }

        private async void Downloader_Load(object sender, EventArgs e)
        {
            _downloadManager.CreateNewDownload(_downloadUri.OriginalString);
            await _downloadManager.StartDownload();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //CancelAllTasks();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _downloadManager.CancelDownload();
        }
    }
}
