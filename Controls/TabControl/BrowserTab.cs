public class BrowserTab : TabPage
{
    private readonly BrowserControl browserControl;
    public event EventHandler<string> TitleChanged;

    public BrowserTab()
    {
        browserControl = new BrowserControl();
        browserControl.Dock = DockStyle.Fill;
        this.Controls.Add(browserControl);
        
        // Set default tab properties
        this.Text = "New Tab";
    }

    public void Navigate(string url)
    {
        browserControl.Navigate(url);
    }
} 