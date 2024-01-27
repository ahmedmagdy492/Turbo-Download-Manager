using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo_Download_Manager.Helpers
{
    public struct DownloadMetaData
    {
        public long startByte, chunkLength;
        public string fileName;
        public DownloadJob job;
    };
}
