namespace Turbo_Download_Manager.Helpers
{
    public class DownloadGroup
    {
        public long FileSize { get; set; }
        public long StartByte { get; set; }
        public long CurrentByte { get; set; }
        public bool IsCompleted { get; set; }
        public CancellationTokenSource CancellationToken { get; set; }
        public Task BackgroundTask { get; set; }
        public string TempFilePath { get; set; }
        public Action OnGroupDone { get; set; }
    }
}
