import React from 'react';
import { SmartBar } from './components/SmartBar';

export const App: React.FC = () => {
    const handleSearch = async (query: string) => {
        console.log('Searching:', query);
    };

    const handleNavigate = async (url: string) => {
        console.log('Navigating to:', url);
    };

    return (
        <div className="app">
            <SmartBar
                onSearch={handleSearch}
                onNavigate={handleNavigate}
                suggestions={[]}
            />
        </div>
    );
}; 