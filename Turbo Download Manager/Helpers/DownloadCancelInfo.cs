﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo_Download_Manager.Helpers
{
    public class DownloadCancelInfo
    {
        public double Progress { get; set; }
        public long CurrentByte { get; set; }
        public long FileSize { get; set; }
    }
}
