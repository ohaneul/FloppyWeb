import { Component } from 'react';
import { Logger } from '../services/Logger';
import { PerformanceMonitor } from '../services/PerformanceMonitor';
import { DebugPanel } from './DebugPanel';

export class BrowserWindow extends Component<{}, IBrowserWindowState> {
    private logger = Logger.getInstance();
    private performanceMonitor = PerformanceMonitor.getInstance();

    // ... existing code ...

    private handleNavigate = async (url: string): Promise<void> => {
        this.setState({ isLoading: true });
        this.logger.info(`Navigating to: ${url}`);
        
        try {
            this.navigationManager.navigate(url);
            this.setState({ currentUrl: url });
            this.performanceMonitor.logNavigationTiming(url);
        } catch (error) {
            this.logger.error('Navigation failed', error);
        } finally {
            this.setState({ isLoading: false });
        }
    };

    public render(): JSX.Element {
        return (
            <div className="browser-window">
                {/* ... existing JSX ... */}
                {process.env.NODE_ENV === 'development' && <DebugPanel />}
            </div>
        );
    }
} 