using StreamStats.Core.Models;

namespace StreamStats.Core.Services;

/// <summary>
/// Builds monthly viewing breakdowns from in-memory watch events.
/// </summary>
public sealed class ViewingStatsService
{
    /// <summary>
    /// Produces a per-category breakdown of watch time for the supplied events.
    /// </summary>
    public IReadOnlyList<CategoryShare> GetMonthlyCategoryBreakdown(IEnumerable<WatchEvent> events)
    {
        if (events is null)
        {
            throw new ArgumentNullException(nameof(events));
        }

        var grouped = events
            .Where(e => e.SecondsWatched > 0)
            .GroupBy(e => ResolveDisplayCategory(e.Category))
            .Select(g => new
            {
                Category = g.Key,
                TotalSeconds = g.Sum(e => e.SecondsWatched),
                TotalMinutes = g.Sum(e => e.SecondsWatched) / 60
            })
            .ToList();

        if (grouped.Count == 0)
        {
            return Array.Empty<CategoryShare>();
        }

        int totalSeconds = grouped.Sum(g => g.TotalSeconds);

        var result = grouped
            .OrderByDescending(g => g.TotalMinutes)
            .Select(g => new CategoryShare
            {
                Category = g.Category,
                TotalMinutes = g.TotalMinutes,
                PercentShare = Math.Round(g.TotalSeconds * 100.0 / totalSeconds, 1, MidpointRounding.AwayFromZero)
            })
            .ToList();

        return result;
    }

    private static string ResolveDisplayCategory(string category)
    {
        return category switch
        {
            "Kids" => "Family",
            "Family" => "Family",
            _ => category
        };
    }
}
