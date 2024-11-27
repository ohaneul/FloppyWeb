interface PerformanceMetrics {
    navigationStart: number;
    loadComplete: number;
    memoryUsage?: number;
    fps: number;
}

export class PerformanceMonitor {
    private static instance: PerformanceMonitor;
    private metrics: PerformanceMetrics[] = [];
    private fpsHistory: number[] = [];
    private lastFrameTimestamp: number = 0;
    private logger = Logger.getInstance();

    private constructor() {
        this.startMonitoring();
    }

    static getInstance(): PerformanceMonitor {
        if (!PerformanceMonitor.instance) {
            PerformanceMonitor.instance = new PerformanceMonitor();
        }
        return PerformanceMonitor.instance;
    }

    private startMonitoring(): void {
        // Monitor FPS
        const measureFPS = (timestamp: number) => {
            if (this.lastFrameTimestamp) {
                const fps = 1000 / (timestamp - this.lastFrameTimestamp);
                this.fpsHistory.push(fps);
                if (this.fpsHistory.length > 60) {
                    this.fpsHistory.shift();
                }
            }
            this.lastFrameTimestamp = timestamp;
            requestAnimationFrame(measureFPS);
        };
        requestAnimationFrame(measureFPS);

        // Monitor memory usage if available
        if (performance.memory) {
            setInterval(() => {
                this.logMemoryUsage();
            }, 5000);
        }
    }

    public logNavigationTiming(url: string): void {
        const timing = performance.getEntriesByType('navigation')[0] as PerformanceNavigationTiming;
        if (timing) {
            this.logger.info('Navigation Timing', {
                url,
                dnsLookup: timing.domainLookupEnd - timing.domainLookupStart,
                tcpConnection: timing.connectEnd - timing.connectStart,
                serverResponse: timing.responseEnd - timing.requestStart,
                domComplete: timing.domComplete - timing.responseEnd,
                loadEvent: timing.loadEventEnd - timing.loadEventStart,
                totalTime: timing.loadEventEnd - timing.navigationStart
            });
        }
    }

    private logMemoryUsage(): void {
        if (performance.memory) {
            const memory = performance.memory;
            this.logger.debug('Memory Usage', {
                usedJSHeapSize: memory.usedJSHeapSize / 1048576 + ' MB',
                totalJSHeapSize: memory.totalJSHeapSize / 1048576 + ' MB'
            });
        }
    }

    public getAverageFPS(): number {
        if (this.fpsHistory.length === 0) return 0;
        const sum = this.fpsHistory.reduce((a, b) => a + b, 0);
        return Math.round(sum / this.fpsHistory.length);
    }
} 