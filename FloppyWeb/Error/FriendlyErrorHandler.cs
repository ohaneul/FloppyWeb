public class FriendlyErrorHandler
{
    private readonly Dictionary<string, string> friendlyMessages = new()
    {
        { "404", "Oops! Looks like this page is playing hide and seek... and winning! 🙈" },
        { "500", "The server is having a coffee break ☕ Try again in a minute!" },
        { "NoInternet", "Hmm... seems like the internet took a nap. Let's check your connection! 🔌" }
    };

    // That one time we had a weird error and I had to add this...
    private readonly Random random = new();
    private readonly string[] encouragingMessages = new[]
    {
        "Don't worry, we've got this! 💪",
        "Take a deep breath, we'll figure it out! 🌟",
        "Even browsers have bad days... 🤷‍♂️"
    };

    public string GetFriendlyMessage(string errorCode)
    {
        // Sometimes you just need a little encouragement
        if (!friendlyMessages.ContainsKey(errorCode))
        {
            return $"{encouragingMessages[random.Next(encouragingMessages.Length)]} \n" +
                   $"Error: {errorCode}";
        }
        return friendlyMessages[errorCode];
    }
} 