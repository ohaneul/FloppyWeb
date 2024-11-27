import React from 'react';

interface ISuggestionsListProps {
    suggestions: string[];
    selectedIndex: number;
    onSelect: (suggestion: string) => void;
}

export const SuggestionsList: React.FC<ISuggestionsListProps> = ({
    suggestions,
    selectedIndex,
    onSelect
}) => {
    return (
        <ul className="suggestions-list">
            {suggestions.map((suggestion, index) => (
                <li
                    key={suggestion}
                    className={`suggestion-item ${index === selectedIndex ? 'selected' : ''}`}
                    onClick={() => onSelect(suggestion)}
                >
                    {suggestion}
                </li>
            ))}
        </ul>
    );
}; 