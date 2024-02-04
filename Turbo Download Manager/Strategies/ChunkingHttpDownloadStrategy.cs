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
        private readonly List<Action<DownloadCancelInfo>> _onDownloadCancelSubscribers;
        private readonly List<Action> _downloadEndedSubscribers;
        private readonly HttpClient _httpClient;
        private readonly List<DownloadGroup> _downloadGroups;
        private readonly long _downloadFileSize;
        private readonly string _tempDownloadDir = Environment.GetEnvironmentVariable("TEMP");
        private int nextGroupToRunIndex = 0;
        private const int chunkSize = 2097152;
        private string currentDownloadMimeType;

        public ChunkingHttpDownloadStrategy(long downloadFileSize, string url, List<Action<DownloadProgressInfo>> progressUpdateSubscribers, List<Action> downloadEndedSubscribers, List<Action<DownloadCancelInfo>> onDownloadCancel)
        {
            _downloadGroups = new List<DownloadGroup>();
            _downloadFileSize = downloadFileSize;
            _onDownloadCancelSubscribers = onDownloadCancel;

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
                int noOfGroups = (int)Math.Ceiling((double)_downloadFileSize / (double)Constants.MaxFileSizeBeforeDividingToGroups);
                long sizePerGroup = (int)Math.Ceiling((double)_downloadFileSize / (double)noOfGroups);

                for(int i = 0; i < noOfGroups; i++)
                {
                    var downloadGroup = new DownloadGroup
                    {
                        IsCompleted = false,
                        TempFilePath = Path.Combine(_tempDownloadDir, Guid.NewGuid().ToString("N")),
                        StartByte = i * (sizePerGroup+1),
                        CurrentByte = i * (sizePerGroup+1),
                        FileSize = i * sizePerGroup + sizePerGroup,
                        CancellationToken = new CancellationTokenSource(),
                        Logger = new FileLogger(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"Group_{i}_log.log")),
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
            }
        }

        private async Task DownloadInChunks(DownloadGroup downloadGroup)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "");
            request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(downloadGroup.CurrentByte, Math.Min(downloadGroup.CurrentByte +
                chunkSize, downloadGroup.FileSize));
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

            BinaryWriter bw = new BinaryWriter(new FileStream(downloadGroup.TempFilePath, FileMode.Append));
            bw.Write(content);
            bw.Close();

            //downloadGroup.Logger.LogWarning($"Current Byte: {downloadGroup.CurrentByte}, Response Length: {content.Length}, FileSize: {downloadGroup.FileSize}");

            downloadGroup.CurrentByte += content.Length;

            //if(!downloadGroup.CancellationToken.IsCancellationRequested && downloadGroup.CurrentByte < (downloadGroup.StartByte + downloadGroup.FileSize))
            //{
            //    await DownloadInChunks(downloadGroup);
            //}

            if (!downloadGroup.CancellationToken.IsCancellationRequested && downloadGroup.CurrentByte < downloadGroup.FileSize)
            {
                await DownloadInChunks(downloadGroup);
            }
            else if(downloadGroup.CancellationToken.IsCancellationRequested)
            {
                foreach(var downloadCancelSubscriber in _onDownloadCancelSubscribers)
                {
                    downloadCancelSubscriber(new DownloadCancelInfo
                    {
                        CurrentByte = downloadGroup.CurrentByte,
                        FileSize = downloadGroup.FileSize,
                        Progress = Math.Floor(progress * 100)
                    });
                }
            }
            else
            {
                downloadGroup.OnGroupDone();

                if(_downloadGroups.Where(g => g.IsCompleted).Count() == _downloadGroups.Count)
                {
                    foreach (var downloadSubscriber in _downloadEndedSubscribers)
                    {
                        downloadSubscriber();
                    }
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

                if(File.Exists(fullPath))
                {
                    fileName = fileName + Guid.NewGuid().ToString("N")[10..];
                    fullPath = Path.Combine(Constants.FinalDownloadDirectory, fileName + fileExtension);
                }                

                foreach(var downloadGroup in _downloadGroups)
                {
                    BinaryWriter bw = new BinaryWriter(new FileStream(fullPath, FileMode.Append));
                    var fileContent = File.ReadAllBytes(downloadGroup.TempFilePath);
                    bw.Write(fileContent);
                    downloadGroup.IsCompleted = true;
                    bw.Close();
                }
            }

            return Task.CompletedTask;
        }

        public void Cancel()
        {
            foreach(var downloadGroup in _downloadGroups)
            {
                downloadGroup.CancellationToken.Cancel();
            }
        }
    }
}
