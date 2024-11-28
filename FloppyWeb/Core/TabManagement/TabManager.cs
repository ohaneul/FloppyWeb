using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloppyWeb.Core.TabManagement
{
    public class TabManager : IDisposable
    {
        private readonly Dictionary<string, BrowserTab> _tabs;
        private readonly ProcessIsolator _processIsolator;
        private readonly ResourceManager _resourceManager;
        private BrowserTab _activeTab;

        public event EventHandler<TabEventArgs> TabCreated;
        public event EventHandler<TabEventArgs> TabClosed;
        public event EventHandler<TabEventArgs> TabActivated;

        public TabManager()
        {
            _tabs = new Dictionary<string, BrowserTab>();
            _processIsolator = new ProcessIsolator();
            _resourceManager = new ResourceManager();
        }

        public async Task<BrowserTab> CreateTab(string url = null)
        {
            var tab = new BrowserTab(_processIsolator.CreateIsolatedProcess());
            _tabs[tab.Id] = tab;

            if (url != null)
            {
                await tab.Navigate(url);
            }

            TabCreated?.Invoke(this, new TabEventArgs(tab));
            return tab;
        }

        public void CloseTab(string tabId)
        {
            if (_tabs.TryGetValue(tabId, out var tab))
            {
                tab.Dispose();
                _tabs.Remove(tabId);
                _processIsolator.TerminateProcess(tab.ProcessId);
                TabClosed?.Invoke(this, new TabEventArgs(tab));
            }
        }

        public void Dispose()
        {
            foreach (var tab in _tabs.Values)
            {
                tab.Dispose();
            }
            _processIsolator.Dispose();
            _resourceManager.Dispose();
        }
    }
} 