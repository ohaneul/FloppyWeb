// Because spinners are boring!
class FunLoadingAnimation {
    private readonly loadingMessages = [
        "🚀 Zooming through the internet...",
        "🔍 Looking for awesome content...",
        "🌈 Sprinkling some magic dust...",
        "🐢 Almost there (but taking it slow)...",
        "🎮 Loading... Want to play Pong while you wait?"
    ];

    // My masterpiece - a loading duck that waddles across the screen
    private createWaddlingDuck(): HTMLElement {
        const duck = document.createElement('div');
        duck.innerHTML = "🦆";
        duck.className = "waddle-animation";
        // The CSS animation that took me 2 hours to perfect
        duck.style.animation = "waddle 2s infinite";
        return duck;
    }

    public showLoading(): void {
        const container = document.createElement('div');
        container.appendChild(this.createWaddlingDuck());
        container.appendChild(this.getRandomMessage());
        document.body.appendChild(container);
    }

    // Sometimes you just need a random message to brighten someone's day
    private getRandomMessage(): HTMLElement {
        const msg = document.createElement('p');
        msg.textContent = this.loadingMessages[
            Math.floor(Math.random() * this.loadingMessages.length)
        ];
        return msg;
    }
} 