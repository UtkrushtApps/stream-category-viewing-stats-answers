namespace StreamStats.Core.Models;

/// <summary>
/// Represents one category row in a subscriber's monthly viewing breakdown.
/// </summary>
public sealed class CategoryShare
{
    /// <summary>
    /// The display category name (after any grouping rules are applied).
    /// </summary>
    public string Category { get; init; } = string.Empty;

    /// <summary>
    /// Total minutes watched in this display category.
    /// </summary>
    public int TotalMinutes { get; init; }

    /// <summary>
    /// This category's share of total valid watch time, as a percentage
    /// rounded to one decimal place.
    /// </summary>
    public double PercentShare { get; init; }
}
