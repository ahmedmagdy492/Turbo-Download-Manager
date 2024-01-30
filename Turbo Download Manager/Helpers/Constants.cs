using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo_Download_Manager.Helpers
{
    public static class Constants
    {
        public const uint MaxSizeBeforeChunking = 1048576; // 1 MB
        public const long MaxFileSizeToDownload = 107374182400; // 100 GB
        public const long MaxFileSizeBeforeDividingToGroups = 104857600; // 100 MB
        public static readonly string FinalDownloadDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
    }
}
