import React, { Component } from 'react';

interface IWebViewProps {
    url: string;
    onLoadStart?: () => void;
    onLoadEnd?: () => void;
    onError?: (error: Error) => void;
}

interface IWebViewState {
    loading: boolean;
    error: Error | null;
}

export class WebView extends Component<IWebViewProps, IWebViewState> {
    private iframeRef: React.RefObject<HTMLIFrameElement>;

    constructor(props: IWebViewProps) {
        super(props);
        this.state = {
            loading: true,
            error: null
        };
        this.iframeRef = React.createRef();
    }

    componentDidMount() {
        this.loadUrl(this.props.url);
    }

    componentDidUpdate(prevProps: IWebViewProps) {
        if (prevProps.url !== this.props.url) {
            this.loadUrl(this.props.url);
        }
    }

    private loadUrl(url: string) {
        this.setState({ loading: true, error: null });
        this.props.onLoadStart?.();

        try {
            if (this.iframeRef.current) {
                this.iframeRef.current.src = url;
            }
        } catch (error) {
            this.setState({ error: error as Error });
            this.props.onError?.(error as Error);
        }
    }

    private handleLoad = () => {
        this.setState({ loading: false });
        this.props.onLoadEnd?.();
    };

    render() {
        return (
            <div className="webview-container">
                {this.state.loading && (
                    <div className="webview-loading">Loading...</div>
                )}
                {this.state.error && (
                    <div className="webview-error">
                        Error: {this.state.error.message}
                    </div>
                )}
                <iframe
                    ref={this.iframeRef}
                    className="webview-iframe"
                    onLoad={this.handleLoad}
                    sandbox="allow-same-origin allow-scripts allow-forms"
                    title="web-content"
                />
            </div>
        );
    }
} 