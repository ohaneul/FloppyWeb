class WebpageHandler {
    constructor() {
        this.observers = new Map();
    }

    injectCustomScript(code) {
        try {
            return eval(code);
        } catch (error) {
            console.error('Script injection failed:', error);
            return null;
        }
    }

    observeDOM(selector, callback) {
        const observer = new MutationObserver((mutations) => {
            mutations.forEach((mutation) => {
                if (mutation.type === 'childList') {
                    const elements = document.querySelectorAll(selector);
                    elements.forEach(callback);
                }
            });
        });

        this.observers.set(selector, observer);
        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    }

    // Custom methods for webpage manipulation
    blockAds() {
        const adSelectors = [
            '.advertisement',
            '[class*="ad-"]',
            '[id*="ad-"]'
        ];
        
        adSelectors.forEach(selector => {
            document.querySelectorAll(selector).forEach(el => el.remove());
        });
    }
} 