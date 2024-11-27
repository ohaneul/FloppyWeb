import React, { Component } from 'react';
import { SmartBar } from './SmartBar';
import { NavigationManager } from '../services/NavigationManager';
import { WebView } from './WebView';

interface IBrowserWindowState {
    currentUrl: string;
    isLoading: boolean;
    suggestions: string[];
}

export class BrowserWindow extends Component<{}, IBrowserWindowState> {
    private navigationManager: NavigationManager;

    constructor(props: {}) {
        super(props);
        this.state = {
            currentUrl: '',
            isLoading: false,
            suggestions: []
        };
        this.navigationManager = new NavigationManager();
    }

    private handleSearch = async (query: string): Promise<void> => {
        // Implement search functionality
        const searchUrl = `https://www.google.com/search?q=${encodeURIComponent(query)}`;
        await this.handleNavigate(searchUrl);
    };

    private handleNavigate = async (url: string): Promise<void> => {
        this.setState({ isLoading: true });
        try {
            // Implement actual navigation logic here
            this.navigationManager.navigate(url);
            this.setState({ currentUrl: url });
        } finally {
            this.setState({ isLoading: false });
        }
    };

    public render(): JSX.Element {
        return (
            <div className="browser-window">
                <div className="browser-toolbar">
                    <SmartBar
                        onSearch={this.handleSearch}
                        onNavigate={this.handleNavigate}
                        suggestions={this.state.suggestions}
                    />
                </div>
                <WebView
                    url={this.state.currentUrl}
                    onLoadStart={() => this.setState({ isLoading: true })}
                    onLoadEnd={() => this.setState({ isLoading: false })}
                    onError={(error) => {
                        console.error('Navigation error:', error);
                        this.setState({ isLoading: false });
                    }}
                />
            </div>
        );
    }
} 