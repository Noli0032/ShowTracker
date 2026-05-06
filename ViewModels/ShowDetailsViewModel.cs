using MyProject.Models;

namespace MyProject.ViewModels;

public class ShowDetailsViewModel
{
    public required TvShow Show {get; init;}
    public bool IsInWatchList {get; init;}
}