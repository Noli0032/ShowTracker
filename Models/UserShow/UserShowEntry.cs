namespace MyProject.Models.UserShow;

using Microsoft.EntityFrameworkCore;
using MyProject.Areas.Identity.Data;

[PrimaryKey(nameof(UserId), nameof(TvMazeShowId))]
public class UserShowEntry
{
    public required string UserId {get; set;}
    public int TvMazeShowId {get; set;}
    public ShowStatus Status {get; set;}
    public int? PersonalRating {get; set;}
    public string? Notes {get; set;}
    public DateOnly? DateWatched {get; set;}
    public DateOnly? DateAdded {get; set;}
    public ApplicationUser? User {get; set;}
}