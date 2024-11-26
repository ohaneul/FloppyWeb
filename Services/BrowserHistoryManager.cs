using System;
using System.Collections.Generic;

namespace FloppyWeb.Services
{
    public class BrowserHistoryManager
    {
        private readonly List<string> history;
        private readonly int maxHistoryItems = 100;

        public BrowserHistoryManager()
        {
            history = new List<string>();
        }

        public void AddToHistory(string url)
        {
            history.Insert(0, url);
            if (history.Count > maxHistoryItems)
            {
                history.RemoveAt(history.Count - 1);
            }
        }

        public List<string> GetHistory()
        {
            return new List<string>(history);
        }

        public void ClearHistory()
        {
            history.Clear();
        }
    }
} 