interface Tab {
    id: string;
    title: string;
    url: string;
    isActive: boolean;
}

class TabManager {
    private tabs: Tab[] = [];
    private readonly maxTabs: number = 20;

    constructor() {
        this.createNewTab();
    }

    public createNewTab(): Tab {
        if (this.tabs.length >= this.maxTabs) {
            throw new Error('Maximum number of tabs reached');
        }

        const newTab: Tab = {
            id: crypto.randomUUID(),
            title: 'New Tab',
            url: 'about:blank',
            isActive: true
        };

        this.deactivateAllTabs();
        this.tabs.push(newTab);
        this.onTabsChanged();
        return newTab;
    }

    public closeTab(tabId: string): void {
        const index = this.tabs.findIndex(tab => tab.id === tabId);
        if (index !== -1) {
            this.tabs.splice(index, 1);
            if (this.tabs.length === 0) {
                this.createNewTab();
            }
            this.onTabsChanged();
        }
    }

    private deactivateAllTabs(): void {
        this.tabs.forEach(tab => tab.isActive = false);
    }

    private onTabsChanged(): void {
        // Emit event for UI update
        window.dispatchEvent(new CustomEvent('tabs-changed', {
            detail: this.tabs
        }));
    }
} 