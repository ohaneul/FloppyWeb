import React from 'react';
import { render, fireEvent, screen } from '@testing-library/react';
import { SmartBar } from '../UI/components/SmartBar';

describe('SmartBar', () => {
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

        // Wait for debounce
        setTimeout(() => {
            expect(onSearch).toHaveBeenCalledWith('test');
        }, 350);
    });
}); 