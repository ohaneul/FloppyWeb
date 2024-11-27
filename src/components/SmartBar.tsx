import React, { Component, ChangeEvent } from 'react';
import { Debouncer } from "../utils/Debouncer";

interface ISmartBarProps {
    onSearch: (query: string) => Promise<void>;
    onNavigate: (url: string) => Promise<void>;
    suggestions: string[];
}

interface ISmartBarState {
    value: string;
    showSuggestions: boolean;
    selectedSuggestionIndex: number;
}

export class SmartBar extends Component<ISmartBarProps, ISmartBarState> {
    private searchDebouncer: Debouncer;

    constructor(props: ISmartBarProps) {
        super(props);
        this.state = {
            value: '',
            showSuggestions: false,
            selectedSuggestionIndex: -1
        };
        this.searchDebouncer = new Debouncer(300);
    }

    private handleInput = (event: ChangeEvent<HTMLInputElement>): void => {
        const value = event.target.value;
        this.setState({ value, showSuggestions: true });
    };

    public render(): JSX.Element {
        const { value } = this.state;

        return (
            <input 
                type="text"
                value={value}
                placeholder="Search or enter URL"
                onChange={this.handleInput}
                className="smart-bar__input"
            />
        );
    }
} 