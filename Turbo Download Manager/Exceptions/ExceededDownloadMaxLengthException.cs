using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo_Download_Manager.Exceptions
{
    public class ExceededDownloadMaxLengthException : Exception
    {
        public ExceededDownloadMaxLengthException(string message) : base(message)
        {
        }
    }
}
