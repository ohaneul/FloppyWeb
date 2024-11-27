import React from 'react';

interface ITab {
    id: string;
    title: string;
    url: string;
    favicon?: string;
}

interface ITabBarProps {
    tabs: ITab[];
    activeTabId: string;
    onTabSelect: (tabId: string) => void;
    onTabClose: (tabId: string) => void;
    onNewTab: () => void;
}

export const TabBar: React.FC<ITabBarProps> = ({
    tabs,
    activeTabId,
    onTabSelect,
    onTabClose,
    onNewTab
}) => {
    return (
        <div className="tab-bar">
            {tabs.map(tab => (
                <div 
                    key={tab.id}
                    className={`tab ${tab.id === activeTabId ? 'active' : ''}`}
                    onClick={() => onTabSelect(tab.id)}
                >
                    {tab.favicon && <img src={tab.favicon} className="tab-favicon" />}
                    <span className="tab-title">{tab.title}</span>
                    <button 
                        className="tab-close"
                        onClick={(e) => {
                            e.stopPropagation();
                            onTabClose(tab.id);
                        }}
                    >
                        ✕
                    </button>
                </div>
            ))}
            <button className="new-tab" onClick={onNewTab}>+</button>
        </div>
    );
}; 