import React from 'react';
import { BrowserWindow } from './UI/components/BrowserWindow';
import { SettingsManager } from './UI/services/SettingsManager';

export const App: React.FC = () => {
    const [settingsManager] = React.useState(() => new SettingsManager());

    React.useEffect(() => {
        // Apply theme from settings
        document.documentElement.setAttribute(
            'data-theme',
            settingsManager.getSetting('theme')
        );
    }, []);

    return (
        <div className="app-container">
            <BrowserWindow />
        </div>
    );
}; 