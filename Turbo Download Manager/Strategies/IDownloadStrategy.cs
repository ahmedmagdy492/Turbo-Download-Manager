using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo_Download_Manager.Strategies
{
    public interface IDownloadStrategy
    {
        Task Download();
    }
}
