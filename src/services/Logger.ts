type LogLevel = 'debug' | 'info' | 'warn' | 'error';

interface LogEntry {
    timestamp: string;
    level: LogLevel;
    message: string;
    data?: any;
}

export class Logger {
    private static instance: Logger;
    private logs: LogEntry[] = [];
    private maxLogs: number = 1000;

    private constructor() {
        window.addEventListener('error', (event) => {
            this.error('Uncaught error:', event.error);
        });
    }

    static getInstance(): Logger {
        if (!Logger.instance) {
            Logger.instance = new Logger();
        }
        return Logger.instance;
    }

    public debug(message: string, data?: any): void {
        this.log('debug', message, data);
    }

    public info(message: string, data?: any): void {
        this.log('info', message, data);
    }

    public warn(message: string, data?: any): void {
        this.log('warn', message, data);
    }

    public error(message: string, data?: any): void {
        this.log('error', message, data);
    }

    private log(level: LogLevel, message: string, data?: any): void {
        const entry: LogEntry = {
            timestamp: new Date().toISOString(),
            level,
            message,
            data
        };

        this.logs.push(entry);
        if (this.logs.length > this.maxLogs) {
            this.logs.shift();
        }

        // Send to console
        console[level](message, data);

        // If it's an error, we might want to report it to our error tracking service
        if (level === 'error') {
            this.reportError(entry);
        }
    }

    private reportError(entry: LogEntry): void {
        // TODO: Implement error reporting service integration
        // This could be Sentry, LogRocket, etc.
    }

    public getLogs(): LogEntry[] {
        return [...this.logs];
    }

    public clearLogs(): void {
        this.logs = [];
    }
} 