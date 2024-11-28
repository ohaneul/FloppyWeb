using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ContentSecurityPolicy
{
    private readonly Dictionary<string, CspDirective> policies;

    public ContentSecurityPolicy()
    {
        policies = new Dictionary<string, CspDirective>
        {
            { "default-src", new CspDirective("'self'") },
            { "script-src", new CspDirective("'self' 'unsafe-inline' 'unsafe-eval'") },
            { "style-src", new CspDirective("'self' 'unsafe-inline'") },
            { "img-src", new CspDirective("'self' data: https:") },
            { "connect-src", new CspDirective("'self'") },
            { "font-src", new CspDirective("'self'") },
            { "object-src", new CspDirective("'none'") },
            { "media-src", new CspDirective("'self'") },
            { "frame-src", new CspDirective("'self'") }
        };
    }

    public async Task<SecurityWarning> ValidateContent(string content)
    {
        var violations = new List<CspViolation>();
        foreach (var policy in policies)
        {
            var directive = policy.Value;
            if (directive.IsViolated(content))
            {
                violations.Add(new CspViolation(policy.Key));
            }
        }

        return violations.Any() 
            ? new SecurityWarning("CSP Violations Detected", violations) 
            : null;
    }
} 