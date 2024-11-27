import React from 'react';

interface INavigationBarProps {
    canGoBack: boolean;
    canGoForward: boolean;
    isLoading: boolean;
    onBack: () => void;
    onForward: () => void;
    onRefresh: () => void;
    onStop: () => void;
}

export const NavigationBar: React.FC<INavigationBarProps> = ({
    canGoBack,
    canGoForward,
    isLoading,
    onBack,
    onForward,
    onRefresh,
    onStop
}) => {
    return (
        <div className="navigation-bar">
            <button 
                onClick={onBack} 
                disabled={!canGoBack}
                className="nav-button"
            >
                ←
            </button>
            <button 
                onClick={onForward} 
                disabled={!canGoForward}
                className="nav-button"
            >
                →
            </button>
            <button 
                onClick={isLoading ? onStop : onRefresh}
                className="nav-button"
            >
                {isLoading ? '✕' : '↻'}
            </button>
        </div>
    );
}; 