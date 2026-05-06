namespace MyProject.Models.UserShow;

using MyProject.Areas.Identity.Data;

public class UserShowEntry
{
    public int Id {get; set;}
    public required string UserId {get; set;}
    public int TvMazeShowId {get; set;}
    public ShowStatus Status {get; set;}
    public int? PersonalRating {get; set;}
    public string? Notes {get; set;}
    public DateOnly? DateWatched {get; set;}
    public DateOnly? DateAdded {get; set;}
    public required ApplicationUser User {get; set;}
}