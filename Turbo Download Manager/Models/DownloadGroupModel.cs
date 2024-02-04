using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo_Download_Manager.Models
{
    public class DownloadGroupModel
    {
        [Key]
        public string GroupID { get; set; } = Guid.NewGuid().ToString("N");
        public long FileSize { get; set; }
        public long StartByte { get; set; }
        public long CurrentByte { get; set; }
        public bool IsCompleted { get; set; }
        public string FileId { get; set; }

        [ForeignKey(nameof(FileId))]
        public FileDownloadEntry FileDownloadEntry { get; set; }
    }
}
