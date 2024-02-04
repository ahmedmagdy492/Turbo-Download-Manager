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
        private bool _isPaused = false;
        private bool _execptionOccured = false;
        private Exception _ex;
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

        #region old downloading mechanism
        private async Task DownloadChunck(DownloadMetaData chunckMetaData)
        {
            double progress = 0;
            int BUFF_SIZE = 4096;
            long start = chunckMetaData.startByte;
            byte[] buffer = new byte[chunckMetaData.chunkLength];
            int lastSrcIndex = 0, curResponseLength = 0;

            while (!chunckMetaData.cancellationTokenSource.IsCancellationRequested && chunckMetaData.startByte < (start + chunckMetaData.chunkLength))
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "");
                    request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(chunckMetaData.startByte, chunckMetaData.startByte + Math.Min(BUFF_SIZE, (start + chunckMetaData.chunkLength) - chunckMetaData.startByte) - 1);
                    var response = await _httpClient.SendAsync(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.PartialContent)
                    {
                        byte[] temp = await response.Content.ReadAsByteArrayAsync();
                        curResponseLength = temp.Length;
                        Array.Copy(temp, 0, buffer, lastSrcIndex, temp.Length);
                        lastSrcIndex += temp.Length;

                        chunckMetaData.startByte += temp.Length;
                        progress = ((double)chunckMetaData.startByte - (double)start) / ((double)chunckMetaData.chunkLength) * 100.0;

                        
                    }
                    else
                    {
                        break;
                    }
                }
                catch(HttpRequestException ex)
                {
                    chunckMetaData.cancellationTokenSource.Cancel();
                    _execptionOccured = true;
                    _ex = ex;
                }
                catch (ObjectDisposedException) { }
                catch (Exception ex)
                {
                    chunckMetaData.cancellationTokenSource.Cancel();
                    _execptionOccured = true;
                    _ex = ex;
                }
            }

            if (!chunckMetaData.cancellationTokenSource.IsCancellationRequested)
            {
                File.WriteAllBytes(chunckMetaData.fileName, buffer);

                chunckMetaData.job.IsCompeleted = true;
            }
        }

        private async Task StratDownloadProcess(int startingByte)
        {
            int MAX_THREADS = 10;
            long totalLength = 1;
            double BUFF_SIZE = 4096;

            var request = new HttpRequestMessage(HttpMethod.Head, "");
            request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(startingByte, (int)BUFF_SIZE);
            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.PartialContent)
            {
                totalLength = response.Content.Headers.ContentRange.Length ?? 1;
                _fileExtension = Utils.GetAccurateExtension(Utils.GetExtensionFromMimeType(response.Content.Headers.ContentType.MediaType), _downloadUri.OriginalString);

                if (totalLength <= Constants.MaxSizeBeforeChunking)
                {
                    MAX_THREADS = 1;
                }

                BUFF_SIZE = (double)totalLength / (double)MAX_THREADS;

                

                for (int thread_counter = startingByte; thread_counter < MAX_THREADS; thread_counter++)
                {
                    var downloadJob = new DownloadJob();
                    var chunckMetaData = new DownloadMetaData
                    {
                        chunkLength = (int)Math.Round(BUFF_SIZE),
                        startByte = ((int)Math.Round(BUFF_SIZE)) * thread_counter,
                        fileName = Path.Combine(_tempFilesParentFolder, Guid.NewGuid().ToString("N")),
                        job = downloadJob,
                        cancellationTokenSource = new CancellationTokenSource()
                    };
                    downloadJob.DownloadTask = Task.Run(async () =>
                    {
                        await DownloadChunck(chunckMetaData);
                    });
                    downloadJob.FileName = chunckMetaData.fileName;
                    downloadJob.JobDownloadMetaData = chunckMetaData;
                    downloadJob.JobCancellationToken = chunckMetaData.cancellationTokenSource;
                    _downloadJobs.Add(downloadJob);
                }

                downloadTasksTracker.Enabled = true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // TODO: download the file from scratch or download the file without byte chuncking
                MessageBox.Show("Error: The Server does not support byte chuncking", "Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            else
            {
                MessageBox.Show($"Error: The Server returned {response.StatusCode}", "Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void downloadTasksTracker_Tick(object sender, EventArgs e)
        {
            if (_execptionOccured)
            {
                _execptionOccured = false;
                MessageBox.Show($"Error Occured while downloading the file: {_ex.Message}, {_ex.StackTrace}", "Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if (!_isTickEventProcessing)
            {
                if (_downloadJobs.Where(db => db.IsCompeleted).Count() == _downloadJobs.Count)
                {
                    _isTickEventProcessing = true;
                    try
                    {
                        if (!Directory.Exists(_finalFileParentFolder))
                        {
                            Directory.CreateDirectory(_finalFileParentFolder);
                        }

                        string fullPath = Path.Combine(_finalFileParentFolder, Utils.GetFileName(_downloadUri) + _fileExtension);
                        if (!File.Exists(fullPath))
                        {
                            File.Create(fullPath).Close();
                        }
                        else
                        {
                            string parentDir = Path.GetDirectoryName(fullPath);
                            string newFileName = Path.GetFileNameWithoutExtension(fullPath) + Guid.NewGuid().ToString("N").Substring(0, 10) + Path.GetExtension(fullPath);
                            fullPath = Path.Combine(parentDir, newFileName);
                        }

                        var fs = new FileStream(fullPath, FileMode.Append);

                        foreach (var downloadJob in _downloadJobs)
                        {
                            byte[] currentJobFileContent = File.ReadAllBytes(downloadJob.FileName);
                            fs.Write(currentJobFileContent, 0, currentJobFileContent.Length);
                        }

                        fs.Close();

                        downloadTasksTracker.Enabled = false;
                        DownloadComplete downloadComplete = new DownloadComplete(Path.GetDirectoryName(fullPath), Path.GetFileName(fullPath));
                        this.Close();
                        downloadComplete.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error Occured while saving the file {ex.Message}", "Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        _isTickEventProcessing = false;
                    }
                }
            }
        }

        private void CancelAllTasks()
        {
            foreach (var downloadJob in _downloadJobs)
            {
                if (!downloadJob.DownloadTask.IsCanceled)
                {
                    downloadJob.JobCancellationToken.Cancel();
                }
            }

            downloadTasksTracker.Enabled = false;

            _downloadJobs.Clear();
        }
        #endregion

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
