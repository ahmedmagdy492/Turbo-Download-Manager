using Turbo_Download_Manager.Helpers;

namespace Turbo_Download_Manager
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            StartupType startupType = StartupType.Normal;
            if(args.Length == 1 && args[0] == "2")
            {
                startupType = StartupType.Download;
            }
            ApplicationConfiguration.Initialize();
            
            if(startupType == StartupType.Normal)
            {
                Application.Run(new TurboMgr());
            }
            else
            {
                //Application.Run(new Downloader());
            }
        }
    }
}