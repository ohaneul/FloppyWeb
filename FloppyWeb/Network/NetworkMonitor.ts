interface NetworkStats {
    bytesReceived: number;
    bytesSent: number;
    requestCount: number;
    activeConnections: number;
    failedRequests: number;
    avgResponseTime: number;
}

class NetworkMonitor {
    private stats: NetworkStats;
    private listeners: Set<(stats: NetworkStats) => void>;
    private readonly updateInterval: number = 1000;

    constructor() {
        this.stats = this.initializeStats();
        this.listeners = new Set();
        this.startMonitoring();
    }

    public addListener(callback: (stats: NetworkStats) => void): void {
        this.listeners.add(callback);
    }

    public removeListener(callback: (stats: NetworkStats) => void): void {
        this.listeners.delete(callback);
    }

    private startMonitoring(): void {
        setInterval(() => {
            this.updateStats();
            this.notifyListeners();
        }, this.updateInterval);
    }

    private updateStats(): void {
        // Update network statistics using Performance API
        const entries = performance.getEntriesByType('resource');
        this.stats.requestCount = entries.length;
        this.stats.bytesReceived = entries.reduce(
            (total, entry) => total + (entry as PerformanceResourceTiming).transferSize, 
            0
        );
        this.stats.avgResponseTime = entries.reduce(
            (total, entry) => total + entry.duration, 
            0
        ) / entries.length;
    }
} 