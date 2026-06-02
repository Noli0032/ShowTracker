namespace ShowTracker.ViewModels;
using ShowTracker.Models;

public class ShowPageViewModel
{
    public required TvShow[] TvShows {get; init;}
    public required int Page {get; set;}
}