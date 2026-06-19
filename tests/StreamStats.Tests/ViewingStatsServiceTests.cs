using StreamStats.Core.Models;
using StreamStats.Core.Services;
using Xunit;

namespace StreamStats.Tests;

public sealed class ViewingStatsServiceTests
{
    private readonly ViewingStatsService _service = new();

    [Fact]
    public void Breakdown_ComputesFractionalPercentages()
    {
        var events = new[]
        {
            new WatchEvent { Category = "Drama", SecondsWatched = 60 * 40 },
            new WatchEvent { Category = "Comedy", SecondsWatched = 60 * 60 }
        };

        var result = _service.GetMonthlyCategoryBreakdown(events);

        var drama = result.Single(r => r.Category == "Drama");
        var comedy = result.Single(r => r.Category == "Comedy");

        Assert.Equal(40.0, drama.PercentShare, 1);
        Assert.Equal(60.0, comedy.PercentShare, 1);
    }

    [Fact]
    public void Breakdown_RoundsPercentToOneDecimalPlace()
    {
        var events = new[]
        {
            new WatchEvent { Category = "Drama", SecondsWatched = 60 * 10 },
            new WatchEvent { Category = "Comedy", SecondsWatched = 60 * 20 }
        };

        var result = _service.GetMonthlyCategoryBreakdown(events);

        var drama = result.Single(r => r.Category == "Drama");
        Assert.Equal(33.3, drama.PercentShare, 1);
    }

    [Fact]
    public void Breakdown_IgnoresNonPositiveWatchEvents()
    {
        var events = new[]
        {
            new WatchEvent { Category = "Drama", SecondsWatched = 60 * 30 },
            new WatchEvent { Category = "Drama", SecondsWatched = 0 },
            new WatchEvent { Category = "Comedy", SecondsWatched = -120 },
            new WatchEvent { Category = "Comedy", SecondsWatched = 60 * 10 }
        };

        var result = _service.GetMonthlyCategoryBreakdown(events);

        var drama = result.Single(r => r.Category == "Drama");
        var comedy = result.Single(r => r.Category == "Comedy");

        Assert.Equal(30, drama.TotalMinutes);
        Assert.Equal(10, comedy.TotalMinutes);
        Assert.Equal(75.0, drama.PercentShare, 1);
        Assert.Equal(25.0, comedy.PercentShare, 1);
    }

    [Fact]
    public void Breakdown_GroupsKidsUnderFamily()
    {
        var events = new[]
        {
            new WatchEvent { Category = "Kids", SecondsWatched = 60 * 20 },
            new WatchEvent { Category = "Family", SecondsWatched = 60 * 30 },
            new WatchEvent { Category = "Drama", SecondsWatched = 60 * 50 }
        };

        var result = _service.GetMonthlyCategoryBreakdown(events);

        Assert.DoesNotContain(result, r => r.Category == "Kids");
        var family = result.Single(r => r.Category == "Family");
        Assert.Equal(50, family.TotalMinutes);
        Assert.Equal(50.0, family.PercentShare, 1);
    }

    [Fact]
    public void Breakdown_OrdersByTotalMinutesDescending()
    {
        var events = new[]
        {
            new WatchEvent { Category = "Drama", SecondsWatched = 60 * 10 },
            new WatchEvent { Category = "Comedy", SecondsWatched = 60 * 50 },
            new WatchEvent { Category = "Documentary", SecondsWatched = 60 * 30 }
        };

        var result = _service.GetMonthlyCategoryBreakdown(events);

        Assert.Equal(new[] { "Comedy", "Documentary", "Drama" }, result.Select(r => r.Category).ToArray());
    }

    [Fact]
    public void Breakdown_ReturnsEmptyWhenNoValidEvents()
    {
        var events = new[]
        {
            new WatchEvent { Category = "Drama", SecondsWatched = 0 },
            new WatchEvent { Category = "Comedy", SecondsWatched = -5 }
        };

        var result = _service.GetMonthlyCategoryBreakdown(events);

        Assert.Empty(result);
    }
}
