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

namespace Turbo_Download_Manager
{
    public partial class Downloader : Form
    {
        private readonly Uri _downloadUri;
        private readonly HttpClient _httpClient;
        private readonly List<DownloadJob> _downloadJobs;
        private readonly string _tempFilesParentFolder = Environment.GetEnvironmentVariable("TEMP");
        private readonly string _finalFileParentFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        private string _fileExtension = string.Empty;
        private bool _isTickEventProcessing = false;

        public Downloader(Uri downloadUri)
        {
            InitializeComponent();
            _downloadUri = downloadUri;
            _httpClient = new HttpClient
            {
                BaseAddress = _downloadUri
            };
            _downloadJobs = new List<DownloadJob>();
        }

        private async Task DownloadChunck(DownloadMetaData chunckMetaData)
        {
            double progress = 0;
            int BUFF_SIZE = 4096;
            long start = chunckMetaData.startByte;
            byte[] buffer = new byte[chunckMetaData.chunkLength];
            int lastSrcIndex = 0, curResponseLength = 0;

            while (chunckMetaData.startByte < (start + chunckMetaData.chunkLength))
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "");
                    request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(chunckMetaData.startByte, chunckMetaData.startByte + Math.Min(BUFF_SIZE, (start + chunckMetaData.chunkLength) - chunckMetaData.startByte)-1);
                    var response = await _httpClient.SendAsync(request);
                    if(response.StatusCode == System.Net.HttpStatusCode.PartialContent)
                    {
                        byte[] temp = await response.Content.ReadAsByteArrayAsync();
                        curResponseLength = temp.Length;
                        Array.Copy(temp, 0, buffer, lastSrcIndex, temp.Length);
                        lastSrcIndex += temp.Length;

                        chunckMetaData.startByte += temp.Length;
                        progress = ((double)chunckMetaData.startByte - (double)start) / ((double)chunckMetaData.chunkLength) * 100.0;
                        lblDownloadPrgrs.Invoke(new Action(() =>
                        {
                            lblDownloadPrgrs.Text = $"Downloading {Math.Round(progress, 2)}%";
                        }));
                        downloadProgressBar.Invoke(new Action(() =>
                        {
                            downloadProgressBar.Value = (int)Math.Round(progress, 2);
                        }));
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error Occured while downloading the file: {ex.Message}, lastSrcIndex: {lastSrcIndex}, chuncLength: {chunckMetaData.chunkLength}, curResponseLength: {curResponseLength}", "Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            File.WriteAllBytes(chunckMetaData.fileName, buffer);

            chunckMetaData.job.IsCompeleted = true;
        }

        private async void Downloader_Load(object sender, EventArgs e)
        {
            const int MAX_THREADS = 10;
            long totalLength = 1;
            double BUFF_SIZE = 4096;

            var request = new HttpRequestMessage(HttpMethod.Head, "");
            request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(0, (int)BUFF_SIZE);
            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.PartialContent)
            {
                totalLength = response.Content.Headers.ContentRange.Length ?? 1;
                _fileExtension = Utils.GetAccurateExtension(Utils.GetExtensionFromMimeType(response.Content.Headers.ContentType.MediaType), _downloadUri.OriginalString);

                BUFF_SIZE = (double)totalLength / (double)MAX_THREADS;

                lblThreadsCount.Invoke(new Action(() =>
                {
                    lblThreadsCount.Text = $"Threads Count: {MAX_THREADS}/File Size: {Math.Round(((double)totalLength) / 1024.0 / 1024.0, 2)} MB/File Type:  {_fileExtension}";
                }));

                Invoke(new Action(() =>
                {
                    Text = $"Downloading {Utils.GetFileName(_downloadUri)}";
                }));

                for (int thread_counter = 0; thread_counter < MAX_THREADS; thread_counter++)
                {
                    var downloadJob = new DownloadJob();
                    var chunckMetaData = new DownloadMetaData
                    {
                        chunkLength = (int)Math.Round(BUFF_SIZE),
                        startByte = ((int)Math.Round(BUFF_SIZE)) * thread_counter,
                        fileName = Path.Combine(_tempFilesParentFolder, Guid.NewGuid().ToString("N")),
                        job = downloadJob
                    };
                    downloadJob.DownloadTask = Task.Run(() =>
                    {
                        DownloadChunck(chunckMetaData);
                    });
                    downloadJob.FileName = chunckMetaData.fileName;
                    _downloadJobs.Add(downloadJob);
                }

                downloadTasksTracker.Enabled = true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
