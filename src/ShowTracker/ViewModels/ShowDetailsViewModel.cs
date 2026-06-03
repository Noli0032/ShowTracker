using ShowTracker.Models;
using ShowTracker.Models.UserShow;

namespace ShowTracker.ViewModels;

public class ShowDetailsViewModel
{
    public required TvShow Show {get; init;}
    public ShowStatus? ShowStatus {get; init;}
}