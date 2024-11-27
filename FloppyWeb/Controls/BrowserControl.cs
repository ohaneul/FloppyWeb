using System;
using System.Windows.Forms;

namespace FloppyWeb.Controls
{
    public class BrowserControl : UserControl
    {
        private readonly WebBrowser webBrowser;
        private readonly ScriptManager scriptManager;
        private readonly HistoryManager historyManager;
        private readonly SecurityManager securityManager;

        public event EventHandler<NavigationEventArgs> BeforeNavigate;
        public event EventHandler<NavigationCompletedEventArgs> NavigationCompleted;
        public event EventHandler<ScriptErrorEventArgs> ScriptError;

        public BrowserControl()
        {
            webBrowser = new WebBrowser
            {
                Dock = DockStyle.Fill,
                ScriptErrorsSuppressed = true
            };

            this.Controls.Add(webBrowser);
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
        }

        public void Navigate(string url)
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "https://" + url;
            }
            webBrowser.Navigate(url);
        }

        public void GoBack()
        {
            if (webBrowser.CanGoBack)
                webBrowser.GoBack();
        }

        public void GoForward()
        {
            if (webBrowser.CanGoForward)
                webBrowser.GoForward();
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            NavigationCompleted?.Invoke(this, webBrowser.Url.ToString());
        }

        private void HandleScriptError(object sender, ScriptErrorEventArgs e)
        {
            if (settingsManager.IgnoreScriptErrors)
            {
                e.Handled = true;
                return;
            }

            if (e.IsRecurring)
            {
                scriptManager.AddToBlocklist(e.Url);
                e.Handled = true;
                return;
            }

            ScriptError?.Invoke(this, e);
        }

        public void SetZoomLevel(int zoomLevel)
        {
            if (zoomLevel < 25 || zoomLevel > 500)
                throw new ArgumentOutOfRangeException(nameof(zoomLevel));

            webBrowser.Document?.ExecCommand("zoom", false, zoomLevel.ToString());
            settingsManager.SaveZoomLevel(zoomLevel);
        }

        public async Task PrintPage()
        {
            try
            {
                using var printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    await webBrowser.PrintAsync(printDialog.PrinterSettings);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Print failed", ex);
                MessageBox.Show("Unable to print page. Please try again.");
            }
        }
    }
}
