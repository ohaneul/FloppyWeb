public class PersonalizedSettings
{
    // Because everyone deserves a browser that matches their mood
    public enum BrowserMood
    {
        Professional = 0,  // For those Monday meetings
        Playful = 1,      // For Friday vibes
        Caffinated = 2,   // For the all-nighters
        Zen = 3           // For when you need some calm
    }

    public BrowserMood CurrentMood { get; set; } = BrowserMood.Professional;

    // Features that made it from my wishlist
    public bool EnableDuckMode { get; set; } = false;  // Turns all icons into ducks
    public bool EnableDadJokes { get; set; } = false;  // Adds dad jokes to error pages
    public bool EnableRainbowTabs { get; set; } = true;  // Because why not?
    
    // That one feature Dave requested
    public bool EnableCoffeeReminder { get; set; } = true;  // Reminds you to drink coffee

    public void ApplyMood()
    {
        switch (CurrentMood)
        {
            case BrowserMood.Playful:
                EnableDuckMode = true;
                EnableDadJokes = true;
                break;
            case BrowserMood.Zen:
                // Calm colors, no notifications
                EnableDuckMode = false;
                EnableCoffeeReminder = false;
                break;
            // More moods to come when I'm feeling creative...
        }
    }
} 