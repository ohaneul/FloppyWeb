using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

public class SecurityManager
{
    private readonly CertificateValidator certificateValidator;
    private readonly ContentSecurityPolicy cspManager;
    private readonly XssProtection xssProtection;
    private readonly PhishingDetector phishingDetector;
    private readonly SandboxManager sandboxManager;

    public SecurityManager()
    {
        certificateValidator = new CertificateValidator();
        cspManager = new ContentSecurityPolicy();
        xssProtection = new XssProtection();
        phishingDetector = new PhishingDetector();
        sandboxManager = new SandboxManager();
    }

    public async Task<SecurityCheckResult> PerformSecurityCheck(string url, string content)
    {
        var tasks = new List<Task<SecurityWarning>>
        {
            certificateValidator.ValidateCertificate(url),
            phishingDetector.CheckForPhishing(url, content),
            xssProtection.ScanForXss(content),
            cspManager.ValidateContent(content)
        };

        var results = await Task.WhenAll(tasks);
        return new SecurityCheckResult(results);
    }

    public void EnforceSandbox(BrowserTab tab)
    {
        sandboxManager.SandboxTab(tab);
    }
} 