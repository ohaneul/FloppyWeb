namespace FloppyWeb.Network
{
    public class NetworkSpeed
    {
        public long DownloadSpeedBps { get; }
        public long UploadSpeedBps { get; }

        public NetworkSpeed(long downloadSpeed, long uploadSpeed)
        {
            DownloadSpeedBps = downloadSpeed;
            UploadSpeedBps = uploadSpeed;
        }

        public string GetFormattedDownloadSpeed()
        {
            return FormatSpeed(DownloadSpeedBps);
        }

        public string GetFormattedUploadSpeed()
        {
            return FormatSpeed(UploadSpeedBps);
        }

        private string FormatSpeed(long bps)
        {
            string[] sizes = { "B/s", "KB/s", "MB/s", "GB/s" };
            double speed = bps;
            int order = 0;

            while (speed >= 1024 && order < sizes.Length - 1)
            {
                speed /= 1024;
                order++;
            }

            return $"{speed:0.##} {sizes[order]}";
        }
    }
} 