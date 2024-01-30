using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Turbo_Download_Manager.Helpers;

namespace Turbo_Download_Manager.Strategies
{
    public class ChunkingHttpDownloadStrategy : IDownloadStrategy
    {
        private readonly List<Action<DownloadProgressInfo>> _progressUpdateSubscribers;
        private readonly List<Action> _downloadEndedSubscribers;
        private readonly HttpClient _httpClient;
        private readonly List<DownloadGroup> _downloadGroups;
        private readonly long _downloadFileSize;
        private readonly string _tempDownloadDir = Environment.GetEnvironmentVariable("TEMP");
        private int nextGroupToRunIndex = 0;
        private const int chunkSize = 2097152;
        private string currentDownloadMimeType;

        public ChunkingHttpDownloadStrategy(long downloadFileSize, string url, List<Action<DownloadProgressInfo>> progressUpdateSubscribers, List<Action> downloadEndedSubscribers)
        {
            _downloadGroups = new List<DownloadGroup>();
            _downloadFileSize = downloadFileSize;

            _progressUpdateSubscribers = progressUpdateSubscribers;
            _downloadEndedSubscribers = downloadEndedSubscribers;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(url)
            };

            CreateGroupsBasedOnFileSize();
        }

        private void CreateGroupsBasedOnFileSize()
        {
            if (_downloadFileSize <= Constants.MaxFileSizeBeforeDividingToGroups)
            {
                var downloadGroup = new DownloadGroup
                {
                    FileSize = _downloadFileSize,
                    StartByte = 0,
                    CancellationToken = new CancellationTokenSource(),
                    TempFilePath = Path.Combine(_tempDownloadDir, Guid.NewGuid().ToString("N")),
                    IsCompleted = false,
                    CurrentByte = 0,
                    OnGroupDone = new Action(() =>
                    {
                        Download();
                    })
                };
                _downloadGroups.Add(downloadGroup);
                downloadGroup.BackgroundTask = new Task(new Action(async () =>
                {
                    await DownloadInChunks(downloadGroup);
                }), 
                downloadGroup.CancellationToken.Token);
            }
            else
            {
                // dividing into multiple groups
            }
        }

        private async Task DownloadInChunks(DownloadGroup downloadGroup)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "");
            request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(downloadGroup.CurrentByte, downloadGroup.CurrentByte +
                chunkSize);
            var response = await _httpClient.SendAsync(request);
            currentDownloadMimeType = response.Content.Headers.ContentType.MediaType;
            var content = await response.Content.ReadAsByteArrayAsync();
            double progress = (double)downloadGroup.CurrentByte / (double)downloadGroup.FileSize;

            foreach (var progressSubscriber in _progressUpdateSubscribers)
            {
                progressSubscriber(new DownloadProgressInfo {
                    Progress = Math.Floor(progress * 100),
                    FileSize = downloadGroup.FileSize,
                    CurrentByte = downloadGroup.CurrentByte
                });
            }

            FileStream fs = new FileStream(downloadGroup.TempFilePath, FileMode.Append);
            fs.Write(content, 0, content.Length);
            fs.Close();

            downloadGroup.CurrentByte += content.Length;

            if(!downloadGroup.CancellationToken.IsCancellationRequested && downloadGroup.CurrentByte < (downloadGroup.StartByte + downloadGroup.FileSize))
            {
                await DownloadInChunks(downloadGroup);
            }
            else
            {
                downloadGroup.OnGroupDone();

                foreach(var downloadSubscriber in _downloadEndedSubscribers)
                {
                    downloadSubscriber();
                }
            }
        }

        public Task Download()
        {
            if(nextGroupToRunIndex < _downloadGroups.Count)
                _downloadGroups[nextGroupToRunIndex++].BackgroundTask.Start();
            else
            {
                string fileName = Utils.GetFileName(_httpClient.BaseAddress);
                string fileExtension = Utils.GetAccurateExtension(Utils.GetExtensionFromMimeType(currentDownloadMimeType), _httpClient.BaseAddress.OriginalString);

                string fullPath = Path.Combine(Constants.FinalDownloadDirectory, fileName + fileExtension);

                if(!File.Exists(fullPath))
                {
                    File.Create(fullPath).Close();
                }

                var fs = new FileStream(fullPath, FileMode.Append);

                foreach(var downloadGroup in _downloadGroups)
                {
                    var fileContent = File.ReadAllBytes(downloadGroup.TempFilePath);
                    fs.Write(fileContent, 0, fileContent.Length);
                }

                fs.Close();
            }

            return Task.CompletedTask;
        }
    }
}
