using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo_Download_Manager.Factories;
using Turbo_Download_Manager.Helpers;
using Turbo_Download_Manager.Protocols;
using Turbo_Download_Manager.Strategies;

namespace Turbo_Download_Manager
{
    public class DownloadManager
    {
        private readonly List<Action<DownloadProgressInfo>> _progressUpdateSubscribers;
        private readonly List<Action> _downloadEndedSubscribers;
        private readonly List<Action<DownloadCancelInfo>> _onDownloadCancelSubscribers;
        private static DownloadManager _instance;
        private IDownloadProtocol _downloadProtocol;

        private DownloadManager()
        {
            _progressUpdateSubscribers = new List<Action<DownloadProgressInfo>>();
            _downloadEndedSubscribers = new List<Action>();
            _onDownloadCancelSubscribers = new List<Action<DownloadCancelInfo>>();
        }

        public static DownloadManager CreateDownloadManager()
        {
            if (_instance == null)
                _instance = new DownloadManager();

            return _instance;
        }

        public void SubscribeToProgressUpdate(Action<DownloadProgressInfo> subscriber)
        {
            _progressUpdateSubscribers.Add(subscriber);
        }

        public void SubscribeToDownloadEnded(Action subscriber)
        {
            _downloadEndedSubscribers.Add(subscriber);
        }

        public void SubscribeToDownloadCancel(Action<DownloadCancelInfo> subscriber)
        {
            _onDownloadCancelSubscribers.Add(subscriber);
        }

        public void CreateNewDownload(string url)
        {
            _downloadProtocol = DownloadProtocolFactory.CreateDownloadProtocol(url, _progressUpdateSubscribers, _downloadEndedSubscribers, _onDownloadCancelSubscribers);
        }

        public void CancelDownload()
        {
            _downloadProtocol.CancelDownload();
        }

        public Task StartDownload()
        {
            return _downloadProtocol.Download();
        }
    }
}
