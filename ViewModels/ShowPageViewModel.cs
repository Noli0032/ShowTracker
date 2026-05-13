namespace MyProject.ViewModels;
using MyProject.Models;

public class ShowPageViewModel
{
    public required TvShow[] TvShows {get; init;}
    public required int Page {get; set;}
}