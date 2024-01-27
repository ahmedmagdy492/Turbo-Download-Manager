using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo_Download_Manager.Models
{
    public class DownloadThreadEntry
    {
        [Key]
        public int Id { get; set; }
        public long ByteToResumeFrom { get; set; }
        public long AssignedDownloadLength { get; set; }
        public string FileDownloadEntryId { get; set; }

        [ForeignKey(nameof(FileDownloadEntryId))]
        public FileDownloadEntry FileDownloadEntry { get; set; }
    }
}
