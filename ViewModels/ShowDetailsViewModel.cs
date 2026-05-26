using MyProject.Models;
using MyProject.Models.UserShow;

namespace MyProject.ViewModels;

public class ShowDetailsViewModel
{
    public required TvShow Show {get; init;}
    public ShowStatus? ShowStatus {get; init;}
}