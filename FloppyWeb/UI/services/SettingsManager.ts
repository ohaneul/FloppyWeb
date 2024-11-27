interface IBrowserSettings {
    searchEngine: string;
    startPage: string;
    enableJavaScript: boolean;
    enableCookies: boolean;
    theme: 'light' | 'dark' | 'system';
}

export class SettingsManager {
    private static readonly SETTINGS_KEY = 'floppy_web_settings';
    private settings: IBrowserSettings;

    constructor() {
        this.settings = this.loadSettings();
    }

    private loadSettings(): IBrowserSettings {
        const defaultSettings: IBrowserSettings = {
            searchEngine: 'https://www.google.com/search?q=',
            startPage: 'about:blank',
            enableJavaScript: true,
            enableCookies: true,
            theme: 'system'
        };

        try {
            const stored = localStorage.getItem(SettingsManager.SETTINGS_KEY);
            return stored ? { ...defaultSettings, ...JSON.parse(stored) } : defaultSettings;
        } catch {
            return defaultSettings;
        }
    }

    public getSetting<K extends keyof IBrowserSettings>(key: K): IBrowserSettings[K] {
        return this.settings[key];
    }

    public setSetting<K extends keyof IBrowserSettings>(key: K, value: IBrowserSettings[K]): void {
        this.settings[key] = value;
        this.saveSettings();
    }

    private saveSettings(): void {
        localStorage.setItem(SettingsManager.SETTINGS_KEY, JSON.stringify(this.settings));
    }
} 