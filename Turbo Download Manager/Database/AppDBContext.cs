using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo_Download_Manager.Models;

namespace Turbo_Download_Manager.Database
{
    public class AppDBContext : DbContext
    {
        public AppDBContext() { }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public virtual DbSet<FileDownloadEntry> FileDownloadEntries { get; set; }

        public virtual DbSet<DownloadGroupModel> DownloadGroupsModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=.\\Database\\downloads.db");
    }
}
