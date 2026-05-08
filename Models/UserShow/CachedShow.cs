using System.ComponentModel.DataAnnotations;

namespace MyProject.Models.UserShow;

public class CachedShow
{
    [Key]
    public int TvMazeShowId {get; init;}
    public required string Name {get; init;}
    public string? ImageUrl {get; set;}
    public DateTime LastSyncedAt {get; set;}
}