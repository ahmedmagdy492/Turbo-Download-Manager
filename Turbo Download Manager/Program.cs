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
            string downloadUri = string.Empty;

            if(args.Length == 2 && args[0] == "2")
            {
                startupType = StartupType.Download;
                downloadUri = args[1];
                if(string.IsNullOrWhiteSpace(downloadUri))
                {
                    Environment.Exit(1);
                }
            }

            ApplicationConfiguration.Initialize();
            
            if(startupType == StartupType.Normal)
            {
                Application.Run(new TurboMgr());
            }
            else
            {
                try
                {
                    Application.Run(new Downloader(new Uri(downloadUri), null));
                }
                catch (Exception)
                {
                    Environment.Exit(2);
                }
            }
        }
    }
}