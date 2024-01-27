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
            return await _context.Items.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public FileDownloadEntry CreateFileDownloadEntry(FileDownloadEntry entry)
        {
            _context.Items.Add(entry);
            return entry;
        }

        public async Task<List<FileDownloadEntry>> SearchByName(string name)
        {
            return await _context.Items.Where(f => f.FileName.Contains(name)).ToListAsync();
        }

        public FileDownloadEntry DeleteEntry(FileDownloadEntry entry)
        {
            _context.Items.Remove(entry);
            return entry;
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
