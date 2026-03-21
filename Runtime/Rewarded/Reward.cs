namespace SametGuzelDev.HMS.Ads
{
    /// <summary>
    /// Immutable data class representing the reward earned by the user after
    /// completing a rewarded ad.
    /// </summary>
    public sealed class Reward
    {
        /// <summary>
        /// The type or name of the reward as configured in AppGallery Connect
        /// (e.g., <c>"coins"</c>, <c>"gems"</c>).
        /// </summary>
        public string Name { get; }

        /// <summary>The quantity of the reward.</summary>
        public int Amount { get; }

        /// <summary>Creates a new <see cref="Reward"/> with the given name and amount.</summary>
        public Reward(string name, int amount)
        {
            Name   = name;
            Amount = amount;
        }

        /// <inheritdoc/>
        public override string ToString() => $"Reward(Name={Name}, Amount={Amount})";
    }
}
