namespace StreamStats.Core.Models;

/// <summary>
/// Represents a single content play recorded for a subscriber.
/// </summary>
public sealed class WatchEvent
{
    /// <summary>
    /// The raw content category for the played title (for example, "Drama", "Kids").
    /// </summary>
    public string Category { get; init; } = string.Empty;

    /// <summary>
    /// Number of seconds actually watched for this event.
    /// Non-positive values represent aborted or paused plays.
    /// </summary>
    public int SecondsWatched { get; init; }
}
