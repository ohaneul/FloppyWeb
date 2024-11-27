using System;
using System.Collections.Generic;
using System.IO;

public class DownloadManager
{
    private readonly Queue<DownloadItem> downloadQueue;
    private readonly string downloadPath;

    public event EventHandler<DownloadProgressEventArgs> DownloadProgress;
    public event EventHandler<DownloadCompletedEventArgs> DownloadCompleted;

    public DownloadManager()
    {
        downloadQueue = new Queue<DownloadItem>();
        downloadPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads"
        );
    }
} 