using Turbo_Download_Manager.Models;

namespace Turbo_Download_Manager.Repository
{
    public interface IFileDownloadRepository
    {
        FileDownloadEntry CreateFileDownloadEntry(FileDownloadEntry entry);
        FileDownloadEntry DeleteEntry(FileDownloadEntry entry);
        Task<List<FileDownloadEntry>> GetDownloadsBy(int pageNo = 1, int pageSize = 10);
        Task<bool> SaveChanges();
        Task<FileDownloadEntry> SearchByFileId(string fileId);
        void UpdateFileDownloadEntry(FileDownloadEntry entry);
    }
}