using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo_Download_Manager.Helpers
{
    public class DownloadJob
    {
        public Task DownloadTask { get; set; }
        public string FileName { get; set; }
        public bool IsCompeleted { get; set; } = false;
        public CancellationTokenSource JobCancellationToken { get; set; }
    }
}
