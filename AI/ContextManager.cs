using System;
using System.Collections.Generic;
using System.Linq;
using FloppyWeb.AI.Models;

public class ContextManager
{
    private Queue<WebPageContext> pageHistory;
    private Dictionary<string, object> userPreferences;

    public ContextManager()
    {
        pageHistory = new Queue<WebPageContext>();
        userPreferences = new Dictionary<string, object>();
    }

    public void AddWebPage(WebPageContext page)
    {
        pageHistory.Enqueue(page);
        if (pageHistory.Count > 10)
        {
            pageHistory.Dequeue();
        }
    }

    public BrowsingContext GetCurrentContext()
    {
        return new BrowsingContext
        {
            RecentPages = pageHistory.ToList(),
            UserPreferences = userPreferences,
            CurrentTime = DateTime.Now
        };
    }
} 