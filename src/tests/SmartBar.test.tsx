import React from 'react';
import { render, screen } from '@testing-library/react';
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
        
        expect(screen.getByPlaceholderText('Search or enter URL')).toBeInTheDocument();
    });
}); 