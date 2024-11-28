interface MediaConfig {
    width: number;
    height: number;
    quality: number;
    enableFilters: boolean;
}

export class MediaManager {
    private processor: MediaProcessor;
    private videoStream: MediaStream | null = null;
    private canvas: HTMLCanvasElement;
    private context: CanvasRenderingContext2D;

    constructor(config: MediaConfig) {
        this.processor = new MediaProcessor(config.width, config.height);
        this.setupCanvas(config);
    }

    public async startVideoCapture(): Promise<void> {
        try {
            this.videoStream = await navigator.mediaDevices.getUserMedia({
                video: {
                    width: { ideal: this.canvas.width },
                    height: { ideal: this.canvas.height }
                }
            });
            
            this.startProcessing();
        } catch (error) {
            console.error('Failed to start video capture:', error);
            throw error;
        }
    }

    public async takeScreenshot(): Promise<Blob> {
        const imageData = this.context.getImageData(
            0, 0, 
            this.canvas.width, 
            this.canvas.height
        );
        
        const processed = await this.processor.process_frame(imageData.data);
        return new Blob([processed], { type: 'image/png' });
    }

    private startProcessing(): void {
        if (!this.videoStream) return;

        const video = document.createElement('video');
        video.srcObject = this.videoStream;
        video.play();

        const processFrame = async () => {
            this.context.drawImage(video, 0, 0);
            const imageData = this.context.getImageData(
                0, 0, 
                this.canvas.width, 
                this.canvas.height
            );

            const processed = await this.processor.process_frame(imageData.data);
            const newImageData = new ImageData(
                new Uint8ClampedArray(processed),
                this.canvas.width,
                this.canvas.height
            );

            this.context.putImageData(newImageData, 0, 0);
            requestAnimationFrame(processFrame);
        };

        requestAnimationFrame(processFrame);
    }
} 