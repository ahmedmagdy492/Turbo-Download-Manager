using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo_Download_Manager.Models
{
    public class FileDownloadEntry
    {
        [Key]
        public string FileId { get; set; } = Guid.NewGuid().ToString("N");
        public string FileName { get; set; }
        public string DownloadUrl { get; set; }
        public string SavePath { get; set; }
        public bool HasCompleted { get; set; } = false;
        public string ProgressPercent { get; set; }
        public DateTime StartDownloadDateTime { get; set; } = DateTime.Now;

        public List<DownloadThreadEntry> DownloadThreadEntrys { get; set; } = new List<DownloadThreadEntry>();
    }
}
