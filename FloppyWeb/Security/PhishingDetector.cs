using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PhishingDetector
{
    private readonly HashSet<string> knownPhishingSites;
    private readonly PhishingPatternMatcher patternMatcher;
    private readonly UrlReputationChecker reputationChecker;

    public PhishingDetector()
    {
        knownPhishingSites = new HashSet<string>();
        patternMatcher = new PhishingPatternMatcher();
        reputationChecker = new UrlReputationChecker();
        LoadPhishingDatabase();
    }

    public async Task<SecurityWarning> CheckForPhishing(string url, string content)
    {
        if (IsKnownPhishingSite(url))
        {
            return new SecurityWarning("Known Phishing Site");
        }

        var tasks = new[]
        {
            patternMatcher.AnalyzeContent(content),
            reputationChecker.CheckReputation(url)
        };

        var results = await Task.WhenAll(tasks);
        return results.Any(r => r.IsPhishing)
            ? new SecurityWarning("Suspected Phishing Site")
            : null;
    }
} 