using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo_Download_Manager.Database;

namespace Turbo_Download_Manager.Repository
{
    public class UnitOfWork
    {
        private static readonly AppDBContext _appDBContext = new AppDBContext();

        public IFileDownloadRepository FileDownloadRepository { 
            get {
                return new FileDownloadRepository(_appDBContext);
            } 
        }
    }
}
