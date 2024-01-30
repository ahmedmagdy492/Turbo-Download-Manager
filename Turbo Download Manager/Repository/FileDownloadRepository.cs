using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo_Download_Manager.Database;
using Turbo_Download_Manager.Models;

namespace Turbo_Download_Manager.Repository
{
    public class FileDownloadRepository : IFileDownloadRepository
    {
        private readonly AppDBContext _context;

        public FileDownloadRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<FileDownloadEntry>> GetDownloadsBy(int pageNo = 1, int pageSize = 10)
        {
            return await _context.FileDownloadEntries.Skip((pageNo - 1) * pageSize).Take(pageSize).OrderByDescending(f => f.StartDownloadDateTime).ToListAsync();
        }

        public FileDownloadEntry CreateFileDownloadEntry(FileDownloadEntry entry)
        {
            _context.FileDownloadEntries.Add(entry);
            return entry;
        }

        public void UpdateFileDownloadEntry(FileDownloadEntry entry)
        {
            _context.Entry(entry).State = EntityState.Modified;
        }

        public async Task<FileDownloadEntry> SearchByFileId(string fileId)
        {
            return await _context.FileDownloadEntries.FirstOrDefaultAsync(f => f.FileId == fileId);
        }

        public FileDownloadEntry DeleteEntry(FileDownloadEntry entry)
        {
            _context.FileDownloadEntries.Remove(entry);
            return entry;
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
