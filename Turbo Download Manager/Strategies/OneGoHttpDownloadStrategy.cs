using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo_Download_Manager.Helpers;

namespace Turbo_Download_Manager.Strategies
{
    public class OneGoHttpDownloadStrategy : IDownloadStrategy
    {
        private readonly HttpClient _httpClient;
        private readonly long _totalDownloadSize;
        private readonly List<Action<DownloadProgressInfo>> _progressUpdateSubscribers;
        private readonly List<Action> _downloadEndedSubscribers;

        public OneGoHttpDownloadStrategy(long totalDownloadSize, string url, List<Action<DownloadProgressInfo>> progressUpdateSubscribers, List<Action> downloadEndedSubscribers)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
            this._totalDownloadSize = totalDownloadSize;
            _progressUpdateSubscribers = progressUpdateSubscribers;
            _downloadEndedSubscribers = downloadEndedSubscribers;
        }

        public void Cancel()
        {
            
        }

        public Task Download()
        {
            return Task.Run(async () =>
            {
                foreach (var subscriber in _progressUpdateSubscribers)
                {
                    subscriber(new DownloadProgressInfo
                    {
                        Progress = 0.0
                    });
                }

                HttpResponseMessage response = await _httpClient.GetAsync("");

                response.EnsureSuccessStatusCode();

                foreach (var subscriber in _progressUpdateSubscribers)
                {
                    subscriber(new DownloadProgressInfo
                    {
                        Progress = 50.0
                    });
                }

                string fileName = Utils.GetFileName(_httpClient.BaseAddress);
                string fileExtension = Utils.GetAccurateExtension(Utils.GetExtensionFromMimeType(response.Content.Headers.ContentType.MediaType), _httpClient.BaseAddress.OriginalString);

                string fullPath = Path.Combine(Constants.FinalDownloadDirectory, fileName + fileExtension);

                var responseContent = await response.Content.ReadAsByteArrayAsync();

                foreach (var subscriber in _progressUpdateSubscribers)
                {
                    subscriber(new DownloadProgressInfo
                    {
                        Progress = 75
                    });
                }

                File.WriteAllBytes(fullPath, responseContent);

                foreach (var subscriber in _progressUpdateSubscribers)
                {
                    subscriber(new DownloadProgressInfo { Progress = 100 });
                }

                foreach (var subscriber in _downloadEndedSubscribers)
                {
                    subscriber();
                }
            });
        }
    }
}
