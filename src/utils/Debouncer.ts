export class Debouncer {
    private timeout: number | null = null;
    
    constructor(private delay: number) {}

    public debounce(callback: () => void): void {
        if (this.timeout) {
            window.clearTimeout(this.timeout);
        }
        
        this.timeout = window.setTimeout(() => {
            callback();
            this.timeout = null;
        }, this.delay);
    }

    public cancel(): void {
        if (this.timeout) {
            window.clearTimeout(this.timeout);
            this.timeout = null;
        }
    }
} 