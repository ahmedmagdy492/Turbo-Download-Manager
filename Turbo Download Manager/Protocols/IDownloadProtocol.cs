using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo_Download_Manager.Protocols
{
    public interface IDownloadProtocol
    {
        Task Download();
    }
}
