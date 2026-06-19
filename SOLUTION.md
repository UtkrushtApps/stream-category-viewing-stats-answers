# Solution Steps

1. Open `src/StreamStats.Core/Services/ViewingStatsService.cs` and keep the existing null guard so `null` input still throws `ArgumentNullException`.

2. Before grouping, filter the input sequence with `Where(e => e.SecondsWatched > 0)` so aborted, paused, zero-second, and negative watch events do not affect totals or percentages.

3. Update the display-category resolver so both raw categories `Kids` and `Family` return the display category `Family`; all other category names should be returned unchanged.

4. Group the filtered events by the resolved display category and calculate each group’s total watch seconds and integer total minutes.

5. After grouping, check whether there are any grouped rows. If none exist, return an empty read-only list instead of calculating percentages and dividing by zero.

6. Calculate the grand total using only valid watch time, then compute each category’s percentage as `categoryTotal * 100.0 / grandTotal` so the calculation uses floating-point division rather than integer division.

7. Round each `PercentShare` to one decimal place with `Math.Round(..., 1, MidpointRounding.AwayFromZero)`.

8. Sort the projected `CategoryShare` rows with `OrderByDescending(g => g.TotalMinutes)` before returning the list.

9. Run `dotnet test` from the solution root to verify the viewing breakdown behavior.

