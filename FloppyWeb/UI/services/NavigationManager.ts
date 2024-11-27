export class NavigationManager {
    private history: string[] = [];
    private currentIndex: number = -1;

    public navigate(url: string): void {
        // Remove any forward history when navigating to a new page
        this.history = this.history.slice(0, this.currentIndex + 1);
        this.history.push(url);
        this.currentIndex++;
    }

    public canGoBack(): boolean {
        return this.currentIndex > 0;
    }

    public canGoForward(): boolean {
        return this.currentIndex < this.history.length - 1;
    }

    public goBack(): string | null {
        if (this.canGoBack()) {
            this.currentIndex--;
            return this.history[this.currentIndex];
        }
        return null;
    }

    public goForward(): string | null {
        if (this.canGoForward()) {
            this.currentIndex++;
            return this.history[this.currentIndex];
        }
        return null;
    }
} 