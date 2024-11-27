using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class DownloadManager
{
    private readonly Dictionary<string, DownloadTask> activeDownloads;
    private readonly Queue<DownloadTask> downloadQueue;
    private readonly int maxConcurrentDownloads = 3;

    public event EventHandler<DownloadProgressEventArgs> DownloadProgress;
    public event EventHandler<DownloadCompletedEventArgs> DownloadCompleted;
    public event EventHandler<DownloadFailedEventArgs> DownloadFailed;

    public async Task StartDownload(string url)
    {
        var downloadTask = new DownloadTask(url);
        
        if (activeDownloads.Count >= maxConcurrentDownloads)
        {
            downloadQueue.Enqueue(downloadTask);
            return;
        }

        await StartDownloadTask(downloadTask);
    }

    private async Task ResumeDownload(DownloadTask task)
    {
        var existingFile = new FileInfo(task.FilePath);
        if (existingFile.Exists)
        {
            task.BytesDownloaded = existingFile.Length;
            task.Request.Headers.Range = new RangeHeaderValue(task.BytesDownloaded, null);
        }
        
        await StartDownloadTask(task);
    }

    private void UpdateProgress(DownloadTask task)
    {
        var progress = new DownloadProgressEventArgs
        {
            DownloadId = task.Id,
            BytesReceived = task.BytesDownloaded,
            TotalBytes = task.TotalBytes,
            Speed = CalculateSpeed(task),
            EstimatedTimeRemaining = CalculateETA(task)
        };

        DownloadProgress?.Invoke(this, progress);
    }
} 