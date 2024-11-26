using System;
using System.Windows.Forms;
using FloppyWeb.Controls;
using FloppyWeb.Services;
using FloppyWeb.Network;

namespace FloppyWeb
{
    public partial class MainWindow : Form
    {
        private readonly NavigationBar navigationBar;
        private readonly BrowserControl browserControl;
        private readonly BrowserHistoryManager historyManager;
        private readonly TabGroupManager tabGroupManager;
        private readonly PrivacyManager privacyManager;
        private readonly ResourceOptimizer resourceOptimizer;
        private readonly SettingsManager settingsManager;

        public MainWindow()
        {
            InitializeComponent();
            
            // Initialize managers
            historyManager = new BrowserHistoryManager();
            tabGroupManager = new TabGroupManager();
            privacyManager = new PrivacyManager();
            resourceOptimizer = new ResourceOptimizer();
            settingsManager = new SettingsManager();

            // Initialize custom controls
            navigationBar = new NavigationBar();
            browserControl = new BrowserControl();

            SetupWindow();
            SetupEventHandlers();
            InitializeNetworking();
            InitializeManagers();
            LoadUserPreferences();
            
            // Start memory optimization timer
            StartMemoryOptimizationTimer();
        }

        private void SetupWindow()
        {
            this.Text = "FloppyWeb Browser";
            this.Size = new System.Drawing.Size(1024, 768);
            this.MinimumSize = new System.Drawing.Size(800, 600);

            // Setup layout
            navigationBar.Dock = DockStyle.Top;
            browserControl.Dock = DockStyle.Fill;

            this.Controls.Add(navigationBar);
            this.Controls.Add(browserControl);
        }

        private void SetupEventHandlers()
        {
            navigationBar.NavigationRequested += OnNavigationRequested;
            navigationBar.BackRequested += OnBackRequested;
            navigationBar.ForwardRequested += OnForwardRequested;
            browserControl.NavigationCompleted += OnNavigationCompleted;
        }

        private void OnNavigationRequested(object sender, string url)
        {
            browserControl.Navigate(url);
        }

        private void OnBackRequested(object sender, EventArgs e)
        {
            browserControl.GoBack();
        }

        private void OnForwardRequested(object sender, EventArgs e)
        {
            browserControl.GoForward();
        }

        private void OnNavigationCompleted(object sender, string url)
        {
            navigationBar.UpdateUrl(url);
            historyManager.AddToHistory(url);
        }

        private void InitializeNetworking()
        {
            NetworkManager.Instance.NetworkStatusChanged += OnNetworkStatusChanged;
            NetworkManager.Instance.NetworkSpeedUpdated += OnNetworkSpeedUpdated;
        }

        private void OnNetworkStatusChanged(object sender, NetworkStatusChangedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                navigationBar.UpdateNetworkStatus(e.IsAvailable);
                if (!e.IsAvailable)
                {
                    MessageBox.Show("Network connection lost!", "Network Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            });
        }

        private void OnNetworkSpeedUpdated(object sender, NetworkSpeedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                navigationBar.UpdateNetworkSpeed(e.Speed);
            });
        }

        private void InitializeManagers()
        {
            tabGroupManager = new TabGroupManager();
            privacyManager = new PrivacyManager();
            resourceOptimizer = new ResourceOptimizer();
            settingsManager = new SettingsManager();
        }

        // Fixed the memory leak in tab handling
        private void OnTabClosed(object sender, TabEventArgs e)
        {
            var tab = e.Tab;
            tab.Dispose(); // Properly dispose tab resources
            resourceOptimizer.CleanupTabResources(tab.Id);
            GC.Collect(); // Force garbage collection for large tabs
        }

        // Added missing error handling
        protected override void OnError(Exception ex)
        {
            Logger.LogError(ex);
            if (!HandleKnownError(ex))
            {
                ShowUserFriendlyError(ex);
            }
        }

        // Added after crash reports
        private void StartMemoryOptimizationTimer()
        {
            var timer = new System.Windows.Forms.Timer
            {
                Interval = 5 * 60 * 1000 // 5 minutes
            };
            timer.Tick += (s, e) => resourceOptimizer.OptimizeMemoryUsage();
            timer.Start();
        }
    }
} 