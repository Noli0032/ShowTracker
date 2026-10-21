using ShowTracker.Models;

namespace ShowTracker.ViewModels;

public class HomePageViewModel
{
    public required TvShow[] MostPopular { get; init; }
    public required TvShow[] HighestRated { get; init; }
    public required TvShow[] NewAndRunning { get; init; }
}
