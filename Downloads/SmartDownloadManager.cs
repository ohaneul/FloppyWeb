public class SmartDownloadManager
{
    private readonly VirusScanner virusScanner;
    private readonly DownloadOptimizer optimizer;
    private readonly ResumeManager resumeManager;

    // Finally, a download manager that doesn't suck!
    public async Task StartSmartDownload(string url)
    {
        // Check if it's safe
        if (!await virusScanner.PreScanUrl(url))
        {
            NotifyUser("Whoa there! This file looks suspicious! 🚫");
            return;
        }

        // Try to find fastest mirror
        var bestSource = await optimizer.FindBestSource(url);
        
        // Use multiple connections for speed
        var segments = optimizer.CalculateOptimalSegments(bestSource.FileSize);
        
        // Start the download with resume support
        await resumeManager.StartSegmentedDownload(bestSource, segments);
    }

    // Because downloads shouldn't take forever
    private async Task<List<DownloadSegment>> SplitIntoSegments(long fileSize)
    {
        var segments = new List<DownloadSegment>();
        var optimalSegmentSize = CalculateOptimalSegmentSize(fileSize);
        
        for (long start = 0; start < fileSize; start += optimalSegmentSize)
        {
            segments.Add(new DownloadSegment(start, 
                Math.Min(start + optimalSegmentSize, fileSize)));
        }
        
        return segments;
    }
} 