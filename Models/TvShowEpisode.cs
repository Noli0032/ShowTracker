namespace MyProject.Models;

public record TvShowEpisode
{
    public int Id {get; init;}
    public required string Name {get; init;}
    public required int Season {get; init;}
    public required int Number {get; init;}
    public DateOnly? Airdate {get; init;}
    public int? Runtime {get; init;}
    public TvShowRating? Rating {get; init;}
    public TvShowImage? Image {get; init;}
    public required string Summary {get; set;}
}