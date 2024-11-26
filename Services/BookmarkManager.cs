using System;
using System.Collections.Generic;
using System.IO;

public class BookmarkManager
{
    private List<Bookmark> bookmarks;
    private string bookmarkFilePath;

    public BookmarkManager()
    {
        bookmarkFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "FloppyWeb",
            "bookmarks.json"
        );
        LoadBookmarks();
    }

    public void AddBookmark(string title, string url)
    {
        bookmarks.Add(new Bookmark { Title = title, Url = url });
        SaveBookmarks();
    }
} 