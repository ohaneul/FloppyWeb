import React, { useState, useEffect } from 'react';
import { Logger } from '../services/Logger';
import { PerformanceMonitor } from '../services/PerformanceMonitor';

export const DebugPanel: React.FC = () => {
    const [isVisible, setIsVisible] = useState(false);
    const [logs, setLogs] = useState<any[]>([]);
    const [fps, setFps] = useState(0);
    
    useEffect(() => {
        const interval = setInterval(() => {
            setLogs(Logger.getInstance().getLogs());
            setFps(PerformanceMonitor.getInstance().getAverageFPS());
        }, 1000);

        return () => clearInterval(interval);
    }, []);

    if (!isVisible) {
        return (
            <button 
                className="debug-toggle"
                onClick={() => setIsVisible(true)}
            >
                Show Debug
            </button>
        );
    }

    return (
        <div className="debug-panel">
            <button 
                className="debug-close"
                onClick={() => setIsVisible(false)}
            >
                Close
            </button>
            <div className="debug-metrics">
                <h3>Performance</h3>
                <p>FPS: {fps}</p>
            </div>
            <div className="debug-logs">
                <h3>Logs</h3>
                {logs.map((log, index) => (
                    <div key={index} className={`log-entry log-${log.level}`}>
                        <span className="log-timestamp">{log.timestamp}</span>
                        <span className="log-level">{log.level}</span>
                        <span className="log-message">{log.message}</span>
                    </div>
                ))}
            </div>
        </div>
    );
}; 