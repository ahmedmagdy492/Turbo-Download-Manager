using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo_Download_Manager.Exceptions;
using Turbo_Download_Manager.Helpers;
using Turbo_Download_Manager.Protocols;
using Turbo_Download_Manager.Strategies;

namespace Turbo_Download_Manager.Factories
{
    public static class DownloadProtocolFactory
    {
        private static string[] supportedProtocols = ["http", "https"];

        public static Protocols.IDownloadProtocol CreateDownloadProtocol(string url, List<Action<DownloadProgressInfo>> progressUpdateSubscribers, List<Action> downloadEndedSubscribers)
        {
            string protocol = new Uri(url).Scheme;

            if (protocol.StartsWith("http"))
            {
                // http or https
                return new HttpDownloadProtocol(url, progressUpdateSubscribers, downloadEndedSubscribers);
            }

            throw new UnSupportedProtocolException($"The protocol given in the url {url} is not supported");
        }
    }
}
