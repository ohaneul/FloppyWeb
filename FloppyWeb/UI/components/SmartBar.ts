import { Debouncer } from "../utils/Debouncer";

interface ISmartBarProps {
    onSearch: (query: string) => void;
    onNavigate: (url: string) => void;
    suggestions: string[];
}

class SmartBar extends Component<ISmartBarProps, ISmartBarState> {
    private searchDebouncer: Debouncer;
    private urlParser: URLParser;

    constructor(props: ISmartBarProps) {
        super(props);
        this.state = {
            value: '',
            showSuggestions: false,
            selectedSuggestionIndex: -1
        };
        this.searchDebouncer = new Debouncer(300);
        this.urlParser = new URLParser();
    }

    private handleInput = (event: ChangeEvent<HTMLInputElement>): void => {
        const value = event.target.value;
        this.setState({ value, showSuggestions: true });

        this.searchDebouncer.debounce(() => {
            if (this.urlParser.isValidURL(value)) {
                this.props.onNavigate(value);
            } else {
                this.props.onSearch(value);
            }
        });
    };

    private handleKeyPress = (event: KeyboardEvent<HTMLInputElement>): void => {
        const { suggestions } = this.props;
        const { selectedSuggestionIndex } = this.state;

        switch (event.key) {
            case 'Enter':
                if (selectedSuggestionIndex >= 0) {
                    this.handleSuggestionSelect(suggestions[selectedSuggestionIndex]);
                } else {
                    this.handleSubmit();
                }
                break;
            case 'ArrowDown':
                event.preventDefault();
                this.setState(prevState => ({
                    selectedSuggestionIndex: Math.min(
                        prevState.selectedSuggestionIndex + 1,
                        suggestions.length - 1
                    )
                }));
                break;
            case 'ArrowUp':
                event.preventDefault();
                this.setState(prevState => ({
                    selectedSuggestionIndex: Math.max(
                        prevState.selectedSuggestionIndex - 1,
                        -1
                    )
                }));
                break;
            case 'Escape':
                this.setState({
                    showSuggestions: false,
                    selectedSuggestionIndex: -1
                });
                break;
        }
    };

    public render(): JSX.Element {
        const { suggestions } = this.props;
        const { value, showSuggestions, selectedSuggestionIndex } = this.state;

        return (
            <div className="smart-bar">
                <input 
                    type="text"
                    value={value}
                    placeholder="Search or enter URL"
                    onChange={this.handleInput}
                    onKeyDown={this.handleKeyPress}
                    onBlur={this.handleBlur}
                    className="smart-bar__input"
                    autoComplete="off"
                />
                {showSuggestions && suggestions.length > 0 && (
                    <SuggestionsList 
                        suggestions={suggestions}
                        selectedIndex={selectedSuggestionIndex}
                        onSelect={this.handleSuggestionSelect}
                    />
                )}
            </div>
        );
    }
}