using System;
using System.Windows.Forms;
using FloppyWeb.Theme;

namespace FloppyWeb.Controls
{
    public class NavigationBar : UserControl
    {
        private Button backButton;
        private Button forwardButton;
        private TextBox urlTextBox;
        private Button goButton;
        private Button refreshButton;

        public event EventHandler<string> NavigationRequested;
        public event EventHandler BackRequested;
        public event EventHandler ForwardRequested;

        public NavigationBar()
        {
            InitializeControls();
            SetupLayout();
            SetupEventHandlers();
        }

        private void InitializeControls()
        {
            backButton = new Button
            {
                Text = "←",
                Width = 40,
                FlatStyle = FlatStyle.Flat
            };

            forwardButton = new Button
            {
                Text = "→",
                Width = 40,
                FlatStyle = FlatStyle.Flat
            };

            urlTextBox = new TextBox
            {
                Width = 500
            };

            goButton = new Button
            {
                Text = "Go",
                Width = 50,
                FlatStyle = FlatStyle.Flat
            };

            refreshButton = new Button
            {
                Text = "↻",
                Width = 40,
                FlatStyle = FlatStyle.Flat
            };

            this.Height = 40;
            this.BackColor = ThemeColors.NavigationBarBackground;
        }

        public void UpdateUrl(string url)
        {
            urlTextBox.Text = url;
        }

        // ... Additional implementation details
    }
} 