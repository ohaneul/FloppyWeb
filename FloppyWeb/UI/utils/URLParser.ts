export class URLParser {
    public isValidURL(url: string): boolean {
        try {
            new URL(url);
            return true;
        } catch {
            // Try adding https:// if no protocol is specified
            try {
                new URL(`https://${url}`);
                return true;
            } catch {
                return false;
            }
        }
    }

    public normalizeURL(url: string): string {
        try {
            return new URL(url).toString();
        } catch {
            return new URL(`https://${url}`).toString();
        }
    }
} 