import React from 'react';
import { render, fireEvent, screen } from '@testing-library/react';
import { SmartBar } from '../components/SmartBar';

describe('SmartBar', () => {
    it('should render without crashing', () => {
        render(
            <SmartBar 
                onSearch={async () => {}}
                onNavigate={async () => {}}
                suggestions={[]}
            />
        );
    });

    it('should handle user input', () => {
        const onSearch = jest.fn();
        const onNavigate = jest.fn();
        
        render(
            <SmartBar 
                onSearch={onSearch}
                onNavigate={onNavigate}
                suggestions={[]}
            />
        );

        const input = screen.getByPlaceholderText('Search or enter URL');
        fireEvent.change(input, { target: { value: 'test' } });
    });
}); 