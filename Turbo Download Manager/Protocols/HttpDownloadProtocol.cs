using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Turbo_Download_Manager.Exceptions;
using Turbo_Download_Manager.Helpers;
using Turbo_Download_Manager.Strategies;

namespace Turbo_Download_Manager.Protocols
{
    public class HttpDownloadProtocol : IDownloadProtocol
    {
        private IDownloadStrategy _downloadStrategy;
        private readonly HttpClient _httpClient;
        List<Action<DownloadProgressInfo>> _progressUpdateSubscribers;
        List<Action> _downloadEndedSubscribers;
        List<Action<DownloadCancelInfo>> _onCancelDownloadSubscribers;

        public HttpDownloadProtocol(string url, List<Action<DownloadProgressInfo>> progressUpdateSubscribers, List<Action> downloadEndedSubscribers, List<Action<DownloadCancelInfo>> onCancelDownloadSubscribers)
        {
            _progressUpdateSubscribers = progressUpdateSubscribers;
            _downloadEndedSubscribers = downloadEndedSubscribers;
            _onCancelDownloadSubscribers = onCancelDownloadSubscribers;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
        }

        private long GetFreeSpaceOfADrive(string driveLetter)
        {
            DriveInfo driveInfo = new DriveInfo(driveLetter);
            return driveInfo.AvailableFreeSpace;
        }

        private async Task<Strategies.IDownloadStrategy> DetermineDownloadStrategy()
        {
            var request = new HttpRequestMessage(HttpMethod.Head, "");
            request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(0, 1024);
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            long fileSize = (response.Content.Headers.ContentRange != null ? response.Content.Headers.ContentRange.Length : response.Content.Headers.ContentLength) ?? 0;

            if(fileSize > Constants.MaxFileSizeToDownload)
            {
                throw new ExceededDownloadMaxLengthException($"Exceeded max download size which is {Constants.MaxFileSizeToDownload}");
            }

            if(fileSize >= GetFreeSpaceOfADrive(Environment.GetEnvironmentVariable("HOMEDRIVE")))
            {
                throw new InsuffcientDiskSpaceException($"File size {fileSize/1024/1024} MB is larger than available disk space in {Constants.FinalDownloadDirectory}");
            }

            if(response.StatusCode == System.Net.HttpStatusCode.MovedPermanently || response.StatusCode == System.Net.HttpStatusCode.Redirect)
            {
                // TODO: call a function to calls itself recursively to finally get the url to download the file from
            }

            if (response.StatusCode == System.Net.HttpStatusCode.PartialContent)
            {
                return new ChunkingHttpDownloadStrategy(fileSize, _httpClient.BaseAddress.OriginalString, _progressUpdateSubscribers, _downloadEndedSubscribers, _onCancelDownloadSubscribers);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new OneGoHttpDownloadStrategy(fileSize, _httpClient.BaseAddress.OriginalString, _progressUpdateSubscribers, _downloadEndedSubscribers);
            }

            throw new HttpRequestException($"The given url {_httpClient.BaseAddress.OriginalString} returned an unsuccessfull status code: {response.StatusCode}");
        }

        public async Task Download()
        {
            _downloadStrategy = await DetermineDownloadStrategy();
            await _downloadStrategy.Download();
        }

        public void CancelDownload()
        {
            _downloadStrategy.Cancel();
        }
    }
}
